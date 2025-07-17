using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfPrivate)]
    public class Sniper : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 25;
        public override int MaxHealth { get; set; } = 105;
        public override string Name { get; set; } = "Sniper";
        public override string Description { get; set; } = "you have a Sniper Railgun";
        public override string CustomInfo { get; set; } = "Sniper";
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 90;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            2.ToString(),
            14.ToString(),
            20.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Radio.ToString(),
            ItemType.Medkit.ToString(),
        };
    }
}