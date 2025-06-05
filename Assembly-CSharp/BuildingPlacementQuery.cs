using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class BuildingPlacementQuery : PathFinderQuery
{
	// Token: 0x06002494 RID: 9364 RVA: 0x000BC3A1 File Offset: 0x000BA5A1
	public BuildingPlacementQuery Reset(int max_results, GameObject toPlace)
	{
		this.max_results = max_results;
		this.toPlace = toPlace;
		this.cellOffsets = toPlace.GetComponent<OccupyArea>().OccupiedCellsOffsets;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x06002495 RID: 9365 RVA: 0x000BC3CE File Offset: 0x000BA5CE
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidPlaceCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x001D6C44 File Offset: 0x001D4E44
	private bool CheckValidPlaceCell(int testCell)
	{
		if (!Grid.IsValidCell(testCell) || Grid.IsSolidCell(testCell) || Grid.ObjectLayers[1].ContainsKey(testCell))
		{
			return false;
		}
		bool flag = true;
		int widthInCells = this.toPlace.GetComponent<OccupyArea>().GetWidthInCells();
		int cell = testCell;
		for (int i = 0; i < widthInCells; i++)
		{
			int cellInDirection = Grid.GetCellInDirection(cell, Direction.Down);
			if (!Grid.IsValidCell(cellInDirection) || !Grid.IsSolidCell(cellInDirection))
			{
				flag = false;
				break;
			}
			cell = Grid.GetCellInDirection(cell, Direction.Right);
		}
		if (flag)
		{
			for (int j = 0; j < this.cellOffsets.Length; j++)
			{
				CellOffset offset = this.cellOffsets[j];
				int num = Grid.OffsetCell(testCell, offset);
				if (!Grid.IsValidCell(num) || Grid.IsSolidCell(num) || !Grid.IsValidBuildingCell(num) || Grid.ObjectLayers[1].ContainsKey(num))
				{
					flag = false;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x04001905 RID: 6405
	public List<int> result_cells = new List<int>();

	// Token: 0x04001906 RID: 6406
	private int max_results;

	// Token: 0x04001907 RID: 6407
	private GameObject toPlace;

	// Token: 0x04001908 RID: 6408
	private CellOffset[] cellOffsets;
}
