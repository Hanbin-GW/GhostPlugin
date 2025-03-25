using System.Linq;
using CustomPlayerEffects;
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
        public override string Name { get; set; } = "<color=#6600CC>기절 수류탄</color>";
        public override string Description { get; set; } = "맞을시 스턴효과가 일어납니다";
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
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