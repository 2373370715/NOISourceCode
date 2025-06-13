using System;

public class FakeFloorAdder : KMonoBehaviour
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.initiallyActive)
		{
			this.SetFloor(true);
		}
	}

	public void SetFloor(bool active)
	{
		if (this.isActive == active)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.floorOffsets)
		{
			CellOffset offset = (component == null) ? cellOffset : component.GetRotatedCellOffset(cellOffset);
			int num = Grid.OffsetCell(cell, offset);
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

	protected override void OnCleanUp()
	{
		this.SetFloor(false);
		base.OnCleanUp();
	}

	public CellOffset[] floorOffsets;

	public bool initiallyActive = true;

	private bool isActive;
}
