using System;

// Token: 0x020016A5 RID: 5797
[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
	// Token: 0x0600779F RID: 30623 RVA: 0x000F32F5 File Offset: 0x000F14F5
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060077A0 RID: 30624 RVA: 0x0031BEB4 File Offset: 0x0031A0B4
	protected bool IsUncomfortable()
	{
		int num = Grid.CellBelow(Grid.PosToCell(base.gameObject));
		return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.Objects[num, 9] == null;
	}

	// Token: 0x020016A6 RID: 5798
	public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
	{
		// Token: 0x060077A2 RID: 30626 RVA: 0x000F330A File Offset: 0x000F150A
		public StatesInstance(SensitiveFeet master) : base(master)
		{
		}
	}

	// Token: 0x020016A7 RID: 5799
	public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
	{
		// Token: 0x060077A3 RID: 30627 RVA: 0x0031BF00 File Offset: 0x0031A100
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.Update("SensitiveFeetCheck", delegate(SensitiveFeet.StatesInstance smi, float dt)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}, UpdateRate.SIM_1000ms, false);
			this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A0C RID: 23052
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;

		// Token: 0x04005A0D RID: 23053
		public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;
	}
}
