using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020004A2 RID: 1186
public class StoredMinionConfig : IEntityConfig
{
	// Token: 0x0600144D RID: 5197 RVA: 0x0019B4E0 File Offset: 0x001996E0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(StoredMinionConfig.ID, StoredMinionConfig.ID, true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.AddOrGet<KPrefabID>();
		gameObject.AddOrGet<Traits>();
		gameObject.AddOrGet<Schedulable>();
		gameObject.AddOrGet<StoredMinionIdentity>();
		gameObject.AddOrGet<KSelectable>().IsSelectable = false;
		gameObject.AddOrGet<MinionModifiers>().addBaseTraits = false;
		return gameObject;
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x0019B538 File Offset: 0x00199738
	public void OnPrefabInit(GameObject go)
	{
		GameObject prefab = Assets.GetPrefab(BionicMinionConfig.ID);
		if (prefab != null)
		{
			StoredMinionIdentity.IStoredMinionExtension[] components = prefab.GetComponents<StoredMinionIdentity.IStoredMinionExtension>();
			if (components != null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					components[i].AddStoredMinionGameObjectRequirements(go);
				}
			}
		}
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000DDC RID: 3548
	public static string ID = "StoredMinion";
}
