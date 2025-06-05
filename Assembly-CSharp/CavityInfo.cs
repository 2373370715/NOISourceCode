using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001853 RID: 6227
public class CavityInfo
{
	// Token: 0x06008063 RID: 32867 RVA: 0x00340C0C File Offset: 0x0033EE0C
	public CavityInfo()
	{
		this.handle = HandleVector<int>.InvalidHandle;
		this.dirty = true;
	}

	// Token: 0x06008064 RID: 32868 RVA: 0x000F9149 File Offset: 0x000F7349
	public void AddBuilding(KPrefabID bc)
	{
		this.buildings.Add(bc);
		this.dirty = true;
	}

	// Token: 0x06008065 RID: 32869 RVA: 0x000F915E File Offset: 0x000F735E
	public void AddPlants(KPrefabID plant)
	{
		this.plants.Add(plant);
		this.dirty = true;
	}

	// Token: 0x06008066 RID: 32870 RVA: 0x00340C60 File Offset: 0x0033EE60
	public void RemoveFromCavity(KPrefabID id, List<KPrefabID> listToRemove)
	{
		int num = -1;
		for (int i = 0; i < listToRemove.Count; i++)
		{
			if (id.InstanceID == listToRemove[i].InstanceID)
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			listToRemove.RemoveAt(num);
		}
	}

	// Token: 0x06008067 RID: 32871 RVA: 0x00340CA4 File Offset: 0x0033EEA4
	public void OnEnter(object data)
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(-832141045, data);
			}
		}
	}

	// Token: 0x06008068 RID: 32872 RVA: 0x000F9173 File Offset: 0x000F7373
	public Vector3 GetCenter()
	{
		return new Vector3((float)(this.minX + (this.maxX - this.minX) / 2), (float)(this.minY + (this.maxY - this.minY) / 2));
	}

	// Token: 0x040061A3 RID: 24995
	public HandleVector<int>.Handle handle;

	// Token: 0x040061A4 RID: 24996
	public bool dirty;

	// Token: 0x040061A5 RID: 24997
	public int numCells;

	// Token: 0x040061A6 RID: 24998
	public int maxX;

	// Token: 0x040061A7 RID: 24999
	public int maxY;

	// Token: 0x040061A8 RID: 25000
	public int minX;

	// Token: 0x040061A9 RID: 25001
	public int minY;

	// Token: 0x040061AA RID: 25002
	public Room room;

	// Token: 0x040061AB RID: 25003
	public List<KPrefabID> buildings = new List<KPrefabID>();

	// Token: 0x040061AC RID: 25004
	public List<KPrefabID> plants = new List<KPrefabID>();

	// Token: 0x040061AD RID: 25005
	public List<KPrefabID> creatures = new List<KPrefabID>();

	// Token: 0x040061AE RID: 25006
	public List<KPrefabID> eggs = new List<KPrefabID>();
}
