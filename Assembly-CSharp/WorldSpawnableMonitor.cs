using System;

// Token: 0x02000A59 RID: 2649
public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
	// Token: 0x06002FD9 RID: 12249 RVA: 0x000C38E8 File Offset: 0x000C1AE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x02000A5A RID: 2650
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040020F1 RID: 8433
		public Func<int, int> adjustSpawnLocationCb;
	}

	// Token: 0x02000A5B RID: 2651
	public new class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
	{
		// Token: 0x06002FDC RID: 12252 RVA: 0x000C38FA File Offset: 0x000C1AFA
		public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def) : base(master, def)
		{
		}
	}
}
