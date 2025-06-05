using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020004E9 RID: 1257
public class OreScrubberConfig : IBuildingConfig
{
	// Token: 0x0600159C RID: 5532 RVA: 0x0019F810 File Offset: 0x0019DA10
	public override BuildingDef CreateBuildingDef()
	{
		string id = "OreScrubber";
		int width = 3;
		int height = 3;
		string anim = "orescrubber_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		string[] array = new string[]
		{
			"Metal"
		};
		float[] construction_mass = new float[]
		{
			TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = array;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
		buildingDef.UtilityInputOffset = new CellOffset(1, 1);
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.AddSearchTerms(SEARCH_TERMS.FILTER);
		return buildingDef;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x0019F898 File Offset: 0x0019DA98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		OreScrubber oreScrubber = go.AddOrGet<OreScrubber>();
		oreScrubber.massConsumedPerUse = 0.07f;
		oreScrubber.consumedElement = SimHashes.ChlorineGas;
		oreScrubber.diseaseRemovalCount = OreScrubberConfig.DISEASE_REMOVAL_COUNT;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.ChlorineGas).tag;
		go.AddOrGet<DirectionControl>();
		OreScrubber.Work work = go.AddOrGet<OreScrubber.Work>();
		work.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_ore_scrubber_kanim")
		};
		work.workTime = 10.200001f;
		work.trackUses = true;
		work.workLayer = Grid.SceneLayer.BuildingUse;
		go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x000B0AEB File Offset: 0x000AECEB
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<RequireInputs>().requireConduitHasMass = false;
	}

	// Token: 0x04000EE3 RID: 3811
	public const string ID = "OreScrubber";

	// Token: 0x04000EE4 RID: 3812
	private const float MASS_PER_USE = 0.07f;

	// Token: 0x04000EE5 RID: 3813
	private static readonly int DISEASE_REMOVAL_COUNT = WashBasinConfig.DISEASE_REMOVAL_COUNT * 4;

	// Token: 0x04000EE6 RID: 3814
	private const SimHashes CONSUMED_ELEMENT = SimHashes.ChlorineGas;
}
