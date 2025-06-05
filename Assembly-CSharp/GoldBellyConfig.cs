using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000233 RID: 563
[EntityConfigOrder(1)]
public class GoldBellyConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x060007BA RID: 1978 RVA: 0x00169760 File Offset: 0x00167960
	public static GameObject CreateGoldBelly(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BaseBellyConfig.BaseBelly(id, name, desc, anim_file, "GoldBellyBaseTrait", is_baby, "king_"), MooTuning.PEN_SIZE_PER_CREATURE);
		gameObject.AddOrGet<WarmBlooded>().BaseGenerationKW = 1.3f;
		Trait trait = Db.Get().CreateTrait("GoldBellyBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, BellyTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -BellyTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 200f, name, false, false, true));
		string alwaysShowDisease = "PollenGerms";
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = alwaysShowDisease;
		WellFedShearable.Def def = gameObject.AddOrGetDef<WellFedShearable.Def>();
		def.effectId = "GoldBellyWellFed";
		def.caloriesPerCycle = BellyTuning.STANDARD_CALORIES_PER_CYCLE;
		def.growthDurationCycles = GoldBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES;
		def.dropMass = 1f;
		def.itemDroppedOnShear = GoldBellyConfig.SCALE_GROWTH_EMIT_ELEMENT;
		def.requiredDiet = "FriesCarrot";
		def.levelCount = 6;
		def.scaleGrowthSymbols = GoldBellyConfig.SCALE_SYMBOLS;
		GameObject gameObject2 = BaseBellyConfig.SetupDiet(gameObject, BaseBellyConfig.StandardDiets(), BellyTuning.CALORIES_PER_UNIT_EATEN, 1f);
		gameObject2.AddTag(GameTags.OriginalCreature);
		return gameObject2;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00169904 File Offset: 0x00167B04
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(GoldBellyConfig.CreateGoldBelly("GoldBelly", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "ice_belly_kanim", false), this, "GoldBellyEgg", CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.EGG_NAME, CREATURES.SPECIES.ICEBELLY.VARIANT_GOLD.DESC, "egg_icebelly_kanim", 8f, "GoldBellyBaby", 120.00001f, 40f, BellyTuning.EGG_CHANCES_GOLD, GoldBellyConfig.EGG_SORT_ORDER, true, false, 1f, false);
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>();
		gameObject.AddTag(GameTags.LargeCreature);
		return gameObject;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005D9 RID: 1497
	public const string ID = "GoldBelly";

	// Token: 0x040005DA RID: 1498
	public const string BASE_TRAIT_ID = "GoldBellyBaseTrait";

	// Token: 0x040005DB RID: 1499
	public const string EGG_ID = "GoldBellyEgg";

	// Token: 0x040005DC RID: 1500
	public const int GERMS_EMMITED_PER_KG_POOPED = 1000;

	// Token: 0x040005DD RID: 1501
	public static Tag SCALE_GROWTH_EMIT_ELEMENT = "GoldBellyCrown";

	// Token: 0x040005DE RID: 1502
	public static float SCALE_INITIAL_GROWTH_PCT = 0.25f;

	// Token: 0x040005DF RID: 1503
	public static float SCALE_GROWTH_TIME_IN_CYCLES = 10f;

	// Token: 0x040005E0 RID: 1504
	public static float GOLD_PER_CYCLE = 25f;

	// Token: 0x040005E1 RID: 1505
	public static int EGG_SORT_ORDER = 0;

	// Token: 0x040005E2 RID: 1506
	public static KAnimHashedString[] SCALE_SYMBOLS = new KAnimHashedString[]
	{
		"antler_0",
		"antler_1",
		"antler_2",
		"antler_3",
		"antler_4"
	};
}
