using System.Collections.Generic;
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

namespace GhostPlugin.SSSS
{
    public class Ssss
    {
        public static ServerSpecificSettingBase[] GetMinimalMusicSetting()
        {
            List<ServerSpecificSettingBase> settings = new List<ServerSpecificSettingBase>();

            settings.Add(new SSGroupHeader("🎵 Music playback settings"));
            settings.Add(new SSTwoButtonsSetting(
                Plugin.Instance.Config.SsssConfig.MusicToggleId,
                "Lobby and Event Music Settings",
                "Listen",  // Option A
                "Turn off",  // Option B
                false,   // 기본값: 듣기
                "Set whether to play music." // 설명
            ));

            return settings.ToArray();
        }
        public static ServerSpecificSettingBase[] GetSettings()
        {
            List<ServerSpecificSettingBase> settings = new List<ServerSpecificSettingBase>();
            settings.Add(new SSGroupHeader($"{Plugin.Instance.Name} | Version {Plugin.Instance.Version}"));
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            settings.Add(new SSGroupHeader(Plugin.Instance.Config.SsssConfig.Header));
            if (Plugin.Instance.Config.CustomRolesConfig.IsEnabled)
            {
                var customRoles = new List<CustomRole>
                {
                    CustomRole.Get(typeof(CiPhantom)),
                    CustomRole.Get(typeof(FedoraAgent)),
                    CustomRole.Get(typeof(JuggernautChaos)),
                    CustomRole.Get(typeof(Ninja)),
                    CustomRole.Get(typeof(SpyAgent)),
                    CustomRole.Get(typeof(D_Alpha)),
                    CustomRole.Get(typeof(Dwarf)),
                    CustomRole.Get(typeof(Sniper)),
                    CustomRole.Get(typeof(Demolitionist)),
                    CustomRole.Get(typeof(Elite)),
                    CustomRole.Get(typeof(Jailbirdman)),
                    CustomRole.Get(typeof(Ksk)),
                    CustomRole.Get(typeof(Viper)),
                    CustomRole.Get(typeof(ChiefScientist)),
                    CustomRole.Get(typeof(O5Administrator)),
                    CustomRole.Get(typeof(DwarfZombie)),
                    CustomRole.Get(typeof(EodSoldierZombie)),
                    CustomRole.Get(typeof(ExplosiveZombie)),
                    CustomRole.Get(typeof(SoleStealer049)),
                    CustomRole.Get(typeof(Scp049AP)),
                    CustomRole.Get(typeof(Tanker106)),
                    CustomRole.Get(typeof(SniperAgent)),
                    CustomRole.Get(typeof(LargeAgent)),
                    CustomRole.Get(typeof(ReinforcementsCommander)),
                    CustomRole.Get(typeof(Agent)),
                    CustomRole.Get(typeof(Specialist)),
                    CustomRole.Get(typeof(Commander)),
                };
				stringBuilder.AppendLine("<size=40>CustomRoles - [CustomRole]</size>");
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
                    CustomItem.Get(typeof(ExplosiveRoundRevolver)),
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
                };
                stringBuilder.AppendLine("<size=40>CustomItem - [CustomItems]</size>");
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
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.ShockwaveId,"Shockwave",
                    KeyCode.B,true,false,"B"));
            }


            if (Plugin.Instance.Config.CustomItemsConfig.IsEnabled)
            {
                settings.Add(new SSGroupHeader("CustomItem Ability Key"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.DetonateC4Id, "Detonate C4",
                    KeyCode.J, true, false,"Take the walkie-talkie and press the custom keybind key to Detonate C4!"));
            }

            if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.IsEnableFPSmap)
            {
                settings.Add(new SSGroupHeader("Map Toggle Key"));
                settings.Add(new SSKeybindSetting(Plugin.Instance.Config.SsssConfig.MapToggleId,"Minimap ON/OFF"
                    ,KeyCode.Y,true,false,"Toggle the minimap during the round."));
            }
            return settings.ToArray();
        }
    }
}
