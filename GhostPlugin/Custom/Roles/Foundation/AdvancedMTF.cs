using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;

namespace GhostPlugin.Custom.Roles.Foundation
{
    public class AdvancedMTF : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 22;
        public override int MaxHealth { get; set; } = 125;
        public override string Name { get; set; } = "<color=#eb6b34>Advanced Ops</color>";
        public override string Description { get; set; } = "고급 무기들을 가지고 있습니다";
        public override string CustomInfo { get; set; } = "Advanced MTF";
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        public int Chance { get; set; } = 80;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
        };

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardMTFOperative.ToString(),
            ItemType.ArmorCombat.ToString(),
            52.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.GunCrossvec.ToString(),
            9.ToString(),
            ItemType.Radio.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato556, 120 },
        };
    }
}