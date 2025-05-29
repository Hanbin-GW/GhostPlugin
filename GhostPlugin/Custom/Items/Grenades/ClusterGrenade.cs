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
    [CustomItem(ItemType.GrenadeHE)]
    public class ClusterGrenade : CustomGrenade
    {
        public override uint Id { get; set; } = 9;
        public override string Name { get; set; } = "<color=#ff4d5b>집속탄</color>";
        public override string Description { get; set; } = "연쇄폭발하는 수류탄 입니다";
        public override float Weight { get; set; } = 8f;
        public override SpawnProperties SpawnProperties { get; set; }
        public override bool ExplodeOnCollision { get; set; } = false;
        public override float FuseTime { get; set; } = 4f;
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;

        private void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            // 폭발이 일어날 위치를 기준으로 추가 폭발 생성
            Vector3 explosionPosition = ev.Projectile.Position;
            if (Check(ev.Projectile))
            {
                //Vector3 explosionPosition = ev.Projectile.Position;
                Timing.RunCoroutine(MyCoroutine(explosionPosition,ev.Player));
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade += OnExplodingGrenade;
            base.SubscribeEvents();
        }
        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Map.ExplodingGrenade -= OnExplodingGrenade;
            base.UnsubscribeEvents();
        }

        private IEnumerator<float> MyCoroutine(Vector3 position, Player player)
        {
            for (int i = 1; i < 5; i++)
            {
                try
                {
                    Log.Debug($"Explosion {i} at {position}");
                    var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                    
                    grenade.FuseTime = 0.3f; // 거의 즉시 터지도록 설정
                    grenade.SpawnActive(position, owner: player);
                    
                    //((ExplosiveGrenade)Item.Create(ItemType.GrenadeHE)).SpawnActive(position, owner:player);
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