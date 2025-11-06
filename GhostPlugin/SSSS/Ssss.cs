using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.Custom.Items.Armor;
using GhostPlugin.Custom.Items.Grenades;
using GhostPlugin.Custom.Items.Etc;
using GhostPlugin.Custom.Items.Firearms;
using GhostPlugin.Custom.Items.Keycard;
using GhostPlugin.Custom.Roles.Chaos;
using GhostPlugin.Custom.Roles.ClassD;
using GhostPlugin.Custom.Roles.Foundation;
using GhostPlugin.Custom.Roles.Scientist;
using GhostPlugin.Custom.Roles.Scps;
using NorthwoodLib.Pools;
using UnityEngine;
using UserSettings.ServerSpecific;
using Reinforcements.Roles;
using C_Squad.Roles;
using Exiled.API.Features;
using GhostPlugin.Custom.Items.Medkit;

namespace GhostPlugin.SSSS
{
    public class Ssss
    {
        public static ServerSpecificSettingBase[] GetMinimalMusicSetting()
        {
            List<ServerSpecificSettingBase> settings = new List<ServerSpecificSettingBase>();

            settings.Add(new SSGroupHeader("🎵 음악 재생 설정"));
            settings.Add(new SSTwoButtonsSetting(
                Plugin.Instance.Config.SsssConfig.MusicToggleId,
                "로비 및 이벤트 음악 설정",
                "듣기",
                "끄기",
                false,
                "음악 재생 여부를 설정합니다.",
                255,
                true
            ));
            settings.Add(new SSTwoButtonsSetting(Plugin.Instance.Config.SsssConfig.RoundStartRolesId, Plugin.Instance.Config.SsssConfig.RoundStartRolesSsssText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingEnabledText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingDisabledText));
            return settings.ToArray();
        }
        public static ServerSpecificSettingBase[] GetSettings()
        {
            List<ServerSpecificSettingBase> settings = new List<ServerSpecificSettingBase>();
            settings.Add(new SSGroupHeader($"{Plugin.Instance.Name} | Version {Plugin.Instance.Version}"));
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            if (Plugin.Instance.Config.CustomRolesConfig.IsEnabled)
            {
                var customRoles = new List<CustomRole>
                {
                    CiPhantom.Get(typeof(CiPhantom)),
                    FedoraAgent.Get(typeof(FedoraAgent)),
                    JuggernautChaos.Get(typeof(JuggernautChaos)),
                    Ninja.Get(typeof(Ninja)),
                    SpyAgent.Get(typeof(SpyAgent)),
                    D_Alpha.Get(typeof(D_Alpha)),
                    Dwarf.Get(typeof(Dwarf)),
                    Demolitionist.Get(typeof(Demolitionist)),
                    Elite.Get(typeof(Elite)),
                    Jailbirdman.Get(typeof(Jailbirdman)),
                    Ksk.Get(typeof(Ksk)),
                    Viper.Get(typeof(Viper)),
                    ChiefScientist.Get(typeof(ChiefScientist)),
                    O5Administrator.Get(typeof(O5Administrator)),
                    DwarfZombie.Get(typeof(DwarfZombie)),
                    EodSoldierZombie.Get(typeof(EodSoldierZombie)),
                    ExplosiveZombie.Get(typeof(ExplosiveZombie)),
                    SoleStealer049.Get(typeof(SoleStealer049)),
                    Scp049AP.Get(typeof(Scp049AP)),
                    Tanker106.Get(typeof(Tanker106)),
                    SniperAgent.Get(typeof(SniperAgent)),
                    LargeAgent.Get(typeof(LargeAgent)),
                    ReinforcementsCommander.Get(typeof(ReinforcementsCommander)),
                    Agent.Get(typeof(Agent)),
                    Specialist.Get(typeof(Specialist)),
                    Commander.Get(typeof(Commander)),
                    ShockWaveZombie.Get(typeof(ShockWaveZombie)),
                };
				stringBuilder.AppendLine("<size=40>CustomRoles - [특수직업]</size>");
                foreach (var role in customRoles)
                {
                    if (role == null || role.CustomAbilities == null) continue;

                    stringBuilder.AppendLine($"Role: {role.Name}");
                    stringBuilder.AppendLine($"- Description: {role.Description}");
                    foreach (var ability in role.CustomAbilities)
                    {
                        stringBuilder.AppendLine($"-- Ability: {ability.Name}, {ability.Description}");
                    }
                    stringBuilder.AppendLine(string.Empty);
                }
            }
            if (Plugin.Instance.Config.CustomItemsConfig.IsEnabled)
            {
                var customItems = new List<IEnumerable<CustomItem>>
                {
                    CustomItem.Get(typeof(AmmoBox)),
                    CustomItem.Get(typeof(TrophySystem)),
                    CustomItem.Get(typeof(EodPadding)),
                    CustomItem.Get(typeof(OverkillVest)),
                    CustomItem.Get(typeof(SmokeGrenade)),
                    CustomItem.Get(typeof(Anti173)),
                    CustomItem.Get(typeof(ArmorPlateKit)),
                    CustomItem.Get(typeof(BattleRage)),
                    CustomItem.Get(typeof(SpikeJailbird)),
                    CustomItem.Get(typeof(Stim)),
                    CustomItem.Get(typeof(ClusterGrenade)),
                    CustomItem.Get(typeof(C4)),
                    CustomItem.Get(typeof(GrenadeLauncher)),
                    CustomItem.Get(typeof(Basilisk)),
                    CustomItem.Get(typeof(Bolter)),
                    CustomItem.Get(typeof(LaserCannon)),
                    CustomItem.Get(typeof(MachineGun)),
                    CustomItem.Get(typeof(Mors)),
                    CustomItem.Get(typeof(ShockwaveGun)),
                    CustomItem.Get(typeof(Riveter)),
                    CustomItem.Get(typeof(ParalyzeRife)),
                    CustomItem.Get(typeof(PhotonCannon)),
                    CustomItem.Get(typeof(PlasmaEmitter)),
                    CustomItem.Get(typeof(PlasmaShockwaveEmitter)),
                    CustomItem.Get(typeof(PlasmaShotgun)),
                    CustomItem.Get(typeof(PoisonGun)),
                    CustomItem.Get(typeof(ReconBattleRife)),
                    CustomItem.Get(typeof(Svd)),
                    CustomItem.Get(typeof(ClusterGrenade)),
                    CustomItem.Get(typeof(PoisonGrenade)),
                    CustomItem.Get(typeof(SmokeGrenade)),
                    CustomItem.Get(typeof(StickyGrenade)),
                    CustomItem.Get(typeof(StunGrenade)),
                    CustomItem.Get(typeof(TripleFlashGrenade)),
                    CustomItem.Get(typeof(HackingDevice)),
                    CustomItem.Get(typeof(LaserCannon)),
                    CustomItem.Get(typeof(Ballista)),
                    CustomItem.Get(typeof(ReviveKit)),
                    CustomItem.Get(typeof(PlasmaBlaster))
                };
                stringBuilder.AppendLine("<size=40>CustomItem - [특수 아이탬]</size>");
                foreach (var itemCollection in customItems)
                {
                    if (itemCollection == null) continue;
                    foreach (var items in itemCollection)
                    {
                        stringBuilder.AppendLine($"Item: {items.Name}");
                        stringBuilder.AppendLine($"- Description: {items.Description}");
                        stringBuilder.AppendLine(string.Empty);
                    }
                    
                }

                settings.Add(new SSGroupHeader("Custom Items & Roles 정보"));
                settings.Add(new SSTextArea(null, StringBuilderPool.Shared.ToStringReturn(stringBuilder),
                    SSTextArea.FoldoutMode.CollapsedByDefault));
                stringBuilder.Clear();
            }

            if (Plugin.Instance.Config.CustomRolesConfig.IsEnabled &&
                Plugin.Instance.Config.CustomRolesAbilitiesConfig.IsEnabled)
            {
                settings.Add(new SSGroupHeader("특수능력 활성화 키"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ActiveCamoId, "Active Camo",
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ChargeId, "Charge", 
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.DetectId, "Detect", 
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.DoorPickingId, "Door Picking",
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.HealingMistId, "Healing Mist",
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.RemoveDisguiseId, "Remove Disguise",
                    KeyCode.B, true, false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.FocousId, "Focous",
                    KeyCode.B,true,false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.EnhanseVisionId, "Enhance Vision",
                    KeyCode.B,true,false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.GhostId,"Ghost",
                    KeyCode.B,true,false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.Scp457Id, "Scp457"
                    ,KeyCode.J,true,false,"J"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.Scp106Id,"Scp106",
                    KeyCode.X,true,false,"X"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ExplosionId,"Explosion",
                    KeyCode.I,true,false,"I"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.Speedy096Id,"Scp096",
                    KeyCode.Alpha9,true,false,"9"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ResupplyId,"Active Resupply",
                    KeyCode.B,true,false,"B"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.OverkillId, "Overkill Ability",
                    KeyCode.O,true,false,"O"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ShockwaveId,"Shockwave (충격파)",
                    KeyCode.B,true,false,"B"));
                settings.Add(new SSTwoButtonsSetting(Plugin.Instance.Config.SsssConfig.RespawnWaveRolesId, Plugin.Instance.Config.SsssConfig.RespawnWaveRolesSsssText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingEnabledText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingDisabledText));
                settings.Add(new SSTwoButtonsSetting(Plugin.Instance.Config.SsssConfig.Scp049ReviveRolesId, Plugin.Instance.Config.SsssConfig.Scp049ReviveRolesSsssText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingEnabledText, Plugin.Instance.Config.SsssConfig.CustomRoleReceivingDisabledText));
            }

            if (Plugin.Instance.Config.CustomItemsConfig.IsEnabled)
            {
                settings.Add(new SSGroupHeader("커스텀 아이탬 능력키"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.DetonateC4Id, "C4 폭발",
                    KeyCode.J, true, false,"무전기를 들고 커스텀 키바인드 키를 눌러 C4 를 폭파시킵니다!"));
            }

            if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.IsEnableFPSmap)
            {
                settings.Add(new SSGroupHeader("Map Toggle Key"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.MapToggleId,"미니맵 ON/OFF"
                    ,KeyCode.Y,true,false,"라운드 중 minimap 을 토글합니다."));
            }
            return settings.ToArray();
        }
        public static void SafeAppendSsssSettings()
        {
            var mySettings = GetSettings();
            var current = ServerSpecificSettingsSync.DefinedSettings?.ToList() ?? new List<ServerSpecificSettingBase>();
            bool needToAddSettings = mySettings.Any(setting => current.All(s => s.SettingId != setting.SettingId));
            if (needToAddSettings)
            {
                if (!current.Any(s => s is SSGroupHeader header && header.Label == Plugin.Instance.Config.SsssConfig.Header))
                {
                    current.Add(new SSGroupHeader(Plugin.Instance.Config.SsssConfig.Header));
                }
                foreach (var setting in mySettings)
                {
                    if (current.All(s => s.SettingId != setting.SettingId))
                        current.Add(setting);
                    else
                        Log.Debug($"CR SSSS: Skipped duplicate SettingId: {setting.SettingId}");
                }
        
                ServerSpecificSettingsSync.DefinedSettings = current.ToArray();
                Log.Debug($"CR SSSS: Appended settings. Total now: {current.Count}");
            }
        }
    }
}
