using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class WireHighWattageConfig : BaseWireConfig
{
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

	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
	}

	public const string ID = "HighWattageWire";
}
