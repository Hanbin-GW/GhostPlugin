using System;
using System.IO;
using Exiled.API.Features;

namespace GhostPlugin.Methods
{
    public class MusicManager
    {
        /// <summary>
        /// check a directory when the music file is exists.
        /// </summary>
        public void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");

            // 폴더가 없으면 생성
            if (!Directory.Exists(path))
            {
                Log.Warn($"음악 폴더가 존재하지 않습니다. 새로 생성합니다: {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("음악 폴더가 이미 존재합니다.");
            }
        }
        /// <summary>
        /// Stop a Music
        /// </summary>
        public void StopMusic()
        {
            if(!AudioPlayer.AudioPlayerByName.TryGetValue("GlobalAudioPlayer",out AudioPlayer ap))
                return;
            //ap.ClipsById.Clear();
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
                Log.Error($"파일이 존재하지 않습니다: {path}");
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

                Log.Info($"음악이 재생됩니다: {filename}");
            }
            catch (Exception ex)
            {
                // 예외 처리
                Log.Error($"음악 재생 명령어 중 오류가 발생했습니다: {ex.Message}");
            }
        }
    }
}