using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.Methods.ParticlePrimitives;
using InventorySystem.Items.Firearms.Modules;
using MEC;
using PlayerStatsSystem;
using UnityEngine;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Warp : ActiveAbility
    {
        public override string Name { get; set; } = "Warp_Charge";
         
        public override string Description { get; set; } = "Charges towards the target location.";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 45f;

        [Description("The amount of damage inflicted when the player collides with something.")]
        public float ContactDamage { get; set; } = 30f;

        [Description("The bonus multiplier if the target player wasn't moving.")] 
        public float AccuracyMultiplier { get; set; } = 2f;

        [Description("How long the ensnare effect lasts.")]
        public float EnsnareDuration { get; set; } = 5f;

        protected override void AbilityUsed(Player player)
        {
            Color color = new Color32(255, 0, 0, 121);
            Color glowColor = new Color(color.r * 75f, color.g * 75f, color.b * 75f, color.a);
            OrbitPrimitiveMethods.StartTrail(player, segmentCount: 10, color: glowColor);

            if (RunRaycast(player, out RaycastHit hit))
            {
                Log.Debug($"{player.Nickname} -- {player.Position} - {hit.point}");
                bool line = Physics.Linecast(hit.point, hit.point + (Vector3.down * 5f), out RaycastHit lineHit);
                if (!line)
                {
                    player.ShowHint(
                        "You cannot charge straight up walls, silly.\nYour cooldown has been lowered to 5sec.");
                    Timing.CallDelayed(0.5f, () => LastUsed[player] = DateTime.Now + TimeSpan.FromSeconds(5));

                    return;
                }

                Log.Debug($"{player.Nickname} -- {lineHit.point}");
                Timing.RunCoroutine(MovePlayer(player, hit));
            }
        }
        // private bool RunRaycast(Player player, out RaycastHit validHit)
        // {
        //     Vector3 forward = player.CameraTransform.forward;
        //     Ray ray = new Ray(player.CameraTransform.position, forward);
        //
        //     foreach (var hit in Physics.RaycastAll(ray, 200f, HitscanHitregModuleBase.HitregMask))
        //     {
        //         var hub = hit.transform.root.GetComponent<ReferenceHub>();
        //         if (hub != null && Player.Get(hub) != player) // 적 플레이어 찾음
        //         {
        //             validHit = hit;
        //             return true;
        //         }
        //     }
        //
        //     // fallback: 아무 플레이어도 못 찾았지만 뭔가에 맞긴 함
        //     return Physics.Raycast(ray, out validHit, 200f, HitscanHitregModuleBase.HitregMask);
        // }
        
        private bool RunRaycast(Player player, out RaycastHit validHit)
        {
            Vector3 forward = player.CameraTransform.forward;
            Ray ray = new Ray(player.CameraTransform.position, forward);

            // 1) 적 플레이어 우선 탐색
            foreach (var hit in Physics.RaycastAll(ray, 200f, HitscanHitregModuleBase.HitregMask))
            {
                var hub = hit.transform.root.GetComponent<ReferenceHub>();
                if (hub != null && Player.Get(hub) != player)
                {
                    validHit = hit;
                    return true;
                }
            }

            // 2) 플레이어는 못 찾았지만, 뭔가(벽/바닥 등)는 맞았으면 그걸로 돌진
            if (Physics.Raycast(ray, out validHit, 200f, HitscanHitregModuleBase.HitregMask))
                return true;

            // 3) 아무것도 안 맞으면: 전방 200m 지점으로 돌진 (가짜 히트 생성)
            validHit = new RaycastHit
            {
                point = ray.origin + forward * 200f,
                distance = 200f
            };
            return true;
        }

        /*private bool RunRaycast(Player player, out RaycastHit hit)
        {
            Vector3 forward = player.CameraTransform.forward;
            //return Physics.Raycast(player.Position + forward, forward, out hit, 200f, HitscanHitregModuleBase.HitregMask);
            return Physics.Raycast(player.CameraTransform.position, forward, out hit, 200f, HitscanHitregModuleBase.HitregMask);
        }*/
        private IEnumerator<float> MovePlayer(Player player, RaycastHit hit)
        {
            while ((player.Position - hit.point).sqrMagnitude >= 2.5f)
            {
                player.Position = Vector3.MoveTowards(player.Position, hit.point, 0.5f);
                yield return Timing.WaitForSeconds(0.00025f);
            }

            // 필요하면 돌진 후 잠시 속박
            Timing.CallDelayed(0.5f, () => player.EnableEffect(EffectType.Ensnared, 0.5f));

            // ReferenceHub 가져오기: 더 확실한 방식
            var hub = hit.transform.root.GetComponent<ReferenceHub>();
            if (hub == null)
            {
                player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
                EndAbility(player);
                yield break;
            }

            Player target = Player.Get(hub);
            if (target == null || target == player)
            {
                player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
                EndAbility(player);
                yield break;
            }

            target.Hurt(new ScpDamageHandler(player.ReferenceHub, ContactDamage * AccuracyMultiplier, DeathTranslations.Zombie));
            target.EnableEffect(EffectType.Ensnared, EnsnareDuration);
        }

        protected override void AbilityEnded(Player player)
        {
            OrbitPrimitiveMethods.StopTrail(player);
            base.AbilityEnded(player);
        }
    }
}