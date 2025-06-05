using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002FE RID: 766
public class CookedPikeappleConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BD5 RID: 3029 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x00179A04 File Offset: 0x00177C04
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("CookedPikeapple", STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.NAME, STRINGS.ITEMS.FOOD.COOKEDPIKEAPPLE.DESC, 1f, false, Assets.GetAnim("iceberry_cooked_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.COOKED_PIKEAPPLE);
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000928 RID: 2344
	public const string ID = "CookedPikeapple";

	// Token: 0x04000929 RID: 2345
	public static ComplexRecipe recipe;
}
