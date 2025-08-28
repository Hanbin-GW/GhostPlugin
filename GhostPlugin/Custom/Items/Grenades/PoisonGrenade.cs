using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeHE)]
    public class PoisonGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 40;
        public override string Name { get; set; } = "<color=#95e68c>Poison Grenade</color>";
        public override string Description { get; set; } = "It's a bomb that releases toxic substances in the event of an explosion..";
        public override float Weight { get; set; } = 6f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 4,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside049Armory,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHczArmory,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside173Armory,
                    Chance = 25
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside096,
                    Chance = 25,
                }
            }
        };
        public override bool ExplodeOnCollision { get; set; } = false;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public override float FuseTime { get; set; } = 4.5f;

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            /*if (Check(ev.Projectile))
            {

            */
            foreach (var player in Player.List.Where(p => Vector3.Distance(p.Position, ev.Position) <= 10f))
            {
                player.DisableEffect<Burned>();
                player.EnableEffect<Poisoned>(duration: 30);
            }
            base.OnExploding(ev);
        }
    }
}