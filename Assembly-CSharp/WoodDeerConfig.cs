using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class WoodDeerConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000931 RID: 2353 RVA: 0x0016E444 File Offset: 0x0016C644
	public static GameObject CreateWoodDeer(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = EntityTemplates.ExtendEntityToWildCreature(BaseDeerConfig.BaseDeer(id, name, desc, anim_file, "WoodDeerBaseTrait", is_baby, null), DeerTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("WoodDeerBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, 1000000f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -166.66667f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		GameObject gameObject = BaseDeerConfig.SetupDiet(prefab, new List<Diet.Info>
		{
			BaseDeerConfig.CreateDietInfo("HardSkinBerryPlant", SimHashes.Dirt.CreateTag(), WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				"HardSkinBerry"
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 3f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			BaseDeerConfig.CreateDietInfo("PrickleFlower", SimHashes.Dirt.CreateTag(), WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 2f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER, null, 0f),
			new Diet.Info(new HashSet<Tag>
			{
				PrickleFruitConfig.ID
			}, SimHashes.Dirt.CreateTag(), WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS * WoodDeerConfig.BRISTLE_CALORIES_PER_KG / 1f, WoodDeerConfig.POOP_MASS_CONVERSION_MULTIPLIER * 6f, null, 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), WoodDeerConfig.MIN_KG_CONSUMED_BEFORE_POOPING);
		gameObject.AddTag(GameTags.OriginalCreature);
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "WoodDeerWellFed";
		def.caloriesPerCycle = 100000f;
		def.growthDurationCycles = WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;
		def.dropMass = WoodDeerConfig.WOOD_MASS_PER_ANTLER;
		def.itemDroppedOnShear = WoodLogConfig.TAG;
		def.levelCount = 6;
		return gameObject;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0016E6A8 File Offset: 0x0016C8A8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(WoodDeerConfig.CreateWoodDeer("WoodDeer", STRINGS.CREATURES.SPECIES.WOODDEER.NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "ice_floof_kanim", false), this, "WoodDeerEgg", STRINGS.CREATURES.SPECIES.WOODDEER.EGG_NAME, STRINGS.CREATURES.SPECIES.WOODDEER.DESC, "egg_ice_floof_kanim", DeerTuning.EGG_MASS, "WoodDeerBaby", 60.000004f, 20f, DeerTuning.EGG_CHANCES_BASE, WoodDeerConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000713 RID: 1811
	public const string ID = "WoodDeer";

	// Token: 0x04000714 RID: 1812
	public const string BASE_TRAIT_ID = "WoodDeerBaseTrait";

	// Token: 0x04000715 RID: 1813
	public const string EGG_ID = "WoodDeerEgg";

	// Token: 0x04000716 RID: 1814
	private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

	// Token: 0x04000717 RID: 1815
	public const float CALORIES_PER_PLANT_BITE = 100000f;

	// Token: 0x04000718 RID: 1816
	public const float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.2f;

	// Token: 0x04000719 RID: 1817
	public static float CONSUMABLE_PLANT_MATURITY_LEVELS = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == "HardSkinBerry").cropDuration / 600f;

	// Token: 0x0400071A RID: 1818
	public static float KG_PLANT_EATEN_A_DAY = 0.2f * WoodDeerConfig.CONSUMABLE_PLANT_MATURITY_LEVELS;

	// Token: 0x0400071B RID: 1819
	public static float HARD_SKIN_CALORIES_PER_KG = 100000f / WoodDeerConfig.KG_PLANT_EATEN_A_DAY;

	// Token: 0x0400071C RID: 1820
	public static float BRISTLE_CALORIES_PER_KG = WoodDeerConfig.HARD_SKIN_CALORIES_PER_KG * 2f;

	// Token: 0x0400071D RID: 1821
	public static float ANTLER_GROWTH_TIME_IN_CYCLES = 6f;

	// Token: 0x0400071E RID: 1822
	public static float ANTLER_STARTING_GROWTH_PCT = 0.5f;

	// Token: 0x0400071F RID: 1823
	public static float WOOD_PER_CYCLE = 60f;

	// Token: 0x04000720 RID: 1824
	public static float WOOD_MASS_PER_ANTLER = WoodDeerConfig.WOOD_PER_CYCLE * WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES;

	// Token: 0x04000721 RID: 1825
	private static float POOP_MASS_CONVERSION_MULTIPLIER = 8.333334f;

	// Token: 0x04000722 RID: 1826
	private static float MIN_KG_CONSUMED_BEFORE_POOPING = 1f;

	// Token: 0x04000723 RID: 1827
	public static int EGG_SORT_ORDER = 0;
}
