using System;
using TUNING;
using UnityEngine;

// Token: 0x02000604 RID: 1540
public class WireRefinedHighWattageConfig : BaseWireConfig
{
	// Token: 0x06001B37 RID: 6967 RVA: 0x001B68EC File Offset: 0x001B4AEC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "WireRefinedHighWattage";
		string anim = "utilities_electric_conduct_hiwatt_kanim";
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		float insulation = 0.05f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, anim, construction_time, tier, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
		return buildingDef;
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x000B630A File Offset: 0x000B450A
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x000B6314 File Offset: 0x000B4514
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x0400116E RID: 4462
	public const string ID = "WireRefinedHighWattage";
}
