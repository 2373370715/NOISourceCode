using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000301 RID: 769
public class DeepFriedFishConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BE4 RID: 3044 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00179B3C File Offset: 0x00177D3C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("DeepFriedFish", STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.NAME, STRINGS.ITEMS.FOOD.DEEPFRIEDFISH.DESC, 1f, false, Assets.GetAnim("deepfried_fish_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.DEEP_FRIED_FISH);
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400092F RID: 2351
	public const string ID = "DeepFriedFish";

	// Token: 0x04000930 RID: 2352
	public static ComplexRecipe recipe;
}
