using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class SealConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060008E1 RID: 2273 RVA: 0x0016D504 File Offset: 0x0016B704
	public static GameObject CreateSeal(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseSealConfig.BaseSeal(id, name, desc, anim_file, "SealBaseTrait", is_baby, null);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, SealTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("SealBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		gameObject = BaseSealConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>
			{
				"SpaceTree"
			}, SimHashes.Ethanol.CreateTag(), 2500f, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_3, null, 0f, false, Diet.Info.FoodType.EatPlantStorage, false, null),
			new Diet.Info(new HashSet<Tag>
			{
				SimHashes.Sucrose.CreateTag()
			}, SimHashes.Ethanol.CreateTag(), 3246.7532f, 1.2987013f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, new string[]
			{
				"eat_ore_pre",
				"eat_ore_loop",
				"eat_ore_pst"
			})
		}, 2500f, SealConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<CreaturePoopLoot.Def>().Loot = new CreaturePoopLoot.LootData[]
		{
			new CreaturePoopLoot.LootData
			{
				tag = "SpaceTreeSeed",
				probability = 0.2f
			}
		};
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0016D708 File Offset: 0x0016B908
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(SealConfig.CreateSeal("Seal", STRINGS.CREATURES.SPECIES.SEAL.NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "seal_kanim", false), this, "SealEgg", STRINGS.CREATURES.SPECIES.SEAL.EGG_NAME, STRINGS.CREATURES.SPECIES.SEAL.DESC, "egg_seal_kanim", SealTuning.EGG_MASS, "SealBaby", 60.000004f, 20f, SealTuning.EGG_CHANCES_BASE, SealConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040006D2 RID: 1746
	public const string ID = "Seal";

	// Token: 0x040006D3 RID: 1747
	public const string BASE_TRAIT_ID = "SealBaseTrait";

	// Token: 0x040006D4 RID: 1748
	public const string EGG_ID = "SealEgg";

	// Token: 0x040006D5 RID: 1749
	public const float SUGAR_TREE_SEED_PROBABILITY_ON_POOP = 0.2f;

	// Token: 0x040006D6 RID: 1750
	public const float SUGAR_WATER_KG_CONSUMED_PER_DAY = 40f;

	// Token: 0x040006D7 RID: 1751
	public const float CALORIES_PER_1KG_OF_SUGAR_WATER = 2500f;

	// Token: 0x040006D8 RID: 1752
	private static float MIN_POOP_SIZE_IN_KG = 10f;

	// Token: 0x040006D9 RID: 1753
	public static int EGG_SORT_ORDER = 0;
}
