using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using Player = Exiled.API.Features.Player;

namespace GhostPlugin.Custom.Roles.Foundation
{
    [CustomRole(RoleTypeId.NtfSergeant)]
    public class Jailbirdman : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 6;
        public override int MaxHealth { get; set; } = 110;
        public override string Name { get; set; } = "<color=#0096FF>Jailbird Man</color>";
        public override string Description { get; set; } = "Agent Bethalang, who is very good at handling Jailbird well.";
        public override string CustomInfo { get; set; } = "Jailbird Man";
        public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Ntf;
        private readonly Dictionary<Player, CoroutineHandle> _altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        private readonly int _altKeyCooldownDuration = 40;
        public int Chance { get; set; } = 100;
    
        public override List<string> Inventory { get; set; } = new()
        {
            ItemType.KeycardMTFOperative.ToString(),
            38.ToString(),
            ItemType.GunCOM15.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.SCP500.ToString(),
            ItemType.Medkit.ToString(),
            ItemType.Radio.ToString(),
        };
    
        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
        {
            {
                AmmoType.Nato9, 36
            },
        };
        
        private void OnTogglingNoclip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (!ev.IsAllowed)
                {
                    if (_altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("<color=red>쿨다운이 아직 진행중입니다..</color>\n<color=blue>옵니 무브먼트</color> 사용할수 없습니다..", 5);
                        //ev.Player.Broadcast(message:"쿨다운이 아직 진행중입니다..\n능력을 사용할수 없습니다..", duration:5, type: global::Broadcast.BroadcastFlags.Normal);
                    }
                    else
                    {
                        ev.Player.ShowHint("<color=blue>옵니 무브먼트</color> 가 활성화 되었습니다!");
                        ev.Player.Health += 20;
                        //ev.Player.EnableEffect<Scp207>(intensity: 4, duration: 5);
                        ev.Player.EnableEffect<MovementBoost>(intensity: 40, duration: 5);
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

        protected override void RoleAdded(Player player)
        {
            //player.EnableEffect<MovementBoost>(intensity: 10);
            base.RoleAdded(player);
        }
        protected override void RoleRemoved(Player player)
        {
            //player.DisableEffect<MovementBoost>();
            base.RoleRemoved(player);
        }
        
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>
            {
                new()
                {
                    Role = RoleTypeId.NtfSpecialist,
                    Chance = 100,
                },
            },
        };

        protected override void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoclip;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoclip;
            base.UnsubscribeEvents();
        }
    }
}