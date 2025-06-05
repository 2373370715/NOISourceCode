using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200143F RID: 5183
public interface IGameObjectEffectDescriptor
{
	// Token: 0x06006A4E RID: 27214
	List<Descriptor> GetDescriptors(GameObject go);
}
