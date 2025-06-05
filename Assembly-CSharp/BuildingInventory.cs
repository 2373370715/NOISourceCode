using System;
using System.Collections.Generic;

// Token: 0x02000CBF RID: 3263
public class BuildingInventory : KMonoBehaviour
{
	// Token: 0x06003E39 RID: 15929 RVA: 0x000CCCAF File Offset: 0x000CAEAF
	public static void DestroyInstance()
	{
		BuildingInventory.Instance = null;
	}

	// Token: 0x06003E3A RID: 15930 RVA: 0x000CCCB7 File Offset: 0x000CAEB7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		BuildingInventory.Instance = this;
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x000CCCC5 File Offset: 0x000CAEC5
	public HashSet<BuildingComplete> GetBuildings(Tag tag)
	{
		return this.Buildings[tag];
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x000CCCD3 File Offset: 0x000CAED3
	public int BuildingCount(Tag tag)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		return this.Buildings[tag].Count;
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x00241F44 File Offset: 0x00240144
	public int BuildingCountForWorld_BAD_PERF(Tag tag, int worldId)
	{
		if (!this.Buildings.ContainsKey(tag))
		{
			return 0;
		}
		int num = 0;
		using (HashSet<BuildingComplete>.Enumerator enumerator = this.Buildings[tag].GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetMyWorldId() == worldId)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x00241FB4 File Offset: 0x002401B4
	public void RegisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			hashSet = new HashSet<BuildingComplete>();
			this.Buildings[prefabTag] = hashSet;
		}
		hashSet.Add(building);
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x00241FF8 File Offset: 0x002401F8
	public void UnregisterBuilding(BuildingComplete building)
	{
		Tag prefabTag = building.prefabid.PrefabTag;
		HashSet<BuildingComplete> hashSet;
		if (!this.Buildings.TryGetValue(prefabTag, out hashSet))
		{
			DebugUtil.DevLogError(string.Format("Unregistering building {0} before it was registered.", prefabTag));
			return;
		}
		DebugUtil.DevAssert(hashSet.Remove(building), string.Format("Building {0} was not found to be removed", prefabTag), null);
	}

	// Token: 0x04002AF1 RID: 10993
	public static BuildingInventory Instance;

	// Token: 0x04002AF2 RID: 10994
	private Dictionary<Tag, HashSet<BuildingComplete>> Buildings = new Dictionary<Tag, HashSet<BuildingComplete>>();
}
