using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Items.Firearms
{
    public class M16 : CustomWeapon
    {
        public override uint Id { get; set; } = 60;
        public override string Name { get; set; } = "M16";
        public override float Damage { get; set; } = 30;
        public override string Description { get; set; } = "Prototype weapon";
        public override float Weight { get; set; } = 5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 30;
        public int Firecount = 0;
        
        protected override void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
                Firecount++;
            if (Firecount == 3)
            {
                ev.IsAllowed = false;
                Firecount = 0;
                Timing.CallDelayed(1.5f, () => ev.IsAllowed = true);
            }
            base.OnShooting(ev);
        }
    }
}