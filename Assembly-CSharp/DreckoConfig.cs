using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class DreckoConfig : IEntityConfig
{
	// Token: 0x0600079F RID: 1951 RVA: 0x00168E70 File Offset: 0x00167070
	public static GameObject CreateDrecko(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseDreckoConfig.BaseDrecko(id, name, desc, anim_file, "DreckoBaseTrait", is_baby, "fbr_", 283.15f, 333.15f, 243.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, DreckoTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("DreckoBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DreckoTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -DreckoTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpiceVine".ToTag(),
				SwampLilyConfig.ID.ToTag(),
				"BasicSingleHarvestPlant".ToTag()
			}, DreckoConfig.POOP_ELEMENT, DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, DreckoConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = DreckoConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		ScaleGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ScaleGrowthMonitor.Def>();
		def2.defaultGrowthRate = 1f / DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES / 600f;
		def2.dropMass = DreckoConfig.FIBER_PER_CYCLE * DreckoConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def2.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
		def2.levelCount = 6;
		def2.targetAtmosphere = SimHashes.Hydrogen;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0016906C File Offset: 0x0016726C
	public virtual GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(DreckoConfig.CreateDrecko("Drecko", CREATURES.SPECIES.DRECKO.NAME, CREATURES.SPECIES.DRECKO.DESC, "drecko_kanim", false), this as IHasDlcRestrictions, "DreckoEgg", CREATURES.SPECIES.DRECKO.EGG_NAME, CREATURES.SPECIES.DRECKO.DESC, "egg_drecko_kanim", DreckoTuning.EGG_MASS, "DreckoBaby", 90f, 30f, DreckoTuning.EGG_CHANCES_BASE, DreckoConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005B2 RID: 1458
	public const string ID = "Drecko";

	// Token: 0x040005B3 RID: 1459
	public const string BASE_TRAIT_ID = "DreckoBaseTrait";

	// Token: 0x040005B4 RID: 1460
	public const string EGG_ID = "DreckoEgg";

	// Token: 0x040005B5 RID: 1461
	public static Tag POOP_ELEMENT = SimHashes.Phosphorite.CreateTag();

	// Token: 0x040005B6 RID: 1462
	public static Tag EMIT_ELEMENT = BasicFabricConfig.ID;

	// Token: 0x040005B7 RID: 1463
	private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.75f;

	// Token: 0x040005B8 RID: 1464
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = DreckoTuning.STANDARD_CALORIES_PER_CYCLE / DreckoConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x040005B9 RID: 1465
	private static float KG_POOP_PER_DAY_OF_PLANT = 13.33f;

	// Token: 0x040005BA RID: 1466
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x040005BB RID: 1467
	private static float MIN_POOP_SIZE_IN_CALORIES = DreckoConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * DreckoConfig.MIN_POOP_SIZE_IN_KG / DreckoConfig.KG_POOP_PER_DAY_OF_PLANT;

	// Token: 0x040005BC RID: 1468
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 8f;

	// Token: 0x040005BD RID: 1469
	public static float FIBER_PER_CYCLE = 0.25f;

	// Token: 0x040005BE RID: 1470
	public static int EGG_SORT_ORDER = 800;
}
