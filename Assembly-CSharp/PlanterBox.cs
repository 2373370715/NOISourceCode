using System;

// Token: 0x02000F55 RID: 3925
[SkipSaveFileSerialization]
public class PlanterBox : StateMachineComponent<PlanterBox.SMInstance>
{
	// Token: 0x06004EB3 RID: 20147 RVA: 0x000D78F0 File Offset: 0x000D5AF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04003739 RID: 14137
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x02000F56 RID: 3926
	public class SMInstance : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.GameInstance
	{
		// Token: 0x06004EB5 RID: 20149 RVA: 0x000D790B File Offset: 0x000D5B0B
		public SMInstance(PlanterBox master) : base(master)
		{
		}
	}

	// Token: 0x02000F57 RID: 3927
	public class States : GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox>
	{
		// Token: 0x06004EB6 RID: 20150 RVA: 0x00277568 File Offset: 0x00275768
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (PlanterBox.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x0400373A RID: 14138
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State empty;

		// Token: 0x0400373B RID: 14139
		public GameStateMachine<PlanterBox.States, PlanterBox.SMInstance, PlanterBox, object>.State full;
	}
}
