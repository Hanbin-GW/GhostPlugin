using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using InventorySystem.Items.Firearms.Modules;
using MEC;
using PlayerStatsSystem;
using UnityEngine;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class ChargeAbility : ActiveAbility
    {
        public override string Name { get; set; } = "Charge";
         
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

        private bool RunRaycast(Player player, out RaycastHit hit)
        {
            Vector3 forward = player.CameraTransform.forward;
            //return Physics.Raycast(player.Position + forward, forward, out hit, 200f, HitscanHitregModuleBase.HitregMask);
            return Physics.Raycast(player.CameraTransform.position, forward, out hit, 200f, HitscanHitregModuleBase.HitregMask);
        }
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
                Log.Debug("ReferenceHub를 찾을 수 없음 (충돌 대상이 유저가 아님)");
                player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
                EndAbility(player);
                yield break;
            }

            Player target = Player.Get(hub);
            if (target == null || target == player)
            {
                Log.Debug("대상이 null이거나 자기 자신임");
                player.Hurt(new UniversalDamageHandler(ContactDamage, DeathTranslations.Falldown));
                EndAbility(player);
                yield break;
            }

            // ✅ 이제 대상이 확실하므로 피해 적용
            target.Hurt(new ScpDamageHandler(player.ReferenceHub, ContactDamage * AccuracyMultiplier, DeathTranslations.Zombie));
            target.EnableEffect(EffectType.Ensnared, EnsnareDuration);
            player.ShowHitMarker(0.7f);

            EndAbility(player);
        }

    }
}