using System;

// Token: 0x02001696 RID: 5782
[SkipSaveFileSerialization]
public class Claustrophobic : StateMachineComponent<Claustrophobic.StatesInstance>
{
	// Token: 0x0600777C RID: 30588 RVA: 0x000F30F3 File Offset: 0x000F12F3
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600777D RID: 30589 RVA: 0x0031BBE4 File Offset: 0x00319DE4
	protected bool IsUncomfortable()
	{
		int num = 4;
		int cell = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = Grid.OffsetCell(cell, 0, i);
			if (Grid.IsValidCell(num2) && Grid.Solid[num2])
			{
				return true;
			}
			if (Grid.IsValidCell(Grid.CellRight(cell)) && Grid.IsValidCell(Grid.CellLeft(cell)) && Grid.Solid[Grid.CellRight(cell)] && Grid.Solid[Grid.CellLeft(cell)])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x02001697 RID: 5783
	public class StatesInstance : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.GameInstance
	{
		// Token: 0x0600777F RID: 30591 RVA: 0x000F3108 File Offset: 0x000F1308
		public StatesInstance(Claustrophobic master) : base(master)
		{
		}
	}

	// Token: 0x02001698 RID: 5784
	public class States : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic>
	{
		// Token: 0x06007780 RID: 30592 RVA: 0x0031BC70 File Offset: 0x00319E70
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClaustrophobicCheck", delegate(Claustrophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Claustrophobic").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A04 RID: 23044
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State satisfied;

		// Token: 0x04005A05 RID: 23045
		public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State suffering;
	}
}
