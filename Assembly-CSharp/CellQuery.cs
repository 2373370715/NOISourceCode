using System;

// Token: 0x0200081E RID: 2078
public class CellQuery : PathFinderQuery
{
	// Token: 0x060024A2 RID: 9378 RVA: 0x000BC47E File Offset: 0x000BA67E
	public CellQuery Reset(int target_cell)
	{
		this.targetCell = target_cell;
		return this;
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000BC488 File Offset: 0x000BA688
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		return cell == this.targetCell;
	}

	// Token: 0x0400190D RID: 6413
	private int targetCell;
}
