using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class AdvancedMTF : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 22;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "<color=#3bffe2>Advanced Ops</color>";
        public override string Description { get; set; } = "The SpecialOps who have an advanced technology weapons";
        public override string CustomInfo { get; set; } = "Advanced MTF";
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 80;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public override bool DisplayCustomItemMessages { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.ArmorCombat.ToString(),
            4.ToString(),
            46.ToString(),
            ItemType.SCP500.ToString(),
            9.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 120 },
        };
    }
}