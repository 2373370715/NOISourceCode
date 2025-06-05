using System;
using TUNING;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class DevLightGeneratorConfig : IBuildingConfig
{
	// Token: 0x06000287 RID: 647 RVA: 0x001513B8 File Offset: 0x0014F5B8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevLightGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_on", NOISE_POLLUTION.NOISY.TIER3);
		SoundEventVolumeCache.instance.AddVolume("dev_lightgenerator_kanim", "PowerSwitch_off", NOISE_POLLUTION.NOISY.TIER3);
		return buildingDef;
	}

	// Token: 0x06000288 RID: 648 RVA: 0x000AAE02 File Offset: 0x000A9002
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Circle;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x000AAE26 File Offset: 0x000A9026
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		go.AddTag(RoomConstraints.ConstraintTags.LightSource);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00151468 File Offset: 0x0014F668
	public override void DoPostConfigureComplete(GameObject go)
	{
		DevLightGenerator devLightGenerator = go.AddOrGet<DevLightGenerator>();
		devLightGenerator.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		devLightGenerator.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		devLightGenerator.Range = 8f;
		devLightGenerator.Angle = 2.6f;
		devLightGenerator.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		devLightGenerator.Offset = new Vector2(0f, 0.5f);
		devLightGenerator.shape = global::LightShape.Circle;
		devLightGenerator.drawOverlay = true;
		devLightGenerator.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x040001A4 RID: 420
	public const string ID = "DevLightGenerator";
}
