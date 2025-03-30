using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Passive;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.FacilityGuard)]
    public class LuckyGuard : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 14;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Lucky Guard";
        public override string Description { get; set; } = "행운이 당신을 도와줍니다..!";
        public override string CustomInfo { get; set; } = "Lucky Guard";
        public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
        public bool IsLucky = true;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.GunFSP9.ToString(),
            ItemType.KeycardGuard.ToString(),
            ItemType.Flashlight.ToString(),
            ItemType.Radio.ToString(),
            ItemType.ArmorLight.ToString(),
            ItemType.GrenadeFlash.ToString(),
        };

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (ev.Player.Health <= 15 && IsLucky)
                {
                    ev.Player.RandomTeleport(typeof(Room));
                    ev.Player.ShowHint("어딘가로 이동되었습니다..!");
                    IsLucky = false;
                }
                else
                {
                    ev.Player.ShowHint("<color=red>항상 행운이 따르지는 않아...\n(이동 이미 사용)</color>");
                }
            }
        }

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new HealOnKill()
            {
                Name = "Hill On Kill",
                Description = "적을 처치시 Hp 를 회복합니다."
            }
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 60 },
        };
        
        protected override void RoleAdded(Player player)
        {
            //layer.RandomTeleport(typeof(Door));
            //player.ShowHint("어딘가로 이동되었습니다..!",5);
            base.RoleAdded(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }

        public StartTeam StartTeam { get; set; } = StartTeam.Guard;
        public int Chance { get; set; } = 100;
    }
}