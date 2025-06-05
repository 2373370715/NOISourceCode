using System;

// Token: 0x0200158D RID: 5517
public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
	// Token: 0x060072D5 RID: 29397 RVA: 0x000EFAFD File Offset: 0x000EDCFD
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.HasDebugDestination, new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell), new Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell));
	}

	// Token: 0x060072D6 RID: 29398 RVA: 0x000EFB30 File Offset: 0x000EDD30
	private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		return smi.targetCell != Grid.InvalidCell;
	}

	// Token: 0x060072D7 RID: 29399 RVA: 0x000EFB42 File Offset: 0x000EDD42
	private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		smi.targetCell = Grid.InvalidCell;
	}

	// Token: 0x0200158E RID: 5518
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200158F RID: 5519
	public new class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x060072DA RID: 29402 RVA: 0x000EFB57 File Offset: 0x000EDD57
		public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x060072DB RID: 29403 RVA: 0x000EFB6C File Offset: 0x000EDD6C
		public void GoToCursor()
		{
			this.targetCell = DebugHandler.GetMouseCell();
		}

		// Token: 0x060072DC RID: 29404 RVA: 0x000EFB79 File Offset: 0x000EDD79
		public void GoToCell(int cellIndex)
		{
			this.targetCell = cellIndex;
		}

		// Token: 0x0400561F RID: 22047
		public int targetCell = Grid.InvalidCell;
	}
}
