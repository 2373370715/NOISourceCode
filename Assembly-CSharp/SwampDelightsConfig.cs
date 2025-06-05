using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class SwampDelightsConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000C9D RID: 3229 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0017ADBC File Offset: 0x00178FBC
	public GameObject CreatePrefab()
	{
		return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity("SwampDelights", STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.NAME, STRINGS.ITEMS.FOOD.SWAMPDELIGHTS.DESC, 1f, false, Assets.GetAnim("swamp_delights_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null), FOOD.FOOD_TYPES.SWAMP_DELIGHTS);
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000983 RID: 2435
	public const string ID = "SwampDelights";
}
