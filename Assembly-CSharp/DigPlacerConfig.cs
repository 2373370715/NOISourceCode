using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig
{
	// Token: 0x06001604 RID: 5636 RVA: 0x001A1A64 File Offset: 0x0019FC64
	public GameObject CreatePrefab()
	{
		GameObject gameObject = base.CreatePrefab(DigPlacerConfig.ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);
		Diggable diggable = gameObject.AddOrGet<Diggable>();
		diggable.workTime = 5f;
		diggable.synchronizeAnims = false;
		diggable.workAnims = new HashedString[]
		{
			"place",
			"release"
		};
		diggable.materials = Assets.instance.digPlacerAssets.materials;
		diggable.materialDisplay = gameObject.GetComponentInChildren<MeshRenderer>(true);
		gameObject.AddOrGet<CancellableDig>();
		return gameObject;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000F21 RID: 3873
	public static string ID = "DigPlacer";

	// Token: 0x02000503 RID: 1283
	[Serializable]
	public class DigPlacerAssets
	{
		// Token: 0x04000F22 RID: 3874
		public Material[] materials;
	}
}
