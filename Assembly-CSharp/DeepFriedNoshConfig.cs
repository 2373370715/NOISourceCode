using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000303 RID: 771
public class DeepFriedNoshConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BF0 RID: 3056 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00179C04 File Offset: 0x00177E04
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedNosh", STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDNOSH.DESC, 1f, false, Assets.GetAnim("deepfried_nosh_beans_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_NOSH);
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000933 RID: 2355
	public const string ID = "DeepFriedNosh";

	// Token: 0x04000934 RID: 2356
	public static ComplexRecipe recipe;
}
