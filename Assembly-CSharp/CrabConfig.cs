using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000223 RID: 547
[EntityConfigOrder(1)]
public class CrabConfig : IEntityConfig
{
	// Token: 0x06000765 RID: 1893 RVA: 0x00168184 File Offset: 0x00166384
	public static GameObject CreateCrab(string id, string name, string desc, string anim_file, bool is_baby, string deathDropID)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseCrabConfig.BaseCrab(id, name, desc, anim_file, "CrabBaseTrait", is_baby, null, deathDropID, 1), CrabTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("CrabBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, CrabTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -CrabTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		List<Diet.Info> diet_infos = BaseCrabConfig.BasicDiet(SimHashes.Sand.CreateTag(), CrabConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f);
		GameObject gameObject = BaseCrabConfig.SetupDiet(prefab, diet_infos, CrabConfig.CALORIES_PER_KG_OF_ORE, CrabConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x001682C4 File Offset: 0x001664C4
	public GameObject CreatePrefab()
	{
		GameObject gameObject = CrabConfig.CreateCrab("Crab", STRINGS.CREATURES.SPECIES.CRAB.NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "pincher_kanim", false, "CrabShell");
		gameObject = EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "CrabEgg", STRINGS.CREATURES.SPECIES.CRAB.EGG_NAME, STRINGS.CREATURES.SPECIES.CRAB.DESC, "egg_pincher_kanim", CrabTuning.EGG_MASS, "CrabBaby", 60.000004f, 20f, CrabTuning.EGG_CHANCES_BASE, CrabConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddOrGetDef<EggProtectionMonitor.Def>().allyTags = new Tag[]
		{
			GameTags.Creatures.CrabFriend
		};
		return gameObject;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400057C RID: 1404
	public const string ID = "Crab";

	// Token: 0x0400057D RID: 1405
	public const string BASE_TRAIT_ID = "CrabBaseTrait";

	// Token: 0x0400057E RID: 1406
	public const string EGG_ID = "CrabEgg";

	// Token: 0x0400057F RID: 1407
	private const SimHashes EMIT_ELEMENT = SimHashes.Sand;

	// Token: 0x04000580 RID: 1408
	private static float KG_ORE_EATEN_PER_CYCLE = 70f;

	// Token: 0x04000581 RID: 1409
	private static float CALORIES_PER_KG_OF_ORE = CrabTuning.STANDARD_CALORIES_PER_CYCLE / CrabConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000582 RID: 1410
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x04000583 RID: 1411
	public static int EGG_SORT_ORDER = 0;
}
