using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class PowerControlStationConfig : IBuildingConfig
{
	// Token: 0x06001625 RID: 5669 RVA: 0x001A20C4 File Offset: 0x001A02C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PowerControlStation";
		int width = 2;
		int height = 4;
		string anim = "electricianworkdesk_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
		return buildingDef;
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x000B422D File Offset: 0x000B242D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x001A2154 File Offset: 0x001A0354
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 50f;
		storage.showInUI = true;
		storage.storageFilters = new List<Tag>
		{
			PowerControlStationConfig.MATERIAL_FOR_TINKER
		};
		TinkerStation tinkerstation = go.AddOrGet<TinkerStation>();
		tinkerstation.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_electricianworkdesk_kanim")
		};
		tinkerstation.inputMaterial = PowerControlStationConfig.MATERIAL_FOR_TINKER;
		tinkerstation.massPerTinker = 5f;
		tinkerstation.outputPrefab = PowerControlStationConfig.TINKER_TOOLS;
		tinkerstation.outputTemperature = 308.15f;
		tinkerstation.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerstation.choreType = Db.Get().ChoreTypes.PowerFabricate.IdHash;
		tinkerstation.useFilteredStorage = true;
		tinkerstation.fetchChoreType = Db.Get().ChoreTypes.PowerFetch.IdHash;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		Prioritizable.AddRef(go);
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			TinkerStation component = game_object.GetComponent<TinkerStation>();
			component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
			component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			component.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			tinkerstation.toolProductionTime = 160f;
		};
	}

	// Token: 0x04000F36 RID: 3894
	public const string ID = "PowerControlStation";

	// Token: 0x04000F37 RID: 3895
	public static Tag MATERIAL_FOR_TINKER = GameTags.RefinedMetal;

	// Token: 0x04000F38 RID: 3896
	public static Tag TINKER_TOOLS = PowerStationToolsConfig.tag;

	// Token: 0x04000F39 RID: 3897
	public const float MASS_PER_TINKER = 5f;

	// Token: 0x04000F3A RID: 3898
	public static string ROLE_PERK = "CanPowerTinker";

	// Token: 0x04000F3B RID: 3899
	public const float OUTPUT_TEMPERATURE = 308.15f;
}
