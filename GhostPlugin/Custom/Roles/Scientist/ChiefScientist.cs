using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    public class ChiefScientist : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 1;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#ffd900>수석과학자</color>";
        public override string Description { get; set; } = "SCP 제단의 총연구 책임자입니다!\n가지고 있는 SCP 아이탬을 사용하여 탈출하십시요!";
        public override string CustomInfo { get; set; } = "Chief Scientist";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public StartTeam StartTeam { get; set; } = StartTeam.Scientist;
        public int Chance { get; set; } = 100;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override bool KeepRoleOnChangingRole { get; set; } = true;
        public override bool KeepRoleOnDeath { get; set; } = false;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardResearchCoordinator.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.SCP268.ToString(),
            ItemType.SCP207.ToString(),
        };
    }
}