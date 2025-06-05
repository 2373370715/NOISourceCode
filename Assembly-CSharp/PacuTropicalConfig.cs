using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000260 RID: 608
[EntityConfigOrder(1)]
public class PacuTropicalConfig : IEntityConfig
{
	// Token: 0x0600089F RID: 2207 RVA: 0x0016C5E4 File Offset: 0x0016A7E4
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuTropicalBaseTrait", name, desc, anim_file, is_baby, "trp_", 303.15f, 353.15f, 283.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
		gameObject.AddOrGet<DecorProvider>().SetValues(PacuTropicalConfig.DECOR);
		return gameObject;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0016C638 File Offset: 0x0016A838
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFertileCreature(EntityTemplates.ExtendEntityToWildCreature(PacuTropicalConfig.CreatePacu("PacuTropical", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "pacu_kanim", false), PacuTuning.PEN_SIZE_PER_CREATURE, false), this as IHasDlcRestrictions, "PacuTropicalEgg", STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.EGG_NAME, STRINGS.CREATURES.SPECIES.PACU.VARIANT_TROPICAL.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuTropicalBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_TROPICAL, 502, false, true, 0.75f, false);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000692 RID: 1682
	public const string ID = "PacuTropical";

	// Token: 0x04000693 RID: 1683
	public const string BASE_TRAIT_ID = "PacuTropicalBaseTrait";

	// Token: 0x04000694 RID: 1684
	public const string EGG_ID = "PacuTropicalEgg";

	// Token: 0x04000695 RID: 1685
	public static readonly EffectorValues DECOR = TUNING.BUILDINGS.DECOR.BONUS.TIER4;

	// Token: 0x04000696 RID: 1686
	public const int EGG_SORT_ORDER = 502;
}
