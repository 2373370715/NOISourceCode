using System;
using UnityEngine;

// Token: 0x02000AD2 RID: 2770
[AddComponentMenu("KMonoBehaviour/scripts/AntiCluster")]
public class MoverLayerOccupier : KMonoBehaviour, ISim200ms
{
	// Token: 0x060032CE RID: 13006 RVA: 0x00212238 File Offset: 0x00210438
	private void RefreshCellOccupy()
	{
		int cell = Grid.PosToCell(this);
		foreach (CellOffset offset in this.cellOffsets)
		{
			int current_cell = Grid.OffsetCell(cell, offset);
			if (this.previousCell != Grid.InvalidCell)
			{
				int previous_cell = Grid.OffsetCell(this.previousCell, offset);
				this.UpdateCell(previous_cell, current_cell);
			}
			else
			{
				this.UpdateCell(this.previousCell, current_cell);
			}
		}
		this.previousCell = cell;
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x000C57CB File Offset: 0x000C39CB
	public void Sim200ms(float dt)
	{
		this.RefreshCellOccupy();
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x002122B0 File Offset: 0x002104B0
	private void UpdateCell(int previous_cell, int current_cell)
	{
		foreach (ObjectLayer layer in this.objectLayers)
		{
			if (previous_cell != Grid.InvalidCell && previous_cell != current_cell && Grid.Objects[previous_cell, (int)layer] == base.gameObject)
			{
				Grid.Objects[previous_cell, (int)layer] = null;
			}
			GameObject gameObject = Grid.Objects[current_cell, (int)layer];
			if (gameObject == null)
			{
				Grid.Objects[current_cell, (int)layer] = base.gameObject;
			}
			else
			{
				KPrefabID component = base.GetComponent<KPrefabID>();
				KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
				if (component.InstanceID > component2.InstanceID)
				{
					Grid.Objects[current_cell, (int)layer] = base.gameObject;
				}
			}
		}
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x00212368 File Offset: 0x00210568
	private void CleanUpOccupiedCells()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		foreach (CellOffset offset in this.cellOffsets)
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			foreach (ObjectLayer layer in this.objectLayers)
			{
				if (Grid.Objects[cell2, (int)layer] == base.gameObject)
				{
					Grid.Objects[cell2, (int)layer] = null;
				}
			}
		}
	}

	// Token: 0x060032D2 RID: 13010 RVA: 0x000C57D3 File Offset: 0x000C39D3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.RefreshCellOccupy();
	}

	// Token: 0x060032D3 RID: 13011 RVA: 0x000C57E1 File Offset: 0x000C39E1
	protected override void OnCleanUp()
	{
		this.CleanUpOccupiedCells();
		base.OnCleanUp();
	}

	// Token: 0x040022BA RID: 8890
	private int previousCell = Grid.InvalidCell;

	// Token: 0x040022BB RID: 8891
	public ObjectLayer[] objectLayers;

	// Token: 0x040022BC RID: 8892
	public CellOffset[] cellOffsets;
}
