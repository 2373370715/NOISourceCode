using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000241 RID: 577
public class LightBugBlackConfig : IEntityConfig
{
	// Token: 0x06000805 RID: 2053 RVA: 0x0016A508 File Offset: 0x00168708
	public static GameObject CreateLightBug(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseLightBugConfig.BaseLightBug(id, name, desc, anim_file, "LightBugBlackBaseTrait", Color.black, DECOR.BONUS.TIER7, is_baby, "blk_");
		EntityTemplates.ExtendEntityToWildCreature(gameObject, LightBugTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("LightBugBlackBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, LightBugTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -LightBugTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 5f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BaseLightBugConfig.SetupDiet(gameObject, new HashSet<Tag>
		{
			TagManager.Create("Salsa"),
			TagManager.Create("Meat"),
			TagManager.Create("CookedMeat"),
			SimHashes.Katairite.CreateTag(),
			SimHashes.Phosphorus.CreateTag()
		}, Tag.Invalid, LightBugBlackConfig.CALORIES_PER_KG_OF_ORE);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.Phosphorus.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0016A6B8 File Offset: 0x001688B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = LightBugBlackConfig.CreateLightBug("LightBugBlack", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "lightbug_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "LightBugBlackEgg", STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.EGG_NAME, STRINGS.CREATURES.SPECIES.LIGHTBUG.VARIANT_BLACK.DESC, "egg_lightbug_kanim", LightBugTuning.EGG_MASS, "LightBugBlackBaby", 45f, 15f, LightBugTuning.EGG_CHANCES_BLACK, LightBugBlackConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000ADF93 File Offset: 0x000AC193
	public void OnSpawn(GameObject inst)
	{
		BaseLightBugConfig.SetupLoopingSounds(inst);
	}

	// Token: 0x04000614 RID: 1556
	public const string ID = "LightBugBlack";

	// Token: 0x04000615 RID: 1557
	public const string BASE_TRAIT_ID = "LightBugBlackBaseTrait";

	// Token: 0x04000616 RID: 1558
	public const string EGG_ID = "LightBugBlackEgg";

	// Token: 0x04000617 RID: 1559
	private static float KG_ORE_EATEN_PER_CYCLE = 1f;

	// Token: 0x04000618 RID: 1560
	private static float CALORIES_PER_KG_OF_ORE = LightBugTuning.STANDARD_CALORIES_PER_CYCLE / LightBugBlackConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000619 RID: 1561
	public static int EGG_SORT_ORDER = LightBugConfig.EGG_SORT_ORDER + 5;
}
