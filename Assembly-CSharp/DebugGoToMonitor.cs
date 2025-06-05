using System;

// Token: 0x02001598 RID: 5528
public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	// Token: 0x060072FF RID: 29439 RVA: 0x0030E1F4 File Offset: 0x0030C3F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.hastarget.ToggleChore((DebugGoToMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex, false), this.satisfied);
	}

	// Token: 0x04005638 RID: 22072
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;

	// Token: 0x04005639 RID: 22073
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

	// Token: 0x02001599 RID: 5529
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200159A RID: 5530
	public new class Instance : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06007302 RID: 29442 RVA: 0x000EFD2E File Offset: 0x000EDF2E
		public Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06007303 RID: 29443 RVA: 0x0030E248 File Offset: 0x0030C448
		public void GoToCursor()
		{
			this.targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x06007304 RID: 29444 RVA: 0x0030E298 File Offset: 0x0030C498
		public void GoToCell(int cellIndex)
		{
			this.targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x0400563A RID: 22074
		public int targetCellIndex = Grid.InvalidCell;
	}
}
