using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class WireConfig : BaseWireConfig
{
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

	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);
	}

	public const string ID = "Wire";
}
