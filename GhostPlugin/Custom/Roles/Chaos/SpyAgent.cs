using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using MEC;
using PlayerRoles;
using YamlDotNet.Serialization;

namespace GhostPlugin.Custom.Roles.Chaos
{
    public class SpyAgent : CustomRole, ICustomRole
    {
        public override uint Id { get; set; } = 17;
        public override int MaxHealth { get; set; } = 100;
        public override string Name { get; set; } = "<color=green>Sleeper Agent</color>";
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public override string Description { get; set; } =
            "제단에 D계급으로 침투한 혼돈의 반란 스파이 요원입니다.\n당신의 정체를 숨기시고, 제단은 몰락시키십시요!\n키카드를 떨어트려 외형을 변할수 있습니다!\n시채 운반을 하여 살인을 은폐하십시요!\n\n<size=15>적을 무장해제시 사살하게됩니다</size>";

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardScientist.ToString(),
            ItemType.KeycardJanitor.ToString(),
            ItemType.DebugRagdollMover.ToString()
        }; 
        public override string CustomInfo
        {
            get => RoleTypeId.ClassD.GetFullName();
            set { }
        }
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosConscript;
        public StartTeam StartTeam { get; set; } = StartTeam.ClassD;
        public int Chance { get; set; } = 60;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 1,
            RoleSpawnPoints = new List<RoleSpawnPoint>()
            {
                new RoleSpawnPoint()
                {
                    Role = RoleTypeId.ClassD,
                    Chance = 100,
                }
            }
        };

        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                player.ChangeAppearance(RoleTypeId.ClassD, true);
            });

            foreach (Player Targetplayer in Player.List)
            {
                if (Targetplayer.Role == RoleTypeId.FacilityGuard)
                {
                    Targetplayer.Broadcast(5,"<size=30>저위험군에 <color=red>카오스 정보요원</color>이 있다 뒤를 조심하도록!</size>");
                }
            }
            base.RoleAdded(player);
        }

        private void OnDroppedItem(DroppedItemEventArgs ev)
        {
            if (Check(ev.Player))
            {
                switch (ev.Pickup.Type)
                {
                    case ItemType.KeycardGuard:
                        ev.Player.ChangeAppearance(RoleTypeId.FacilityGuard,true);
                        break;
                    case ItemType.KeycardResearchCoordinator or ItemType.KeycardScientist:
                        ev.Player.ChangeAppearance(RoleTypeId.Scientist, true);
                        break;
                    case ItemType.KeycardChaosInsurgency:
                        ev.Player.ChangeAppearance(RoleTypeId.ChaosRifleman);
                        break;
                    case ItemType.KeycardMTFOperative:
                        ev.Player.ChangeAppearance(RoleTypeId.NtfPrivate);
                        break;
                    case ItemType.KeycardMTFCaptain:
                        ev.Player.ChangeAppearance(RoleTypeId.NtfCaptain);
                        break;
                }
            }
        }
        
        private void OnDisarm(HandcuffingEventArgs ev)
        {
            if (Check(ev.Player))
            {
                ev.Target.EnableEffect<Slowness>(duration: 1.5f, intensity: 80);
                Timing.CallDelayed(1.5f, () =>ev.Target.Kill("한순간의 여러 타박상과 칼에 찔려서 과다출혈로 사망하셧습니다"));
                ev.Target.Broadcast(5, "<color=red><size=32>당신은 반란요원한테 사살당했습니다.</size></color>");
                ev.Player.ChangeAppearance(ev.Target.Role,true);
            }

            if (Check(ev.Target))
            {
                ev.Target.ShowHint("<color=red><b>너한테는 투항이라는 선택지는 없다...</b></color>",10);
                ev.Target.Vaporize();
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Handcuffing += OnDisarm;
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.Handcuffing -= OnDisarm;
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            base.UnsubscribeEvents();
        }
    }
}