namespace GhostPlugin.Configs
{
    public sealed class MusicPlaybackModule
    {
        // yt-dlp / ffmpeg 실행파일 경로 (심볼릭 링크를 PATH에 걸었으면 "yt-dlp", "ffmpeg"만 둬도 됨)
        public string YtDlpPath { get; init; } = "yt-dlp";
        public string FfmpegPath { get; init; } = "ffmpeg";

        // 작업 디렉토리 및 최종 오디오 폴더 (SCP:SL 서버 루트 기준)
        //public string WorkDir { get; init; } = "/home/vscode/steamcmd/scpsl/tmp-audio";
        public string WorkDir { get; init; } = "/data/scpsl/tmp-audio";
        //public string AudioDir { get; init; } = "/home/hanbin/steamcmd/scpsl/Audio";
        public string AudioDir { get; init; } = "/data/scpsl/Audio";

        // 오디오 출력 설정
        public int SampleRate { get; init; } = 48000;   // 48kHz
        public int Channels { get; init; } = 1;         // mono
        public string VorbisBitrate { get; init; } = "160k"; // 112~192k 권장
        public string? CookiesPath { get; init; } = null; // 예: "/home/vscode/yt-cookies.txt"
        public bool ForceIPv4 { get; init; } = true;
        public string? CustomUserAgent { get; init; } =
            "com.google.android.youtube/19.12.4 (Linux; U; Android 13)";
        public bool UseAndroidClient { get; init; } = true; // youtube:player_client=android
    }
}