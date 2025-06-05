using System;

// Token: 0x020016A8 RID: 5800
[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
	// Token: 0x060077A6 RID: 30630 RVA: 0x000F3343 File Offset: 0x000F1543
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060077A7 RID: 30631 RVA: 0x0031BF68 File Offset: 0x0031A168
	protected bool IsUncomfortable()
	{
		if (!base.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
		{
			return false;
		}
		int num = 5;
		bool flag = true;
		bool flag2 = true;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 1; i < num; i++)
		{
			int num2 = Grid.OffsetCell(cell, i, 0);
			int num3 = Grid.OffsetCell(cell, -i, 0);
			if (Grid.Solid[num3])
			{
				flag = false;
			}
			if (Grid.Solid[num2])
			{
				flag2 = false;
			}
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				if (flag && Grid.PosToCell(minionIdentity.gameObject) == num3)
				{
					return true;
				}
				if (flag2 && Grid.PosToCell(minionIdentity.gameObject) == num2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x020016A9 RID: 5801
	public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
	{
		// Token: 0x060077A9 RID: 30633 RVA: 0x000F3358 File Offset: 0x000F1558
		public StatesInstance(SolitarySleeper master) : base(master)
		{
		}
	}

	// Token: 0x020016AA RID: 5802
	public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
	{
		// Token: 0x060077AA RID: 30634 RVA: 0x0031C064 File Offset: 0x0031A264
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.TagTransition(GameTags.Dead, null, false).EventTransition(GameHashes.NewDay, this.satisfied, null).Update("SolitarySleeperCheck", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					if (smi.GetCurrentState() != this.suffering)
					{
						smi.GoTo(this.suffering);
						return;
					}
				}
				else if (smi.GetCurrentState() != this.satisfied)
				{
					smi.GoTo(this.satisfied);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, null).Update("PeopleTooCloseSleepFail", delegate(SolitarySleeper.StatesInstance smi, float dt)
			{
				smi.master.gameObject.Trigger(1338475637, this);
			}, UpdateRate.SIM_1000ms, false);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A0E RID: 23054
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;

		// Token: 0x04005A0F RID: 23055
		public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;
	}
}
