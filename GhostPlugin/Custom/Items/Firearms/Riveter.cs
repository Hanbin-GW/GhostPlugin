using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using UnityEngine;
using YamlDotNet.Serialization;
using GhostPlugin.Methods.Objects;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunE11SR)]
    public class Riveter : CustomWeapon
    {
        public override uint Id { get; set; } = 51;
        public override string Name { get; set; } = "Riveter";
        public override string Description { get; set; } = "AR 이지만 <color=#ebc934>12게이지 용의 숨결</color> 을 사용하는 연발 샷건입니다.";
        public override float Weight { get; set; } = 7.5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override byte ClipSize { get; set; } = 15;
        public override ItemType Type { get; set; } = ItemType.GunE11SR;
        [YamlIgnore] 
        public override float Damage { get; set; }
        
        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                PlasmaCube dragonbreath = new PlasmaCube();
                //SpawnParticleSpark spark = new SpawnParticleSpark();
                Color glowColor = new Color(1.0f, 0.0f, 0.0f, 0.1f) * 50f;
                //spark.SpawnREDSpark(ev.Player,ev.Firearm.Base.transform.position,1);
                dragonbreath.SpawmSparkBuckshot(ev.Player, ev.Firearm.Base.transform.position,10,25f,0.1f,glowColor); 
            }
            base.OnShot(ev);
        }
    }
}