using System;
using UnityEngine;

// Token: 0x0200039F RID: 927
public class WoodLogConfig : IOreConfig
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000EF6 RID: 3830 RVA: 0x000B0DB3 File Offset: 0x000AEFB3
	public SimHashes ElementID
	{
		get
		{
			return SimHashes.WoodLog;
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x001850B8 File Offset: 0x001832B8
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateSolidOreEntity(this.ElementID, null);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.prefabInitFn += this.OnInit;
		component.prefabSpawnFn += this.OnSpawn;
		component.RemoveTag(GameTags.HideFromSpawnTool);
		return gameObject;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x000B0DBA File Offset: 0x000AEFBA
	public void OnInit(GameObject inst)
	{
		PrimaryElement component = inst.GetComponent<PrimaryElement>();
		component.SetElement(this.ElementID, true);
		Element element = component.Element;
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x000B0DD5 File Offset: 0x000AEFD5
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<PrimaryElement>().SetElement(this.ElementID, true);
	}

	// Token: 0x04000B09 RID: 2825
	public const string ID = "WoodLog";

	// Token: 0x04000B0A RID: 2826
	public const float C02MassEmissionWhenBurned = 0.142f;

	// Token: 0x04000B0B RID: 2827
	public const float HeatWhenBurned = 7500f;

	// Token: 0x04000B0C RID: 2828
	public const float EnergyWhenBurned = 250f;

	// Token: 0x04000B0D RID: 2829
	public static readonly Tag TAG = TagManager.Create("WoodLog");
}
