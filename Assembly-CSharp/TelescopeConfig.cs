using System;
using TUNING;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class TelescopeConfig : IBuildingConfig
{
	// Token: 0x06001A59 RID: 6745 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetForbiddenDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x001B38D8 File Offset: 0x001B1AD8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telescope";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		return buildingDef;
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x001B3998 File Offset: 0x001B1B98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Telescope telescope = go.AddOrGet<Telescope>();
		telescope.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		telescope.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		telescope.workLayer = Grid.SceneLayer.BuildingFront;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x000B593B File Offset: 0x000B3B3B
	public override void DoPostConfigureComplete(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06001A5D RID: 6749 RVA: 0x000B5943 File Offset: 0x000B3B43
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06001A5E RID: 6750 RVA: 0x000B593B File Offset: 0x000B3B3B
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x000AA718 File Offset: 0x000A8918
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400110B RID: 4363
	public const string ID = "Telescope";

	// Token: 0x0400110C RID: 4364
	public const float POINTS_PER_DAY = 2f;

	// Token: 0x0400110D RID: 4365
	public const float MASS_PER_POINT = 2f;

	// Token: 0x0400110E RID: 4366
	public const float CAPACITY = 30f;

	// Token: 0x0400110F RID: 4367
	public const int SCAN_RADIUS = 4;

	// Token: 0x04001110 RID: 4368
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x04001111 RID: 4369
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

	// Token: 0x04001112 RID: 4370
	public static readonly Tag INPUT_MATERIAL = GameTags.Glass;
}
