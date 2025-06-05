using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000602 RID: 1538
public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
	// Token: 0x06001B2F RID: 6959 RVA: 0x000B62B9 File Offset: 0x000B44B9
	protected override string GetID()
	{
		return "WireRefinedBridgeHighWattage";
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x001B6820 File Offset: 0x001B4A20
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("heavywatttile_conductive_kanim")
		};
		buildingDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		return buildingDef;
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x000B62C0 File Offset: 0x000B44C0
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000B62D0 File Offset: 0x000B44D0
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	// Token: 0x0400116C RID: 4460
	public new const string ID = "WireRefinedBridgeHighWattage";
}
