using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;
using GhostPlugin.MusicConfigs;

namespace GhostPlugin.Methods.Music.Module
{
    public sealed class MusicPlayModule
    {
        private readonly MusicPlaybackModule _cfg;

        public MusicPlayModule(MusicPlaybackModule cfg) => _cfg = cfg;

        public async Task<string?> DownloadAndConvertAsync(string youtubeUrl, CancellationToken ct = default)
        {
            Directory.CreateDirectory(_cfg.WorkDir);
            Directory.CreateDirectory(_cfg.AudioDir);

            // 공통 옵션
            string uaWeb = string.IsNullOrWhiteSpace(_cfg.CustomUserAgent)
                ? "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36"
                : _cfg.CustomUserAgent!;
            string cookies = (!string.IsNullOrWhiteSpace(_cfg.CookiesPath) && File.Exists(_cfg.CookiesPath))
                ? $" --cookies \"{_cfg.CookiesPath}\""
                : "";
            string ipv4 = _cfg.ForceIPv4 ? " -4" : "";
            const string sleep = " --sleep-requests 1 --sleep-interval 2 --max-sleep-interval 5";

            // 포맷 선택: webm(opus) > m4a(aac) > 기타 bestaudio
            const string formatSel = "-f \"ba[ext=webm]/ba[ext=m4a]/bestaudio\"";

            // 1차 시도: 데스크톱 Web UA (+쿠키/IPv4/슬립)
            var tempBase = Path.Combine(_cfg.WorkDir, "yt_audio");
            var argsWeb =
                $"--no-playlist {formatSel} -o \"{tempBase}.%(ext)s\" --extract-audio --audio-format best --newline " +
                $"--user-agent \"{uaWeb}\"{cookies}{ipv4}{sleep} \"{youtubeUrl}\"";

            Log.Info($"[YT] {argsWeb}");
            var (exit1, so1, se1) = await MusicProcUtil.RunAsync(_cfg.YtDlpPath, argsWeb, _cfg.WorkDir, 600_000);
            if (exit1 != 0)
            {
                var errAll = (so1 + "\n" + se1);
                // 2차 시도: iOS 클라이언트(특정 앱 제한 회피)
                var argsIos =
                    $"--no-playlist {formatSel} -o \"{tempBase}.%(ext)s\" --extract-audio --audio-format best --newline " +
                    $"--extractor-args \"youtube:player_client=ios\" " +
                    $"--user-agent \"com.google.ios.youtube/19.12.3 (iPhone; CPU iPhone OS 17_5 like Mac OS X)\"" +
                    $"{cookies}{ipv4}{sleep} \"{youtubeUrl}\"";

                Log.Info($"[YT] fallback(iOS) {argsIos}");
                var (exit2, so2, se2) = await MusicProcUtil.RunAsync(_cfg.YtDlpPath, argsIos, _cfg.WorkDir, 600_000);

                if (exit2 != 0)
                {
                    Log.Error($"[YT] yt-dlp failed\n" +
                              $"[1st(web)] exit={exit1}\nSTDOUT:\n{so1}\nSTDERR:\n{se1}\n" +
                              $"[2nd(iOS)] exit={exit2}\nSTDOUT:\n{so2}\nSTDERR:\n{se2}");
                    // 흔한 원인 힌트 출력
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

            // 3) 생성된 파일 찾기
            var src = Directory.GetFiles(_cfg.WorkDir, "yt_audio.*").FirstOrDefault();
            if (src == null)
            {
                Log.Error("[YT] Downloaded file not found.");
                return null;
            }

            // 4) 안전한 파일명
            var safeName = MakeSafeFileName(Path.GetFileNameWithoutExtension(src));
            if (string.IsNullOrWhiteSpace(safeName))
                safeName = $"music_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            var dstOgg = Path.Combine(_cfg.WorkDir, $"{safeName}.ogg");

            // 5) ffmpeg 변환: OGG/Vorbis/48kHz/Mono (AudioPlayer 요구사항)
            var ffArgs =
                $"-y -i \"{src}\" -ar {_cfg.SampleRate} -ac {_cfg.Channels} -c:a libvorbis -b:a {_cfg.VorbisBitrate} \"{dstOgg}\"";
            Log.Info($"[FFMPEG] {ffArgs}");

            var (exitF, soF, seF) = await MusicProcUtil.RunAsync(_cfg.FfmpegPath, ffArgs, _cfg.WorkDir, 600_000);
            if (exitF != 0 || !File.Exists(dstOgg))
            {
                Log.Error($"[FFMPEG] convert failed (exit {exitF})\nSTDOUT:\n{soF}\nSTDERR:\n{seF}");
                return null;
            }

            // 6) 최종 위치로 이동 (덮어쓰기 허용)
            var finalPath = Path.Combine(_cfg.AudioDir, $"{safeName}.ogg");
            if (File.Exists(finalPath)) File.Delete(finalPath);
            File.Move(dstOgg, finalPath);

            // 7) 임시파일 정리
            TryDeleteQuiet(src);
            foreach (var f in Directory.GetFiles(_cfg.WorkDir, "yt_audio.*"))
                TryDeleteQuiet(f);

            // 반환: Audio/상대경로
            return $"Audio/{Path.GetFileName(finalPath)}";
        }

        private static string MakeSafeFileName(string name)
        {
            // 한글/영문/숫자/공백/일부 특수기호만 허용
            var s = Regex.Replace(name, @"[^\w\s\-\.\(\)\[\]가-힣]", "_");
            return Regex.Replace(s, @"\s+", " ").Trim();
        }

        private static void TryDeleteQuiet(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); } catch { }
        }
    }
}
