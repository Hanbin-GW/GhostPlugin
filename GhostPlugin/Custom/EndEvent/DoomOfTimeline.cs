using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.EndEvent
{
    [CustomRole(RoleTypeId.Tutorial)]
    public class DoomOfTimeline : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 100;
        public override int MaxHealth { get; set; } = 1000;
        public override string Name { get; set; } = "Timeline Of Doom";
        public override string Description { get; set; } = "<color=red>이 시간선(서버) 을 파괴하십시요</color>\n`.omega` 를 입력하여 오매가 워해드를 작동하시요.";
        public override string CustomInfo { get; set; } = "Timeline Of Doom";
        public StartTeam StartTeam { get; set; } = StartTeam.Escape;
        public override RoleTypeId Role { get; set; } =  RoleTypeId.Tutorial;
        public int Chance { get; set; } = 15;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardO5.ToString(),
            70.ToString(),
            41.ToString(),
            666.ToString(),
            20.ToString(),
            33.ToString(),
            9.ToString(),
        };

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.Amount *= 0.05f;
            }
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                //ev.Player.Kick("Error: The EventHandler OnDied cause a Critical Error");
                ev.Attacker.ChangeAppearance(ev.Player.Role,true);
                ev.Attacker.Heal(500);
            }
        }

        protected override void RoleAdded(Player player)
        {
            player.EnableEffect<MovementBoost>(intensity: 135);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableAllEffects();
            base.RoleRemoved(player);
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Died += OnDied;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            base.UnsubscribeEvents();
        }
    }
}