using System;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
	// Token: 0x06002104 RID: 8452 RVA: 0x001CAA38 File Offset: 0x001C8C38
	public StressIdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200075A RID: 1882
	public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
	{
		// Token: 0x06002105 RID: 8453 RVA: 0x000BA12F File Offset: 0x000B832F
		public StatesInstance(StressIdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
		}
	}

	// Token: 0x0200075B RID: 1883
	public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
	{
		// Token: 0x06002106 RID: 8454 RVA: 0x000BA151 File Offset: 0x000B8351
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
		}

		// Token: 0x04001612 RID: 5650
		public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;

		// Token: 0x04001613 RID: 5651
		public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;
	}
}
