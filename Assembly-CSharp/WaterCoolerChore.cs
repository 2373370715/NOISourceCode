using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200077C RID: 1916
public class WaterCoolerChore : Chore<WaterCoolerChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x06002177 RID: 8567 RVA: 0x001CD16C File Offset: 0x001CB36C
	public WaterCoolerChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Relax, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new WaterCoolerChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
		this.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x000BA4E4 File Offset: 0x000B86E4
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.drinker.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x001CD240 File Offset: 0x001CB440
	public bool GetWorkerPriority(WorkerBase worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04001684 RID: 5764
	public int basePriority = RELAXATION.PRIORITY.TIER2;

	// Token: 0x04001685 RID: 5765
	public string specificEffect = "Socialized";

	// Token: 0x04001686 RID: 5766
	public string trackingEffect = "RecentlySocialized";

	// Token: 0x0200077D RID: 1917
	public class States : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore>
	{
		// Token: 0x0600217A RID: 8570 RVA: 0x001CD2A0 File Offset: 0x001CB4A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.drink_move;
			base.Target(this.drinker);
			this.drink_move.InitializeStates(this.drinker, this.masterTarget, this.drink, null, null, null);
			this.drink.ToggleAnims(new Func<WaterCoolerChore.StatesInstance, KAnimFile>(WaterCoolerChore.States.GetAnimFileName)).DefaultState(this.drink.drink);
			this.drink.drink.Face(this.masterTarget, 0.5f).PlayAnim("working_pre").QueueAnim("working_loop", false, null).OnAnimQueueComplete(this.drink.post);
			this.drink.post.Enter("Drink", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.TriggerDrink)).Enter("Mark", new StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State.Callback(this.MarkAsRecentlySocialized)).PlayAnim("working_pst").OnAnimQueueComplete(this.chat_move);
			this.chat_move.InitializeStates(this.drinker, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<SocialGatheringPointWorkable>(this.chitchatlocator, this.success, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x001CD3E0 File Offset: 0x001CB5E0
		public static KAnimFile GetAnimFileName(WaterCoolerChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.drinker.Get(smi);
			if (gameObject == null)
			{
				return Assets.GetAnim("anim_interacts_watercooler_kanim");
			}
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null && component.model == BionicMinionConfig.MODEL)
			{
				return Assets.GetAnim("anim_bionic_interacts_watercooler_kanim");
			}
			return Assets.GetAnim("anim_interacts_watercooler_kanim");
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x001CD45C File Offset: 0x001CB65C
		private void MarkAsRecentlySocialized(WaterCoolerChore.StatesInstance smi)
		{
			Effects component = this.stateTarget.Get<WorkerBase>(smi).GetComponent<Effects>();
			if (!string.IsNullOrEmpty(smi.master.trackingEffect))
			{
				component.Add(smi.master.trackingEffect, true);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x001CD4A0 File Offset: 0x001CB6A0
		private void TriggerDrink(WaterCoolerChore.StatesInstance smi)
		{
			WorkerBase workerBase = this.stateTarget.Get<WorkerBase>(smi);
			smi.master.target.gameObject.GetSMI<WaterCooler.StatesInstance>().Drink(workerBase.gameObject, true);
		}

		// Token: 0x04001687 RID: 5767
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter drinker;

		// Token: 0x04001688 RID: 5768
		public StateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.TargetParameter chitchatlocator;

		// Token: 0x04001689 RID: 5769
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<WaterCooler> drink_move;

		// Token: 0x0400168A RID: 5770
		public WaterCoolerChore.States.DrinkStates drink;

		// Token: 0x0400168B RID: 5771
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x0400168C RID: 5772
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State chat;

		// Token: 0x0400168D RID: 5773
		public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State success;

		// Token: 0x0200077E RID: 1918
		public class DrinkStates : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State
		{
			// Token: 0x0400168E RID: 5774
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State drink;

			// Token: 0x0400168F RID: 5775
			public GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.State post;
		}
	}

	// Token: 0x0200077F RID: 1919
	public class StatesInstance : GameStateMachine<WaterCoolerChore.States, WaterCoolerChore.StatesInstance, WaterCoolerChore, object>.GameInstance
	{
		// Token: 0x06002180 RID: 8576 RVA: 0x000BA525 File Offset: 0x000B8725
		public StatesInstance(WaterCoolerChore master) : base(master)
		{
		}
	}
}
