using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp096;
using InventorySystem.Items.Jailbird;
using InventorySystem.Items.MicroHID;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace GhostPlugin.EventHandlers
{
  public class NoobSupport
  {
    public static Plugin Plugin;
    public static void RegisterEvents()
    {
      Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
      Exiled.Events.Handlers.Player.Dying += OnDying;
      Exiled.Events.Handlers.Player.ChangingItem += OnEquipMicroHid;
      Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
      Exiled.Events.Handlers.Player.Hurting += OnHurting;
      Exiled.Events.Handlers.Scp096.AddingTarget += OnLookingAtScp096;
      Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpSCP207;
      Exiled.Events.Handlers.Map.AnnouncingDecontamination += OnAnnouncingDecontemination;
    }
    
    public static void UnregisterEvents()
    {
      Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
      Exiled.Events.Handlers.Player.Dying -= OnDying;
      Exiled.Events.Handlers.Player.ChangingItem -= OnEquipMicroHid;
      Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
      Exiled.Events.Handlers.Player.Hurting -= OnHurting;
      Exiled.Events.Handlers.Scp096.AddingTarget -= OnLookingAtScp096;
      Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpSCP207;
      Exiled.Events.Handlers.Map.AnnouncingDecontamination -= OnAnnouncingDecontemination;
    }
    private static void OnChangingRole(ChangingRoleEventArgs ev)
    {
      if (ev.Player.Role == (Role) null)
        return;
      /*if (ev.Player.Role == RoleTypeId.Scp079)
      {
        if (ev.Player.SessionVariables.ContainsKey("079_LockPoints"))
          ev.Player.SessionVariables["079_LockPoints"] = (object) 100f;
        else
          ev.Player.SessionVariables.Add("079_LockPoints", (object) 100f);
        Log.Info("SCP-079 spawned with initial Lockdown Points: 100.");
      }*/
      if (ev.Player.Role == RoleTypeId.CustomRole)
        Timing.CallDelayed(15,
          () => ev.Player.ShowHint(new string('\n',10) + "당신은 일반유저와 다른 <color=green>특수직업</color>을 부여받았습니다!\n`.ri`명령어를 쳐서 자세한 정보를 알수 있습니다!",
            5));
      if (ev.NewRole == RoleTypeId.Scp049)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Scp049SpawnMessage), 5f);
      if (ev.NewRole == RoleTypeId.ClassD)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.DclassSpawnMessage), 5f);
      if (ev.NewRole == RoleTypeId.Scp0492)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Scp0492SpawnMessage), 5f);
      if (ev.NewRole == RoleTypeId.Scientist)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ScientistSpawnMessage), 5f);
      if (ev.NewRole == RoleTypeId.FacilityGuard)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.FacilityGuardSpawnMessage), 5f);
      if (ev.NewRole == RoleTypeId.ChaosConscript || ev.NewRole == RoleTypeId.ChaosMarauder || ev.NewRole == RoleTypeId.ChaosRepressor || ev.NewRole == RoleTypeId.ChaosRifleman)
        ev.Player.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ChaosInsurgencySpawnMessage), 5f);
      if ((ev.NewRole != RoleTypeId.NtfCaptain) && (ev.NewRole != RoleTypeId.NtfPrivate) && (ev.NewRole != RoleTypeId.NtfSergeant) && ev.NewRole != RoleTypeId.NtfSpecialist)
        return;
      ev.Player.ShowHint(new string('\n', 10) + Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.NtfSpawnMessage, 5f);
    }

    private static void OnDying(DyingEventArgs ev)
    {
      if (ev.Attacker == null || ev.Player == null)
        Log.Warn("Attacker is null in OnDying event.");
      else if (Plugin.Instance.Config == null)
      {
        Log.Warn("Config is null in OnDying event.");
      }
      else
      {
        if (ev.Attacker.Role.Team != Team.SCPs)
          return;
        if (ev.Attacker.Role.Type == RoleTypeId.Scp173)
        {
          int amount = UnityEngine.Random.Range(0, 80);
          ev.Attacker.Heal((float) amount);
          ev.Attacker.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ScpHealMessage, (object) amount), 5f);
        }
        if (ev.Attacker.Role.Type == RoleTypeId.Scp096)
        {
          int amount = UnityEngine.Random.Range(0, 30);
          ev.Attacker.Heal((float) amount);
          ev.Attacker.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ScpHealMessage, (object) amount), 5f);
        }
        if (ev.Attacker.Role.Type == RoleTypeId.Scp049)
        {
          int amount = UnityEngine.Random.Range(0, 75);
          ev.Attacker.Heal((float) amount);
          ev.Attacker.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ScpHealMessage, (object) amount), 5f);
        }
        if (ev.Attacker.Role.Type != RoleTypeId.Scp939)
          return;
        int amount1 = UnityEngine.Random.Range(0, 60);
        ev.Attacker.Heal((float) amount1);
        ev.Attacker.ShowHint(new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ScpHealMessage, (object) amount1), 5f);
      }
    }
    
    private static void OnEquipMicroHid(ChangingItemEventArgs ev)
    {
      if (!Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.ShowHintOnEquipItem || ev.Item == null)
        return;
      if (ev.Item.Base is MicroHIDItem microHidItem)
      {
        float num = (float) Math.Round((double) microHidItem.EnergyManager.Energy * 100.0, 1);
        if ((double) num < 5.0)
          ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.MicroHidLowEnergyMessage) + "</color>", 2f);
        else
          ev.Player.ShowHint("<color=#4169E1>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.MicroHidEnergyMessage, (object) num) + "</color>", 2f);
      }
      if (ev.Item.Type == ItemType.GrenadeHE || ev.Item.Type == ItemType.GrenadeFlash || ev.Item.Type == ItemType.SCP018 || ev.Item.Type == ItemType.SCP2176)
        ev.Player.ShowHint(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.GrenadeMessage, 4f);
      if (ev.Item.Type != ItemType.Adrenaline)
        return;
      ev.Player.ShowHint(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.AdrenalineMessage, 4f);
    }

    private static void OnChargingJailbird(ChargingJailbirdEventArgs ev)
    {
      if (ev.Item == null || !(ev.Item.Base is JailbirdItem jailbirdItem))
        return;
      int num = 5 - jailbirdItem.TotalChargesPerformed;
      if (num > 1)
        ev.Player.ShowHint("<color=#00B7EB>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.JailbirdUseMessage, (object) num) + "</color>", 2f);
      else
        ev.Player.ShowHint("<color=#C73804>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.JailbirdUseMessage, (object) num) + "</color>", 2f);
    }

    private static void OnHurting(HurtingEventArgs ev)
    {
      switch (ev.DamageHandler.Type)
      {
        case DamageType.Poison:
          ev.Player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.PoisonMessage) + "</color>");
          break;
        case DamageType.Bleeding:
          ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.BleedingMessage) + "</color>");
          break;
        case DamageType.PocketDimension:
          ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.PocketDimensionMessage) + "</color>");
          break;
        case DamageType.SeveredHands:
          ev.Player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.PoisonMessage) + "</color>");
          break;
        case DamageType.CardiacArrest:
          ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.CardiacArrestMessage) + "</color>");
          break;
        case DamageType.A7:
          ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.A7Info) + "</color>");
          break;
      }
      if ((double) ev.Player.Health > 20.0 || !ev.Player.IsHuman)
        return;
      ev.Player.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.LowHpMessage) + "</color>");
    }

    private static void OnLookingAtScp096(AddingTargetEventArgs ev)
    {
      ev.Target.Broadcast((ushort) 5, "<color=red>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Looking096) + "</color>");
    }

    private static void OnPickingUpSCP207(PickingUpItemEventArgs ev)
    {
      if (ev.Pickup.Type == ItemType.SCP207)
      {
        StatusEffectBase statusEffectBase = ev.Player.ActiveEffects.FirstOrDefault<StatusEffectBase>((Func<StatusEffectBase, bool>) (effect => effect.GetEffectType() == EffectType.Scp207));
        if ((UnityEngine.Object) statusEffectBase != (UnityEngine.Object) null)
          ev.Player.ShowHint("<color=#A60C0E>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Scp207HintMessage, (object) statusEffectBase.Intensity) + "</color>", 4f);
      }
      if (ev.Pickup.Type == ItemType.AntiSCP207)
      {
        StatusEffectBase statusEffectBase = ev.Player.ActiveEffects.FirstOrDefault<StatusEffectBase>((Func<StatusEffectBase, bool>) (effect => effect.GetEffectType() == EffectType.AntiScp207));
        if ((UnityEngine.Object) statusEffectBase != (UnityEngine.Object) null)
          ev.Player.ShowHint("<color=#C53892>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.AntiScp207HintMessage, (object) statusEffectBase.Intensity) + "</color>", 4f);
      }
      if (ev.Pickup.Type != ItemType.Adrenaline)
        return;
      ev.Player.ShowHint("<color=#eeff6b>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.AdrenalineMessage) + "</color>", 4f);
    }

    private static void OnAnnouncingDecontemination(AnnouncingDecontaminationEventArgs ev)
    {
      switch (ev.Id)
      {
        case 0:
          if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage)
            BroadCastLcZinPlayers(0);
          else
            Exiled.API.Features.Map.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz15_hint) + "</color>", 10f);
          Log.Debug("Announcing LCZ Decontemination T-minus 15 min");
          break;
        case 1:
          if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage)
            BroadCastLcZinPlayers(1);
          else
            Exiled.API.Features.Map.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz15_hint) + "</color>", 10f);
          Log.Debug("Announcing LCZ Decontemination T-minus 10 min");
          break;
        case 2:
          if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage)
            BroadCastLcZinPlayers(2);
          else
            Exiled.API.Features.Map.ShowHint("<color=yellow>" + new string('\n', 10) + string.Format(Plugin.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz5_hint) + "</color>", 10f);
          Log.Debug("Announcing LCZ Decontemination T-minus 5 min");
          break;
        case 3:
          if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage)
            BroadCastLcZinPlayers(3);
          else
            Exiled.API.Features.Map.ShowHint("<color=orange>" + new string('\n', 10) + string.Format(Plugin.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz1_hint) + "</color>", 10f);
          Log.Debug("Announcing LCZ Decontemination T-minus 1 min");
          break;
        case 4:
          if (Plugin.Instance.Config.ServerEventsMasterConfig.ClassicConfig.OnlyLcZinMessage)
            BroadCastLcZinPlayers(4);
          else
            Exiled.API.Features.Map.ShowHint("<color=red>" + new string('\n', 10) + string.Format(Plugin.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz30s_hint) + "</color>", 10f);
          Log.Debug("Announcing LCZ Decontemination T-minus 30 sec");
          break;
      }
    }

    private static void BroadCastLcZinPlayers(int a)
    {
      foreach (Exiled.API.Features.Player player1 in (IEnumerable<Exiled.API.Features.Player>) Exiled.API.Features.Player.List)
      {
        Exiled.API.Features.Player player = player1;
        if ((double) player.Position.y < 20.0 && (double) player.Position.y > -1.0 && player.IsAlive && player.Role != RoleTypeId.None)
        {
          switch (a)
          {
            case 0:
              Timing.CallDelayed((float) Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.DelayLczMessageTime, (Action) (() => player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz15_hint) + "</color>", 10f)));
              continue;
            case 1:
              Timing.CallDelayed((float) Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.DelayLczMessageTime, (Action) (() => player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz10_hint) + "</color>", 10f)));
              continue;
            case 2:
              Timing.CallDelayed((float) Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.DelayLczMessageTime, (Action) (() => player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz5_hint) + "</color>", 10f)));
              continue;
            case 3:
              Timing.CallDelayed((float) Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.DelayLczMessageTime, (Action) (() => player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz1_hint) + "</color>", 10f)));
              continue;
            case 4:
              player.ShowHint("<color=green>" + new string('\n', 10) + string.Format(Plugin.Instance.Config.ServerEventsMasterConfig.NoobSupportConfig.Lcz30s_hint) + "</color>", 10f);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }
}
