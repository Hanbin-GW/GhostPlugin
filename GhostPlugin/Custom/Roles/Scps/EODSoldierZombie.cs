using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp0492)]
    public class EodSoldierZombie : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 26;
        public override int MaxHealth { get; set; } = 120;
        public override string Name { get; set; } = "EOD Soldier Zombie";
        public override string Description { get; set; } = "당신은 군인이었던 좀비입니다!\nEOD 방탄복을 갖고있습니다\n80% 폭발 저항";
        public override string CustomInfo { get; set; } = "EOD Scp049-2";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;
        public int Chance { get; set; } = 40;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFPrivate.ToString(),
            ItemType.GunRevolver.ToString(),
            11.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo44Cal, 12 },
        };
    }
}