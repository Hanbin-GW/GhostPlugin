using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
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
        public void OnVerified(VerifiedEventArgs ev)
        { 
            if (Plugin.Instance.SsssEventHandler == null)
                return;
            if (!Plugin.Instance.Config.SsssConfig.IsEnabled)
                   return;
            if (!Round.IsStarted)
            {
                try
                {
                    ServerSpecificSettingsSync.DefinedSettings = Ssss.GetMinimalMusicSetting();
                    ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
                }
                catch (InvalidCastException ex)
                {
                    Log.Error($"VVUP: InvalidCastException occurred: {ex.Message}");
                }
            }
            
            else
            {
                Log.Debug($"VVUP: Adding SSSS functions to {ev.Player.Nickname}");
                try
                {
                    ServerSpecificSettingsSync.DefinedSettings = Ssss.GetSettings();
                    ServerSpecificSettingsSync.SendToPlayer(ev.Player.ReferenceHub);
                }
                catch (InvalidCastException ex)
                {
                    Log.Error($"VVUP: InvalidCastException occurred: {ex.Message}");
                }
            }
        }

        /*public void OnWaitingForPlayers()
        {
            if (!Plugin.Instance.Config.SsssConfig.IsEnabled)
                return;

            foreach (PlayerAPI player in Player.List)
            {
                Log.Debug($"VVUP: Sending SSSS settings early to {player.Nickname}");
                try
                {
                    ServerSpecificSettingsSync.DefinedSettings = Ssss.GetMinimalMusicSetting();
                    ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);
                }
                catch (InvalidCastException ex)
                {
                    Log.Error($"VVUP: InvalidCastException occurred: {ex.Message}");
                }
            }
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
            
            if (settingBase is SSTwoButtonsSetting musicSetting &&
                musicSetting.SettingId == Plugin.Instance.Config.SsssConfig.MusicToggleId)
            {
                if (!Player.TryGet(hub, out player)) return;

                if (musicSetting.SyncIsA)
                {
                    Plugin.Instance.musicDisabledPlayers[player.Id] = false;
                    player.ShowHint("<color=green>🎵 Turn on the music</color>", 2f);
                }
                else if (musicSetting.SyncIsB)
                {
                    Plugin.Instance.musicDisabledPlayers[player.Id] = true;
                    player.ShowHint("<color=red>🔇 Turn off the music</color>", 2f);
                }
                return;
            }
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
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Scp457Id
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Scp106Id
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ExplosionId
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Speedy096Id
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.MapToggleId
                     || ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ResupplyId)
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
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.MapToggleId)
                    {
                        if (!Plugin.Instance.miniMapEnabled.ContainsKey(player.Id))
                            Plugin.Instance.miniMapEnabled[player.Id] = true;
                        else
                            Plugin.Instance.miniMapEnabled[player.Id] = !Plugin.Instance.miniMapEnabled[player.Id];
                        
                        player.ShowHint(Plugin.Instance.miniMapEnabled[player.Id]
                            ? "<color=green>Minimap ON</color>"
                            : "<color=red>Minimap OFF</color>", 2f);
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
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Scp106Id)
                    {
                        var scp106Ability = abilities.FirstOrDefault(ability => ability.GetType() == typeof(SCP106));
                        if (scp106Ability != null && scp106Ability.CanUseAbility(player, out response))
                        {
                            scp106Ability.SelectAbility(player);
                            scp106Ability.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.Scp457ActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ExplosionId)
                    {
                        var scpexAbility = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Explotion));
                        if (scpexAbility != null && scpexAbility.CanUseAbility(player, out response))
                        {
                            scpexAbility.SelectAbility(player);
                            scpexAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.Scp457ActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.Speedy096Id)
                    {
                        var scp096Ability = abilities.FirstOrDefault(ability => ability.GetType() == typeof(Speedy096));
                        if (scp096Ability != null && scp096Ability.CanUseAbility(player, out response))
                        {
                            scp096Ability.SelectAbility(player);
                            scp096Ability.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.Scp457ActivationMessage);
                        }
                        else
                        {
                            player.ShowHint(response);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ResupplyId)
                    {
                        var resupplyAbility =
                            abilities.FirstOrDefault(ability => ability.GetType() == typeof(Resupply));
                        if (resupplyAbility != null && resupplyAbility.CanUseAbility(player, out response))
                        {
                            resupplyAbility.SelectAbility(player);
                            resupplyAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.ResupplyActivatMessage);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.OverkillId)
                    {
                        var overkillAbility =
                            abilities.FirstOrDefault(ability => ability.GetType() == typeof(Overkill));
                        if (overkillAbility != null && overkillAbility.CanUseAbility(player, out response))
                        {
                            overkillAbility.SelectAbility(player);
                            overkillAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.OverkillActivationMessage);
                        }
                    }
                    else if (ssKeybindSetting.SettingId == Plugin.Instance.Config.SsssConfig.ShockwaveId)
                    {
                        var shockwaveAbility = 
                            abilities.FirstOrDefault(ability => ability.GetType() == typeof(Shockwave));
                        if (shockwaveAbility != null && shockwaveAbility.CanUseAbility(player, out response))
                        {
                            shockwaveAbility.SelectAbility(player);
                            shockwaveAbility.UseAbility(player);
                            player.ShowHint(Plugin.Instance.Config.SsssConfig.ShockwaveActivateMessage);
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
                            player.SendConsoleMessage($"It fell too far from the explosives. you need to go closely {Mathf.Round(distance - C4.Instance.MaxDistance)}M !.", "yellow");
                        }
                    } 
                    player.ShowHint(Plugin.Instance.Config.SsssConfig.SsssDetonateC4ActivationMessage);
                    //string response = i == 1 ? $"\n<color=green>{i} C4 charge has been detonated!</color>" : $"\n<color=green>{i} C4 charges have been detonated!</color>"; player.SendConsoleMessage(response, "green");
                }
            }
        }
    }
}