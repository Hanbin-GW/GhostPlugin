namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class Scp914Config
    {
        public bool IsEnabled { get; set; } = false;
        public string ActivationMessage { get; set; } = "<color=yellow>{activator} activated SCP-914! Mode: <b>{mode}</b></color>";
        public ushort MessageDuration { get; set; } = 7;
    }
}