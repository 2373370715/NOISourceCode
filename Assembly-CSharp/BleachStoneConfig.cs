using System;
using UnityEngine;

// Token: 0x020004E1 RID: 1249
public class BleachStoneConfig : IOreConfig
{
	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06001580 RID: 5504 RVA: 0x000B4049 File Offset: 0x000B2249
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.BleachStone;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06001581 RID: 5505 RVA: 0x000B4050 File Offset: 0x000B2250
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ChlorineGas;
		}
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x0019F3A8 File Offset: 0x0019D5A8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.00020000001f, 0.0025000002f, 1.8f, 0.5f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
