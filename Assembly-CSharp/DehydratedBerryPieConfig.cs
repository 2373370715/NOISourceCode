using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class DehydratedBerryPieConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06000BAA RID: 2986 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x001795F8 File Offset: 0x001777F8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dehydrated_food_berry_pie_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DehydratedBerryPieConfig.ID.Name, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.NAME, STRINGS.ITEMS.FOOD.BERRYPIE.DEHYDRATED.DESC, 1f, true, anim, "idle", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Polypropylene, null);
		EntityTemplates.ExtendEntityToDehydratedFoodPackage(gameObject, FOOD.FOOD_TYPES.BERRY_PIE);
		return gameObject;
	}

	// Token: 0x04000912 RID: 2322
	public static Tag ID = new Tag("DehydratedBerryPie");

	// Token: 0x04000913 RID: 2323
	public const float MASS = 1f;

	// Token: 0x04000914 RID: 2324
	public const string ANIM_FILE = "dehydrated_food_berry_pie_kanim";

	// Token: 0x04000915 RID: 2325
	public const string INITIAL_ANIM = "idle";
}
