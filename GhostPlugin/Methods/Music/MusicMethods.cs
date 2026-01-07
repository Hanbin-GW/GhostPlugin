using System;
using System.IO;
using System.Threading.Tasks;
using Exiled.API.Features;
using GhostPlugin.Commands.MusicCommand;
using MEC;

namespace GhostPlugin.Methods.Music
{
    public class MusicMethods
    {
        /// <summary>
        /// check a directory when the music file is exists.
        /// </summary>
        public readonly AudioCommands audioCommands;
        public MusicMethods(string audioDir, string workDir) {
            Directory.CreateDirectory(audioDir);
            audioCommands = new AudioCommands(audioDir, workDir); // 생성자에서 주입
        }
        public static void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");

            // 폴더가 없으면 생성
            if (!Directory.Exists(path))
            {
                Log.Warn($"Music Folder Doesn't exists create new: {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("Music Folder Exists");
            }
        }
        /// <summary>
        /// Stop a Music
        /// </summary>
        public static void StopMusic()
        {
            if(!AudioPlayer.AudioPlayerByName.TryGetValue("GlobalAudioPlayer",out AudioPlayer ap))
                return;
            ap.ClipsById.Clear();
            ap.RemoveAllClips();
        }
        /// <summary>
        /// play a music
        /// </summary>
        /// <param name="filename">Insert a file name</param>
        public void PlaySpecificMusic(string filename)
        {
            var path = Path.Combine(Plugin.Instance.AudioDirectory, filename);

            // 파일 존재 여부 확인
            if (!File.Exists(path))
            {
                Log.Error($"File Doesn't exists: {path}");
                return;
            }

            try
            {
                // 오디오 클립 로드
                StopMusic();
                AudioClipStorage.LoadClip(path, "Music");

                // 오디오 플레이어 생성 또는 가져오기
                AudioPlayer musicPlayer = AudioPlayer.CreateOrGet("GlobalAudioPlayer", onIntialCreation: (p) =>
                {
                    p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000f);
                });

                // 클립 추가
                musicPlayer.AddClip("Music", 1f, false, false);

                Log.Info($"Playing a music: {filename}");
            }
            catch (Exception ex)
            {
                // 예외 처리
                Log.Error($"Error Occured playing music command: {ex.Message}");
            }
        }

        public static void PlaySoundEffect(string filename, Player player, float duration, float maxDistance)
        {
            var path = Path.Combine(Plugin.Instance.EffectDirectory, filename);
            if (!File.Exists(path))
            {
                Log.Error($"File don't exists: {path}");
                return;
            }

            try
            {
                var baseName = Path.GetFileNameWithoutExtension(filename);
                var clipName = $"{baseName}_{player.Id}_{Guid.NewGuid():N}";

                AudioClipStorage.LoadClip(path, clipName);

                var playerKey = $"Effect_{player.Id}";
                var audioPlayer = AudioPlayer.CreateOrGet(
                    playerKey,
                    condition: hub => hub.PlayerId == player.Id,
                    onIntialCreation: p =>
                    {
                        p.AddSpeaker("Main", isSpatial: true, maxDistance: maxDistance, position: player.Position);
                    });
                Log.Debug($"[Debug] Playing a ID: Effect_{player.Id}");
                audioPlayer.SetSpeakerPosition("Main", player.Position);
                audioPlayer.AddClip(clipName, 1f, false, false);

                // var destroyAfter = Math.Max(duration + 0.3f, 2.0f);
                Timing.CallDelayed(duration, () =>
                {
                    try { AudioClipStorage.DestroyClip(clipName); }
                    catch (Exception e) { Log.Warn($"DestroyClip failed for {clipName}: {e.Message}"); }
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error Occured playing effect command: {ex}");
            }
        }

        public static void PlaySoundPlayer(string filename, Player player, float duration)
        {
            var path = Path.Combine(Plugin.Instance.AudioDirectory, filename);
            if (!File.Exists(path))
            {
                Log.Error($"File Doesn't exists: {path}");
                return;
            }

            try
            {
                // 파일 이름(확장자 제거)을 클립 이름으로 사용
                var clipName = Path.GetFileNameWithoutExtension(filename);

                // 이미 로드된 클립이면 다시 로드 안 하도록 (선택)
                // bool loaded = AudioClipStorage.LoadClip(path, clipName);
                // if (!loaded) { Log.Error($"Failed to load clip: {clipName}"); return; }

                AudioClipStorage.LoadClip(path, clipName);

                AudioPlayer musicPlayer = AudioPlayer.CreateOrGet(
                    "KillstreakAudioPlayer",
                    condition: hub => hub.PlayerId == player.Id,
                    onIntialCreation: p =>
                    {
                        p.AddSpeaker("Main", isSpatial: false, maxDistance: 20f);
                    });

                // 여기서도 "Music"이 아니라 clipName 사용
                musicPlayer.AddClip(clipName, 1f, false, false);

                // 일정 시간 뒤에 언로드 하고 싶으면
                Timing.CallDelayed(duration, () =>
                {
                    try
                    {
                        AudioClipStorage.DestroyClip(clipName);
                    }
                    catch
                    {
                        // 이미 언로드 됐으면 무시
                    }
                });

                Log.Info($"Playing a music: {filename}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error Occured playing music command: {ex.Message}");
            }
        }

        
        public async Task PlayPreparedAlias(string alias)
        {
            try
            {
                //StopMusic();
                var fileName = await audioCommands.PrepareClipFromYouTubeAsync(alias);
                if (fileName != null)
                {
                    PlaySpecificMusic(fileName); // 밑의 함수 호출
                }
                Log.Info($"Music is playing (alias): {alias}");
            }
            catch (Exception ex)
            {
                Log.Error($"Error Occured playing music command: {ex.Message}");
            }
        }

    }
}