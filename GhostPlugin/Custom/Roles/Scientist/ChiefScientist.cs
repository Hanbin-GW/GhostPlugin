using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    public class ChiefScientist : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 1;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#ffd900>Chief Scientist</color>";
        public override string Description { get; set; } = "SCP 제단의 총연구 책임자이며 대부분의 제단의 특수무기를 제작한사람\n특수아이탬이 제공되었습니다.";
        public override string CustomInfo { get; set; } = "Chief Scientist";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public StartTeam StartTeam { get; set; } = StartTeam.Scientist;
        public int Chance { get; set; } = 100;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardResearchCoordinator.ToString(),
            28.ToString(),
            24.ToString(),
            11.ToString(),
        };

        private void OnEscaped(EscapedEventArgs ev)
        {
            if (Check(ev.Player))
            {
            }
        }
    }
}