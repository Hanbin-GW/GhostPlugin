namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class BlackoutModeConfig
    {
        /// <summary>
        /// BlackoutMod mode Config
        /// </summary>
        public bool IsEnabled { get; set; } = false;
        public float BlackoutChance { get; set; } = 40f;
        public string CassieMessage { get; set; } = "<size=0> PITCH_.2 .G4 .G4 PITCH_.9 ATTENTION ALL PITCH_.6 PERSONNEL .G2 PITCH_.8 JAM_027_4 . PITCH_.15 .G4 .G4 PITCH_9999</size><color=#d64542>Attention, <color=#f5e042>all personnel...<split><size=0> PITCH_.9 GENERATORS PITCH_.7 IN THE PITCH_.85 FACILITY HAVE BEEN PITCH_.8 DAMAGED PITCH_.2 .G4 .G4 PITCH_9999</size><color=#d67d42>Generators in <color=#f5e042>the facility <color=#d67d42>have been <color=#d64542>damaged.<split><size=0> PITCH_.8 THE FACILITY PITCH_.9 IS GOING THROUGH PITCH_.85 A BLACK OUT PITCH_.15 .G4 .G4 PITCH_9999</size><color=#d64542><color=#f5e042>The facility <color=#d67d42>is going through a <color=#000000>blackout.";
        public string CassieOperationalMessage { get; set; } = "PITCH_.2 .G4 .G4 PITCH_.9 ATTENTION ALL PERSONNEL .G2 PITCH_.8 JAM_027_4 SYSTEM OPERATION STABLE";
        public string CassieOperationalMessageTranslation { get; set; } = "Attention, all personnel... System Operation Stable";
    }
}