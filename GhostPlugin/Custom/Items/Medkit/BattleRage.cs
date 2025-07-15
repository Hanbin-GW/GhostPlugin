using System.Collections.Generic;
using System.IO;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace GhostPlugin.Custom.Items.Etc
{
    [CustomItem(ItemType.Adrenaline)]
    public class BattleRage : CustomItem
    {
        public override uint Id { get; set; } = 14;
        public override string Name { get; set; } = "<color=#ff9500>Battle Rage</color>";
        public override string Description { get; set; } = "Temporary 30% increase in travel speed and unlimited stamina for 30 seconds.";
        public override float Weight { get; set; } = 2f;

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties()
        {
            Limit = 2,
            RoomSpawnPoints = new List<RoomSpawnPoint>()
            {
                new RoomSpawnPoint()
                {
                    Room = RoomType.HczNuke,
                    Chance = 100
                }
            }
        };
        public override ItemType Type { get; set; } = ItemType.Adrenaline;

        private void OnUsedItem(UsedItemEventArgs ev)
        {
            if (Check(ev.Item))
            {
                Plugin.Instance.EnsureMusicDirectoryExists();
                var path = Path.Combine(Plugin.Instance.EffectDirectory, "Battle_Rage.ogg");
                if (!File.Exists(path))
                {
                    Log.Error($"파일이 존재하지 않습니다: {path}");
                    return;
                }

                AudioClipStorage.LoadClip(path,"Battle_Rage_Sound");
                AudioPlayer effectPlayer = AudioPlayer.CreateOrGet("EffectAudioPlayer",condition: hub => (Check(ev.Item)), 
                    onIntialCreation: p =>
                    {
                        Speaker speaker = p.AddSpeaker("Lcz_Music", isSpatial: false, maxDistance: 5000f);
                    });
                effectPlayer.AddClip("Battle_Rage_Sound", 1f, false, true);
                ev.Player.ShowHint("<color=red>WRAAAGH!\nLET'S GO</color>",5);
                ev.Player.EnableEffect<MovementBoost>(30, 30);
                ev.Player.IsUsingStamina = false;
                Timing.CallDelayed(30,() => ev.Player.IsUsingStamina = true);
            }
            //Timing.CallDelayed(BoostTime, () => ev.Player.DisableEffect<CustomPlayerEffects.MovementBoost>());
        }

        
        protected override void SubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            base.SubscribeEvents();
        }

        protected override void UnsubscribeEvents()
        {
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            base.UnsubscribeEvents();
        }
    }
}