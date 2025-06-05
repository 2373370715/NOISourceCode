using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class SimpleFXConfig : IEntityConfig
{
	// Token: 0x06001448 RID: 5192 RVA: 0x000B33CB File Offset: 0x000B15CB
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SimpleFXConfig.ID, SimpleFXConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		return gameObject;
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DDB RID: 3547
	public static readonly string ID = "SimpleFX";
}
