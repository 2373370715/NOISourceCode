using System;

// Token: 0x020006A6 RID: 1702
public class DieChore : Chore<DieChore.StatesInstance>
{
	// Token: 0x06001E50 RID: 7760 RVA: 0x001BF938 File Offset: 0x001BDB38
	public DieChore(IStateMachineTarget master, Death death) : base(Db.Get().ChoreTypes.Die, master, master.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		this.showAvailabilityInHoverText = false;
		base.smi = new DieChore.StatesInstance(this, death);
	}

	// Token: 0x020006A7 RID: 1703
	public class StatesInstance : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.GameInstance
	{
		// Token: 0x06001E51 RID: 7761 RVA: 0x000B881F File Offset: 0x000B6A1F
		public StatesInstance(DieChore master, Death death) : base(master)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x001BF984 File Offset: 0x001BDB84
		public void PlayPreAnim()
		{
			string preAnim = base.sm.death.Get(base.smi).preAnim;
			base.GetComponent<KAnimControllerBase>().Play(preAnim, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x020006A8 RID: 1704
	public class States : GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore>
	{
		// Token: 0x06001E53 RID: 7763 RVA: 0x001BF9CC File Offset: 0x001BDBCC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.dying;
			this.dying.OnAnimQueueComplete(this.dead).Enter("PlayAnim", delegate(DieChore.StatesInstance smi)
			{
				smi.PlayPreAnim();
			});
			this.dead.ReturnSuccess();
		}

		// Token: 0x040013B9 RID: 5049
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dying;

		// Token: 0x040013BA RID: 5050
		public GameStateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.State dead;

		// Token: 0x040013BB RID: 5051
		public StateMachine<DieChore.States, DieChore.StatesInstance, DieChore, object>.ResourceParameter<Death> death;
	}
}
