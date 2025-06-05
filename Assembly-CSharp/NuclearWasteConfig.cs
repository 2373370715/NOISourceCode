using System;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
public class NuclearWasteConfig : IOreConfig
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600158D RID: 5517 RVA: 0x000B4065 File Offset: 0x000B2265
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.NuclearWaste;
		}
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x0019F6B8 File Offset: 0x0019D8B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLiquidOreEntity(this.ElementID, null);
		Sublimates sublimates = gameObject.AddOrGet<Sublimates>();
		sublimates.decayStorage = true;
		sublimates.spawnFXHash = SpawnFXHashes.NuclearWasteDrip;
		sublimates.info = new Sublimates.Info(0.066f, 6.6f, 1000f, 0f, this.ElementID, byte.MaxValue, 0);
		return gameObject;
	}
}
