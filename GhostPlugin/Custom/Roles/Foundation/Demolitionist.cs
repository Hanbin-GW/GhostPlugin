using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Foundation
{
    public class Demolitionist : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 12;
        public override int MaxHealth { get; set; } = 110;
        public override string Name { get; set; } = "<color=#0096FF>MTF Demolitionist</color>";
        public override string Description { get; set; } = "MTF 에서 폭발물 에특회된 요원입니다.";
        public override string CustomInfo { get; set; } = "Demolitionist";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 20;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.GunFSP9.ToString(),
            34.ToString(),
            34.ToString(),
            28.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato9, 120 },
        };
    }
}