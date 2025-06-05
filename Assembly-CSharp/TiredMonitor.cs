using System;

// Token: 0x02001663 RID: 5731
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
	// Token: 0x0600767E RID: 30334 RVA: 0x003188AC File Offset: 0x00316AAC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventTransition(GameHashes.SleepFail, this.tired, null);
		this.tired.Enter(delegate(TiredMonitor.Instance smi)
		{
			smi.SetInterruptDay();
		}).EventTransition(GameHashes.NewDay, (TiredMonitor.Instance smi) => GameClock.Instance, this.root, (TiredMonitor.Instance smi) => smi.AllowInterruptClear()).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f).ToggleAnims("anim_idle_slouch_kanim", 0f);
	}

	// Token: 0x0400591F RID: 22815
	public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

	// Token: 0x02001664 RID: 5732
	public new class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007680 RID: 30336 RVA: 0x000F26EE File Offset: 0x000F08EE
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007681 RID: 30337 RVA: 0x000F2705 File Offset: 0x000F0905
		public void SetInterruptDay()
		{
			this.interruptedDay = GameClock.Instance.GetCycle();
		}

		// Token: 0x06007682 RID: 30338 RVA: 0x000F2717 File Offset: 0x000F0917
		public bool AllowInterruptClear()
		{
			bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
			if (flag)
			{
				this.interruptedDay = -1;
			}
			return flag;
		}

		// Token: 0x04005920 RID: 22816
		public int disturbedDay = -1;

		// Token: 0x04005921 RID: 22817
		public int interruptedDay = -1;
	}
}
