using System;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
public class DirtyWaterConfig : IOreConfig
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x000B4057 File Offset: 0x000B2257
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.DirtyWater;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06001585 RID: 5509 RVA: 0x000B405E File Offset: 0x000B225E
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x0019F3FC File Offset: 0x0019D5FC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubbleWater;
		sublimates.info = new Sublimates.Info(4.0000006E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
