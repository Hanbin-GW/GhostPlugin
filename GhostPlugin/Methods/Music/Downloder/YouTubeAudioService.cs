using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;
using GhostPlugin.MusicConfigs;

namespace GhostPlugin.Methods.Music.Downloder
{
    public sealed class YouTubeAudioService
    {
        private readonly MusicPlayback _cfg;

        public YouTubeAudioService(MusicPlayback cfg) => _cfg = cfg;

        public async Task<string?> DownloadAndConvertAsync(string youtubeUrl, CancellationToken ct = default)
        {
            Directory.CreateDirectory(_cfg.WorkDir);
            Directory.CreateDirectory(_cfg.AudioDir);

            // Common Options
            string uaWeb = string.IsNullOrWhiteSpace(_cfg.CustomUserAgent)
                ? "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36"
                : _cfg.CustomUserAgent!;
            string cookies = (!string.IsNullOrWhiteSpace(_cfg.CookiesPath) && File.Exists(_cfg.CookiesPath))
                ? $" --cookies \"{_cfg.CookiesPath}\""
                : "";
            string ipv4 = _cfg.ForceIPv4 ? " -4" : "";
            const string sleep = " --sleep-requests 1 --sleep-interval 2 --max-sleep-interval 5";

            // select a format: webm(opus) > m4a(aac) > 기타 bestaudio
            const string formatSel = "-f \"ba[ext=webm]/ba[ext=m4a]/bestaudio\"";

            // 1st try: desktop Web UA (+cookies/IPv4/slip)
            var tempBase = Path.Combine(_cfg.WorkDir, "yt_audio");
            var argsWeb =
                $"--no-playlist {formatSel} -o \"{tempBase}.%(ext)s\" --extract-audio --audio-format best --newline " +
                $"--user-agent \"{uaWeb}\"{cookies}{ipv4}{sleep} \"{youtubeUrl}\"";

            Log.Info($"[YT] {argsWeb}");
            var (exit1, so1, se1) = await ProcUtil.RunAsync(_cfg.YtDlpPath, argsWeb, _cfg.WorkDir, 600_000);
            if (exit1 != 0)
            {
                var errAll = (so1 + "\n" + se1);
                // 2nd try: iOS client(Avoid certain app restrictions)
                var argsIos =
                    $"--no-playlist {formatSel} -o \"{tempBase}.%(ext)s\" --extract-audio --audio-format best --newline " +
                    $"--extractor-args \"youtube:player_client=ios\" " +
                    $"--user-agent \"com.google.ios.youtube/19.12.3 (iPhone; CPU iPhone OS 17_5 like Mac OS X)\"" +
                    $"{cookies}{ipv4}{sleep} \"{youtubeUrl}\"";

                Log.Info($"[YT] fallback(iOS) {argsIos}");
                var (exit2, so2, se2) = await ProcUtil.RunAsync(_cfg.YtDlpPath, argsIos, _cfg.WorkDir, 600_000);

                if (exit2 != 0)
                {
                    Log.Error($"[YT] yt-dlp failed\n" +
                              $"[1st(web)] exit={exit1}\nSTDOUT:\n{so1}\nSTDERR:\n{se1}\n" +
                              $"[2nd(iOS)] exit={exit2}\nSTDOUT:\n{so2}\nSTDERR:\n{se2}");
                    // Common Cause Hint Output
                    bool NeedConsent(string s) =>
                        !string.IsNullOrEmpty(s) &&
                        s.IndexOf("Sign in to confirm", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (NeedConsent(errAll) || NeedConsent(so2) || NeedConsent(se2))
                    {
                        Log.Error("[YT] 쿠키가 필요합니다. 브라우저에서 로그인한 cookies.txt를 CookiesPath에 지정하세요.");
                    }
                    return null;
                }
            }

            // 3) Find generated files
            var src = Directory.GetFiles(_cfg.WorkDir, "yt_audio.*").FirstOrDefault();
            if (src == null)
            {
                Log.Error("[YT] Downloaded file not found.");
                return null;
            }

            // 4) Secure File Name
            var safeName = MakeSafeFileName(Path.GetFileNameWithoutExtension(src));
            if (string.IsNullOrWhiteSpace(safeName))
                safeName = $"music_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            var dstOgg = Path.Combine(_cfg.WorkDir, $"{safeName}.ogg");

            // 5) conversion ffmpeg: OGG/Vorbis/48kHz/Mono (AudioPlayer Requirement)
            var ffArgs =
                $"-y -i \"{src}\" -ar {_cfg.SampleRate} -ac {_cfg.Channels} -c:a libvorbis -b:a {_cfg.VorbisBitrate} \"{dstOgg}\"";
            Log.Info($"[FFMPEG] {ffArgs}");

            var (exitF, soF, seF) = await ProcUtil.RunAsync(_cfg.FfmpegPath, ffArgs, _cfg.WorkDir, 600_000);
            if (exitF != 0 || !File.Exists(dstOgg))
            {
                Log.Error($"[FFMPEG] convert failed (exit {exitF})\nSTDOUT:\n{soF}\nSTDERR:\n{seF}");
                return null;
            }

            // 6) Move to final position (allow overwriting)
            var finalPath = Path.Combine(_cfg.AudioDir, $"{safeName}.ogg");
            if (File.Exists(finalPath)) File.Delete(finalPath);
            File.Move(dstOgg, finalPath);

            // 7) Remove temporary files
            TryDeleteQuiet(src);
            foreach (var f in Directory.GetFiles(_cfg.WorkDir, "yt_audio.*"))
                TryDeleteQuiet(f);

            // 반환: Audio/relative race
            return $"Audio/{Path.GetFileName(finalPath)}";
        }

        private static string MakeSafeFileName(string name)
        {
            // Only Korean/English/number/blank/some special symbols are allowed
            var s = Regex.Replace(name, @"[^\w\s\-\.\(\)\[\]가-힣]", "_");
            return Regex.Replace(s, @"\s+", " ").Trim();
        }

        private static void TryDeleteQuiet(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }
    }
}