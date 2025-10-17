/*using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using InventorySystem.Items;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using UnityEngine;
using UserSettings.ServerSpecific;
using UserSettings.ServerSpecific.Examples;

namespace GhostPlugin.SSSS
{
	/// <summary>
	/// This example shows an ability to organize longer lists of entries by introducing a page selector.
	/// <para /> This example uses auto-generated IDs, since it doesn't provide additional functionality, and reliability of saving isn't important here.
	/// </summary>
	public class SpecificMenu : SSExampleImplementationBase
	{
		/// <inheritdoc />
		public override string Name => "Multiple pages demo";
		private const float TacticalMeleeDamage = 25f;
		private const float MeleeRange = 2.5f;
		private const float MeleeCooldown = 2.0f;
		private const float TacticalStaminaDrainRate = 0.5f;
		private SSDropdownSetting _pageSelectorDropdown;
		private const byte BoostIntensity = 40;
		private ServerSpecificSettingBase[] _pinnedSection;
		private SettingsPage[] _pages;
		private Dictionary<ReferenceHub, int> _lastSentPages;
		private Dictionary<ReferenceHub, Dictionary<ExampleId, float>> lastAbilityUsage = new Dictionary<ReferenceHub, Dictionary<ExampleId, float>>();
		private static HashSet<ReferenceHub> _activeSpeedBoosts;

		private enum ExampleId
		{
			TacticalSmash,
			TacticalSprint,
			TacticalSprintToggle,
		}

		/// <inheritdoc />
		public override void Activate()
		{
			_activeSpeedBoosts = new HashSet<ReferenceHub>();
			_lastSentPages = new Dictionary<ReferenceHub, int>();

			_pages = new SettingsPage[]
			{
				new SettingsPage("Advanced Abilities", new ServerSpecificSettingBase[]
				{
					new SSGroupHeader("Advanced Abilities"),
					new SSKeybindSetting((int)ExampleId.TacticalSprint, "전력질주 부스트 (Human-only)", KeyCode.Y, hint: "<color=yellow>스태미나</color> 을 더 빨리 소모하여 속도를 높이세요."),
					new SSTwoButtonsSetting((int)ExampleId.TacticalSprintToggle, "전력질주 부스트 - 활성화 모드", "Hold", "Toggle"),
					new SSKeybindSetting((int)ExampleId.TacticalSmash,"Tactical Smash",KeyCode.Z,hint:$"총기의 개머리판으로 적의 면상을 후려치십시요! {TacticalMeleeDamage} Damage"),
					
				}),

				new SettingsPage("Update Log", new ServerSpecificSettingBase[]
				{
					new SSTextArea(null,"• <color=green>전력질주 부스트</color> 의 이속 이 60% 에서 40% 로 감소\n• <color=green>독성수류탄</color>이 많은 위치에서 스폰됩니다\n• 더이상 충격시 폭발되지 않으며, 충격후 4.5초뒤에 폭발합니다.\n<size=12><align=left>2025-1-31</align> <align=right>Ghost-Plugin 1.4.5</align></size>"),
					new SSTextArea(null,"• <color=blue>Containment SpecialList</color>, <color=#008f1e>Juggernaut Chaos</color> 가 추가 되었습니다!\n(더 많은 정보는 고스트서버 위키에서 참고 바랍니다)<size=12><align=left>2025-1-30</align> <align=right>Ghost-Plugin 1.4.3</align></size>"),
					new SSTextArea(null,"• <color=blue>MORS (벨런스 조정됨)</color>, <color=#6600CC>Obscures Veil-5</color>, Paralyze Rife(재작업) 가 재추가 되었습니다!\n(더 많은 정보는 고스트서버 위키에서 참고 바랍니다)<size=10><align=left>2025-1-10</align> <align=right>Ghost-Plugin 1.3.6</align></size>"),
					new SSTextArea(null,"• 특수진영의 ID 가 다 재설정되었습니다!\n• 많은 특수직업이 제거 또는 재작업중입니다. \n• <color=red>베틀레인지</color> & <color=green>전투 자극제</color> 가 추가되었습니다!\n\n<size=10><align=left>2025-1-10</align><align=right>Server Specific Ghost 1.3.4</align></size>"),
					new SSTextArea(null,"• <color=blue>Elite Agent</color> 의 특수능력이 변경되었습니다! (집중 --> 향상된 고글 비전)\n• Senior Guard 가 다시 추가되었습니다!\n• 안티 173 아이탬이 추가되었습니다.\n\n<size=10><align=right>CustomRoles-lite 1.0.3\n2024-12-16</align></size>"),
					new SSTextArea(null,"• 대부분의 특수직업 & 일부의 특수아이탬이 <color=red>재정비</color>상태로 돌입했습니다..\n\n<size=10><align=right>CustomRoles-lite 1.0.2\n2024-12-16</align></size>"),
					new SSTextArea(null,"• 전술 백병전의 쿨다운 시간이 0.5초 감소하였습니다! (2.5초 --> 2.0초)\n• 전술 전력질수 스태미나 사용양이 매우 많이 감소하였습니다 (3.0 --> 0.5)\n• <color=red>접근 거부됨 (문제 발생)</color>\n\n<size=15><align=left>2024-12-11</align><align=right>Server Specific Ghost 1.3.4</align></size>"),
				}),
				
				
			};


			string[] dropdownPageOptions = new string[_pages.Length];

			for (int i = 0; i < dropdownPageOptions.Length; i++)
				dropdownPageOptions[i] = $"{_pages[i].Name} ({i + 1} out of {_pages.Length})";

			_pinnedSection = new ServerSpecificSettingBase[]
			{
				_pageSelectorDropdown = new SSDropdownSetting(null, "Page", dropdownPageOptions, entryType: SSDropdownSetting.DropdownEntryType.HybridLoop),
			};

			_pages.ForEach(page => page.GenerateCombinedEntries(_pinnedSection));

			// All settings must be included in DefinedSettings, even if we're only sending a small part at the time.
			List<ServerSpecificSettingBase> allSettings = new(_pinnedSection);
			_pages.ForEach(page => allSettings.AddRange(page.OwnEntries));
			ServerSpecificSettingsSync.DefinedSettings = allSettings.ToArray();

			// We're technically sending ALL settings here, but clients will immediately send back the response which will allow us to re-send only the portion they're interested in.
			// You can optimize this process by only sending the page selector, but I didn't want to complicate this example more than it needs to.
			ServerSpecificSettingsSync.SendToAll();
			StaticUnityMethods.OnUpdate += OnUpdate;
			ReferenceHub.OnPlayerRemoved += OnPlayerDisconnected;
			ServerSpecificSettingsSync.ServerOnSettingValueReceived += ServerOnSettingValueReceived;
			ServerSpecificSettingsSync.ServerOnSettingValueReceived += ProcessUserInput;
		}

		/// <inheritdoc />
		public override void Deactivate()
		{
			StaticUnityMethods.OnUpdate -= OnUpdate;
			ReferenceHub.OnPlayerRemoved -= OnPlayerDisconnected;
			ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ServerOnSettingValueReceived;
			ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ProcessUserInput;
		}
		private void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase setting)
		{
			switch ((ExampleId)setting.SettingId)
			{
				case ExampleId.TacticalSmash
					when setting is SSKeybindSetting keybinding:
				{
					if (keybinding.SyncIsPressed)
						Smash(sender);
				}
					break;

				case ExampleId.TacticalSprint
					when setting is SSKeybindSetting keybind:
				{
					bool toggleMode = ServerSpecificSettingsSync
						.GetSettingOfUser<SSTwoButtonsSetting>(sender, (int)ExampleId.TacticalSprintToggle).SyncIsB;

					if (toggleMode)
					{
						if (!keybind.SyncIsPressed)
							break;

						SetHealBoost(sender, !_activeSpeedBoosts.Contains(sender));
					}
					else
					{
						SetHealBoost(sender, keybind.SyncIsPressed);
					}
				}
					break;
			}
		}
		
		private void SetHealBoost(ReferenceHub hub, bool state)
		{
			MovementBoost statusEffect = hub.playerEffectsController.GetEffect<MovementBoost>();
			
			if (statusEffect == null)
				return; 
			
			if (state && hub.IsHuman())
			{
				statusEffect.ServerSetState(BoostIntensity);
				_activeSpeedBoosts.Add(hub);
			}
			else
			{
				statusEffect.ServerDisable();
				_activeSpeedBoosts.Remove(hub);
			}
		}
		
		private void OnUpdate()
		{
			if (!StaticUnityMethods.IsPlaying)
				return;

			foreach (ReferenceHub hub in _activeSpeedBoosts)
			{
				if (Mathf.Approximately(hub.GetVelocity().SqrMagnitudeIgnoreY(), 0))
					continue; // Prevent damage when stationary.
				var stamina = hub.playerStats.GetModule<StaminaStat>();
				//float staminaDrainRate = TacticalStaminaDrainRate * Time.deltaTime;
				if (stamina != null)
				{
					float staminaDrainRate = TacticalStaminaDrainRate * Time.deltaTime;
					stamina.ModifyAmount(-staminaDrainRate);
				}
				//stamina.ModifyAmount(-staminaDrainRate);
			}
		}
		private bool IsAbilityOnCooldown(ReferenceHub hub, ExampleId ability, float cooldown)
		{
			if (!lastAbilityUsage.ContainsKey(hub))
				lastAbilityUsage[hub] = new Dictionary<ExampleId, float>();

			if (lastAbilityUsage[hub].TryGetValue(ability, out float lastUsageTime))
			{
				float elapsedTime = Time.time - lastUsageTime;
				if (elapsedTime < cooldown)
				{
					Log.Debug($"{hub.nicknameSync.MyNick} tried to use {ability}, but it's on cooldown ({cooldown - elapsedTime:F1}s remaining).");
					return true;
				}
			}

			lastAbilityUsage[hub][ability] = Time.time; // 쿨다운 갱신
			return false;
		}
		private void Smash(ReferenceHub hub)
		{
			if (IsAbilityOnCooldown(hub, ExampleId.TacticalSmash, MeleeCooldown))
				return;

			Vector3 origin = hub.PlayerCameraReference.position;
			Vector3 forward = hub.PlayerCameraReference.forward;
			Player player = Player.Get(hub);
			ItemIdentifier heldItem = hub.inventory.CurItem;
			var firearmTypes = new HashSet<ItemType>
			{
				ItemType.GunCOM15,
				ItemType.GunE11SR,
				ItemType.GunAK,
				ItemType.GunLogicer,
				ItemType.GunE11SR,
				ItemType.GunCrossvec,
				ItemType.GunFSP9,
				ItemType.GunShotgun,
				ItemType.GunRevolver,
				ItemType.GunA7,
				ItemType.ParticleDisruptor,
				ItemType.GunFRMG0,
			};

			if (!firearmTypes.Contains(heldItem.TypeId))
			{
				player.ShowHint($"{heldItem.TypeId} 로 근접 공격을 수행할 수 없습니다\n오직 총기로만 근접 공격이 가능합니다!!", 3);
				Log.Debug($"{hub.nicknameSync.MyNick} tried to perform a melee attack with {heldItem.TypeId}, which is not a firearm.");
				return;
			}

			if (!Physics.Raycast(origin, forward, out RaycastHit hit, MeleeRange))
			{
				Log.Debug($"{hub.nicknameSync.MyNick} missed the melee attack!");
				player.ShowHint("백병전 공격에 실패하였습니다...");
				return;
			}

			if (!hit.collider.TryGetComponent(out HitboxIdentity hitbox) || hitbox.TargetHub == null)
			{
				Log.Debug($"{hub.nicknameSync.MyNick} didn't hit a valid player.");
				
				return;
			}

			ReferenceHub target = hitbox.TargetHub;

			if (HitboxIdentity.IsEnemy(target, hub))
			{
				target.playerStats.DealDamage(new UniversalDamageHandler(TacticalMeleeDamage, DeathTranslations.Crushed));
				player.ShowHitMarker();
				Log.Debug($"{hub.nicknameSync.MyNick} hit {target.nicknameSync.MyNick} for {TacticalMeleeDamage} damage!");
				player.ShowHint($"<color=green>You hit <b>{target.nicknameSync.MyNick}</b> for <b>{TacticalMeleeDamage}</b> damage!</color>", 3);
			}
			else
			{
				Log.Debug($"{hub.nicknameSync.MyNick} tried to attack a teammate.");
			}
		}
		
		private void ServerOnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase setting)
		{
			if (setting is SSDropdownSetting dropdown && dropdown.SettingId == _pageSelectorDropdown.SettingId)
			{
				ServerSendSettingsPage(hub, dropdown.SyncSelectionIndexValidated);
			}
		}

		private void ServerSendSettingsPage(ReferenceHub hub, int settingIndex)
		{
			// Client automatically re-sends values of all the field after reception of the settings collection.
			// This can result in triggering this event, so we want to save the previously sent value to avoid going into infinite loops.
			if (_lastSentPages.TryGetValue(hub, out int prevSent) && prevSent == settingIndex)
				return; 

			_lastSentPages[hub] = settingIndex;
			ServerSpecificSettingsSync.SendToPlayer(hub, _pages[settingIndex].CombinedEntries);
		}

		private void OnPlayerDisconnected(ReferenceHub hub)
		{
			_lastSentPages?.Remove(hub);
			_activeSpeedBoosts.Remove(hub);
		}

		/// <summary>
		/// Represents a collection of settings that can be displayed one at the time.
		/// </summary>
		private class SettingsPage
		{
			/// <summary>
			/// Name of the collection, used to gennerate a header and dropdown label.
			/// </summary>
			public readonly string Name;

			/// <summary>
			/// Entries included on this page.
			/// </summary>
			public readonly ServerSpecificSettingBase[] OwnEntries;

			/// <summary>
			/// Entries to include when this page is selected. Combines the pinned page selector with its own entry.
			/// </summary>
			public ServerSpecificSettingBase[] CombinedEntries { get; private set; }

			/// <summary>
			/// Creates a new page of settings.
			/// </summary>
			/// <param name="name">Name that will be used to identify the setting.</param>
			/// <param name="entries">List of all entries included on this page.</param>
			public SettingsPage(string name, ServerSpecificSettingBase[] entries)
			{
				Name = name;
				OwnEntries = entries;
			}

			/// <summary>
			/// Generates the combined list of entries, which includes the page selector section, page name header, and the actual setting entries.
			/// </summary>
			public void GenerateCombinedEntries(ServerSpecificSettingBase[] pageSelectorSection)
			{
				int combinedLength = pageSelectorSection.Length + OwnEntries.Length + 1; // +1 to accomodate for auto-generated name header.
				CombinedEntries = new ServerSpecificSettingBase[combinedLength];

				int nextIndex = 0;

				// Include page selector section.
				foreach (ServerSpecificSettingBase entry in pageSelectorSection)
					CombinedEntries[nextIndex++] = entry;

				// Add auto-generated name header.
				CombinedEntries[nextIndex++] = new SSGroupHeader(Name);

				// Include own entries.
				foreach (ServerSpecificSettingBase entry in OwnEntries)
					CombinedEntries[nextIndex++] = entry;
			}
		}
	}
}
*/