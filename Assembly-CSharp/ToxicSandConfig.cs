using System;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class ToxicSandConfig : IOreConfig
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06001598 RID: 5528 RVA: 0x000B4081 File Offset: 0x000B2281
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.ToxicSand;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06001599 RID: 5529 RVA: 0x000B405E File Offset: 0x000B225E
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x0019F7BC File Offset: 0x0019D9BC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(2.0000001E-05f, 0.05f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
