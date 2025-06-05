using System;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class ResearchDestinationConfig : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x0600143C RID: 5180 RVA: 0x000AA117 File Offset: 0x000A8317
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000B339A File Offset: 0x000B159A
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("ResearchDestination", "ResearchDestination", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<ResearchDestination>();
		return gameObject;
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000DD5 RID: 3541
	public const string ID = "ResearchDestination";
}
