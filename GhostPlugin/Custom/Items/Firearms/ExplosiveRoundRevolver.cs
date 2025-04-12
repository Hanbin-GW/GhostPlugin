using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Custom.Items.MonoBehavior;
using JetBrains.Annotations;
using YamlDotNet.Serialization;
using Player = Exiled.Events.Handlers.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunA7)]
    public class ExplosiveRoundRevolver : CustomWeapon
    {
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GunA7;
        public override uint Id { get; set; } = 21;
        public override string Name { get; set; } = "<color=#FF0000>Explosive Grenade Luncher ( M79 )</color>";
        public override string Description { get; set; } = "해당 무기는 폭탄을 생성합니다.";
        public override float Weight { get; set; } = 1f;

        [CanBeNull]
        public override SpawnProperties SpawnProperties { get; set; }

        public override float Damage { get; set; } = 0;
        public override byte ClipSize { get; set; } = 1;
        private float FuseTime { get; set; } = 1.5f;
        private float ScpGrenadeDamageMultiplier { get; set; } = .5f;

        protected override void SubscribeEvents()
        {
            Player.Shot += OnShot;
        }

        protected override void UnsubscribeEvents()
        {
            Player.Shot -= OnShot;
        }

        protected override void OnShot(ShotEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;
            Log.Debug($"VVUP Custom Items: Explosive Round Revolver, spawning grenade at {ev.Position}");
            ev.CanHurt = false;
            ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
            grenade.Base.GetComponent<StickyGrenadeBehavior>();
            grenade.FuseTime = FuseTime;
            grenade.ScpDamageMultiplier = ScpGrenadeDamageMultiplier;
            grenade.SpawnActive(ev.Position);
        }
    }
}