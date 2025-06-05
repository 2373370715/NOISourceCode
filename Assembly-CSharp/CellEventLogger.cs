using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02001309 RID: 4873
public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
	// Token: 0x060063DD RID: 25565 RVA: 0x000E5A7F File Offset: 0x000E3C7F
	public static void DestroyInstance()
	{
		CellEventLogger.Instance = null;
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x000E5A87 File Offset: 0x000E3C87
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackSend(int cell, int callback_id)
	{
		if (callback_id != -1)
		{
			this.CallbackToCellMap[callback_id] = cell;
		}
	}

	// Token: 0x060063DF RID: 25567 RVA: 0x002C9A64 File Offset: 0x002C7C64
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackReceive(int callback_id)
	{
		int invalidCell = Grid.InvalidCell;
		this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell);
	}

	// Token: 0x060063E0 RID: 25568 RVA: 0x002C9A88 File Offset: 0x002C7C88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CellEventLogger.Instance = this;
		this.SimMessagesSolid = (base.AddEvent(new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent);
		this.SimCellOccupierDestroy = (base.AddEvent(new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent);
		this.SimCellOccupierForceSolid = (base.AddEvent(new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent);
		this.SimCellOccupierSolidChanged = (base.AddEvent(new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent);
		this.DoorOpen = (base.AddEvent(new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent);
		this.DoorClose = (base.AddEvent(new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent);
		this.Excavator = (base.AddEvent(new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent);
		this.DebugTool = (base.AddEvent(new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent);
		this.SandBoxTool = (base.AddEvent(new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent);
		this.TemplateLoader = (base.AddEvent(new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent);
		this.Scenario = (base.AddEvent(new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent);
		this.SimCellOccupierOnSpawn = (base.AddEvent(new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent);
		this.SimCellOccupierDestroySelf = (base.AddEvent(new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent);
		this.WorldGapManager = (base.AddEvent(new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent);
		this.ReceiveElementChanged = (base.AddEvent(new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent);
		this.ObjectSetSimOnSpawn = (base.AddEvent(new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent);
		this.DecompositionDirtyWater = (base.AddEvent(new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent);
		this.SendCallback = (base.AddEvent(new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent);
		this.ReceiveCallback = (base.AddEvent(new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent);
		this.Dig = (base.AddEvent(new CellDigEvent(true)) as CellDigEvent);
		this.WorldDamageDelayedSpawnFX = (base.AddEvent(new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent);
		this.OxygenModifierSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.LiquidChunkOnStore = (base.AddEvent(new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent);
		this.FallingWaterAddToSim = (base.AddEvent(new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent);
		this.ExploderOnSpawn = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent);
		this.ExhaustSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.ElementConsumerSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.SublimatesEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent);
		this.Mop = (base.AddEvent(new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent);
		this.OreMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent);
		this.ConstructTile = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent);
		this.Dumpable = (base.AddEvent(new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent);
		this.Cough = (base.AddEvent(new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent);
		this.Meteor = (base.AddEvent(new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent);
		this.ElementChunkTransition = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent);
		this.OxyrockEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent);
		this.BleachstoneEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent);
		this.UnstableGround = (base.AddEvent(new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitFlowEmptyConduit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitConsumerWrongElement = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent);
		this.OverheatableMeltingDown = (base.AddEvent(new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent);
		this.FabricatorProduceMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent);
		this.PumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.WallPumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.Vomit = (base.AddEvent(new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent);
		this.Tears = (base.AddEvent(new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent);
		this.Pee = (base.AddEvent(new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent);
		this.AlgaeHabitat = (base.AddEvent(new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent);
		this.CO2FilterOxygen = (base.AddEvent(new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent);
		this.ToiletEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent);
		this.ElementEmitted = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent);
		this.CO2ManagerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent);
		this.EnvironmentConsumerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent);
		this.ExcavatorShockwave = (base.AddEvent(new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent);
		this.OxygenBreatherSimUpdate = (base.AddEvent(new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent);
		this.CO2ScrubberSimUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent);
		this.RiverSourceSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent);
		this.RiverTerminusSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent);
		this.DebugToolModifyMass = (base.AddEvent(new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent);
		this.EnergyGeneratorModifyMass = (base.AddEvent(new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent);
		this.SolidFilterEvent = (base.AddEvent(new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent);
	}

	// Token: 0x0400478C RID: 18316
	public static CellEventLogger Instance;

	// Token: 0x0400478D RID: 18317
	public CellSolidEvent SimMessagesSolid;

	// Token: 0x0400478E RID: 18318
	public CellSolidEvent SimCellOccupierDestroy;

	// Token: 0x0400478F RID: 18319
	public CellSolidEvent SimCellOccupierForceSolid;

	// Token: 0x04004790 RID: 18320
	public CellSolidEvent SimCellOccupierSolidChanged;

	// Token: 0x04004791 RID: 18321
	public CellElementEvent DoorOpen;

	// Token: 0x04004792 RID: 18322
	public CellElementEvent DoorClose;

	// Token: 0x04004793 RID: 18323
	public CellElementEvent Excavator;

	// Token: 0x04004794 RID: 18324
	public CellElementEvent DebugTool;

	// Token: 0x04004795 RID: 18325
	public CellElementEvent SandBoxTool;

	// Token: 0x04004796 RID: 18326
	public CellElementEvent TemplateLoader;

	// Token: 0x04004797 RID: 18327
	public CellElementEvent Scenario;

	// Token: 0x04004798 RID: 18328
	public CellElementEvent SimCellOccupierOnSpawn;

	// Token: 0x04004799 RID: 18329
	public CellElementEvent SimCellOccupierDestroySelf;

	// Token: 0x0400479A RID: 18330
	public CellElementEvent WorldGapManager;

	// Token: 0x0400479B RID: 18331
	public CellElementEvent ReceiveElementChanged;

	// Token: 0x0400479C RID: 18332
	public CellElementEvent ObjectSetSimOnSpawn;

	// Token: 0x0400479D RID: 18333
	public CellElementEvent DecompositionDirtyWater;

	// Token: 0x0400479E RID: 18334
	public CellElementEvent LaunchpadDesolidify;

	// Token: 0x0400479F RID: 18335
	public CellCallbackEvent SendCallback;

	// Token: 0x040047A0 RID: 18336
	public CellCallbackEvent ReceiveCallback;

	// Token: 0x040047A1 RID: 18337
	public CellDigEvent Dig;

	// Token: 0x040047A2 RID: 18338
	public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;

	// Token: 0x040047A3 RID: 18339
	public CellAddRemoveSubstanceEvent SublimatesEmit;

	// Token: 0x040047A4 RID: 18340
	public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;

	// Token: 0x040047A5 RID: 18341
	public CellAddRemoveSubstanceEvent LiquidChunkOnStore;

	// Token: 0x040047A6 RID: 18342
	public CellAddRemoveSubstanceEvent FallingWaterAddToSim;

	// Token: 0x040047A7 RID: 18343
	public CellAddRemoveSubstanceEvent ExploderOnSpawn;

	// Token: 0x040047A8 RID: 18344
	public CellAddRemoveSubstanceEvent ExhaustSimUpdate;

	// Token: 0x040047A9 RID: 18345
	public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;

	// Token: 0x040047AA RID: 18346
	public CellAddRemoveSubstanceEvent ElementChunkTransition;

	// Token: 0x040047AB RID: 18347
	public CellAddRemoveSubstanceEvent OxyrockEmit;

	// Token: 0x040047AC RID: 18348
	public CellAddRemoveSubstanceEvent BleachstoneEmit;

	// Token: 0x040047AD RID: 18349
	public CellAddRemoveSubstanceEvent UnstableGround;

	// Token: 0x040047AE RID: 18350
	public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;

	// Token: 0x040047AF RID: 18351
	public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;

	// Token: 0x040047B0 RID: 18352
	public CellAddRemoveSubstanceEvent OverheatableMeltingDown;

	// Token: 0x040047B1 RID: 18353
	public CellAddRemoveSubstanceEvent FabricatorProduceMelted;

	// Token: 0x040047B2 RID: 18354
	public CellAddRemoveSubstanceEvent PumpSimUpdate;

	// Token: 0x040047B3 RID: 18355
	public CellAddRemoveSubstanceEvent WallPumpSimUpdate;

	// Token: 0x040047B4 RID: 18356
	public CellAddRemoveSubstanceEvent Vomit;

	// Token: 0x040047B5 RID: 18357
	public CellAddRemoveSubstanceEvent Tears;

	// Token: 0x040047B6 RID: 18358
	public CellAddRemoveSubstanceEvent Pee;

	// Token: 0x040047B7 RID: 18359
	public CellAddRemoveSubstanceEvent AlgaeHabitat;

	// Token: 0x040047B8 RID: 18360
	public CellAddRemoveSubstanceEvent CO2FilterOxygen;

	// Token: 0x040047B9 RID: 18361
	public CellAddRemoveSubstanceEvent ToiletEmit;

	// Token: 0x040047BA RID: 18362
	public CellAddRemoveSubstanceEvent ElementEmitted;

	// Token: 0x040047BB RID: 18363
	public CellAddRemoveSubstanceEvent Mop;

	// Token: 0x040047BC RID: 18364
	public CellAddRemoveSubstanceEvent OreMelted;

	// Token: 0x040047BD RID: 18365
	public CellAddRemoveSubstanceEvent ConstructTile;

	// Token: 0x040047BE RID: 18366
	public CellAddRemoveSubstanceEvent Dumpable;

	// Token: 0x040047BF RID: 18367
	public CellAddRemoveSubstanceEvent Cough;

	// Token: 0x040047C0 RID: 18368
	public CellAddRemoveSubstanceEvent Meteor;

	// Token: 0x040047C1 RID: 18369
	public CellModifyMassEvent CO2ManagerFixedUpdate;

	// Token: 0x040047C2 RID: 18370
	public CellModifyMassEvent EnvironmentConsumerFixedUpdate;

	// Token: 0x040047C3 RID: 18371
	public CellModifyMassEvent ExcavatorShockwave;

	// Token: 0x040047C4 RID: 18372
	public CellModifyMassEvent OxygenBreatherSimUpdate;

	// Token: 0x040047C5 RID: 18373
	public CellModifyMassEvent CO2ScrubberSimUpdate;

	// Token: 0x040047C6 RID: 18374
	public CellModifyMassEvent RiverSourceSimUpdate;

	// Token: 0x040047C7 RID: 18375
	public CellModifyMassEvent RiverTerminusSimUpdate;

	// Token: 0x040047C8 RID: 18376
	public CellModifyMassEvent DebugToolModifyMass;

	// Token: 0x040047C9 RID: 18377
	public CellModifyMassEvent EnergyGeneratorModifyMass;

	// Token: 0x040047CA RID: 18378
	public CellSolidFilterEvent SolidFilterEvent;

	// Token: 0x040047CB RID: 18379
	public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
}
