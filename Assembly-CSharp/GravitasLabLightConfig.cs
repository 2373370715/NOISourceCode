using System;
using TUNING;
using UnityEngine;

// Token: 0x02000378 RID: 888
public class GravitasLabLightConfig : IBuildingConfig
{
	// Token: 0x06000E29 RID: 3625 RVA: 0x00181F18 File Offset: 0x00180118
	public override BuildingDef CreateBuildingDef()
	{
		string id = "GravitasLabLight";
		int width = 1;
		int height = 1;
		string anim = "gravitas_lab_light_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ShowInBuildMenu = false;
		buildingDef.Entombable = false;
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x000B0985 File Offset: 0x000AEB85
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.Gravitas);
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000A7C RID: 2684
	public const string ID = "GravitasLabLight";
}
