using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class LightBugOrangeConfig : IEntityConfig
{
	// Token: 0x0600082D RID: 2093 RVA: 0x0016ADF0 File Offset: 0x00168FF0
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugOrangeBaseTrait", LIGHT2D.LIGHTBUG_COLOR_ORANGE, DECOR.BONUS.TIER6, is_baby, "org_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugOrangeBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 25f, name, false, false, true));
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(TagManager.Create(MushroomConfig.ID));
		hashSet.Add(TagManager.Create("FriedMushroom"));
		hashSet.Add(TagManager.Create("GrilledPrickleFruit"));
		if (DlcManager.IsContentSubscribed("DLC2_ID"))
		{
			hashSet.Add(TagManager.Create("CookedPikeapple"));
		}
		hashSet.Add(SimHashes.Phosphorite.CreateTag());
		return BaseLightBugConfig.SetupDiet(prefab, hashSet, Tag.Invalid, LightBugOrangeConfig.CALORIES_PER_KG_OF_ORE);
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x0016AF78 File Offset: 0x00169178
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugOrangeConfig.CreateLightBug("LightBugOrange", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugOrangeEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_ORANGE.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugOrangeBaby", 15.000001f, 5f, LightBugTuning.EGG_CHANCES_ORANGE, LightBugOrangeConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x000ADF93 File Offset: 0x000AC193
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000630 RID: 1584
	public const string ID = "LightBugOrange";

	// Token: 0x04000631 RID: 1585
	public const string BASE_TRAIT_ID = "LightBugOrangeBaseTrait";

	// Token: 0x04000632 RID: 1586
	public const string EGG_ID = "LightBugOrangeEgg";

	// Token: 0x04000633 RID: 1587
	private static float KG_ORE_EATEN_PER_CYCLE = 0.25f;

	// Token: 0x04000634 RID: 1588
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugOrangeConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000635 RID: 1589
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 1;
}
