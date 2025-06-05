using System;
using TUNING;
using UnityEngine;

// Token: 0x02000391 RID: 913
public class IceMachineConfig : IBuildingConfig
{
	// Token: 0x06000EAB RID: 3755 RVA: 0x001847E0 File Offset: 0x001829E0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "IceMachine";
		int width = 2;
		int height = 3;
		string anim = "freezerator_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = this.energyConsumption;
		buildingDef.ExhaustKilowattsWhenActive = 4f;
		buildingDef.SelfHeatKilowattsWhenActive = 12f;
		buildingDef.ViewMode = OverlayModes.Temperature.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00184868 File Offset: 0x00182A68
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage.showInUI = true;
		storage.capacityKg = 60f;
		Storage storage2 = go.AddComponent<Storage>();
		storage2.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		storage2.showInUI = true;
		storage2.capacityKg = 300f;
		storage2.allowItemRemoval = true;
		storage2.ignoreSourcePriority = true;
		storage2.allowUIItemRemoval = true;
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		IceMachine iceMachine = go.AddOrGet<IceMachine>();
		iceMachine.SetStorages(storage, storage2);
		iceMachine.targetTemperature = 253.15f;
		iceMachine.heatRemovalRate = 80f;
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = GameTags.Water;
		manualDeliveryKG.capacity = 60f;
		manualDeliveryKG.refillMass = 12f;
		manualDeliveryKG.MinimumMass = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000AD6 RID: 2774
	public const string ID = "IceMachine";

	// Token: 0x04000AD7 RID: 2775
	private const float WATER_STORAGE = 60f;

	// Token: 0x04000AD8 RID: 2776
	private const float ICE_STORAGE = 300f;

	// Token: 0x04000AD9 RID: 2777
	private const float WATER_INPUT_RATE = 0.5f;

	// Token: 0x04000ADA RID: 2778
	private const float ICE_OUTPUT_RATE = 0.5f;

	// Token: 0x04000ADB RID: 2779
	private const float ICE_PER_LOAD = 30f;

	// Token: 0x04000ADC RID: 2780
	private const float TARGET_ICE_TEMP = 253.15f;

	// Token: 0x04000ADD RID: 2781
	private const float KDTU_TRANSFER_RATE = 80f;

	// Token: 0x04000ADE RID: 2782
	private const float THERMAL_CONSERVATION = 0.2f;

	// Token: 0x04000ADF RID: 2783
	private float energyConsumption = 240f;

	// Token: 0x04000AE0 RID: 2784
	public static Tag[] ELEMENT_OPTIONS = new Tag[]
	{
		SimHashes.Ice.CreateTag(),
		SimHashes.Snow.CreateTag()
	};
}
