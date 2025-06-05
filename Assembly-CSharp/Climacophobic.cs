using System;
using UnityEngine;

// Token: 0x02001699 RID: 5785
[SkipSaveFileSerialization]
public class Climacophobic : StateMachineComponent<Climacophobic.StatesInstance>
{
	// Token: 0x06007783 RID: 30595 RVA: 0x000F3141 File Offset: 0x000F1341
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06007784 RID: 30596 RVA: 0x0031BCD8 File Offset: 0x00319ED8
	protected bool IsUncomfortable()
	{
		int num = 5;
		int cell = Grid.PosToCell(base.gameObject);
		if (this.isCellLadder(cell))
		{
			int num2 = 1;
			bool flag = true;
			bool flag2 = true;
			for (int i = 1; i < num; i++)
			{
				int cell2 = Grid.OffsetCell(cell, 0, i);
				int cell3 = Grid.OffsetCell(cell, 0, -i);
				if (flag && this.isCellLadder(cell2))
				{
					num2++;
				}
				else
				{
					flag = false;
				}
				if (flag2 && this.isCellLadder(cell3))
				{
					num2++;
				}
				else
				{
					flag2 = false;
				}
			}
			return num2 >= num;
		}
		return false;
	}

	// Token: 0x06007785 RID: 30597 RVA: 0x0031BD60 File Offset: 0x00319F60
	private bool isCellLadder(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		return !(gameObject == null) && !(gameObject.GetComponent<Ladder>() == null);
	}

	// Token: 0x0200169A RID: 5786
	public class StatesInstance : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.GameInstance
	{
		// Token: 0x06007787 RID: 30599 RVA: 0x000F3156 File Offset: 0x000F1356
		public StatesInstance(Climacophobic master) : base(master)
		{
		}
	}

	// Token: 0x0200169B RID: 5787
	public class States : GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic>
	{
		// Token: 0x06007788 RID: 30600 RVA: 0x0031BDA0 File Offset: 0x00319FA0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("ClimacophobicCheck", delegate(Climacophobic.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("Vertigo").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A06 RID: 23046
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State satisfied;

		// Token: 0x04005A07 RID: 23047
		public GameStateMachine<Climacophobic.States, Climacophobic.StatesInstance, Climacophobic, object>.State suffering;
	}
}
