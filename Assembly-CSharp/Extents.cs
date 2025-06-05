using System;
using UnityEngine;

// Token: 0x0200188C RID: 6284
public struct Extents
{
	// Token: 0x060081B1 RID: 33201 RVA: 0x00347204 File Offset: 0x00345404
	public static Extents OneCell(int cell)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return new Extents(num, num2, 1, 1);
	}

	// Token: 0x060081B2 RID: 33202 RVA: 0x000F9D1E File Offset: 0x000F7F1E
	public Extents(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x060081B3 RID: 33203 RVA: 0x00347224 File Offset: 0x00345424
	public Extents(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.x = num - radius;
		this.y = num2 - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x060081B4 RID: 33204 RVA: 0x000F9D3D File Offset: 0x000F7F3D
	public Extents(int center_x, int center_y, int radius)
	{
		this.x = center_x - radius;
		this.y = center_y - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x060081B5 RID: 33205 RVA: 0x00347268 File Offset: 0x00345468
	public Extents(int cell, CellOffset[] offsets)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, offset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x060081B6 RID: 33206 RVA: 0x00347304 File Offset: 0x00345504
	public Extents(int cell, CellOffset[] offsets, Extents.BoundExtendsToGridFlag _)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			int cell2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(cell2))
			{
				Grid.CellToXY(cell2, out val, out val2);
				num = Math.Min(num, val);
				num2 = Math.Min(num2, val2);
				num3 = Math.Max(num3, val);
				num4 = Math.Max(num4, val2);
			}
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x060081B7 RID: 33207 RVA: 0x003473AC File Offset: 0x003455AC
	public Extents(int cell, CellOffset[] offsets, Orientation orientation)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		for (int i = 0; i < offsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offsets[i], orientation);
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x060081B8 RID: 33208 RVA: 0x0034744C File Offset: 0x0034564C
	public Extents(int cell, CellOffset[][] offset_table)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset[] array in offset_table)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, array[0]), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x060081B9 RID: 33209 RVA: 0x003474E8 File Offset: 0x003456E8
	public bool Contains(Vector2I pos)
	{
		return this.x <= pos.x && pos.x < this.x + this.width && this.y <= pos.y && pos.y < this.y + this.height;
	}

	// Token: 0x060081BA RID: 33210 RVA: 0x00347540 File Offset: 0x00345740
	public bool Contains(Vector3 pos)
	{
		return (float)this.x <= pos.x && pos.x < (float)(this.x + this.width) && (float)this.y <= pos.y && pos.y < (float)(this.y + this.height);
	}

	// Token: 0x040062A4 RID: 25252
	public int x;

	// Token: 0x040062A5 RID: 25253
	public int y;

	// Token: 0x040062A6 RID: 25254
	public int width;

	// Token: 0x040062A7 RID: 25255
	public int height;

	// Token: 0x040062A8 RID: 25256
	public static Extents.BoundExtendsToGridFlag BoundsCheckCoords;

	// Token: 0x0200188D RID: 6285
	public struct BoundExtendsToGridFlag
	{
	}
}
