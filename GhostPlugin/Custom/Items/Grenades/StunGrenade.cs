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
    
    [CustomItem(ItemType.GrenadeFlash)]
    public class StunGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 10;
        public override string Name { get; set; } = "<color=#6600CC>Stun Grenade</color>";
        public override string Description { get; set; } = "If you get it right, you'll get a <color=yellow>Stun</color> effect";
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHczArmory,
                },
                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.Inside330,
                },
                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.Inside106Primary,
                },
                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideLczArmory,
                },
            },
        };
        public override float FuseTime { get; set; } = 3f;
        public override bool ExplodeOnCollision { get; set; } = false;

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            foreach (var player in Player.List.Where(p => Vector3.Distance(p.Position, ev.Position) <= 15f))
            {
                if (player.IsScp)
                {
                    player.DisableEffect<Flashed>();
                    player.EnableEffect<Slowness>(30, 3);
                }
                else
                {
                    player.DisableEffect<Flashed>();
                    player.Hurt(1);
                    player.EnableEffect<Slowness>(duration: 5, intensity: 60);
                }
            }
            base.OnExploding(ev);
        }
    }
}