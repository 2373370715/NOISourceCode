using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	// Token: 0x060010D9 RID: 4313 RVA: 0x0018C3B8 File Offset: 0x0018A5B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("covered").Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MajorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
		this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x000B1DDC File Offset: 0x000AFFDC
	public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0018C668 File Offset: 0x0018A868
	public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.questCriteria,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
		}
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x000B1DEF File Offset: 0x000AFFEF
	public static void TurnOnLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(true);
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x000B1DF8 File Offset: 0x000AFFF8
	public static void TurnOffLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(false);
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0018C6C8 File Offset: 0x0018A8C8
	public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000B1E01 File Offset: 0x000B0001
	public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x000B1E0A File Offset: 0x000B000A
	public static void UnlockFossilMine(MajorFossilDigSite.Instance smi)
	{
		smi.UnlockFossilMine();
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x000B1E12 File Offset: 0x000B0012
	public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x000B1E1A File Offset: 0x000B001A
	public static void CompleteStory(MajorFossilDigSite.Instance smi)
	{
		if (smi.sm.IsQuestCompleted.Get(smi))
		{
			return;
		}
		smi.sm.IsQuestCompleted.Set(true, smi, false);
		smi.CompleteStoryTrait();
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0018C704 File Offset: 0x0018A904
	public static void Reveal(MajorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x000B1E4A File Offset: 0x000B004A
	public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x000B1E52 File Offset: 0x000B0052
	public static void RefreshUI(MajorFossilDigSite.Instance smi)
	{
		smi.RefreshUI();
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x000B1E5A File Offset: 0x000B005A
	public static void StartWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x000B1E62 File Offset: 0x000B0062
	public static void CancelWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x04000BC3 RID: 3011
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;

	// Token: 0x04000BC4 RID: 3012
	public MajorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x04000BC5 RID: 3013
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x04000BC6 RID: 3014
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;

	// Token: 0x04000BC7 RID: 3015
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000BC8 RID: 3016
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000BC9 RID: 3017
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000BCA RID: 3018
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;

	// Token: 0x04000BCB RID: 3019
	public const string ANIM_COVERED_NAME = "covered";

	// Token: 0x04000BCC RID: 3020
	public const string ANIM_REVEALED_NAME = "reveal";

	// Token: 0x02000401 RID: 1025
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000BCD RID: 3021
		public HashedString questCriteria;
	}

	// Token: 0x02000402 RID: 1026
	public class ReadyToBeWorked : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
	{
		// Token: 0x04000BCE RID: 3022
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;

		// Token: 0x04000BCF RID: 3023
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02000403 RID: 1027
	public new class Instance : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x000B1E7A File Offset: 0x000B007A
		public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def) : base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0018C768 File Offset: 0x0018A968
		public override void StartSM()
		{
			this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			base.GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			this.fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				this.UnlockStandarBuildingButtons();
				this.ScheduleNextFrame(delegate(object obj)
				{
					this.ChangeUIDescriptionToCompleted();
				}, null);
			}
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0018C86C File Offset: 0x0018AA6C
		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn, true);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x000B1E8F File Offset: 0x000B008F
		public Workable GetWorkable()
		{
			return this.excavateWorkable;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0018C898 File Offset: 0x0018AA98
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.excavateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x000B1E97 File Offset: 0x000B0097
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MajorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000B1EB8 File Offset: 0x000B00B8
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0018C8E0 File Offset: 0x0018AAE0
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0018C930 File Offset: 0x0018AB30
		public ExcavateButton CreateExcavateButton()
		{
			if (this.excavateButton == null)
			{
				this.excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton excavateButton = this.excavateButton;
				excavateButton.OnButtonPressed = (System.Action)Delegate.Combine(excavateButton.OnButtonPressed, new System.Action(this.OnExcavateButtonPressed));
				this.excavateButton.isMarkedForDig = (() => base.sm.MarkedForDig.Get(this));
			}
			return this.excavateButton;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000B1EC6 File Offset: 0x000B00C6
		public void DestroyExcavateButton()
		{
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x000AFFE5 File Offset: 0x000AE1E5
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x000B1EF4 File Offset: 0x000B00F4
		public string Description
		{
			get
			{
				if (base.sm.IsRevealed.Get(this))
				{
					return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED;
				}
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
			}
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x000B1F1E File Offset: 0x000B011E
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x000B1F34 File Offset: 0x000B0134
		public void RevealMinorDigSites()
		{
			if (this.storyInitializer == null)
			{
				this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (this.storyInitializer != null)
			{
				this.storyInitializer.RevealMinorFossilDigSites();
			}
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x000B1F62 File Offset: 0x000B0162
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000B1F8D File Offset: 0x000B018D
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0018C9A0 File Offset: 0x0018ABA0
		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			smi.sm.CompleteStory.Trigger(smi);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x000B1FC2 File Offset: 0x000B01C2
		public void UnlockFossilMine()
		{
			this.fossilMine.SetActiveState(true);
			this.UnlockStandarBuildingButtons();
			this.ChangeUIDescriptionToCompleted();
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0018C9CC File Offset: 0x0018ABCC
		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000B1FDC File Offset: 0x000B01DC
		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000B1FEA File Offset: 0x000B01EA
		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255, null);
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0018CA20 File Offset: 0x0018AC20
		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x000B1FFD File Offset: 0x000B01FD
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0018CA80 File Offset: 0x0018AC80
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x04000BD0 RID: 3024
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04000BD1 RID: 3025
		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		// Token: 0x04000BD2 RID: 3026
		[MyCmpGet]
		private FossilMine fossilMine;

		// Token: 0x04000BD3 RID: 3027
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04000BD4 RID: 3028
		private Chore chore;

		// Token: 0x04000BD5 RID: 3029
		private FossilHuntInitializer.Instance storyInitializer;

		// Token: 0x04000BD6 RID: 3030
		private ExcavateButton excavateButton;
	}
}
