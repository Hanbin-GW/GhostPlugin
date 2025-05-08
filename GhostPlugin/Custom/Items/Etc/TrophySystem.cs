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
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override float Weight { get; set; }
        public override SpawnProperties SpawnProperties { get; set; }
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
                    grenadeScanCoroutine = Timing.RunCoroutine(ScanAndDestroyGrenades(obj));
                    obj = ObjectManager.SpawnObject("TrophySystem", ev.Player.Position + ev.Player.Transform.forward * 1 + ev.Player.Transform.up, Vector3.zero);
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
                var grenades = UnityEngine.Object.FindObjectsOfType<ThrowableItem>();

                foreach (var grenade in grenades)
                {
                    var proj = grenade?.Projectile;
                    if (proj == null) continue;
                    
                    if (Vector3.Distance(grenade.Projectile.Position, center.transform.position) <= radius)
                    {
                        Object.Destroy(proj.gameObject);
                        // 이펙트: 폭발 방어 메시지 출력
                        //Map.Broadcast(1, "<color=yellow>💥 트로피 시스템이 수류탄을 파괴했습니다!</color>");
                    }
                }

                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}