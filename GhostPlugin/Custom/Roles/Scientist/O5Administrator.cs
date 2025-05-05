using System.Collections.Generic;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scientist
{
    [CustomRole(RoleTypeId.Scientist)]
    public class O5Administrator : CustomRole, ICustomRole
    {
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override uint Id { get; set; } = 7;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#ffc2b0>O5 Administrator</color>";
        public override string Description { get; set; } = "당신은 O5 관리자입니다!";
        //public override string Description { get; set; } = "당신은 O5 관리자입니다!\n시설을 탈출하여 강화부대(Reinforcements MTF)한테 구조 신호를 요청하십시요!!!!";
        public override string CustomInfo { get; set; } = "O5 Administrator";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
        public StartTeam StartTeam { get; set; } = StartTeam.Scientist;
        public int Chance { get; set; } = 10;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardResearchCoordinator.ToString(),
            10.ToString(),
            43.ToString(),
        };

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };
        private void OnEscaped(EscapedEventArgs ev)
        {
            if (Check(ev.Player))
            {
                //Reinforcements.Plugin.Instance.IsSpawnable = true;
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaped += OnEscaped;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Escaped -= OnEscaped;
            base.UnsubscribeEvents();
        }
    }
}