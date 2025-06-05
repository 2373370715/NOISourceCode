using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class PuftConfig : IEntityConfig
{
	// Token: 0x060008BD RID: 2237 RVA: 0x0016CB68 File Offset: 0x0016AD68
	public static GameObject CreatePuft(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject prefab = BasePuftConfig.BasePuft(id, name, STRINGS.CREATURES.SPECIES.PUFT.DESC, "PuftBaseTrait", anim_file, is_baby, null, 288.15f, 328.15f, 223.15f, 373.15f);
		EntityTemplates.ExtendEntityToWildCreature(prefab, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		GameObject gameObject = BasePuftConfig.SetupDiet(prefab, SimHashes.ContaminatedOxygen.CreateTag(), SimHashes.SlimeMold.CreateTag(), PuftConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, "SlimeLung", 0f, PuftConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0016CCD8 File Offset: 0x0016AED8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftConfig.CreatePuft("Puft", STRINGS.CREATURES.SPECIES.PUFT.NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftEgg", STRINGS.CREATURES.SPECIES.PUFT.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftBaby", 45f, 15f, PuftTuning.EGG_CHANCES_BASE, PuftConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006AE RID: 1710
	public const string ID = "Puft";

	// Token: 0x040006AF RID: 1711
	public const string BASE_TRAIT_ID = "PuftBaseTrait";

	// Token: 0x040006B0 RID: 1712
	public const string EGG_ID = "PuftEgg";

	// Token: 0x040006B1 RID: 1713
	public const SimHashes CONSUME_ELEMENT = SimHashes.ContaminatedOxygen;

	// Token: 0x040006B2 RID: 1714
	public const SimHashes EMIT_ELEMENT = SimHashes.SlimeMold;

	// Token: 0x040006B3 RID: 1715
	public const string EMIT_DISEASE = "SlimeLung";

	// Token: 0x040006B4 RID: 1716
	public const float EMIT_DISEASE_PER_KG = 0f;

	// Token: 0x040006B5 RID: 1717
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040006B6 RID: 1718
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006B7 RID: 1719
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x040006B8 RID: 1720
	public static int EGG_SORT_ORDER = 300;
}
