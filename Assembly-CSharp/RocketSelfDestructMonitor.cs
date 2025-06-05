using System;

// Token: 0x0200120A RID: 4618
public class RocketSelfDestructMonitor : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance>
{
	// Token: 0x06005DC5 RID: 24005 RVA: 0x000E19E9 File Offset: 0x000DFBE9
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.RocketSelfDestructRequested, this.exploding, null);
		this.exploding.Update(delegate(RocketSelfDestructMonitor.Instance smi, float dt)
		{
			if (smi.timeinstate >= 3f)
			{
				smi.master.Trigger(-1311384361, null);
				smi.GoTo(this.idle);
			}
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040042E6 RID: 17126
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040042E7 RID: 17127
	public GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.State exploding;

	// Token: 0x0200120B RID: 4619
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200120C RID: 4620
	public new class Instance : GameStateMachine<RocketSelfDestructMonitor, RocketSelfDestructMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06005DC9 RID: 24009 RVA: 0x000E1A59 File Offset: 0x000DFC59
		public Instance(IStateMachineTarget master, RocketSelfDestructMonitor.Def def) : base(master)
		{
		}

		// Token: 0x040042E8 RID: 17128
		public KBatchedAnimController eyes;
	}
}
