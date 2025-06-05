using System;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class ApproachableLocator : IEntityConfig
{
	// Token: 0x060013C8 RID: 5064 RVA: 0x000B300B File Offset: 0x000B120B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(ApproachableLocator.ID, ApproachableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		return gameObject;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D95 RID: 3477
	public static readonly string ID = "ApproachableLocator";
}
