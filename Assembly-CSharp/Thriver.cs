using System;

// Token: 0x02001A39 RID: 6713
[SkipSaveFileSerialization]
public class Thriver : StateMachineComponent<Thriver.StatesInstance>
{
	// Token: 0x06008BD8 RID: 35800 RVA: 0x000FFFF0 File Offset: 0x000FE1F0
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001A3A RID: 6714
	public class StatesInstance : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.GameInstance
	{
		// Token: 0x06008BDA RID: 35802 RVA: 0x00100005 File Offset: 0x000FE205
		public StatesInstance(Thriver master) : base(master)
		{
		}

		// Token: 0x06008BDB RID: 35803 RVA: 0x0036FAE4 File Offset: 0x0036DCE4
		public bool IsStressed()
		{
			StressMonitor.Instance smi = base.master.GetSMI<StressMonitor.Instance>();
			return smi != null && smi.IsStressed();
		}
	}

	// Token: 0x02001A3B RID: 6715
	public class States : GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver>
	{
		// Token: 0x06008BDC RID: 35804 RVA: 0x0036FB08 File Offset: 0x0036DD08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.EventTransition(GameHashes.NotStressed, this.idle, null).EventTransition(GameHashes.Stressed, this.stressed, null).EventTransition(GameHashes.StressedHadEnough, this.stressed, null).Enter(delegate(Thriver.StatesInstance smi)
			{
				StressMonitor.Instance smi2 = smi.master.GetSMI<StressMonitor.Instance>();
				if (smi2 != null && smi2.IsStressed())
				{
					smi.GoTo(this.stressed);
				}
			});
			this.idle.DoNothing();
			this.stressed.ToggleEffect("Thriver");
			this.toostressed.DoNothing();
		}

		// Token: 0x04006996 RID: 27030
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State idle;

		// Token: 0x04006997 RID: 27031
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State stressed;

		// Token: 0x04006998 RID: 27032
		public GameStateMachine<Thriver.States, Thriver.StatesInstance, Thriver, object>.State toostressed;
	}
}
