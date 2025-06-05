using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class MouthAnimation : IEntityConfig
{
	// Token: 0x06001421 RID: 5153 RVA: 0x0019B138 File Offset: 0x00199338
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(MouthAnimation.ID, MouthAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_mouth_flap_kanim")
		};
		return gameObject;
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DD0 RID: 3536
	public static string ID = "MouthAnimation";
}
