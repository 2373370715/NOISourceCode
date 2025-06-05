using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012DB RID: 4827
public interface IMultiEntityConfig
{
	// Token: 0x06006312 RID: 25362
	List<GameObject> CreatePrefabs();

	// Token: 0x06006313 RID: 25363
	void OnPrefabInit(GameObject inst);

	// Token: 0x06006314 RID: 25364
	void OnSpawn(GameObject inst);
}
