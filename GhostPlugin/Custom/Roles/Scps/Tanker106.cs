using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp106;
using GhostPlugin.API;
using MEC;
using PlayerRoles;
using RemoteAdmin;

namespace GhostPlugin.Custom.Roles.Scps
{
    [CustomRole(RoleTypeId.Scp106)]
    public class Tanker106 : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 8;
        public override int MaxHealth { get; set; } = 1000;
        public override string Name { get; set; } = "<color=#ff3636>SCP 106 Tanker</color>";
        public override string Description { get; set; } = "구버전 106 입니다.\n스토킹이 불가능한 대신, 데미지 75% 저항이 있습니다!\n`.g` 명령어를 입력하여 일시적으로 무적이 될수 있습니다! ";
        public override string CustomInfo { get; set; } = "SCP 106 Tanker";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp106;
        public StartTeam StartTeam { get; set; } = StartTeam.Scp;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        private readonly float _altKeyCooldownDuration = 80f;

        
        public int Chance { get; set; } = 50;
        private void OnStalking(StalkingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.IsAllowed = false;
                ev.Player.ShowHint("<color=red>구버전 106은 스토킹 기능이 작동되지 않습니다..!</color>");
            }
        }
        
        private void OnHurting(HurtingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.Amount *= 0.25f;
            }
        }
        
        protected override void RoleAdded(Player player)
        {
            Log.SendRaw($"{player.Nickname} is Chosen by old SCP-106",ConsoleColor.DarkCyan);
            //player.EnableEffect<DamageReduction>(duration: 70);
            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableAllEffects();
            base.RoleRemoved(player);
        }

        private void OnToggleNoClip(TogglingNoClipEventArgs ev)
        {
            if (!ev.IsAllowed)
            {
                if (Check(ev.Player))
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("<color=red>쿨다운이 아직 진행중입니다..\n은폐 능력을 사용할수 없습니다..</color>",5);
                    }
                    else
                    {
                        ev.Player.ShowHint("HS 100 회복",5);
                        ev.Player.HumeShield += 100;
                        CoroutineHandle handle = Timing.RunCoroutine(CooldownCoroutine(ev.Player));
                        _altKeyCooldowns[ev.Player] = handle;
                    }
                }
            }
        }
        private IEnumerator<float> CooldownCoroutine(Player player)
        {
            yield return Timing.WaitForSeconds(_altKeyCooldownDuration);

            _altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n능력을 사용할수 있습니다.",5);

        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip += OnToggleNoClip;
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Scp106.Stalking += OnStalking;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnToggleNoClip;
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Scp106.Stalking -= OnStalking;
            base.UnsubscribeEvents();
        }
    }
}