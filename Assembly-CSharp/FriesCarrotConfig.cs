using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class FriesCarrotConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000C0C RID: 3084 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00179E60 File Offset: 0x00178060
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("FriesCarrot", STRINGS.ITEMS.FOOD.FRIESCARROT.NAME, STRINGS.ITEMS.FOOD.FRIESCARROT.DESC, 1f, false, Assets.GetAnim("rootfries_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.FRIES_CARROT);
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x0400093D RID: 2365
	public const string ID = "FriesCarrot";

	// Token: 0x0400093E RID: 2366
	public static ComplexRecipe recipe;
}
