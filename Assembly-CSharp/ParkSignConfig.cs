using System;
using TUNING;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class ParkSignConfig : IBuildingConfig
{
	// Token: 0x060015E6 RID: 5606 RVA: 0x001A10D4 File Offset: 0x0019F2D4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("ParkSign", 1, 2, "parksign_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER1, MATERIALS.ANY_BUILDABLE, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.NONE, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		return buildingDef;
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x000B4169 File Offset: 0x000B2369
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.Park, false);
		go.AddOrGet<ParkSign>();
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000F0F RID: 3855
	public const string ID = "ParkSign";
}
