using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class MinorFossilDigSite : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
	// Token: 0x060011B8 RID: 4536 RVA: 0x00190920 File Offset: 0x0018EB20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(true);
		}).PlayAnim("object_dirty").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}, delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MinorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.Reveal)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.ProgressStoryTrait)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable)).PlayAnim("object").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Completed.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("object").Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x000B1DDC File Offset: 0x000AFFDC
	public static void MakeItDemolishable(MinorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x000B234B File Offset: 0x000B054B
	public static void DestroyUIExcavateButton(MinorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x000B2353 File Offset: 0x000B0553
	public static void SetEntombedStatusItemVisibility(MinorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x000B235C File Offset: 0x000B055C
	public static void UnregisterFromComponents(MinorFossilDigSite.Instance smi)
	{
		Components.MinorFossilDigSites.Remove(smi);
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x000AEE7C File Offset: 0x000AD07C
	public static void SelfDestroy(MinorFossilDigSite.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x000B2369 File Offset: 0x000B0569
	public static void StartWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000B2371 File Offset: 0x000B0571
	public static void CancelWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000B2379 File Offset: 0x000B0579
	public static void Reveal(MinorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			smi.ShowCompletionNotification();
			MinorFossilDigSite.DropLoot(smi);
		}
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x00190C14 File Offset: 0x0018EE14
	public static void DropLoot(MinorFossilDigSite.Instance smi)
	{
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000B23B1 File Offset: 0x000B05B1
	public static void ProgressStoryTrait(MinorFossilDigSite.Instance smi)
	{
		MinorFossilDigSite.ProgressQuest(smi);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x00190CC8 File Offset: 0x0018EEC8
	public static QuestInstance ProgressQuest(MinorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.fossilQuestCriteriaID,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
			return instance;
		}
		return null;
	}

	// Token: 0x04000C60 RID: 3168
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Idle;

	// Token: 0x04000C61 RID: 3169
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Completed;

	// Token: 0x04000C62 RID: 3170
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x04000C63 RID: 3171
	public MinorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x04000C64 RID: 3172
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000C65 RID: 3173
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000C66 RID: 3174
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000C67 RID: 3175
	private const string UNEXCAVATED_ANIM_NAME = "object_dirty";

	// Token: 0x04000C68 RID: 3176
	private const string EXCAVATED_ANIM_NAME = "object";

	// Token: 0x0200042A RID: 1066
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000C69 RID: 3177
		public HashedString fossilQuestCriteriaID;
	}

	// Token: 0x0200042B RID: 1067
	public class ReadyToBeWorked : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State
	{
		// Token: 0x04000C6A RID: 3178
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Operational;

		// Token: 0x04000C6B RID: 3179
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x0200042C RID: 1068
	public new class Instance : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x060011C7 RID: 4551 RVA: 0x00190D2C File Offset: 0x0018EF2C
		public Instance(IStateMachineTarget master, MinorFossilDigSite.Def def) : base(master, def)
		{
			Components.MinorFossilDigSites.Add(this);
			this.negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, true, false, true);
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x000B23CA File Offset: 0x000B05CA
		public void SetDecorState(bool isDusty)
		{
			if (isDusty)
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Add(this.negativeDecorModifier);
				return;
			}
			base.gameObject.GetComponent<DecorProvider>().decor.Remove(this.negativeDecorModifier);
		}

		// Token: 0x060011C9 RID: 4553 RVA: 0x00190D80 File Offset: 0x0018EF80
		public override void StartSM()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x060011CA RID: 4554 RVA: 0x000B2406 File Offset: 0x000B0606
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000B2431 File Offset: 0x000B0631
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.IsQuestCompleted.Set(true, this, false);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x00190E50 File Offset: 0x0018F050
		protected override void OnCleanUp()
		{
			MinorFossilDigSite.ProgressQuest(base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MinorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x000B2469 File Offset: 0x000B0669
		public void OnStorytraitProgressChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RefreshUI();
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x000B247A File Offset: 0x000B067A
		public void RefreshUI()
		{
			base.Trigger(1980521255, null);
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000B2488 File Offset: 0x000B0688
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x00190F00 File Offset: 0x0018F100
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.workable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x000B2490 File Offset: 0x000B0690
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MinorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000B24B1 File Offset: 0x000B06B1
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x0018CA80 File Offset: 0x0018AC80
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00190F48 File Offset: 0x0018F148
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00190F98 File Offset: 0x0018F198
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

		// Token: 0x060011D6 RID: 4566 RVA: 0x000B24BF File Offset: 0x000B06BF
		public void DestroyExcavateButton()
		{
			this.workable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x000AFFE5 File Offset: 0x000AE1E5
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000B24ED File Offset: 0x000B06ED
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

		// Token: 0x060011D9 RID: 4569 RVA: 0x000B2517 File Offset: 0x000B0717
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x000B1FFD File Offset: 0x000B01FD
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04000C6C RID: 3180
		[MyCmpGet]
		private MinorDigSiteWorkable workable;

		// Token: 0x04000C6D RID: 3181
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x04000C6E RID: 3182
		private ExcavateButton excavateButton;

		// Token: 0x04000C6F RID: 3183
		private Chore chore;

		// Token: 0x04000C70 RID: 3184
		private AttributeModifier negativeDecorModifier;
	}
}
