using System;
using TUNING;
using UnityEngine;

// Token: 0x0200038E RID: 910
public class IceCooledFanConfig : IBuildingConfig
{
	// Token: 0x06000E9D RID: 3741 RVA: 0x001842D8 File Offset: 0x001824D8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceCooledFan";
		int width = 2;
		int height = 2;
		string anim = "fanice_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SelfHeatKilowattsWhenActive = -this.COOLING_RATE * 0.25f;
		buildingDef.ExhaustKilowattsWhenActive = -this.COOLING_RATE * 0.75f;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00184364 File Offset: 0x00182564
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 50f;
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 273.15f;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		IceCooledFan iceCooledFan = go.AddOrGet<IceCooledFan>();
		iceCooledFan.coolingRate = this.COOLING_RATE;
		iceCooledFan.targetTemperature = this.TARGET_TEMPERATURE;
		iceCooledFan.iceStorage = storage;
		iceCooledFan.liquidStorage = storage2;
		iceCooledFan.minCooledTemperature = 278.15f;
		iceCooledFan.minEnvironmentMass = 0.25f;
		iceCooledFan.minCoolingRange = new Vector2I(-2, 0);
		iceCooledFan.maxCoolingRange = new Vector2I(2, 4);
		iceCooledFan.consumptionTag = GameTags.IceOre;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.IceOre;
		manualDeliveryKG.capacity = this.ICE_CAPACITY;
		manualDeliveryKG.refillMass = this.ICE_CAPACITY * 0.2f;
		manualDeliveryKG.MinimumMass = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		go.AddOrGet<IceCooledFanWorkable>().overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_icefan_kanim")
		};
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x000B0B5E File Offset: 0x000AED5E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
			StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
			int cell = Grid.PosToCell(game_object);
			payload.OverrideExtents(new Extents(cell, IceCooledFanConfig.overrideOffsets));
			GameComps.StructureTemperatures.SetPayload(handle, ref payload);
		};
	}

	// Token: 0x04000AC0 RID: 2752
	public const string ID = "IceCooledFan";

	// Token: 0x04000AC1 RID: 2753
	private float COOLING_RATE = 32f;

	// Token: 0x04000AC2 RID: 2754
	private float TARGET_TEMPERATURE = 278.15f;

	// Token: 0x04000AC3 RID: 2755
	private float ICE_CAPACITY = 50f;

	// Token: 0x04000AC4 RID: 2756
	private static readonly CellOffset[] overrideOffsets = new CellOffset[]
	{
		new CellOffset(-2, 1),
		new CellOffset(2, 1),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};
}
