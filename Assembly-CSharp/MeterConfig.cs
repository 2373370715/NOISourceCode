using System;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class MeterConfig : IEntityConfig
{
	// Token: 0x060013E6 RID: 5094 RVA: 0x000B30DA File Offset: 0x000B12DA
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MeterConfig.ID, MeterConfig.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>();
		gameObject.AddOrGet<KBatchedAnimTracker>();
		return gameObject;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DB5 RID: 3509
	public static readonly string ID = "Meter";
}
