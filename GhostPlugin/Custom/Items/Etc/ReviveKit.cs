using System.Collections.Generic;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
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
        public override string Description { get; set; } = "시체에 조준하고 사용시 시체를 부활시킬수 있습니다!";
        public override ItemType Type { get; set; } = ItemType.Medkit;
        public override float Weight { get; set; } = 4f;
        public override SpawnProperties SpawnProperties { get; set; }
        private readonly Dictionary<Player, Vector3> deathPositions = new();

        private void OnDied(DiedEventArgs ev)
        {
            deathPositions[ev.Player] = ev.Player.Position;
        }

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (!Check(ev.Item)) return;

            const float MaxReviveDistance = 20f;

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

            Player closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (var p in Player.List)
            {
                if (!p.IsDead || !deathPositions.ContainsKey(p)) continue;

                float distance = Vector3.Distance(ev.Player.Position, deathPositions[p]);
                if (distance < closestDistance && distance <= MaxReviveDistance)
                {
                    closestPlayer = p;
                    closestDistance = distance;
                }
            }

            if (closestPlayer != null)
            {
                RevivePlayer(closestPlayer);
                ev.Player.ShowHint($"You have revived {closestPlayer.Nickname} within range ({closestDistance:F1}m)", 5);
            }
            else
            {
                ev.Player.ShowHint("No dead players within revive range.", 5);
            }
        }


        private void RevivePlayer(Player player)
        {
            if (player.IsDead)
            {
                player.Role.Set(player.PreviousRole, RoleSpawnFlags.AssignInventory);
                player.Health = 25;
                Log.Info($"{player.Nickname} has been revived!");
            }
            else
            {
                Log.Warn($"{player.Nickname} is not dead, cannot revive.");
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }
    }
}