using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.MER;
using ProjectMER.Features.Objects;

namespace GhostPlugin.Custom.Items.Firearms
{
    [CustomItem(ItemType.GunCOM18)]
    public class PortableEnergyShild : CustomWeapon
    {
        public override uint Id { get; set; } = 54;
        public override string Name { get; set; } = "Portable Energy Shild";
        public override string Description { get; set; } = "The energy shild is activated when ADS.";
        public override float Weight { get; set; } = 2.3f;
        public SchematicObject obj = null;
        public override byte ClipSize { get; set; } = 0;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 4,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside096,
                    Chance = 25,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside106Primary,
                    Chance = 25
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHidChamber,
                    Chance = 25
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideSurfaceNuke,
                    Chance = 12
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside330,
                    Chance = 13,
                }
            }
        };
        
        private void OnAimDownSight(AimingDownSightEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem) && ev.AdsIn)
            {
                obj = ObjectManager.SpawnObject("Shield", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, ev.Player.Transform.rotation);
            }
            else if (Check(ev.Player.CurrentItem) && ev.AdsIn == false)
            {
                if (obj != null)
                    ObjectManager.RemoveObject(obj);
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.AimingDownSight += OnAimDownSight;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.AimingDownSight -= OnAimDownSight;
            base.UnsubscribeEvents();
        }
    }
}