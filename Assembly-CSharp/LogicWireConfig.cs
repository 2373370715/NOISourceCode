using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class LogicWireConfig : BaseLogicWireConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicWire";
		string anim = "logic_wires_kanim";
		float construction_time = 3f;
		float[] tier_TINY = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier_TINY, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	public const string ID = "LogicWire";
}
