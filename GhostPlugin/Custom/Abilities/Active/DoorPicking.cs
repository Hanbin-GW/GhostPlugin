using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class DoorPicking : ActiveAbility
    {
        public override string Name { get; set; } = "Door Picking Ability";
        public override string Description { get; set; } = "짧은 시간 동안 어떤 문이든 열 수 있지만, 외부 요인에 의해 제한됩니다";
        public override float Duration { get; set; } = 15f;
        public override float Cooldown { get; set; } = 180f;
        public float TimeToDoorPickMin { get; set; } = 3f;
        public float TimeToDoorPickMax { get; set; } = 6f;
        public float TimeForDoorToBeOpen { get; set; } = 5f;
        public string BeforePickingDoorText { get; set; } = "문과 상호작용하여 문을 따기 시작하세요";
        public string PickingDoorText { get; set; } = "문열는중...";
        public Dictionary<EffectType, byte> EffectsToApply { get; set; } = new Dictionary<EffectType, byte>()
        {
            {EffectType.Ensnared, 1},
            {EffectType.Slowness, 255},
        };
        public List<Player> PlayersWithPickingDoorAbility = new List<Player>();
        
        protected override void AbilityUsed(Player player)
        {
            player.ShowHint(BeforePickingDoorText, 5f);
            PlayersWithPickingDoorAbility.Add(player);
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            base.UnsubscribeEvents();
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!PlayersWithPickingDoorAbility.Contains(ev.Player))
                return;
            
            if (ev.Door.IsOpen)
                return;

            if (ev.Player.CurrentItem != null)
                return;
            
            Log.Debug("Custom Abilities: Door Picking Ability, processing methods");
            ev.IsAllowed = false;
            int randomTime = new Random().Next((int)TimeToDoorPickMin, (int)TimeToDoorPickMax);
            ev.Player.ShowHint(PickingDoorText, randomTime);
            foreach (var effect in EffectsToApply)
            {
                ev.Player.EnableEffect(effect.Key, effect.Value, randomTime);
            }

            Timing.CallDelayed(randomTime, () =>
            {
                Log.Debug($"Custom Abilities: Opening {ev.Door.Name}");
                ev.Door.IsOpen = true;
                PlayersWithPickingDoorAbility.Remove(ev.Player);
                Timing.CallDelayed(TimeForDoorToBeOpen, () =>
                {
                    ev.Door.IsOpen = false;
                });
            });
        }
    }
}