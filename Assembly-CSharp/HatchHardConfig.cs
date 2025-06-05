using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000238 RID: 568
[EntityConfigOrder(1)]
public class HatchHardConfig : IEntityConfig
{
	// Token: 0x060007D5 RID: 2005 RVA: 0x00169C8C File Offset: 0x00167E8C
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchHardBaseTrait", is_baby, "hvy_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchHardBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 200f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> list = BaseHatchConfig.HardRockDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		list.AddRange(BaseHatchConfig.MetalDiet(SimHashes.Carbon.CreateTag(), HatchHardConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_1, null, 0f));
		return BaseHatchConfig.SetupDiet(prefab, list, HatchHardConfig.CALORIES_PER_KG_OF_ORE, HatchHardConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00169DE8 File Offset: 0x00167FE8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchHardConfig.CreateHatch("HatchHard", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchHardEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_HARD.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchHardBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_HARD, HatchHardConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005EF RID: 1519
	public const string ID = "HatchHard";

	// Token: 0x040005F0 RID: 1520
	public const string BASE_TRAIT_ID = "HatchHardBaseTrait";

	// Token: 0x040005F1 RID: 1521
	public const string EGG_ID = "HatchHardEgg";

	// Token: 0x040005F2 RID: 1522
	private const SimHashes EMIT_ELEMENT = SimHashes.Carbon;

	// Token: 0x040005F3 RID: 1523
	private static float KG_ORE_EATEN_PER_CYCLE = 140f;

	// Token: 0x040005F4 RID: 1524
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchHardConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040005F5 RID: 1525
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040005F6 RID: 1526
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 2;
}
