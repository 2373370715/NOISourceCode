using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class LogicWireConfig : BaseLogicWireConfig
{
	// Token: 0x060010C2 RID: 4290 RVA: 0x0018BBB4 File Offset: 0x00189DB4
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

	// Token: 0x060010C3 RID: 4291 RVA: 0x000B1D46 File Offset: 0x000AFF46
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.OneBit, go);
	}

	// Token: 0x04000BA8 RID: 2984
	public const string ID = "LogicWire";
}
