using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class MoleConfig : IEntityConfig
{
	// Token: 0x0600084B RID: 2123 RVA: 0x0016B44C File Offset: 0x0016964C
	public static GameObject CreateMole(string id, string name, string desc, string anim_file, bool is_baby = false)
	{
		GameObject gameObject = BaseMoleConfig.BaseMole(id, name, STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby, 173.15f, 673.15f, 73.149994f, 773.15f, null, 10);
		gameObject.AddTag(GameTags.Creatures.Digger);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>
		{
			SimHashes.Regolith.CreateTag(),
			SimHashes.Dirt.CreateTag(),
			SimHashes.IronOre.CreateTag()
		}, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
		CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
		gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		foreach (HashedString hash in MoleTuning.GINGER_SYMBOL_NAMES)
		{
			gameObject.GetComponent<KAnimControllerBase>().SetSymbolVisiblity(hash, false);
		}
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0016B63C File Offset: 0x0016983C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(MoleConfig.CreateMole("Mole", STRINGS.CREATURES.SPECIES.MOLE.NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim", false), this as IHasDlcRestrictions, "MoleEgg", STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME, STRINGS.CREATURES.SPECIES.MOLE.DESC, "egg_driller_kanim", MoleTuning.EGG_MASS, "MoleBaby", 60.000004f, 20f, MoleTuning.EGG_CHANCES_BASE, MoleConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x000AE222 File Offset: 0x000AC422
	public void OnSpawn(GameObject inst)
	{
		MoleConfig.SetSpawnNavType(inst);
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0016B6BC File Offset: 0x001698BC
	public static void SetSpawnNavType(GameObject inst)
	{
		int cell = Grid.PosToCell(inst);
		Navigator component = inst.GetComponent<Navigator>();
		Pickupable component2 = inst.GetComponent<Pickupable>();
		if (component != null && (component2 == null || component2.storage == null))
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

	// Token: 0x04000645 RID: 1605
	public const string ID = "Mole";

	// Token: 0x04000646 RID: 1606
	public const string BASE_TRAIT_ID = "MoleBaseTrait";

	// Token: 0x04000647 RID: 1607
	public const string EGG_ID = "MoleEgg";

	// Token: 0x04000648 RID: 1608
	private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;

	// Token: 0x04000649 RID: 1609
	private static float CALORIES_PER_KG_OF_DIRT = 1000f;

	// Token: 0x0400064A RID: 1610
	public static int EGG_SORT_ORDER = 800;
}
