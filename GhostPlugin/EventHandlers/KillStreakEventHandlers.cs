using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using UnityEngine;

namespace GhostPlugin.EventHandlers
{
    public class KillStreakEventHandlers
    {
        public Dictionary<Player, int> KillStreak = new();
        public Dictionary<Player, float> LastKillTime = new();
        public Dictionary<Player, int> ComboKill = new();
        private const float ComboWindow = 8f;
        
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
                    Map.Broadcast(4, $"{player.Nickname} - <color=cyan>더블킬!</color>");
                    break;
                case 3:
                    Map.Broadcast(4, $"{player.Nickname} - <color=cyan>트리플킬!!</color>");
                    break;
                case 4:
                    Map.Broadcast(4, $"{player.Nickname} - <color=cyan>쿼드라킬!!!</color>");
                    break;
                case 5:
                    Map.Broadcast(4, $"{player.Nickname} - <color=cyan>펜타킬!!!!</color>");
                    break;
            }
        }

        private void ShowKillStreakMessage(Player player, Player victim, int count)
        {
            switch (count)
            {
                case 2:
                    player.ShowHint($"{victim.Nickname} | {victim.UnitName} 처치 \n<size=20>💀💀</size>",5);
                    break;
                case 3:
                    player.ShowHint($"{victim.Nickname} | {victim.UnitName} 처치 \n<size=20>💀💀💀</size>",5);
                    break;
                case 4:
                    player.ShowHint($"{victim.Nickname} | {victim.UnitName} 처치 \n<size=20>💀💀💀💀</size>",5);
                    break;
                case 5:
                    player.ShowHint($"{victim.Nickname} | {victim.UnitName} 처치 \n<size=20>💀💀💀💀💀</size>",5);
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