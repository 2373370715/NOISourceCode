using System;
using Klei.AI;
using TUNING;

// Token: 0x02000705 RID: 1797
public class PartyChore : Chore<PartyChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06001FC4 RID: 8132 RVA: 0x001C6058 File Offset: 0x001C4258
	public PartyChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new PartyChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x001C60E4 File Offset: 0x001C42E4
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.partyer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		base.smi.sm.partyer.Get(base.smi).gameObject.AddTag(GameTags.Partying);
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x001C614C File Offset: 0x001C434C
	protected override void End(string reason)
	{
		if (base.smi.sm.partyer.Get(base.smi) != null)
		{
			base.smi.sm.partyer.Get(base.smi).gameObject.RemoveTag(GameTags.Partying);
		}
		base.End(reason);
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x000B94F7 File Offset: 0x000B76F7
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04001505 RID: 5381
	public int basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;

	// Token: 0x04001506 RID: 5382
	public const string specificEffect = "Socialized";

	// Token: 0x04001507 RID: 5383
	public const string trackingEffect = "RecentlySocialized";

	// Token: 0x02000706 RID: 1798
	public class States : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore>
	{
		// Token: 0x06001FC8 RID: 8136 RVA: 0x001C61B0 File Offset: 0x001C43B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.stand;
			base.Target(this.partyer);
			this.stand.InitializeStates(this.partyer, this.masterTarget, this.chat, null, null, null);
			this.chat_move.InitializeStates(this.partyer, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<Workable>(this.chitchatlocator, this.success, null, null);
			this.success.Enter(delegate(PartyChore.StatesInstance smi)
			{
				this.partyer.Get(smi).gameObject.GetComponent<Effects>().Add("RecentlyPartied", true);
			}).ReturnSuccess();
		}

		// Token: 0x04001508 RID: 5384
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter partyer;

		// Token: 0x04001509 RID: 5385
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter chitchatlocator;

		// Token: 0x0400150A RID: 5386
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> stand;

		// Token: 0x0400150B RID: 5387
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x0400150C RID: 5388
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State chat;

		// Token: 0x0400150D RID: 5389
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State success;
	}

	// Token: 0x02000707 RID: 1799
	public class StatesInstance : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.GameInstance
	{
		// Token: 0x06001FCB RID: 8139 RVA: 0x000B952E File Offset: 0x000B772E
		public StatesInstance(PartyChore master) : base(master)
		{
		}
	}
}
