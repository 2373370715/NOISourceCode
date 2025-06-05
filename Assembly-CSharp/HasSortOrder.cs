using System;
using UnityEngine;

// Token: 0x02001D54 RID: 7508
[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x06009CBB RID: 40123 RVA: 0x0010A6B0 File Offset: 0x001088B0
	// (set) Token: 0x06009CBC RID: 40124 RVA: 0x0010A6B8 File Offset: 0x001088B8
	public int sortOrder { get; set; }
}
