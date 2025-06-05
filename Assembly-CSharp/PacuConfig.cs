using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200025E RID: 606
[EntityConfigOrder(1)]
public class PacuConfig : IEntityConfig
{
	// Token: 0x06000896 RID: 2198 RVA: 0x0016C518 File Offset: 0x0016A718
	public static GameObject CreatePacu(string id, string name, string desc, string anim_file, bool is_baby)
	{
		return EntityTemplates.ExtendEntityToWildCreature(BasePacuConfig.CreatePrefab(id, "PacuBaseTrait", name, desc, anim_file, is_baby, null, 273.15f, 333.15f, 253.15f, 373.15f), PacuTuning.PEN_SIZE_PER_CREATURE, false);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0016C558 File Offset: 0x0016A758
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.ExtendEntityToFertileCreature(PacuConfig.CreatePacu("Pacu", CREATURES.SPECIES.PACU.NAME, CREATURES.SPECIES.PACU.DESC, "pacu_kanim", false), this as IHasDlcRestrictions, "PacuEgg", CREATURES.SPECIES.PACU.EGG_NAME, CREATURES.SPECIES.PACU.DESC, "egg_pacu_kanim", PacuTuning.EGG_MASS, "PacuBaby", 15.000001f, 5f, PacuTuning.EGG_CHANCES_BASE, 500, false, true, 0.75f, false);
		gameObject.AddTag(GameTags.OriginalCreature);
		return gameObject;
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000AE4CE File Offset: 0x000AC6CE
	public void OnPrefabInit(GameObject prefab)
	{
		prefab.AddOrGet<LoopingSounds>();
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400068D RID: 1677
	public const string ID = "Pacu";

	// Token: 0x0400068E RID: 1678
	public const string BASE_TRAIT_ID = "PacuBaseTrait";

	// Token: 0x0400068F RID: 1679
	public const string EGG_ID = "PacuEgg";

	// Token: 0x04000690 RID: 1680
	public const int EGG_SORT_ORDER = 500;
}
