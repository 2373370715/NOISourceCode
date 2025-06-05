using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class MovePickupablePlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001610 RID: 5648 RVA: 0x001A1B68 File Offset: 0x0019FD68
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(MovePickupablePlacerConfig.ID, MISC.PLACERS.MOVEPICKUPABLEPLACER.NAME, Assets.instance.movePickupToPlacerAssets.material);
		gameObject.AddOrGet<CancellableMove>();
		Storage storage = gameObject.AddOrGet<Storage>();
		storage.showInUI = false;
		storage.showUnreachableStatus = true;
		gameObject.AddOrGet<Approachable>();
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddTag(GameTags.NotConversationTopic);
		return gameObject;
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000F25 RID: 3877
	public static string ID = "MovePickupablePlacer";

	// Token: 0x02000507 RID: 1287
	[Serializable]
	public class MovePickupablePlacerAssets
	{
		// Token: 0x04000F26 RID: 3878
		public Material material;
	}
}
