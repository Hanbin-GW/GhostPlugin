using CustomPlayerEffects;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunFSP9)]
    public class PoisonGun : CustomWeapon
    {
        public override uint Id { get; set; } = 8;
        public override string Name { get; set; } = "<color=#80f573>BSP-9</color>";
        public override string Description { get; set; } = "독성물질이 포함된 기관단총입니다.\n공격시 적한테 독성 효과 부여";
        public override float Weight { get; set; } = 4.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        [YamlIgnore]
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 32;
        public override ItemType Type { get; set; } = ItemType.GunFSP9;

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Player != null && ev.Attacker != ev.Player)
            {
                if (Check(ev.Attacker.CurrentItem))
                {
                    ev.Player.EnableEffect<Poisoned>(duration:5);
                }
            }
        }
        
        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            base.OnReloading(ev);
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            base.OnShot(ev);
        }
    }
}