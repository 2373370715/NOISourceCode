using System;
using System.Collections.Generic;

// Token: 0x02000823 RID: 2083
public class MineableCellQuery : PathFinderQuery
{
	// Token: 0x060024B3 RID: 9395 RVA: 0x000BC54D File Offset: 0x000BA74D
	public MineableCellQuery Reset(Tag element, int max_results)
	{
		this.element = element;
		this.max_results = max_results;
		this.result_cells.Clear();
		return this;
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x001D6F3C File Offset: 0x001D513C
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidMineCell(this.element, cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x001D6F88 File Offset: 0x001D5188
	private bool CheckValidMineCell(Tag element, int testCell)
	{
		if (!Grid.IsValidCell(testCell))
		{
			return false;
		}
		foreach (Direction d in MineableCellQuery.DIRECTION_CHECKS)
		{
			int cellInDirection = Grid.GetCellInDirection(testCell, d);
			if (Grid.IsValidCell(cellInDirection) && Grid.IsSolidCell(cellInDirection) && !Grid.Foundation[cellInDirection] && Grid.Element[cellInDirection].tag == element)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001917 RID: 6423
	public List<int> result_cells = new List<int>();

	// Token: 0x04001918 RID: 6424
	private Tag element;

	// Token: 0x04001919 RID: 6425
	private int max_results;

	// Token: 0x0400191A RID: 6426
	public static List<Direction> DIRECTION_CHECKS = new List<Direction>
	{
		Direction.Down,
		Direction.Right,
		Direction.Left,
		Direction.Up
	};
}
