using System;
using TUNING;
using UnityEngine;

// Token: 0x020005A8 RID: 1448
public class SolidConduitBridgeConfig : IBuildingConfig
{
	// Token: 0x0600190E RID: 6414 RVA: 0x001AD73C File Offset: 0x001AB93C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduitBridge";
		int width = 3;
		int height = 1;
		string anim = "utilities_conveyorbridge_kanim";
		int hitpoints = 10;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.ObjectLayer = ObjectLayer.SolidConduitConnection;
		buildingDef.SceneLayer = Grid.SceneLayer.SolidConduitBridges;
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.OutputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Entombable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitBridge");
		return buildingDef;
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000B1845 File Offset: 0x000AFA45
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000B4DF7 File Offset: 0x000B2FF7
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000B4E1F File Offset: 0x000B301F
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SolidConduitBridge>();
	}

	// Token: 0x04001050 RID: 4176
	public const string ID = "SolidConduitBridge";

	// Token: 0x04001051 RID: 4177
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
