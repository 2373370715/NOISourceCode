using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class RotPileConfig : IEntityConfig
{
	// Token: 0x06000C6B RID: 3179 RVA: 0x0017A8C8 File Offset: 0x00178AC8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity(RotPileConfig.ID, STRINGS.ITEMS.FOOD.ROTPILE.NAME, STRINGS.ITEMS.FOOD.ROTPILE.DESC, 1f, false, Assets.GetAnim("rotfood_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Organics, false);
		component.AddTag(GameTags.Compostable, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<OccupyArea>();
		gameObject.AddOrGet<Modifiers>();
		gameObject.AddOrGet<RotPile>();
		gameObject.AddComponent<DecorProvider>().SetValues(DECOR.PENALTY.TIER2);
		return gameObject;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x000AFC90 File Offset: 0x000ADE90
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<DecorProvider>().overrideName = STRINGS.ITEMS.FOOD.ROTPILE.NAME;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000966 RID: 2406
	public static string ID = "RotPile";
}
