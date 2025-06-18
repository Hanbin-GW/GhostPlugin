using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using GhostPlugin.API;

namespace GhostPlugin.EventHandlers
{
    public class CustomRoleHandler
    {
        private readonly Plugin Plugin;

        public CustomRoleHandler(Plugin plugin) => Plugin = plugin;

        public void OnRoundStarted()
        {
            if (!Plugin.Instance.Config.CustomRolesConfig.IsEnabled)
                return;
            List<ICustomRole>.Enumerator dClassRoles = new();
            List<ICustomRole>.Enumerator scientistRoles = new();
            List<ICustomRole>.Enumerator guardRoles = new();
            List<ICustomRole>.Enumerator scpRoles = new();

            foreach (KeyValuePair<StartTeam, List<ICustomRole>> kvp in Plugin.Roles)
            {
                Log.Debug($"VVUP Custom Roles: Setting enumerator for {kvp.Key} - {kvp.Value.Count}");
                switch (kvp.Key)
                {
                    case StartTeam.ClassD:
                        Log.Debug("Class d funny");
                        dClassRoles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scientist:
                        scientistRoles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Guard:
                        guardRoles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp:
                        scpRoles = kvp.Value.GetEnumerator();
                        break;
                }
            }

            foreach (Player player in Player.List)
            {
                Log.Debug($"VVUP Custom Roles: Trying to give {player.Nickname} a role | {player.Role.Type}");
                CustomRole? role = null;
                switch (player.Role.Type)
                {
                    case RoleTypeId.FacilityGuard:
                        role = CustomRoleMethods.GetCustomRole(ref guardRoles);
                        break;
                    case RoleTypeId.Scientist:
                        role = CustomRoleMethods.GetCustomRole(ref scientistRoles);
                        break;
                    case RoleTypeId.ClassD:
                        role = CustomRoleMethods.GetCustomRole(ref dClassRoles);
                        break;
                    case { } when player.Role.Side == Side.Scp:
                        role = CustomRoleMethods.GetCustomRole(ref scpRoles);
                        break;
                }

                role?.AddRole(player);
            }

            guardRoles.Dispose();
            scientistRoles.Dispose();
            dClassRoles.Dispose();
            scpRoles.Dispose();
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (!Plugin.Instance.Config.CustomRolesConfig.IsEnabled)
                return;
            if (ev.Players.Count == 0)
            {
                Log.Warn(
                    $"VVUP Custom Roles: {nameof(OnRespawningTeam)}: The respawn list is empty ?!? -- {ev.NextKnownTeam} / {ev.MaximumRespawnAmount}");

                foreach (Player player in Player.Get(RoleTypeId.Spectator))
                    ev.Players.Add(player);
                ev.MaximumRespawnAmount = ev.Players.Count;
            }

            List<ICustomRole>.Enumerator roles = new();
            switch (ev.NextKnownTeam)
            {
                case (Faction)SpawnableFaction.ChaosWave or (Faction)SpawnableFaction.ChaosMiniWave:
                {
                    if (Plugin.Roles.TryGetValue(StartTeam.Chaos, out List<ICustomRole> role))
                        roles = role.GetEnumerator();
                    break;
                }
                case (Faction)SpawnableFaction.NtfWave or (Faction)SpawnableFaction.NtfMiniWave:
                {
                    if (Plugin.Roles.TryGetValue(StartTeam.Ntf, out List<ICustomRole> pluginRole))
                        roles = pluginRole.GetEnumerator();
                    break;
                }
            }

            foreach (Player player in ev.Players)
            {
                CustomRole? role = CustomRoleMethods.GetCustomRole(ref roles);

                role?.AddRole(player);
            }

            roles.Dispose();
        }

        public void FinishingRecall(FinishingRecallEventArgs ev)
        {
            if (!Plugin.Instance.Config.CustomRolesConfig.IsEnabled)
                return;
            Log.Debug($"VVUP Custom Roles: {nameof(FinishingRecall)}: Selecting random zombie role.");
            if (Plugin.Roles.ContainsKey(StartTeam.Scp) && ev.Target is not null)
            {
                Log.Debug($"VVUP Custom Roles: {nameof(FinishingRecall)}: List count {Plugin.Roles[StartTeam.Scp].Count}");
                List<ICustomRole>.Enumerator roles = Plugin.Roles[StartTeam.Scp].GetEnumerator();
                CustomRole? role = CustomRoleMethods.GetCustomRole(ref roles, false, true);

                Log.Debug($"VVUP Custom Roles: Got custom role {role?.Name}");

                if (role != null)
                {
                    int activeRoleCount = role.TrackedPlayers.Count;
                    Log.Debug($"VVUP Custom Roles: Active count for role {role.Name} is {activeRoleCount}");

                    if (activeRoleCount < role.SpawnProperties.Limit)
                    {
                        if (ev.Target.GetCustomRoles().Count == 0)
                            role.AddRole(ev.Target);
                    }
                    else
                    {
                        Log.Debug($"VVUP Custom Roles: Role {role.Name} has reached its spawn limit. Not Spawning");
                    }
                }

                roles.Dispose();
            }
        }
    }
}