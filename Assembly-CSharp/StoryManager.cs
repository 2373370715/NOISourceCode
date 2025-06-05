using System;
using System.Collections.Generic;
using Database;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

// Token: 0x02001A0B RID: 6667
[SerializationConfig(MemberSerialization.OptIn)]
public class StoryManager : KMonoBehaviour
{
	// Token: 0x17000918 RID: 2328
	// (get) Token: 0x06008ACB RID: 35531 RVA: 0x000FF4F3 File Offset: 0x000FD6F3
	// (set) Token: 0x06008ACC RID: 35532 RVA: 0x000FF4FA File Offset: 0x000FD6FA
	public static StoryManager Instance { get; private set; }

	// Token: 0x06008ACD RID: 35533 RVA: 0x000FF502 File Offset: 0x000FD702
	public static IReadOnlyList<StoryManager.StoryTelemetry> GetTelemetry()
	{
		return StoryManager.storyTelemetry;
	}

	// Token: 0x06008ACE RID: 35534 RVA: 0x0036B414 File Offset: 0x00369614
	protected override void OnPrefabInit()
	{
		StoryManager.Instance = this;
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Combine(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06008ACF RID: 35535 RVA: 0x0036B46C File Offset: 0x0036966C
	protected override void OnCleanUp()
	{
		GameClock.Instance.Unsubscribe(631075836, new Action<object>(this.OnNewDayStarted));
		Game instance = Game.Instance;
		instance.OnLoad = (Action<Game.GameSaveData>)Delegate.Remove(instance.OnLoad, new Action<Game.GameSaveData>(this.OnGameLoaded));
	}

	// Token: 0x06008AD0 RID: 35536 RVA: 0x0036B4BC File Offset: 0x003696BC
	public void InitialSaveSetup()
	{
		this.highestStoryCoordinateWhenGenerated = Db.Get().Stories.GetHighestCoordinate();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			foreach (string storyTraitTemplate in worldContainer.StoryTraitIds)
			{
				Story storyFromStoryTrait = Db.Get().Stories.GetStoryFromStoryTrait(storyTraitTemplate);
				this.CreateStory(storyFromStoryTrait, worldContainer.id);
			}
		}
		this.LogInitialSaveSetup();
	}

	// Token: 0x06008AD1 RID: 35537 RVA: 0x0036B584 File Offset: 0x00369784
	public StoryInstance CreateStory(string id, int worldId)
	{
		Story story = Db.Get().Stories.Get(id);
		return this.CreateStory(story, worldId);
	}

	// Token: 0x06008AD2 RID: 35538 RVA: 0x0036B5AC File Offset: 0x003697AC
	public StoryInstance CreateStory(Story story, int worldId)
	{
		StoryInstance storyInstance = new StoryInstance(story, worldId);
		this._stories.Add(story.HashId, storyInstance);
		StoryManager.InitTelemetry(storyInstance);
		if (story.autoStart)
		{
			this.BeginStoryEvent(story);
		}
		return storyInstance;
	}

	// Token: 0x06008AD3 RID: 35539 RVA: 0x000FF509 File Offset: 0x000FD709
	public StoryInstance GetStoryInstance(Story story)
	{
		return this.GetStoryInstance(story.HashId);
	}

	// Token: 0x06008AD4 RID: 35540 RVA: 0x0036B5EC File Offset: 0x003697EC
	public StoryInstance GetStoryInstance(int hash)
	{
		StoryInstance result;
		this._stories.TryGetValue(hash, out result);
		return result;
	}

	// Token: 0x06008AD5 RID: 35541 RVA: 0x000FF517 File Offset: 0x000FD717
	public Dictionary<int, StoryInstance> GetStoryInstances()
	{
		return this._stories;
	}

	// Token: 0x06008AD6 RID: 35542 RVA: 0x000FF51F File Offset: 0x000FD71F
	public int GetHighestCoordinate()
	{
		return this.highestStoryCoordinateWhenGenerated;
	}

	// Token: 0x06008AD7 RID: 35543 RVA: 0x000FF527 File Offset: 0x000FD727
	private string GetCompleteUnlockId(string id)
	{
		return id + "_STORY_COMPLETE";
	}

	// Token: 0x06008AD8 RID: 35544 RVA: 0x000FF534 File Offset: 0x000FD734
	public void ForceCreateStory(Story story, int worldId)
	{
		if (this.GetStoryInstance(story.HashId) == null)
		{
			this.CreateStory(story, worldId);
		}
	}

	// Token: 0x06008AD9 RID: 35545 RVA: 0x0036B60C File Offset: 0x0036980C
	public void DiscoverStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.DISCOVERED, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.DISCOVERED;
	}

	// Token: 0x06008ADA RID: 35546 RVA: 0x0036B63C File Offset: 0x0036983C
	public void BeginStoryEvent(Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || this.CheckState(StoryInstance.State.IN_PROGRESS, story))
		{
			return;
		}
		storyInstance.CurrentState = StoryInstance.State.IN_PROGRESS;
	}

	// Token: 0x06008ADB RID: 35547 RVA: 0x000FF54D File Offset: 0x000FD74D
	public void CompleteStoryEvent(Story story, MonoBehaviour keepsakeSpawnTarget, FocusTargetSequence.Data sequenceData)
	{
		if (this.GetStoryInstance(story.HashId) == null || this.CheckState(StoryInstance.State.COMPLETE, story))
		{
			return;
		}
		FocusTargetSequence.Start(keepsakeSpawnTarget, sequenceData);
	}

	// Token: 0x06008ADC RID: 35548 RVA: 0x0036B66C File Offset: 0x0036986C
	public void CompleteStoryEvent(Story story, Vector3 keepsakeSpawnPosition)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null)
		{
			return;
		}
		GameObject prefab = Assets.GetPrefab(storyInstance.GetStory().keepsakePrefabId);
		if (prefab != null)
		{
			keepsakeSpawnPosition.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = Util.KInstantiate(prefab, keepsakeSpawnPosition);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
		storyInstance.CurrentState = StoryInstance.State.COMPLETE;
		Game.Instance.unlocks.Unlock(this.GetCompleteUnlockId(story.Id), true);
	}

	// Token: 0x06008ADD RID: 35549 RVA: 0x0036B70C File Offset: 0x0036990C
	public bool CheckState(StoryInstance.State state, Story story)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.CurrentState >= state;
	}

	// Token: 0x06008ADE RID: 35550 RVA: 0x000FF56F File Offset: 0x000FD76F
	public bool IsStoryComplete(Story story)
	{
		return this.CheckState(StoryInstance.State.COMPLETE, story);
	}

	// Token: 0x06008ADF RID: 35551 RVA: 0x000FF579 File Offset: 0x000FD779
	public bool IsStoryCompleteGlobal(Story story)
	{
		return Game.Instance.unlocks.IsUnlocked(this.GetCompleteUnlockId(story.Id));
	}

	// Token: 0x06008AE0 RID: 35552 RVA: 0x0036B738 File Offset: 0x00369938
	public StoryInstance DisplayPopup(Story story, StoryManager.PopupInfo info, System.Action popupCB = null, Notification.ClickCallback notificationCB = null)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		if (storyInstance == null || storyInstance.HasDisplayedPopup(info.PopupType))
		{
			return null;
		}
		EventInfoData eventInfoData = EventInfoDataHelper.GenerateStoryTraitData(info.Title, info.Description, info.CloseButtonText, info.TextureName, info.PopupType, info.CloseButtonToolTip, info.Minions, popupCB);
		if (info.extraButtons != null && info.extraButtons.Length != 0)
		{
			foreach (StoryManager.ExtraButtonInfo extraButtonInfo in info.extraButtons)
			{
				eventInfoData.SimpleOption(extraButtonInfo.ButtonText, extraButtonInfo.OnButtonClick).tooltip = extraButtonInfo.ButtonToolTip;
			}
		}
		Notification notification = null;
		if (!info.DisplayImmediate)
		{
			notification = EventInfoScreen.CreateNotification(eventInfoData, notificationCB);
		}
		storyInstance.SetPopupData(info, eventInfoData, notification);
		return storyInstance;
	}

	// Token: 0x06008AE1 RID: 35553 RVA: 0x0036B808 File Offset: 0x00369A08
	public bool HasDisplayedPopup(Story story, EventInfoDataHelper.PopupType type)
	{
		StoryInstance storyInstance = this.GetStoryInstance(story.HashId);
		return storyInstance != null && storyInstance.HasDisplayedPopup(type);
	}

	// Token: 0x06008AE2 RID: 35554 RVA: 0x0036B830 File Offset: 0x00369A30
	private void LogInitialSaveSetup()
	{
		int num = 0;
		StoryManager.StoryCreationTelemetry[] array = new StoryManager.StoryCreationTelemetry[CustomGameSettings.Instance.CurrentStoryLevelsBySetting.Count];
		foreach (KeyValuePair<string, string> keyValuePair in CustomGameSettings.Instance.CurrentStoryLevelsBySetting)
		{
			array[num] = new StoryManager.StoryCreationTelemetry
			{
				StoryId = keyValuePair.Key,
				Enabled = CustomGameSettings.Instance.IsStoryActive(keyValuePair.Key, keyValuePair.Value)
			};
			num++;
		}
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "StoryTraitsCreation", array);
	}

	// Token: 0x06008AE3 RID: 35555 RVA: 0x000FF596 File Offset: 0x000FD796
	private void OnNewDayStarted(object _)
	{
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "SavedHighestStoryCoordinate", this.highestStoryCoordinateWhenGenerated);
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, "StoryTraits", StoryManager.storyTelemetry);
	}

	// Token: 0x06008AE4 RID: 35556 RVA: 0x0036B8D4 File Offset: 0x00369AD4
	private static void InitTelemetry(StoryInstance story)
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(story.worldId);
		if (world == null)
		{
			return;
		}
		story.Telemetry.StoryId = story.storyId;
		story.Telemetry.WorldId = world.worldName;
		StoryManager.storyTelemetry.Add(story.Telemetry);
	}

	// Token: 0x06008AE5 RID: 35557 RVA: 0x0036B930 File Offset: 0x00369B30
	private void OnGameLoaded(object _)
	{
		StoryManager.storyTelemetry.Clear();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair in this._stories)
		{
			StoryManager.InitTelemetry(keyValuePair.Value);
		}
		CustomGameSettings.Instance.DisableAllStories();
		foreach (KeyValuePair<int, StoryInstance> keyValuePair2 in this._stories)
		{
			SettingConfig config;
			if (keyValuePair2.Value.Telemetry.Retrofitted < 0f && CustomGameSettings.Instance.StorySettings.TryGetValue(keyValuePair2.Value.storyId, out config))
			{
				CustomGameSettings.Instance.SetStorySetting(config, true);
			}
		}
	}

	// Token: 0x06008AE6 RID: 35558 RVA: 0x000FF5BE File Offset: 0x000FD7BE
	public static void DestroyInstance()
	{
		StoryManager.storyTelemetry.Clear();
		StoryManager.Instance = null;
	}

	// Token: 0x040068CF RID: 26831
	public const int BEFORE_STORIES = -2;

	// Token: 0x040068D1 RID: 26833
	private static List<StoryManager.StoryTelemetry> storyTelemetry = new List<StoryManager.StoryTelemetry>();

	// Token: 0x040068D2 RID: 26834
	[Serialize]
	private Dictionary<int, StoryInstance> _stories = new Dictionary<int, StoryInstance>();

	// Token: 0x040068D3 RID: 26835
	[Serialize]
	private int highestStoryCoordinateWhenGenerated = -2;

	// Token: 0x040068D4 RID: 26836
	private const string STORY_TRAIT_KEY = "StoryTraits";

	// Token: 0x040068D5 RID: 26837
	private const string STORY_CREATION_KEY = "StoryTraitsCreation";

	// Token: 0x040068D6 RID: 26838
	private const string STORY_COORDINATE_KEY = "SavedHighestStoryCoordinate";

	// Token: 0x02001A0C RID: 6668
	public struct ExtraButtonInfo
	{
		// Token: 0x040068D7 RID: 26839
		public string ButtonText;

		// Token: 0x040068D8 RID: 26840
		public string ButtonToolTip;

		// Token: 0x040068D9 RID: 26841
		public System.Action OnButtonClick;
	}

	// Token: 0x02001A0D RID: 6669
	public struct PopupInfo
	{
		// Token: 0x040068DA RID: 26842
		public string Title;

		// Token: 0x040068DB RID: 26843
		public string Description;

		// Token: 0x040068DC RID: 26844
		public string CloseButtonText;

		// Token: 0x040068DD RID: 26845
		public string CloseButtonToolTip;

		// Token: 0x040068DE RID: 26846
		public StoryManager.ExtraButtonInfo[] extraButtons;

		// Token: 0x040068DF RID: 26847
		public string TextureName;

		// Token: 0x040068E0 RID: 26848
		public GameObject[] Minions;

		// Token: 0x040068E1 RID: 26849
		public bool DisplayImmediate;

		// Token: 0x040068E2 RID: 26850
		public EventInfoDataHelper.PopupType PopupType;
	}

	// Token: 0x02001A0E RID: 6670
	[SerializationConfig(MemberSerialization.OptIn)]
	public class StoryTelemetry : ISaveLoadable
	{
		// Token: 0x06008AE9 RID: 35561 RVA: 0x0036BA1C File Offset: 0x00369C1C
		public void LogStateChange(StoryInstance.State state, float time)
		{
			switch (state)
			{
			case StoryInstance.State.RETROFITTED:
				this.Retrofitted = ((this.Retrofitted >= 0f) ? this.Retrofitted : time);
				return;
			case StoryInstance.State.NOT_STARTED:
				break;
			case StoryInstance.State.DISCOVERED:
				this.Discovered = ((this.Discovered >= 0f) ? this.Discovered : time);
				return;
			case StoryInstance.State.IN_PROGRESS:
				this.InProgress = ((this.InProgress >= 0f) ? this.InProgress : time);
				return;
			case StoryInstance.State.COMPLETE:
				this.Completed = ((this.Completed >= 0f) ? this.Completed : time);
				break;
			default:
				return;
			}
		}

		// Token: 0x040068E3 RID: 26851
		public string StoryId;

		// Token: 0x040068E4 RID: 26852
		public string WorldId;

		// Token: 0x040068E5 RID: 26853
		[Serialize]
		public float Retrofitted = -1f;

		// Token: 0x040068E6 RID: 26854
		[Serialize]
		public float Discovered = -1f;

		// Token: 0x040068E7 RID: 26855
		[Serialize]
		public float InProgress = -1f;

		// Token: 0x040068E8 RID: 26856
		[Serialize]
		public float Completed = -1f;
	}

	// Token: 0x02001A0F RID: 6671
	public class StoryCreationTelemetry
	{
		// Token: 0x040068E9 RID: 26857
		public string StoryId;

		// Token: 0x040068EA RID: 26858
		public bool Enabled;
	}
}
