using System.Collections.Generic;
using System.ComponentModel;

namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class ClassicConfig
    {
        public bool OnEnabled { get; set; } = true;

        /// <summary>
        /// Ghost Plugin Classic Config
        /// </summary>
        public BroadcastSystem JoinMessage { get; set; } = new()
        {
            Duration = 5,
            Message =
                "<b>Welcome, %name% this is <color=#6203fc>G</color><color=#7724ff>h</color><color=#8033ff>o</color><color=#8b45ff>s</color><color=#995cff>t</color> <color=#00ffae>server!</color></b>",
            Override = false
        };

        [Description("Enable The SafeMode?")] public bool IsSafeMode { get; set; } = false;

        [Description("Round Start message")]
        public string RoundStartMSG { get; set; } =
            "<size=38>👻The Round has been started...👻</size>\n<size=30>Good Luck</size>";

        [Description("Reminder: This is imcomplete!")]
        public bool IsEnableFPSmap { get; set; } = false;

        [Description("저위험군 봉쇄 15분 메세지")]
        public string Lcz15 { get; set; } = "<color=red>Alert</color>\n<size=30>LCZ lockdown 15min left</size>";

        [Description("저위험군 봉쇄 15분 메세지 시간(초단위)")]
        public ushort Lcz15Time { get; set; } = 10;

        [Description("저위험군 봉쇄 10분 메세지")]
        public string Lcz10 { get; set; } = "<color=red>Alert</color>\n<size=30>LCZ lockdown 10min left</size>";

        [Description("저위험군 봉쇄 10분 메세지 시간(초단위)")]
        public ushort Lcz10Time { get; set; } = 10;

        [Description("저위험군 봉쇄 5분 메세지")]
        public string Lcz5 { get; set; } = "<color=red>Alert</color>\n<size=30>LCZ lockdown 5min left</size>";

        [Description("저위험군 봉쇄 5분 메세지 시간(초단위)")]
        public ushort Lcz5Time { get; set; } = 10;

        [Description("저위험군 봉쇄 1분 메세지")]
        public string Lcz1 { get; set; } = "<color=red>Alert</color>\n<size=30>LCZ lockdown 1min left</size>";

        [Description("저위험군 봉쇄 1분 메세지 시간(초단위)")]
        public ushort Lcz1Time { get; set; } = 10;

        [Description("저위험군 봉쇄 30초 메세지")]
        public string Lcz30 { get; set; } =
            "<color=red>Alert</color>\n<size=30>LCZ lockdown 30sec left\n<color=red>Evacuate Immediately</color></size>";

        [Description("저위험군 봉쇄 30초 메세지 시간(초단위)")]
        public ushort Lcz30Time { get; set; } = 10;

        [Description("저위험군 봉쇄 시작 메세지")]
        public string LcZstart { get; set; } =
            "<size30>Light Containment Zone is locked down and ready for decontamination. \n<color=red>The removal of organic substances has now begun</color></size>";

        [Description("저위험군 봉쇄 시작 메세지 시간(초단위)")]
        public ushort LcZstarttime { get; set; } = 10;

        [Description("Should low-risk containment messages only be posted to low-risk groups (except for the start of low-risk containment)")]
        public bool OnlyLcZinMessage { get; set; } = true;

        [Description("NTF Respawn Message({0} = MTF Unit Name, {1} = MTF Unit Number, {2} = SCP objects waiting for re-quarantine except SCP-049-2)")]
        public string NtfRespawn { get; set; } =
            "<color=#1100ff>M</color><color=#3021ff>T</color><color=#4c40ff>F</color> {0}-{1} entered the facility.\n Awaiting re-containment of {2} SCP subject(s).";

        [Description("NTF Repawn Announcement Time")] public ushort NtfRespawntime { get; set; } = 10;

        [Description("Write today's announcement")]
        public string AnnouncmentMessage { get; set; } =
            "<size=38><b><color=#6900ff>📣An</color><color=#7919ff>no</color><color=#852eff>un</color><color=#8f40ff>cements📣</color></b></size>\n<size=34>There are no announcements today! \nHave a wonderful day!</size>";
        public List<string> DonatorList { get; set; } = new List<string>()
        {
            "서버 관리자",
            "서버 운영자",
            "[Level4] - 후원자"
        };
    }
}