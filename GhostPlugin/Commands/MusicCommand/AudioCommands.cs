using System.IO;
using System.Threading.Tasks;
using GhostPlugin.MusicConfigs;
using GhostPlugin.Methods.Music.Downloder;

namespace GhostPlugin.Commands.MusicCommand
{
    public class AudioCommands
    {
        private readonly YouTubeAudioService _svc;

        public AudioCommands(string audioDir, string workDir)
        {
            _svc = new YouTubeAudioService(new AudioDownloadConfig
            {
                YtDlpPath  = "yt-dlp",
                FfmpegPath = "ffmpeg",
                WorkDir    = "/root/Steam/steamapps/common/SCP Secret Laboratory Dedicated Server/tmp-audio",
                AudioDir   = Plugin.Instance.AudioDirectory,
                SampleRate = 48000,
                Channels   = 1,
                VorbisBitrate = "160k",
                CookiesPath     = null,
                ForceIPv4       = true,
                CustomUserAgent = "com.google.android.youtube/19.12.4 (Linux; U; Android 13)",
                UseAndroidClient = true,
            });
        }

        /// <summary>
        /// Takes a YouTube URL, converts it to OGG, registers it in the AudioPlayer,
        /// and returns an IAudioFile alias. (Returns null if failed)
        /// </summary>
        public async Task<string?> PrepareClipFromYouTubeAsync(string youtubeUrl, string? clipAlias = null)
        {
            // 1) Download and convert → returns relative path like "Audio/xxx.ogg"
            var relPath = await _svc.DownloadAndConvertAsync(youtubeUrl);
            if (string.IsNullOrEmpty(relPath))
                return null;

            // 2) AudioClipStorage.LoadClip works safely with absolute paths.
            var absPath = Path.Combine("/root/Steam/steamapps/common/SCP Secret Laboratory Dedicated Server", relPath);

            // 3) Decide the alias (clipAlias); use the filename if clipAlias is not provided.
            var alias = clipAlias ?? Path.GetFileNameWithoutExtension(absPath);

            // 4) Preload the clip. Later you can refer to it by alias in AddClip.
            AudioClipStorage.LoadClip(absPath, alias);

            // 5) Return only the alias (the caller can handle AddClip/play timing)
            return alias;
        }

        public async Task<string?> PrepareFileFromYouTubeAsync(string youtubeUrl)
        {
            // Gets a relative path like "Audio/xxx.ogg"
            var relPath = await _svc.DownloadAndConvertAsync(youtubeUrl);
            if (string.IsNullOrEmpty(relPath))
                return null;

            // Does not load the file, only returns the filename → PlaySpecificMusic will load+play it
            return Path.GetFileName(relPath); // "xxx.ogg"
        }
    }
}
