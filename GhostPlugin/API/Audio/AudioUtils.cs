using System.IO;
using NVorbis;

namespace GhostPlugin.API.Audio
{
    public static class AudioUtils
    {
        public static float GetOggDurationInSeconds(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Cannot find the audio", filePath);
            }

            using (var stream = File.OpenRead(filePath))
            using (var vorbis = new VorbisReader(stream, false))
            {
                double duration = vorbis.TotalTime.TotalSeconds;
                return (float)duration;
            }
        }
    }
}