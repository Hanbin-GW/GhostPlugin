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
        public override string Name { get; set; } = "Spy Agent";

        public override string Description { get; set; } =
            "제단에 D계급으로 침투한 혼돈의 반란 스파이 요원입니다.\n본인이 카오스인 정체를 숨기시고, 제단은 몰락시키십시요!\n키카드를 떨어트려 외형을 변할수 있습니다!";

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardScientist.ToString(),
            ItemType.KeycardJanitor.ToString(),
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
                ev.Target.EnableEffect<Slowness>(duration: 2, intensity: 100);
                Timing.CallDelayed(2, () =>ev.Target.Kill("한순간의 여러 타박상과 칼에 찔려서 과다출혈로 사망하셧습니다"));
                ev.Target.Broadcast(5,"<color=red>당신은 반란요원한테 처형당했습니다.</color>");
            }

            if (Check(ev.Target))
            {
                ev.Target.ShowHint("너한테는 투항이라는 선택지는 없다...",5);
                ev.Target.Kill("당신은 MTF 탈출이 불가능합니다...\nYou cannot Escape to MTF");
            }
        }

        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem += OnDroppedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.DroppedItem -= OnDroppedItem;
            base.UnsubscribeEvents();
        }
    }
}