using System;
using UnityEngine;

// Token: 0x020006F3 RID: 1779
public class MoveChore : Chore<MoveChore.StatesInstance>
{
	// Token: 0x06001F96 RID: 8086 RVA: 0x001C5498 File Offset: 0x001C3698
	public MoveChore(IStateMachineTarget target, ChoreType chore_type, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveChore.StatesInstance(this, target.gameObject, get_cell_callback, update_cell);
	}

	// Token: 0x020006F4 RID: 1780
	public class StatesInstance : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.GameInstance
	{
		// Token: 0x06001F97 RID: 8087 RVA: 0x000B939F File Offset: 0x000B759F
		public StatesInstance(MoveChore master, GameObject mover, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(master)
		{
			this.getCellCallback = get_cell_callback;
			base.sm.mover.Set(mover, base.smi, false);
		}

		// Token: 0x040014DB RID: 5339
		public Func<MoveChore.StatesInstance, int> getCellCallback;
	}

	// Token: 0x020006F5 RID: 1781
	public class States : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore>
	{
		// Token: 0x06001F98 RID: 8088 RVA: 0x001C54D4 File Offset: 0x001C36D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.mover);
			this.root.MoveTo((MoveChore.StatesInstance smi) => smi.getCellCallback(smi), null, null, false);
		}

		// Token: 0x040014DC RID: 5340
		public GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040014DD RID: 5341
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter mover;

		// Token: 0x040014DE RID: 5342
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter locator;
	}
}
