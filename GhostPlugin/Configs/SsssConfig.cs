namespace GhostPlugin.Configs
{
    public class SsssConfig
    {
        public bool IsEnabled { get; set; } = true;
        public string Header { get; set; } = "Custom Ability Keybind";
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
        public int ShockwaveId { get; set; } = 10014;
        public int MapToggleId { get; set; } = 20000;
        public int MusicToggleId { get; set; } = 20001;
        public int ResupplyId { get; set; } = 20002;
        public int OverkillId { get; set; } = 20003;

        public string SsssActiveCamoActivationMessage { get; set; } = "Activated Active Camo";
        public string SsssChargeActivationMessage { get; set; } = "Activated Charge";
        public string SsssDoorPickingActivationMessage { get; set; } = "Activated Door Picking, Interact with the door you want to pick.";
        public string SsssHealingMistActivationMessage { get; set; } = "Activated Healing Mist";
        public string SsssRemoveDisguiseActivationMessage { get; set; } = "Removing Disguise";
        public string FocousActivationMessage { get; set; } = "Extrem SCP-1853 effect!";
        public string EnhanseVisionActivationMessage { get; set; } = "You can see all the enemies temporarily.";
        public string Scp457ActivationMessage { get; set; } = "burning down all the enemies within 5m!";
        public string SsssC4NoC4Deployed { get; set; } = "You didn't installed C4!";
        public string SsssC4DetonatorNeeded { get; set; } = "You need to hold the detonator (radio)";
        public string SsssC4TooFarAway { get; set; } = "You are too far from C4, Recommend to go little closely.";
        public string SsssDetonateC4ActivationMessage { get; set; } = "C4 폭발";
        public string ResupplyActivatMessage { get; set; } = "Grenades distributed.";
        public string ShockwaveActivateMessage { get; set; } = "충격파로 근처 모든 인원이 마비되었습니다!";
        public string OverkillActivationMessage { get; set; } = "오버킬 능력이 작동되었습니다!";

    }
}