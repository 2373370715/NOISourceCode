using System;
using TUNING;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class RadiationLightConfig : IBuildingConfig
{
	// Token: 0x0600173F RID: 5951 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x001A4FC8 File Offset: 0x001A31C8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RadiationLight";
		int width = 1;
		int height = 1;
		string anim = "radiation_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 60f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		return buildingDef;
	}

	// Token: 0x06001741 RID: 5953 RVA: 0x001A5060 File Offset: 0x001A3260
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.showInUI = true;
		storage.capacityKg = 50f;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = this.FUEL_ELEMENT;
		manualDeliveryKG.capacity = 50f;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		RadiationEmitter radiationEmitter = go.AddComponent<RadiationEmitter>();
		radiationEmitter.emitAngle = 90f;
		radiationEmitter.emitDirection = 0f;
		radiationEmitter.emissionOffset = Vector3.right;
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.emitRadiusX = 16;
		radiationEmitter.emitRadiusY = 4;
		radiationEmitter.emitRads = 240f;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(this.FUEL_ELEMENT, 0.016666668f, true)
		};
		elementConverter.outputElements = new ElementConverter.OutputElement[]
		{
			new ElementConverter.OutputElement(0.008333334f, this.WASTE_ELEMENT, 0f, false, true, 0f, 0.5f, 0.5f, byte.MaxValue, 0, true)
		};
		ElementDropper elementDropper = go.AddOrGet<ElementDropper>();
		elementDropper.emitTag = this.WASTE_ELEMENT.CreateTag();
		elementDropper.emitMass = 5f;
		RadiationLight radiationLight = go.AddComponent<RadiationLight>();
		radiationLight.elementToConsume = this.FUEL_ELEMENT;
		radiationLight.consumptionRate = 0.016666668f;
	}

	// Token: 0x06001742 RID: 5954 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x06001743 RID: 5955 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x04000F4D RID: 3917
	public const string ID = "RadiationLight";

	// Token: 0x04000F4E RID: 3918
	private Tag FUEL_ELEMENT = SimHashes.UraniumOre.CreateTag();

	// Token: 0x04000F4F RID: 3919
	private SimHashes WASTE_ELEMENT = SimHashes.DepletedUranium;

	// Token: 0x04000F50 RID: 3920
	private const float FUEL_PER_CYCLE = 10f;

	// Token: 0x04000F51 RID: 3921
	private const float CYCLES_PER_REFILL = 5f;

	// Token: 0x04000F52 RID: 3922
	private const float FUEL_TO_WASTE_RATIO = 0.5f;

	// Token: 0x04000F53 RID: 3923
	private const float FUEL_STORAGE_AMOUNT = 50f;

	// Token: 0x04000F54 RID: 3924
	private const float FUEL_CONSUMPTION_RATE = 0.016666668f;

	// Token: 0x04000F55 RID: 3925
	private const short RAD_LIGHT_SIZE_X = 16;

	// Token: 0x04000F56 RID: 3926
	private const short RAD_LIGHT_SIZE_Y = 4;
}
