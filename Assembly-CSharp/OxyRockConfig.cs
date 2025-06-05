using System;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class OxyRockConfig : IOreConfig
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06001590 RID: 5520 RVA: 0x000B406C File Offset: 0x000B226C
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.OxyRock;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06001591 RID: 5521 RVA: 0x000B4073 File Offset: 0x000B2273
	public SimHashes SublimeElementID
	{
		get
		{
			return SimHashes.Oxygen;
		}
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x0019F714 File Offset: 0x0019D914
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
		sublimates.info = new Sublimates.Info(0.010000001f, 0.0050000004f, 1.8f, 0.7f, this.SublimeElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
