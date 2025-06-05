using System;
using TUNING;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public class SunLampConfig : IBuildingConfig
{
	// Token: 0x06001A3C RID: 6716 RVA: 0x001B29AC File Offset: 0x001B0BAC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SunLamp";
		int width = 2;
		int height = 4;
		string anim = "sun_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			200f,
			50f
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Glass"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x001B2A54 File Offset: 0x001B0C54
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = LIGHT2D.SUNLAMP_LUX;
		lightShapePreview.radius = 16f;
		lightShapePreview.shape = global::LightShape.Cone;
		lightShapePreview.offset = new CellOffset((int)LIGHT2D.SUNLAMP_OFFSET.x, (int)LIGHT2D.SUNLAMP_OFFSET.y);
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x000AA614 File Offset: 0x000A8814
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x001B2AA4 File Offset: 0x001B0CA4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<EnergyConsumer>();
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Lux = LIGHT2D.SUNLAMP_LUX;
		light2D.overlayColour = LIGHT2D.SUNLAMP_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.SUNLAMP_COLOR;
		light2D.Range = 16f;
		light2D.Angle = 5.2f;
		light2D.Direction = LIGHT2D.SUNLAMP_DIRECTION;
		light2D.Offset = LIGHT2D.SUNLAMP_OFFSET;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x040010FA RID: 4346
	public const string ID = "SunLamp";
}
