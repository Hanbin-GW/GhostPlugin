using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using GhostPlugin.Methods.Music;
using UnityEngine;

namespace GhostPlugin.EventHandlers
{
    public class KillStreakEventHandlers
    {
        public Dictionary<Player, int> KillStreak = new();
        public Dictionary<Player, float> LastKillTime = new();
        public Dictionary<Player, int> ComboKill = new();
        private const float ComboWindow = 4.5f;
        
        public void OnRoundStarted()
        {
            KillStreak.Clear();
            ComboKill.Clear();
            LastKillTime.Clear();
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            KillStreak.Clear();
            ComboKill.Clear();
            LastKillTime.Clear();
        }
        public void OnDied(DiedEventArgs ev)
        {
            var killer = ev.Attacker;
            var victim = ev.Player;

            // Check suicide, environmental history, etc
            if (killer is null || killer == victim)
            {
                KillStreak.Remove(victim);
                ComboKill.Remove(victim);
            }

            float now = Time.time;

            // kill streak
            if (killer != null && !KillStreak.ContainsKey(killer))
                KillStreak[killer] = 0;

            KillStreak[killer]++;          // 죽지 않고 킬했으니 +1

            // Reset victim's killstreak due to dead
            KillStreak[victim] = 0;
            ComboKill[victim] = 0;
            LastKillTime[victim] = 0;

            // Kill combo logic
            if (!LastKillTime.ContainsKey(killer))
            {
                // Started in 1 when initial kill
                ComboKill[killer] = 1;
            }
            else
            {
                float diff = now - LastKillTime[killer];

                if (diff <= ComboWindow)
                {
                    // eliminate untile kill time is done. → increase kill combo
                    ComboKill[killer]++;
                }
                else
                {
                    // Timeout → Reset Kill Count
                    ComboKill[killer] = 1;
                }
            }

            LastKillTime[killer] = now;

            // print message
            ShowKillStreakMessage(killer, victim,KillStreak[killer]);
            ShowComboMessage(killer, ComboKill[killer]);
        }
        private void ShowComboMessage(Player player, int combo)
        {
            switch (combo)
            {
                case 2:
                    player.Broadcast(4, $"<color=#947b00>Crazy</color>");
                    MusicMethods.PlaySoundPlayer("Crazy.ogg",player,5);
                    break;
                case 3:
                    player.Broadcast(4, "<size=35>💀</size>\n<color=#36d1b7>Badass</color>");
                    MusicMethods.PlaySoundPlayer("Badass.ogg",player,5);
                    break;
                case 4:
                    player.Broadcast(4, $"<size=35>🔪</size>\n<color=#b7ed2f>Savage</color>");
                    MusicMethods.PlaySoundPlayer("Headshot.ogg",player,5);
                    break;
                case 5:
                    player.Broadcast(4, $"<size=35>🔥</size>\n<color=#ed492f>SickSkills</color>");
                    MusicMethods.PlaySoundPlayer("SAVAGE.ogg",player, 5);
                    break;
                case 6:
                    player.Broadcast(4, $"<size=35>🔥</size>\n<color=#c7b622>Smocking Sexy Style</color>");
                    MusicMethods.PlaySoundPlayer("SAVAGE.ogg",player, 5);
                    break;
            }
        }

        private void ShowKillStreakMessage(Player player, Player victim, int count)
        {
            switch (count)
            {
                case 1:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀</size>",5);
                    MusicMethods.PlaySoundPlayer("OneKill.ogg",player, 5);
                    break;
                case 2:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("TwoKill.ogg",player, 5);
                    break;
                case 3:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("ThreeKill.ogg",player, 5);
                    break;
                case 4:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("`.ogg",player, 5);
                    break;
                case 5:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("ACE.ogg",player, 5);
                    break;
                case 6:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("killsound.ogg",player, 1);

                    break;
                case 7:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("killsound.ogg",player, 1);
                    break;
                case 8:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("killsound.ogg",player, 1);
                    break;
                case 9:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("killsound.ogg",player, 1);
                    break;
                case 10:
                    player.ShowHint($"{victim.Nickname} | {victim.PreviousRole} <color=red>eliminated</color> \n<size=25>💀💀💀💀💀💀💀💀💀💀</size>",5);
                    MusicMethods.PlaySoundPlayer("killsound.ogg",player, 1);
                    break;
            }
        }
        private void ResetStats(Player player)
        {
            if (player is null)
                return;

            KillStreak.Remove(player);
            ComboKill.Remove(player);
            LastKillTime.Remove(player);
        }

    }
}