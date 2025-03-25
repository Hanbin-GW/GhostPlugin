using System.ComponentModel;

namespace GhostPlugin.Configs.CustomConfigs
{
    public class CustomRolesAbilitiesConfig
    {
        [Description("Enables Custom Role Abilities")]
        public bool IsEnabled { get; set; } = true;
    }
}