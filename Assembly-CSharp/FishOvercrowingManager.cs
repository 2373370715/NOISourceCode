using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001335 RID: 4917
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060064C4 RID: 25796 RVA: 0x000E6272 File Offset: 0x000E4472
	public static void DestroyInstance()
	{
		FishOvercrowingManager.Instance = null;
	}

	// Token: 0x060064C5 RID: 25797 RVA: 0x000E627A File Offset: 0x000E447A
	protected override void OnPrefabInit()
	{
		FishOvercrowingManager.Instance = this;
		this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
	}

	// Token: 0x060064C6 RID: 25798 RVA: 0x000E6292 File Offset: 0x000E4492
	public void Add(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Add(fish);
	}

	// Token: 0x060064C7 RID: 25799 RVA: 0x000E62A0 File Offset: 0x000E44A0
	public void Remove(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Remove(fish);
	}

	// Token: 0x060064C8 RID: 25800 RVA: 0x002CE9AC File Offset: 0x002CCBAC
	public void Sim1000ms(float dt)
	{
		int num = this.versionCounter;
		this.versionCounter = num + 1;
		int num2 = num;
		int num3 = 1;
		this.cavityIdToCavityInfo.Clear();
		this.cellToFishCount.Clear();
		ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.PooledList pooledList = ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.Allocate();
		foreach (FishOvercrowdingMonitor.Instance instance in this.fishes)
		{
			int num4 = Grid.PosToCell(instance);
			if (Grid.IsValidCell(num4))
			{
				FishOvercrowingManager.FishInfo item = new FishOvercrowingManager.FishInfo
				{
					cell = num4,
					fish = instance
				};
				pooledList.Add(item);
				int num5 = 0;
				this.cellToFishCount.TryGetValue(num4, out num5);
				num5++;
				this.cellToFishCount[num4] = num5;
			}
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo in pooledList)
		{
			ListPool<int, FishOvercrowingManager>.PooledList pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
			pooledList2.Add(fishInfo.cell);
			int i = 0;
			int num6 = num3++;
			while (i < pooledList2.Count)
			{
				int num7 = pooledList2[i++];
				if (Grid.IsValidCell(num7))
				{
					FishOvercrowingManager.Cell cell = this.cells[num7];
					if (cell.version != num2 && Grid.IsLiquid(num7))
					{
						cell.cavityId = num6;
						cell.version = num2;
						int num8 = 0;
						this.cellToFishCount.TryGetValue(num7, out num8);
						FishOvercrowingManager.CavityInfo value = default(FishOvercrowingManager.CavityInfo);
						if (!this.cavityIdToCavityInfo.TryGetValue(num6, out value))
						{
							value = default(FishOvercrowingManager.CavityInfo);
						}
						value.fishCount += num8;
						value.cellCount++;
						this.cavityIdToCavityInfo[num6] = value;
						pooledList2.Add(Grid.CellLeft(num7));
						pooledList2.Add(Grid.CellRight(num7));
						pooledList2.Add(Grid.CellAbove(num7));
						pooledList2.Add(Grid.CellBelow(num7));
						this.cells[num7] = cell;
					}
				}
			}
			pooledList2.Recycle();
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo2 in pooledList)
		{
			FishOvercrowingManager.Cell cell2 = this.cells[fishInfo2.cell];
			FishOvercrowingManager.CavityInfo cavityInfo = default(FishOvercrowingManager.CavityInfo);
			this.cavityIdToCavityInfo.TryGetValue(cell2.cavityId, out cavityInfo);
			fishInfo2.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
		}
		pooledList.Recycle();
	}

	// Token: 0x0400487D RID: 18557
	public static FishOvercrowingManager Instance;

	// Token: 0x0400487E RID: 18558
	private List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();

	// Token: 0x0400487F RID: 18559
	private Dictionary<int, FishOvercrowingManager.CavityInfo> cavityIdToCavityInfo = new Dictionary<int, FishOvercrowingManager.CavityInfo>();

	// Token: 0x04004880 RID: 18560
	private Dictionary<int, int> cellToFishCount = new Dictionary<int, int>();

	// Token: 0x04004881 RID: 18561
	private FishOvercrowingManager.Cell[] cells;

	// Token: 0x04004882 RID: 18562
	private int versionCounter = 1;

	// Token: 0x02001336 RID: 4918
	private struct Cell
	{
		// Token: 0x04004883 RID: 18563
		public int version;

		// Token: 0x04004884 RID: 18564
		public int cavityId;
	}

	// Token: 0x02001337 RID: 4919
	private struct FishInfo
	{
		// Token: 0x04004885 RID: 18565
		public int cell;

		// Token: 0x04004886 RID: 18566
		public FishOvercrowdingMonitor.Instance fish;
	}

	// Token: 0x02001338 RID: 4920
	private struct CavityInfo
	{
		// Token: 0x04004887 RID: 18567
		public int fishCount;

		// Token: 0x04004888 RID: 18568
		public int cellCount;
	}
}
