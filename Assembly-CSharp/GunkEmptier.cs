using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000E16 RID: 3606
public class GunkEmptier : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>
{
	// Token: 0x06004674 RID: 18036 RVA: 0x0025D744 File Offset: 0x0025B944
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noOperational;
		this.noOperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.noOperational, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.IsOperational))).DefaultState(this.operational.noStorageSpace);
		this.operational.noStorageSpace.ToggleStatusItem(Db.Get().BuildingStatusItems.GunkEmptierFull, null).EventTransition(GameHashes.OnStorageChange, this.operational.ready, new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank));
		this.operational.ready.EventTransition(GameHashes.OnStorageChange, this.operational.noStorageSpace, GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Not(new StateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.Transition.ConditionCallback(GunkEmptier.HasSpaceToEmptyABionicGunkTank))).ToggleRecurringChore(new Func<GunkEmptier.Instance, Chore>(GunkEmptier.CreateChore), null);
	}

	// Token: 0x06004675 RID: 18037 RVA: 0x000D211D File Offset: 0x000D031D
	public static bool HasSpaceToEmptyABionicGunkTank(GunkEmptier.Instance smi)
	{
		return smi.RemainingStorageCapacity >= GunkMonitor.GUNK_CAPACITY;
	}

	// Token: 0x06004676 RID: 18038 RVA: 0x000D212F File Offset: 0x000D032F
	public static bool IsOperational(GunkEmptier.Instance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06004677 RID: 18039 RVA: 0x0025D844 File Offset: 0x0025BA44
	private static WorkChore<GunkEmptierWorkable> CreateChore(GunkEmptier.Instance smi)
	{
		WorkChore<GunkEmptierWorkable> workChore = new WorkChore<GunkEmptierWorkable>(Db.Get().ChoreTypes.ExpellGunk, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
		return workChore;
	}

	// Token: 0x0400312F RID: 12591
	private static string DISEASE_ID = DUPLICANTSTATS.BIONICS.Secretions.PEE_DISEASE;

	// Token: 0x04003130 RID: 12592
	private static int DISEASE_ON_DUPE_COUNT_PER_USE = DUPLICANTSTATS.BIONICS.Secretions.DISEASE_PER_PEE / 20;

	// Token: 0x04003131 RID: 12593
	public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noOperational;

	// Token: 0x04003132 RID: 12594
	public GunkEmptier.OperationalStates operational;

	// Token: 0x02000E17 RID: 3607
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E18 RID: 3608
	public class OperationalStates : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State
	{
		// Token: 0x04003133 RID: 12595
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State noStorageSpace;

		// Token: 0x04003134 RID: 12596
		public GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.State ready;
	}

	// Token: 0x02000E19 RID: 3609
	public new class Instance : GameStateMachine<GunkEmptier, GunkEmptier.Instance, IStateMachineTarget, GunkEmptier.Def>.GameInstance
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x000D2174 File Offset: 0x000D0374
		public float RemainingStorageCapacity
		{
			get
			{
				return this.storage.RemainingCapacity();
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600467D RID: 18045 RVA: 0x000D2181 File Offset: 0x000D0381
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0025D898 File Offset: 0x0025BA98
		public Instance(IStateMachineTarget master, GunkEmptier.Def def) : base(master, def)
		{
			GunkEmptierWorkable component = base.GetComponent<GunkEmptierWorkable>();
			GunkEmptierWorkable gunkEmptierWorkable = component;
			gunkEmptierWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(gunkEmptierWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed));
			Components.GunkExtractors.Add(component);
			this.storage = base.GetComponent<Storage>();
			this.operational = base.GetComponent<Operational>();
			base.gameObject.AddOrGet<Ownable>().AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x0025D918 File Offset: 0x0025BB18
		protected override void OnCleanUp()
		{
			GunkEmptierWorkable component = base.GetComponent<GunkEmptierWorkable>();
			GunkEmptierWorkable gunkEmptierWorkable = component;
			gunkEmptierWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(gunkEmptierWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnGunkEmptierUsed));
			Components.GunkExtractors.Remove(component);
			base.OnCleanUp();
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x000CBB6E File Offset: 0x000C9D6E
		private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
		{
			return worker.GetMinionModel() == BionicMinionConfig.MODEL;
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x000D218E File Offset: 0x000D038E
		public void OnGunkEmptierUsed(Workable workable, Workable.WorkableEvent ev)
		{
			if (ev == Workable.WorkableEvent.WorkCompleted)
			{
				this.AddDisseaseToWorker(workable.worker);
			}
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x0025D960 File Offset: 0x0025BB60
		public void AddDisseaseToWorker(WorkerBase worker)
		{
			if (worker != null)
			{
				byte index = Db.Get().Diseases.GetIndex(GunkEmptier.DISEASE_ID);
				worker.GetComponent<PrimaryElement>().AddDisease(index, GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE, "GunkEmptier.Flush");
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, GunkEmptier.DISEASE_ON_DUPE_COUNT_PER_USE), base.transform, Vector3.up, 1.5f, false, false);
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
				return;
			}
			DebugUtil.LogWarningArgs(new object[]
			{
				"Tried to add disease on gunk emptier use but worker was null"
			});
		}

		// Token: 0x04003135 RID: 12597
		private Operational operational;

		// Token: 0x04003136 RID: 12598
		private Storage storage;
	}
}
