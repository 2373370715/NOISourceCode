using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class OrbitalBGConfig : IEntityConfig
{
	// Token: 0x06001426 RID: 5158 RVA: 0x000B3324 File Offset: 0x000B1524
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OrbitalBGConfig.ID, OrbitalBGConfig.ID, false);
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<OrbitalObject>();
		gameObject.AddOrGet<SaveLoadRoot>();
		return gameObject;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DD1 RID: 3537
	public static string ID = "OrbitalBG";
}
