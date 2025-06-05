using System;

// Token: 0x02000EB8 RID: 3768
public class MakeBaseSolid : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>
{
	// Token: 0x06004B52 RID: 19282 RVA: 0x000D5199 File Offset: 0x000D3399
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToSolid)).Exit(new StateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.State.Callback(MakeBaseSolid.ConvertToVacuum));
	}

	// Token: 0x06004B53 RID: 19283 RVA: 0x0026C780 File Offset: 0x0026A980
	private static void ConvertToSolid(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		Building component2 = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component2.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (smi.def.occupyFoundationLayer)
			{
				SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, component.Mass, component.Temperature, byte.MaxValue, 0, -1);
				Grid.Objects[num, 9] = smi.gameObject;
			}
			else
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, 0f, byte.MaxValue, 0, -1);
			}
			Grid.Foundation[num] = true;
			Grid.SetSolid(num, true, CellEventLogger.Instance.SimCellOccupierForceSolid);
			SimMessages.SetCellProperties(num, 103);
			Grid.RenderedByWorld[num] = false;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x06004B54 RID: 19284 RVA: 0x0026C8CC File Offset: 0x0026AACC
	private static void ConvertToVacuum(MakeBaseSolid.Instance smi)
	{
		if (smi.buildingComplete == null)
		{
			return;
		}
		int cell = Grid.PosToCell(smi.gameObject);
		Building component = smi.GetComponent<Building>();
		foreach (CellOffset offset in smi.def.solidOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierOnSpawn, 0f, -1f, byte.MaxValue, 0, -1);
			Grid.Objects[num, 9] = null;
			Grid.Foundation[num] = false;
			Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
			SimMessages.ClearCellProperties(num, 103);
			Grid.RenderedByWorld[num] = true;
			World.Instance.OnSolidChanged(num);
			GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
	}

	// Token: 0x040034B3 RID: 13491
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)103;

	// Token: 0x02000EB9 RID: 3769
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040034B4 RID: 13492
		public CellOffset[] solidOffsets;

		// Token: 0x040034B5 RID: 13493
		public bool occupyFoundationLayer = true;
	}

	// Token: 0x02000EBA RID: 3770
	public new class Instance : GameStateMachine<MakeBaseSolid, MakeBaseSolid.Instance, IStateMachineTarget, MakeBaseSolid.Def>.GameInstance
	{
		// Token: 0x06004B57 RID: 19287 RVA: 0x000D51E3 File Offset: 0x000D33E3
		public Instance(IStateMachineTarget master, MakeBaseSolid.Def def) : base(master, def)
		{
		}

		// Token: 0x040034B6 RID: 13494
		[MyCmpGet]
		public BuildingComplete buildingComplete;
	}
}
