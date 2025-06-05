using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class WireRefinedBridgeConfig : WireBridgeConfig
{
	// Token: 0x06001B2B RID: 6955 RVA: 0x000B629A File Offset: 0x000B449A
	protected override string GetID()
	{
		return "WireRefinedBridge";
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x001B67A8 File Offset: 0x001B49A8
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = base.CreateBuildingDef();
		buildingDef.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("utilityelectricbridgeconductive_kanim")
		};
		buildingDef.Mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		buildingDef.AddSearchTerms(SEARCH_TERMS.WIRE);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
		return buildingDef;
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000B62A1 File Offset: 0x000B44A1
	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max2000;
		return wireUtilityNetworkLink;
	}

	// Token: 0x0400116B RID: 4459
	public new const string ID = "WireRefinedBridge";
}
