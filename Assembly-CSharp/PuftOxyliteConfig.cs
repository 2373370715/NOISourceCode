using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class PuftOxyliteConfig : IEntityConfig
{
	// Token: 0x060008C7 RID: 2247 RVA: 0x0016CD58 File Offset: 0x0016AF58
	public static GameObject CreatePuftOxylite(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftOxyliteBaseTrait", anim_file, is_baby, "com_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftOxyliteBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), SimHashes.OxyRock.CreateTag(), PuftOxyliteConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftOxyliteConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.OxyRock.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0016CED4 File Offset: 0x0016B0D4
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftOxyliteConfig.CreatePuftOxylite("PuftOxylite", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftOxyliteEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftOxyliteBaby", 45f, 15f, PuftTuning.EGG_CHANCES_OXYLITE, PuftOxyliteConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006BA RID: 1722
	public const string ID = "PuftOxylite";

	// Token: 0x040006BB RID: 1723
	public const string BASE_TRAIT_ID = "PuftOxyliteBaseTrait";

	// Token: 0x040006BC RID: 1724
	public const string EGG_ID = "PuftOxyliteEgg";

	// Token: 0x040006BD RID: 1725
	public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

	// Token: 0x040006BE RID: 1726
	public const SimHashes EMIT_ELEMENT = SimHashes.OxyRock;

	// Token: 0x040006BF RID: 1727
	private static float KG_ORE_EATEN_PER_CYCLE = 50f;

	// Token: 0x040006C0 RID: 1728
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftOxyliteConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006C1 RID: 1729
	private static float MIN_POOP_SIZE_IN_KG = 25f;

	// Token: 0x040006C2 RID: 1730
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 2;
}
