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
using MEC;
using GhostPlugin.Enums;
//using HarmonyLib;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using ProjectMER.Features.Objects;
using UserSettings.ServerSpecific;
using Server = Exiled.Events.Handlers.Server;

namespace GhostPlugin
{
    public class Plugin : Plugin<MasterConfig>
    {
        public static Plugin Instance;
        public List<Player> StopRagdollList { get; } = new ();
        public Dictionary<StartTeam, List<ICustomRole>> Roles { get; } = new();
        //private Harmony Harmony { get; set; }
        /// <summary>
        /// Speakers List
        /// </summary>
        public Dictionary<int, SchematicObject> Speakers { get; private set; } = new();
        public RunMode CurrentRunMode { get; private set; } 

        public Dictionary<int, bool> MusicDisabledPlayers = new();
        public int CurrentId = 1;
        public override Version Version { get; } = new(9, 3, 0);
        public override string Author { get; } = "Hanbin-GW";
        public override string Name { get; } = "Ghost-Plugin";
        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        //private MyCustomKeyBind _myCustomKeyBind;
        public CustomRoleHandler CustomRoleHandler;
        /// <summary>
        /// Minimap Dict
        /// </summary>
        public readonly Dictionary<int, bool> miniMapEnabled = new();
        
        /// <summary>
        /// SSSS Ecent Handler
        /// </summary>
        public SsssEventHandler SsssEventHandler;
        
        /// <summary>
        /// Casual FPS Mode
        /// </summary>
        public CasualFPSModeHandler CasualFPSModeHandler;

        //public MusicMethods _musicManager;

        public PerkEventHandlers PerkEventHandlers;

        public CustomItemHandler CustomItemHandler;

        public MusicEventHandlers MusicEventHandlers;
        
        public KillStreakEventHandlers KillStreakEventHandlers;
        
        private bool _fullUpgraded;
        //Audio Dir
        public readonly string AudioDirectory;
        public readonly string EffectDirectory;
        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            EffectDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "effects");
            AudioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }
        
        private void Run(string name, Action a)
        {
            try { Log.Info($"[STAGE] {name}: enter"); a(); Log.Info($"[STAGE] {name}: ok"); }
            catch (Exception e) { Log.Error($"[STAGE] {name}: FAIL -> {e.GetType().Name}: {e.Message}\n{e}"); throw; }
        }
        public override void OnEnabled()
        {
            Instance = this;
            Server.WaitingForPlayers += OnWaitingPlayer;
            Run("config.load", () =>
            {
                if (Config == null) throw new NullReferenceException("Config is null");
                Config.LoadConfigs();
                if (Config.SsssConfig == null) throw new NullReferenceException("SsssConfig is null");
                //Log.Send($"[Exiled.API] {Name} is enabled By {Author} | Version: {Version}", LogLevel.Info, ConsoleColor.DarkRed);

                Log.Info($"[CHK] Blackout null? {Config?.ServerEventsMasterConfig?.BlackoutModeConfig == null}");
                Log.Info($"[CHK] Music null? {Config?.MusicConfig == null}");
                Log.Info($"[CHK] scp914 null? {Config?.Scp914Config == null}");
                Log.Info($"[CHK] Classic null? {Config?.ServerEventsMasterConfig?.ClassicConfig == null}");
                Log.Info($"[CHK] Ssss isEnabled? {Config?.SsssConfig?.IsEnabled}");
            });
            CurrentRunMode = RunModeResolver.Resolve();
            Log.Info($"[GhostPlugin] Mode: {CurrentRunMode}, IP: {Exiled.API.Features.Server.IpAddress}");
            Run("blackout.register", () =>
            {
                if (Config?.ServerEventsMasterConfig?.BlackoutModeConfig?.IsEnabled == true && CurrentRunMode == RunMode.Limited)
                    BlackoutMod.RegisterEvents();
            });

            Run("items.register", () =>
            {
                if (Config?.CustomItemsConfig?.IsEnabled == true && CurrentRunMode == RunMode.Full)
                {
                    CustomItemHandler = new CustomItemHandler(this);

                    // 여기서 하나라도 null이면 바로 터집니다. ?. 로 가드
                    var ci = Config.CustomItemsConfig;
                    ci.HackingDevices?.Register();
                    ci.Morses?.Register();
                    ci.ShockwaveGuns?.Register();
                    ci.FtacReacon?.Register();
                    ci.PoisonGuns?.Register();
                    ci.ClusterGrenades?.Register();
                    ci.TripleFlashGrenades?.Register();
                    ci.StunGrenades?.Register();
                    ci.Stims?.Register();
                    ci.BattleRages?.Register();
                    ci.Svds?.Register();
                    ci.EodPaddings?.Register();
                    ci.SmokeGrenades?.Register();
                    ci.GrenadeLaunchers?.Register();
                    ci.ParalyzeRifes?.Register();
                    ci.PlasmaEmitters?.Register();
                    ci.PlasmaShockwaveEmitters?.Register();
                    ci.PhotonCannons?.Register();
                    ci.ArmorPlateKits?.Register();
                    ci.ImpactGrenades?.Register();
                    ci.StickyGrenades?.Register();
                    ci.PlasmaShotguns?.Register();
                    ci.Bolters?.Register();
                    ci.AcidShooters?.Register();
                    ci.C4s?.Register();
                    ci.SpikeJailbirds?.Register();
                    ci.ReviveKits?.Register();
                    ci.PoisonGrenades?.Register();
                    ci.LaserCannons?.Register();
                    ci.Anti173s?.Register();
                    ci.Basilisks?.Register();
                    ci.AmmoBoxes?.Register();
                    ci.TrophySystems?.Register();
                    ci.OverkillVests?.Register();
                    ci.PlasmaBlasters?.Register();
                    ci.MachineGuns?.Register();
                    ci.Riveters?.Register();
                    ci.LaserGuns?.Register();
                    ci.MorsReworks?.Register();
                    ci.PortableEnergyShilds?.Register();
                    ci.M16s?.Register();
                    ci.ActiveArmors?.Register();
                    ci.JuggernautArmors?.Register();
                    ci.LowGravityGrenadeItems?.Register();
                    ci.He1s?.Register();

                    if (Config?.EnablePerkEvents == true && CurrentRunMode == RunMode.Full)
                    {
                        PerkEventHandlers = new PerkEventHandlers(this);
                        PerkEventHandlers.RegisterEvents();
                        ci.QuickfixPerks?.Register();
                        ci.FocusPerks?.Register();
                        ci.BoostOnKillPerks?.Register();
                        ci.MartydomPerks?.Register();
                        ci.EngineerPerks?.Register();
                        ci.OverkillPerks?.Register();
                        ci.EnhancedVisionPerks?.Register();
                        ci.BetralPerks?.Register();
                    }
                    Server.WaitingForPlayers += CustomItemHandler.OnWaitingForPlayers;
                    Exiled.Events.Handlers.Map.PickupAdded += CustomItemHandler.AddGlow;
                    Exiled.Events.Handlers.Map.PickupDestroyed += CustomItemHandler.RemoveGlow;
                    Server.RoundStarted += CustomItemHandler.OnRoundStarted;
                    Exiled.Events.Handlers.Item.InspectingItem += CustomItemHandler.OnInspectingItem;
                    Exiled.Events.Handlers.Item.InspectingItem += CustomItemHandler.OnInspectingItem;
                    Exiled.Events.Handlers.Player.ChangingItem += CustomItemHandler.OnChangingItem;
                }
            });
            Run("classic.register", () =>
            {
                if (Config.ServerEventsMasterConfig.ClassicConfig.OnEnabled && CurrentRunMode == RunMode.Limited)
                {
                    ClassicPlugin.RegisterEvents();
                }
            });
            Run("roles.register", () =>
            {
                if (Config?.CustomRolesConfig?.IsEnabled == true && CurrentRunMode == RunMode.Full)
                {
                    CustomRoleHandler = new CustomRoleHandler(this);

                    var cr = Config.CustomRolesConfig;
                    cr.ChiefScientists?.Register();
                    cr.CiPhantoms?.Register();
                    cr.DAlphas?.Register();
                    cr.Tanker106S?.Register();
                    cr.Vipers?.Register();
                    cr.Jailbirdmans?.Register();
                    cr.Administrators?.Register();
                    cr.FedoraAgents?.Register();
                    cr.Elites?.Register();
                    cr.JuggernautChaosList?.Register();
                    cr.Scp682s?.Register();
                    cr.SoleStealer049s?.Register();
                    cr.Scp049Aps?.Register();
                    cr.LuckyGuards?.Register();
                    cr.Gunslingers?.Register();
                    cr.Demolitionists?.Register();
                    cr.Dwarves?.Register();
                    cr.SpyAgents?.Register();
                    cr.Enforcers?.Register();
                    cr.Strategists?.Register();
                    cr.Quartermasters?.Register();
                    cr.Medics?.Register();
                    cr.AdvancedMtfs?.Register();
                    cr.Hunters?.Register();
                    cr.HugoBosses?.Register();
                    cr.Trackers?.Register();
                    cr.Directors?.Register();
                    cr.Commandos?.Register();
                    cr.ReconJuggernauts?.Register();
                    cr.DwarfZombies?.Register();
                    cr.ExplosiveZombies?.Register();
                    cr.EodSoldierZombies?.Register();
                    cr.ShockWaveZombies?.Register();
                    cr.ReinforceZombies?.Register();

                    foreach (CustomRole role in CustomRole.Registered)
                    {
                        if (role?.CustomAbilities != null)
                            foreach (var ability in role.CustomAbilities)
                                ability?.Register();

                        if (role is ICustomRole custom)
                        {
                            var team =
                                custom.StartTeam.HasFlag(StartTeam.Chaos) ? StartTeam.Chaos :
                                custom.StartTeam.HasFlag(StartTeam.Guard) ? StartTeam.Guard :
                                custom.StartTeam.HasFlag(StartTeam.Ntf) ? StartTeam.Ntf :
                                custom.StartTeam.HasFlag(StartTeam.Scientist) ? StartTeam.Scientist :
                                custom.StartTeam.HasFlag(StartTeam.ClassD) ? StartTeam.ClassD :
                                custom.StartTeam.HasFlag(StartTeam.Scp) ? StartTeam.Scp :
                                custom.StartTeam.HasFlag(StartTeam.Revived) ? StartTeam.Revived :
                                StartTeam.Other;

                            if (!Roles.ContainsKey(team)) Roles.Add(team, new());
                            uint limit = role.SpawnProperties?.Limit ?? 0; // ★ 널 가드
                            for (int i = 0; i < limit; i++) Roles[team].Add(custom);
                        }
                    }

                    Server.RoundStarted += CustomRoleHandler.OnRoundStarted;
                    Server.RespawningTeam += CustomRoleHandler.OnRespawningTeam;
                    Scp049Events.FinishingRecall += CustomRoleHandler.FinishingRecall;
                }
            });

            Run("abilities.register", () =>
            {
                if (Config?.CustomRolesAbilitiesConfig?.IsEnabled == true && CurrentRunMode == RunMode.Full)
                    CustomAbility.RegisterAbilities(false);
            });
            
            if (Config.ServerEventsMasterConfig.NoobSupportConfig.OnEnabled && CurrentRunMode == RunMode.Limited) {NoobSupport.RegisterEvents();}
            if (Config.Scp914Config.IsEnabled && CurrentRunMode == RunMode.Limited) {Scp914Handler.RegisterEvents();}

            Run("music.register", () =>
            {
                if (Config?.MusicConfig?.OnEnabled == true && CurrentRunMode == RunMode.Full)
                {
                    string tmpAudio =
                        Environment.OSVersion.Platform == PlatformID.Win32NT
                            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "EXILED", "Plugins", "tmp-audio")
                            : "/data/scpsl/tmp-audio";
                    if (!Directory.Exists(tmpAudio)) Directory.CreateDirectory(tmpAudio);

                    // 인스턴스 기반일 때
                    MusicEventHandlers = new MusicEventHandlers(this, tmpAudio);
                    MusicEventHandlers.RegisterEvents();
                }
            });

            Run("ssss.register", () =>
            {
                if (Config?.SsssConfig?.IsEnabled == true && CurrentRunMode == RunMode.Full)
                {
                    SsssEventHandler = new SsssEventHandler(this);
                    Server.RoundStarted += SsssEventHandler.OnRoundStarted;
                    Exiled.Events.Handlers.Player.Verified += SsssEventHandler.OnVerified;
                    ServerSpecificSettingsSync.ServerOnSettingValueReceived += SsssEventHandler.OnSettingValueReceived;
                }
            });

            Run("fpsmap.register", () =>
            {
                if (Config?.ServerEventsMasterConfig?.ClassicConfig?.IsEnableFPSmap == true && CurrentRunMode == RunMode.Full)
                {
                    CasualFPSModeHandler = new CasualFPSModeHandler(this);
                    CasualFPSModeHandler.RegisterEvents();
                }
            });
            
            Run("killsteak.register", () =>
            {
                if (Config?.ServerEventsMasterConfig?.KillStreakConfig?.Enabled == true &&
                    CurrentRunMode == RunMode.Full)
                {
                    KillStreakEventHandlers = new KillStreakEventHandlers();
                    Exiled.Events.Handlers.Player.Died += KillStreakEventHandlers.OnDied;
                    Server.RoundStarted +=  KillStreakEventHandlers.OnRoundStarted;
                    Server.RoundEnded += KillStreakEventHandlers.OnRoundEnded;
                }
            });

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            
            //BlackOut mode
            if(Config.ServerEventsMasterConfig.BlackoutModeConfig.IsEnabled && CurrentRunMode == RunMode.Limited){BlackoutMod.UnregisterEvents();}
            
            //CustItem
            if (Config.CustomItemsConfig.IsEnabled && CurrentRunMode == RunMode.Full)
            {
                CustomItem.UnregisterItems();
                Server.WaitingForPlayers -= CustomItemHandler.OnWaitingForPlayers;
                Exiled.Events.Handlers.Map.PickupAdded -= CustomItemHandler.AddGlow;
                Exiled.Events.Handlers.Map.PickupDestroyed -= CustomItemHandler.RemoveGlow;
                Server.RoundStarted -= CustomItemHandler.OnRoundStarted;
                Exiled.Events.Handlers.Item.InspectingItem -= CustomItemHandler.OnInspectingItem;
                Exiled.Events.Handlers.Item.InspectingItem -= CustomItemHandler.OnInspectingItem;
                Exiled.Events.Handlers.Player.ChangingItem -= CustomItemHandler.OnChangingItem;
                CustomItemHandler = null;
            }
            
            //ClassicPlugin
            if (Config.ServerEventsMasterConfig.ClassicConfig.OnEnabled && CurrentRunMode == RunMode.Limited)
            {
                ClassicPlugin.UnregisterEvents();
            }
            
            //Noob Support
            if (Config.ServerEventsMasterConfig.NoobSupportConfig.OnEnabled && CurrentRunMode == RunMode.Limited) {NoobSupport.UnregisterEvents();}
            //Scp914 Event
            if (Config.Scp914Config.IsEnabled) {Scp914Handler.UnregisterEvents();}
            //Music Event
            if (Config?.MusicConfig.OnEnabled == true)
            {
                MusicEventHandlers.UnregisterEvents();
            }

            //CustomRoles
            if (Config?.CustomRolesConfig.IsEnabled == true)
            {
                CustomRole.UnregisterRoles();
                Server.RoundStarted -= CustomRoleHandler.OnRoundStarted;
                Server.RespawningTeam -= CustomRoleHandler.OnRespawningTeam;
                Scp049Events.FinishingRecall -= CustomRoleHandler.FinishingRecall;
                CustomRoleHandler = null;
            }
            //Harmony
            //Harmony.UnpatchAll();
            
            //Legacy SSSS
            /*if (Config.ServerEventsMasterConfig.SsssConfig.IsEnabled)
            {
                _myCustomKeyBind = new MyCustomKeyBind();
                _myCustomKeyBind.Deactivate();
            }*/
            if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.IsEnableFPSmap && CurrentRunMode == RunMode.Full)
            {
                CasualFPSModeHandler.UnregisterEvents();
                CasualFPSModeHandler = null;
            }
            
            
            //PerkEventHandler
            if(Plugin.Instance?.Config.EnablePerkEvents == true && CurrentRunMode == RunMode.Full)
            {
                PerkEventHandlers.UnregisterEvents();
                PerkEventHandlers = null; 
            }
            
            if (Instance?.Config.ServerEventsMasterConfig?.KillStreakConfig?.Enabled == true &&
                CurrentRunMode == RunMode.Full)
            {
                Exiled.Events.Handlers.Player.Died -= KillStreakEventHandlers.OnDied;
                Server.RoundStarted -=  KillStreakEventHandlers.OnRoundStarted;
                Server.RoundEnded -= KillStreakEventHandlers.OnRoundEnded;
                KillStreakEventHandlers = null;
            }
            //SSSS - REWORK
            Server.RoundStarted -= SsssEventHandler.OnRoundStarted;
            Exiled.Events.Handlers.Player.Verified -= SsssEventHandler.OnVerified;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= SsssEventHandler.OnSettingValueReceived;
            SsssEventHandler = null;
            Instance = null;
            Server.WaitingForPlayers += OnWaitingPlayer;
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

        private void UpgradeToFullIfAllowed()
        {
            if (_fullUpgraded) return;

            var newMode = RunModeResolver.Resolve();
            if (newMode != RunMode.Full) return;

            CurrentRunMode = RunMode.Full;
            _fullUpgraded = true;
            Log.Warn("[GhostPlugin] Upgrading to FULL mode after IP resolved.");

            // 이제 Run() 함수로 감싸서 단계별 로그 찍기
            Run("items.register", () =>
            {
                if (Config?.CustomItemsConfig?.IsEnabled == true && CustomItemHandler == null)
                {
                    CustomItemHandler = new CustomItemHandler(this);
                    var ci = Config.CustomItemsConfig;
                    ci.HackingDevices?.Register();
                    ci.Morses?.Register();
                    ci.ShockwaveGuns?.Register();
                    ci.FtacReacon?.Register();
                    ci.PoisonGuns?.Register();
                    ci.ClusterGrenades?.Register();
                    ci.TripleFlashGrenades?.Register();
                    ci.StunGrenades?.Register();
                    ci.Stims?.Register();
                    ci.BattleRages?.Register();
                    ci.Svds?.Register();
                    ci.EodPaddings?.Register();
                    ci.SmokeGrenades?.Register();
                    ci.GrenadeLaunchers?.Register();
                    ci.ParalyzeRifes?.Register();
                    ci.PlasmaEmitters?.Register();
                    ci.PlasmaShockwaveEmitters?.Register();
                    ci.PhotonCannons?.Register();
                    ci.ArmorPlateKits?.Register();
                    ci.ImpactGrenades?.Register();
                    ci.StickyGrenades?.Register();
                    ci.PlasmaShotguns?.Register();
                    ci.Bolters?.Register();
                    ci.AcidShooters?.Register();
                    ci.C4s?.Register();
                    ci.SpikeJailbirds?.Register();
                    ci.ReviveKits?.Register();
                    ci.PoisonGrenades?.Register();
                    ci.LaserCannons?.Register();
                    ci.Anti173s?.Register();
                    ci.Basilisks?.Register();
                    ci.AmmoBoxes?.Register();
                    ci.TrophySystems?.Register();
                    ci.OverkillVests?.Register();
                    ci.PlasmaBlasters?.Register();
                    ci.MachineGuns?.Register();
                    ci.Riveters?.Register();
                    ci.LaserGuns?.Register();
                    ci.MorsReworks?.Register();
                    ci.PortableEnergyShilds?.Register();
                    ci.M16s?.Register();
                    ci.ActiveArmors?.Register();
                    ci.JuggernautArmors?.Register();
                    ci.LowGravityGrenadeItems?.Register();
                    ci.He1s?.Register();
                    ci.FrMg03S?.Register();
                    ci.CombatKnives?.Register();
                    ci.EnergyBlades?.Register();
                    ci.DoomBlades?.Register();
                    if (Config.EnablePerkEvents && PerkEventHandlers == null)
                    {
                        PerkEventHandlers = new PerkEventHandlers(this);
                        PerkEventHandlers.RegisterEvents();
                        ci.QuickfixPerks?.Register();
                        ci.FocusPerks?.Register();
                        ci.BoostOnKillPerks?.Register();
                        ci.MartydomPerks?.Register();
                        ci.EngineerPerks?.Register();
                        ci.OverkillPerks?.Register();
                        ci.EnhancedVisionPerks?.Register();
                        ci.BetralPerks?.Register();
                    }
                    Server.WaitingForPlayers += CustomItemHandler.OnWaitingForPlayers;
                    Exiled.Events.Handlers.Map.PickupAdded += CustomItemHandler.AddGlow;
                    Exiled.Events.Handlers.Map.PickupDestroyed += CustomItemHandler.RemoveGlow;
                    Server.RoundStarted += CustomItemHandler.OnRoundStarted;
                    Exiled.Events.Handlers.Item.InspectingItem += CustomItemHandler.OnInspectingItem;
                    Exiled.Events.Handlers.Item.InspectingItem += CustomItemHandler.OnInspectingItem;
                    Exiled.Events.Handlers.Player.ChangingItem += CustomItemHandler.OnChangingItem;
                }
            });

            Run("roles.register", () =>
            {
                if (Config?.CustomRolesConfig?.IsEnabled == true && CustomRoleHandler == null)
                {
                    CustomRoleHandler = new CustomRoleHandler(this);
                    var cr = Config.CustomRolesConfig;
                    cr.ChiefScientists?.Register();
                    cr.CiPhantoms?.Register();
                    cr.DAlphas?.Register();
                    cr.Tanker106S?.Register();
                    cr.Vipers?.Register();
                    cr.Jailbirdmans?.Register();
                    cr.Administrators?.Register();
                    cr.FedoraAgents?.Register();
                    cr.Elites?.Register();
                    cr.JuggernautChaosList?.Register();
                    cr.Scp682s?.Register();
                    cr.SoleStealer049s?.Register();
                    cr.Scp049Aps?.Register();
                    cr.LuckyGuards?.Register();
                    cr.Gunslingers?.Register();
                    cr.Demolitionists?.Register();
                    cr.Dwarves?.Register();
                    cr.SpyAgents?.Register();
                    cr.Enforcers?.Register();
                    cr.Strategists?.Register();
                    cr.Quartermasters?.Register();
                    cr.Medics?.Register();
                    cr.AdvancedMtfs?.Register();
                    cr.Hunters?.Register();
                    cr.HugoBosses?.Register();
                    cr.Trackers?.Register();
                    cr.Directors?.Register();
                    cr.Commandos?.Register();
                    cr.ReconJuggernauts?.Register();
                    cr.DwarfZombies?.Register();
                    cr.ExplosiveZombies?.Register();
                    cr.EodSoldierZombies?.Register();
                    cr.ShockWaveZombies?.Register();
                    cr.ReinforceZombies?.Register();
                    Server.RoundStarted += CustomRoleHandler.OnRoundStarted;
                    Server.RespawningTeam += CustomRoleHandler.OnRespawningTeam;
                    Scp049Events.FinishingRecall += CustomRoleHandler.FinishingRecall;
                }
                foreach (CustomRole role in CustomRole.Registered)
                {
                    if (role?.CustomAbilities != null)
                        foreach (var ability in role.CustomAbilities)
                            ability?.Register();

                    if (role is ICustomRole custom)
                    {
                        var team =
                            custom.StartTeam.HasFlag(StartTeam.Chaos) ? StartTeam.Chaos :
                            custom.StartTeam.HasFlag(StartTeam.Guard) ? StartTeam.Guard :
                            custom.StartTeam.HasFlag(StartTeam.Ntf) ? StartTeam.Ntf :
                            custom.StartTeam.HasFlag(StartTeam.Scientist) ? StartTeam.Scientist :
                            custom.StartTeam.HasFlag(StartTeam.ClassD) ? StartTeam.ClassD :
                            custom.StartTeam.HasFlag(StartTeam.Scp) ? StartTeam.Scp :
                            custom.StartTeam.HasFlag(StartTeam.Revived) ? StartTeam.Revived :
                            StartTeam.Other;

                        if (!Roles.ContainsKey(team)) Roles.Add(team, new());
                        uint limit = role.SpawnProperties?.Limit ?? 0; // ★ 널 가드
                        for (int i = 0; i < limit; i++) Roles[team].Add(custom);
                    }
                }
            });

            Run("abilities.register", () =>
            {
                if (Config?.CustomRolesAbilitiesConfig?.IsEnabled == true)
                    CustomAbility.RegisterAbilities(false);
            });

            Run("music.register", () =>
            {
                if (Config?.MusicConfig?.OnEnabled == true)
                {
                    string tmpAudio =
                        Environment.OSVersion.Platform == PlatformID.Win32NT
                            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                "EXILED", "Plugins", "tmp-audio")
                            : "/data/scpsl/tmp-audio";
                    if (!Directory.Exists(tmpAudio)) Directory.CreateDirectory(tmpAudio);

                    // 인스턴스 기반일 때
                    MusicEventHandlers = new MusicEventHandlers(this, tmpAudio);
                    MusicEventHandlers.RegisterEvents();
                }
            });

            Run("fpsmap.register", () =>
            {
                if (Config?.ServerEventsMasterConfig?.ClassicConfig?.IsEnableFPSmap == true &&
                    CasualFPSModeHandler == null)
                {
                    CasualFPSModeHandler = new CasualFPSModeHandler(this);
                    CasualFPSModeHandler.RegisterEvents();
                }
            });
            
            Run("killsteak.register", () =>
            {
                if (Config?.ServerEventsMasterConfig?.KillStreakConfig?.Enabled == true &&
                    CurrentRunMode == RunMode.Full)
                {
                    KillStreakEventHandlers = new KillStreakEventHandlers();
                    Exiled.Events.Handlers.Player.Died += KillStreakEventHandlers.OnDied;
                    Server.RoundStarted +=  KillStreakEventHandlers.OnRoundStarted;
                    Server.RoundEnded += KillStreakEventHandlers.OnRoundEnded;
                }
            });

            Run("ssss.register", () =>
            {
                if (Config?.SsssConfig?.IsEnabled == true && SsssEventHandler == null)
                {
                    SsssEventHandler = new SsssEventHandler(this);
                    Server.RoundStarted += SsssEventHandler.OnRoundStarted;
                    Exiled.Events.Handlers.Player.Verified += SsssEventHandler.OnVerified;
                    ServerSpecificSettingsSync.ServerOnSettingValueReceived += SsssEventHandler.OnSettingValueReceived;
                }
            });

            Log.Info("[GhostPlugin] All FULL features registered successfully!");
        }

        private void OnWaitingPlayer()
        {
            Log.Send($"Your IP is: {Exiled.API.Features.Server.IpAddress}", LogLevel.Info, ConsoleColor.Yellow);

            // 블랙리스트 서버는 셧다운
            if (Config.BlackListedIP.Contains(Exiled.API.Features.Server.IpAddress))
            {
                Log.Send("BLACKLISTED SERVER. Shutting down in 10 seconds...", LogLevel.Error, ConsoleColor.DarkRed);
                Timing.CallDelayed(10, Exiled.API.Features.Server.Shutdown);
                return;
            }

            // 허용된 IP면 Full로 승격
            UpgradeToFullIfAllowed();

            if (!Config.AllowedIP.Contains(Exiled.API.Features.Server.IpAddress))
                Log.Warn("NOT ALLOWED IP → Running Classic-only (Limited) mode.");
        }
    }
}
