using System;
using Klei.AI;

// Token: 0x0200156B RID: 5483
public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	// Token: 0x0600724A RID: 29258 RVA: 0x0030C748 File Offset: 0x0030A948
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).Transition(this.breakwant, (BladderMonitor.Instance smi) => smi.WantsToPee(), UpdateRate.SIM_200ms);
		this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, null).ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleStateMachine((BladderMonitor.Instance smi) => new PeeChoreMonitor.Instance(smi.master)).ToggleEffect("FullBladder");
		this.breakwant.InitializeStates(this.satisfied);
		this.breakwant.wanting.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (BladderMonitor.Instance smi) => !smi.WantsToPee());
		this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, null);
	}

	// Token: 0x040055B8 RID: 21944
	public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040055B9 RID: 21945
	public BladderMonitor.WantsToPeeStates urgentwant;

	// Token: 0x040055BA RID: 21946
	public BladderMonitor.WantsToPeeStates breakwant;

	// Token: 0x0200156C RID: 5484
	public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0600724C RID: 29260 RVA: 0x0030C8C0 File Offset: 0x0030AAC0
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
		{
			base.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((BladderMonitor.Instance smi) => new ToiletMonitor.Instance(smi.master));
			this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (BladderMonitor.Instance smi) => smi.IsPeeing());
			this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (BladderMonitor.Instance smi) => !smi.IsPeeing());
			return this;
		}

		// Token: 0x040055BB RID: 21947
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;

		// Token: 0x040055BC RID: 21948
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;
	}

	// Token: 0x0200156E RID: 5486
	public new class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007253 RID: 29267 RVA: 0x000EF528 File Offset: 0x000ED728
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}

		// Token: 0x06007254 RID: 29268 RVA: 0x0030C97C File Offset: 0x0030AB7C
		public bool NeedsToPee()
		{
			if (base.master.IsNullOrDestroyed())
			{
				return false;
			}
			if (base.master.GetComponent<KPrefabID>().HasTag(GameTags.Asleep))
			{
				return false;
			}
			DebugUtil.DevAssert(this.bladder != null, "bladder is null", null);
			return this.bladder.value >= 100f;
		}

		// Token: 0x06007255 RID: 29269 RVA: 0x000EF55D File Offset: 0x000ED75D
		public bool WantsToPee()
		{
			return this.NeedsToPee() || (this.IsPeeTime() && this.bladder.value >= 40f);
		}

		// Token: 0x06007256 RID: 29270 RVA: 0x000EF588 File Offset: 0x000ED788
		public bool IsPeeing()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
		}

		// Token: 0x06007257 RID: 29271 RVA: 0x000EF5B8 File Offset: 0x000ED7B8
		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}

		// Token: 0x040055C1 RID: 21953
		private AmountInstance bladder;

		// Token: 0x040055C2 RID: 21954
		private ChoreDriver choreDriver;
	}
}
