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

        public List<string> messages { get; set; } = new List<string>()
        {
            "test",
            "test1"
        };

        [Description("Enable The SafeMode?")] 
        public bool IsSafeMode { get; set; } = true;
        [Description("라운드 시작 message")]
        public string RoundStartMSG { get; set; } = "<size=38>👻라운드가 시작되었습니다...👻</size>\n<size=30>Good Luck</size>";

        [Description("저위험군 봉쇄 15분 메세지")] public string Lcz15 { get; set; } = "저위험군 봉쇄 15분 남음.";

        [Description("저위험군 봉쇄 15분 메세지 시간(초단위)")]
        public ushort Lcz15Time { get; set; } = 10;

        [Description("저위험군 봉쇄 10분 메세지")] public string Lcz10 { get; set; } = "저위험군 봉쇄 10분 남음.";

        [Description("저위험군 봉쇄 10분 메세지 시간(초단위)")]
        public ushort Lcz10Time { get; set; } = 10;

        [Description("저위험군 봉쇄 5분 메세지")] public string Lcz5 { get; set; } = "저위험군 봉쇄 5분 남음.";

        [Description("저위험군 봉쇄 5분 메세지 시간(초단위)")]
        public ushort Lcz5Time { get; set; } = 10;

        [Description("저위험군 봉쇄 1분 메세지")] public string Lcz1 { get; set; } = "저위험군 봉쇄 1분 남음.";

        [Description("저위험군 봉쇄 1분 메세지 시간(초단위)")]
        public ushort Lcz1Time { get; set; } = 10;

        [Description("저위험군 봉쇄 30초 메세지")] public string Lcz30 { get; set; } = "저위험군 봉쇄 30초 남음.";

        [Description("저위험군 봉쇄 30초 메세지 시간(초단위)")]
        public ushort Lcz30Time { get; set; } = 10;

        [Description("저위험군 봉쇄 시작 메세지")] public string LcZstart { get; set; } = "저위험군 봉쇄 됨";

        [Description("저위험군 봉쇄 시작 메세지 시간(초단위)")]
        public ushort LcZstarttime { get; set; } = 10;

        [Description("저위험군 봉쇄 메세지를 오직 저위험군에만 띄울까요?(저위험군 봉쇄 시작은 제외)")]
        public bool OnlyLcZinMessage { get; set; } = true;

        [Description("NTF 지원 메세지({0} = MTF 유닛 이름, {1} = MTF 유닛 숫자, {2} = SCP-049-2를 제외한 재격리 대기중인 SCP 개체)")]
        public string NtfRespawn { get; set; } =
            "<color=#1100ff>M</color><color=#3021ff>T</color><color=#4c40ff>F</color> {0}-{1}(이)가 지원이 왔습니다.\n재격리 대기 중인 SCP개체는 {2}마리입니다.";

        [Description("NTF 지원 메세지 시간")] public ushort NtfRespawntime { get; set; } = 10;

        [Description("오늘의 공지사할 적기")]
        public string AnnouncmentMessage { get; set; } =
            "<size=38><b><color=#6900ff>📣공</color><color=#7919ff>지</color><color=#852eff>사</color><color=#8f40ff>항📣</color></b></size>\n<size=34>오늘의 공시사항은 없습니다!\n즐거운 하루 되십시오!</size>";

    }
}