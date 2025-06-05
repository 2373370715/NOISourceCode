using System;
using KSerialization;

// Token: 0x0200120D RID: 4621
public class RoverChoreMonitor : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
	// Token: 0x06005DCA RID: 24010 RVA: 0x002ADD68 File Offset: 0x002ABF68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (RoverChoreMonitor.Instance smi) => true, null).ToggleBehaviour(GameTags.Creatures.Builder, (RoverChoreMonitor.Instance smi) => true, null);
	}

	// Token: 0x040042E9 RID: 17129
	public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

	// Token: 0x0200120E RID: 4622
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200120F RID: 4623
	public new class Instance : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance
	{
		// Token: 0x06005DCD RID: 24013 RVA: 0x000E1A6A File Offset: 0x000DFC6A
		public Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005DCE RID: 24014 RVA: 0x000E1A7B File Offset: 0x000DFC7B
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x040042EA RID: 17130
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x040042EB RID: 17131
		private Action<object> OnDestinationReachedDelegate;
	}
}
