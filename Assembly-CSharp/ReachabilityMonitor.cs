using System;

// Token: 0x02000B03 RID: 2819
public class ReachabilityMonitor : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable>
{
	// Token: 0x0600343F RID: 13375 RVA: 0x00216D5C File Offset: 0x00214F5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unreachable;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.FastUpdate("UpdateReachability", ReachabilityMonitor.updateReachabilityCB, UpdateRate.SIM_1000ms, true);
		this.reachable.ToggleTag(GameTags.Reachable).Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.unreachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsFalse);
		this.unreachable.Enter("TriggerEvent", delegate(ReachabilityMonitor.Instance smi)
		{
			smi.TriggerEvent();
		}).ParamTransition<bool>(this.isReachable, this.reachable, GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.IsTrue);
	}

	// Token: 0x040023C9 RID: 9161
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State reachable;

	// Token: 0x040023CA RID: 9162
	public GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.State unreachable;

	// Token: 0x040023CB RID: 9163
	public StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter isReachable = new StateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.BoolParameter(false);

	// Token: 0x040023CC RID: 9164
	private static ReachabilityMonitor.UpdateReachabilityCB updateReachabilityCB = new ReachabilityMonitor.UpdateReachabilityCB();

	// Token: 0x02000B04 RID: 2820
	private class UpdateReachabilityCB : UpdateBucketWithUpdater<ReachabilityMonitor.Instance>.IUpdater
	{
		// Token: 0x06003442 RID: 13378 RVA: 0x000C68F1 File Offset: 0x000C4AF1
		public void Update(ReachabilityMonitor.Instance smi, float dt)
		{
			smi.UpdateReachability();
		}
	}

	// Token: 0x02000B05 RID: 2821
	public new class Instance : GameStateMachine<ReachabilityMonitor, ReachabilityMonitor.Instance, Workable, object>.GameInstance
	{
		// Token: 0x06003444 RID: 13380 RVA: 0x000C68F9 File Offset: 0x000C4AF9
		public Instance(Workable workable) : base(workable)
		{
			this.UpdateReachability();
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x00216E28 File Offset: 0x00215028
		public void TriggerEvent()
		{
			bool flag = base.sm.isReachable.Get(base.smi);
			base.Trigger(-1432940121, flag);
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x00216E60 File Offset: 0x00215060
		public void UpdateReachability()
		{
			if (base.master == null)
			{
				return;
			}
			int cell = base.master.GetCell();
			base.sm.isReachable.Set(MinionGroupProber.Get().IsAllReachable(cell, base.master.GetOffsets(cell)), base.smi, false);
		}
	}
}
