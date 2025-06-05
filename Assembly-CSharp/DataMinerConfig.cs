using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class DataMinerConfig : IBuildingConfig
{
	// Token: 0x0600025E RID: 606 RVA: 0x000AA12F File Offset: 0x000A832F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x0600025F RID: 607 RVA: 0x001505CC File Offset: 0x0014E7CC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DataMiner";
		int width = 3;
		int height = 2;
		string anim = "data_miner_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.EnergyConsumptionWhenActive = 1000f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 3f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00150670 File Offset: 0x0014E870
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGet<CopyBuildingSettings>();
		DataMiner dataMiner = go.AddOrGet<DataMiner>();
		dataMiner.duplicantOperated = false;
		dataMiner.showProgressBar = true;
		dataMiner.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
		BuildingTemplates.CreateComplexFabricatorStorage(go, dataMiner);
		ComplexRecipe.RecipeElement[] array = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(this.INPUT_MATERIAL_TAG, 5f)
		};
		ComplexRecipe.RecipeElement[] array2 = new ComplexRecipe.RecipeElement[]
		{
			new ComplexRecipe.RecipeElement(this.OUTPUT_MATERIAL_TAG, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature, false)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("DataMiner", this.OUTPUT_MATERIAL_TAG);
		string text = ComplexRecipeManager.MakeRecipeID("DataMiner", array, array2);
		ComplexRecipe complexRecipe = new ComplexRecipe(text, array, array2);
		complexRecipe.time = 200f;
		complexRecipe.description = string.Format(STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, ElementLoader.FindElementByHash(this.INPUT_MATERIAL).name, this.OUTPUT_MATERIAL_NAME);
		complexRecipe.fabricators = new List<Tag>
		{
			TagManager.Create("DataMiner")
		};
		complexRecipe.nameDisplay = ComplexRecipe.RecipeNameDisplay.IngredientToResult;
		complexRecipe.sortOrder = 300;
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, text);
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000181 RID: 385
	public const string ID = "DataMiner";

	// Token: 0x04000182 RID: 386
	public const float POWER_USAGE_W = 1000f;

	// Token: 0x04000183 RID: 387
	public const float BASE_UNITS_PRODUCED_PER_CYCLE = 3f;

	// Token: 0x04000184 RID: 388
	public const float BASE_DTU_PRODUCTION = 3f;

	// Token: 0x04000185 RID: 389
	public const float STORAGE_CAPACITY_KG = 1000f;

	// Token: 0x04000186 RID: 390
	public const float MASS_CONSUMED_PER_BANK_KG = 5f;

	// Token: 0x04000187 RID: 391
	public const float BASE_DURATION_SECONDS = 200f;

	// Token: 0x04000188 RID: 392
	public static MathUtil.MinMax PRODUCTION_RATE_SCALE = new MathUtil.MinMax(0.6f, 5.3333335f);

	// Token: 0x04000189 RID: 393
	public static MathUtil.MinMax TEMPERATURE_SCALING_RANGE = new MathUtil.MinMax(10f, 325f);

	// Token: 0x0400018A RID: 394
	public SimHashes INPUT_MATERIAL = SimHashes.Polypropylene;

	// Token: 0x0400018B RID: 395
	public Tag INPUT_MATERIAL_TAG = SimHashes.Polypropylene.CreateTag();

	// Token: 0x0400018C RID: 396
	public Tag OUTPUT_MATERIAL_TAG = DatabankHelper.TAG;

	// Token: 0x0400018D RID: 397
	public string OUTPUT_MATERIAL_NAME = DatabankHelper.NAME;

	// Token: 0x0400018E RID: 398
	public const float BASE_PRODUCTION_PROGRESS_PER_TICK = 0.001f;
}
