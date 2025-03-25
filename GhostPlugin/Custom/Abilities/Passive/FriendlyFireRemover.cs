using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Custom.Abilities.Passive
{
    [CustomAbility]
    public class FriendlyFireRemover : PassiveAbility
    {
        public override string Name { get; set; } = "Friendly Fire Remover";

        public override string Description { get; set; } =
            "Removes friendly fire, usually used in conjunction with other abilities";

        public float TimeBeforeRemovingAbility { get; set; } = 5f;
        
        public Dictionary<Player, RoleTypeId> HasNoFf = new Dictionary<Player, RoleTypeId>();

        public List<RoleTypeId> ScpRoleTypes = new List<RoleTypeId>
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp0492,
            RoleTypeId.Scp096,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp939,
            RoleTypeId.Scp3114,
        };

        public List<RoleTypeId> FoundationRoleTypes = new List<RoleTypeId>
        {
            RoleTypeId.Scientist,
            RoleTypeId.FacilityGuard,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist
        };

        public List<RoleTypeId> CiRoleTypes = new List<RoleTypeId>
        {
            RoleTypeId.ClassD,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman
        };

        protected override void AbilityAdded(Player player)
        {
            HasNoFf.Add(player, player.Role);
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }
        protected override void AbilityRemoved(Player player)
        {
            Timing.CallDelayed(TimeBeforeRemovingAbility, () =>
            {
                HasNoFf.Remove(player);
                Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            });
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (!HasNoFf.ContainsKey(ev.Attacker))
                return;

            if (ev.Player == null)
                return;
            
            bool containsScp = ScpRoleTypes.Any(roles => HasNoFf.ContainsValue(roles));

            bool containsFoundation = FoundationRoleTypes.Any(roles => HasNoFf.ContainsValue(roles));

            bool containsCi = CiRoleTypes.Any(roles => HasNoFf.ContainsValue(roles));

            if (containsScp && ev.Player.IsScp)
                ev.IsAllowed = false;
            
            if (containsFoundation && ev.Player.IsFoundationForces)
                ev.IsAllowed = false;
            
            if (containsCi && ev.Player.IsCHI)
                ev.IsAllowed = false;
        }
    }
}