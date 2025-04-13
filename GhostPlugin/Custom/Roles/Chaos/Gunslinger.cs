using System.Collections.Generic;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;

namespace GhostPlugin.Custom.Roles.Chaos
{
    public class Gunslinger : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 15;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "Gunslinger";
        public override string Description { get; set; } = "오버킬 조끼를 갖고있는 요원입니다.";
        public override string CustomInfo { get; set; } = "Gunslinger";
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public int Chance { get; set; } = 90;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            45.ToString(),
        };
    }
}