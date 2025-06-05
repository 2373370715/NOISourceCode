using System;
using TUNING;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class SolidConduitOutboxConfig : IBuildingConfig
{
	// Token: 0x0600191C RID: 6428 RVA: 0x001ADA68 File Offset: 0x001ABC68
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidConduitOutbox";
		int width = 1;
		int height = 2;
		string anim = "conveyorout_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.PermittedRotations = PermittedRotations.R360;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidConduitOutbox");
		return buildingDef;
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x000B4EDE File Offset: 0x000B30DE
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		go.AddOrGet<SolidConduitOutbox>();
		go.AddOrGet<SolidConduitConsumer>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.allowItemRemoval = true;
		go.AddOrGet<SimpleVent>();
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x000B4DF7 File Offset: 0x000B2FF7
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x000B4F1A File Offset: 0x000B311A
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		go.AddOrGet<Automatable>();
	}

	// Token: 0x04001054 RID: 4180
	public const string ID = "SolidConduitOutbox";
}
