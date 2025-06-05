using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class ApothecaryConfig : IBuildingConfig
{
	// Token: 0x06000083 RID: 131 RVA: 0x001488D8 File Offset: 0x00146AD8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Apothecary";
		int width = 2;
		int height = 3;
		string anim = "apothecary_kanim";
		int hitpoints = 30;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.EnergyConsumptionWhenActive = 0f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(default(CellOffset));
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanCompound.Id;
		buildingDef.AddSearchTerms(SEARCH_TERMS.MEDICINE);
		return buildingDef;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0014899C File Offset: 0x00146B9C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Apothecary apothecary = go.AddOrGet<Apothecary>();
		BuildingTemplates.CreateComplexFabricatorStorage(go, apothecary);
		apothecary.inStorage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		go.AddOrGet<ComplexFabricatorWorkable>();
		go.AddOrGet<FabricatorIngredientStatusManager>();
		go.AddOrGet<CopyBuildingSettings>();
	}

	// Token: 0x06000085 RID: 133 RVA: 0x000AA1E0 File Offset: 0x000A83E0
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveStoppableController.Def>();
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x04000068 RID: 104
	public const string ID = "Apothecary";
}
