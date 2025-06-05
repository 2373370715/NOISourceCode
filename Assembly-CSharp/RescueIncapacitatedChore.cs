using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000734 RID: 1844
public class RescueIncapacitatedChore : Chore<RescueIncapacitatedChore.StatesInstance>
{
	// Token: 0x06002074 RID: 8308 RVA: 0x001C836C File Offset: 0x001C656C
	public RescueIncapacitatedChore(IStateMachineTarget master, GameObject incapacitatedDuplicant) : base(Db.Get().ChoreTypes.RescueIncapacitated, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RescueIncapacitatedChore.StatesInstance(this);
		this.runUntilComplete = true;
		this.AddPrecondition(ChorePreconditions.instance.NotChoreCreator, incapacitatedDuplicant.gameObject);
		this.AddPrecondition(RescueIncapacitatedChore.CanReachIncapacitated, incapacitatedDuplicant);
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x001C83D4 File Offset: 0x001C65D4
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rescuer.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.rescueTarget.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<BeIncapacitatedChore.StatesInstance>().master.GetChosenClinic(), base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000B9C2C File Offset: 0x000B7E2C
	protected override void End(string reason)
	{
		this.DropIncapacitatedDuplicant();
		base.End(reason);
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x001C8468 File Offset: 0x001C6668
	private void DropIncapacitatedDuplicant()
	{
		if (base.smi.sm.rescuer.Get(base.smi) != null && base.smi.sm.rescueTarget.Get(base.smi) != null)
		{
			base.smi.sm.rescuer.Get(base.smi).GetComponent<Storage>().Drop(base.smi.sm.rescueTarget.Get(base.smi), true);
		}
	}

	// Token: 0x04001592 RID: 5522
	public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition
	{
		id = "CanReachIncapacitated",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return false;
			}
			int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(gameObject.transform.GetPosition()));
			if (-1 != navigationCost)
			{
				context.cost += navigationCost;
				return true;
			}
			return false;
		}
	};

	// Token: 0x02000735 RID: 1845
	public class StatesInstance : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.GameInstance
	{
		// Token: 0x06002079 RID: 8313 RVA: 0x000B9C3B File Offset: 0x000B7E3B
		public StatesInstance(RescueIncapacitatedChore master) : base(master)
		{
		}
	}

	// Token: 0x02000736 RID: 1846
	public class States : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore>
	{
		// Token: 0x0600207A RID: 8314 RVA: 0x001C8550 File Offset: 0x001C6750
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachIncapacitated;
			this.approachIncapacitated.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset, null).Enter(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				DeathMonitor.Instance smi2 = this.rescueTarget.GetSMI<DeathMonitor.Instance>(smi);
				if (smi2 == null || smi2.IsDead())
				{
					smi.StopSM("target died");
				}
			});
			this.holding.Target(this.rescuer).Enter(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				smi.sm.rescueTarget.Get(smi).Subscribe(1623392196, delegate(object d)
				{
					smi.GoTo(this.holding.ditch);
				});
				GameObject gameObject = this.rescuer.Get(smi).gameObject;
				if (!gameObject.IsNullOrDestroyed() && gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					smi.master.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
					smi.master.GetComponent<KAnimControllerBase>().AddAnimOverrides(anim, 0f);
				}
			}).Exit(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				GameObject gameObject = this.rescuer.Get(smi).gameObject;
				if (!gameObject.IsNullOrDestroyed() && gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					smi.master.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
				}
			});
			this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				this.rescueTarget.Get(smi).gameObject.GetComponent<KBatchedAnimController>().Play("pickup", KAnim.PlayMode.Once, 1f, 0f);
			}).Exit(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				this.rescuer.Get(smi).GetComponent<Storage>().Store(this.rescueTarget.Get(smi), false, false, true, false);
				this.rescueTarget.Get(smi).transform.SetLocalPosition(Vector3.zero);
				KBatchedAnimTracker component = this.rescueTarget.Get(smi).GetComponent<KBatchedAnimTracker>();
				if (component != null)
				{
					component.symbol = new HashedString("snapTo_pivot");
					component.offset = new Vector3(0f, 0f, 1f);
				}
			}).EventTransition(GameHashes.AnimQueueComplete, this.holding.delivering, null);
			this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch, null, null).Enter(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				DeathMonitor.Instance smi2 = this.rescueTarget.GetSMI<DeathMonitor.Instance>(smi);
				if (smi2 == null || smi2.IsDead())
				{
					smi.StopSM("target died");
				}
			}).Update(delegate(RescueIncapacitatedChore.StatesInstance smi, float dt)
			{
				if (this.deliverTarget.Get(smi) == null)
				{
					smi.GoTo(this.holding.ditch);
				}
			}, UpdateRate.SIM_200ms, false);
			this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				smi.master.DropIncapacitatedDuplicant();
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("complete");
			});
			this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, this.failure).Exit(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				smi.master.DropIncapacitatedDuplicant();
			});
			this.failure.Enter(delegate(RescueIncapacitatedChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("failed");
			});
		}

		// Token: 0x04001593 RID: 5523
		public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.ApproachSubState<Chattable> approachIncapacitated;

		// Token: 0x04001594 RID: 5524
		public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State failure;

		// Token: 0x04001595 RID: 5525
		public RescueIncapacitatedChore.States.HoldingIncapacitated holding;

		// Token: 0x04001596 RID: 5526
		public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter rescueTarget;

		// Token: 0x04001597 RID: 5527
		public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter deliverTarget;

		// Token: 0x04001598 RID: 5528
		public StateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.TargetParameter rescuer;

		// Token: 0x02000737 RID: 1847
		public class HoldingIncapacitated : GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State
		{
			// Token: 0x04001599 RID: 5529
			public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State pickup;

			// Token: 0x0400159A RID: 5530
			public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.ApproachSubState<IApproachable> delivering;

			// Token: 0x0400159B RID: 5531
			public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State deposit;

			// Token: 0x0400159C RID: 5532
			public GameStateMachine<RescueIncapacitatedChore.States, RescueIncapacitatedChore.StatesInstance, RescueIncapacitatedChore, object>.State ditch;
		}
	}
}
