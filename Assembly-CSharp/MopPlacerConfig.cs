using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x0600160A RID: 5642 RVA: 0x001A1B0C File Offset: 0x0019FD0C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MopPlacerConfig.ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
		gameObject.AddTag(GameTags.NotConversationTopic);
		Moppable moppable = gameObject.AddOrGet<Moppable>();
		moppable.synchronizeAnims = false;
		moppable.amountMoppedPerTick = 20f;
		gameObject.AddOrGet<Cancellable>();
		return gameObject;
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000F23 RID: 3875
	public static string ID = "MopPlacer";

	// Token: 0x02000505 RID: 1285
	[Serializable]
	public class MopPlacerAssets
	{
		// Token: 0x04000F24 RID: 3876
		public Material material;
	}
}
