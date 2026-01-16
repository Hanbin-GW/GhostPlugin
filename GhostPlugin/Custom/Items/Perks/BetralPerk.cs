using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Perks
{
    [CustomItem(ItemType.Coin)]
    public class BetralPerk : CustomItem
    {
        public override uint Id { get; set; } = 68;
        public override string Name { get; set; } = "배신 퍽";
        public override string Description { get; set; } = "외부진영과 평화적으로 협력 / 본인의 팀을 배신할수 있게 할수있는 장치입니다.";
        public override float Weight { get; set; } = 1f;
        public override ItemType Type { get; set; } = ItemType.Coin;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            LockerSpawnPoints = new List<LockerSpawnPoint>()
            {
                new LockerSpawnPoint()
                {
                    Type = LockerType.Medkit,
                    Chance = 100,
                    UseChamber = true,
                    Offset = new Vector3(1,2,1),
                },
                new LockerSpawnPoint()
                {
                    Type = LockerType.Misc,
                    Chance = 100,
                    UseChamber = true,
                    Offset = new Vector3(1.5f,2f,1.5f),
                }
            }
        };
        private void OnFlippingCoin(FlippingCoinEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (ev.Player.Role == RoleTypeId.Scientist && ev.Player.LeadingTeam == LeadingTeam.FacilityForces)
                {
                    ev.Player.Role.Set(RoleTypeId.ChaosConscript, RoleSpawnFlags.UseSpawnpoint);
                }
                else if (ev.Player.Role == RoleTypeId.ClassD && ev.Player.LeadingTeam == LeadingTeam.ChaosInsurgency)
                {
                    ev.Player.Role.Set(RoleTypeId.FacilityGuard, RoleSpawnFlags.UseSpawnpoint);
                }
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin += OnFlippingCoin;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.FlippingCoin -= OnFlippingCoin;
            base.UnsubscribeEvents();
        }
    }
}