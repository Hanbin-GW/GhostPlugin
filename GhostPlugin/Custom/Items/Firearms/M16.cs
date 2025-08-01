using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class M16 : CustomWeapon
    {
        public override uint Id { get; set; } = 60;
        public override string Name { get; set; } = "M16";
        public override float Damage { get; set; } = 30;
        public override string Description { get; set; } = "Prototype weapon";
        public override float Weight { get; set; } = 5f;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 30;
        public byte RemainAmmo;
        
        private int fireCount = 0;
        private bool isCooldown = false;

        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;

            if (isCooldown)
            {
                ev.IsAllowed = false; // 발사 자체 막음
                ClipSize = 0;
                return;
            }

            fireCount++;

            if (fireCount >= 3)
            {
                isCooldown = true;
                fireCount = 0;
                RemainAmmo -= 3;
                // 1초 뒤에 다시 발사 가능
                Timing.CallDelayed(1f, () =>
                {
                    isCooldown = false;
                    ClipSize = RemainAmmo;
                });
            }
        }
    }
}