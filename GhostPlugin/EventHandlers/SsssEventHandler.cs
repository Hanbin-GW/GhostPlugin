using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using GhostPlugin.Custom.Abilities.Active;
using GhostPlugin.Custom.Items.Grenades;
using GhostPlugin.SSSS;
using UnityEngine;
using UserSettings.ServerSpecific;
using PlayerAPI = Exiled.API.Features.Player;

namespace GhostPlugin.EventHandlers
{
    public class SsssEventHandler
    {
        public Plugin Plugin;
        public SsssEventHandler(Plugin plugin) => Plugin = plugin;
        /*public SsssEventHandler(Plugin plugin)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin), " Plugin 객체가 null입니다!");
            }

            Plugin = plugin;

            if (Plugin.Config == null)
            {
                throw new NullReferenceException(" Plugin.Config가 null입니다!");
            }

            if (Plugin.Config.SsssConfig == null)
            {
                throw new NullReferenceException(" Plugin.Config.SsssConfig가 null입니다!");
            }

            Log.Debug("✅ SsssEventHandler가 정상적으로 생성되었습니다.");
        }*/

        public void OnRoundStarted()
        {
            if (!Plugin.Instance.Config.SsssConfig.IsEnabled)
                return;
            
            foreach (PlayerAPI player in Player.List)
            {
                Log.Debug($"VVUP: Adding SSSS functions to {player.Nickname}");
                try
                {
                    ServerSpecificSettingsSync.DefinedSettings = Ssss.GetSettings();
                    ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);
                }
                catch (InvalidCastException ex)
                {
                    Log.Error($"VVUP: InvalidCastException occurred: {ex.Message}");
                }
            }
        }

        public void OnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (Plugin.Instance == null)
            {
                Log.Error("Plugin.Instance is null in SsssEventHandler!");
                return;
            }

            if (Plugin.Instance.Config?.SsssConfig == null)
            {
                Log.Error("SsssConfig is null in SsssEventHandler!");
                return;
            }
            
            if (Plugin.Instance.Config.SsssConfig.IsEnabled == false)
                return;
            
            if (!Player.TryGet(hub, out Player player))
                return;

            if (hub == null)
                return;
            
            if (player == null)
                return;
            if (settingBase is SSKeybindSetting ssKeybindSetting && ssKeybindSetting.SyncIsPressed)
            {
                if ((ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ActiveCamoId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ChargeId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.DetectId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.DoorPickingId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.HealingMistId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.RemoveDisguiseId 
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.FocousId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.EnhanseVisionId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.GhostId
                    || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Scp457Id)
                    && ActiveAbility.AllActiveAbilities.TryGetValue(player, out var abilities))
                {
                    string response = String.Empty;
                    if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ActiveCamoId)
                    {
                        var activeCamoAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(ActiveCamo));
                        if (activeCamoAbility != null && activeCamoAbility.CanUseAbility(player, out response))
                        {
                            activeCamoAbility.SelectAbility(player);
                            activeCamoAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssActiveCamoActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ChargeId)
                    {
                        var chargeAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(ChargeAbility));
                        if (chargeAbility != null && chargeAbility.CanUseAbility(player, out response))
                        {
                            chargeAbility.SelectAbility(player);
                            chargeAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssChargeActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.DetectId)
                    {
                        var detectAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Detect));
                        if (detectAbility != null && detectAbility.CanUseAbility(player, out response))
                        {
                            detectAbility.SelectAbility(player);
                            detectAbility.UseAbility(player);

                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if(ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.FocousId)
                    {
                        var focousAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Focous));
                        if (focousAbility != null && focousAbility.CanUseAbility(player, out response))
                        {
                            focousAbility.SelectAbility(player);
                            focousAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.FocousActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.EnhanseVisionId)
                    {
                        var enhanceVisionAbility =
                            abilities.FirstOrDefault(ability => ability.GetType() == typeof(EnhancedGoggleVision));
                        if (enhanceVisionAbility != null && enhanceVisionAbility.CanUseAbility(player, out response))
                        {
                            enhanceVisionAbility.SelectAbility(player);
                            enhanceVisionAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.EnhanseVisionActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.GhostId)
                    {
                        var ghostAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Ghost));
                        if (ghostAbility != null && ghostAbility.CanUseAbility(player, out response))
                        {
                            ghostAbility.SelectAbility(player);
                            ghostAbility.UseAbility(player);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Scp457Id)
                    {
                        var scp457Ability = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Scp457));
                        if (scp457Ability != null && scp457Ability.CanUseAbility(player, out response))
                        {
                            scp457Ability.SelectAbility(player);
                            scp457Ability.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.Scp457ActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                }
                else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.DetonateC4Id)
                {
                    if (!C4.PlacedCharges.ContainsValue(player))
                    {
                        player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssC4NoC4Deployed);
                        player.SendConsoleMessage("\n<color=red>You've haven't placed any C4 charges!</color>", "red"); 
                        return;
                    }
                    if (C4.Instance.RequireDetonator 
                        && (player.CurrentItem is null || player.CurrentItem.Type !=
                            C4.Instance.DetonatorItem))
                    {
                        player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssC4DetonatorNeeded);
                        player.SendConsoleMessage($"\n<color=red>You need to have a Remote Detonator ({C4.Instance.DetonatorItem}) in your hand to detonate C4!</color>", "red"); 
                        return;
                    } 
                    int i = 0;
                    foreach (var charge in C4.PlacedCharges.ToList())
                    {
                        if (charge.Value != player) 
                            continue; 
                        float distance = Vector3.Distance(charge.Key.Position, player.Position);
                        if (distance < C4.Instance.MaxDistance)
                        {
                            C4.Instance.C4Handler(charge.Key); i++;
                        }
                        else
                        {
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssC4TooFarAway);
                            player.SendConsoleMessage($"너무 멀리 폭발물로부터 떨어졌습니다. {Mathf.Round(distance - C4.Instance.MaxDistance)} 미터 만큼 가까이 가셔야 합니다!.", "yellow");
                        }
                    } 
                    player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssDetonateC4ActivationMessage);
                    //string response = i == 1 ? $"\n<color=green>{i} C4 charge has been detonated!</color>" : $"\n<color=green>{i} C4 charges have been detonated!</color>"; player.SendConsoleMessage(response, "green");
                }
            }
        }
    }
}