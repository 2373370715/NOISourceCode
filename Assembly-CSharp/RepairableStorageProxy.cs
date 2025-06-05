using System;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class RepairableStorageProxy : IEntityConfig
{
	// Token: 0x06001437 RID: 5175 RVA: 0x000B336A File Offset: 0x000B156A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(RepairableStorageProxy.ID, RepairableStorageProxy.ID, true);
		gameObject.AddOrGet<Storage>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DD4 RID: 3540
	public static string ID = "RepairableStorageProxy";
}
