using System;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class MoveToQuarantineChore : Chore<MoveToQuarantineChore.StatesInstance>
{
	// Token: 0x06001FB6 RID: 8118 RVA: 0x001C5E90 File Offset: 0x001C4090
	public MoveToQuarantineChore(IStateMachineTarget target, KMonoBehaviour quarantine_area) : base(Db.Get().ChoreTypes.MoveToQuarantine, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToQuarantineChore.StatesInstance(this, target.gameObject);
		base.smi.sm.locator.Set(quarantine_area.gameObject, base.smi, false);
	}

	// Token: 0x020006FF RID: 1791
	public class StatesInstance : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.GameInstance
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x000B945F File Offset: 0x000B765F
		public StatesInstance(MoveToQuarantineChore master, GameObject quarantined) : base(master)
		{
			base.sm.quarantined.Set(quarantined, base.smi, false);
		}
	}

	// Token: 0x02000700 RID: 1792
	public class States : GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore>
	{
		// Token: 0x06001FB8 RID: 8120 RVA: 0x000B9481 File Offset: 0x000B7681
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			this.approach.InitializeStates(this.quarantined, this.locator, this.success, null, null, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x040014F9 RID: 5369
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter locator;

		// Token: 0x040014FA RID: 5370
		public StateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.TargetParameter quarantined;

		// Token: 0x040014FB RID: 5371
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040014FC RID: 5372
		public GameStateMachine<MoveToQuarantineChore.States, MoveToQuarantineChore.StatesInstance, MoveToQuarantineChore, object>.State success;
	}
}
