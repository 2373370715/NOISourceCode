using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class EggShellConfig : IEntityConfig
{
	// Token: 0x06000ECD RID: 3789 RVA: 0x00184D68 File Offset: 0x00182F68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("EggShell", ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.NAME, ITEMS.INDUSTRIAL_PRODUCTS.EGG_SHELL.DESC, 1f, false, Assets.GetAnim("eggshells_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.9f, 0.6f, true, 0, SimHashes.Creature, null);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Organics, false);
		gameObject.AddOrGet<EntitySplitter>();
		gameObject.AddOrGet<SimpleMassStatusItem>();
		EntityTemplates.CreateAndRegisterCompostableFromPrefab(gameObject);
		return gameObject;
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AF6 RID: 2806
	public const string ID = "EggShell";

	// Token: 0x04000AF7 RID: 2807
	public static readonly Tag TAG = TagManager.Create("EggShell");

	// Token: 0x04000AF8 RID: 2808
	public const float EGG_TO_SHELL_RATIO = 0.5f;
}
