using System;
using System.Collections.Generic;
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
        private readonly MusicMethods _music;
        private readonly AudioManagemanet _audioMgmt = new AudioManagemanet();

        public MusicEventHandlers(Plugin plugin, string tmpAudio)
        {
            _plugin = plugin;
            _music = new MusicMethods(plugin.AudioDirectory, tmpAudio);
        }

        private static CoroutineHandle loopCoroutine;

        /// <summary>
        /// evnet regsister
        /// </summary>
        public static void RegisterEvents()
        {
            Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
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
            Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
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
            MusicMethods.EnsureMusicDirectoryExists();
            string[] playlist = Plugin.Instance.Config.MusicConfig.MusicPlayList;

            /*var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.LobbySongPath);
            AudioClipStorage.LoadClip(path, "lobby_music");*/

            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("Lobby", condition: (hub) =>
            {
                var plr = Player.Get(hub);
                return !Plugin.Instance.musicDisabledPlayers.TryGetValue(plr.Id, out bool disabled) || !disabled;
            });

            if (globalPlayer != null)
            {
                globalPlayer.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);

                /*globalPlayer.AddClip("lobby_music",
                    volume: Plugin.Instance.Config.MusicConfig.Volume,
                    loop: Plugin.Instance.Config.MusicConfig.Loop,
                    destroyOnEnd: false);*/
                loopCoroutine = Timing.RunCoroutine(LoopPlaylist(globalPlayer, playlist));
                
                Log.Info("main song playing");
            }
            else
            {
                Log.Error("Failed to spawn globalPlayer");
            }
        }
        public static void OnDetonated()
        {
            var path = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.WarheadBGMPath);
            Log.Info($"Warhead BGM Path: {path}");

            if (!File.Exists(path))
            {
                Log.Error("AudioFile does not exist.");
                return;
            }

            AudioClipStorage.LoadClip(path, "Doom");

            if (!AudioClipStorage.AudioClips.ContainsKey("Doom"))
            {
                Log.Error("Filed to load AudioClip");
                return;
            }
            MusicMethods.PlaySpecificMusic(path);

            Timing.CallDelayed(24f, () =>
            {
                MusicMethods.StopMusic();
            });
        }

        /// <summary>
        /// when the round is started, the music is stopped.
        /// </summary>
        public static void OnRoundStarted()
        {
            if (loopCoroutine.IsRunning)
                Timing.KillCoroutines(loopCoroutine);
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
                    Log.Send("The SafeMode Is Enabled! Not using CustomRoleMethods!", LogLevel.Info, ConsoleColor.Green);
                    AudioPlayer globalPlayer = AudioPlayer.CreateOrGet("GlobalAudioPlayer",onIntialCreation: (p) =>
                    {
                        p.AddSpeaker("Main", isSpatial: false, volume: Plugin.Instance.Config.MusicConfig.Volume ,maxDistance: 5000f);
                    });
                    Timing.CallDelayed(50f, () => globalPlayer.ClipsById.Clear());

                }
                else
                {
                    MusicMethods.PlaySpecificMusic(filePath);
                    Timing.CallDelayed(50f, () => MusicMethods.StopMusic());
                }
            }

            if (ev.Wave.Faction == Faction.FoundationEnemy)
            {
                //MusicManager.StopMusic();
                string filePath = Path.Combine(Plugin.Instance.AudioDirectory, Plugin.Instance.Config.MusicConfig.ChaosSpawmBgm);
                MusicMethods.PlaySpecificMusic(filePath);
                Timing.CallDelayed(50f, () => MusicMethods.StopMusic());
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
                            if (player.CurrentRoom.Zone == ZoneType.LightContainment) 
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
        public static IEnumerator<float> LoopPlaylist(AudioPlayer player, string[] playlist)
        {
            while (true)
            {
                foreach (string fileName in playlist)
                {
                    string path = Path.Combine(Plugin.Instance.AudioDirectory, fileName);
                    string clipId = Path.GetFileNameWithoutExtension(fileName);

                    AudioClipStorage.LoadClip(path, clipId);
                    player.AddClip(clipId, volume: Plugin.Instance.Config.MusicConfig.Volume, loop: false, destroyOnEnd: true);
                    float duration = API.Audio.AudioUtils.GetOggDurationInSeconds(path);
                    if (Plugin.Instance.Config.MusicConfig.Debug)
                    {
                        Log.Send($"Playing : {clipId}", LogLevel.Debug, ConsoleColor.DarkGreen);
                        Log.Send($"{duration} secound", LogLevel.Debug, ConsoleColor.DarkGreen);
                    }

                    yield return Timing.WaitForSeconds(duration);
                    player.ClipsById.Clear();
                }
            }
        }
    }
}