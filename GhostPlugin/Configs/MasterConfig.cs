using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using GhostPlugin.Configs.CustomConfigs;
using GhostPlugin.Configs.ServerEventsConfigs;
using YamlDotNet.Serialization;

namespace GhostPlugin.Configs
{
    public class MasterConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        
        [Description("There is a LOT of debug statements, turn this on if you really need top check something, otherwise keep it off to avoid flooding your server console")]
        public bool Debug { get; set; } = false;

        [YamlIgnore]
        public CustomItemsConfig CustomItemsConfig { get; set; } = null!;
        [YamlIgnore]
        public CustomRolesConfig CustomRolesConfig { get; set; } = null!;
        [YamlIgnore]
        public CustomRolesAbilitiesConfig CustomRolesAbilitiesConfig { get; set; } = null!;
        [YamlIgnore]
        public ServerEventsMasterConfig ServerEventsMasterConfig { get; set; } = null!;
        [YamlIgnore]
        public SsssConfig SsssConfig { get; set; } = null!;

        public string ConfigFolder { get; set; } =
            Path.Combine(Paths.Configs, "GhostServerPluginPackage");
        
        public string CustomItemConfigFile { get; set; } = "CustomItems.yml";
        public string CustomRolesConfigFile { get; set; } = "CustomRoles.yml";
        public string CustomRolesAbilitiesConfigFile { get; set; } = "CustomAbilities.yml";
        public string MicroDamageReductionConfigFile { get; set; } = "MicroDamageReduction.yml";
        public string ServerEventsMasterConfigFile { get; set; } = "ServerEvents.yml";
        public string SsssConfigFile { get; set; } = "Ssss.yml";

        public void LoadConfigs()
        {
            if(!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);
            
            string ciFilePath = Path.Combine(ConfigFolder, CustomItemConfigFile);
            if (!File.Exists(ciFilePath))
            {
                CustomItemsConfig = new CustomItemsConfig();
                File.WriteAllText(ciFilePath, Loader.Serializer.Serialize(CustomItemsConfig));
            }
            else
            {
                CustomItemsConfig = Loader.Deserializer.Deserialize<CustomItemsConfig>(File.ReadAllText(ciFilePath));
                File.WriteAllText(ciFilePath, Loader.Serializer.Serialize(CustomItemsConfig));
            }
            
            string crFilePath = Path.Combine(ConfigFolder, CustomRolesConfigFile);
            if (!File.Exists(crFilePath))
            {
                CustomRolesConfig = new CustomRolesConfig();
                File.WriteAllText(crFilePath, Loader.Serializer.Serialize(CustomRolesConfig));
            }
            else
            {
                CustomRolesConfig = Loader.Deserializer.Deserialize<CustomRolesConfig>(File.ReadAllText(crFilePath));
                File.WriteAllText(crFilePath, Loader.Serializer.Serialize(CustomRolesConfig));
            }
            
            string caFilePath = Path.Combine(ConfigFolder, CustomRolesAbilitiesConfigFile);
            if (!File.Exists(caFilePath))
            {
                CustomRolesAbilitiesConfig = new CustomRolesAbilitiesConfig();
                File.WriteAllText(caFilePath, Loader.Serializer.Serialize(CustomRolesAbilitiesConfig));
            }
            else
            {
                CustomRolesAbilitiesConfig = Loader.Deserializer.Deserialize<CustomRolesAbilitiesConfig>(File.ReadAllText(caFilePath));
                File.WriteAllText(caFilePath, Loader.Serializer.Serialize(CustomRolesAbilitiesConfig));
            }
            
            string serverEventsFilePath = Path.Combine(ConfigFolder, ServerEventsMasterConfigFile);
            if (!File.Exists(serverEventsFilePath))
            {
                ServerEventsMasterConfig = new ServerEventsMasterConfig();
                File.WriteAllText(serverEventsFilePath, Loader.Serializer.Serialize(ServerEventsMasterConfig));
            }
            else
            {
                ServerEventsMasterConfig = Loader.Deserializer.Deserialize<ServerEventsMasterConfig>(File.ReadAllText(serverEventsFilePath));
                File.WriteAllText(serverEventsFilePath, Loader.Serializer.Serialize(ServerEventsMasterConfig));
            }

            string ssssFilePath = Path.Combine(ConfigFolder, SsssConfigFile);
            if (!File.Exists(ssssFilePath))
            {
                SsssConfig = new SsssConfig();
                File.WriteAllText(ssssFilePath, Loader.Serializer.Serialize(SsssConfig));
            }
            else
            {
                SsssConfig = Loader.Deserializer.Deserialize<SsssConfig>(File.ReadAllText(ssssFilePath));
                File.WriteAllText(ssssFilePath, Loader.Serializer.Serialize(SsssConfig));
            }
        }
    }
}