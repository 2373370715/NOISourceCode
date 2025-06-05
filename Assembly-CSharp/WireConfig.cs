using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class WireConfig : BaseWireConfig
{
	// Token: 0x06001B24 RID: 6948 RVA: 0x001B66F0 File Offset: 0x001B48F0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Wire";
		string anim = "utilities_electric_kanim";
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x000B6275 File Offset: 0x000B4475
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	// Token: 0x04001169 RID: 4457
	public const string ID = "Wire";
}
