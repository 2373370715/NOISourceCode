using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class TemporalTearConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600145C RID: 5212 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000B343B File Offset: 0x000B163B
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("TemporalTear", "TemporalTear", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<TemporalTear>();
		return gameObject;
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DE0 RID: 3552
	public const string ID = "TemporalTear";
}
