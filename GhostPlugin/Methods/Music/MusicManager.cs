using System;
using System.IO;
using Exiled.API.Features;

namespace GhostPlugin.Methods.Music
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
                Log.Warn($"music folder is not existed create new one : {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("music folder already exists.");
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
                Log.Error($"file didn't exists: {path}");
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

                Log.Info($"playing music: {filename}");
            }
            catch (Exception ex)
            {
                // 예외 처리
                Log.Error($"the error occured during playing music: {ex.Message}");
            }
        }
    }
}
