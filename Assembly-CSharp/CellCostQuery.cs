using System;

// Token: 0x0200081C RID: 2076
public class CellCostQuery : PathFinderQuery
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600249B RID: 9371 RVA: 0x000BC42E File Offset: 0x000BA62E
	// (set) Token: 0x0600249C RID: 9372 RVA: 0x000BC436 File Offset: 0x000BA636
	public int resultCost { get; private set; }

	// Token: 0x0600249D RID: 9373 RVA: 0x000BC43F File Offset: 0x000BA63F
	public void Reset(int target_cell, int max_cost)
	{
		this.targetCell = target_cell;
		this.maxCost = max_cost;
		this.resultCost = -1;
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x000BC456 File Offset: 0x000BA656
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (cost > this.maxCost)
		{
			return true;
		}
		if (cell == this.targetCell)
		{
			this.resultCost = cost;
			return true;
		}
		return false;
	}

	// Token: 0x0400190A RID: 6410
	private int targetCell;

	// Token: 0x0400190B RID: 6411
	private int maxCost;
}
