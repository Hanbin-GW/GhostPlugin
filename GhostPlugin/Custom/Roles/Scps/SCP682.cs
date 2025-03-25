using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using GhostPlugin.API;
using MEC;
using PlayerRoles;
using GhostPlugin.Custom.Roles.Foundation;
using Random = UnityEngine.Random;


namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp939)]
    public class Scp682 : CustomRole, ICustomRole
    {
        private Player _scp682Player;
        public override uint Id { get; set; } = 11;
        public override int MaxHealth { get; set; } = 80000;
        public override string Name { get; set; } = "SCP-682";
        public override string Description { get; set; } = "당신은 최강의 SCP 입니다..!\n 다른 SCP 들과 협력해 재단을 몰락하십시오!";
        public override string CustomInfo { get; set; } = "SCP-682";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp939;
        private readonly float _altKeyCooldownDuration = 90f;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        public StartTeam StartTeam { get; set; } = StartTeam.Scp;
        public int Chance { get; set; } = 0;
        public List<string> SteamUserids { get; set; } = new List<string>
        {
            "76561199133709329@steam", //Steam64ID 추가
            "76561199073020404@steam",
            "76561199091279621@steam"
        };
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (ev.DamageHandler.Type == DamageType.Explosion)
                {
                    ev.IsAllowed = true;
                    ev.Amount *= 1.1f;
                }

                if (ev.DamageHandler.Type == DamageType.Jailbird)
                {
                    ev.IsAllowed = true;
                    ev.Amount *= 4f;
                }
            }

            if (ev.Attacker != null && Check(ev.Attacker))
            {
                ev.Attacker.HumeShield += 10;
                ev.Player?.EnableEffect<Bleeding>(duration: 3);
            }
        }
        
        protected override void RoleAdded(Player player)
        {
            if (SteamUserids.Contains(player.UserId))
            {
                //player.ArtificialHealth = 100; // AHP 설정
                base.RoleAdded(player);
                _scp682Player = player;
                var scpPlayers = Player.List.Where(p => p.Role.Team == Team.SCPs && p != player).ToList();
        
                if (scpPlayers.Count >= 2)
                {
                    var randomScpPlayers = scpPlayers.OrderBy(x => Random.value).Take(2).ToList();

                    // Convert the two SCPs to a human role (for example, ClassD)
                    foreach (var scp in randomScpPlayers)
                    {
                        RoleTypeId roleReference = RoleTypeIdData();
                        scp.Role.Set(roleReference, RoleSpawnFlags.UseSpawnpoint); // Change role to ClassD
                        scp.ShowHint("SCP 682가 있는 관계로 당신의 진영이 인간진영으로 변환되었습니다..!", 10); // Notify the player
                        scp.Broadcast(5,"<color=red>SCP 682</color>가 있는 관계로 당신의 진영이 인간진영으로 변환되었습니다..!"); // Notify the player
                        Cassie.MessageTranslated(
                            message: "Attention SCP 6 8 2 detected", 
                            isSubtitles: true,
                            translation: "Attention <color=red>SCP682</color> detected",
                            isNoisy: true
                        );
                    }
                }
                // 전체 플레이어에게 보드캐스트 메시지 전송
                Map.Broadcast(7, "신원미상의 존재가 시설내에 존재합니다."); // 7초 동안 메시지 표시
            }
            else
            {
                // 특정 유저가 아닐 경우 아무 작업도 하지 않음
                // 필요시 로그를 추가하거나 다른 동작을 추가할 수 있음
                Log.Info($"{player.Nickname}은(는) 지정된 유저가 아니므로 커스텀 롤이 적용되지 않았습니다.");
                player.ShowHint($"{player.Nickname}은(는) 지정된 유저가 아니므로 커스텀 롤이 적용되지 않았습니다.");
                RoleTypeId roleReference = RoleTypeIdData();
                Timing.CallDelayed(5, () => player.Role.Set(roleReference));
                Log.Debug($"Replace a role to {roleReference.ToString()}");
            }
        }
        
        private void OnToggleNoclip(TogglingNoClipEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                if (Check(ev.Player))
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("<color=red>쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..</color>",5);
                    }
                    else
                    {
                        ev.Player.EnableEffect<MovementBoost>(intensity: 40,duration:10);
                        Log.Info($"{ev.Player.Nickname} activated the Speedy");
                        CoroutineHandle handle = Timing.RunCoroutine(CooldownCorutine(ev.Player));
                        _altKeyCooldowns[ev.Player] = handle;
                    }
                }
            }
        }
    
        private IEnumerator<float> CooldownCorutine(Player player)
        {
            yield return Timing.WaitForSeconds(_altKeyCooldownDuration);

            _altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n능력을 사용할수 있습니다.",5);

        }    
        private RoleTypeId RoleTypeIdData()
        {
            RoleTypeId[] role =
            {
                RoleTypeId.ClassD,
                RoleTypeId.Scientist,
                RoleTypeId.FacilityGuard,
            };
            var newRole = role[Random.Range(0, role.Length)];
            return newRole;
        }

        private void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            // SCP-682가 살아있는지 확인
            bool isScp682Alive = _scp682Player != null && _scp682Player.IsAlive;

            // 팀이 NineTailedFox이고 SCP-682가 살아있을 때만 이벤트를 처리
            if (ev.NextKnownTeam == Faction.FoundationStaff && isScp682Alive)
            {
                // 커스텀 롤이 없는 플레이어 목록을 필터링
                var nonCustomRolePlayers = Player.List.Where(p => CustomRole.Get(p.UserId) == null && p.Role.Team != Team.SCPs).ToList();

                foreach (var player in nonCustomRolePlayers)
                {
                    // 50% 확률로 커스텀 롤 부여
                    if (Random.value < 0.5f)
                    {
                        var customRole = new Ksk();  
                        customRole.AddRole(player);

                        // 플레이어에게 알림
                        player.ShowHint("당신은 [KSK] 특수요원 롤을 받았습니다!", 10);
                    }
                }
            }
        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip += OnToggleNoclip;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnToggleNoclip;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            base.UnsubscribeEvents();
        }

        public override void RemoveRole(Player player)
        {
            base.RemoveRole(player);
        }
    }
}