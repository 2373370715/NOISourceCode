using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class MooConfig : IEntityConfig
{
	// Token: 0x06000861 RID: 2145 RVA: 0x0016BACC File Offset: 0x00169CCC
	public static GameObject CreateMoo(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseMooConfig.BaseMoo(id, name, CREATURES.SPECIES.MOO.DESC, "MooBaseTrait", anim_file, is_baby, null);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MooTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MooBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MooTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MooTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, MooTuning.STANDARD_LIFESPAN, name, false, false, true));
		Diet diet = new Diet(new Diet.Info[]
		{
			new Diet.Info(new HashSet<Tag>
			{
				"GasGrass".ToTag()
			}, MooConfig.POOP_ELEMENT, MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, MooConfig.KG_POOP_PER_DAY_OF_PLANT, null, 0f, false, Diet.Info.FoodType.EatPlantDirectly, false, null)
		});
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MooConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x000AE2CE File Offset: 0x000AC4CE
	public GameObject CreatePrefab()
	{
		return MooConfig.CreateMoo("Moo", CREATURES.SPECIES.MOO.NAME, CREATURES.SPECIES.MOO.DESC, "gassy_moo_kanim", false);
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x000AE2F4 File Offset: 0x000AC4F4
	public void OnSpawn(GameObject inst)
	{
		BaseMooConfig.OnSpawn(inst);
	}

	// Token: 0x0400065A RID: 1626
	public const string ID = "Moo";

	// Token: 0x0400065B RID: 1627
	public const string BASE_TRAIT_ID = "MooBaseTrait";

	// Token: 0x0400065C RID: 1628
	public const SimHashes CONSUME_ELEMENT = SimHashes.Carbon;

	// Token: 0x0400065D RID: 1629
	public static Tag POOP_ELEMENT = SimHashes.Methane.CreateTag();

	// Token: 0x0400065E RID: 1630
	public static readonly float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 2f;

	// Token: 0x0400065F RID: 1631
	private static float CALORIES_PER_DAY_OF_PLANT_EATEN = MooTuning.STANDARD_CALORIES_PER_CYCLE / MooConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;

	// Token: 0x04000660 RID: 1632
	private static float KG_POOP_PER_DAY_OF_PLANT = 5f;

	// Token: 0x04000661 RID: 1633
	private static float MIN_POOP_SIZE_IN_KG = 1.5f;

	// Token: 0x04000662 RID: 1634
	private static float MIN_POOP_SIZE_IN_CALORIES = MooConfig.CALORIES_PER_DAY_OF_PLANT_EATEN * MooConfig.MIN_POOP_SIZE_IN_KG / MooConfig.KG_POOP_PER_DAY_OF_PLANT;
}
