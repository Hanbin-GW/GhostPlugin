using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfSergeant)]
    public class HugoBoss : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 24;
        public override int MaxHealth { get; set; } = 150;
        public override string Name { get; set; } = "<color=#ffe53b>Hugo Boss</color>";
        public override string Description { get; set; } = "많은 적을 분쇠시키시시요!\n여러 고급무기들을 가지고있습니다.";
        public override string CustomInfo { get; set; } = "Hugo Boss";
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 55;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>();

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            25.ToString(),
            ItemType.Jailbird.ToString(),
            ItemType.Jailbird.ToString(),
            ItemType.Painkillers.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.ArmorHeavy.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 150 },
        };

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect<MovementBoost>(5);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect<MovementBoost>();
            base.RoleRemoved(player);
        }
    }
}