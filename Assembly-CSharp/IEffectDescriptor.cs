using System;
using System.Collections.Generic;

// Token: 0x02001440 RID: 5184
[Obsolete("No longer used. Use IGameObjectEffectDescriptor instead", false)]
public interface IEffectDescriptor
{
	// Token: 0x06006A4F RID: 27215
	List<Descriptor> GetDescriptors(BuildingDef def);
}
