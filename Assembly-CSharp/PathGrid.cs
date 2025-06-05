using System;
using System.Collections.Generic;

// Token: 0x02000817 RID: 2071
public class PathGrid
{
	// Token: 0x0600247E RID: 9342 RVA: 0x000BC29B File Offset: 0x000BA49B
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.groupProber = group_prober;
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x001D6690 File Offset: 0x001D4890
	public PathGrid(int width_in_cells, int height_in_cells, bool apply_offset, NavType[] valid_nav_types)
	{
		this.applyOffset = apply_offset;
		this.widthInCells = width_in_cells;
		this.heightInCells = height_in_cells;
		this.ValidNavTypes = valid_nav_types;
		int num = 0;
		this.NavTypeTable = new int[11];
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
		DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!", null);
		this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
		this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
		this.serialNo = 0;
		this.previousSerialNo = -1;
		this.isUpdating = false;
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000BC2A4 File Offset: 0x000BA4A4
	public void OnCleanUp()
	{
		if (this.groupProber != null)
		{
			this.groupProber.ReleaseProber(this);
		}
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x000BC2BB File Offset: 0x000BA4BB
	public void ResetUpdate()
	{
		this.previousSerialNo = -1;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x001D676C File Offset: 0x001D496C
	public void BeginUpdate(int root_cell, bool isContinuation)
	{
		this.isUpdating = true;
		this.freshlyOccupiedCells.Clear();
		if (isContinuation)
		{
			return;
		}
		if (this.applyOffset)
		{
			Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
			this.rootX -= this.widthInCells / 2;
			this.rootY -= this.heightInCells / 2;
		}
		this.serialNo += 1;
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.previousSerialNo, this.serialNo);
		}
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x001D6804 File Offset: 0x001D4A04
	public void EndUpdate(bool isComplete)
	{
		this.isUpdating = false;
		if (this.groupProber != null)
		{
			this.groupProber.Occupy(this, this.serialNo, this.freshlyOccupiedCells);
		}
		if (!isComplete)
		{
			return;
		}
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.serialNo, this.serialNo);
		}
		this.previousSerialNo = this.serialNo;
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000BC2C4 File Offset: 0x000BA4C4
	private bool IsValidSerialNo(short serialNo)
	{
		return serialNo == this.serialNo || (!this.isUpdating && this.previousSerialNo != -1 && serialNo == this.previousSerialNo);
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000BC2ED File Offset: 0x000BA4ED
	public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
	{
		return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x001D6868 File Offset: 0x001D4A68
	public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
	{
		int num = this.OffsetCell(cell);
		is_cell_in_range = (-1 != num);
		if (!is_cell_in_range)
		{
			return PathGrid.InvalidCell;
		}
		PathFinder.Cell cell2 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type]];
		if (!this.IsValidSerialNo(cell2.queryId))
		{
			return PathGrid.InvalidCell;
		}
		return cell2;
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x001D68C4 File Offset: 0x001D4AC4
	public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
	{
		int num = this.OffsetCell(potential_path.cell);
		if (-1 == num)
		{
			return;
		}
		cell_data.queryId = this.serialNo;
		int num2 = this.NavTypeTable[(int)potential_path.navType];
		int num3 = num * this.ValidNavTypes.Length + num2;
		this.Cells[num3] = cell_data;
		if (potential_path.navType != NavType.Tube)
		{
			PathGrid.ProberCell proberCell = this.ProberCells[num];
			if (cell_data.queryId != proberCell.queryId || cell_data.cost < proberCell.cost)
			{
				proberCell.queryId = cell_data.queryId;
				proberCell.cost = cell_data.cost;
				this.ProberCells[num] = proberCell;
				this.freshlyOccupiedCells.Add(potential_path.cell);
			}
		}
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x001D6988 File Offset: 0x001D4B88
	public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		int num = -1;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(num2))
			{
				PathGrid.ProberCell proberCell = this.ProberCells[num2];
				if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
				{
					num = proberCell.cost;
				}
			}
		}
		return num;
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x001D69F8 File Offset: 0x001D4BF8
	public int GetCost(int cell)
	{
		int num = this.OffsetCell(cell);
		if (-1 == num)
		{
			return -1;
		}
		PathGrid.ProberCell proberCell = this.ProberCells[num];
		if (!this.IsValidSerialNo(proberCell.queryId))
		{
			return -1;
		}
		return proberCell.cost;
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x001D6A38 File Offset: 0x001D4C38
	private int OffsetCell(int cell)
	{
		if (!this.applyOffset)
		{
			return cell;
		}
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		if (num < this.rootX || num >= this.rootX + this.widthInCells || num2 < this.rootY || num2 >= this.rootY + this.heightInCells)
		{
			return -1;
		}
		int num3 = num - this.rootX;
		return (num2 - this.rootY) * this.widthInCells + num3;
	}

	// Token: 0x040018EA RID: 6378
	private PathFinder.Cell[] Cells;

	// Token: 0x040018EB RID: 6379
	private PathGrid.ProberCell[] ProberCells;

	// Token: 0x040018EC RID: 6380
	private List<int> freshlyOccupiedCells = new List<int>();

	// Token: 0x040018ED RID: 6381
	private NavType[] ValidNavTypes;

	// Token: 0x040018EE RID: 6382
	private int[] NavTypeTable;

	// Token: 0x040018EF RID: 6383
	private int widthInCells;

	// Token: 0x040018F0 RID: 6384
	private int heightInCells;

	// Token: 0x040018F1 RID: 6385
	private bool applyOffset;

	// Token: 0x040018F2 RID: 6386
	private int rootX;

	// Token: 0x040018F3 RID: 6387
	private int rootY;

	// Token: 0x040018F4 RID: 6388
	private short serialNo;

	// Token: 0x040018F5 RID: 6389
	private short previousSerialNo;

	// Token: 0x040018F6 RID: 6390
	private bool isUpdating;

	// Token: 0x040018F7 RID: 6391
	private IGroupProber groupProber;

	// Token: 0x040018F8 RID: 6392
	public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell
	{
		cost = -1
	};

	// Token: 0x02000818 RID: 2072
	private struct ProberCell
	{
		// Token: 0x040018F9 RID: 6393
		public int cost;

		// Token: 0x040018FA RID: 6394
		public short queryId;
	}
}
