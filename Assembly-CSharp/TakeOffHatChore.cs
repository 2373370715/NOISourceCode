using System;
using UnityEngine;

// Token: 0x02000768 RID: 1896
public class TakeOffHatChore : Chore<TakeOffHatChore.StatesInstance>
{
	// Token: 0x0600213E RID: 8510 RVA: 0x001CC298 File Offset: 0x001CA498
	public TakeOffHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new TakeOffHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02000769 RID: 1897
	public class StatesInstance : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.GameInstance
	{
		// Token: 0x0600213F RID: 8511 RVA: 0x000BA307 File Offset: 0x000B8507
		public StatesInstance(TakeOffHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x0200076A RID: 1898
	public class States : GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore>
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x001CC2D4 File Offset: 0x001CA4D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.remove_hat_pre;
			base.Target(this.duplicant);
			this.remove_hat_pre.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat != null)
				{
					smi.GoTo(this.remove_hat);
					return;
				}
				smi.GoTo(this.complete);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.complete);
			this.complete.Enter(delegate(TakeOffHatChore.StatesInstance smi)
			{
				smi.master.GetComponent<MinionResume>().RemoveHat();
			}).ReturnSuccess();
		}

		// Token: 0x0400164A RID: 5706
		public StateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.TargetParameter duplicant;

		// Token: 0x0400164B RID: 5707
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat_pre;

		// Token: 0x0400164C RID: 5708
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State remove_hat;

		// Token: 0x0400164D RID: 5709
		public GameStateMachine<TakeOffHatChore.States, TakeOffHatChore.StatesInstance, TakeOffHatChore, object>.State complete;
	}
}
