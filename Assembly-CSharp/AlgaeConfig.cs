using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
public class AlgaeConfig : IOreConfig
{
	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600157D RID: 5501 RVA: 0x000B4025 File Offset: 0x000B2225
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Algae;
		}
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000B402C File Offset: 0x000B222C
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, new List<Tag>
		{
			GameTags.Life
		});
	}
}
