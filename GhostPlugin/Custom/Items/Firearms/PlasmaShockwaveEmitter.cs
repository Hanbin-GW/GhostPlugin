using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class PlasmaShockwaveEmitter : CustomWeapon
    {
        public override uint Id { get; set; } = 26;
        public override string Name { get; set; } = "Plasma Shockwave Emitter";
        public override string Description { get; set; } = "점사시 폭탄을 생성하는 충격파 플라즈마 소총입니다.";
        public override float Weight { get; set; } = 6f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override float Damage { get; set; } = 200;
        public override byte ClipSize { get; set; } = 3;
        public override ItemType Type { get; set; } = ItemType.ParticleDisruptor;
        private float FuseTime { get; set; } = 1.5f;
        
        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            ev.CanHurt = true;
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.FuseTime = FuseTime;
            grenade.MaxRadius = 10f;
            //grenade.ScpDamageMultiplier = ScpGrenadeDamageMultiplier;
            grenade.SpawnActive(ev.Position);
        }
    }
}