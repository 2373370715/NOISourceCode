﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200022F RID: 559
public class DreckoPlasticConfig : IEntityConfig
{
	// Token: 0x060007A9 RID: 1961 RVA: 0x0016917C File Offset: 0x0016737C
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoPlasticBaseTrait", is_baby, null, 293.15f, 323.15f, 243.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoPlasticBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"BasicSingleHarvestPlant".ToTag(),
				"PrickleFlower".ToTag()
			}, DreckoPlasticConfig.POOP_ELEMENT, DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = DreckoPlasticConfig.MIN_POOP_SIZE_IN_CALORIES;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoPlasticConfig.PLASTIC_PER_CYCLE * DreckoPlasticConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoPlasticConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return gameObject;
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x00169358 File Offset: 0x00167558
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DreckoPlasticConfig.CreateDrecko("DreckoPlastic", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC, "drecko_kanim", false), this as IHasDlcRestrictions, "DreckoPlasticEgg", CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.EGG_NAME, CREATURES.SPECIES.DRECKO.VARIANT_PLASTIC.DESC, "egg_drecko_kanim", DreckoTuning.EGG_MASS, "DreckoPlasticBaby", 90f, 30f, DreckoTuning.EGG_CHANCES_PLASTIC, DreckoPlasticConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C0 RID: 1472
	public const string ID = "DreckoPlastic";

	// Token: 0x040005C1 RID: 1473
	public const string BASE_TRAIT_ID = "DreckoPlasticBaseTrait";

	// Token: 0x040005C2 RID: 1474
	public const string EGG_ID = "DreckoPlasticEgg";

	// Token: 0x040005C3 RID: 1475
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x040005C4 RID: 1476
	public static Tag EMIT_ELEMENT = SimHashes.Polypropylene.CreateTag();

	// Token: 0x040005C5 RID: 1477
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 1f;

	// Token: 0x040005C6 RID: 1478
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoPlasticConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040005C7 RID: 1479
	private static float KG_POOP_PER_DAY_OF_PLANT = 9f;

	// Token: 0x040005C8 RID: 1480
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x040005C9 RID: 1481
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoPlasticConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoPlasticConfig.MIN_POOP_SIZE_IN_KG / DreckoPlasticConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x040005CA RID: 1482
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 3f;

	// Token: 0x040005CB RID: 1483
	public static float PLASTIC_PER_CYCLE = 50f;

	// Token: 0x040005CC RID: 1484
	public static int EGG_SORT_ORDER = 800;
}
