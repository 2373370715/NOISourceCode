using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class DeepFriedShellfishConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BF6 RID: 3062 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00179C68 File Offset: 0x00177E68
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedShellfish", STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDSHELLFISH.DESC, 1f, false, Assets.GetAnim("deepfried_shellfish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_SHELLFISH);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000935 RID: 2357
	public const string ID = "DeepFriedShellfish";

	// Token: 0x04000936 RID: 2358
	public static ComplexRecipe recipe;
}
