using System.IO;
using System.Threading.Tasks;
using GhostPlugin.MusicConfigs;
using GhostPlugin.Methods.Music.Module;

namespace GhostPlugin.Commands.MusicCommand
{
    public class AudioCommands
    {
        private readonly MusicPlayModule _svc;

        public AudioCommands(string audioDir, string workDir)
        {
            _svc = new MusicPlayModule(new MusicPlaybackModule
            {
                YtDlpPath  = "yt-dlp",
                FfmpegPath = "ffmpeg",
                WorkDir    = "/home/vscode/steamcmd/scpsl/tmp-audio",
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
        /// 유튜브 URL을 받아 OGG 변환 후 AudioPlayer에 등록하고,
        /// IAudioFile 객체를 리턴합니다. (실패시 null)
        /// </summary>
        public async Task<string?> PrepareClipFromYouTubeAsync(string youtubeUrl, string? clipAlias = null)
        {
            // 1) 변환 → Audio/xxx.ogg 상대경로
            var relPath = await _svc.DownloadAndConvertAsync(youtubeUrl);
            if (string.IsNullOrEmpty(relPath))
                return null;

            // 2) AudioClipStorage.LoadClip은 '절대경로'가 안전합니다(README 예시).
            //    서버 루트를 아신다면 절대경로로 합쳐주세요.
            //    아래는 relPath가 "Audio/xxx.ogg" 형태라고 가정.
            var absPath = Path.Combine("/home/vscode/steamcmd/scpsl", relPath);

            // 3) 별칭(clipAlias) 정하기
            var alias = clipAlias ?? Path.GetFileNameWithoutExtension(absPath);

            // 4) 로드(사전 로딩). 이후 AddClip에서 alias로 참조.
            AudioClipStorage.LoadClip(absPath, alias);

            // 5) 호출자에게 alias만 반환 (호출자가 AddClip/재생 시점 제어)
            return alias;
        }
        public async Task<string?> PrepareFileFromYouTubeAsync(string youtubeUrl)
        {
            // "Audio/xxx.ogg" 형태의 상대 경로를 받음
            var relPath = await _svc.DownloadAndConvertAsync(youtubeUrl);
            if (string.IsNullOrEmpty(relPath))
                return null;

            // 로드는 하지 않고 파일명만 리턴 → PlaySpecificMusic에서 로드+재생
            return Path.GetFileName(relPath); // "xxx.ogg"
        }
    }
}