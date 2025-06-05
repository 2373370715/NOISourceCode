using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000283 RID: 643
public class FishTrapConfig : IBuildingConfig
{
	// Token: 0x0600095B RID: 2395 RVA: 0x0016EE30 File Offset: 0x0016D030
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.AudioCategory = "Metal";
		buildingDef.Floodable = false;
		buildingDef.Deprecated = true;
		buildingDef.AddSearchTerms(SEARCH_TERMS.RANCHING);
		buildingDef.AddSearchTerms(SEARCH_TERMS.CRITTER);
		return buildingDef;
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0016EEB0 File Offset: 0x0016D0B0
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		storage.allowItemRemoval = true;
		storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
		storage.sendOnStoreOnSpawn = true;
		TrapTrigger trapTrigger = go.AddOrGet<TrapTrigger>();
		trapTrigger.trappableCreatures = new Tag[]
		{
			GameTags.Creatures.Swimmer
		};
		trapTrigger.trappedOffset = new Vector2(0f, 1f);
		go.AddOrGet<Trap>();
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0016EF18 File Offset: 0x0016D118
	public override void DoPostConfigureComplete(GameObject go)
	{
		Lure.Def def = go.AddOrGetDef<Lure.Def>();
		def.defaultLurePoints = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		def.radius = 32;
		def.initialLures = new Tag[]
		{
			GameTags.Creatures.FishTrapLure
		};
	}

	// Token: 0x04000734 RID: 1844
	public const string ID = "FishTrap";

	// Token: 0x04000735 RID: 1845
	private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
}
