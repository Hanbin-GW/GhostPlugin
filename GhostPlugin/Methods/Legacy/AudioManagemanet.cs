using System;
using System.IO;
using Exiled.API.Features;
using SCPSLAudioApi.AudioCore;

namespace GhostPlugin.Methods.Legacy
{
    public class AudioManagemanet
    {
        private AudioPlayerBase SharedAudioPlayer;
        private bool IsMusicPlaying = false;
        /// <summary>
        /// SCPSLAudioApi's play music
        /// </summary>
        /// <param name="filepath"></param>
        public void PlaySpecificMusic(string filepath)
        {
            if (SharedAudioPlayer == null)
            {
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                if (SharedAudioPlayer == null)
                {
                    Log.Error("sharedAudioPlayer 초기화 실패. 음악을 재생할 수 없습니다.");
                    return;
                }
            }

            if (!File.Exists(filepath))
            {
                Log.Error($"오디오 파일을 찾을 수 없습니다: {filepath}");
                return;
            }
            
            StopLobbyMusic();
            IsMusicPlaying = true;
            SharedAudioPlayer.CurrentPlay = filepath;
            SharedAudioPlayer.Loop = false;  // 특정 곡은 반복하지 않음
            SharedAudioPlayer.Play(-1);
            Log.Info($"특정 곡이 재생 중입니다: {filepath}");
        }

        /// <summary>
        /// SCPSLAudioApi's stop music
        /// </summary>
        public void StopLobbyMusic()
        {
            if (SharedAudioPlayer != null && IsMusicPlaying == true)
            {
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                SharedAudioPlayer.Loop = false;
                SharedAudioPlayer.Stoptrack(true);
                Log.SendRaw("음악 중지 중...",ConsoleColor.DarkRed);
            }
            else
            {
                Log.Error("현재 재생 중인 음악이 없습니다.");
            }

        }
    }
}