using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Other
{
    [CustomRole(RoleTypeId.Tutorial)]
    public class SecretAgent : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 12;
        public override int MaxHealth { get; set; } = 200;
        public override string Name { get; set; } = "SecretAgent";
        public override string Description { get; set; } = "";
        public override string CustomInfo { get; set; } = "Secret Agent";

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>()
            {
                new RoleSpawnPoint()
                {
                    Role = RoleTypeId.FacilityGuard,
                    Chance = 100,
                }
            }
        };
        public StartTeam StartTeam { get; set; } = StartTeam.Guard;
        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
        public int Chance { get; set; } = 0;
        private readonly int _altKeyCooldownDuration = 30;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardGuard.ToString(),
            ItemType.ArmorCombat.ToString(),
        };
        private void OnDroppingKeycard(DroppingItemEventArgs ev)
        {
            if(!Check(ev.Player))
                return;
            if (ev.Item.Type == ItemType.KeycardGuard)
                ev.Player.ChangeAppearance(RoleTypeId.FacilityGuard,true);
            if(ev.Item.Type == ItemType.KeycardScientist || ev.Item.Type == ItemType.KeycardResearchCoordinator)
                ev.Player.ChangeAppearance(RoleTypeId.Scientist,true);
            if(ev.Item.Type == ItemType.KeycardChaosInsurgency)
                ev.Player.ChangeAppearance(RoleTypeId.ChaosRepressor,true);
        }

        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (!ev.IsAllowed)
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("쿨다운이 아직 진행중입니다..\n[랜덤 텔레포트] 능력을 사용할수 없습니다..", 5);
                        //ev.Player.Broadcast(message:"쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", duration:5, type: global::Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        ev.Player.ShowHint("랜덤 텔레포트 능력이 횔성화 되았습니다.", 5);
                        ev.Player.RandomTeleport(typeof(Door));
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
            player.ShowHint("쿨다운이 끝났습니다.\n<color=blue>향상된 비전 고글</color> 를 사용할수 있습니다.",5);

        }
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
            Exiled.Events.Handlers.Player.DroppingItem += OnDroppingKeycard;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;
            Exiled.Events.Handlers.Player.DroppingItem -= OnDroppingKeycard;
            base.UnsubscribeEvents();
        }
    }
}