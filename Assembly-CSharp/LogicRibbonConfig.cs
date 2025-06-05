using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class LogicRibbonConfig : BaseLogicWireConfig
{
	// Token: 0x0600109A RID: 4250 RVA: 0x0018B1E0 File Offset: 0x001893E0
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

	// Token: 0x0600109B RID: 4251 RVA: 0x000B1B51 File Offset: 0x000AFD51
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(LogicWire.BitDepth.FourBit, go);
	}

	// Token: 0x04000B9D RID: 2973
	public const string ID = "LogicRibbon";
}
