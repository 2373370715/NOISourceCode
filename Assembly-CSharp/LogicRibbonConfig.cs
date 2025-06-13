using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class LogicRibbonConfig : BaseLogicWireConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LogicRibbon";
		string anim = "logic_ribbon_kanim";
		float construction_time = 10f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, none);
		buildingDef.AddSearchTerms(SEARCH_TERMS.AUTOMATION);
		return buildingDef;
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	public const string ID = "LogicRibbon";
}
