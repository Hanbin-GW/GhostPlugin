namespace GhostPlugin.MusicConfigs
{
    public sealed class MusicPlayback
    {
        // yt-dlp / ffmpeg executable path (you can just leave "yt-dlp" and "ffmpeg" if you put a symbolic link on PATH)
        public string YtDlpPath { get; init; } = "/usr/local/bin/yt-dlp";
        public string FfmpegPath { get; init; } = "/usr/local/bin/ffmpeg";

        // Working directory and final audio folder (based on SCP:SL server root)
        public string WorkDir { get; init; } = "/home/Omega/steamcmd/scpsl/tmp-audio";
        public string AudioDir { get; init; } = "/home/Omega/steamcmd/scpsl/Audio";

        // Audio Output Settings
        public int SampleRate { get; init; } = 48000;   // 48kHz
        public int Channels { get; init; } = 1;         // mono
        public string VorbisBitrate { get; init; } = "160k"; // 112~192k 
        public string? CookiesPath { get; init; } = null; // 예: "/home/vscode/yt-cookies.txt"
        public bool ForceIPv4 { get; init; } = true;
        public string? CustomUserAgent { get; init; } =
            "com.google.android.youtube/19.12.4 (Linux; U; Android 13)";
        public bool UseAndroidClient { get; init; } = true; // youtube:player_client=android
    }
}