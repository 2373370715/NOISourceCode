using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class LaunchPadConfig : IBuildingConfig
{
	// Token: 0x06000F65 RID: 3941 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00186B64 File Offset: 0x00184D64
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LaunchPad";
		int width = 7;
		int height = 2;
		string anim = "rocket_launchpad_kanim";
		int hitpoints = 1000;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.UseStructureTemperature = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.DefaultAnimState = "idle";
		buildingDef.CanMove = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("TriggerLaunch", new CellOffset(-1, 0), STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LAUNCH, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LAUNCH_ACTIVE, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LAUNCH_INACTIVE, false, false)
		};
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("LaunchReady", new CellOffset(1, 0), STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_READY, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_READY_ACTIVE, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_READY_INACTIVE, false, false),
			LogicPorts.Port.OutputPort("LandedRocket", new CellOffset(0, 1), STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LANDED_ROCKET, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LANDED_ROCKET_ACTIVE, STRINGS.BUILDINGS.PREFABS.LAUNCHPAD.LOGIC_PORT_LANDED_ROCKET_INACTIVE, false, false)
		};
		return buildingDef;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00186CD0 File Offset: 0x00184ED0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.NotRocketInteriorBuilding, false);
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		LaunchPad launchPad = go.AddOrGet<LaunchPad>();
		launchPad.triggerPort = "TriggerLaunch";
		launchPad.statusPort = "LaunchReady";
		launchPad.landedRocketPort = "LandedRocket";
		FakeFloorAdder fakeFloorAdder = go.AddOrGet<FakeFloorAdder>();
		fakeFloorAdder.floorOffsets = new CellOffset[7];
		for (int i = 0; i < 7; i++)
		{
			fakeFloorAdder.floorOffsets[i] = new CellOffset(i - 3, 1);
		}
		go.AddOrGet<LaunchPadConditions>();
		ChainedBuilding.Def def = go.AddOrGetDef<ChainedBuilding.Def>();
		def.headBuildingTag = "LaunchPad".ToTag();
		def.linkBuildingTag = BaseModularLaunchpadPortConfig.LinkTag;
		def.objectLayer = ObjectLayer.Building;
		go.AddOrGetDef<LaunchPadMaterialDistributor.Def>();
		go.AddOrGet<UserNameable>();
		go.AddOrGet<CharacterOverlay>().shouldShowName = true;
		ModularConduitPortTiler modularConduitPortTiler = go.AddOrGet<ModularConduitPortTiler>();
		modularConduitPortTiler.manageRightCap = true;
		modularConduitPortTiler.manageLeftCap = false;
		modularConduitPortTiler.leftCapDefaultSceneLayerAdjust = 1;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000B38 RID: 2872
	public const string ID = "LaunchPad";

	// Token: 0x04000B39 RID: 2873
	public const int WIDTH = 7;

	// Token: 0x04000B3A RID: 2874
	private const string TRIGGER_LAUNCH_PORT_ID = "TriggerLaunch";

	// Token: 0x04000B3B RID: 2875
	private const string LAUNCH_READY_PORT_ID = "LaunchReady";

	// Token: 0x04000B3C RID: 2876
	private const string LANDED_ROCKET_ID = "LandedRocket";
}
