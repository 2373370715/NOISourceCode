using System;

// Token: 0x020016AB RID: 5803
[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
	// Token: 0x060077AE RID: 30638 RVA: 0x000F3381 File Offset: 0x000F1581
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060077AF RID: 30639 RVA: 0x000F338E File Offset: 0x000F158E
	protected bool IsUncomfortable()
	{
		return base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
	}

	// Token: 0x020016AC RID: 5804
	public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
	{
		// Token: 0x060077B1 RID: 30641 RVA: 0x000F33B5 File Offset: 0x000F15B5
		public StatesInstance(Workaholic master) : base(master)
		{
		}
	}

	// Token: 0x020016AD RID: 5805
	public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
	{
		// Token: 0x060077B2 RID: 30642 RVA: 0x0031C150 File Offset: 0x0031A350
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("WorkaholicCheck", delegate(Workaholic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A10 RID: 23056
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;

		// Token: 0x04005A11 RID: 23057
		public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;
	}
}
