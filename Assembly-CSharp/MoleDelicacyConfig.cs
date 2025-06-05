using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000251 RID: 593
public class MoleDelicacyConfig : IEntityConfig
{
	// Token: 0x06000856 RID: 2134 RVA: 0x0016B740 File Offset: 0x00169940
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, desc, "MoleDelicacyBaseTrait", anim_file, is_baby, 173.15f, 373.15f, 73.149994f, 773.15f, "del_", 5);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleDelicacyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.DELICACY_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleDelicacyConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MoleDelicacyConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		if (!is_baby)
		{
			ElementGrowthMonitor.Def def2 = gameObject.AddOrGetDef<ElementGrowthMonitor.Def>();
			def2.defaultGrowthRate = 1f / MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES / 600f;
			def2.dropMass = MoleDelicacyConfig.GINGER_PER_CYCLE * MoleDelicacyConfig.GINGER_GROWTH_TIME_IN_CYCLES;
			def2.itemDroppedOnShear = MoleDelicacyConfig.SHEAR_DROP_ELEMENT;
			def2.levelCount = 5;
			def2.minTemperature = MoleDelicacyConfig.MIN_GROWTH_TEMPERATURE;
			def2.maxTemperature = MoleDelicacyConfig.MAX_GROWTH_TEMPERATURE;
		}
		else
		{
			gameObject.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}
		return gameObject;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0016B970 File Offset: 0x00169B70
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(MoleDelicacyConfig.CreateMole("MoleDelicacy", STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.NAME, STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC, "driller_kanim", false), this as IHasDlcRestrictions, "MoleDelicacyEgg", STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.EGG_NAME, STRINGS.CREATURES.SPECIES.MOLE.VARIANT_DELICACY.DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleDelicacyBaby", 60.000004f, 20f, MoleTuning.EGG_CHANCES_DELICACY, MoleDelicacyConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x000AE288 File Offset: 0x000AC488
	public void OnSpawn(GameObject inst)
	{
		MoleDelicacyConfig.SetSpawnNavType(inst);
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0016B9F0 File Offset: 0x00169BF0
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		if (component != null)
		{
			if (Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
				inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
				return;
			}
			inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}
	}

	// Token: 0x0400064C RID: 1612
	public const string ID = "MoleDelicacy";

	// Token: 0x0400064D RID: 1613
	public const string BASE_TRAIT_ID = "MoleDelicacyBaseTrait";

	// Token: 0x0400064E RID: 1614
	public const string EGG_ID = "MoleDelicacyEgg";

	// Token: 0x0400064F RID: 1615
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x04000650 RID: 1616
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x04000651 RID: 1617
	public static int EGG_SORT_ORDER = 800;

	// Token: 0x04000652 RID: 1618
	public static float GINGER_GROWTH_TIME_IN_CYCLES = 8f;

	// Token: 0x04000653 RID: 1619
	public static float GINGER_PER_CYCLE = 1f;

	// Token: 0x04000654 RID: 1620
	public static Tag SHEAR_DROP_ELEMENT = GingerConfig.ID;

	// Token: 0x04000655 RID: 1621
	public static float MIN_GROWTH_TEMPERATURE = 343.15f;

	// Token: 0x04000656 RID: 1622
	public static float MAX_GROWTH_TEMPERATURE = 353.15f;

	// Token: 0x04000657 RID: 1623
	public static float EGG_CHANCES_TEMPERATURE_MIN = 333.15f;

	// Token: 0x04000658 RID: 1624
	public static float EGG_CHANCES_TEMPERATURE_MAX = 373.15f;
}
