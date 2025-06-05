using System;
using UnityEngine;

// Token: 0x02000489 RID: 1161
public class TargetLocator : IEntityConfig
{
	// Token: 0x060013C3 RID: 5059 RVA: 0x000B2FE2 File Offset: 0x000B11E2
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(TargetLocator.ID, TargetLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D94 RID: 3476
	public static readonly string ID = "TargetLocator";
}
