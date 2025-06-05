using System;
using TUNING;
using UnityEngine;

// Token: 0x020005A3 RID: 1443
public class SolarPanelModuleConfig : IBuildingConfig
{
	// Token: 0x060018F2 RID: 6386 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x001AD154 File Offset: 0x001AB354
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolarPanelModule";
		int width = 3;
		int height = 1;
		string anim = "rocket_solar_panel_module_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, glasses, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "grounded";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.GeneratorWattageRating = 60f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.ForegroundLayer = Grid.SceneLayer.Front;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.PowerInputOffset = SolarPanelModuleConfig.PLUG_OFFSET;
		buildingDef.PowerOutputOffset = SolarPanelModuleConfig.PLUG_OFFSET;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x001AD244 File Offset: 0x001AB444
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddComponent<RequireInputs>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, null)
		};
		go.AddComponent<PartialLightBlocking>();
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000B4D26 File Offset: 0x000B2F26
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<ModuleSolarPanel>().showConnectedConsumerStatusItems = false;
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.INSIGNIFICANT, 0f, 0f);
		go.GetComponent<RocketModule>().operationalLandedRequired = false;
	}

	// Token: 0x04001045 RID: 4165
	public const string ID = "SolarPanelModule";

	// Token: 0x04001046 RID: 4166
	private static readonly CellOffset PLUG_OFFSET = new CellOffset(-1, 0);

	// Token: 0x04001047 RID: 4167
	private const float EFFICIENCY_RATIO = 0.75f;

	// Token: 0x04001048 RID: 4168
	public const float MAX_WATTS = 60f;
}
