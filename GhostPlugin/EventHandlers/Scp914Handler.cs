using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Scp914;
using MEC;
using PlayerRoles;
using Scp914;
using UnityEngine;
using scp914 = Exiled.Events.Handlers.Scp914;

namespace GhostPlugin.EventHandlers
{
    public class Scp914Handler
    {
        public static void RegisterEvents()
        {
            scp914.UpgradingPlayer += OnUpgradingPlayer;
            scp914.Activating += OnActivating914;
        }

        public static void UnregisterEvents()
        {
            scp914.UpgradingPlayer += OnUpgradingPlayer;
            scp914.Activating += OnActivating914;
        }

        private static readonly List<RoomType> RoomTypes = new List<RoomType>
        {
            //RoomType.Unknown,
            RoomType.LczArmory,
            RoomType.LczCurve,
            RoomType.LczStraight,
            RoomType.Lcz914,
            RoomType.LczCrossing,
            RoomType.LczTCross,
            RoomType.LczCafe,
            RoomType.LczPlants,
            RoomType.LczToilets,
            RoomType.LczAirlock,
            RoomType.Lcz173,
            RoomType.LczClassDSpawn,
            RoomType.LczCheckpointB,
            RoomType.LczGlassBox,
            RoomType.LczCheckpointA,
            RoomType.Hcz079,
            RoomType.HczEzCheckpointA,
            RoomType.HczEzCheckpointB,
            RoomType.HczArmory,
            RoomType.Hcz939,
            RoomType.HczHid,
            RoomType.Hcz049,
            RoomType.HczCrossing,
            RoomType.Hcz106,
            RoomType.HczNuke,
            RoomType.HczTesla,
            RoomType.HczCurve,
            RoomType.Hcz096,
            RoomType.EzVent,
            RoomType.EzIntercom,
            //RoomType.EzGateA,
            RoomType.EzDownstairsPcs,
            RoomType.EzCurve,
            RoomType.EzPcs,
            RoomType.EzCrossing,
            RoomType.EzCollapsedTunnel,
            RoomType.EzConference,
            RoomType.EzStraight,
            RoomType.EzCafeteria,
            RoomType.EzUpstairsPcs,
            //RoomType.EzGateB,
            //RoomType.EzShelter,
            //RoomType.Pocket,
            //RoomType.Surface,
            RoomType.HczStraight,
            RoomType.EzTCross,
            RoomType.Lcz330,
            RoomType.EzCheckpointHallwayA,
            RoomType.EzCheckpointHallwayB,
            RoomType.HczTestRoom,
            RoomType.HczElevatorA,
            RoomType.HczElevatorB
        };

        private static void OnActivating914(ActivatingEventArgs ev)
        {
            string mode = ev.KnobSetting.ToString();
            List<Player> playersIn914Chamber = new List<Player>();
            string activatorName = ev.Player.Nickname;
            string message = Plugin.Instance.Config.Scp914Config.ActivationMessage.Replace("{activator}", activatorName).Replace( "{mode}", mode);
            foreach (Player player in Player.List)
            {
                if (player.CurrentRoom.Type == RoomType.Lcz914)
                {
                    playersIn914Chamber.Add(player);
                    player.Broadcast(Plugin.Instance.Config.Scp914Config.MessageDuration ,message);
                }
            }
            
            Log.Debug($"SCP-914 activated on {mode}. Notifying {playersIn914Chamber.Count} players.");
        }
        private static void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            Scp914KnobSetting scp914KnobSetting = ev.KnobSetting;
            if (ev.Player == null)
            {
                Log.Error("Player is null in OnUpgradingPlayer.");
                return;
            }
            
            foreach (Player player in Player.List.Where(p => p.CurrentRoom.Type == RoomType.Lcz914))
            {
                // Notify the player about the current knob setting
                player.Broadcast(5, $"SCP-914 is set to: {scp914KnobSetting}");
            }

            if (ev.KnobSetting == Scp914KnobSetting.VeryFine && ev.Player.IsScp && ev.Player.Role != RoleTypeId.Scp0492)
            {
                if (Random.Range(0, 100) <= 70)
                {
                    ChangeScpRole(ev.Player);
                    ev.Player.Broadcast(5, "<size=35>당신의 진영이 변경 되었습니다.</size>");
                    Log.Info("class change detected");
                }

                else
                {
                    ev.Player.EnableEffect<Flashed>(duration: 1);
                    ev.Player.EnableEffect<Slowness>(duration: 50, intensity: 15);
                    ev.Player.ShowHint("당신은 914에 내부 결함으로 인해 15초간 스턴 효과가 부여되었습니다...",15);
                }
            }
            if (ev.KnobSetting == Scp914KnobSetting.VeryFine && ev.Player.IsHuman)
            {
                if (Random.Range(0, 100) < 5)
                {
                    ev.Player.Broadcast(5,"<size=30><color=red>당신은 Scp3114 로 변환 되었습니다..</color></size>");
                    ev.Player.Role.Set(RoleTypeId.Scp3114,RoleSpawnFlags.UseSpawnpoint);
                }
                if (Random.Range(0, 100) < 10) 
                { 
                    ev.Player.EnableEffect<MovementBoost>(intensity:225,duration:30); 
                    ev.Player.Broadcast(5, "<size=30>당신은 30초간 <b><color=aqua>glitcher</color></b> 입니다!! (255%속도증가)</size>");
                }
                    
                else if (Random.Range(0, 100) < 18)
                {
                }
                
                else if (Random.Range(0, 100) < 25)
                {
                }

                else if(Random.Range(0,100) < 40)
                {
                    Log.Debug($"{ev.Player}'s [w][a][s][d] key is Reversed");
                    ev.Player.ShowHint("<color=red>당신은 어지럼증을 느끼기 시작합니다...</color>\n(15 초간 wasd 키 반전)",10);
                    ev.Player.EnableEffect<Exhausted>(duration: 3);
                    Timing.CallDelayed(5, () => ev.Player.EnableEffect<Slowness>(intensity: 200, duration: 15));
                }
                
                else if (Random.Range(0, 100) < 50)
                {
                    ev.Player.Broadcast(5,"당신은 과학자로 외형만 변경되었습니다.");
                    ev.Player.ChangeAppearance(RoleTypeId.Scientist,skipJump:true);
                    Log.Debug($"{ev.Player.Nickname} has changed only Scientist Reference..");
                }

                else if(Random.Range(0,100) < 60)
                {
                    ev.Player.Broadcast(5,"당신은 D-Class 로 외형만 변경되었습니다.");
                    ev.Player.ChangeAppearance(RoleTypeId.ClassD,skipJump:true);
                    Log.Debug($"{ev.Player.Nickname} has changed only D-Class Reference..");
                }

                else if(Random.Range(0,100) < 90)
                {
                }
                
                else
                {
                    ev.Player.ShowHint("아무일도 없었다...");
                    //ChangeHumanRole(ev.Player);
                    //ev.Player.Broadcast(5,"<size=35>진영이 10% 확률로 변경되었습니다</size>");
                }
            }

            if (ev.KnobSetting == Scp914KnobSetting.Coarse && ev.Player.IsHuman)
            {
                //ev.Player.Teleport(Room.Get(RoomType.EzPcs).Position);
                ev.Player.Health /= 2;
                int randomIndex = Random.Range(0, RoomTypes.Count);
                Room randomRoom = Room.Get(RoomTypes[randomIndex]);
                ev.OutputPosition = randomRoom.Position + Vector3.up;
                Log.Info($"{ev.Player.Nickname} has moved to {randomRoom.Name}");
            }


            if (ev.KnobSetting == Scp914KnobSetting.Rough && ev.Player.IsHuman)
            {
                if (Random.Range(0, 100) < 5)
                {
                    ev.Player.EnableEffect<MovementBoost>(intensity:225,duration:50); 
                    ev.Player.Broadcast(5, "<size=40>당신은 50초간 <b><color=aqua>glitcher</color></b> 입니다!! (255%속도증가)</size>");
                }
                else if (Random.Range(0, 100) < 15)
                {
                    ev.Player.EnableEffect<CardiacArrest>(duration: 10);
                    ev.Player.ShowHint("심작박동이 늦어지고 있습니다!\n아드레날린 주사기를 사용하십시요!",5);
                }
                else if (Random.Range(0, 100) < 20)
                {
                    ev.Player.Broadcast(5,"<size=30><color=red>당신은 좀비 바이러스에 감영되었습니다..</color></size>");
                    ev.Player.EnableEffect<Poisoned>(duration: 5);
                    Timing.CallDelayed(5f, () => ev.Player.Role.Set(RoleTypeId.Scp0492, RoleSpawnFlags.UseSpawnpoint));
                }
                else if(Random.Range(0,100) < 50)
                {
                    ev.Player.Health /= 4;
                    ev.Player.EnableEffect<Bleeding>(duration: 10);
                    ev.OutputPosition = Room.Get(RoomType.Surface).Position + Vector3.up;
                    ev.Player.ShowHint("시간 삭제 감지됨",10);
                    ev.Player.Broadcast(3,"<color=red>탈출구가 앞에있어요!</color>");
                    ev.Player.Broadcast(3,"<color=red>다친몸을 회복하고 전력을 다해 달리세요!</color>");
                }
                else
                {
                    ev.Player.ShowHint("아무일도 없었다...");
                }
            }
        }
        private static void ChangeScpRole(Player player)
        {
            RoleTypeId newScpRole = GetRandomScpRole(player.Role);
            float currentHealthPercent = player.Health / player.MaxHealth;
            player.ClearInventory();
            player.Role.Set(newScpRole, RoleSpawnFlags.UseSpawnpoint);
            player.Health = player.MaxHealth * currentHealthPercent;
        }
        private static RoleTypeId GetRandomScpRole(RoleTypeId currentScpRole)
        {
            RoleTypeId[] scpRoles = { RoleTypeId.Scp049 , RoleTypeId.Scp096, RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939, RoleTypeId.Scp3114 };

            RoleTypeId newRole;
            do
            {
                newRole = scpRoles[Random.Range(0, scpRoles.Length)];
            }
            while (newRole == currentScpRole);
            return newRole;
        }
    }
}