using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class PemmicanConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000C45 RID: 3141 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x0017A4D0 File Offset: 0x001786D0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("Pemmican", STRINGS.ITEMS.FOOD.PEMMICAN.NAME, STRINGS.ITEMS.FOOD.PEMMICAN.DESC, 1f, false, Assets.GetAnim("pemmican_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.PEMMICAN);
		ComplexRecipeManager.Get().GetRecipe(PemmicanConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000956 RID: 2390
	public const string ID = "Pemmican";

	// Token: 0x04000957 RID: 2391
	public static ComplexRecipe recipe;
}
