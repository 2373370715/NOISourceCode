using System;

// Token: 0x020010A0 RID: 4256
public class CellVisibility
{
	// Token: 0x06005678 RID: 22136 RVA: 0x000DCD39 File Offset: 0x000DAF39
	public CellVisibility()
	{
		Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
	}

	// Token: 0x06005679 RID: 22137 RVA: 0x00290418 File Offset: 0x0028E618
	public bool IsVisible(int cell)
	{
		int num = Grid.CellColumn(cell);
		if (num < this.MinX || num > this.MaxX)
		{
			return false;
		}
		int num2 = Grid.CellRow(cell);
		return num2 >= this.MinY && num2 <= this.MaxY;
	}

	// Token: 0x04003D3F RID: 15679
	private int MinX;

	// Token: 0x04003D40 RID: 15680
	private int MinY;

	// Token: 0x04003D41 RID: 15681
	private int MaxX;

	// Token: 0x04003D42 RID: 15682
	private int MaxY;
}
