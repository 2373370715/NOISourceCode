using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000270 RID: 624
public class SquirrelConfig : IEntityConfig
{
	// Token: 0x060008F3 RID: 2291 RVA: 0x0016D864 File Offset: 0x0016BA64
	public static GameObject CreateSquirrel(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, null, false), SquirrelTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet.Info[] diet_infos = BaseSquirrelConfig.BasicDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f);
		GameObject gameObject = BaseSquirrelConfig.SetupDiet(prefab, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0016D99C File Offset: 0x0016BB9C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SquirrelConfig.CreateSquirrel("Squirrel", CREATURES.SPECIES.SQUIRREL.NAME, CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false), this as IHasDlcRestrictions, "SquirrelEgg", CREATURES.SPECIES.SQUIRREL.EGG_NAME, CREATURES.SPECIES.SQUIRREL.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelBaby", 60.000004f, 20f, SquirrelTuning.EGG_CHANCES_BASE, SquirrelConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006DC RID: 1756
	public const string ID = "Squirrel";

	// Token: 0x040006DD RID: 1757
	public const string BASE_TRAIT_ID = "SquirrelBaseTrait";

	// Token: 0x040006DE RID: 1758
	public const string EGG_ID = "SquirrelEgg";

	// Token: 0x040006DF RID: 1759
	public const float OXYGEN_RATE = 0.023437504f;

	// Token: 0x040006E0 RID: 1760
	public const float BABY_OXYGEN_RATE = 0.011718752f;

	// Token: 0x040006E1 RID: 1761
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x040006E2 RID: 1762
	public static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;

	// Token: 0x040006E3 RID: 1763
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040006E4 RID: 1764
	private static float KG_POOP_PER_DAY_OF_PLANT = 50f;

	// Token: 0x040006E5 RID: 1765
	private static float MIN_POOP_SIZE_KG = 40f;

	// Token: 0x040006E6 RID: 1766
	public static int EGG_SORT_ORDER = 0;
}
