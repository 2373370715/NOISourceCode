using System;

// Token: 0x02000D9F RID: 3487
public class FarmTile : StateMachineComponent<FarmTile.SMInstance>
{
	// Token: 0x060043C6 RID: 17350 RVA: 0x000D03EB File Offset: 0x000CE5EB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002EEE RID: 12014
	[MyCmpReq]
	private PlantablePlot plantablePlot;

	// Token: 0x04002EEF RID: 12015
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02000DA0 RID: 3488
	public class SMInstance : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.GameInstance
	{
		// Token: 0x060043C8 RID: 17352 RVA: 0x000D0406 File Offset: 0x000CE606
		public SMInstance(FarmTile master) : base(master)
		{
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x0025431C File Offset: 0x0025251C
		public bool HasWater()
		{
			PrimaryElement primaryElement = base.master.storage.FindPrimaryElement(SimHashes.Water);
			return primaryElement != null && primaryElement.Mass > 0f;
		}
	}

	// Token: 0x02000DA1 RID: 3489
	public class States : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile>
	{
		// Token: 0x060043CA RID: 17354 RVA: 0x00254358 File Offset: 0x00252558
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.EventTransition(GameHashes.OccupantChanged, this.full, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant != null);
			this.empty.wet.EventTransition(GameHashes.OnStorageChange, this.empty.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.empty.dry.EventTransition(GameHashes.OnStorageChange, this.empty.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.EventTransition(GameHashes.OccupantChanged, this.empty, (FarmTile.SMInstance smi) => smi.master.plantablePlot.Occupant == null);
			this.full.wet.EventTransition(GameHashes.OnStorageChange, this.full.dry, (FarmTile.SMInstance smi) => !smi.HasWater());
			this.full.dry.EventTransition(GameHashes.OnStorageChange, this.full.wet, (FarmTile.SMInstance smi) => !smi.HasWater());
		}

		// Token: 0x04002EF0 RID: 12016
		public FarmTile.States.FarmStates empty;

		// Token: 0x04002EF1 RID: 12017
		public FarmTile.States.FarmStates full;

		// Token: 0x02000DA2 RID: 3490
		public class FarmStates : GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State
		{
			// Token: 0x04002EF2 RID: 12018
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State wet;

			// Token: 0x04002EF3 RID: 12019
			public GameStateMachine<FarmTile.States, FarmTile.SMInstance, FarmTile, object>.State dry;
		}
	}
}
