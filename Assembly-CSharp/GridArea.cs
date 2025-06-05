using System;
using UnityEngine;

// Token: 0x0200140E RID: 5134
public struct GridArea
{
	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x060068F9 RID: 26873 RVA: 0x000E932E File Offset: 0x000E752E
	public Vector2I Min
	{
		get
		{
			return this.min;
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x060068FA RID: 26874 RVA: 0x000E9336 File Offset: 0x000E7536
	public Vector2I Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x060068FB RID: 26875 RVA: 0x002E7C58 File Offset: 0x002E5E58
	public void SetArea(int cell, int width, int height)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x + width, vector2I.y + height);
		this.SetExtents(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y);
	}

	// Token: 0x060068FC RID: 26876 RVA: 0x002E7CA4 File Offset: 0x002E5EA4
	public void SetExtents(int min_x, int min_y, int max_x, int max_y)
	{
		this.min.x = Math.Max(min_x, 0);
		this.min.y = Math.Max(min_y, 0);
		this.max.x = Math.Min(max_x, Grid.WidthInCells);
		this.max.y = Math.Min(max_y, Grid.HeightInCells);
		this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
		this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
	}

	// Token: 0x060068FD RID: 26877 RVA: 0x002E7D44 File Offset: 0x002E5F44
	public bool Contains(int cell)
	{
		if (cell >= this.MinCell && cell < this.MaxCell)
		{
			int num = cell % Grid.WidthInCells;
			return num >= this.Min.x && num < this.Max.x;
		}
		return false;
	}

	// Token: 0x060068FE RID: 26878 RVA: 0x000E933E File Offset: 0x000E753E
	public bool Contains(int x, int y)
	{
		return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
	}

	// Token: 0x060068FF RID: 26879 RVA: 0x002E7D8C File Offset: 0x002E5F8C
	public bool Contains(Vector3 pos)
	{
		return (float)this.min.x <= pos.x && pos.x < (float)this.max.x && (float)this.min.y <= pos.y && pos.y <= (float)this.max.y;
	}

	// Token: 0x06006900 RID: 26880 RVA: 0x000E937A File Offset: 0x000E757A
	public void RunIfInside(int cell, Action<int> action)
	{
		if (this.Contains(cell))
		{
			action(cell);
		}
	}

	// Token: 0x06006901 RID: 26881 RVA: 0x002E7DF0 File Offset: 0x002E5FF0
	public void Run(Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				int obj = Grid.XYToCell(j, i);
				action(obj);
			}
		}
	}

	// Token: 0x06006902 RID: 26882 RVA: 0x002E7E4C File Offset: 0x002E604C
	public void RunOnDifference(GridArea subtract_area, Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				if (!subtract_area.Contains(j, i))
				{
					int obj = Grid.XYToCell(j, i);
					action(obj);
				}
			}
		}
	}

	// Token: 0x06006903 RID: 26883 RVA: 0x000E938C File Offset: 0x000E758C
	public int GetCellCount()
	{
		return (this.max.x - this.min.x) * (this.max.y - this.min.y);
	}

	// Token: 0x04004FB1 RID: 20401
	private Vector2I min;

	// Token: 0x04004FB2 RID: 20402
	private Vector2I max;

	// Token: 0x04004FB3 RID: 20403
	private int MinCell;

	// Token: 0x04004FB4 RID: 20404
	private int MaxCell;
}
