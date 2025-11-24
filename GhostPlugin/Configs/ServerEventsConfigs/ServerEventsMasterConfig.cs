using System.Collections.Generic;
using System.ComponentModel;
using GhostPlugin.Configs.ServerEventsConfigs;

namespace GhostPlugin.Configs.ServerEventsConfigs
{
    public class ServerEventsMasterConfig
    {
        [Description("Is the plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        public BlackoutModeConfig BlackoutModeConfig { get; set; } = new();
        public ClassicConfig ClassicConfig { get; set; } = new();
        public NoobSupportConfig NoobSupportConfig { get; set; } = new();
        
        public KillStreakConfig KillStreakConfig { get; set; } = new();
        //public SsssConfig SsssConfig { get; set; } = new SsssConfig();
    }
}