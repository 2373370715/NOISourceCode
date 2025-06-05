using System;

// Token: 0x02000A51 RID: 2641
public class RanchableMonitor : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>
{
	// Token: 0x06002FB7 RID: 12215 RVA: 0x000C36FE File Offset: 0x000C18FE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetRanched, (RanchableMonitor.Instance smi) => smi.ShouldGoGetRanched(), null);
	}

	// Token: 0x02000A52 RID: 2642
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A53 RID: 2643
	public new class Instance : GameStateMachine<RanchableMonitor, RanchableMonitor.Instance, IStateMachineTarget, RanchableMonitor.Def>.GameInstance
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000C3741 File Offset: 0x000C1941
		// (set) Token: 0x06002FBB RID: 12219 RVA: 0x000C3749 File Offset: 0x000C1949
		public ChoreConsumer ChoreConsumer { get; private set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000C3752 File Offset: 0x000C1952
		public Navigator NavComponent
		{
			get
			{
				return this.navComponent;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000C375A File Offset: 0x000C195A
		public RanchedStates.Instance States
		{
			get
			{
				if (this.states == null)
				{
					this.states = this.controller.GetSMI<RanchedStates.Instance>();
				}
				return this.states;
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000C377B File Offset: 0x000C197B
		public Instance(IStateMachineTarget master, RanchableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.navComponent = base.GetComponent<Navigator>();
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000C379D File Offset: 0x000C199D
		public bool ShouldGoGetRanched()
		{
			return this.TargetRanchStation != null && this.TargetRanchStation.IsRunning() && this.TargetRanchStation.IsRancherReady;
		}

		// Token: 0x040020DD RID: 8413
		public RanchStation.Instance TargetRanchStation;

		// Token: 0x040020DE RID: 8414
		private Navigator navComponent;

		// Token: 0x040020DF RID: 8415
		private RanchedStates.Instance states;
	}
}
