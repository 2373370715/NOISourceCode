using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000236 RID: 566
[EntityConfigOrder(1)]
public class HatchConfig : IEntityConfig
{
	// Token: 0x060007CB RID: 1995 RVA: 0x00169AA8 File Offset: 0x00167CA8
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchBaseTrait", is_baby, null), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.BasicRockDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.FoodDiet(SimHashes.Carbon.CreateTag(), HatchConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f));
		GameObject gameObject = BaseHatchConfig.SetupDiet(prefab, list, HatchConfig.CALORIES_PER_KG_OF_ORE, HatchConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x00169C0C File Offset: 0x00167E0C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchConfig.CreateHatch("Hatch", STRINGS.CREATURES.SPECIES.HATCH.NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchEgg", STRINGS.CREATURES.SPECIES.HATCH.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_BASE, HatchConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005E6 RID: 1510
	public const string ID = "Hatch";

	// Token: 0x040005E7 RID: 1511
	public const string BASE_TRAIT_ID = "HatchBaseTrait";

	// Token: 0x040005E8 RID: 1512
	public const string EGG_ID = "HatchEgg";

	// Token: 0x040005E9 RID: 1513
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x040005EA RID: 1514
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x040005EB RID: 1515
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040005EC RID: 1516
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040005ED RID: 1517
	public static int EGG_SORT_ORDER = 0;
}
