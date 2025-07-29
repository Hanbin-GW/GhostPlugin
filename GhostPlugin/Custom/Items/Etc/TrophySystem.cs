using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.Methods.MER;
using InventorySystem.Items.ThrowableProjectiles;
using MEC;
using ProjectMER.Features.Objects;
using UnityEngine;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Lantern)]
    public class TrophySystem : CustomItem
    {
        public override uint Id { get; set; } = 19;
        public override string Name { get; set; } = "Trophy System";
        public override string Description { get; set; } = "근처에 있는 활성화된 수류탄을 요격합니다!";
        public override float Weight { get; set; } = 3f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override ItemType Type { get; set; } = ItemType.Lantern;
        public SchematicObject obj = null;
        private CoroutineHandle grenadeScanCoroutine;
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem += OnDrop;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppingItem -= OnDrop;
            base.UnsubscribeEvents();
        }

        private void OnDrop(DroppingItemEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (ev.IsThrown)
                {
                    ev.Item.Destroy();
                    obj = ObjectManager.SpawnObject("TrophySystem", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, ev.Player.Transform.localRotation);
                    grenadeScanCoroutine = Timing.RunCoroutine(ScanAndDestroyGrenades(obj));
                    Timing.CallDelayed(60, () =>
                    {
                        ObjectManager.RemoveObject(obj);
                        Timing.KillCoroutines(grenadeScanCoroutine);
                    });
                }
            }
        }
        
        private IEnumerator<float> ScanAndDestroyGrenades(SchematicObject center)
        {
            while (true)
            {
                float radius = 5f;

                // 1. ExplosiveGrenade
                foreach (var grenade in Object.FindObjectsOfType<ExplosionGrenade>())
                {
                    if (Vector3.Distance(grenade.transform.position, center.transform.position) <= radius)
                    {
                        grenade.DestroySelf(); // 안전하게 폭발시켜 제거
                    }
                }

                // 2. FlashbangGrenade
                foreach (var flash in Object.FindObjectsOfType<FlashbangGrenade>())
                {
                    if (Vector3.Distance(flash.transform.position, center.transform.position) <= radius)
                    {
                        flash.DestroySelf();
                    }
                }

                // 3. Scp018Grenade
                foreach (var ball in Object.FindObjectsOfType<Scp018Projectile>())
                {
                    if (Vector3.Distance(ball.transform.position, center.transform.position) <= radius)
                    {
                        ball.DestroySelf();
                    }
                }

                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}