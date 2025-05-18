using System;
using System.IO;
using System.Linq;
using Discord;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using GhostPlugin.Methods.Legacy;
using GhostPlugin.Methods.Music;
using MEC;
using PlayerRoles;

namespace GhostPlugin.EventHandlers
{
    public class MusicEventHandlers
    {
        private readonly Plugin _plugin;
        public MusicEventHandlers(Plugin plugin) => this._plugin = plugin;
        public static MusicManager MusicManager = new MusicManager();
        public static AudioManagemanet AudioManagemanet = new AudioManagemanet();
        /// <summary>
        /// evnet regsister
        /// </summary>
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnedTeam;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnAnnouncingDecontemination;
        }
        /// <summary>
        /// event unregsister
        /// </summary>
        public static void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnedTeam;
            Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnAnnouncingDecontemination;
        }
        /// <summary>
        /// During the round is not started, the music is playing...
        /// </summary>
        /*public static void OnWaitingPlayers()
        { 
            MusicManager.EnsureMusicDirectoryExists();
            var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.LobbySongPath);
            AudioClipStorage.LoadClip(path,"lobby_music");

            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("Lobby",onIntialCreation: (p) =>
            {
                p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
            });

            globalPlayer.AddClip("lobby_music", volume: Plugin.Instance.Config.MusicConfig.Volume, loop: Plugin.Instance.Config.MusicConfig.Loop, destroyOnEnd: false);
            Log.Info("main song playing");
        }*/
        public static void OnWaitingPlayers()
        { 
            MusicManager.EnsureMusicDirectoryExists();
            var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.LobbySongPath);
            AudioClipStorage.LoadClip(path, "lobby_music");

            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("Lobby", condition: (hub) =>
            {
                var plr = Player.Get(hub);
                return !Plugin.Instance.musicDisabledPlayers.TryGetValue(plr.Id, out bool disabled) || !disabled;
            });

            // AudioPlayer가 완전히 준비된 이후에 스피커 추가
            if (globalPlayer != null)
            {
                globalPlayer.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);

                globalPlayer.AddClip("lobby_music",
                    volume: Plugin.Instance.Config.MusicConfig.Volume,
                    loop: Plugin.Instance.Config.MusicConfig.Loop,
                    destroyOnEnd: false);

                Log.Info("main song playing");
            }
            else
            {
                Log.Error("Failed to spawn the globalPlayer!");
            }
        }
        
        /// <summary>
        /// when the round is started, the music is stopped.
        /// </summary>
        public static void OnRoundStarted()
        {
            if (!AudioPlayer.TryGet("Lobby", out AudioPlayer lobbyPlayer))
                return;
            lobbyPlayer.ClipsById.Clear();
        }
        /// <summary>
        /// Play a music when the Military forces are spawned.
        /// </summary>
        /// <param name="ev">RespawningTeamEventArgs</param>
        public static void OnRespawnedTeam(RespawningTeamEventArgs ev)
        {
            if(ev.Wave.IsMiniWave)
                return;
            if (ev.Wave.Faction == Faction.FoundationStaff)
            {
                //MusicManager.StopMusic();
                string filePath = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.RespawnMtfBgm);
                if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.IsSafeMode)
                {
                    Log.Send("The SafeMode Is Enabled! Not using Methods!", LogLevel.Info, ConsoleColor.Green);
                    AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("GlobalAudioPlayer",onIntialCreation: (p) =>
                    {
                        p.AddSpeaker("Main", isSpatial: false, volume: Plugin.Instance.Config.MusicConfig.Volume ,maxDistance: 5000f);
                    });
                    Timing.CallDelayed(50f, () => globalPlayer.ClipsById.Clear());

                }
                else
                {
                    MusicManager.PlaySpecificMusic(filePath);
                    Timing.CallDelayed(50f, () => MusicManager.StopMusic());
                }
            }

            if (ev.Wave.Faction == Faction.FoundationEnemy)
            {
                //MusicManager.StopMusic();
                string filePath = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.ChaosSpawmBgm);
                MusicManager.PlaySpecificMusic(filePath);
                Timing.CallDelayed(50f, () => MusicManager.StopMusic());
            }
        }
        
        /// <summary>
        /// each situation, the music was playing
        /// </summary>
        /// <param name="ev"></param>
        public static void OnAnnouncingDecontemination(AnnouncingDecontaminationEventArgs ev)
        {
            if (Plugin.Instance.Config.MusicConfig.EnableSpecialEvent)
            {
                switch (ev.Id)
                {
                    case 0:
                    {
                        var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.Lcz15min);
                        AudioClipStorage.LoadClip(path, "Lcz15");

                        AudioPlayer musicPlayer = AudioPlayer.CreateOrGet("LczAudioPlayer", condition: (hub) =>
                            {
                                return Player.List.Any(player => player != null && player.CurrentRoom != null && player.CurrentRoom.Zone == ZoneType.LightContainment);
                            }
                            , onIntialCreation: (p) =>
                            {
                                Speaker speaker = p.AddSpeaker("Lcz_Music", isSpatial: false, maxDistance: 5000f);
                            }
                        );
                        musicPlayer.AddClip("Lcz15", 1f, false, true);
                        Timing.CallDelayed(60f, () =>
                        {
                            if(!AudioPlayer.AudioPlayerByName.TryGetValue("LczAudioPlayer",out AudioPlayer ap))
                                return;
                            ap.ClipsById.Clear();
                        });
                        break;
                    }
                    case 1:
                    {
                        var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.Lcz10min);
                        AudioClipStorage.LoadClip(path, "lcz10");
                        AudioPlayer lczPlayer = AudioPlayer.CreateOrGet("LczAudioPlayer",
                            condition: (hub) =>
                            {
                                return Player.List.Any(player => player != null && player.CurrentRoom != null && player.CurrentRoom.Zone == ZoneType.LightContainment);
                            }, onIntialCreation: p =>
                            {
                                Speaker speaker = p.AddSpeaker("Lcz_Music", isSpatial: true, volume: 1f, maxDistance: 5000f);
                            }
                        );
                        lczPlayer.AddClip("lcz10");
                        Timing.CallDelayed(60f, () =>
                        {
                            if(!AudioPlayer.AudioPlayerByName.TryGetValue("LczAudioPlayer",out AudioPlayer ap))
                                return;
                            ap.ClipsById.Clear();
                        });
                        break;
                    }
                    case 2:
                    {
                        
                        var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.Lcz5min);
                        AudioClipStorage.LoadClip(path, "lcz5");
                        AudioPlayer lczPlayer = AudioPlayer.CreateOrGet("LczAudioPlayer",
                            condition: (hub) =>
                            {
                                return Player.List.Any(player => player != null && player.CurrentRoom != null && player.CurrentRoom.Zone == ZoneType.LightContainment);
                            }, onIntialCreation: p =>
                            {
                                Speaker speaker = p.AddSpeaker("Lcz_Music", isSpatial: true, volume: 1f, maxDistance: 5000f);
                            }
                        );
                        lczPlayer.AddClip("lcz5", 1f, false, true);
                        Timing.CallDelayed(60f, () =>
                        {
                            if(!AudioPlayer.AudioPlayerByName.TryGetValue("LczAudioPlayer",out AudioPlayer ap))
                                return;
                            ap.ClipsById.Clear();
                        });
                        break;
                    }
                    case 3:
                    {
                        var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.Lcz1min);
                        AudioClipStorage.LoadClip(path, "Lcz1");
                        AudioPlayer lczPlayer = AudioPlayer.CreateOrGet("LczAudioPlayer", condition: (hub) =>
                            {
                                return Player.List.Any(player => player != null && player.CurrentRoom != null && player.CurrentRoom.Zone == ZoneType.LightContainment);
                            }
                            , onIntialCreation: (p) =>
                            {
                                Speaker speaker = p.AddSpeaker("Lcz_Music", isSpatial: true, maxDistance: 5000f);
                            }
                        );
                        lczPlayer.AddClip("Lcz1", 1f, false, true);
                        Timing.CallDelayed(60f, () =>
                        {
                            if(!AudioPlayer.AudioPlayerByName.TryGetValue("LczAudioPlayer",out AudioPlayer ap))
                                return;
                            ap.ClipsById.Clear();
                        });
                        break;
                    }
                    case 4:
                    {
                        foreach (Player player in Player.List)
                        {
                            if (player.CurrentRoom.Zone == ZoneType.LightContainment)  // 특정 구역에 있는지 확인
                            {
                                string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");
                                string filePath = Path.Combine(directory, Plugin.Instance.Config.MusicConfig.Lcz30sec);
                                AudioManagemanet.PlaySpecificMusic(filePath);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
