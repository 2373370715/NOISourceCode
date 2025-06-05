using System;

// Token: 0x0200131A RID: 4890
public class FakeFloorAdder : KMonoBehaviour
{
	// Token: 0x0600641F RID: 25631 RVA: 0x000E5D69 File Offset: 0x000E3F69
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	// Token: 0x06006420 RID: 25632 RVA: 0x002CAAD8 File Offset: 0x002C8CD8
	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Building component = base.GetComponent<Building>();
		foreach (CellOffset offset in this.floorOffsets)
		{
			CellOffset rotatedOffset = component.GetRotatedOffset(offset);
			int num = Grid.OffsetCell(cell, rotatedOffset);
			if (active)
			{
				Grid.FakeFloor.Add(num);
			}
			else
			{
				Grid.FakeFloor.Remove(num);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
		this.isActive = active;
	}

	// Token: 0x06006421 RID: 25633 RVA: 0x000E5D80 File Offset: 0x000E3F80
	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	// Token: 0x040047F0 RID: 18416
	public CellOffset[] floorOffsets;

	// Token: 0x040047F1 RID: 18417
	public bool initiallyActive = true;

	// Token: 0x040047F2 RID: 18418
	private bool isActive;
}
