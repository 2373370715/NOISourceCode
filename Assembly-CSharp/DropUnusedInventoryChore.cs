using System;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public class DropUnusedInventoryChore : Chore<DropUnusedInventoryChore.StatesInstance>
{
	// Token: 0x06002939 RID: 10553 RVA: 0x001E1FF4 File Offset: 0x001E01F4
	public DropUnusedInventoryChore(ChoreType chore_type, IStateMachineTarget target) : base(chore_type, target, target.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DropUnusedInventoryChore.StatesInstance(this);
	}

	// Token: 0x0200092D RID: 2349
	public class StatesInstance : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.GameInstance
	{
		// Token: 0x0600293A RID: 10554 RVA: 0x000BF482 File Offset: 0x000BD682
		public StatesInstance(DropUnusedInventoryChore master) : base(master)
		{
		}
	}

	// Token: 0x0200092E RID: 2350
	public class States : GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore>
	{
		// Token: 0x0600293B RID: 10555 RVA: 0x001E2028 File Offset: 0x001E0228
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dropping;
			this.dropping.Enter(delegate(DropUnusedInventoryChore.StatesInstance smi)
			{
				smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
			}).GoTo(this.success);
			this.success.ReturnSuccess();
		}

		// Token: 0x04001C0A RID: 7178
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State dropping;

		// Token: 0x04001C0B RID: 7179
		public GameStateMachine<DropUnusedInventoryChore.States, DropUnusedInventoryChore.StatesInstance, DropUnusedInventoryChore, object>.State success;
	}
}
