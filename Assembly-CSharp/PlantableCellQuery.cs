using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000824 RID: 2084
public class PlantableCellQuery : PathFinderQuery
{
	// Token: 0x060024B8 RID: 9400 RVA: 0x000BC5A4 File Offset: 0x000BA7A4
	public PlantableCellQuery Reset(PlantableSeed seed, int max_results)
	{
		this.seed = seed;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x001D7020 File Offset: 0x001D5220
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlotCell(this.seed, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x001D706C File Offset: 0x001D526C
	private bool CheckValidPlotCell(PlantableSeed seed, int plant_cell)
	{
		if (!Grid.IsValidCell(plant_cell))
		{
			return false;
		}
		int num;
		if (seed.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(plant_cell);
		}
		else
		{
			num = Grid.CellBelow(plant_cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (!Grid.Solid[num])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 5])
		{
			return false;
		}
		if (Grid.Objects[plant_cell, 1])
		{
			return false;
		}
		GameObject gameObject = Grid.Objects[num, 1];
		if (gameObject)
		{
			PlantablePlot component = gameObject.GetComponent<PlantablePlot>();
			if (component == null)
			{
				return false;
			}
			if (component.Direction != seed.Direction)
			{
				return false;
			}
			if (component.Occupant != null)
			{
				return false;
			}
		}
		else
		{
			if (!seed.TestSuitableGround(plant_cell))
			{
				return false;
			}
			if (PlantableCellQuery.CountNearbyPlants(num, this.plantDetectionRadius) > this.maxPlantsInRadius)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x001D7148 File Offset: 0x001D5348
	private static int CountNearbyPlants(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(Grid.CellToPos(cell), out num, out num2);
		int num3 = radius * 2;
		num -= radius;
		num2 -= radius;
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(num, num2, num3, num3, GameScenePartitioner.Instance.plants, pooledList);
		int num4 = 0;
		using (List<ScenePartitionerEntry>.Enumerator enumerator = pooledList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!((KPrefabID)enumerator.Current.obj).GetComponent<TreeBud>())
				{
					num4++;
				}
			}
		}
		pooledList.Recycle();
		return num4;
	}

	// Token: 0x0400191B RID: 6427
	public List<int> result_cells = new List<int>();

	// Token: 0x0400191C RID: 6428
	private PlantableSeed seed;

	// Token: 0x0400191D RID: 6429
	private int max_results;

	// Token: 0x0400191E RID: 6430
	private int plantDetectionRadius = 6;

	// Token: 0x0400191F RID: 6431
	private int maxPlantsInRadius = 2;
}
