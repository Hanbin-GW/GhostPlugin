using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Structs;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;

namespace GhostPlugin.Custom.Items.Armor
{
    [CustomItem(ItemType.ArmorHeavy)]
    public class JuggernautArmor : CustomArmor {
        public override ItemType Type { get; set; } = ItemType.ArmorHeavy;
        public override uint Id { get; set; } = 70;
        public override string Name { get; set; } = "Juggernaut Armor";
        public override string Description { get; set; } = "Although, it decrease your mobility, It reduces damage";
        public override float Weight { get; set; } = 30f;
        public override SpawnProperties SpawnProperties { get; set; }
        private readonly Dictionary<Player, CoroutineHandle> _drainRoutines = new();
        private const float ExtraDrainPerSecond = 0.5f;
        private const float Tick = 0.2f;
        
        //private void OnDied(Exiled.Events.EventArgs.Player.DiedEventArgs ev) => StopDrain(ev.Player);
        //private void OnLeft(Exiled.Events.EventArgs.Player.LeftEventArgs ev) => StopDrain(ev.Player);
        //private void OnChangingRole(Exiled.Events.EventArgs.Player.ChangingRoleEventArgs ev) => StopDrain(ev.Player);
        
        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            base.OnAcquired(player, item, displayMessage);
        }

        private void OnChangingMoveState(ChangingMoveStateEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor) && ev.NewState == PlayerMovementState.Sprinting)
            {
                ev.Player.EnableEffect<Slowness>(intensity: 60);
                // StartDrain(ev.Player);
            }
            else
            {
                ev.Player.DisableEffect<Slowness>();
                // StopDrain(ev.Player);
            }
        }

        /*private void StartDrain(Player player)
        {
            if (_drainRoutines.ContainsKey(player))
                return;

            var handle = Timing.RunCoroutine(DrainRoutine(player));
            _drainRoutines[player] = handle;
        }

        private IEnumerator<float> DrainRoutine(Player player)
        {
            var stamina = player.GetModule<StaminaStat>(); // PlayerStatsSystem

            while (player.IsAlive && Check(player.CurrentArmor))
            {
                // 달리는 동안에만 추가 드레인
                if (player.IsUsingStamina)
                {
                    // Tick마다 깎을 양 = (초당깎는양 / 1초) * Tick
                    float delta = (ExtraDrainPerSecond * Tick);
                    stamina.ModifyAmount(-delta);
                }

                yield return Timing.WaitForSeconds(Tick);
            }

            StopDrain(player);
        }

        private void StopDrain(Player player)
        {
            if (_drainRoutines.TryGetValue(player, out var handle))
            {
                Timing.KillCoroutines(handle);
                _drainRoutines.Remove(player);
            }
        }*/
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player.CurrentArmor))
            {
                if (ev.Player.ArtificialHealth != 0)
                {
                    ev.Player.ArtificialHealth -= ev.Amount;
                    ev.Amount = 0;
                }
                else
                    ev.Amount *= 0.15f;
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingMoveState += OnChangingMoveState;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            /*Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;*/
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.ChangingMoveState -= OnChangingMoveState;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            /*Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;*/
            base.UnsubscribeEvents();
        }
    }
}