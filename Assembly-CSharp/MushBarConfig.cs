using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class MushBarConfig : IEntityConfig
{
	// Token: 0x06000C26 RID: 3110 RVA: 0x0017A0E0 File Offset: 0x001782E0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("MushBar", STRINGS.ITEMS.FOOD.MUSHBAR.NAME, STRINGS.ITEMS.FOOD.MUSHBAR.DESC, 1f, false, Assets.GetAnim("mushbar_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.4f, true, 0, SimHashes.Creature, null);
		gameObject = EntityTemplates.ExtendEntityToFood(gameObject, FOOD.FOOD_TYPES.MUSHBAR);
		ComplexRecipeManager.Get().GetRecipe(MushBarConfig.recipe.id).FabricationVisualizer = MushBarConfig.CreateFabricationVisualizer(gameObject);
		return gameObject;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0017A168 File Offset: 0x00178368
	public static GameObject CreateFabricationVisualizer(GameObject result)
	{
		KBatchedAnimController component = result.GetComponent<KBatchedAnimController>();
		GameObject gameObject = new GameObject();
		gameObject.name = result.name + "Visualizer";
		gameObject.SetActive(false);
		gameObject.transform.SetLocalPosition(Vector3.zero);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = component.AnimFiles;
		kbatchedAnimController.initialAnim = "fabricating";
		kbatchedAnimController.isMovable = true;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = new HashedString("meter_ration");
		kbatchedAnimTracker.offset = Vector3.zero;
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		return gameObject;
	}

	// Token: 0x04000947 RID: 2375
	public const string ID = "MushBar";

	// Token: 0x04000948 RID: 2376
	public static ComplexRecipe recipe;
}
