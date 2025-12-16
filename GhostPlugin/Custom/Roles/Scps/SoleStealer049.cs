using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp049;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using InventorySystem;
using InventorySystem.Items;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp049)]
    public class SoleStealer049 : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 10;
        public override int MaxHealth { get; set; } = 2000;
        public override string Name { get; set; } = "<color=#aa2bff>Scp049 The Soul Stealer</color>";
        public override string Description { get; set; } = "This is the wake-up version of SCP049.\nThere are various abilities, and use various SCP abilities to destroy facilities.";
        public override string CustomInfo { get; set; } = "The Soul Stealer";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp049;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp;
        public int Chance { get; set; } = 10;
        public override float SpawnChance { get; set; } = 0;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        public float AltKeyCooldownDuration = 90f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
        };

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new Speedy096(),
            new Explotion(),
            new SCP106(),
            new Scp457(),
        };

        public List<string> SteamUserIds { get; set; } = new List<string>
        {
            "76561199073020404@steam",
            "76561199161109510@steam",
            "76561199091279621@steam"
        };

        public List<string> BetaAccess { get; set; } = new List<string>
        {
            "76561199133709329@steam",
        };

        private void OnDying(DyingEventArgs ev)
        {
            SoleStealer049 soleStealer049 = Get(10)as SoleStealer049;
            if (ev.Player != null && ev.Attacker !=null && soleStealer049 != null && soleStealer049.Check(ev.Attacker))
            {
                ev.Attacker.Heal(50);
            }
        }
        private void OnAttacking(AttackingEventArgs ev)
        {
            if (ev.Target == null ||ev.Player == null || !Check(ev.Player))
            {
                return;
            }
            
            if (ev.Player.CurrentItem is Firearm)
            {
                return;
            }
            
            if (ev.Target != null && ev.Player != null && Check(ev.Player))
            {
                ev.Scp049.CallCooldown = 0.25f;
                ev.Target.Vaporize();
                //ev.Target.Kill("your soul is blow up on your body");
                ev.Target.Broadcast(5,"<color=red><size>당신은 SCP049 영혼 탈취자에게 제거되었습니다.</size></color>");
            }
        }
    
        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (!ev.IsAllowed)
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("쿨다운이 아직 진행중입니다..\n[Test] 능력을 사용할수 없습니다..", 5);
                        //ev.Player.Broadcast(message:"쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", duration:5, type: global::Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        ItemBase newItem = ev.Player.Inventory.ServerAddItem(ItemType.GunE11SR, ItemAddReason.AdminCommand);
                        ushort serial = newItem.ItemSerial;
                        ev.Player.Inventory.ServerSelectItem(serial);
                        //Timing.CallDelayed(10, () => ev.Player.Inventory.ServerSelectItem(serial2));
                        //쿨다운
                        CoroutineHandle handle = Timing.RunCoroutine(CooldownCorutine(ev.Player));
                        _altKeyCooldowns[ev.Player] = handle;
                    }
                }
            }
        }

    
        private IEnumerator<float> CooldownCorutine(Player player)
        {
            yield return Timing.WaitForSeconds(AltKeyCooldownDuration);

            _altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n총기를 사용할수 있습니다.",5);

        }    
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Scp049.Attacking += OnAttacking;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Scp049.Attacking -= OnAttacking;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            base.UnsubscribeEvents();
        }

        protected override void RoleAdded(Player player)
        {
            if (SteamUserIds.Contains(player.UserId))
            {
                base.RoleAdded(player);
                Exiled.API.Features.Cassie.MessageTranslated(".G4 .G4 Error detected PITCH_.2 evacuate immediately","<color=red>Error</color> <color=#ff2200>[Rejected]</color> detected evacuate immediately",false,true,true);
                player.Broadcast(15,$"<size=30>{player.Nickname} 님 고스트 서버에 <color=gold>후원</color>해주셔서 감사드립니다!\n지금 걸린 진영은 다양한 능력을 가지고 있는 <color=red>특수직업</color> 입니다!</size>");
                // 전체 플레이어에게 보드캐스트 메시지 전송
                //Map.Broadcast(7, "신원미상의 존재가 시설내에 존재합니다."); // 7초 동안 메시지 표시
                CoroutineHandle handle = Timing.RunCoroutine(CooldownCorutine(player));
                _altKeyCooldowns[player] = handle;
            }
            else if (BetaAccess.Contains(player.UserId))
            {
                player.Broadcast(10,"<color=green>베타 태스터의 권한으로 슈퍼 직업이 제공되었습니다</color>");
                base.RoleAdded(player);
            }
            else
            {
                // 특정 유저가 아닐 경우 아무 작업도 하지 않음
                // 필요시 로그를 추가하거나 다른 동작을 추가할 수 있음
                RoleTypeId roleReference = RoleTypeIdData();
                player.Kill($"{player.Nickname}은(는) 지정된 유저가 아니므로 커스텀 롤이 적용되지 않았습니다.");
                Log.Info($"{player.Nickname}은(는) 지정된 유저가 아니므로 커스텀 롤이 적용되지 않았습니다.");
                player.ShowHint($"{player.Nickname}은(는) 지정된 유저가 아니므로 커스텀 롤이 적용되지 않았습니다.");
                Timing.CallDelayed(5, () => player.Role.Set(roleReference));
                Log.Warn($"Replace a role to {roleReference.ToString()}");
            }
        }
        
        private RoleTypeId RoleTypeIdData()
        {
            RoleTypeId[] role =
            {
                RoleTypeId.ClassD,
                RoleTypeId.Scientist,
                RoleTypeId.FacilityGuard,
                RoleTypeId.ChaosRepressor,
                RoleTypeId.Scp096,
                RoleTypeId.Scp106,
                RoleTypeId.Scp173
            };
            var newRole = role[Random.Range(0, role.Length)];
            return newRole;
        }
    }
}