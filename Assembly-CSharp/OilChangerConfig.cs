using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004DB RID: 1243
public class OilChangerConfig : IBuildingConfig
{
	// Token: 0x06001562 RID: 5474 RVA: 0x000AA12F File Offset: 0x000A832F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x0019EA64 File Offset: 0x0019CC64
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OilChanger";
		int width = 3;
		int height = 3;
		string anim = "oilchange_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.Overheatable = false;
		buildingDef.ExhaustKilowattsWhenActive = 0.25f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.PowerInputOffset = new CellOffset(1, 0);
		buildingDef.UtilityInputOffset = new CellOffset(1, 2);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AddSearchTerms(SEARCH_TERMS.BIONIC);
		return buildingDef;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x0019EB28 File Offset: 0x0019CD28
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.BionicUpkeepType, false);
		Storage storage = go.AddComponent<Storage>();
		storage.capacityKg = this.OIL_CAPACITY;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		OilChangerWorkableUse oilChangerWorkableUse = go.AddOrGet<OilChangerWorkableUse>();
		oilChangerWorkableUse.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oilchange_kanim")
		};
		oilChangerWorkableUse.resetProgressOnStop = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = this.OIL_CAPACITY;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		go.AddOrGetDef<OilChanger.Def>();
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000EC9 RID: 3785
	public const string ID = "OilChanger";

	// Token: 0x04000ECA RID: 3786
	public float OIL_CAPACITY = 400f;
}
