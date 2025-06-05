using System;
using UnityEngine;

// Token: 0x02000761 RID: 1889
public class SwitchRoleHatChore : Chore<SwitchRoleHatChore.StatesInstance>
{
	// Token: 0x0600212C RID: 8492 RVA: 0x001CBE48 File Offset: 0x001CA048
	public SwitchRoleHatChore(IStateMachineTarget target, ChoreType chore_type) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SwitchRoleHatChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02000762 RID: 1890
	public class StatesInstance : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.GameInstance
	{
		// Token: 0x0600212D RID: 8493 RVA: 0x000BA244 File Offset: 0x000B8444
		public StatesInstance(SwitchRoleHatChore master, GameObject duplicant) : base(master)
		{
			base.sm.duplicant.Set(duplicant, base.smi, false);
		}
	}

	// Token: 0x02000763 RID: 1891
	public class States : GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore>
	{
		// Token: 0x0600212E RID: 8494 RVA: 0x001CBE84 File Offset: 0x001CA084
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.start;
			base.Target(this.duplicant);
			this.start.Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				if (this.duplicant.Get(smi).GetComponent<MinionResume>().CurrentHat == null)
				{
					smi.GoTo(this.delay);
					return;
				}
				smi.GoTo(this.remove_hat);
			});
			this.remove_hat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("hat_off").OnAnimQueueComplete(this.delay);
			this.delay.ToggleThought(Db.Get().Thoughts.NewRole, null).ToggleExpression(Db.Get().Expressions.Happy, null).ToggleAnims("anim_selfish_kanim", 0f).QueueAnim("working_pre", false, null).QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.applyHat_pre);
			this.applyHat_pre.ToggleAnims("anim_hat_kanim", 0f).Enter(delegate(SwitchRoleHatChore.StatesInstance smi)
			{
				this.duplicant.Get(smi).GetComponent<MinionResume>().ApplyTargetHat();
			}).PlayAnim("hat_first").OnAnimQueueComplete(this.applyHat);
			this.applyHat.ToggleAnims("anim_hat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.complete);
			this.complete.ReturnSuccess();
		}

		// Token: 0x04001636 RID: 5686
		public StateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.TargetParameter duplicant;

		// Token: 0x04001637 RID: 5687
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State remove_hat;

		// Token: 0x04001638 RID: 5688
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State start;

		// Token: 0x04001639 RID: 5689
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay;

		// Token: 0x0400163A RID: 5690
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State delay_pst;

		// Token: 0x0400163B RID: 5691
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat_pre;

		// Token: 0x0400163C RID: 5692
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State applyHat;

		// Token: 0x0400163D RID: 5693
		public GameStateMachine<SwitchRoleHatChore.States, SwitchRoleHatChore.StatesInstance, SwitchRoleHatChore, object>.State complete;
	}
}
