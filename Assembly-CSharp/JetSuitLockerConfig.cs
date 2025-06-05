using System;
using TUNING;
using UnityEngine;

// Token: 0x020003A4 RID: 932
public class JetSuitLockerConfig : IBuildingConfig
{
	// Token: 0x06000F0F RID: 3855 RVA: 0x001855F4 File Offset: 0x001837F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "JetSuitLocker";
		int width = 2;
		int height = 4;
		string anim = "changingarea_jetsuit_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = refined_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.PreventIdleTraversalPastBuilding = true;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SuitIDs, "JetSuitLocker");
		return buildingDef;
	}

	// Token: 0x06000F10 RID: 3856 RVA: 0x000B0E36 File Offset: 0x000AF036
	private void AttachPort(GameObject go)
	{
		go.AddComponent<ConduitSecondaryInput>().portInfo = this.secondaryInputPort;
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x00185684 File Offset: 0x00183884
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<SuitLocker>().OutfitTags = new Tag[]
		{
			GameTags.JetSuit
		};
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.forceAlwaysSatisfied = true;
		conduitConsumer.capacityKG = 200f;
		go.AddComponent<JetSuitLocker>().portInfo = this.secondaryInputPort;
		go.AddOrGet<AnimTileable>().tags = new Tag[]
		{
			new Tag("JetSuitLocker"),
			new Tag("JetSuitMarker")
		};
		go.AddOrGet<Storage>().capacityKg = 500f;
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000B0E49 File Offset: 0x000AF049
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPort(go);
	}

	// Token: 0x06000F13 RID: 3859 RVA: 0x000B0E5A File Offset: 0x000AF05A
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPort(go);
	}

	// Token: 0x06000F14 RID: 3860 RVA: 0x000B0E6A File Offset: 0x000AF06A
	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x04000B12 RID: 2834
	public const string ID = "JetSuitLocker";

	// Token: 0x04000B13 RID: 2835
	public const float O2_CAPACITY = 200f;

	// Token: 0x04000B14 RID: 2836
	public const float SUIT_CAPACITY = 200f;

	// Token: 0x04000B15 RID: 2837
	private ConduitPortInfo secondaryInputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 1));
}
