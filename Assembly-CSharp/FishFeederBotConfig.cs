using System;
using UnityEngine;

// Token: 0x02000282 RID: 642
public class FishFeederBotConfig : IEntityConfig
{
	// Token: 0x06000957 RID: 2391 RVA: 0x0016EDD8 File Offset: 0x0016CFD8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("FishFeederBot", "FishFeederBot", true);
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("fishfeeder_kanim")
		};
		kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
		SymbolOverrideControllerUtil.AddToPrefab(kbatchedAnimController.gameObject);
		return gameObject;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000733 RID: 1843
	public const string ID = "FishFeederBot";
}
