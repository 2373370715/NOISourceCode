using System;

// Token: 0x02000803 RID: 2051
public class NavTable
{
	// Token: 0x0600242D RID: 9261 RVA: 0x001D59A0 File Offset: 0x001D3BA0
	public NavTable(int cell_count)
	{
		this.ValidCells = new short[cell_count];
		this.NavTypeMasks = new short[11];
		for (short num = 0; num < 11; num += 1)
		{
			this.NavTypeMasks[(int)num] = (short)(1 << (int)num);
		}
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000BBEE5 File Offset: 0x000BA0E5
	public bool IsValid(int cell, NavType nav_type = NavType.Floor)
	{
		return Grid.IsValidCell(cell) && (this.NavTypeMasks[(int)nav_type] & this.ValidCells[cell]) != 0;
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x001D59EC File Offset: 0x001D3BEC
	public void SetValid(int cell, NavType nav_type, bool is_valid)
	{
		short num = this.NavTypeMasks[(int)nav_type];
		short num2 = this.ValidCells[cell];
		if ((num2 & num) != 0 != is_valid)
		{
			if (is_valid)
			{
				this.ValidCells[cell] = (num | num2);
			}
			else
			{
				this.ValidCells[cell] = (~num & num2);
			}
			if (this.OnValidCellChanged != null)
			{
				this.OnValidCellChanged(cell, nav_type);
			}
		}
	}

	// Token: 0x040018A2 RID: 6306
	public Action<int, NavType> OnValidCellChanged;

	// Token: 0x040018A3 RID: 6307
	private short[] NavTypeMasks;

	// Token: 0x040018A4 RID: 6308
	private short[] ValidCells;
}
