using System;
using TUNING;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class RoleStationConfig : IBuildingConfig
{
	// Token: 0x06001855 RID: 6229 RVA: 0x001AAA48 File Offset: 0x001A8C48
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RoleStation";
		int width = 2;
		int height = 2;
		string anim = "job_station_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000B4BE0 File Offset: 0x000B2DE0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06001857 RID: 6231 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400100E RID: 4110
	public const string ID = "RoleStation";
}
