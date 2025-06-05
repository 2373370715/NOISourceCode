using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class MachinePartsConfig : IEntityConfig
{
	// Token: 0x06000ED7 RID: 3799 RVA: 0x00184E58 File Offset: 0x00183058
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity("MachineParts", ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim("buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true, 0, SimHashes.Creature, null);
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000AFC RID: 2812
	public const string ID = "MachineParts";

	// Token: 0x04000AFD RID: 2813
	public static readonly Tag TAG = TagManager.Create("MachineParts");

	// Token: 0x04000AFE RID: 2814
	public const float MASS = 5f;
}
