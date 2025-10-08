using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Scp049;
using GhostPlugin.API;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp049)]
    public class Scp049AP : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 13;
        public override int MaxHealth { get; set; } = 2400;
        public override string Name { get; set; } = "049 Apocalipse";
        public override string Description { get; set; } = "평행세계 에서 온 세상을 멸망시킨 049 입니다.\n모든 공격이 즉사이며 적을 죽이고 나서 3초뒤에 좀비로 부활시킵니다.";
        public override string CustomInfo { get; set; } = "SCP 049";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp049;
        public override SpawnProperties SpawnProperties { get; set; }

        private void OnAttacking(AttackingEventArgs ev)
        {
            if (!Check(ev.Player))
                return;
            ev.Target.Kill("그대로 있어... 내가 너를 완벽하게 만들을거야...");
            Timing.CallDelayed(3, () => ev.Target.Role.Set(RoleTypeId.Scp0492));
            ev.Target.Broadcast(5,"<size=30><color=red>완벽하다..! 나의 창조물이여..!</color></size>");
        }
        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.Attacking += OnAttacking;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking;
            base.UnsubscribeEvents();
        }

        public StartTeam StartTeam { get; set; } = StartTeam.Scp049;
        public int Chance { get; set; } = 35;
    }
}