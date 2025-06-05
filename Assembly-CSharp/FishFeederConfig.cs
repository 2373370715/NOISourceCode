using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class FishFeederConfig : IBuildingConfig
{
	// Token: 0x06000951 RID: 2385 RVA: 0x0016EB0C File Offset: 0x0016CD0C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FishFeeder";
		int width = 1;
		int height = 3;
		string anim = "fishfeeder_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Entombable = true;
		buildingDef.Floodable = true;
		buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0016EB94 File Offset: 0x0016CD94
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 200f;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.allowItemRemoval = false;
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.showCapacityStatusItem = true;
		storage.showCapacityAsMainStatus = true;
		storage.dropOffset = Vector2.up * 1f;
		storage.storageID = new Tag("FishFeederTop");
		Storage storage2 = go.AddComponent<Storage>();
		storage2.capacityKg = 200f;
		storage2.showInUI = true;
		storage2.showDescriptor = true;
		storage2.allowItemRemoval = false;
		storage2.dropOffset = Vector2.up * 3.5f;
		storage2.storageID = new Tag("FishFeederBot");
		go.AddOrGet<StorageLocker>().choreTypeID = Db.Get().ChoreTypes.RanchingFetch.Id;
		go.AddOrGet<UserNameable>();
		Effect effect = new Effect("AteFromFeeder", STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.TOOLTIP, 1200f, true, false, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.033333335f, STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, false, false, true));
		Db.Get().effects.Add(effect);
		go.AddOrGet<TreeFilterable>().filterAllStoragesOnBuilding = true;
		CreatureFeeder creatureFeeder = go.AddOrGet<CreatureFeeder>();
		creatureFeeder.effectId = effect.Id;
		creatureFeeder.feederOffset = new CellOffset(0, -2);
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x000AEAAC File Offset: 0x000ACCAC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<StorageController.Def>();
		go.AddOrGetDef<FishFeeder.Def>();
		go.AddOrGetDef<MakeBaseSolid.Def>().solidOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		SymbolOverrideControllerUtil.AddToPrefab(go);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0016ED54 File Offset: 0x0016CF54
	public override void ConfigurePost(BuildingDef def)
	{
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, Diet> keyValuePair in DietManager.CollectDiets(new Tag[]
		{
			GameTags.Creatures.Species.PacuSpecies
		}))
		{
			list.Add(keyValuePair.Key);
		}
		def.BuildingComplete.GetComponent<Storage>().storageFilters = list;
	}

	// Token: 0x04000732 RID: 1842
	public const string ID = "FishFeeder";
}
