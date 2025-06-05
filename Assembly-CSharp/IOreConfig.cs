using System;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public interface IOreConfig
{
	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06001588 RID: 5512
	SimHashes ElementID { get; }

	// Token: 0x06001589 RID: 5513
	GameObject CreatePrefab();
}
