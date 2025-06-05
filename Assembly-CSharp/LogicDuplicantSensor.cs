using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000E79 RID: 3705
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDuplicantSensor : Switch, ISim1000ms, ISim200ms
{
	// Token: 0x060048DE RID: 18654 RVA: 0x000D3B14 File Offset: 0x000D1D14
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x00265078 File Offset: 0x00263278
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.RefreshReachableCells();
		this.wasOn = this.switchedOn;
		Vector2I vector2I = Grid.CellToXY(this.NaturalBuildingCell());
		int cell = Grid.XYToCell(vector2I.x, vector2I.y + this.pickupRange / 2);
		CellOffset rotatedCellOffset = new CellOffset(0, this.pickupRange / 2);
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
			if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
			{
				cell = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
			}
		}
		this.pickupableExtents = new Extents(cell, this.pickupRange / 2);
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("DuplicantSensor.PickupablesChanged", base.gameObject, this.pickupableExtents, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.pickupablesDirty = true;
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x000D3B23 File Offset: 0x000D1D23
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		MinionGroupProber.Get().ReleaseProber(this);
		base.OnCleanUp();
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x000D3B47 File Offset: 0x000D1D47
	public void Sim1000ms(float dt)
	{
		this.RefreshReachableCells();
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x000D3B4F File Offset: 0x000D1D4F
	public void Sim200ms(float dt)
	{
		this.RefreshPickupables();
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x0026517C File Offset: 0x0026337C
	private void RefreshReachableCells()
	{
		ListPool<int, LogicDuplicantSensor>.PooledList pooledList = ListPool<int, LogicDuplicantSensor>.Allocate(this.reachableCells);
		this.reachableCells.Clear();
		int num;
		int num2;
		Grid.CellToXY(this.NaturalBuildingCell(), out num, out num2);
		int num3 = num - this.pickupRange / 2;
		for (int i = num2; i < num2 + this.pickupRange + 1; i++)
		{
			for (int j = num3; j < num3 + this.pickupRange + 1; j++)
			{
				int num4 = Grid.XYToCell(j, i);
				CellOffset rotatedCellOffset = new CellOffset(j - num, i - num2);
				if (this.rotatable)
				{
					rotatedCellOffset = this.rotatable.GetRotatedCellOffset(rotatedCellOffset);
					if (Grid.IsCellOffsetValid(this.NaturalBuildingCell(), rotatedCellOffset))
					{
						num4 = Grid.OffsetCell(this.NaturalBuildingCell(), rotatedCellOffset);
						Vector2I vector2I = Grid.CellToXY(num4);
						if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, vector2I.x, vector2I.y, true))
						{
							this.reachableCells.Add(num4);
						}
					}
				}
				else if (Grid.IsValidCell(num4) && Grid.IsPhysicallyAccessible(num, num2, j, i, true))
				{
					this.reachableCells.Add(num4);
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x000D3B57 File Offset: 0x000D1D57
	public bool IsCellReachable(int cell)
	{
		return this.reachableCells.Contains(cell);
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x002652B0 File Offset: 0x002634B0
	private void RefreshPickupables()
	{
		if (!this.pickupablesDirty)
		{
			return;
		}
		this.duplicants.Clear();
		ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicDuplicantSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(this.pickupableExtents.x, this.pickupableExtents.y, this.pickupableExtents.width, this.pickupableExtents.height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		int cell_a = Grid.PosToCell(this);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			int pickupableCell = this.GetPickupableCell(pickupable);
			int cellRange = Grid.GetCellRange(cell_a, pickupableCell);
			if (this.IsPickupableRelevantToMyInterestsAndReachable(pickupable) && cellRange <= this.pickupRange)
			{
				this.duplicants.Add(pickupable);
			}
		}
		this.SetState(this.duplicants.Count > 0);
		this.pickupablesDirty = false;
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x00265390 File Offset: 0x00263590
	private void OnPickupablesChanged(object data)
	{
		Pickupable pickupable = data as Pickupable;
		if (pickupable && this.IsPickupableRelevantToMyInterests(pickupable))
		{
			this.pickupablesDirty = true;
		}
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x000D3B65 File Offset: 0x000D1D65
	private bool IsPickupableRelevantToMyInterests(Pickupable pickupable)
	{
		return pickupable.KPrefabID.HasTag(GameTags.DupeBrain);
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x002653BC File Offset: 0x002635BC
	private bool IsPickupableRelevantToMyInterestsAndReachable(Pickupable pickupable)
	{
		if (!this.IsPickupableRelevantToMyInterests(pickupable))
		{
			return false;
		}
		int pickupableCell = this.GetPickupableCell(pickupable);
		return this.IsCellReachable(pickupableCell);
	}

	// Token: 0x060048E9 RID: 18665 RVA: 0x000D3B7C File Offset: 0x000D1D7C
	private int GetPickupableCell(Pickupable pickupable)
	{
		return pickupable.cachedCell;
	}

	// Token: 0x060048EA RID: 18666 RVA: 0x000D3B84 File Offset: 0x000D1D84
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060048EB RID: 18667 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060048EC RID: 18668 RVA: 0x002653E8 File Offset: 0x002635E8
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060048ED RID: 18669 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x0400331A RID: 13082
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x0400331B RID: 13083
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400331C RID: 13084
	public int pickupRange = 4;

	// Token: 0x0400331D RID: 13085
	private bool wasOn;

	// Token: 0x0400331E RID: 13086
	private List<Pickupable> duplicants = new List<Pickupable>();

	// Token: 0x0400331F RID: 13087
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04003320 RID: 13088
	private bool pickupablesDirty;

	// Token: 0x04003321 RID: 13089
	private Extents pickupableExtents;

	// Token: 0x04003322 RID: 13090
	private List<int> reachableCells = new List<int>(100);
}
