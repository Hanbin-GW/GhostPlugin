using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using GhostPlugin.API;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Custom.Abilities.Passive;
using MEC;
using PlayerRoles;

namespace GhostPlugin.Custom.Roles.Chaos
{
    public class CiPhantom : CustomRole, ICustomRole
    {
        private readonly Dictionary<Player, CoroutineHandle> altKeyCooldowns = new Dictionary<Player, CoroutineHandle>();
        private readonly float _altKeyCooldownDuration = 60f;
        public override uint Id { get; set; } = 2;
        public override int MaxHealth { get; set; } = 110;
        public override string Name { get; set; } = "<color=#53db78>Phantom</color>";
        public override string Description { get; set; } = "Ghost Sniper Team of Chaos.";
        public override string CustomInfo { get; set; } = "Phantom";
        public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
        public override bool DisplayCustomItemMessages { get; set; } = false;
        public StartTeam StartTeam { get; set; } = StartTeam.Chaos;
        public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
        {
            new HealOnKill()
            {
                Name = "HealOnKill",
                Description = "Heals the player when they kill someone.",
            },
            new Focus()
            {
                Name = "Focus (집중)",
                Description = "Extrem 1853 effect!"
            },
            new Ghost()
            {
                Name = "Ghost",
                Description = "You'll became quiet for 20 seconds, it's through the door, and invisible!"
            }
        };
        public int Chance { get; set; } = 80;

        public override List<string> Inventory { get; set; } = new List<string>()
        {
            ItemType.KeycardChaosInsurgency.ToString(),
            ItemType.ArmorCombat.ToString(),
            ItemType.Medkit.ToString(),
            30.ToString(),
            17.ToString(),
            14.ToString(),
            12.ToString(),
        };
        
        private void OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (Check(ev.Player))
            {
                if (!ev.IsAllowed)
                {
                    if (altKeyCooldowns.ContainsKey(ev.Player))
                    {
                        ev.Player.ShowHint("<color=red>쿨다운이 아직 진행중입니다..\n유령 능력을 사용할수 없습니다..</color>",5);
                    }
                    else
                    {
                        ev.Player.ShowHint("유령 능력이 횔성화 되았습니다.",5);
                        ev.Player.EnableEffect<SilentWalk>(duration: 20);
                        ev.Player.EnableEffect<Ghostly>(duration: 20);
                        ev.Player.EnableEffect<Invisible>(duration: 5);
                        CoroutineHandle handle = Timing.RunCoroutine(CooldownCoroutine(ev.Player));
                        altKeyCooldowns[ev.Player] = handle;
                    }
                }
            }
        }
        
        private IEnumerator<float> CooldownCoroutine(Player player)
        {
            yield return Timing.WaitForSeconds(_altKeyCooldownDuration);

            altKeyCooldowns.Remove(player);
            player.ShowHint("쿨다운이 끝났습니다.\n능력을 사용할수 있습니다.",5);

        }

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
        {
            { AmmoType.Nato762, 80 }
        };

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties();
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