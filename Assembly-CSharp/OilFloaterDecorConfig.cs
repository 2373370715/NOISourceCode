using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class OilFloaterDecorConfig : IEntityConfig
{
	// Token: 0x06000876 RID: 2166 RVA: 0x0016BF48 File Offset: 0x0016A148
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterDecorBaseTrait", 273.15f, 323.15f, 223.15f, 373.15f, is_baby, "oxy_");
		gameObject.AddOrGet<DecorProvider>().SetValues(DECOR.BONUS.TIER6);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterDecorBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), Tag.Invalid, OilFloaterDecorConfig.CALORIES_PER_KG_OF_ORE, 0f, null, 0f, 0f);
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0016C09C File Offset: 0x0016A29C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecor", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "OilfloaterDecorEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterDecorBaby", 90f, 30f, OilFloaterTuning.EGG_CHANCES_DECOR, OilFloaterDecorConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000672 RID: 1650
	public const string ID = "OilfloaterDecor";

	// Token: 0x04000673 RID: 1651
	public const string BASE_TRAIT_ID = "OilfloaterDecorBaseTrait";

	// Token: 0x04000674 RID: 1652
	public const string EGG_ID = "OilfloaterDecorEgg";

	// Token: 0x04000675 RID: 1653
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x04000676 RID: 1654
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x04000677 RID: 1655
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterDecorConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000678 RID: 1656
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 2;
}
