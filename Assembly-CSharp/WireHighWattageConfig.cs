using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class WireHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001B27 RID: 6951 RVA: 0x001B6748 File Offset: 0x001B4948
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighWattageWire";
		string anim = "utilities_electric_insulated_kanim";
		float construction_time = 3f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, TUNING.BUILDINGS.DECOR.PENALTY.TIER5, none);
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000B6287 File Offset: 0x000B4487
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000B6291 File Offset: 0x000B4491
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	// Token: 0x0400116A RID: 4458
	public const string ID = "HighWattageWire";
}
