using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200033D RID: 829
public class FossilHuntInitializer : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>
{
	// Token: 0x06000D0B RID: 3339 RVA: 0x0017BFC4 File Offset: 0x0017A1C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Inactive.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).ParamTransition<bool>(this.wasStoryStarted, this.Active.inProgress, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue);
		this.Active.inProgress.ParamTransition<bool>(this.storyCompleted, this.Active.StoryComplete, GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.IsTrue).OnSignal(this.CompleteStory, this.Active.StoryComplete);
		this.Active.StoryComplete.Enter(new StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State.Callback(FossilHuntInitializer.CompleteStoryTrait));
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x0017C078 File Offset: 0x0017A278
	public static bool OnUI_Quest_ObjectiveRowClicked(string rowLinkID)
	{
		rowLinkID = rowLinkID.ToUpper();
		if (!rowLinkID.Contains("MOVECAMERATO"))
		{
			return true;
		}
		string b = rowLinkID.Replace("MOVECAMERATO", "");
		if (Components.MajorFossilDigSites.Count > 0 && CodexCache.FormatLinkID(Components.MajorFossilDigSites[0].gameObject.PrefabID().ToString()) == b)
		{
			GameUtil.FocusCamera(Components.MajorFossilDigSites[0].transform, true, true);
			return false;
		}
		foreach (object obj in Components.MinorFossilDigSites)
		{
			MinorFossilDigSite.Instance instance = (MinorFossilDigSite.Instance)obj;
			if (CodexCache.FormatLinkID(instance.PrefabID().ToString()) == b)
			{
				GameUtil.FocusCamera(instance.transform.GetPosition(), 2f, true, true);
				SelectTool.Instance.Select(instance.gameObject.GetComponent<KSelectable>(), false);
				return false;
			}
		}
		return false;
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x0017C1A4 File Offset: 0x0017A3A4
	public static void CompleteStoryTrait(FossilHuntInitializer.Instance smi)
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
		if (storyInstance == null)
		{
			return;
		}
		smi.sm.storyCompleted.Set(true, smi, false);
		if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
		{
			return;
		}
		smi.CompleteEvent();
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x000AFFB1 File Offset: 0x000AE1B1
	public static string ResolveStrings_QuestObjectivesRowTooltips(string originalText, object obj)
	{
		return originalText + CODEX.STORY_TRAITS.FOSSILHUNT.QUEST.LINKED_TOOLTIP;
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x0017C1F8 File Offset: 0x0017A3F8
	public static string ResolveQuestTitle(string title, QuestInstance quest)
	{
		int discoveredDigsitesRequired = FossilDigSiteConfig.DiscoveredDigsitesRequired;
		string str = Mathf.RoundToInt(quest.CurrentProgress * (float)discoveredDigsitesRequired).ToString() + "/" + discoveredDigsitesRequired.ToString();
		return title + " - " + str;
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0017C240 File Offset: 0x0017A440
	public static ICheckboxListGroupControl.ListGroup[] GetFossilHuntQuestData()
	{
		QuestInstance quest = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		ICheckboxListGroupControl.CheckboxItem[] checkBoxData = quest.GetCheckBoxData(null);
		for (int i = 0; i < checkBoxData.Length; i++)
		{
			checkBoxData[i].overrideLinkActions = new Func<string, bool>(FossilHuntInitializer.OnUI_Quest_ObjectiveRowClicked);
			checkBoxData[i].resolveTooltipCallback = new Func<string, object, string>(FossilHuntInitializer.ResolveStrings_QuestObjectivesRowTooltips);
		}
		if (quest != null)
		{
			return new ICheckboxListGroupControl.ListGroup[]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.FossilHuntQuest.Title, checkBoxData, (string title) => FossilHuntInitializer.ResolveQuestTitle(title, quest), null)
			};
		}
		return new ICheckboxListGroupControl.ListGroup[0];
	}

	// Token: 0x040009BA RID: 2490
	private GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State Inactive;

	// Token: 0x040009BB RID: 2491
	private FossilHuntInitializer.ActiveState Active;

	// Token: 0x040009BC RID: 2492
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter storyCompleted;

	// Token: 0x040009BD RID: 2493
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.BoolParameter wasStoryStarted;

	// Token: 0x040009BE RID: 2494
	public StateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.Signal CompleteStory;

	// Token: 0x040009BF RID: 2495
	public const string LINK_OVERRIDE_PREFIX = "MOVECAMERATO";

	// Token: 0x0200033E RID: 830
	public class Def : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitDef
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x0017C300 File Offset: 0x0017A500
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.FossilHunt;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(1, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_fossilhunt_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.BEGIN_POPUP.BUTTON,
				TextureName = "fossildigdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.CompleteLoreId = "story_trait_fossilhunt_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.NAME,
				Description = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.FOSSILHUNT.END_POPUP.BUTTON,
				TextureName = "fossildigmining_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x040009C0 RID: 2496
		public const string LORE_UNLOCK_PREFIX = "story_trait_fossilhunt_";

		// Token: 0x040009C1 RID: 2497
		public bool IsMainDigsite;
	}

	// Token: 0x0200033F RID: 831
	public class ActiveState : GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State
	{
		// Token: 0x040009C2 RID: 2498
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State inProgress;

		// Token: 0x040009C3 RID: 2499
		public GameStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, StateMachineController, FossilHuntInitializer.Def>.State StoryComplete;
	}

	// Token: 0x02000340 RID: 832
	public new class Instance : StoryTraitStateMachine<FossilHuntInitializer, FossilHuntInitializer.Instance, FossilHuntInitializer.Def>.TraitInstance
	{
		// Token: 0x06000D15 RID: 3349 RVA: 0x000AFFDB File Offset: 0x000AE1DB
		public Instance(StateMachineController master, FossilHuntInitializer.Def def) : base(master, def)
		{
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000D16 RID: 3350 RVA: 0x000AFFE5 File Offset: 0x000AE1E5
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x000AFFF1 File Offset: 0x000AE1F1
		public string Description
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION;
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0017C418 File Offset: 0x0017A618
		public override void StartSM()
		{
			base.StartSM();
			base.gameObject.GetSMI<MajorFossilDigSite>();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance == null)
			{
				return;
			}
			if (base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				StoryInstance.State currentState = storyInstance.CurrentState;
				if (currentState != StoryInstance.State.IN_PROGRESS)
				{
					if (currentState == StoryInstance.State.COMPLETE)
					{
						this.GoTo(base.sm.Active.StoryComplete);
					}
				}
				else
				{
					base.sm.wasStoryStarted.Set(true, this, false);
				}
			}
			StoryInstance storyInstance2 = storyInstance;
			storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0017C4D8 File Offset: 0x0017A6D8
		protected override void OnCleanUp()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x000AFFFD File Offset: 0x000AE1FD
		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state == StoryInstance.State.IN_PROGRESS)
			{
				base.sm.wasStoryStarted.Set(true, this, false);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0017C530 File Offset: 0x0017A730
		protected override void OnObjectSelect(object clicked)
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				this.RevealMajorFossilDigSites();
				this.RevealMinorFossilDigSites();
			}
			if (!(bool)clicked)
			{
				return;
			}
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
			if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE && (storyInstance.PendingType != EventInfoDataHelper.PopupType.COMPLETE || base.def.IsMainDigsite))
			{
				base.OnNotificationClicked(null);
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
			{
				base.DisplayPopup(base.def.EventIntroInfo);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000B0017 File Offset: 0x000AE217
		public override void OnPopupClosed()
		{
			if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.COMPLETE))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}
			base.OnPopupClosed();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0017C5D8 File Offset: 0x0017A7D8
		protected override void OnBuildingActivated(object activated)
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || base.sm.wasStoryStarted.Get(this) || storyInstance.CurrentState >= StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RevealMinorFossilDigSites();
			this.RevealMajorFossilDigSites();
			base.OnBuildingActivated(activated);
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000B003E File Offset: 0x000AE23E
		public void RevealMajorFossilDigSites()
		{
			this.RevealAll(8, new Tag[]
			{
				"FossilDig"
			});
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0017C638 File Offset: 0x0017A838
		public void RevealMinorFossilDigSites()
		{
			this.RevealAll(3, new Tag[]
			{
				"FossilResin",
				"FossilIce",
				"FossilRock"
			});
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0017C688 File Offset: 0x0017A888
		private void RevealAll(int radius, params Tag[] tags)
		{
			foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(false, tags))
			{
				int baseX;
				int baseY;
				Grid.CellToXY(spawnable.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, radius, (float)radius);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000B005E File Offset: 0x000AE25E
		public override void OnCompleteStorySequence()
		{
			if (base.def.IsMainDigsite)
			{
				base.OnCompleteStorySequence();
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0017C6F8 File Offset: 0x0017A8F8
		public void ShowLoreUnlockedPopup(int popupID)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(popupID)) != null;
			Option<string> option = FossilDigSiteConfig.GetBodyContentForFossil(popupID);
			if (flag && option.HasValue)
			{
				infoDialogScreen.AddPlainText(option.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.UNLOCK_DNADATA_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITFOSSILHUNT"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0017C77C File Offset: 0x0017A97C
		public void ShowObjectiveCompletedNotification()
		{
			FossilHuntInitializer.Instance.<>c__DisplayClass16_0 CS$<>8__locals1 = new FossilHuntInitializer.Instance.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance == null)
			{
				return;
			}
			CS$<>8__locals1.objectivesCompleted = Mathf.RoundToInt(instance.CurrentProgress * (float)instance.CriteriaCount);
			if (CS$<>8__locals1.objectivesCompleted == 0)
			{
				this.ShowFirstFossilExcavatedNotification();
				return;
			}
			string unlockID = FossilDigSiteConfig.FOSSIL_HUNT_LORE_UNLOCK_ID.For(CS$<>8__locals1.objectivesCompleted);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowObjectiveCompletedNotification>g__ShowNotificationAndWaitForClick|1().Then(delegate
			{
				CS$<>8__locals1.<>4__this.ShowLoreUnlockedPopup(CS$<>8__locals1.objectivesCompleted);
			});
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000B0073 File Offset: 0x000AE273
		public void ShowFirstFossilExcavatedNotification()
		{
			this.<ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1().Then(delegate
			{
				this.ShowQuestUnlockedPopup();
			});
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0017C814 File Offset: 0x0017AA14
		public void ShowQuestUnlockedPopup()
		{
			LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.NAME).AddDefaultOK(false).AddPlainText(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.DESCRIPTION.text.Value).AddOption(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_POPUP.CHECK_BUTTON, delegate(InfoDialogScreen dialog)
			{
				dialog.Deactivate();
				GameUtil.FocusCamera(base.transform, true, true);
			}, false);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000B0095 File Offset: 0x000AE295
		[CompilerGenerated]
		private Promise <ShowFirstFossilExcavatedNotification>g__ShowNotificationAndWaitForClick|17_1()
		{
			return new Promise(delegate(System.Action resolve)
			{
				Notification notification = new Notification(CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.NAME, NotificationType.Event, (List<Notification> notifications, object obj) => CODEX.STORY_TRAITS.FOSSILHUNT.QUEST_AVAILABLE_NOTIFICATION.TOOLTIP, null, false, 0f, delegate(object obj)
				{
					resolve();
				}, null, null, true, true, false);
				base.gameObject.AddOrGet<Notifier>().Add(notification, "");
			});
		}
	}
}
