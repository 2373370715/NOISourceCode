using System;

// Token: 0x02000DB0 RID: 3504
public class FlowerVase : StateMachineComponent<FlowerVase.SMInstance>
{
	// Token: 0x0600441B RID: 17435 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600441C RID: 17436 RVA: 0x000D0768 File Offset: 0x000CE968
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002F2F RID: 12079
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04002F30 RID: 12080
	[MyCmpReq]
	private KBoxCollider2D boxCollider;

	// Token: 0x02000DB1 RID: 3505
	public class SMInstance : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.GameInstance
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x000D0783 File Offset: 0x000CE983
		public SMInstance(FlowerVase master) : base(master)
		{
		}
	}

	// Token: 0x02000DB2 RID: 3506
	public class States : GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase>
	{
		// Token: 0x0600441F RID: 17439 RVA: 0x00255538 File Offset: 0x00253738
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant != null).PlayAnim("off");
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FlowerVase.SMInstance smi) => smi.master.plantablePlot.Occupant == null).PlayAnim("on");
		}

		// Token: 0x04002F31 RID: 12081
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State empty;

		// Token: 0x04002F32 RID: 12082
		public GameStateMachine<FlowerVase.States, FlowerVase.SMInstance, FlowerVase, object>.State full;
	}
}
