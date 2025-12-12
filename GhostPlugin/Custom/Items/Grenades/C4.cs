using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using GhostPlugin.Custom.Items.MonoBehavior;
using InventorySystem.Items.ThrowableProjectiles;
using JetBrains.Annotations;
using UnityEngine;
using YamlDotNet.Serialization;
using PlayerEvent = Exiled.Events.Handlers.Player;
using System.Globalization;
using GhostPlugin.API;
using GhostPlugin.API.CustomItem;

namespace GhostPlugin.Custom.Items.Grenades
{
    [CustomItem(ItemType.GrenadeHE)]
    public class C4 : CustomGrenade, ICustomItemGlow
    {
        public enum C4RemoveMethod
        {
            Remove = 0,
            Detonate = 1,
            Drop = 2,
        }
        public static C4 Instance { get; private set; } = null!;
        public static Dictionary<Pickup, Player> PlacedCharges { get; } = new();
        public override uint Id { get; set; } = 34;
        public override string Name { get; set; } = "<color=#FF0000>C4</color>";
        public override float Weight { get; set; } = 0.75f;
        [CanBeNull]
        public override SpawnProperties SpawnProperties { get; set; } = new()
        {
            Limit = 5,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new()
                {
                    Chance = 10,
                    Location = SpawnLocationType.InsideLczArmory,
                },

                new()
                {
                    Chance = 25,
                    Location = SpawnLocationType.InsideHczArmory,
                },

                new()
                {
                    Chance = 50,
                    Location = SpawnLocationType.Inside049Armory,
                },

                new()
                {
                    Chance = 100,
                    Location = SpawnLocationType.InsideSurfaceNuke,
                },
            },
        };
        public override string Description { get; set; } = "Remote blast (using radio) (requires server keybind setting)";
        
        [Description("Should C4 require a specific item to be detonated.")]
        public bool RequireDetonator { get; set; } = true;
        
        [Description("The Detonator Item that will be used to detonate C4 Charges")]
        public ItemType DetonatorItem { get; set; } = ItemType.Radio;
        
        [Description(
            "What happens with C4 charges placed by player, when he dies/leaves the game. (Remove / Detonate / Drop)")]
        public C4RemoveMethod MethodOnDeath { get; set; } = C4RemoveMethod.Drop;
        [Description("Should shooting at C4 charges do something.")]
        public bool AllowShoot { get; set; } = true;
        
        [Description("What happens with C4 charges after they are shot. (Remove / Detonate / Drop)")]
        public C4RemoveMethod ShotMethod { get; set; } = C4RemoveMethod.Remove;
        
        [Description("Maximum distance between C4 Charge and player to detonate.")]
        public float MaxDistance { get; set; } = 100f;
        
        [Description("Time after which the C4 charge will automatically detonate.")]
        public override float FuseTime { get; set; } = 9999f;
        
        [YamlIgnore]
        public override bool ExplodeOnCollision { get; set; } = false;
        [YamlIgnore]
        public override ItemType Type { get; set; } = ItemType.GrenadeHE;
        public bool Sticky { get; set; } = true;
        public void C4Handler(Pickup charge, C4RemoveMethod removeMethod = C4RemoveMethod.Detonate)
        {
            if (charge == null || charge.Position == null)
                return;
            switch (removeMethod)
            {
                case C4RemoveMethod.Remove:
                {
                    break;
                }

                case C4RemoveMethod.Detonate:
                {
                    ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(Type);
                    grenade.FuseTime = 0.1f;
                    grenade.SpawnActive(charge.Position);
                    break;
                }

                case C4RemoveMethod.Drop:
                {
                    TrySpawn(Id, charge.Position, out _);
                    break;
                }
            }

            PlacedCharges.Remove(charge);
            charge.Destroy();
        }
        
        protected override void SubscribeEvents()
        {
            Instance = this;

            StickyGrenadeApi.RegisterStickyGrenade("C4", pickup => Sticky && PlacedCharges.ContainsKey(pickup));
            
            PlayerEvent.Destroying += OnDestroying;
            PlayerEvent.Died += OnDied;
            PlayerEvent.Shooting += OnShooting;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;

            base.SubscribeEvents();
        }
        
        protected override void UnsubscribeEvents()
        {
            StickyGrenadeApi.UnregisterStickyGrenade("C4");
            PlayerEvent.Destroying -= OnDestroying;
            PlayerEvent.Died -= OnDied;
            PlayerEvent.Shooting -= OnShooting;

            base.UnsubscribeEvents();
        }

        protected override void OnWaitingForPlayers()
        {
            PlacedCharges.Clear();

            base.OnWaitingForPlayers();
        }

        protected override void OnThrownProjectile(ThrownProjectileEventArgs ev)
        {
            if (!PlacedCharges.ContainsKey(ev.Projectile))
            {
                PlacedCharges.Add(ev.Projectile, ev.Player);
                //ev.Projectile.GameObject.AddComponent<StickyGrenadeBehavior>();
            }
            base.OnThrownProjectile(ev);
        }

        protected override void OnExploding(ExplodingGrenadeEventArgs ev)
        {
            PlacedCharges.Remove(Pickup.Get(ev.Projectile.Base));
        }

        private void OnDestroying(DestroyingEventArgs ev)
        {
            foreach (var charge in PlacedCharges.ToList())
            {
                if (charge.Value == ev.Player)
                {
                    C4Handler(charge.Key, C4RemoveMethod.Remove);
                }
            }
        }

        private void OnDied(DiedEventArgs ev)
        {
            if (ev.Player == null)
                return;
            
            foreach (var charge in PlacedCharges.ToList())
            {
                if (charge.Value == ev.Player)
                {
                    C4Handler(charge.Key, MethodOnDeath);
                }
            }
        }

        private void OnShooting(ShootingEventArgs ev)
        {
            if (!AllowShoot)
                return;

            Vector3 forward = ev.Player.CameraTransform.forward;
            if (Physics.Raycast(ev.Player.CameraTransform.position + forward, forward, out var hit, 500))
            {
                EffectGrenade grenade = hit.collider.gameObject.GetComponentInParent<EffectGrenade>();
                if (grenade == null)
                {
                    return;
                }

                if (PlacedCharges.ContainsKey(Pickup.Get(grenade)))
                {
                    C4Handler(Pickup.Get(grenade), ShotMethod);
                }
            }
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            PlacedCharges.Clear();
        }

        protected override void OnAcquired(Player player, Item item, bool displayMessage)
        {
            base.OnAcquired(player, item, displayMessage);
        }

        public bool HasCustomItemGlow { get; set; } = true;
        public Color CustomItemGlowColor { get; set; } = new Color(235f / 255f, 125f / 255f, 52f / 255f) * 3f;
    }
}