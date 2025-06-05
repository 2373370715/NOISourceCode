using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class PuftBleachstoneConfig : IEntityConfig
{
	// Token: 0x060008B3 RID: 2227 RVA: 0x0016C96C File Offset: 0x0016AB6C
	public static GameObject CreatePuftBleachstone(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = BasePuftConfig.BasePuft(id, name, desc, "PuftBleachstoneBaseTrait", anim_file, is_baby, "anti_", 273.15f, 333.15f, 223.15f, 373.15f);
		gameObject = EntityTemplates.ExtendEntityToWildCreature(gameObject, PuftTuning.PEN_SIZE_PER_CREATURE);
		Trait trait = Db.Get().CreateTrait("PuftBleachstoneBaseTrait", name, name, null, false, null, true, true);
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, PuftTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -PuftTuning.STANDARD_CALORIES_PER_CYCLE / 600f, UI.TOOLTIPS.BASE_VALUE, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
		trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 75f, name, false, false, true));
		gameObject = BasePuftConfig.SetupDiet(gameObject, SimHashes.ChlorineGas.CreateTag(), SimHashes.BleachStone.CreateTag(), PuftBleachstoneConfig.CALORIES_PER_KG_OF_ORE, TUNING.CREATURES.CONVERSION_EFFICIENCY.GOOD_2, null, 0f, PuftBleachstoneConfig.MIN_POOP_SIZE_IN_KG);
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		return gameObject;
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0016CAE8 File Offset: 0x0016ACE8
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(PuftBleachstoneConfig.CreatePuftBleachstone("PuftBleachstone", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "puft_kanim", false), this as IHasDlcRestrictions, "PuftBleachstoneEgg", STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.EGG_NAME, STRINGS.CREATURES.SPECIES.PUFT.VARIANT_BLEACHSTONE.DESC, "egg_puft_kanim", PuftTuning.EGG_MASS, "PuftBleachstoneBaby", 45f, 15f, PuftTuning.EGG_CHANCES_BLEACHSTONE, PuftBleachstoneConfig.EGG_SORT_ORDER, true, false, 1f, false);
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000AE578 File Offset: 0x000AC778
	public void OnSpawn(GameObject inst)
	{
		BasePuftConfig.OnSpawn(inst);
	}

	// Token: 0x040006A4 RID: 1700
	public const string ID = "PuftBleachstone";

	// Token: 0x040006A5 RID: 1701
	public const string BASE_TRAIT_ID = "PuftBleachstoneBaseTrait";

	// Token: 0x040006A6 RID: 1702
	public const string EGG_ID = "PuftBleachstoneEgg";

	// Token: 0x040006A7 RID: 1703
	public const SimHashes CONSUME_ELEMENT = SimHashes.ChlorineGas;

	// Token: 0x040006A8 RID: 1704
	public const SimHashes EMIT_ELEMENT = SimHashes.BleachStone;

	// Token: 0x040006A9 RID: 1705
	private static float KG_ORE_EATEN_PER_CYCLE = 30f;

	// Token: 0x040006AA RID: 1706
	private static float CALORIES_PER_KG_OF_ORE = PuftTuning.STANDARD_CALORIES_PER_CYCLE / PuftBleachstoneConfig.KG_ORE_EATEN_PER_CYCLE;

	// Token: 0x040006AB RID: 1707
	private static float MIN_POOP_SIZE_IN_KG = 15f;

	// Token: 0x040006AC RID: 1708
	public static int EGG_SORT_ORDER = PuftConfig.EGG_SORT_ORDER + 3;
}
