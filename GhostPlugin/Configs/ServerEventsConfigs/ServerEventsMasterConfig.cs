using System.Collections.Generic;
using System.ComponentModel;
using GhostPlugin.Configs.ServerEventsConfigs;

namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class ServerEventsMasterConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        public List<string> RandomEventsAllowedToStart { get; set; } = new List<string>
        {
            "BlackoutModeConfig",
            "ClassicConfig",
            "NoobSupportConfig",
        };

        public BlackoutModeConfig BlackoutModeConfig { get; set; } = new BlackoutModeConfig();
        public ClassicConfig ClassicConfig { get; set; } = new ClassicConfig();
        public NoobSupportConfig NoobSupportConfig { get; set; } = new NoobSupportConfig();
        //public SsssConfig SsssConfig { get; set; } = new SsssConfig();
    }
}