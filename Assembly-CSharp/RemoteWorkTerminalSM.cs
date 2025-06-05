using System;

// Token: 0x02001796 RID: 6038
public class RemoteWorkTerminalSM : StateMachineComponent<RemoteWorkTerminalSM.StatesInstance>
{
	// Token: 0x06007C3E RID: 31806 RVA: 0x000F62AE File Offset: 0x000F44AE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04005D9E RID: 23966
	[MyCmpGet]
	private RemoteWorkTerminal terminal;

	// Token: 0x04005D9F RID: 23967
	[MyCmpGet]
	private Operational operational;

	// Token: 0x02001797 RID: 6039
	public class StatesInstance : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.GameInstance
	{
		// Token: 0x06007C40 RID: 31808 RVA: 0x000F62C9 File Offset: 0x000F44C9
		public StatesInstance(RemoteWorkTerminalSM master) : base(master)
		{
		}
	}

	// Token: 0x02001798 RID: 6040
	public class States : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM>
	{
		// Token: 0x06007C41 RID: 31809 RVA: 0x0032D704 File Offset: 0x0032B904
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			this.offline.Transition(this.online, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational)), UpdateRate.SIM_200ms).Transition(this.offline.no_dock, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock)), UpdateRate.SIM_200ms);
			this.offline.no_dock.Transition(this.offline, new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().BuildingStatusItems.RemoteWorkTerminalNoDock, null);
			this.online.ToggleRecurringChore(new Func<RemoteWorkTerminalSM.StatesInstance, Chore>(RemoteWorkTerminalSM.States.CreateChore), null).Transition(this.offline, GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Not(GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.And(new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.HasAssignedDock), new StateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.Transition.ConditionCallback(RemoteWorkTerminalSM.States.IsOperational))), UpdateRate.SIM_200ms);
		}

		// Token: 0x06007C42 RID: 31810 RVA: 0x000F62D2 File Offset: 0x000F44D2
		public static bool IsOperational(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06007C43 RID: 31811 RVA: 0x000F62E4 File Offset: 0x000F44E4
		public static bool HasAssignedDock(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return smi.master.terminal.CurrentDock != null;
		}

		// Token: 0x06007C44 RID: 31812 RVA: 0x000F62FC File Offset: 0x000F44FC
		public static Chore CreateChore(RemoteWorkTerminalSM.StatesInstance smi)
		{
			return new RemoteChore(smi.master.terminal);
		}

		// Token: 0x04005DA0 RID: 23968
		public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State online;

		// Token: 0x04005DA1 RID: 23969
		public RemoteWorkTerminalSM.States.OfflineStates offline;

		// Token: 0x02001799 RID: 6041
		public class OfflineStates : GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State
		{
			// Token: 0x04005DA2 RID: 23970
			public GameStateMachine<RemoteWorkTerminalSM.States, RemoteWorkTerminalSM.StatesInstance, RemoteWorkTerminalSM, object>.State no_dock;
		}
	}
}
