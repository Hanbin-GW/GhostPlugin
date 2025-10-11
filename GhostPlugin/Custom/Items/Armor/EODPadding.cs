using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorHeavy)]
    public class EodPadding : CustomArmor
    {
        public override uint Id { get; set; } = 11;
        public override string Name { get; set; } = "<color=#ffb145>EODPadding</color>";
        public override string Description { get; set; } = "폭발 및 화염 대미지의 80% 를 감소시킵니다.";
        public override float Weight { get; set; } = 16f;
        public override int VestEfficacy { get; set; } = 80;
        public override int HelmetEfficacy { get; set; } = 55;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>()
            {
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.InsideHczArmory,
                    Chance = 40,
                },
                new DynamicSpawnPoint()
                {
                    Location = SpawnLocationType.Inside096,
                    Chance = 60,
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.ArmorHeavy;

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor))
            {
                ev.Player.DisableEffect<Burned>();
                if (ev.DamageHandler.Type == DamageType.Explosion)
                {
                    ev.Amount *= 0.2f;
                    //ev.IsAllowed = false;
                }
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }
    }
}