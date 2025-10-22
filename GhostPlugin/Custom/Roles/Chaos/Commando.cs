using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;

namespace GhostPlugin.Custom.Roles.Chaos
{
    public class Commando : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 73;
        public override int MaxHealth { get; set; } = 130;
        public override string Name { get; set; } = "Commando";
        public override string Description { get; set; } = "A heavy unit with have a anhialate weapon";
        public override string CustomInfo { get; set; }
        public StartTeam StartTeam { get; set; }
        public int Chance { get; set; }

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            71.ToString(), // HE-1
            73.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Adrenaline.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; }
    }
}