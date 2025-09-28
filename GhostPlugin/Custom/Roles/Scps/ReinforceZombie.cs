using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Scps
{
    public class ReinforceZombie : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 45;
        public override int MaxHealth { get; set; } = 700;
        public override string Name { get; set; } = "강화 좀비";
        public override string Description { get; set; } = "HP 랑 공격력이 높은 좀비입니다.";
        public override string CustomInfo { get; set; } = "Reinforce Zombie";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp | StartTeam.Revived;
        public int Chance { get; set; } = 60;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();

        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Attacker))
            {
                ev.Amount += 20;
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }
    }
}