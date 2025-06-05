using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class EyeAnimation : IEntityConfig
{
	// Token: 0x06001381 RID: 4993 RVA: 0x00198A30 File Offset: 0x00196C30
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
		gameObject.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("anim_blinks_kanim")
		};
		return gameObject;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D65 RID: 3429
	public static string ID = "EyeAnimation";
}
