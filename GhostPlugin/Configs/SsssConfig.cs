namespace GhostPlugin.Configs
{
    public class SsssConfig
    {
        public bool IsEnabled { get; set; } = true;

        public int ActiveCamoId { get; set; } = 10000;
        public int ChargeId { get; set; } = 10001;
        public int DetectId { get; set; } = 10002;
        public int DoorPickingId { get; set; } = 10003;
        public int HealingMistId { get; set; } = 10004;
        public int RemoveDisguiseId { get; set; } = 10005;
        public int DetonateC4Id { get; set; } = 10006;
        public int FocousId { get; set; } = 10007;
        public int EnhanseVisionId { get; set; } = 10008;
        public int GhostId { get; set; } = 10009;
        public int Scp457Id { get; set; } = 10010;
        public int Scp106Id { get; set; } = 10011;
        public int ExplosionId { get; set; } = 10012;
        public int Speedy096Id { get; set; } = 10013;
        public int OverKillId { get; set; } = 10014;
        public string SsssActiveCamoActivationMessage { get; set; } = "Activated Active Camo";
        public string SsssChargeActivationMessage { get; set; } = "Activated Charge";
        public string SsssDoorPickingActivationMessage { get; set; } = "Activated Door Picking, Interact with the door you want to pick.";
        public string SsssHealingMistActivationMessage { get; set; } = "Activated Healing Mist";
        public string SsssRemoveDisguiseActivationMessage { get; set; } = "Removing Disguise";
        public string FocousActivationMessage { get; set; } = "극단의 SCP-1853 이 적용되었습니다!";
        public string EnhanseVisionActivationMessage { get; set; } = "일시적으로 모든 적을 볼수 있습니다.";
        public string Scp457ActivationMessage { get; set; } = "5m 내의 적들을 다 불태웁니다!";
        public string SsssC4NoC4Deployed { get; set; } = "C4 를 설치 안하셧어요!";
        public string SsssC4DetonatorNeeded { get; set; } = "기폭 장치(무전기)를 들고 있어야 합니다";
        public string SsssC4TooFarAway { get; set; } = "C4 로부터 너무 떨어져 계십니다, 가까이 가시는거를 고려하시기 바랍니다.";
        public string SsssDetonateC4ActivationMessage { get; set; } = "C4 폭발";
        public string SsssOverkillActivationMessage { get; set; } = "오버킬 능력 활성화!";
    }
}