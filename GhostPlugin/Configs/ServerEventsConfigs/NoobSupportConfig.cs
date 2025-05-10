namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class NoobSupportConfig
    {
        public bool OnEnabled { get; set; } = false;
        public string MicroHidLowEnergyMessage { get; set; } = "레일건의 베터리가 매우 부족합니다...";

        public string MicroHidEnergyMessage { get; set; } = "남은 에너지: {0}%";

        public bool ShowHintOnEquipItem { get; set; }

        public string Scp207HintMessage { get; set; } = "당신은 {0} 개의 SCP-207 를 마셨습니다!";

        public string AntiScp207HintMessage { get; set; } = "당신은 {0} 개의 Anti SCP-207 를 마셨습니다!";

        public string CardiacArrestMessage { get; set; } = "심정지 가 감지되었습니다.. 대량의 의료용 아이탬 또는 아드레난린 주사기를 사용하여 회복하십시요..!";

        public string PoisonMessage { get; set; } = "당신은 중독되었습니다..!\n<color=green>SCP-500</color> 을 사용하여 독을 완화하십시요...!";
        public string LowHpMessage { get; set; } = "HP 가 매우낮습니다...\n가능한 빨리 의료장비를 사용하십시요..!";

        public string A7Info { get; set; } = "A7 에 피격당하셨습니다...\nA7 의 피격시 화상효과가 일어납니다..";

        public string Looking096 { get; set; } = "SCP096 이 당신을 추적하고있습니다..!\n폭주가 끝날때까지 버티십시요..!";

        public string JailbirdUseMessage { get; set; } = "남은 돌진공격 수: {0}";

        public string ScpHealMessage { get; set; } = "적을 처치하여 {0} 의 HP 를 회복하셨습니다.";

        public string BleedingMessage { get; set; } = "출혈이 감지되었습니다..\n의료용 아이탬을 사용하십시요..!";

        public string PocketDimensionMessage { get; set; } = "당신의 신체가 썩어가고 있습니다..!\n빨리 이곳에서 나가십시요...!";

        public int DelayLczMessageTime { get; set; } = 18;

        public string Lcz15_hint { get; set; } =
            "저위험군에서 나갈수 없으신가요?\n키카드를 얻어서 914에서 업그래이드 하세요\n O5 같은 권한이 높은 키카드를 얻으셨다면 바로 나가셔도 됩니다!";

        public string Lcz10_hint { get; set; } =
            "저위험군 폐쇠까지 10분 남았습니다!\n키카드를 얻어서 914에서 업그래이드 하세요\n O5 같은 권한이 높은 키카드를 얻셨셨다면 바로 나가셔도 됩니다!";

        public string Lcz5_hint { get; set; } = "저위험군 폐쇠까지 5분 남았습니다!\n슬슬 나갈 준비를 해야합니다..";

        public string Lcz1_hint { get; set; } = "저위험군 폐쇠까지 1분 남았습니다!\n곧 저위험군의 검문소가 자동으로 열릴겁니다...";

        public string Lcz30s_hint { get; set; } = "저위험군 폐쇠까지 30초 남았습니다!\n지금 나가지 않으면, 죽음이 당신을 기달립니다...";

        public string GrenadeMessage { get; set; } = "좌클릭 으로 멀리,  우클릭으로 가까이 던질수 있습니다!";

        public string AdrenalineMessage { get; set; } =
            "아드레난린 주사기를 사용하여 <color=red>SCP049 의 심정지 공격</color>을 <color=green>무력화</color> 시킬수 있습니다!";

        public string Scp049SpawnMessage { get; set; } =
            "당신은 <color=red>SCP-049</color> 입니다..!\n시체의 [E] 키를 꾹눌러 <color=green>SCP-049-2</color> 를 만들수 있습니다..!";

        public string Scp0492SpawnMessage { get; set; } =
            "당신은 <color=#c77036>SCP-049-2</color> 입니다..!\n<color=red>SCP-049</color> 가 당신을 부활시킬수 있습니다..!";

        public string Scp079SpawnMessage { get; set; } =
            "당신은 <color=#757aff>SCP-079</color> 입니다..!\n SCP들을 도와 제단의 접근 권한티어를 올리고 시설을 장학하십시요!";

        public string DclassSpawnMessage { get; set; } = "당신은 <color=orange>D-Class</color> 입니다..!\n시설에서 탈출하십시요..!";

        public string ScientistSpawnMessage { get; set; } = "당신은 <color=yellow>과학자</color> 입니다..!\n시설에서 탈출하십시요..!";

        public string FacilityGuardSpawnMessage { get; set; } =
            "당신은 <color=#9c9c9c>시설경비</color> 입니다..!\n반란죄수를 사살하고, 과학자의 탈출을 도우십시요!";

        public string ChaosInsurgencySpawnMessage { get; set; } =
            "당신은 <color=#418234>혼돈의 반란</color> 입니다..!\n반란죄수를 구출하고, 시설을 파괴하십시요!";

        public string NtfSpawnMessage { get; set; } =
            "당신은 <color=#196fe0>특수작전부대</color> 입니다..!\n과학자를 구출하고, 시설을 정리하십시요!";
        
        
    }
}