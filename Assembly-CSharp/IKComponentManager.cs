using System;
using UnityEngine;

// Token: 0x02000CEF RID: 3311
public interface IKComponentManager
{
	// Token: 0x06003F7E RID: 16254
	HandleVector<int>.Handle Add(GameObject go);

	// Token: 0x06003F7F RID: 16255
	void Remove(GameObject go);
}
