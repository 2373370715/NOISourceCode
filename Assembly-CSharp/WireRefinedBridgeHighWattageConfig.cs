﻿using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class WireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
{
	protected override string GetID()
	{
		return "WireRefinedBridgeHighWattage";
	}

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

	protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
	{
		WireUtilityNetworkLink wireUtilityNetworkLink = base.AddNetworkLink(go);
		wireUtilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
		return wireUtilityNetworkLink;
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.CanPowerTinker.Id;
	}

	public new const string ID = "WireRefinedBridgeHighWattage";
}
