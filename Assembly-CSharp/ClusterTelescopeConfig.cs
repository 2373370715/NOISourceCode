using System;
using TUNING;
using UnityEngine;

// Token: 0x0200004C RID: 76
public class ClusterTelescopeConfig : IBuildingConfig
{
	// Token: 0x06000157 RID: 343 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0014C6F0 File Offset: 0x0014A8F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ClusterTelescope";
		int width = 3;
		int height = 3;
		string anim = "telescope_low_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.RequiredSkillPerkID = Db.Get().SkillPerks.CanUseClusterTelescope.Id;
		return buildingDef;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0014C79C File Offset: 0x0014A99C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		ClusterTelescope.Def def = go.AddOrGetDef<ClusterTelescope.Def>();
		def.clearScanCellRadius = 4;
		def.workableOverrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_low_kanim")
		};
		def.skyVisibilityInfo = ClusterTelescopeConfig.SKY_VISIBILITY_INFO;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x000AA6B2 File Offset: 0x000A88B2
	public override void DoPostConfigureComplete(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000AA6BA File Offset: 0x000A88BA
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x000AA6B2 File Offset: 0x000A88B2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		ClusterTelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x000AA6C2 File Offset: 0x000A88C2
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 1;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 4;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040000D2 RID: 210
	public const string ID = "ClusterTelescope";

	// Token: 0x040000D3 RID: 211
	public const int SCAN_RADIUS = 4;

	// Token: 0x040000D4 RID: 212
	public const int VERTICAL_SCAN_OFFSET = 1;

	// Token: 0x040000D5 RID: 213
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 1), 4, new CellOffset(0, 1), 4, 0);
}
