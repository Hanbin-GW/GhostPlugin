using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using MEC;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeFlash)]
    public class TripleFlashGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 12;
        public override string Name { get; set; } = "<color=#6600CC>Cluster Flash Gernade</color>";
        public override string Description { get; set; } = "It's a flash that explodes three times in a row";
        public override float Weight { get; set; } = 5f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool ExplodeOnCollision { get; set; } = false;
        public override float FuseTime { get; set; } = 4f;
        public override ItemType Type { get; set; } = ItemType.GrenadeFlash;

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            Vector3 explosionPosition = ev.Projectile.Position;
            if (Check(ev.Projectile))
            {
                Timing.RunCoroutine(MyCoroutine(explosionPosition,ev.Player));
            }
        }

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
        }

        private IEnumerator<float> MyCoroutine(Vector3 position, Player player)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Log.Debug($"Explosion {i} at {position}");
                    ((FlashGrenade)Item.Create(ItemType.GrenadeFlash)).SpawnActive(position, owner:player);
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to create explosion: {ex}");
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}