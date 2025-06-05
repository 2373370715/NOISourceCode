using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class MinionAssignablesProxyConfig : IEntityConfig
{
	// Token: 0x060013F0 RID: 5104 RVA: 0x000B3112 File Offset: 0x000B1312
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MinionAssignablesProxyConfig.ID, MinionAssignablesProxyConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<Ownables>();
		gameObject.AddOrGet<Equipment>();
		gameObject.AddOrGet<MinionAssignablesProxy>();
		return gameObject;
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DB9 RID: 3513
	public static string ID = "MinionAssignablesProxy";
}
