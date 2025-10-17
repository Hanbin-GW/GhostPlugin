using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfSergeant)]
    public class Tracker : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 25;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Tracker";
        public override string Description { get; set; } = "Track your objective with a FTAC-recon and kill your enemy!";
        public override string CustomInfo { get; set; } = "Tracker";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 100;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.ArmorCombat.ToString(),
            3.ToString(),                           // FTAC Recon (battle Rife)
            ItemType.Radio.ToString(),
            28.ToString(),                          //Armor Plate Kit
            29.ToString(),                          //Impact Grenade
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 80 },
            { AmmoType.Nato9 ,30},
        };
    }
}