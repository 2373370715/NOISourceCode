using System;
using UnityEngine;

// Token: 0x0200074C RID: 1868
public class SighChore : Chore<SighChore.StatesInstance>
{
	// Token: 0x060020CD RID: 8397 RVA: 0x001C9AA0 File Offset: 0x001C7CA0
	public SighChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.Sigh, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new SighChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200074D RID: 1869
	public class StatesInstance : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.GameInstance
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x000B9F2F File Offset: 0x000B812F
		public StatesInstance(SighChore master, GameObject sigher) : base(master)
		{
			base.sm.sigher.Set(sigher, base.smi, false);
		}
	}

	// Token: 0x0200074E RID: 1870
	public class States : GameStateMachine<SighChore.States, SighChore.StatesInstance, SighChore>
	{
		// Token: 0x060020CF RID: 8399 RVA: 0x000B9F51 File Offset: 0x000B8151
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.sigher);
			this.root.PlayAnim("emote_depressed").OnAnimQueueComplete(null);
		}

		// Token: 0x040015DC RID: 5596
		public StateMachine<SighChore.States, SighChore.StatesInstance, SighChore, object>.TargetParameter sigher;
	}
}
