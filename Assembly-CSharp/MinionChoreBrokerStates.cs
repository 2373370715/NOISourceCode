using System;

// Token: 0x020001DE RID: 478
public class MinionChoreBrokerStates : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>
{
	// Token: 0x06000682 RID: 1666 RVA: 0x00164B78 File Offset: 0x00162D78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.hasChore;
		this.root.DoNothing();
		this.hasChore.Enter(delegate(MinionChoreBrokerStates.Instance smi)
		{
		});
	}

	// Token: 0x040004C4 RID: 1220
	private GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State hasChore;

	// Token: 0x020001DF RID: 479
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001E0 RID: 480
	public new class Instance : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.GameInstance
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x000AD1F6 File Offset: 0x000AB3F6
		public Instance(Chore<MinionChoreBrokerStates.Instance> chore, MinionChoreBrokerStates.Def def) : base(chore, def)
		{
		}
	}
}
