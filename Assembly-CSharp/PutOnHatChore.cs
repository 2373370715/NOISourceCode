using System;
using UnityEngine;

// Token: 0x0200070D RID: 1805
public class PutOnHatChore : Chore<PutOnHatChore.StatesInstance>
{
	// Token: 0x06001FDE RID: 8158 RVA: 0x001C6718 File Offset: 0x001C4918
	public PutOnHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new PutOnHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200070E RID: 1806
	public class StatesInstance : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.GameInstance
	{
		// Token: 0x06001FDF RID: 8159 RVA: 0x000B95B9 File Offset: 0x000B77B9
		public StatesInstance(PutOnHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x0200070F RID: 1807
	public class States : GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore>
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x001C6754 File Offset: 0x001C4954
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.applyHat_pre;
			base.Target(this.duplicant);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(PutOnHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x0400151C RID: 5404
		public StateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.TargetParameter duplicant;

		// Token: 0x0400151D RID: 5405
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat_pre;

		// Token: 0x0400151E RID: 5406
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State applyHat;

		// Token: 0x0400151F RID: 5407
		public GameStateMachine<PutOnHatChore.States, PutOnHatChore.StatesInstance, PutOnHatChore, object>.State complete;
	}
}
