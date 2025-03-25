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

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                const float MaxReviveDistance = 20f;
                
                // Raycast로 시체 대상 확인
                if (Physics.Raycast(ev.Player.CameraTransform.position, ev.Player.CameraTransform.forward,
                        out RaycastHit hit, MaxReviveDistance))
                {
                    Log.Info($"Raycast hit: {hit.collider.gameObject.name} at position {hit.point}");
                    //var targetPlayer = Player.Get(hit.collider.GetComponentInParent<ReferenceHub>());
                    Player targetPlayer = Player.Get(hit.collider);
                    if (targetPlayer != null && targetPlayer.IsDead)
                    {
                        RevivePlayer(targetPlayer);
                        ev.Player.ShowHint($"You have revived {targetPlayer.Nickname}", 5);
                        return;
                    }
                }

                // Raycast로 타겟을 찾지 못했을 경우, 거리 계산
                Player closestPlayer = null;
                float closestDistance = float.MaxValue;

                foreach (var player in Player.List)
                {
                    if (player.IsDead)
                    {
                        float distance = Vector3.Distance(ev.Player.Position, player.Position);
                        Log.Info($"Distance to {player.Nickname}: {distance:F2}m");
                        if (distance < closestDistance && distance <= MaxReviveDistance)
                        {
                            closestPlayer = player;
                            closestDistance = distance;
                        }
                    }
                }

                if (closestPlayer != null)
                {
                    RevivePlayer(closestPlayer);
                    ev.Player.ShowHint(
                        $"You have revived {closestPlayer.Nickname} within range ({closestDistance:F1}m)", 5);
                }
                else
                {
                    ev.Player.ShowHint("No dead players within revive range.", 5);
                }
            }
        }

        private void RevivePlayer(Player player)
        {
            if (player.IsDead)
            {
                player.Role.Set(player.Role.Type, RoleSpawnFlags.AssignInventory);
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
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }
    }
}