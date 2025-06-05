using System;

// Token: 0x0200081B RID: 2075
public class CellArrayQuery : PathFinderQuery
{
	// Token: 0x06002498 RID: 9368 RVA: 0x000BC41C File Offset: 0x000BA61C
	public CellArrayQuery Reset(int[] target_cells)
	{
		this.targetCells = target_cells;
		return this;
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x001D6D24 File Offset: 0x001D4F24
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		for (int i = 0; i < this.targetCells.Length; i++)
		{
			if (this.targetCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001909 RID: 6409
	private int[] targetCells;
}
