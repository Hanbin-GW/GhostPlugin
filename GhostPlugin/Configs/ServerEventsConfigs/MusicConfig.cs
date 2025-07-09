using UnityEngine;

namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class MusicConfig
    {
        public bool OnEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        public string LobbySongPath { get; set; } = "Epic SciFi Military by Infraction No Copyright Music  War.ogg";
        public string WarheadBGMPath { get; set; } = "badending.ogg";
        public float Volume { get; set; } = 0.4f;

        public bool Loop { get; set; } = true;

        public Vector3 SpeakerScale { get; set; } = new Vector3(1f, 1f, 1f);

        public string Lcz15min { get; set; } = "74-Melancholy_trimmed.ogg";
        public string Lcz10min { get; set; } = "74-Melancholy_trimmed.ogg";
        public string Lcz5min { get; set; } = "77-Fearofthedark.ogg";
        public string Lcz1min { get; set; } = "77-Cauterizer.ogg";
        public string Lcz30sec { get; set; } = "Slow_Light.ogg";

        public string ChaosSpawmBgm { get; set; } = "Flower Crown of Poppy.ogg";
        public string CSquadSpawmBgm { get; set; } = "Flower Crown of Poppy.ogg";
        public string RespawnMtfBgm { get; set; } = "77part2ost.ogg";

        public string ReinforcementsSpawmBgm { get; set; } = "77part2ost.ogg";
        public bool EnableSpecialEvent { get; set; } = false;
        public string[] AllowedSteamIDs { get; set; } = 
        {
            "76561199133709329@steam",
            "76561199248290923@steam"
        };

        public string[] MusicPlayList { get; set; } = new[]
        {
            "77part2ost.ogg",
            "Slow_Light.ogg",
            "77.ogg"
        };
    }
}