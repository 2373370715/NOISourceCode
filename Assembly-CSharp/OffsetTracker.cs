using System;
using UnityEngine;

// Token: 0x020016CD RID: 5837
public class OffsetTracker
{
	// Token: 0x06007873 RID: 30835 RVA: 0x0031FAC8 File Offset: 0x0031DCC8
	public virtual CellOffset[] GetOffsets(int current_cell)
	{
		if (current_cell != this.previousCell)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateCell(this.previousCell, current_cell);
			this.previousCell = current_cell;
		}
		if (this.offsets == null)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateOffsets(this.previousCell);
		}
		return this.offsets;
	}

	// Token: 0x06007874 RID: 30836 RVA: 0x000F3B4F File Offset: 0x000F1D4F
	public virtual bool ValidateOffsets(int current_cell)
	{
		return current_cell == this.previousCell && this.offsets != null;
	}

	// Token: 0x06007875 RID: 30837 RVA: 0x0031FB30 File Offset: 0x0031DD30
	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	// Token: 0x06007876 RID: 30838 RVA: 0x000F3B65 File Offset: 0x000F1D65
	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	// Token: 0x06007877 RID: 30839 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	// Token: 0x06007878 RID: 30840 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	// Token: 0x06007879 RID: 30841 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Clear()
	{
	}

	// Token: 0x0600787A RID: 30842 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void DebugDrawExtents()
	{
	}

	// Token: 0x0600787B RID: 30843 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void DebugDrawEditor()
	{
	}

	// Token: 0x0600787C RID: 30844 RVA: 0x0031FB58 File Offset: 0x0031DD58
	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	// Token: 0x04005A82 RID: 23170
	public static bool isExecutingWithinJob;

	// Token: 0x04005A83 RID: 23171
	protected CellOffset[] offsets;

	// Token: 0x04005A84 RID: 23172
	protected int previousCell = Grid.InvalidCell;
}
