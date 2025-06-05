using System;

// Token: 0x02000822 RID: 2082
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	// Token: 0x060024B0 RID: 9392 RVA: 0x000BC524 File Offset: 0x000BA724
	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		this.targetCell = Grid.InvalidCell;
		this.isRotated = is_rotated;
		this.markerX = marker_x;
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x001D6EF0 File Offset: 0x001D50F0
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
		{
			this.targetCell = cell;
		}
		return this.targetCell != Grid.InvalidCell;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000BC545 File Offset: 0x000BA745
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001914 RID: 6420
	private int targetCell;

	// Token: 0x04001915 RID: 6421
	private bool isRotated;

	// Token: 0x04001916 RID: 6422
	private int markerX;
}
