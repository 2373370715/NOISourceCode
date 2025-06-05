using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class SquirrelHugConfig : IEntityConfig
{
	// Token: 0x060008FD RID: 2301 RVA: 0x0016DA1C File Offset: 0x0016BC1C
	public static GameObject CreateSquirrelHug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelHugBaseTrait", is_baby, "hug_", true);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SquirrelTuning.PEN_SIZE_PER_CREATURE_HUG);
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER3);
		Trait trait = Db.Get().CreateTrait("SquirrelHugBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelHugConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelHugConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		gameObject = BaseSquirrelConfig.SetupDiet(gameObject, diet_infos, SquirrelHugConfig.MIN_POOP_SIZE_KG);
		if (!is_baby)
		{
			gameObject.AddOrGetDef<HugMonitor.Def>();
		}
		return gameObject;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0016DB70 File Offset: 0x0016BD70
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelHugConfig.CreateSquirrelHug("SquirrelHug", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "squirrel_kanim", false), this as IHasDlcRestrictions, "SquirrelHugEgg", STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.EGG_NAME, STRINGS.CREATURES.SPECIES.SQUIRREL.VARIANT_HUG.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelHugBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_HUG, SquirrelHugConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006E8 RID: 1768
	public const string ID = "SquirrelHug";

	// Token: 0x040006E9 RID: 1769
	public const string BASE_TRAIT_ID = "SquirrelHugBaseTrait";

	// Token: 0x040006EA RID: 1770
	public const string EGG_ID = "SquirrelHugEgg";

	// Token: 0x040006EB RID: 1771
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x040006EC RID: 1772
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x040006ED RID: 1773
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x040006EE RID: 1774
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.5f;

	// Token: 0x040006EF RID: 1775
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelHugConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040006F0 RID: 1776
	private static float KG_POOP_PER_DAY_OF_PLANT = 25f;

	// Token: 0x040006F1 RID: 1777
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x040006F2 RID: 1778
	public static int EGG_SORT_ORDER = 0;
}
