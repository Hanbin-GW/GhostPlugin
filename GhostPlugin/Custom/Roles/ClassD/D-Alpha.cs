using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.ClassD
{
    [CustomRole(RoleTypeId.ClassD)]
    public class D_Alpha : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 3;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=#ff3636>실험체 알파</color>";
        public override string Description { get; set; } = "제단의 인체실험을 당한 당신은 어떤 <color=#ebd234>특수한 능력</color>이 있습니다..!\n------------보유중인 능력------------\n<size=15>• Active Camo</size>\n--------------------------------";
        public override string CustomInfo { get; set; } = "Test Subject";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
        public StartTeam StartTeam { get; set; } = StartTeam.ClassD;
        public int Chance { get; set; } = 60;
        
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        private readonly int _altKeyCooldownDuration = 40;

        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>
        {
            new ActiveCamo()
            {
                Name = "ActiveCamo",
                Description = "몇초간 투명 상태가 됩니다!"
            }
        };
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
        };

        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (!ev.IsAllowed)
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", 5);
                        //ev.Player.Broadcast(message:"쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", duration:5, type: global::Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        ev.Player.ShowHint("투명 능력이 횔성화 되았습니다.", 5);
                        ev.Player.EnableEffect<Invisible>(intensity: 1, duration: 20);
                        //ev.Player.RandomTeleport(type: typeof(Room));
                        //쿨다운
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
            player.ShowHint("쿨다운이 끝났습니다.\n<color=blue>Super Boost</color> 를 사용할수 있습니다.",5);

        }
        protected override void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            base.UnsubscribeEvents();
        }
    }
}