using System;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class SleepLocator : IEntityConfig
{
	// Token: 0x060013CD RID: 5069 RVA: 0x000B303B File Offset: 0x000B123B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(SleepLocator.ID, SleepLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Sleepable>().isNormalBed = false;
		return gameObject;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D96 RID: 3478
	public static readonly string ID = "SleepLocator";
}
