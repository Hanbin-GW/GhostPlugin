using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Scp049;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using GhostPlugin.API;
using GhostPlugin.Methods.CustomRoles;

namespace GhostPlugin.EventHandlers
{
    public class CustomRoleHandler_old
    {
        private readonly Plugin Plugin;
        public CustomRoleHandler_old(Plugin plugin) => Plugin = plugin;
        public void OnRoundStarted()
        {
            List<ICustomRole>.Enumerator dClassRoles = new();
            List<ICustomRole>.Enumerator scientistRoles = new();
            List<ICustomRole>.Enumerator guardRoles = new();
            // Can apply to any SCPs
            List<ICustomRole>.Enumerator scpRoles = new(); 
            // Specific SCP roles
            List<ICustomRole>.Enumerator scp173Roles = new();
            List<ICustomRole>.Enumerator scp106Roles = new();
            List<ICustomRole>.Enumerator scp049Roles = new();
            List<ICustomRole>.Enumerator scp079Roles = new();
            List<ICustomRole>.Enumerator scp096Roles = new();
            List<ICustomRole>.Enumerator scp939Roles = new();
            List<ICustomRole>.Enumerator scp3114Roles = new();

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
                    case StartTeam.Scp173:
                        scp173Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp106:
                        scp106Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp049:
                        scp049Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp079:
                        scp079Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp096:
                        scp096Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp939:
                        scp939Roles = kvp.Value.GetEnumerator();
                        break;
                    case StartTeam.Scp3114:
                        scp3114Roles = kvp.Value.GetEnumerator();
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
                    case RoleTypeId.Scp173:
                        role = CustomRoleMethods.GetCustomRole(ref scp173Roles);
                        break;
                    case RoleTypeId.Scp106:
                        role = CustomRoleMethods.GetCustomRole(ref scp106Roles);
                        break;
                    case RoleTypeId.Scp049:
                        role = CustomRoleMethods.GetCustomRole(ref scp049Roles);
                        break;
                    case RoleTypeId.Scp079:
                        role = CustomRoleMethods.GetCustomRole(ref scp079Roles);
                        break;
                    case RoleTypeId.Scp096:
                        role = CustomRoleMethods.GetCustomRole(ref scp096Roles);
                        break;
                    case RoleTypeId.Scp939:
                        role = CustomRoleMethods.GetCustomRole(ref scp939Roles);
                        break;
                    case RoleTypeId.Scp3114:
                        role = CustomRoleMethods.GetCustomRole(ref scp3114Roles);
                        break;
                    case { } when player.Role.Side == Side.Scp:
                        role = CustomRoleMethods.GetCustomRole(ref scpRoles);
                        break;
                }

                if (player.GetCustomRoles().Count == 0)
                    role?.AddRole(player);
            }

            guardRoles.Dispose();
            scientistRoles.Dispose();
            dClassRoles.Dispose();
            scpRoles.Dispose();
            scp173Roles.Dispose();
            scp106Roles.Dispose();
            scp049Roles.Dispose();
            scp079Roles.Dispose();
            scp096Roles.Dispose();
            scp939Roles.Dispose();
            scp3114Roles.Dispose();
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            /*if (C_Squad.Plugin.Instance.IsSpawnable || Reinforcements.Plugin.Instance.IsSpawnable)
                return;*/
            bool cSquadActive = false;
            bool reinfActive = false;
            try
            {
                var asm = Assembly.Load("C_Squad");
                var type = asm.GetType("C_Squad.Plugin");
                var instance = type?.GetProperty("Instance")?.GetValue(null);
                cSquadActive = instance != null &&
                               (bool)(type.GetProperty("IsSpawnable")?.GetValue(instance) ?? false);
            }
            catch
            {
                Log.Warn($"[CustomRoles] Failed to check C_Squad");
            }

            try
            {
                var asm = Assembly.Load("Reinforcements");
                var type = asm.GetType("Reinforcements.Plugin");
                var instance = type?.GetProperty("Instance")?.GetValue(null);
                reinfActive = instance != null &&
                              (bool)(type.GetProperty("IsSpawnable")?.GetValue(instance) ?? false);
            }
            catch
            {
                Log.Warn($"[CustomRoles] Failed to check Reinforcements");
            }

            // 외부가 스폰 담당이면 그냥 빠져나감
            if (cSquadActive || reinfActive)
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
                case Faction.FoundationEnemy:
                {
                    if (Plugin.Roles.TryGetValue(StartTeam.Chaos, out List<ICustomRole> role))
                        roles = role.GetEnumerator();
                    break;
                }
                case Faction.FoundationStaff:
                {
                    if (Plugin.Roles.TryGetValue(StartTeam.Ntf, out List<ICustomRole> pluginRole))
                        roles = pluginRole.GetEnumerator();
                    break;
                }
            }

            foreach (Player player in ev.Players)
            {
                CustomRole role = CustomRoleMethods.GetCustomRole(ref roles);

                if (player.GetCustomRoles().Count == 0)
                    role?.AddRole(player);
            }

            roles.Dispose();
        }

        public void FinishingRecall(FinishingRecallEventArgs ev)
        {
            Log.Debug($"VVUP Custom Roles: {nameof(FinishingRecall)}: Selecting random zombie role.");
            if (Plugin.Roles.ContainsKey(StartTeam.Scp) && ev.Target is not null)
            {
                Log.Debug($"VVUP Custom Roles: {nameof(FinishingRecall)}: List count {Plugin.Roles[StartTeam.Scp].Count}");
                List<ICustomRole>.Enumerator roles = Plugin.Roles[StartTeam.Scp].GetEnumerator();
                CustomRole role = CustomRoleMethods.GetCustomRole(ref roles, false, true);

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