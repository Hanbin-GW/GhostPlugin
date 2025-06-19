using System.Collections.Generic;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Medkit)]
    public class ReviveKit : CustomItem
    {
        public override uint Id { get; set; } = 39;
        public override string Name { get; set; } = "Revive kit [Test Version]";
        public override string Description { get; set; } = "You can aim at a dead body and revive it when you use it!";
        public override ItemType Type { get; set; } = ItemType.Medkit;
        public override float Weight { get; set; } = 4f;
        public override SpawnProperties SpawnProperties { get; set; }

        private readonly Dictionary<Player, Vector3> deathPositions = new();
        private readonly Dictionary<Player, RoleTypeId> deathRoles = new();

        private void OnDying(DyingEventArgs ev)
        {
            Log.Debug($"[ReviveKit] {ev.Player.Nickname} died at {ev.Player.Position}");
            deathPositions[ev.Player] = ev.Player.Position;
            deathRoles[ev.Player] = ev.Player.Role.Type;
            //deathRoles[ev.Player] = ev.Player.PreviousRole;
        }

        // 아이템 사용 시 부활 처리
        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            const float MaxReviveDistance = 20f;

            // Raycast로 시체 조준 감지
            if (Physics.Raycast(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward, out RaycastHit hit, MaxReviveDistance))
            {
                var hub = hit.collider.GetComponentInParent<ReferenceHub>();
                if (hub != null)
                {
                    Player targetPlayer = Player.Get(hub);
                    if (targetPlayer != null && targetPlayer.IsDead)
                    {
                        RevivePlayer(targetPlayer);
                        ev.Player.ShowHint($"You have revived {targetPlayer.Nickname}", 5);
                        return;
                    }
                }
            }

            // Raycast 실패 시 주변 시체 탐색
            float closestDistance = float.MaxValue;
            Player closestPlayer = null;

            Log.Debug($"[ReviveKit] 사용자가 {ev.Player.Nickname} - 위치: {ev.Player.Position}");

            foreach (var p in Player.List)
            {
                Log.Debug($"[ReviveKit] 후보자: {p.Nickname}, 죽음 여부: {p.IsDead}, 저장됨: {deathPositions.ContainsKey(p)}");

                if (!p.IsDead || !deathPositions.ContainsKey(p)) continue;

                float distance = Vector3.Distance(ev.Player.Position, deathPositions[p]);
                Log.Debug($"[ReviveKit] {p.Nickname} 거리: {distance:F2}");

                if (distance < closestDistance && distance <= MaxReviveDistance)
                {
                    closestDistance = distance;
                    closestPlayer = p;
                }
            }

            if (closestPlayer != null)
            {
                RevivePlayer(closestPlayer);
                ev.Player.ShowHint($"You have revived {closestPlayer.Nickname} within range ({closestDistance:F1}m)", 5);
                closestPlayer.ShowHint($"You're Rivived by {ev.Player}!");
            }
            else
            {
                ev.Player.ShowHint("No dead players within revive range.", 5);
            }
        }

        private void RevivePlayer(Player player)
        {
            if (player.IsDead || player.PreviousRole.IsHuman())
            {
                RoleTypeId reviveRole = RoleTypeId.ClassD;
                if (deathRoles.TryGetValue(player, out RoleTypeId savedRole))
                    reviveRole = savedRole;

                player.Role.Set(reviveRole, RoleSpawnFlags.AssignInventory);
                player.Health = 10;

                Log.Info($"[ReviveKit] {player.Nickname} has been revived as {reviveRole}!");
            }
            else
            {
                Log.Warn($"[ReviveKit] {player.Nickname} is not dead, cannot revive.");
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }
    }
}