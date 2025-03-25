using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class ActiveCamo : ActiveAbility
    {
        public override string Name { get; set; } = "Active Camo";

        public override string Description { get; set; } =
        "SCP-268처럼 작동하지만 물체와의 상호작용을 방해하지 않는 위장 효과를 활성화합니다.\n총기 사용시 효과 제거";

        public override float Duration { get; set; } = 30f;

        public override float Cooldown { get; set; } = 120f;
        public bool IsActive { get; set; }

        protected override void SubscribeEvents()
            {
                Exiled.Events.Handlers.Player.Shooting += OnShooting;
                Exiled.Events.Handlers.Player.Interacted += OnInteracted;
                Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
                Exiled.Events.Handlers.Player.InteractingElevator += OnInteractingElevator;
                Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
                base.SubscribeEvents();
            }
        protected override void AbilityUsed(Player player)
        {
            try
            {
                IsActive = true;
                Log.Debug($"{Name} enabled for {Duration}");
                player.EnableEffect(EffectType.Invisible, Duration);
            }
            catch (Exception e)
            {
                Log.Error($"{e}\n{e.StackTrace}");
            }
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Shooting -= OnShooting;
            Exiled.Events.Handlers.Player.Interacted -= OnInteracted;
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractingElevator;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            foreach (Player player in ActivePlayers.ToList())
                EndAbility(player);
            base.UnsubscribeEvents();
        }

        protected override void AbilityEnded(Player player)
        {
            IsActive = false;
            Log.Debug($"{Name} ended.");
            player.DisableEffect(EffectType.Invisible);
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (Check(ev.Player))
                EndAbility(ev.Player);
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.25f, () => AbilityUsed(ev.Player));
        }

        private void OnInteractingElevator(InteractingElevatorEventArgs ev)
        {
            if (Check(ev.Player))
                Timing.CallDelayed(0.25f, () => AbilityUsed(ev.Player));
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            Log.Debug(Check(ev.Player));
            if (Check(ev.Player))
                Timing.CallDelayed(0.25f, () => AbilityUsed(ev.Player));
        }

        private void OnInteracted(InteractedEventArgs ev)
        {
            Log.Debug(Check(ev.Player));
            if (Check(ev.Player))
                Timing.CallDelayed(0.25f, () => AbilityUsed(ev.Player));
        }
    }
}