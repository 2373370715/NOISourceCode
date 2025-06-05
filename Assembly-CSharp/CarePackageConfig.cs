using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class CarePackageConfig : IEntityConfig
{
	// Token: 0x060012CC RID: 4812 RVA: 0x00196A44 File Offset: 0x00194C44
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, ITEMS.CARGO_CAPSULE.NAME, ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim("portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, false, 0, SimHashes.Creature, null);
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x000B2DC0 File Offset: 0x000B0FC0
	public void OnPrefabInit(GameObject go)
	{
		go.AddOrGet<CarePackage>();
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D33 RID: 3379
	public static readonly string ID = "CarePackage";
}
