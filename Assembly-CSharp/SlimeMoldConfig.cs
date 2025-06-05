using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class SlimeMoldConfig : IOreConfig
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06001594 RID: 5524 RVA: 0x000B407A File Offset: 0x000B227A
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.SlimeMold;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x000B405E File Offset: 0x000B225E
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.ContaminatedOxygen;
		}
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x0019F768 File Offset: 0x0019D968
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
		sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
