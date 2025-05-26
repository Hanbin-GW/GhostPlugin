using System;
using Discord;
using Exiled.API.Features;
using UnityEngine;

namespace GhostPlugin.Methods.Music
{
    public class JukeboxManagement
    {
        public void PlayMusicSpeaker(string filepath, Vector3 pos, int id)
        {
            AudioClipStorage.LoadClip(filepath,filepath);

            AudioPlayer globalPlayer = AudioPlayer.CreateOrGet($"SpeakerBox{id}",onIntialCreation: (p) =>
            {
                p.AddSpeaker("Main", isSpatial: true, maxDistance: 5f, volume: 0.7f, position:pos);
            });
            
            globalPlayer.AddClip(filepath, volume: Plugin.Instance.Config.MusicConfig.Volume, loop: Plugin.Instance.Config.MusicConfig.Loop, destroyOnEnd: false);
            Log.Send("main song playing",LogLevel.Info,ConsoleColor.DarkRed);
            Log.Info($"music playing in speaker: {filepath}, position: {pos}");
        }
        
        public void StopMusicSpeaker(int id)
        {
            if(!AudioPlayer.AudioPlayerByName.TryGetValue($"SpeakerBox{id}",out AudioPlayer ap))
                return;
            //ap.ClipsById.Clear();
            ap.RemoveAllClips();
            //ap.Destroy();
        }
    }
}
