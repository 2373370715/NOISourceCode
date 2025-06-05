using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000302 RID: 770
public class DeepFriedMeatConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BEA RID: 3050 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00179BA0 File Offset: 0x00177DA0
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedMeat", STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDMEAT.DESC, 1f, false, Assets.GetAnim("deepfried_meat_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_MEAT);
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000931 RID: 2353
	public const string ID = "DeepFriedMeat";

	// Token: 0x04000932 RID: 2354
	public static ComplexRecipe recipe;
}
