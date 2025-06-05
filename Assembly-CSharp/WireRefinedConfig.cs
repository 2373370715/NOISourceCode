using System;
using TUNING;
using UnityEngine;

// Token: 0x02000603 RID: 1539
public class WireRefinedConfig : BaseWireConfig
{
	// Token: 0x06001B34 RID: 6964 RVA: 0x001B68A8 File Offset: 0x001B4AA8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefined";
		string anim = "utilities_electric_conduct_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.NONE, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		return buildingDef;
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000B6300 File Offset: 0x000B4500
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max2000, go);
	}

	// Token: 0x0400116D RID: 4461
	public const string ID = "WireRefined";
}
