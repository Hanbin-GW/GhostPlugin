using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Loader;
using MEC;
using PlayerRoles;
using ServerEvents = Exiled.Events.Handlers.Server;

namespace GhostPlugin.EventHandlers
{
    public class BlackoutMod
    {
        public static bool IsBlackout;
        private static CoroutineHandle _checkBlackoutCoroutineHandle;
        
        public static void RegisterEvents()
        {
            ServerEvents.RoundStarted += OnRoundStarted;
            ServerEvents.EndingRound += OnRoundEnded;
            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
        }

        public static void UnregisterEvents()
        {
            ServerEvents.RoundStarted -= OnRoundStarted; 
            ServerEvents.EndingRound -= OnRoundEnded;
            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
        }
        
        private static void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if (IsBlackout)
                ev.IsAllowed = false;
        }
        private static void OnRoundStarted()
        {
            _checkBlackoutCoroutineHandle = Timing.RunCoroutine(CheckBlackout());
        }
        
        private static void OnRoundEnded(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(_checkBlackoutCoroutineHandle);
            IsBlackout = false;
        }
        
        private static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (BlackoutMod.IsBlackout == false)
                return;
            if (IsBlackout)
            { 
                if (ev.Player.Role.Team is Team.FoundationForces or Team.Scientists or Team.ChaosInsurgency)
                    Timing.CallDelayed(1.0f, () => ev.Player.AddItem(ItemType.Flashlight));
                if(ev.Player.Role.Team is Team.ClassD)
                    Timing.CallDelayed(1.0f, () => ev.Player.AddItem(ItemType.Lantern));
            }
            else
                return;
        }
        
        private static IEnumerator<float> CheckBlackout()
        {
            while (Round.IsStarted)
            {
                if (Round.IsEnded)
                {
                    yield break;
                }

                IsBlackout = Loader.Random.Next(100) <=
                             Plugin.Instance.Config.ServerEventsMasterConfig.BlackoutModeConfig.BlackoutChance;
                Log.Send($"[{Plugin.Instance.Name}] The Facility IsBlackout status: {IsBlackout}", LogLevel.Debug, ConsoleColor.DarkMagenta);
                
                if (IsBlackout)
                {
                    Cassie.Message(Plugin.Instance.Config.ServerEventsMasterConfig.BlackoutModeConfig.CassieMessage, false, 
                        false, true);
                    Log.Info("The Facility will be blackout for 100 seconds...");
                    Map.TurnOffAllLights(100);
                    Timing.WaitForSeconds(2f);
                    foreach (var player in Player.List)
                    {
                        if (Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.OnEnabled)
                        {
                            player.ShowHint("<color=white>" + new string('\n',10) + "제단의 전력이 파손되어 시설 전체가 정전상태가 되었습니다!\n전력이 복구될때까지 손전을을 사용하고 조심하셔야 됩니다!" + "</color>",10);
                        }
                        if (!player.Items.Any(item => item.Type == ItemType.Flashlight) || !player.Items.Any(item => item.Type == ItemType.Lantern))
                        {
                            // 플래시라이트가 없으면 추가
                            player.AddItem(ItemType.Flashlight);
                            //player.ShowHint("플래시라이트를 받았습니다!", 5);
                        }
                        else
                        {
                            // 이미 플래시라이트가 있는 경우 처리 (선택 사항)
                            //player.ShowHint("이미 플래시라이트를 소지하고 있습니다.", 5);
                        }                    }
                    Timing.CallDelayed(100, () =>
                    {
                        IsBlackout = false;
                        Cassie.MessageTranslated(
                            Plugin.Instance.Config.ServerEventsMasterConfig.BlackoutModeConfig.CassieOperationalMessage,
                            Plugin.Instance.Config.ServerEventsMasterConfig.BlackoutModeConfig
                                .CassieOperationalMessageTranslation, false, false, true);
                    });
                }
                else
                {
                    Log.Info("The Black Out is False");
                }
                
                yield return Timing.WaitForSeconds(150);

            }
        }
    }
}
