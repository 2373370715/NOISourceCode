using System;
using TUNING;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class SolidVentConfig : IBuildingConfig
{
	// Token: 0x06001946 RID: 6470 RVA: 0x001AE200 File Offset: 0x001AC400
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolidVent";
		int width = 1;
		int height = 1;
		string anim = "conveyer_dropper_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.InputConduitType = ConduitType.Solid;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.SolidConveyor.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidVent");
		return buildingDef;
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000AAF59 File Offset: 0x000A9159
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000B4DF7 File Offset: 0x000B2FF7
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		go.GetComponent<Constructable>().requiredSkillPerk = Db.Get().SkillPerks.ConveyorBuild.Id;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000B50DB File Offset: 0x000B32DB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<SimpleVent>();
		go.AddOrGet<SolidConduitConsumer>();
		go.AddOrGet<SolidConduitDropper>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
		storage.capacityKg = 100f;
		storage.showInUI = true;
	}

	// Token: 0x04001061 RID: 4193
	public const string ID = "SolidVent";

	// Token: 0x04001062 RID: 4194
	private const ConduitType CONDUIT_TYPE = ConduitType.Solid;
}
