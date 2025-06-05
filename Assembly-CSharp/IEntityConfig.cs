using System;
using UnityEngine;

// Token: 0x020012DA RID: 4826
public interface IEntityConfig
{
	// Token: 0x0600630E RID: 25358
	GameObject CreatePrefab();

	// Token: 0x0600630F RID: 25359
	void OnPrefabInit(GameObject inst);

	// Token: 0x06006310 RID: 25360
	void OnSpawn(GameObject inst);

	// Token: 0x06006311 RID: 25361 RVA: 0x000AA765 File Offset: 0x000A8965
	[Obsolete("Use IHasDlcRestrictions instead")]
	string[] GetDlcIds()
	{
		return null;
	}
}
