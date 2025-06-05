using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class PuftAlphaConfig : IEntityConfig
{
	// Token: 0x060008A9 RID: 2217 RVA: 0x0016C6C4 File Offset: 0x0016A8C4
	public static GameObject CreatePuftAlpha(string id, string name, string desc, string anim_file, bool is_baby)
	{
		string symbol_override_prefix = "alp_";
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftAlphaBaseTrait", anim_file, is_baby, symbol_override_prefix, 293.15f, 313.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftAlphaBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, new List<Diet.Info>
		{
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ContaminatedOxygen.CreateTag()
			}), SimHashes.SlimeMold.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.ChlorineGas.CreateTag()
			}), SimHashes.BleachStone.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null),
			new Diet.Info(new HashSet<Tag>(new Tag[]
			{
				SimHashes.Oxygen.CreateTag()
			}), SimHashes.OxyRock.CreateTag(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.BAD_2, "SlimeLung", 0f, false, Diet.Info.FoodType.EatSolid, false, null)
		}.ToArray(), PuftAlphaConfig.CALORIES_PER_KG_OF_ORE, PuftAlphaConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		return gameObject;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x0016C8EC File Offset: 0x0016AAEC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftAlphaConfig.CreatePuftAlpha("PuftAlpha", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftAlphaEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_ALPHA.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftAlphaBaby", 45f, 15f, PuftTuning.EGG_CHANCES_ALPHA, PuftAlphaConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x000AE55F File Offset: 0x000AC75F
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<KBatchedAnimController>().animScale *= 1.1f;
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x04000698 RID: 1688
	public const string ID = "PuftAlpha";

	// Token: 0x04000699 RID: 1689
	public const string BASE_TRAIT_ID = "PuftAlphaBaseTrait";

	// Token: 0x0400069A RID: 1690
	public const string EGG_ID = "PuftAlphaEgg";

	// Token: 0x0400069B RID: 1691
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x0400069C RID: 1692
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x0400069D RID: 1693
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x0400069E RID: 1694
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x0400069F RID: 1695
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040006A0 RID: 1696
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftAlphaConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006A1 RID: 1697
	private static float MIN_POOP_SIZE_IN_KG = 5f;

	// Token: 0x040006A2 RID: 1698
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 1;
}
