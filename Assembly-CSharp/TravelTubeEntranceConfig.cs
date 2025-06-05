using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class TravelTubeEntranceConfig : IBuildingConfig
{
	// Token: 0x06001A8A RID: 6794 RVA: 0x001B4288 File Offset: 0x001B2488
	public override BuildingDef CreateBuildingDef()
	{
		string id = "TravelTubeEntrance";
		int width = 3;
		int height = 2;
		string anim = "tube_launcher_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 1));
		return buildingDef;
	}

	// Token: 0x06001A8B RID: 6795 RVA: 0x001B4318 File Offset: 0x001B2518
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		TravelTubeEntrance travelTubeEntrance = go.AddOrGet<TravelTubeEntrance>();
		travelTubeEntrance.waxPerLaunch = 0.05f;
		travelTubeEntrance.joulesPerLaunch = 10000f;
		travelTubeEntrance.jouleCapacity = 40000f;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 10f;
		List<Storage.StoredItemModifier> defaultStoredItemModifiers = new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Preserve
		};
		storage.SetDefaultStoredItemModifiers(defaultStoredItemModifiers);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.requestedItemTag = SimHashes.MilkFat.CreateTag();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.Fetch.IdHash;
		manualDeliveryKG.capacity = storage.capacityKg;
		manualDeliveryKG.refillMass = 0.05f;
		manualDeliveryKG.SetStorage(storage);
		go.AddOrGet<TravelTubeEntrance.Work>();
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<EnergyConsumerSelfSustaining>();
	}

	// Token: 0x06001A8C RID: 6796 RVA: 0x000B5B44 File Offset: 0x000B3D44
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
	}

	// Token: 0x04001122 RID: 4386
	public const string ID = "TravelTubeEntrance";

	// Token: 0x04001123 RID: 4387
	public const float WAX_PER_LAUNCH = 0.05f;

	// Token: 0x04001124 RID: 4388
	public const int STORAGE_WAX_LAUNCHECOUNT_CAPACITY = 200;

	// Token: 0x04001125 RID: 4389
	private const float JOULES_PER_LAUNCH = 10000f;

	// Token: 0x04001126 RID: 4390
	private const float LAUNCHES_FROM_FULL_CHARGE = 4f;
}
