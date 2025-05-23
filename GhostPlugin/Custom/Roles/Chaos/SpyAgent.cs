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
            "Chaotic Rebellion spy who infiltrated the altar as a Class D.\nHide your identity, and bring down the altar!\nYou can change the appearance by dropping the key card!";

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

            foreach (Player Targetplayer in Player.List)
            {
                if (Targetplayer.Role == RoleTypeId.FacilityGuard)
                {
                    Targetplayer.Broadcast(5,"<size=30><color=red>Chaos Intelligence Agent</color> in the LCZ. Watch your back!</size>");
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
                Timing.CallDelayed(1.5f, () =>ev.Target.Kill("He died of excessive bleeding from multiple bruises and stab wounds in an instant"));
                ev.Target.Broadcast(5, "<color=red>You were executed by Sleeper Agent.</color>");
                ev.Player.ChangeAppearance(ev.Target.Role,true);
            }

            if (Check(ev.Target))
            {
                ev.Target.ShowHint("<color=red><b>You don't have a choice of surrender...</b></color>",10);
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