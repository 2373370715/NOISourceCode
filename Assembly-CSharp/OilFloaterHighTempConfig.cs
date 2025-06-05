using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class OilFloaterHighTempConfig : IEntityConfig
{
	// Token: 0x06000880 RID: 2176 RVA: 0x0016C120 File Offset: 0x0016A320
	public static GameObject CreateOilFloater(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterHighTempBaseTrait", 373.15f, 473.15f, 323.15f, 573.15f, is_baby, "hot_");
		EntityTemplates.ExtendEntityToWildCreature(prefab, OilFloaterTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("OilfloaterHighTempBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
		return BaseOilFloaterConfig.SetupDiet(prefab, SimHashes.CarbonDioxide.CreateTag(), SimHashes.Petroleum.CreateTag(), OilFloaterHighTempConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL, null, 0f, OilFloaterHighTempConfig.MIN_POOP_SIZE_IN_KG);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0016C26C File Offset: 0x0016A46C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = OilFloaterHighTempConfig.CreateOilFloater("OilfloaterHighTemp", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "oilfloater_kanim", false);
		EntityTemplates.ExtendEntityToFertileCreature(gameObject, this as IHasDlcRestrictions, "OilfloaterHighTempEgg", STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.EGG_NAME, STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_HIGHTEMP.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterHighTempBaby", 60.000004f, 20f, OilFloaterTuning.EGG_CHANCES_HIGHTEMP, OilFloaterHighTempConfig.EGG_SORT_ORDER, true, false, 1f, false);
		return gameObject;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400067A RID: 1658
	public const string ID = "OilfloaterHighTemp";

	// Token: 0x0400067B RID: 1659
	public const string BASE_TRAIT_ID = "OilfloaterHighTempBaseTrait";

	// Token: 0x0400067C RID: 1660
	public const string EGG_ID = "OilfloaterHighTempEgg";

	// Token: 0x0400067D RID: 1661
	public const SimHashes CONSUME_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x0400067E RID: 1662
	public const SimHashes EMIT_ELEMENT = SimHashes.Petroleum;

	// Token: 0x0400067F RID: 1663
	private static float KG_ORE_EATEN_PER_CYCLE = 20f;

	// Token: 0x04000680 RID: 1664
	private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterHighTempConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x04000681 RID: 1665
	private static float MIN_POOP_SIZE_IN_KG = 0.5f;

	// Token: 0x04000682 RID: 1666
	public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 1;
}
