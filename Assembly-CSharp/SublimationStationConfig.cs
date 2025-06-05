using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
public class SublimationStationConfig : IBuildingConfig
{
	// Token: 0x06001A1B RID: 6683 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x001B19FC File Offset: 0x001AFBFC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SublimationStation";
		int width = 2;
		int height = 1;
		string anim = "sublimation_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 1f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.ViewMode = OverlayModes.Oxygen.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.Breakable = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.OXYGEN);
		return buildingDef;
	}

	// Token: 0x06001A1D RID: 6685 RVA: 0x001B1AAC File Offset: 0x001AFCAC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		CellOffset cellOffset = new CellOffset(0, 0);
		Electrolyzer electrolyzer = go.AddOrGet<Electrolyzer>();
		electrolyzer.maxMass = 1.8f;
		electrolyzer.hasMeter = false;
		electrolyzer.emissionOffset = cellOffset;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 600f;
		storage.showInUI = true;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(SimHashes.ToxicSand.CreateTag(), 1f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.66f, SimHashes.ContaminatedOxygen, 303.15f, false, false, (float)cellOffset.x, (float)cellOffset.y, 1f, byte.MaxValue, 0, true)
		};
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = SimHashes.ToxicSand.CreateTag();
		manualDeliveryKG.capacity = 600f;
		manualDeliveryKG.refillMass = 240f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
	}

	// Token: 0x06001A1E RID: 6686 RVA: 0x000AA1AD File Offset: 0x000A83AD
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x040010E9 RID: 4329
	public const string ID = "SublimationStation";

	// Token: 0x040010EA RID: 4330
	private const float DIRT_CONSUME_RATE = 1f;

	// Token: 0x040010EB RID: 4331
	private const float DIRT_STORAGE = 600f;

	// Token: 0x040010EC RID: 4332
	private const float OXYGEN_GENERATION_RATE = 0.66f;

	// Token: 0x040010ED RID: 4333
	private const float OXYGEN_TEMPERATURE = 303.15f;
}
