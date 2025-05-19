using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp173;
using MEC;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Painkillers)]
    public class Anti173 : CustomItem
    {
        public override uint Id { get; set; } = 43;
        public override string Name { get; set; } = "anti 173";
        public override string Description { get; set; } = "If you take SCP173 after taking this itam, it interferes with the teleportation of 173.\nThis item MAY NOT WORK 100%...";
        public override float Weight { get; set; } = 0.5f;
        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
        private readonly HashSet<Player> _protectedPlayers = new();
        private const int DurationTime = 30;
        
        private void OnLooking173(BlinkingEventArgs ev)
        {
            foreach (Player target in ev.Targets)
            {
                // 보호 상태인지 확인
                if (_protectedPlayers.Contains(target))
                {
                    ev.Player.ShowHint($"{new string('\n',10)}Anti 173 약물을 복용한 유저가 있습니다!\n순간이동을 할수 없습니다!");
                    ev.IsAllowed = false; // SCP-173 효과를 무효화
                    break; // 보호받는 플레이어가 발견되면 무효화 후 종료
                }
            }
        }
        
        private IEnumerator<float> CooldownCoroutine(Player player)
        {
            yield return Timing.WaitForSeconds(DurationTime);
            
            _protectedPlayers.Remove(player);
            player.ShowHint("Anti-173 보호 효과가 종료되었습니다.", 5);
        }

        private void UsingItem(UsingItemEventArgs ev)
        {
            if (Check(ev.Player.CurrentItem))
            {
                if (_protectedPlayers.Contains(ev.Player))
                {
                    ev.Player.ShowHint("이미 보호 효과가 활성화되어 있습니다!", 5);
                    return;
                }
                
                _protectedPlayers.Add(ev.Player);
                Timing.RunCoroutine(CooldownCoroutine(ev.Player));
                ev.Player.ShowHint("30초 동안 SCP-173으로부터 보호받습니다!", 5);
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingItem += UsingItem;
            Exiled.Events.Handlers.Scp173.Blinking += OnLooking173;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsingItem -= UsingItem;
            Exiled.Events.Handlers.Scp173.Blinking -= OnLooking173;
            base.UnsubscribeEvents();
        }
    }
}