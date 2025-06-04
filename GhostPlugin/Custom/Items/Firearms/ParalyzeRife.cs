using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.ParticleDisruptor)]
    public class ParalyzeRife : CustomWeapon
    {
        public override uint Id { get; set; } = 24;
        public override string Name { get; set; } = "<color=#4fdbe8>Paralyze Rife</color>";
        public override string Description { get; set; } = "점사공격시 스턴효과가 부여되는 15발 에너지 소총입니다";
        public override float Weight { get; set; } = 3.75f;
        public override ItemType Type { get; set; } = ItemType.ParticleDisruptor;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside173Armory,
                    Chance = 10,
                }
            },
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new ()
                {
                    Room = RoomType.EzShelter,
                    Chance = 40,
                }
            }
        };
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 15;
        
        /*protected override void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (ev.Attacker != ev.Player && ev.DamageHandler.Base is FirearmDamageHandler firearmDamageHandler &&
                    firearmDamageHandler.WeaponType == ev.Attacker.CurrentItem.Type)
                {
                    ev.Player.EnableEffect<Slowness>(intensity: 65, duration: 3);
                }
            }
        }*/

        protected override void OnShot(ShotEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.CanHurt = false;
                ev.Target.EnableEffect<Slowness>(intensity: 50, duration: 5f);
            }
        }

        protected override void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                ev.Amount = 0;
                ev.DamageHandler.Damage = 0;
            }
            base.OnHurting(ev);
        }

        protected override void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (ev.Firearm.MagazineAmmo < 10)
            {
                ev.Player.ShowHint("<color=orange>Battery is low Check your ammo</color>");
            }
            else if (ev.Firearm.MagazineAmmo < 5)
            {
                ev.Player.ShowHint("<color=red>Battery is very low\n<b>use wisely...</b></color>");
            }
            else
            {
                return;
            }
            base.OnReloading(ev);
        }
    }
}