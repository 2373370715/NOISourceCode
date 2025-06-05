using System;

// Token: 0x020016CC RID: 5836
public class OffsetTableTracker : OffsetTracker
{
	// Token: 0x17000793 RID: 1939
	// (get) Token: 0x0600786A RID: 30826 RVA: 0x000F3ACE File Offset: 0x000F1CCE
	private static NavGrid navGrid
	{
		get
		{
			if (OffsetTableTracker.navGridImpl == null)
			{
				OffsetTableTracker.navGridImpl = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
			}
			return OffsetTableTracker.navGridImpl;
		}
	}

	// Token: 0x0600786B RID: 30827 RVA: 0x000F3AF0 File Offset: 0x000F1CF0
	public OffsetTableTracker(CellOffset[][] table, KMonoBehaviour cmp)
	{
		this.table = table;
		this.cmp = cmp;
	}

	// Token: 0x0600786C RID: 30828 RVA: 0x0031F8A8 File Offset: 0x0031DAA8
	protected override void UpdateCell(int previous_cell, int current_cell)
	{
		if (previous_cell == current_cell)
		{
			return;
		}
		base.UpdateCell(previous_cell, current_cell);
		Extents extents = new Extents(current_cell, this.table);
		extents.height += 2;
		extents.y--;
		if (!this.solidPartitionerEntry.IsValid())
		{
			this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnCellChanged));
			this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("OffsetTableTracker.UpdateCell", this.cmp.gameObject, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		}
		else
		{
			GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, extents);
			GameScenePartitioner.Instance.UpdatePosition(this.validNavCellChangedPartitionerEntry, extents);
		}
		this.offsets = null;
	}

	// Token: 0x0600786D RID: 30829 RVA: 0x0031F990 File Offset: 0x0031DB90
	private static bool IsValidRow(int current_cell, CellOffset[] row, int rowIdx, int[] debugIdxs)
	{
		for (int i = 1; i < row.Length; i++)
		{
			int num = Grid.OffsetCell(current_cell, row[i]);
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (Grid.Solid[num])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600786E RID: 30830 RVA: 0x0031F9D4 File Offset: 0x0031DBD4
	private void UpdateOffsets(int cell, CellOffset[][] table)
	{
		HashSetPool<CellOffset, OffsetTableTracker>.PooledHashSet pooledHashSet = HashSetPool<CellOffset, OffsetTableTracker>.Allocate();
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < table.Length; i++)
			{
				CellOffset[] array = table[i];
				if (!pooledHashSet.Contains(array[0]))
				{
					int cell2 = Grid.OffsetCell(cell, array[0]);
					for (int j = 0; j < OffsetTableTracker.navGrid.ValidNavTypes.Length; j++)
					{
						NavType navType = OffsetTableTracker.navGrid.ValidNavTypes[j];
						if (navType != NavType.Tube && OffsetTableTracker.navGrid.NavTable.IsValid(cell2, navType) && OffsetTableTracker.IsValidRow(cell, array, i, this.DEBUG_rowValidIdx))
						{
							pooledHashSet.Add(array[0]);
							break;
						}
					}
				}
			}
		}
		if (this.offsets == null || this.offsets.Length != pooledHashSet.Count)
		{
			this.offsets = new CellOffset[pooledHashSet.Count];
		}
		pooledHashSet.CopyTo(this.offsets);
		pooledHashSet.Recycle();
	}

	// Token: 0x0600786F RID: 30831 RVA: 0x000F3B06 File Offset: 0x000F1D06
	protected override void UpdateOffsets(int current_cell)
	{
		base.UpdateOffsets(current_cell);
		this.UpdateOffsets(current_cell, this.table);
	}

	// Token: 0x06007870 RID: 30832 RVA: 0x000F3B1C File Offset: 0x000F1D1C
	private void OnCellChanged(object data)
	{
		this.offsets = null;
	}

	// Token: 0x06007871 RID: 30833 RVA: 0x000F3B25 File Offset: 0x000F1D25
	public override void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
	}

	// Token: 0x06007872 RID: 30834 RVA: 0x000F3B47 File Offset: 0x000F1D47
	public static void OnPathfindingInvalidated()
	{
		OffsetTableTracker.navGridImpl = null;
	}

	// Token: 0x04005A7C RID: 23164
	private readonly CellOffset[][] table;

	// Token: 0x04005A7D RID: 23165
	public HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x04005A7E RID: 23166
	public HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04005A7F RID: 23167
	private static NavGrid navGridImpl;

	// Token: 0x04005A80 RID: 23168
	private KMonoBehaviour cmp;

	// Token: 0x04005A81 RID: 23169
	private int[] DEBUG_rowValidIdx;
}
