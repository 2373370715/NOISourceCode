using System;
using UnityEngine;

// Token: 0x0200133D RID: 4925
[AddComponentMenu("KMonoBehaviour/scripts/FloorSwitchActivator")]
public class FloorSwitchActivator : KMonoBehaviour
{
	// Token: 0x1700064E RID: 1614
	// (get) Token: 0x060064D6 RID: 25814 RVA: 0x000E63A8 File Offset: 0x000E45A8
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x060064D7 RID: 25815 RVA: 0x000E63B0 File Offset: 0x000E45B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x060064D8 RID: 25816 RVA: 0x000E63C4 File Offset: 0x000E45C4
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x060064D9 RID: 25817 RVA: 0x002CEF24 File Offset: 0x002CD124
	private void OnCellChange()
	{
		int num = Grid.PosToCell(this);
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
		if (Grid.IsValidCell(this.last_cell_occupied) && num != this.last_cell_occupied)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.NotifyChanged(num);
		this.last_cell_occupied = num;
	}

	// Token: 0x060064DA RID: 25818 RVA: 0x000E63D2 File Offset: 0x000E45D2
	private void NotifyChanged(int cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, this);
	}

	// Token: 0x060064DB RID: 25819 RVA: 0x000E63EA File Offset: 0x000E45EA
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x060064DC RID: 25820 RVA: 0x000E63F8 File Offset: 0x000E45F8
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x060064DD RID: 25821 RVA: 0x002CEF7C File Offset: 0x002CD17C
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", this, cell, GameScenePartitioner.Instance.floorSwitchActivatorLayer, null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "FloorSwitchActivator.Register");
		this.registered = true;
	}

	// Token: 0x060064DE RID: 25822 RVA: 0x002CEFE4 File Offset: 0x002CD1E4
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		if (this.last_cell_occupied > -1)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.registered = false;
	}

	// Token: 0x04004892 RID: 18578
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04004893 RID: 18579
	private bool registered;

	// Token: 0x04004894 RID: 18580
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04004895 RID: 18581
	private int last_cell_occupied = -1;
}
