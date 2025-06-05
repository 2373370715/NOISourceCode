using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AD4 RID: 2772
[AddComponentMenu("KMonoBehaviour/scripts/NavigationReservations")]
public class NavigationReservations : KMonoBehaviour
{
	// Token: 0x060032E2 RID: 13026 RVA: 0x000C585F File Offset: 0x000C3A5F
	public static void DestroyInstance()
	{
		NavigationReservations.Instance = null;
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x000C5867 File Offset: 0x000C3A67
	public int GetOccupancyCount(int cell)
	{
		if (this.cellOccupancyDensity.ContainsKey(cell))
		{
			return this.cellOccupancyDensity[cell];
		}
		return 0;
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x00212720 File Offset: 0x00210920
	public void AddOccupancy(int cell)
	{
		if (!this.cellOccupancyDensity.ContainsKey(cell))
		{
			this.cellOccupancyDensity.Add(cell, 1);
			return;
		}
		Dictionary<int, int> dictionary = this.cellOccupancyDensity;
		dictionary[cell]++;
	}

	// Token: 0x060032E5 RID: 13029 RVA: 0x00212764 File Offset: 0x00210964
	public void RemoveOccupancy(int cell)
	{
		int num = 0;
		if (this.cellOccupancyDensity.TryGetValue(cell, out num))
		{
			if (num == 1)
			{
				this.cellOccupancyDensity.Remove(cell);
				return;
			}
			this.cellOccupancyDensity[cell] = num - 1;
		}
	}

	// Token: 0x060032E6 RID: 13030 RVA: 0x000C5885 File Offset: 0x000C3A85
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NavigationReservations.Instance = this;
	}

	// Token: 0x040022C2 RID: 8898
	public static NavigationReservations Instance;

	// Token: 0x040022C3 RID: 8899
	public static int InvalidReservation = -1;

	// Token: 0x040022C4 RID: 8900
	private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
}
