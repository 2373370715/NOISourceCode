using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010E3 RID: 4323
[AddComponentMenu("KMonoBehaviour/scripts/ColonyAchievementTracker")]
public class ColonyAchievementTracker : KMonoBehaviour, ISaveLoadableDetails, IRenderEveryTick
{
	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x0600584F RID: 22607 RVA: 0x000DE07C File Offset: 0x000DC27C
	// (set) Token: 0x06005850 RID: 22608 RVA: 0x000DE089 File Offset: 0x000DC289
	public bool GeothermalFacilityDiscovered
	{
		get
		{
			return (this.geothermalProgress & 1) == 1;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 1;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -2;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06005851 RID: 22609 RVA: 0x000DE0B8 File Offset: 0x000DC2B8
	// (set) Token: 0x06005852 RID: 22610 RVA: 0x000DE0C5 File Offset: 0x000DC2C5
	public bool GeothermalControllerRepaired
	{
		get
		{
			return (this.geothermalProgress & 2) == 2;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 2;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -3;
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06005853 RID: 22611 RVA: 0x000DE0F4 File Offset: 0x000DC2F4
	// (set) Token: 0x06005854 RID: 22612 RVA: 0x000DE101 File Offset: 0x000DC301
	public bool GeothermalControllerHasVented
	{
		get
		{
			return (this.geothermalProgress & 4) == 4;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 4;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -5;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06005855 RID: 22613 RVA: 0x000DE130 File Offset: 0x000DC330
	// (set) Token: 0x06005856 RID: 22614 RVA: 0x000DE13D File Offset: 0x000DC33D
	public bool GeothermalClearedEntombedVent
	{
		get
		{
			return (this.geothermalProgress & 8) == 8;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 8;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -9;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06005857 RID: 22615 RVA: 0x000DE16C File Offset: 0x000DC36C
	// (set) Token: 0x06005858 RID: 22616 RVA: 0x000DE17B File Offset: 0x000DC37B
	public bool GeothermalVictoryPopupDismissed
	{
		get
		{
			return (this.geothermalProgress & 16) == 16;
		}
		set
		{
			if (value)
			{
				this.geothermalProgress |= 16;
				return;
			}
			DebugUtil.DevAssert(value, "unsetting progress? why", null);
			this.geothermalProgress &= -17;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06005859 RID: 22617 RVA: 0x000DE1AB File Offset: 0x000DC3AB
	public List<string> achievementsToDisplay
	{
		get
		{
			return this.completedAchievementsToDisplay;
		}
	}

	// Token: 0x0600585A RID: 22618 RVA: 0x000DE1B3 File Offset: 0x000DC3B3
	public void ClearDisplayAchievements()
	{
		this.achievementsToDisplay.Clear();
	}

	// Token: 0x0600585B RID: 22619 RVA: 0x00297538 File Offset: 0x00295738
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (!this.achievements.ContainsKey(colonyAchievement.Id))
			{
				ColonyAchievementStatus value = new ColonyAchievementStatus(colonyAchievement.Id);
				this.achievements.Add(colonyAchievement.Id, value);
			}
		}
		this.forceCheckAchievementHandle = Game.Instance.Subscribe(395452326, new Action<object>(this.CheckAchievements));
		base.Subscribe<ColonyAchievementTracker>(631075836, ColonyAchievementTracker.OnNewDayDelegate);
		this.UpgradeTamedCritterAchievements();
	}

	// Token: 0x0600585C RID: 22620 RVA: 0x002975FC File Offset: 0x002957FC
	private void UpgradeTamedCritterAchievements()
	{
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in Db.Get().ColonyAchievements.TameAllBasicCritters.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits = colonyAchievementRequirement as CritterTypesWithTraits;
			if (critterTypesWithTraits != null)
			{
				critterTypesWithTraits.UpdateSavedState();
			}
		}
		foreach (ColonyAchievementRequirement colonyAchievementRequirement2 in Db.Get().ColonyAchievements.TameAGassyMoo.requirementChecklist)
		{
			CritterTypesWithTraits critterTypesWithTraits2 = colonyAchievementRequirement2 as CritterTypesWithTraits;
			if (critterTypesWithTraits2 != null)
			{
				critterTypesWithTraits2.UpdateSavedState();
			}
		}
	}

	// Token: 0x0600585D RID: 22621 RVA: 0x002976BC File Offset: 0x002958BC
	public void RenderEveryTick(float dt)
	{
		if (this.updatingAchievement >= this.achievements.Count)
		{
			this.updatingAchievement = 0;
		}
		KeyValuePair<string, ColonyAchievementStatus> keyValuePair = this.achievements.ElementAt(this.updatingAchievement);
		this.updatingAchievement++;
		if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
		{
			keyValuePair.Value.UpdateAchievement();
			if (keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
				this.completedAchievementsToDisplay.Add(keyValuePair.Key);
				this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				RetireColonyUtility.SaveColonySummaryData();
			}
		}
	}

	// Token: 0x0600585E RID: 22622 RVA: 0x0029777C File Offset: 0x0029597C
	private void CheckAchievements(object data = null)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (!keyValuePair.Value.success && !keyValuePair.Value.failed)
			{
				keyValuePair.Value.UpdateAchievement();
				if (keyValuePair.Value.success && !keyValuePair.Value.failed)
				{
					ColonyAchievementTracker.UnlockPlatformAchievement(keyValuePair.Key);
					this.completedAchievementsToDisplay.Add(keyValuePair.Key);
					this.TriggerNewAchievementCompleted(keyValuePair.Key, null);
				}
			}
		}
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x0600585F RID: 22623 RVA: 0x00297844 File Offset: 0x00295A44
	private static void UnlockPlatformAchievement(string achievement_id)
	{
		if (DebugHandler.InstantBuildMode)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: instant build mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (SaveGame.Instance.sandboxEnabled)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: sandbox mode", new object[]
			{
				achievement_id
			});
			return;
		}
		if (Game.Instance.debugWasUsed)
		{
			global::Debug.LogWarningFormat("UnlockPlatformAchievement {0} skipping: debug was used.", new object[]
			{
				achievement_id
			});
			return;
		}
		ColonyAchievement colonyAchievement = Db.Get().ColonyAchievements.Get(achievement_id);
		if (colonyAchievement != null && !string.IsNullOrEmpty(colonyAchievement.platformAchievementId))
		{
			if (SteamAchievementService.Instance)
			{
				SteamAchievementService.Instance.Unlock(colonyAchievement.platformAchievementId);
				return;
			}
			global::Debug.LogWarningFormat("Steam achievement [{0}] was achieved, but achievement service was null", new object[]
			{
				colonyAchievement.platformAchievementId
			});
		}
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x000DE1C0 File Offset: 0x000DC3C0
	public void DebugTriggerAchievement(string id)
	{
		this.achievements[id].failed = false;
		this.achievements[id].success = true;
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x00297908 File Offset: 0x00295B08
	private void BeginVictorySequence(string achievementID)
	{
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
		this.ToggleVictoryUI(true);
		StoryMessageScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.StoryMessageScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<StoryMessageScreen>();
		component.restoreInterfaceOnClose = false;
		component.title = COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_HEADER;
		component.body = string.Format(COLONY_ACHIEVEMENTS.PRE_VICTORY_MESSAGE_BODY, "<b>" + Db.Get().ColonyAchievements.Get(achievementID).Name + "</b>\n" + Db.Get().ColonyAchievements.Get(achievementID).description);
		component.Show(true);
		CameraController.Instance.SetWorldInteractive(false);
		component.OnClose = (System.Action)Delegate.Combine(component.OnClose, new System.Action(delegate()
		{
			SpeedControlScreen.Instance.SetSpeed(1);
			if (!SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			CameraController.Instance.SetWorldInteractive(true);
			Db.Get().ColonyAchievements.Get(achievementID).victorySequence(this);
		}));
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x00297A4C File Offset: 0x00295C4C
	public bool IsAchievementUnlocked(ColonyAchievement achievement)
	{
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			if (keyValuePair.Key == achievement.Id)
			{
				if (keyValuePair.Value.success)
				{
					return true;
				}
				keyValuePair.Value.UpdateAchievement();
				return keyValuePair.Value.success;
			}
		}
		return false;
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x000DE1E6 File Offset: 0x000DC3E6
	protected override void OnCleanUp()
	{
		this.victorySchedulerHandle.ClearScheduler();
		Game.Instance.Unsubscribe(this.forceCheckAchievementHandle);
		this.checkAchievementsHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005864 RID: 22628 RVA: 0x00297ADC File Offset: 0x00295CDC
	private void TriggerNewAchievementCompleted(string achievement, GameObject cameraTarget = null)
	{
		this.unlockedAchievementMetric[ColonyAchievementTracker.UnlockedAchievementKey] = achievement;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedAchievementMetric, "TriggerNewAchievementCompleted");
		bool flag = false;
		if (Db.Get().ColonyAchievements.Get(achievement).isVictoryCondition)
		{
			flag = true;
			this.BeginVictorySequence(achievement);
		}
		if (!flag)
		{
			AchievementEarnedMessage message = new AchievementEarnedMessage();
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x06005865 RID: 22629 RVA: 0x00297B48 File Offset: 0x00295D48
	private void ToggleVictoryUI(bool victoryUIActive)
	{
		List<KScreen> list = new List<KScreen>();
		list.Add(NotificationScreen.Instance);
		list.Add(OverlayMenu.Instance);
		if (PlanScreen.Instance != null)
		{
			list.Add(PlanScreen.Instance);
		}
		if (BuildMenu.Instance != null)
		{
			list.Add(BuildMenu.Instance);
		}
		list.Add(ManagementMenu.Instance);
		list.Add(ToolMenu.Instance);
		list.Add(ToolMenu.Instance.PriorityScreen);
		list.Add(ResourceCategoryScreen.Instance);
		list.Add(TopLeftControlScreen.Instance);
		list.Add(global::DateTime.Instance);
		list.Add(BuildWatermark.Instance);
		list.Add(HoverTextScreen.Instance);
		list.Add(DetailsScreen.Instance);
		list.Add(DebugPaintElementScreen.Instance);
		list.Add(DebugBaseTemplateButton.Instance);
		list.Add(StarmapScreen.Instance);
		foreach (KScreen kscreen in list)
		{
			if (kscreen != null)
			{
				kscreen.Show(!victoryUIActive);
			}
		}
	}

	// Token: 0x06005866 RID: 22630 RVA: 0x00297C78 File Offset: 0x00295E78
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.achievements.Count);
		foreach (KeyValuePair<string, ColonyAchievementStatus> keyValuePair in this.achievements)
		{
			writer.WriteKleiString(keyValuePair.Key);
			keyValuePair.Value.Serialize(writer);
		}
	}

	// Token: 0x06005867 RID: 22631 RVA: 0x00297CF0 File Offset: 0x00295EF0
	public void Deserialize(IReader reader)
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 10))
		{
			return;
		}
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = reader.ReadKleiString();
			ColonyAchievementStatus value = ColonyAchievementStatus.Deserialize(reader, text);
			if (Db.Get().ColonyAchievements.Exists(text))
			{
				this.achievements.Add(text, value);
			}
		}
	}

	// Token: 0x06005868 RID: 22632 RVA: 0x00297D58 File Offset: 0x00295F58
	public void LogFetchChore(GameObject fetcher, ChoreType choreType)
	{
		if (choreType == Db.Get().ChoreTypes.StorageFetch || choreType == Db.Get().ChoreTypes.BuildFetch || choreType == Db.Get().ChoreTypes.RepairFetch || choreType == Db.Get().ChoreTypes.FoodFetch || choreType == Db.Get().ChoreTypes.Transport)
		{
			return;
		}
		Dictionary<int, int> dictionary = null;
		if (fetcher.GetComponent<SolidTransferArm>() != null)
		{
			dictionary = this.fetchAutomatedChoreDeliveries;
		}
		else if (fetcher.GetComponent<MinionIdentity>() != null)
		{
			dictionary = this.fetchDupeChoreDeliveries;
		}
		if (dictionary != null)
		{
			int cycle = GameClock.Instance.GetCycle();
			if (!dictionary.ContainsKey(cycle))
			{
				dictionary.Add(cycle, 0);
			}
			Dictionary<int, int> dictionary2 = dictionary;
			int key = cycle;
			int num = dictionary2[key];
			dictionary2[key] = num + 1;
		}
	}

	// Token: 0x06005869 RID: 22633 RVA: 0x000DE214 File Offset: 0x000DC414
	public void LogCritterTamed(Tag prefabId)
	{
		this.tamedCritterTypes.Add(prefabId);
	}

	// Token: 0x0600586A RID: 22634 RVA: 0x00297E24 File Offset: 0x00296024
	public void LogSuitChore(ChoreDriver driver)
	{
		if (driver == null || driver.GetComponent<MinionIdentity>() == null)
		{
			return;
		}
		bool flag = false;
		foreach (AssignableSlotInstance assignableSlotInstance in driver.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().IsAnyPrefabID(ColonyAchievementTracker.SuitTags))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			int cycle = GameClock.Instance.GetCycle();
			int instanceID = driver.GetComponent<KPrefabID>().InstanceID;
			if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
			{
				this.dupesCompleteChoresInSuits.Add(cycle, new List<int>
				{
					instanceID
				});
				return;
			}
			if (!this.dupesCompleteChoresInSuits[cycle].Contains(instanceID))
			{
				this.dupesCompleteChoresInSuits[cycle].Add(instanceID);
			}
		}
	}

	// Token: 0x0600586B RID: 22635 RVA: 0x000DE223 File Offset: 0x000DC423
	public void LogAnalyzedSeed(Tag seed)
	{
		this.analyzedSeeds.Add(seed);
	}

	// Token: 0x0600586C RID: 22636 RVA: 0x00297F2C File Offset: 0x0029612C
	public void OnNewDay(object data)
	{
		foreach (MinionStorage minionStorage in Components.MinionStorages.Items)
		{
			if (minionStorage.GetComponent<CommandModule>() != null)
			{
				List<MinionStorage.Info> storedMinionInfo = minionStorage.GetStoredMinionInfo();
				if (storedMinionInfo.Count > 0)
				{
					int cycle = GameClock.Instance.GetCycle();
					if (!this.dupesCompleteChoresInSuits.ContainsKey(cycle))
					{
						this.dupesCompleteChoresInSuits.Add(cycle, new List<int>());
					}
					for (int i = 0; i < storedMinionInfo.Count; i++)
					{
						KPrefabID kprefabID = storedMinionInfo[i].serializedMinion.Get();
						if (kprefabID != null)
						{
							this.dupesCompleteChoresInSuits[cycle].Add(kprefabID.InstanceID);
						}
					}
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = Db.Get().ColonyAchievements.SurviveInARocket.requirementChecklist[0] as SurviveARocketWithMinimumMorale;
			if (surviveARocketWithMinimumMorale != null)
			{
				float minimumMorale = surviveARocketWithMinimumMorale.minimumMorale;
				int numberOfCycles = surviveARocketWithMinimumMorale.numberOfCycles;
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					if (worldContainer.IsModuleInterior)
					{
						if (!this.cyclesRocketDupeMoraleAboveRequirement.ContainsKey(worldContainer.id))
						{
							this.cyclesRocketDupeMoraleAboveRequirement.Add(worldContainer.id, 0);
						}
						if (worldContainer.GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Grounded)
						{
							List<MinionIdentity> worldItems = Components.MinionIdentities.GetWorldItems(worldContainer.id, false);
							bool flag = worldItems.Count > 0;
							foreach (MinionIdentity cmp in worldItems)
							{
								if (Db.Get().Attributes.QualityOfLife.Lookup(cmp).GetTotalValue() < minimumMorale)
								{
									flag = false;
									break;
								}
							}
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = (flag ? (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] + 1) : 0);
						}
						else if (this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] < numberOfCycles)
						{
							this.cyclesRocketDupeMoraleAboveRequirement[worldContainer.id] = 0;
						}
					}
				}
			}
		}
	}

	// Token: 0x04003E32 RID: 15922
	public Dictionary<string, ColonyAchievementStatus> achievements = new Dictionary<string, ColonyAchievementStatus>();

	// Token: 0x04003E33 RID: 15923
	[Serialize]
	public Dictionary<int, int> fetchAutomatedChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04003E34 RID: 15924
	[Serialize]
	public Dictionary<int, int> fetchDupeChoreDeliveries = new Dictionary<int, int>();

	// Token: 0x04003E35 RID: 15925
	[Serialize]
	public Dictionary<int, List<int>> dupesCompleteChoresInSuits = new Dictionary<int, List<int>>();

	// Token: 0x04003E36 RID: 15926
	[Serialize]
	public HashSet<Tag> tamedCritterTypes = new HashSet<Tag>();

	// Token: 0x04003E37 RID: 15927
	[Serialize]
	public bool defrostedDuplicant;

	// Token: 0x04003E38 RID: 15928
	[Serialize]
	public HashSet<Tag> analyzedSeeds = new HashSet<Tag>();

	// Token: 0x04003E39 RID: 15929
	[Serialize]
	public float totalMaterialsHarvestFromPOI;

	// Token: 0x04003E3A RID: 15930
	[Serialize]
	public float radBoltTravelDistance;

	// Token: 0x04003E3B RID: 15931
	[Serialize]
	public bool harvestAHiveWithoutGettingStung;

	// Token: 0x04003E3C RID: 15932
	[Serialize]
	public Dictionary<int, int> cyclesRocketDupeMoraleAboveRequirement = new Dictionary<int, int>();

	// Token: 0x04003E3D RID: 15933
	[Serialize]
	public bool efficientlyGatheredData;

	// Token: 0x04003E3E RID: 15934
	[Serialize]
	public bool fullyBoostedBionic;

	// Token: 0x04003E3F RID: 15935
	[Serialize]
	private int geothermalProgress;

	// Token: 0x04003E40 RID: 15936
	private const int GEO_DISCOVERED_BIT = 1;

	// Token: 0x04003E41 RID: 15937
	private const int GEO_CONTROLLER_REPAIRED_BIT = 2;

	// Token: 0x04003E42 RID: 15938
	private const int GEO_CONTROLLER_VENTED_BIT = 4;

	// Token: 0x04003E43 RID: 15939
	private const int GEO_CLEARED_ENTOMBED_BIT = 8;

	// Token: 0x04003E44 RID: 15940
	private const int GEO_VICTORY_ACK_BIT = 16;

	// Token: 0x04003E45 RID: 15941
	private SchedulerHandle checkAchievementsHandle;

	// Token: 0x04003E46 RID: 15942
	private int forceCheckAchievementHandle = -1;

	// Token: 0x04003E47 RID: 15943
	[Serialize]
	private int updatingAchievement;

	// Token: 0x04003E48 RID: 15944
	[Serialize]
	private List<string> completedAchievementsToDisplay = new List<string>();

	// Token: 0x04003E49 RID: 15945
	private SchedulerHandle victorySchedulerHandle;

	// Token: 0x04003E4A RID: 15946
	public static readonly string UnlockedAchievementKey = "UnlockedAchievement";

	// Token: 0x04003E4B RID: 15947
	private Dictionary<string, object> unlockedAchievementMetric = new Dictionary<string, object>
	{
		{
			ColonyAchievementTracker.UnlockedAchievementKey,
			null
		}
	};

	// Token: 0x04003E4C RID: 15948
	private static readonly Tag[] SuitTags = new Tag[]
	{
		GameTags.AtmoSuit,
		GameTags.JetSuit,
		GameTags.LeadSuit
	};

	// Token: 0x04003E4D RID: 15949
	private static readonly EventSystem.IntraObjectHandler<ColonyAchievementTracker> OnNewDayDelegate = new EventSystem.IntraObjectHandler<ColonyAchievementTracker>(delegate(ColonyAchievementTracker component, object data)
	{
		component.OnNewDay(data);
	});
}
