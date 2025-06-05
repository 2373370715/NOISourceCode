using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200023A RID: 570
[EntityConfigOrder(1)]
public class HatchMetalConfig : IEntityConfig
{
	// Token: 0x1700001B RID: 27
	// (get) Token: 0x060007DF RID: 2015 RVA: 0x00169E68 File Offset: 0x00168068
	public static HashSet<Tag> METAL_ORE_TAGS
	{
		get
		{
			HashSet<Tag> hashSet = new HashSet<Tag>
			{
				SimHashes.Cuprite.CreateTag(),
				SimHashes.GoldAmalgam.CreateTag(),
				SimHashes.IronOre.CreateTag(),
				SimHashes.Wolframite.CreateTag(),
				SimHashes.AluminumOre.CreateTag()
			};
			if (DlcManager.IsExpansion1Active())
			{
				hashSet.Add(SimHashes.Cobaltite.CreateTag());
			}
			return hashSet;
		}
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00169EEC File Offset: 0x001680EC
	public static GameObject CreateHatch(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseHatchConfig.BaseHatch(id, name, desc, anim_file, "HatchMetalBaseTrait", is_baby, "mtl_"), HatchTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("HatchMetalBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, HatchTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -HatchTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 400f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseHatchConfig.MetalDiet(GameTags.Metal, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_1, null, 0f);
		return BaseHatchConfig.SetupDiet(prefab, diet_infos, HatchMetalConfig.CALORIES_PER_KG_OF_ORE, HatchMetalConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0016A01C File Offset: 0x0016821C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(HatchMetalConfig.CreateHatch("HatchMetal", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "hatch_kanim", false), this as IHasDlcRestrictions, "HatchMetalEgg", STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.EGG_NAME, STRINGS.CREATURES.SPECIES.HATCH.VARIANT_METAL.DESC, "egg_hatch_kanim", HatchTuning.EGG_MASS, "HatchMetalBaby", 60.000004f, 20f, HatchTuning.EGG_CHANCES_METAL, HatchMetalConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005F8 RID: 1528
	public const string ID = "HatchMetal";

	// Token: 0x040005F9 RID: 1529
	public const string BASE_TRAIT_ID = "HatchMetalBaseTrait";

	// Token: 0x040005FA RID: 1530
	public const string EGG_ID = "HatchMetalEgg";

	// Token: 0x040005FB RID: 1531
	private static float KG_ORE_EATEN_PER_CYCLE = 100f;

	// Token: 0x040005FC RID: 1532
	private static float CALORIES_PER_KG_OF_ORE = HatchTuning.STANDARD_CALORIES_PER_CYCLE / HatchMetalConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040005FD RID: 1533
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x040005FE RID: 1534
	public static int EGG_SORT_ORDER = HatchConfig.EGG_SORT_ORDER + 3;
}
