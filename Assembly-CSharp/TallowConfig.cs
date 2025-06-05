using System;
using UnityEngine;

// Token: 0x020004A3 RID: 1187
public class TallowConfig : IOreConfig
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06001452 RID: 5202 RVA: 0x000B33FC File Offset: 0x000B15FC
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.Tallow;
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000B3403 File Offset: 0x000B1603
	public GameObject CreatePrefab()
	{
		return EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
	}

	// Token: 0x04000DDD RID: 3549
	public const string ID = "Tallow";

	// Token: 0x04000DDE RID: 3550
	public static readonly Tag TAG = TagManager.Create("Tallow");
}
