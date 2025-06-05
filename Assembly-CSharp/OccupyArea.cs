using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020016C6 RID: 5830
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour
{
	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x0600783E RID: 30782 RVA: 0x000F3964 File Offset: 0x000F1B64
	public CellOffset[] OccupiedCellsOffsets
	{
		get
		{
			this.UpdateRotatedCells();
			return this._RotatedOccupiedCellsOffsets;
		}
	}

	// Token: 0x17000792 RID: 1938
	// (get) Token: 0x0600783F RID: 30783 RVA: 0x000F3972 File Offset: 0x000F1B72
	// (set) Token: 0x06007840 RID: 30784 RVA: 0x000F397A File Offset: 0x000F1B7A
	public bool ApplyToCells
	{
		get
		{
			return this.applyToCells;
		}
		set
		{
			if (value != this.applyToCells)
			{
				if (value)
				{
					this.UpdateOccupiedArea();
				}
				else
				{
					this.ClearOccupiedArea();
				}
				this.applyToCells = value;
			}
		}
	}

	// Token: 0x06007841 RID: 30785 RVA: 0x000F399D File Offset: 0x000F1B9D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.applyToCells)
		{
			this.UpdateOccupiedArea();
		}
	}

	// Token: 0x06007842 RID: 30786 RVA: 0x000F39B3 File Offset: 0x000F1BB3
	private void ValidatePosition()
	{
		if (!Grid.IsValidCell(Grid.PosToCell(this)))
		{
			global::Debug.LogWarning(base.name + " is outside the grid! DELETING!");
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06007843 RID: 30787 RVA: 0x000F39E2 File Offset: 0x000F1BE2
	[OnSerializing]
	private void OnSerializing()
	{
		this.ValidatePosition();
	}

	// Token: 0x06007844 RID: 30788 RVA: 0x000F39E2 File Offset: 0x000F1BE2
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ValidatePosition();
	}

	// Token: 0x06007845 RID: 30789 RVA: 0x0031D8CC File Offset: 0x0031BACC
	public int GetOffsetCellWithRotation(CellOffset cellOffset)
	{
		CellOffset offset = cellOffset;
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedCellOffset(cellOffset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset);
	}

	// Token: 0x06007846 RID: 30790 RVA: 0x000F39EA File Offset: 0x000F1BEA
	public void SetCellOffsets(CellOffset[] cells)
	{
		this._UnrotatedOccupiedCellsOffsets = cells;
		this._RotatedOccupiedCellsOffsets = cells;
		this.UpdateRotatedCells();
	}

	// Token: 0x06007847 RID: 30791 RVA: 0x0031D908 File Offset: 0x0031BB08
	private void UpdateRotatedCells()
	{
		if (this.rotatable != null && this.appliedOrientation != this.rotatable.Orientation)
		{
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int i = 0; i < this._UnrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset offset = this._UnrotatedOccupiedCellsOffsets[i];
				this._RotatedOccupiedCellsOffsets[i] = this.rotatable.GetRotatedCellOffset(offset);
			}
			this.appliedOrientation = this.rotatable.Orientation;
		}
	}

	// Token: 0x06007848 RID: 30792 RVA: 0x0031D994 File Offset: 0x0031BB94
	public bool CheckIsOccupying(int checkCell)
	{
		int num = Grid.PosToCell(base.gameObject);
		if (checkCell == num)
		{
			return true;
		}
		foreach (CellOffset offset in this.OccupiedCellsOffsets)
		{
			if (Grid.OffsetCell(num, offset) == checkCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007849 RID: 30793 RVA: 0x000F3A00 File Offset: 0x000F1C00
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearOccupiedArea();
	}

	// Token: 0x0600784A RID: 30794 RVA: 0x0031D9E0 File Offset: 0x0031BBE0
	private void ClearOccupiedArea()
	{
		if (this.occupiedGridCells == null)
		{
			return;
		}
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				foreach (int cell in this.occupiedGridCells)
				{
					if (Grid.Objects[cell, (int)objectLayer] == base.gameObject)
					{
						Grid.Objects[cell, (int)objectLayer] = null;
					}
				}
			}
		}
	}

	// Token: 0x0600784B RID: 30795 RVA: 0x0031DA5C File Offset: 0x0031BC5C
	public void UpdateOccupiedArea()
	{
		if (this.objectLayers.Length == 0)
		{
			return;
		}
		if (this.occupiedGridCells == null)
		{
			this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
		}
		this.ClearOccupiedArea();
		int cell = Grid.PosToCell(base.gameObject);
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				for (int j = 0; j < this.OccupiedCellsOffsets.Length; j++)
				{
					CellOffset offset = this.OccupiedCellsOffsets[j];
					int num = Grid.OffsetCell(cell, offset);
					Grid.Objects[num, (int)objectLayer] = base.gameObject;
					this.occupiedGridCells[j] = num;
				}
			}
		}
	}

	// Token: 0x0600784C RID: 30796 RVA: 0x0031DB0C File Offset: 0x0031BD0C
	public int GetWidthInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.x);
			num2 = Math.Max(num2, cellOffset.x);
		}
		return num2 - num + 1;
	}

	// Token: 0x0600784D RID: 30797 RVA: 0x0031DB64 File Offset: 0x0031BD64
	public int GetHeightInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.y);
			num2 = Math.Max(num2, cellOffset.y);
		}
		return num2 - num + 1;
	}

	// Token: 0x0600784E RID: 30798 RVA: 0x000F3A0E File Offset: 0x000F1C0E
	public Extents GetExtents()
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets);
	}

	// Token: 0x0600784F RID: 30799 RVA: 0x000F3A26 File Offset: 0x000F1C26
	public Extents GetExtents(Orientation orientation)
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets, orientation);
	}

	// Token: 0x06007850 RID: 30800 RVA: 0x0031DBBC File Offset: 0x0031BDBC
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (this.OccupiedCellsOffsets != null)
		{
			foreach (CellOffset offset in this.OccupiedCellsOffsets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
			}
		}
		if (this.AboveOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset2 in this.AboveOccupiedCellOffsets)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
		if (this.BelowOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset3 in this.BelowOccupiedCellOffsets)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
	}

	// Token: 0x06007851 RID: 30801 RVA: 0x0031DD34 File Offset: 0x0031BF34
	public bool CanOccupyArea(int rootCell, ObjectLayer layer)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int cell = Grid.OffsetCell(rootCell, offset);
			if (Grid.Objects[cell, (int)layer] != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007852 RID: 30802 RVA: 0x0031DD80 File Offset: 0x0031BF80
	public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int arg = Grid.OffsetCell(rootCell, offset);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007853 RID: 30803 RVA: 0x0031DDC4 File Offset: 0x0031BFC4
	public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.AboveOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y + 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.AboveOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.AboveOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007854 RID: 30804 RVA: 0x0031DE74 File Offset: 0x0031C074
	public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.BelowOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y - 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.BelowOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.BelowOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.BelowOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04005A60 RID: 23136
	private CellOffset[] AboveOccupiedCellOffsets;

	// Token: 0x04005A61 RID: 23137
	private CellOffset[] BelowOccupiedCellOffsets;

	// Token: 0x04005A62 RID: 23138
	private int[] occupiedGridCells;

	// Token: 0x04005A63 RID: 23139
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04005A64 RID: 23140
	private Orientation appliedOrientation;

	// Token: 0x04005A65 RID: 23141
	public CellOffset[] _UnrotatedOccupiedCellsOffsets;

	// Token: 0x04005A66 RID: 23142
	public CellOffset[] _RotatedOccupiedCellsOffsets;

	// Token: 0x04005A67 RID: 23143
	public ObjectLayer[] objectLayers = new ObjectLayer[0];

	// Token: 0x04005A68 RID: 23144
	[SerializeField]
	private bool applyToCells = true;
}
