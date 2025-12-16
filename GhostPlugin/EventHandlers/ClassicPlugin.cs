using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using GhostPlugin.Methods.Objects;
using GhostPlugin.Methods.ToyUtils;
using MEC;
using PlayerRoles;
using UnityEngine;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MapEvents = Exiled.Events.Handlers.Map;

namespace GhostPlugin.EventHandlers
{
    public class ClassicPlugin
    {
        private static int _activatedGenerators = 0;
        private static CoroutineHandle _broadcastCoroutine;
        private static bool _printedReadyToEndOnce;
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Scp096.AddingTarget += OnLookingAtScp096;
            Exiled.Events.Handlers.Scp096.Enraging += OnEnraging;
            MapEvents.AnnouncingDecontamination += OnAnnouncingDecontemination;
            MapEvents.Decontaminating += OnDecontaminating;
            MapEvents.GeneratorActivating += OnActivateGenerator;
            Exiled.Events.Handlers.Warhead.Starting += OnWarheadStarting;
            Exiled.Events.Handlers.Warhead.Stopping += OnWarheadStopped;
            MapEvents.AnnouncingScpTermination += OnScpDied;
            MapEvents.AnnouncingNtfEntrance += OnAnnouncingNtfEntrance;
            PlayerEvents.Verified += OnVerified;
            ServerEvents.RoundStarted += OnRoundStarted; 
            ServerEvents.RestartingRound += OnRestartingRound;
            ServerEvents.EndingRound += OnRoundEnding;
            PlayerEvents.Left += OnPlayerLeft;
            PlayerEvents.Dying += OnDying;
        }

        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Scp096.AddingTarget -= OnLookingAtScp096;
            Exiled.Events.Handlers.Scp096.Enraging -= OnEnraging;
            MapEvents.AnnouncingDecontamination -= OnAnnouncingDecontemination;
            MapEvents.Decontaminating -= OnDecontaminating;
            MapEvents.GeneratorActivating -= OnActivateGenerator;
            Exiled.Events.Handlers.Warhead.Starting -= OnWarheadStarting;
            Exiled.Events.Handlers.Warhead.Stopping -= OnWarheadStopped;
            MapEvents.AnnouncingScpTermination -= OnScpDied;
            MapEvents.AnnouncingNtfEntrance -= OnAnnouncingNtfEntrance;
            PlayerEvents.Verified -= OnVerified;
            ServerEvents.RoundStarted -= OnRoundStarted; 
            ServerEvents.RestartingRound -= OnRestartingRound;
            ServerEvents.EndingRound -= OnRoundEnding;
            PlayerEvents.Left -= OnPlayerLeft;
            PlayerEvents.Dying -= OnDying;
        }

        private static void OnRoundEnding(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(_broadcastCoroutine);
            /*if (TeamDeathmatch.Plugin.Instance?.Config?.IsEnabled == true)
                return;*/
            var alive = Player.List.Where(p => p.IsAlive).ToList();
            int scps   = alive.Count(p => p.Role.Team == Team.SCPs);
            int chaos  = alive.Count(p => p.Role.Team == Team.ChaosInsurgency);
            int mtf    = alive.Count(p => p.Role.Team == Team.FoundationForces );
            int others = alive.Count(p => p.Role.Team != Team.SCPs
                                          && p.Role.Team != Team.ChaosInsurgency
                                          && p.Role.Team != Team.FoundationForces);
            int scientist = alive.Count(p => p.Role.Team == Team.Scientists);
            int dboy = alive.Count(p => p.Role.Team == Team.ClassD);
            /*int dboy = alive.Count(p =>
                p.Role.Team == Team.ClassD &&
                !(p.IsCuffed && p.Cuffer != null && p.Cuffer.Role.Team == Team.FoundationForces)
            );*/
            if (!!_printedReadyToEndOnce || !Plugin.Instance.Config.Debug)
            {
                _printedReadyToEndOnce = true;
                //Log.Send($"[ClassicPlugin] EndingRound fired | IsAllowed={ev.IsAllowed} | SCP={scps}, CHAOS={chaos}, MTF={mtf}, OTHERS={others}, Dclass={dboy}", LogLevel.Debug, ConsoleColor.Blue);
            }

            // ✅ 적대 진영(SCP/Chaos)이 전멸했고 MTF만 남았으면 강제로 허용
            if (!_printedReadyToEndOnce || !ev.IsAllowed && dboy == 0 && scps == 0 && chaos == 0 && mtf >= 1)
            {
                ev.IsAllowed = true; // 핵심: 다른 플러그인의 취소를 무력화
                _printedReadyToEndOnce = true;
                Timing.CallDelayed(5, () => Round.Restart());
                Log.Info("[ClassicPlugin] Forced round end allowed (MTF victory).");
            }

            // 문제 많던 커스텀 롤/튜토리얼 추적
            var tutorials = Player.List.Where(p => p.IsAlive && p.Role.Type == RoleTypeId.Tutorial).ToList();
            if (tutorials.Count > 0)
                Log.Info($"[DEBUG] Alive Tutorials: {string.Join(", ", tutorials.Select(t => t.Nickname))}");

            // 예: Spy/스파이 에이전트 표식이 있다면 그 기준으로도 찍어주세요.
            // var spies = Player.List.Where(IsSpyAgentPredicate).ToList();
            // Log.Info($"[DEBUG] Alive Spies: {spies.Count}");
        }

        private static void OnRestartingRound()
        {
            /*if (Plugin.Instance.Config.EnableTestVersion)
            {
                CustomLiteNetLib4MirrorTransport.DelayConnections = true;
                IdleMode.SetIdleMode(false);
                IdleMode.PauseIdleMode = true;
                ServerConsole.AddOutputEntry((IOutputEntry)new ExitActionShutdownEntry());
                new RoundRestartMessage(RoundRestartType.RedirectRestart, 0.1f, Server.Port, true, false)
                    .SendToAuthenticated<RoundRestartMessage>();
                Timing.CallDelayed(5, () => { Shutdown.Quit(); });
            }*/
            if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.IsSafeMode)
                Timing.CallDelayed(3.5f, () => Server.ExecuteCommand("sr"));
        }
        
        private static void OnLookingAtScp096(AddingTargetEventArgs ev)
        {
            ev.Target.Broadcast(5,"<color=#ff0000><size=40>You cannot able to survive...</size></color>");
        }
        
        
        private static void OnEnraging(EnragingEventArgs ev)
        {
            Log.Debug(ev.Player.Nickname + " has just been enraged by SCP-096!");
            ev.Player.Broadcast(5, "<size=35><color=red>Rip and Tear...Until it's done...</color></size>");;
        }
        
        private static void OnVerified(VerifiedEventArgs ev)
        {
            /*if (Config.JoinMessage != null && Config.JoinMessage.Duration > 0 && !Config.JoinMessage.Message.IsEmpty())
            {
                Log.Debug("Showing Verified message to " + ev.Player.Nickname);
                string message = Config.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
                ev.Player.Broadcast(Config.JoinMessage.Duration, message, default, Config.JoinMessage.Override);
            }*/
            Log.Debug("Showing Verified message to " + ev.Player.Nickname);
            string message = Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.JoinMessage.Message.Replace("%name%", ev.Player.Nickname);
            ev.Player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.JoinMessage.Duration, message, default, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.JoinMessage.Override);
        }
        
        private static void OnActivateGenerator(GeneratorActivatingEventArgs ev)
        {
            _activatedGenerators++;
            Map.Broadcast(10, $"<size=30><color=#69f5ff>⚙️</color><color=#00ff00>{_activatedGenerators} out of three</color>Generators <color=green>Activated</color></size>",shouldClearPrevious:true);
            if (_activatedGenerators >= 3)
            {
                _activatedGenerators = 0;
            }
        }
        
        private static void OnPlayerLeft(LeftEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                Map.Broadcast(10,$"<color=red><b>{ev.Player.Role.Name}Quit the game...</b></color>\n<size=30><color=#d44444>User name : {ev.Player.Nickname}\n User ID: {ev.Player.UserId.ToString()}</color></size>");
                Log.SendRaw($"[NAME] - {ev.Player.Nickname} SCP player Quit the game...",ConsoleColor.Red);
                Log.SendRaw($"[ID] - {ev.Player.UserId} was quit the game...",ConsoleColor.Red);
            }

            else switch (Round.IsStarted)
            {
                case false:
                    Log.SendRaw($"[NAME] - {ev.Player.Nickname} 이 탈주하였습니다...",ConsoleColor.Green);
                    Log.SendRaw($"[ID] - {ev.Player.UserId} 이 탈주하였습니다...",ConsoleColor.Green);
                    break;
                case true:
                    if (ev.Player.Role.Type == RoleTypeId.Spectator)
                    {
                        Log.SendRaw($"[NAME] - {ev.Player.Nickname} 이 탈주하였습니다...",ConsoleColor.DarkGreen);
                        Log.SendRaw($"[ID] - {ev.Player.UserId} 이 탈주하였습니다...",ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        Log.SendRaw($"[NAME] - {ev.Player.Nickname} 이 탈주하였습니다...",ConsoleColor.DarkRed);
                        Log.SendRaw($"[ID] - {ev.Player.UserId} 이 탈주하였습니다...",ConsoleColor.DarkRed);
                    }
                    break;
            }
        }
        
        private static void OnDying(DyingEventArgs ev)
        {
            if (ev.Player == null)
                return;
            var attacker = ev.Attacker;
            if (attacker == null || attacker.Group == null || string.IsNullOrEmpty(attacker.Group.BadgeText))
                return;
            if(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.DonatorList.Contains(ev.Attacker.Group
                   .BadgeText))
            {
                Color glowcolor =new Color(1f, 0.0f, 0.0f, 0.1f) * 50f;
                SpawnPrimitiveToy.Spawn(ev.Player, 15, glowcolor);
                TextUtils.SpawnText(ev.Player, ev.Player.Position,"<size=10>Content Deleted</size>",15f);
                ev.Player.Vaporize();
            }
            else
            {
                return;
            }
        }
        
        private static void OnScpDied(AnnouncingScpTerminationEventArgs ev)
        {
            string message = $"<size=35><color=orange>📢</color>{ev.Role.Name} was <color=#d0ff4f>contain<color> Succesfully. \n{DetermineCauseOfDeath(ev)}</size>";
            Map.Broadcast(7,message);
            Log.Debug(message);
        }
        private static string DetermineCauseOfDeath(AnnouncingScpTerminationEventArgs ev)
        {
            if (ev.DamageHandler.Type == DamageType.Explosion)
            {
                return "<color=#ff8336>Reason: 💥Explosion💥</color>";
            }

            if (ev.DamageHandler.Type == DamageType.Tesla)
            {
                return "<color=#42e9ff>Reason: Security System </color>";
            }
            if (ev.DamageHandler.Type == DamageType.Decontamination)
            {
                return "<color=#687548>Reason: ☢ organic removal protocol ☢ </color>";
            }
            if (ev.Attacker != null && ev.Attacker.UnitName != null)
            {
                return $"<size=35><color=#d7ff36>🔫Name: </color> by {ev.Attacker.Nickname}\n Role: {GetTeamName(ev.Attacker.Role.Team)}</size>";
            }
            if (ev.DamageHandler.Type == DamageType.Unknown)
            {
                return "<size=35><color=#c2c2c2>사유: ❓Unknown ❓</color>/size>";
            }
            if (ev.Attacker == null)
            {
                return "<size=35><color=#c2c2c2>사유: ❓Unknown ❓</color></size>";
            }
            
            return ev.DamageHandler.Type.ToString();
        }
        private static string GetTeamName(Team team)
        {
            switch (team)
            {
                case Team.FoundationForces:
                    return "<size=35><color=blue>Mobile Task Force</color></size>";
                case Team.ChaosInsurgency:
                    return "<size=35><color=green>C.I</color></size>";
                case Team.Scientists:
                    return "<size=35><color=yellow>Scientist</color></size>";
                case Team.ClassD:
                    return "<size=35><color=orange>D-Class Personal</color></size>";
                case Team.OtherAlive:
                    return "<size=35><color=#c2c2c2>Unkown</color></size>";
                case Team.SCPs:
                    return "<size=35><color=red>SCP</color></size>";
                default:
                    return "<size=35><color=#c2c2c2>Unkown</color></size>";
            }
        }
        private static void OnRoundStarted()
        {
            _activatedGenerators = 0;
            _broadcastCoroutine = Timing.RunCoroutine(BroadcastEveryThreeMinutes());
            Map.Broadcast(5, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.RoundStartMSG);
            if (BlackoutMod.IsBlackout)
                return;
            if(!TeamDeathmatch.Plugin.Instance.Config.IsEnabled)
                Exiled.API.Features.Cassie.MessageTranslated(message: "Attention Containment breach detected", isSubtitles: true, translation: "Attention <color=red>Containment breach</color> detected", isNoisy: false);
        }
        /*private static void OnRoundEnded(EndingRoundEventArgs ev)
        {
            Timing.KillCoroutines(_broadcastCoroutine);
        }*/
        private static void OnWarheadStopped(StoppingEventArgs ev)
        {
            Map.Broadcast(5, "<size=30><color=#418043> ⚠ Detonation Cancelled ⚠ </color></size>",shouldClearPrevious:true);
        }

        private static void OnAnnouncingNtfEntrance(AnnouncingNtfEntranceEventArgs ev)
        {
            Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.NtfRespawntime, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.NtfRespawn.ToString().Replace("{0}", $"{ev.UnitName}").Replace("{1}", $"{ev.UnitNumber}").Replace("{2}", $"{ev.ScpsLeft}"));
        }
        
        private static void OnWarheadStarting(StartingEventArgs ev)
        { 
            var timeLeft = Warhead.RealDetonationTimer;
            Map.Broadcast(5, $"<color=#fc6603><size=30> ☢ 경고! 알파 핵탄두가 작동을 시작했습니다. ☢ \n남은 시간안에 모든 인원은 신속히 <color=#74ff5e>시설외밖</color>으로 대피하시기 바랍니다.\n<color=red>남은시간: {(int)timeLeft}초 </color></size></color>", shouldClearPrevious:true);
        }

        private static void OnDecontaminating(DecontaminatingEventArgs ev)
        {
            Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.LcZstarttime, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.LcZstart);
            Log.Debug("OnDecontaminating activated");
        }
        
        private static void OnAnnouncingDecontemination(AnnouncingDecontaminationEventArgs ev)
        {
            if(BlackoutMod.IsBlackout == true)
                return;
            switch (ev.Id)
            {
                case 0:
                {
                    if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage) BroadCastLcZinPlayers(0);
                    else Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz15Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz15);
                    Log.Debug("Announcing LCZ Decontemination T-minus 15 min");
                    break;
                }
                case 1:
                {
                    if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage) BroadCastLcZinPlayers(1);
                    else Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz10Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz10);
                    Log.Debug("Announcing LCZ Decontemination T-minus 10 min");
                    break;
                }
                case 2:
                {
                    if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage) BroadCastLcZinPlayers(2);
                    else Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz5Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz5);
                    Log.Debug("Announcing LCZ Decontemination T-minus 5 min");
                    break;
                }
                case 3:
                {
                    if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage) BroadCastLcZinPlayers(3);
                    else Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz1Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz1);
                    Log.Debug("Announcing LCZ Decontemination T-minus 1 min");
                    break;
                }
                case 4:
                {
                    if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage) BroadCastLcZinPlayers(4);
                    else Map.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz30Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz30);
                    Log.Debug("Announcing LCZ Decontemination T-minus 30 sec");
                    break;
                }
            }
        }
        private static void BroadCastLcZinPlayers(int a)
        {
            foreach (var player in Player.List)
            {
                if (player.Position.y < 20 && player.Position.y > -1 && player.IsAlive && player.Role != RoleTypeId.None)
                {
                    if (a == 0) player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz15Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz15);
                    else if (a == 1) player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz10Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz10);
                    else if (a == 2) player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz5Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz5);
                    else if (a == 3) player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz1Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz1);
                    else if (a == 4) player.Broadcast(Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz30Time, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.Lcz30);
                }
            }
        }

        private static IEnumerator<float> BroadcastEveryThreeMinutes()
        {
            while (Round.IsStarted)
            {
                if (Round.IsEnded)
                {
                    yield break;
                }
                
                // 180초 대기
                yield return Timing.WaitForSeconds(180);
                
                // 메시지 방송
                Map.Broadcast(15, Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.AnnouncmentMessage, type: Broadcast.BroadcastFlags.Normal);
                Log.Debug($"Announcement Send {Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.AnnouncmentMessage}");
            }
        }
    }
}