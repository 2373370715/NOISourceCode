using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A93 RID: 6803
public class WaterTrapConfig : IBuildingConfig
{
	// Token: 0x06008DE2 RID: 36322 RVA: 0x00377A8C File Offset: 0x00375C8C
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("WaterTrap", 1, 2, "critter_trap_water_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.RAW_METALS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.INPUT_LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("TRAP_HAS_PREY_STATUS_PORT", new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.REUSABLETRAP.LOGIC_PORT_INACTIVE, false, false)
		};
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x06008DE3 RID: 36323 RVA: 0x00377B8C File Offset: 0x00375D8C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<Prioritizable>();
		Prioritizable.AddRef(go);
		ArmTrapWorkable armTrapWorkable = go.AddOrGet<ArmTrapWorkable>();
		armTrapWorkable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_critter_trap_water_kanim")
		};
		armTrapWorkable.initialOffsets = new CellOffset[]
		{
			new CellOffset(0, 1)
		};
		go.AddOrGet<Operational>();
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.BlockingTileVisible = false;
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(WaterTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		go.AddOrGetDef<WaterTrapTrail.Def>();
		ReusableTrap.Def def = go.AddOrGetDef<ReusableTrap.Def>();
		def.releaseCellOffset = new CellOffset(0, 1);
		def.OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";
		def.lures = new Tag[]
		{
			GameTags.Creatures.FishTrapLure
		};
		def.usingSymbolChaseCapturingAnimations = true;
		def.getTrappedAnimationNameCallback = (() => "trapped");
		go.AddOrGet<LogicPorts>();
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06008DE4 RID: 36324 RVA: 0x00377CDC File Offset: 0x00375EDC
	private static void AddGuide(GameObject go, bool occupy_tiles)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = go.transform;
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(new HashedString("critter_trap_water_kanim"))
		};
		kbatchedAnimController.initialAnim = "place_guide";
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
		kbatchedAnimController.isMovable = true;
		WaterTrapGuide waterTrapGuide = gameObject.AddComponent<WaterTrapGuide>();
		waterTrapGuide.parent = go;
		waterTrapGuide.occupyTiles = occupy_tiles;
	}

	// Token: 0x06008DE5 RID: 36325 RVA: 0x00377D78 File Offset: 0x00375F78
	public override void DoPostConfigureComplete(GameObject go)
	{
		WaterTrapConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
		WaterTrapConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, false);
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		go.AddOrGet<FakeFloorAdder>().floorOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
	}

	// Token: 0x04006B22 RID: 27426
	public const string ID = "WaterTrap";

	// Token: 0x04006B23 RID: 27427
	public const string OUTPUT_LOGIC_PORT_ID = "TRAP_HAS_PREY_STATUS_PORT";

	// Token: 0x04006B24 RID: 27428
	public const int TRAIL_LENGTH = 4;

	// Token: 0x04006B25 RID: 27429
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
