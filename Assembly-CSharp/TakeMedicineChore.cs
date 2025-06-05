using System;
using STRINGS;

// Token: 0x02000764 RID: 1892
public class TakeMedicineChore : Chore<TakeMedicineChore.StatesInstance>
{
	// Token: 0x06002132 RID: 8498 RVA: 0x001CBFCC File Offset: 0x001CA1CC
	public TakeMedicineChore(MedicinalPillWorkable master) : base(Db.Get().ChoreTypes.TakeMedicine, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.medicine = master;
		this.pickupable = this.medicine.GetComponent<Pickupable>();
		base.smi = new TakeMedicineChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.CanPickup, this.pickupable);
		this.AddPrecondition(TakeMedicineChore.CanCure, this);
		this.AddPrecondition(TakeMedicineChore.IsConsumptionPermitted, this);
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x001CC060 File Offset: 0x001CA260
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.source.Set(this.pickupable.gameObject, base.smi, false);
		base.smi.sm.requestedpillcount.Set(1f, base.smi, false);
		base.smi.sm.eater.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		new TakeMedicineChore(this.medicine);
	}

	// Token: 0x0400163E RID: 5694
	private Pickupable pickupable;

	// Token: 0x0400163F RID: 5695
	private MedicinalPillWorkable medicine;

	// Token: 0x04001640 RID: 5696
	public static readonly Chore.Precondition CanCure = new Chore.Precondition
	{
		id = "CanCure",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CURE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((TakeMedicineChore)data).medicine.CanBeTakenBy(context.consumerState.gameObject);
		}
	};

	// Token: 0x04001641 RID: 5697
	public static readonly Chore.Precondition IsConsumptionPermitted = new Chore.Precondition
	{
		id = "IsConsumptionPermitted",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_CONSUMPTION_PERMITTED,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			TakeMedicineChore takeMedicineChore = (TakeMedicineChore)data;
			ConsumableConsumer consumableConsumer = context.consumerState.consumableConsumer;
			return consumableConsumer == null || consumableConsumer.IsPermitted(takeMedicineChore.medicine.PrefabID().Name);
		}
	};

	// Token: 0x02000765 RID: 1893
	public class StatesInstance : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.GameInstance
	{
		// Token: 0x06002135 RID: 8501 RVA: 0x000BA2B9 File Offset: 0x000B84B9
		public StatesInstance(TakeMedicineChore master) : base(master)
		{
		}
	}

	// Token: 0x02000766 RID: 1894
	public class States : GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore>
	{
		// Token: 0x06002136 RID: 8502 RVA: 0x001CC188 File Offset: 0x001CA388
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.eater);
			this.fetch.InitializeStates(this.eater, this.source, this.chunk, this.requestedpillcount, this.actualpillcount, this.takemedicine, null);
			this.takemedicine.ToggleAnims("anim_eat_floor_kanim", 0f).ToggleTag(GameTags.TakingMedicine).ToggleWork("TakeMedicine", delegate(TakeMedicineChore.StatesInstance smi)
			{
				MedicinalPillWorkable workable = this.chunk.Get<MedicinalPillWorkable>(smi);
				this.eater.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (TakeMedicineChore.StatesInstance smi) => this.chunk.Get<MedicinalPill>(smi) != null, null, null);
		}

		// Token: 0x04001642 RID: 5698
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter eater;

		// Token: 0x04001643 RID: 5699
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter source;

		// Token: 0x04001644 RID: 5700
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.TargetParameter chunk;

		// Token: 0x04001645 RID: 5701
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter requestedpillcount;

		// Token: 0x04001646 RID: 5702
		public StateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FloatParameter actualpillcount;

		// Token: 0x04001647 RID: 5703
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.FetchSubState fetch;

		// Token: 0x04001648 RID: 5704
		public GameStateMachine<TakeMedicineChore.States, TakeMedicineChore.StatesInstance, TakeMedicineChore, object>.State takemedicine;
	}
}
