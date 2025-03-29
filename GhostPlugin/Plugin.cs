using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.API;
using GhostPlugin.Configs;
using GhostPlugin.EventHandlers;
using HarmonyLib;
using UserSettings.ServerSpecific;
using Server = Exiled.Events.Handlers.Server;

namespace GhostPlugin
{
    public class Plugin : Plugin<MasterConfig>
    {
        public static Plugin Instance;
        public List<Player> StopRagdollList { get; } = new ();
        public Dictionary<StartTeam, List<ICustomRole>> Roles { get; } = new();
        private Harmony Harmony { get; set; }
        /// <summary>
        /// Speakers List
        /// </summary>
        public int CurrentId = 1;
        public override Version Version { get; } = new(4, 1, 1);
        public override string Author { get; } = "Hanbin-GW";
        public override string Name { get; } = "Ghost-Plugin";
        public override PluginPriority Priority { get; } = PluginPriority.Low;
        //private MyCustomKeyBind _myCustomKeyBind;
        public SsssEventHandler SsssEventHandler;
        public readonly string AudioDirectory;
        public readonly string EffectDirectory;
        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            EffectDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "Effects_Audio");
            AudioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "Audio");
        }

        public override void OnEnabled()
        {
            Instance = this;
            if (Config == null)
            {
                Log.Error("Config is null!");
                return;
            }

            Config.LoadConfigs();
            if (Config.SsssConfig == null)
            {
                Log.Error("Config.SsssConfig is null!");
                return;
            }

            Log.Send($"[GHOST.API] {Instance.Name} is enabled By {Instance.Author} | Version: {Instance.Version}",LogLevel.Info, ConsoleColor.DarkYellow);
            Config.LoadConfigs();
            if(Config.ServerEventsMasterConfig.BlackoutModeConfig.IsEnabled){BlackoutMod.RegisterEvents();}

            //CustomItem Config
            if (Config.CustomItemsConfig.IsEnabled)
            {
                Config.CustomItemsConfig.HackingDevices.Register();
                Config.CustomItemsConfig.Morses.Register();
                Config.CustomItemsConfig.ShockwaveGuns.Register();
                Config.CustomItemsConfig.FtacReacon.Register();
                Config.CustomItemsConfig.PoisonGuns.Register();
                Config.CustomItemsConfig.ClusterGrenades.Register();
                Config.CustomItemsConfig.TripleFlashGrenades.Register();
                Config.CustomItemsConfig.StunGrenades.Register();
                Config.CustomItemsConfig.Stims.Register();
                Config.CustomItemsConfig.BattleRages.Register();
                Config.CustomItemsConfig.Svds.Register();
                Config.CustomItemsConfig.EodPaddings.Register();
                Config.CustomItemsConfig.SmokeGrenades.Register();
                Config.CustomItemsConfig.ExplosiveRoundRevolvers.Register();
                Config.CustomItemsConfig.ParalyzeRifes.Register();
                Config.CustomItemsConfig.PlasmaEmitters.Register();
                Config.CustomItemsConfig.PlasmaShockwaveEmitters.Register();
                Config.CustomItemsConfig.PhotonCannons.Register();
                Config.CustomItemsConfig.ArmorPlateKits.Register();
                Config.CustomItemsConfig.StickyGrenades.Register();
                Config.CustomItemsConfig.PlasmaShotguns.Register();
                Config.CustomItemsConfig.Bolters.Register();
                Config.CustomItemsConfig.AcidShooters.Register();
                Config.CustomItemsConfig.C4s.Register();
                Config.CustomItemsConfig.SpikeJailbirds.Register();
                Config.CustomItemsConfig.ReviveKits.Register();
                Config.CustomItemsConfig.PoisonGrenades.Register();
                Config.CustomItemsConfig.LaserCannons.Register();
                Config.CustomItemsConfig.Anti173s.Register();
                Config.CustomItemsConfig.Basilisks.Register();
                Config.CustomItemsConfig.OverkillVests.Register();
                Config.CustomItemsConfig.MachineGuns.Register();
                Config.CustomItemsConfig.Riveters.Register();
            }
            
            //CustomRole Config
            if (Config.CustomRolesConfig.IsEnabled)
            {
                Config.CustomRolesConfig.ChiefScientists.Register();
                Config.CustomRolesConfig.CiPhantoms.Register();
                Config.CustomRolesConfig.DAlphas.Register();
                Config.CustomRolesConfig.Tanker106S.Register();
                Config.CustomRolesConfig.Vipers.Register();
                Config.CustomRolesConfig.Jailbirdmans.Register();
                Config.CustomRolesConfig.Administrators.Register();
                Config.CustomRolesConfig.FedoraAgents.Register();
                Config.CustomRolesConfig.Elites.Register();
                Config.CustomRolesConfig.JuggernautChaosList.Register();
                Config.CustomRolesConfig.Scp682s.Register();
                Config.CustomRolesConfig.SoleStealer049s.Register();
                Config.CustomRolesConfig.Scp049Aps.Register();
                Config.CustomRolesConfig.Demolitionists.Register();
                Config.CustomRolesConfig.DwarfZombies.Register();
                Config.CustomRolesConfig.ExplosiveZombies.Register();
                Config.CustomRolesConfig.EodSoldierZombies.Register();
                foreach (CustomRole role in CustomRole.Registered)
                {
                    Instance.Config.LoadConfigs();
                    if (role.CustomAbilities is not null)
                    {
                        foreach (CustomAbility ability in role.CustomAbilities)
                        {
                            ability.Register();
                        }
                    }

                    if (role is ICustomRole custom)
                    {
                        Log.Debug(
                            $"Adding {role.Name} to dictionary..");
                        StartTeam team;
                        if (custom.StartTeam.HasFlag(StartTeam.Chaos))
                            team = StartTeam.Chaos;
                        else if (custom.StartTeam.HasFlag(StartTeam.Guard))
                            team = StartTeam.Guard;
                        else if (custom.StartTeam.HasFlag(StartTeam.Ntf))
                            team = StartTeam.Ntf;
                        else if (custom.StartTeam.HasFlag(StartTeam.Scientist))
                            team = StartTeam.Scientist;
                        else if (custom.StartTeam.HasFlag(StartTeam.ClassD))
                            team = StartTeam.ClassD;
                        else if (custom.StartTeam.HasFlag(StartTeam.Scp))
                            team = StartTeam.Scp;
                        else
                            team = StartTeam.Other;

                        if (!Instance.Roles.ContainsKey(team))
                            Instance.Roles.Add(team, new());

                        for (int i = 0; i < role.SpawnProperties.Limit; i++)
                            Instance.Roles[team].Add(custom);
                        Log.Debug($"CustomRolesConfig {team} now has {Instance.Roles[team].Count} elements.");
                    }
                }
            }
            //CustomAbility Config
            if (Instance.Config.CustomRolesAbilitiesConfig.IsEnabled)
                CustomAbility.RegisterAbilities();
            
            if (Config.ServerEventsMasterConfig.BlackoutModeConfig.IsEnabled) { ClassicPlugin.RegisterEvents(); }
            if (Config.CustomRolesConfig.IsEnabled) {CustomRoleHandler.RegisterEvents();}
            if (Config.ServerEventsMasterConfig.NoobSupportConfig.OnEnabled) {NoobSupport.RegisterEvents();}

            /*if (Config.EnableHarmony)
            {
                Harmony = new Harmony($"Hanbin-GW.GhostPlugin.{DateTime.Now.Ticks}");
                Harmony.PatchAll();
                Log.Info($"{Harmony.Id} is enabled");
                //Harmony.DEBUG = true;
            }*/
            
            //SSSS
            /*if (Config.SsssConfig.IsEnabled == true)
            {
                SsssEventHandler = new SsssEventHandler(this);
                Server.RoundStarted += SsssEventHandler.OnRoundStarted;
                ServerSpecificSettingsSync.ServerOnSettingValueReceived += SsssEventHandler.OnSettingValueReceived;
            }*/
            try
            {
                if (Config.SsssConfig.IsEnabled)
                {
                    SsssEventHandler = new SsssEventHandler(this);

                    if (SsssEventHandler is null)
                    {
                        Log.Error("SsssEventHandler가 null입니다!");
                        return;
                    }

                    Server.RoundStarted += SsssEventHandler.OnRoundStarted;
                    ServerSpecificSettingsSync.ServerOnSettingValueReceived += SsssEventHandler.OnSettingValueReceived;
                    Log.Info("SsssEventHandler 이벤트 정상 등록됨");
                }
                else
                {
                    Log.Warn(" SsssConfig가 비활성화됨.");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"SsssEventHandler 생성 중 예외 발생: {ex.Message}");
                return;
            }

            /*if (Config.ServerEventsMasterConfig.SsssConfig.IsEnabled)
            {
                _myCustomKeyBind = new MyCustomKeyBind();
                _myCustomKeyBind.Activate();
            }*/
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            
            //BlackOut mode
            if(Config.ServerEventsMasterConfig.BlackoutModeConfig.IsEnabled){BlackoutMod.UnregisterEvents();}
            
            //CustItem
            if (Config.CustomItemsConfig.IsEnabled) {CustomItem.UnregisterItems();}
            
            //ClassicPlugin
            if (Config.ServerEventsMasterConfig.ClassicConfig.OnEnabled){ClassicPlugin.UnregisterEvents();}
            
            //Noob Support
            if (Config.ServerEventsMasterConfig.NoobSupportConfig.OnEnabled) {NoobSupport.UnregisterEvents();}
            //Music Event

            //CustomRoles
            if (Config.CustomRolesConfig.IsEnabled)
            {
                CustomRole.UnregisterRoles();
                CustomRoleHandler.UnregisterEvents();
            }
            //Harmony
            Harmony.UnpatchAll();
            
            //Legacy SSSS
            /*if (Config.ServerEventsMasterConfig.SsssConfig.IsEnabled)
            {
                _myCustomKeyBind = new MyCustomKeyBind();
                _myCustomKeyBind.Deactivate();
            }*/
            
            //SSSS - REWORK
            Server.RoundStarted -= SsssEventHandler.OnRoundStarted;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SsssEventHandler.OnSettingValueReceived;
            SsssEventHandler = null;
            Instance = null;
            base.OnDisabled();
        }
        
        public void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "Effects_Audio");

            // 폴더가 없으면 생성
            if (!Directory.Exists(path))
            {
                Log.Warn($"음악 폴더가 존재하지 않습니다. 새로 생성합니다: {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("음악 폴더가 이미 존재합니다.");
            }
        }
    }
}
