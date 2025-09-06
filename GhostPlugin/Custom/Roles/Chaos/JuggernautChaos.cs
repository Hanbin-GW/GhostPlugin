using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    [CustomRole(RoleTypeId.ChaosConscript)]
    public class JuggernautChaos : CustomRole, ICustomRole
    {
        public int Chance { get; set; } = 35;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;

        public override uint Id { get; set; } = 39;

        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosConscript;

        public override int MaxHealth { get; set; } = 200;

        public override string Name { get; set; } = "<color=#008f1e>Juggernaut Chaos</color>";

        public override string Description { get; set; } = "매우 해비하게 무장한 저거너트 혼돈의 반란입니다!";

        public override string CustomInfo { get; set; } = "Juggernaut Chaos";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleTypeId.ChaosConscript,
                    Chance = 100,
                },
            },
        };

        public override List<string> Inventory { get; set; } = new()
        {
            $"{ItemType.KeycardChaosInsurgency}",
            $"{ItemType.GrenadeHE}",
            $"{ItemType.ArmorHeavy}",
            30.ToString(),
            //21.ToString(),
            32.ToString(),
            50.ToString(),
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Ammo44Cal, 12 },
            { AmmoType.Nato762 ,200},
        };

        protected override void RoleAdded(Player player)
        {
            if (Check(player))
            {
                player.EnableEffect<AntiScp207>(intensity: 3);
            }
            base.RoleAdded(player);
        }
    }
}