using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Abilities.Active
{
    [CustomAbility]
    public class Detect : ActiveAbility
    {
        public override string Name { get; set; } = "Detect";

        public override string Description { get; set; } = "Detects Hostiles Near By.";

        public override float Duration { get; set; } = 0f;

        public override float Cooldown { get; set; } = 120f;

        public string message;

        public float DetectRange { get; set; } = 30f;

        public Dictionary<RoleTypeId, string> RoleNames { get; set; } = new Dictionary<RoleTypeId, string>()
        {
            {RoleTypeId.Scientist, "Scientist"},
            {RoleTypeId.NtfCaptain, "MTF Captain"},
            {RoleTypeId.NtfPrivate, "MTF Private"},
            {RoleTypeId.NtfSergeant, "MTF Sergeant"},
            {RoleTypeId.NtfSpecialist, "MTF Specialist"},
            {RoleTypeId.FacilityGuard, "Facility Guard"},
            {RoleTypeId.ChaosConscript, "Chaos Conscript"},
            {RoleTypeId.ChaosMarauder, "Chaos Marauder"},
            {RoleTypeId.ChaosRepressor, "Chaos Repressor"},
            {RoleTypeId.ChaosRifleman, "Chaos Rifleman"},
            {RoleTypeId.ClassD, "Class D"},
            {RoleTypeId.Scp049, "SCP-049"},
            {RoleTypeId.Scp0492, "SCP-049-2"},
            {RoleTypeId.Scp079, "SCP-079"},
            {RoleTypeId.Scp096, "SCP-096"},
            {RoleTypeId.Scp106, "SCP-106"},
            {RoleTypeId.Scp173, "SCP-173"},
            {RoleTypeId.Scp939, "SCP-939"},
            {RoleTypeId.Scp3114, "SCP-3114"}
        };

        protected override void AbilityUsed(Player player)
        {
            ActivateDetect(player);
            DisplayHint(player);
        }

        private void ActivateDetect(Player ply)
        {
            Log.Debug("VVUP Custom Abilities: Activating Detect");
            List<Player> detectedPlayers = new List<Player>();

            foreach (Player p in Player.List)
            {
                if (ply.IsCHI)
                {
                    if (Vector3.Distance(ply.Position, p.Position) <= DetectRange &&
                        (p.Role == RoleTypeId.Scientist || p.IsNTF || p.Role == RoleTypeId.FacilityGuard ||
                         p.IsScp || p.Role == RoleTypeId.Tutorial))
                        detectedPlayers.Add(p);
                }
                else if (ply.IsNTF)
                {
                    if (Vector3.Distance(ply.Position, p.Position) <= DetectRange &&
                        (p.IsCHI || p.Role == RoleTypeId.ClassD || p.IsScp || p.Role == RoleTypeId.Tutorial))
                        detectedPlayers.Add(p);
                }
                else
                {
                    if (Vector3.Distance(ply.Position, p.Position) <= DetectRange &&
                        (p.IsCHI || p.IsNTF || p.Role == RoleTypeId.ClassD || p.Role == RoleTypeId.Scientist))
                        detectedPlayers.Add(p);
                }
            }

            if (detectedPlayers.Count > 0)
            {
                Log.Debug($"VVUP Custom Abilities: Showing detected players to {ply.Nickname}");
                foreach (Player detectedPlayer in detectedPlayers)
                {
                    if (RoleNames.TryGetValue(detectedPlayer.Role, out string roleName))
                    {
                        message += $"{roleName}\n";
                    }
                    else
                    {
                        message += "Unknown Role\n";
                    }
                }
            }
            else
            {
                message = "There is no detected targets near you";
            }
        }

        public void DisplayHint(Player pl)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                pl.ShowHint(message, 10f);
            });
        }
    }
}