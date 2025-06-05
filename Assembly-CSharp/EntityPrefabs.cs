using System;
using UnityEngine;

// Token: 0x020012E2 RID: 4834
[AddComponentMenu("KMonoBehaviour/scripts/EntityPrefabs")]
public class EntityPrefabs : KMonoBehaviour
{
	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06006329 RID: 25385 RVA: 0x000E51E3 File Offset: 0x000E33E3
	// (set) Token: 0x0600632A RID: 25386 RVA: 0x000E51EA File Offset: 0x000E33EA
	public static EntityPrefabs Instance { get; private set; }

	// Token: 0x0600632B RID: 25387 RVA: 0x000E51F2 File Offset: 0x000E33F2
	public static void DestroyInstance()
	{
		EntityPrefabs.Instance = null;
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x000E51FA File Offset: 0x000E33FA
	protected override void OnPrefabInit()
	{
		EntityPrefabs.Instance = this;
	}

	// Token: 0x04004716 RID: 18198
	public GameObject SelectMarker;

	// Token: 0x04004717 RID: 18199
	public GameObject ForegroundLayer;
}
