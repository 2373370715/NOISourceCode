using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class AsteroidConfig : IEntityConfig
{
	// Token: 0x0600120B RID: 4619 RVA: 0x00192030 File Offset: 0x00190230
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Asteroid", "Asteroid", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<WorldInventory>();
		gameObject.AddOrGet<WorldContainer>();
		gameObject.AddOrGet<AsteroidGridEntity>();
		gameObject.AddOrGet<OrbitalMechanics>();
		gameObject.AddOrGetDef<GameplaySeasonManager.Def>();
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		return gameObject;
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000C9B RID: 3227
	public const string ID = "Asteroid";
}
