using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000227 RID: 551
[EntityConfigOrder(1)]
public class CrabWoodConfig : IEntityConfig
{
	// Token: 0x06000779 RID: 1913 RVA: 0x0016871C File Offset: 0x0016691C
	public static GameObject CreateCrabWood(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID = "CrabWoodShell")
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabWoodBaseTrait", is_baby, CrabWoodConfig.animPrefix, deathDropID, 5), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabWoodBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.DietWithSlime(SimHashes.Sand.CreateTag(), CrabWoodConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f);
		return BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabWoodConfig.CALORIES_PER_KG_OF_ORE, CrabWoodConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00168854 File Offset: 0x00166A54
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabWoodConfig.CreateCrabWood("CrabWood", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "pincher_kanim", false, "CrabWoodShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "CrabWoodEgg", STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.VARIANT_WOOD.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabWoodBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_WOOD, CrabWoodConfig.EGG_SORT_ORDER, true, false, 1f, false);
		EggProtectionMonitor.Def def = gameObject.AddOrGetDef<EggProtectionMonitor.Def>();
		def.allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		def.animPrefix = CrabWoodConfig.animPrefix;
		MoltDropperMonitor.Def def2 = gameObject.AddOrGetDef<MoltDropperMonitor.Def>();
		def2.onGrowDropID = "CrabWoodShell";
		def2.massToDrop = 100f;
		def2.isReadyToMolt = new Func<MoltDropperMonitor.Instance, bool>(CrabTuning.IsReadyToMolt);
		return gameObject;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400058F RID: 1423
	public const string ID = "CrabWood";

	// Token: 0x04000590 RID: 1424
	public const string BASE_TRAIT_ID = "CrabWoodBaseTrait";

	// Token: 0x04000591 RID: 1425
	public const string EGG_ID = "CrabWoodEgg";

	// Token: 0x04000592 RID: 1426
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x04000593 RID: 1427
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x04000594 RID: 1428
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabWoodConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000595 RID: 1429
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000596 RID: 1430
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x04000597 RID: 1431
	private static string animPrefix = "wood_";
}
