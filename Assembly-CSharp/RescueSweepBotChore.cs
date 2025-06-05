using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200073B RID: 1851
public class RescueSweepBotChore : Chore<RescueSweepBotChore.StatesInstance>
{
	// Token: 0x0600208E RID: 8334 RVA: 0x001C8974 File Offset: 0x001C6B74
	public RescueSweepBotChore(IStateMachineTarget master, GameObject sweepBot, GameObject baseStation)
	{
		Chore.Precondition canReachBaseStation = default(Chore.Precondition);
		canReachBaseStation.id = "CanReachBaseStation";
		canReachBaseStation.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		canReachBaseStation.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			return !(kmonoBehaviour == null) && context.consumerState.consumer.navigator.CanReach(Grid.PosToCell(kmonoBehaviour));
		};
		this.CanReachBaseStation = canReachBaseStation;
		base..ctor(Db.Get().ChoreTypes.RescueIncapacitated, master, null, false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		base.smi = new RescueSweepBotChore.StatesInstance(this);
		this.runUntilComplete = true;
		this.AddPrecondition(RescueSweepBotChore.CanReachIncapacitated, sweepBot.GetComponent<Storage>());
		this.AddPrecondition(this.CanReachBaseStation, baseStation.GetComponent<Storage>());
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x001C8A2C File Offset: 0x001C6C2C
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.rescuer.Set(context.consumerState.gameObject, base.smi, false);
		base.smi.sm.rescueTarget.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverTarget.Set(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>().sm.GetSweepLocker(this.gameObject.GetSMI<SweepBotTrappedStates.Instance>()).gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x06002090 RID: 8336 RVA: 0x000B9D22 File Offset: 0x000B7F22
	protected override void End(string reason)
	{
		this.DropSweepBot();
		base.End(reason);
	}

	// Token: 0x06002091 RID: 8337 RVA: 0x001C8AD0 File Offset: 0x001C6CD0
	private void DropSweepBot()
	{
		if (base.smi.sm.rescuer.Get(base.smi) != null && base.smi.sm.rescueTarget.Get(base.smi) != null)
		{
			base.smi.sm.rescuer.Get(base.smi).GetComponent<Storage>().Drop(base.smi.sm.rescueTarget.Get(base.smi), true);
		}
	}

	// Token: 0x040015A4 RID: 5540
	public Chore.Precondition CanReachBaseStation;

	// Token: 0x040015A5 RID: 5541
	public static Chore.Precondition CanReachIncapacitated = new Chore.Precondition
	{
		id = "CanReachIncapacitated",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			if (kmonoBehaviour == null)
			{
				return false;
			}
			int navigationCost = context.consumerState.navigator.GetNavigationCost(Grid.PosToCell(kmonoBehaviour.transform.GetPosition()));
			if (-1 != navigationCost)
			{
				context.cost += navigationCost;
				return true;
			}
			return false;
		}
	};

	// Token: 0x0200073C RID: 1852
	public class StatesInstance : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.GameInstance
	{
		// Token: 0x06002093 RID: 8339 RVA: 0x000B9D31 File Offset: 0x000B7F31
		public StatesInstance(RescueSweepBotChore master) : base(master)
		{
		}
	}

	// Token: 0x0200073D RID: 1853
	public class States : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore>
	{
		// Token: 0x06002094 RID: 8340 RVA: 0x001C8BB8 File Offset: 0x001C6DB8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approachSweepBot;
			this.approachSweepBot.InitializeStates(this.rescuer, this.rescueTarget, this.holding.pickup, this.failure, Grid.DefaultOffset, null);
			this.holding.Target(this.rescuer).Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().AddAnimOverrides(anim, 0f);
				}
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				if (this.rescuer.Get(smi).gameObject.HasTag(GameTags.BaseMinion))
				{
					KAnimFile anim = Assets.GetAnim("anim_incapacitated_carrier_kanim");
					this.rescuer.Get(smi).GetComponent<KAnimControllerBase>().RemoveAnimOverrides(anim);
				}
			});
			this.holding.pickup.Target(this.rescuer).PlayAnim("pickup").Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
			}).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
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
			this.holding.delivering.InitializeStates(this.rescuer, this.deliverTarget, this.holding.deposit, this.holding.ditch, null, null).Update(delegate(RescueSweepBotChore.StatesInstance smi, float dt)
			{
				if (this.deliverTarget.Get(smi) == null)
				{
					smi.GoTo(this.holding.ditch);
				}
			}, UpdateRate.SIM_200ms, false);
			this.holding.deposit.PlayAnim("place").EventHandler(GameHashes.AnimQueueComplete, delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("complete");
			});
			this.holding.ditch.PlayAnim("place").ScheduleGoTo(0.5f, this.failure).Exit(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.master.DropSweepBot();
			});
			this.failure.Enter(delegate(RescueSweepBotChore.StatesInstance smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("failed");
			});
		}

		// Token: 0x040015A6 RID: 5542
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<Storage> approachSweepBot;

		// Token: 0x040015A7 RID: 5543
		public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State failure;

		// Token: 0x040015A8 RID: 5544
		public RescueSweepBotChore.States.HoldingSweepBot holding;

		// Token: 0x040015A9 RID: 5545
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescueTarget;

		// Token: 0x040015AA RID: 5546
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter deliverTarget;

		// Token: 0x040015AB RID: 5547
		public StateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.TargetParameter rescuer;

		// Token: 0x0200073E RID: 1854
		public class HoldingSweepBot : GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State
		{
			// Token: 0x040015AC RID: 5548
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State pickup;

			// Token: 0x040015AD RID: 5549
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.ApproachSubState<IApproachable> delivering;

			// Token: 0x040015AE RID: 5550
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State deposit;

			// Token: 0x040015AF RID: 5551
			public GameStateMachine<RescueSweepBotChore.States, RescueSweepBotChore.StatesInstance, RescueSweepBotChore, object>.State ditch;
		}
	}
}
