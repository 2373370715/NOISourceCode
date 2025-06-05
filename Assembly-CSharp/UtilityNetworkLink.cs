using System;
using UnityEngine;

// Token: 0x02001A69 RID: 6761
public abstract class UtilityNetworkLink : KMonoBehaviour
{
	// Token: 0x06008CD7 RID: 36055 RVA: 0x00100928 File Offset: 0x000FEB28
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
		base.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
		this.Connect();
	}

	// Token: 0x06008CD8 RID: 36056 RVA: 0x00100958 File Offset: 0x000FEB58
	protected override void OnCleanUp()
	{
		base.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
		this.Disconnect();
		base.OnCleanUp();
	}

	// Token: 0x06008CD9 RID: 36057 RVA: 0x00373E34 File Offset: 0x00372034
	protected void Connect()
	{
		if (!this.visualizeOnly && !this.connected)
		{
			this.connected = true;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnConnect(cell, cell2);
		}
	}

	// Token: 0x06008CDA RID: 36058 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnConnect(int cell1, int cell2)
	{
	}

	// Token: 0x06008CDB RID: 36059 RVA: 0x00373E6C File Offset: 0x0037206C
	protected void Disconnect()
	{
		if (!this.visualizeOnly && this.connected)
		{
			this.connected = false;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnDisconnect(cell, cell2);
		}
	}

	// Token: 0x06008CDC RID: 36060 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDisconnect(int cell1, int cell2)
	{
	}

	// Token: 0x06008CDD RID: 36061 RVA: 0x00373EA4 File Offset: 0x003720A4
	public void GetCells(out int linked_cell1, out int linked_cell2)
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			Orientation orientation = component.Orientation;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.GetCells(cell, orientation, out linked_cell1, out linked_cell2);
			return;
		}
		linked_cell1 = -1;
		linked_cell2 = -1;
	}

	// Token: 0x06008CDE RID: 36062 RVA: 0x00373EEC File Offset: 0x003720EC
	public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
		linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
	}

	// Token: 0x06008CDF RID: 36063 RVA: 0x00373F28 File Offset: 0x00372128
	public bool AreCellsValid(int cell, Orientation orientation)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
	}

	// Token: 0x06008CE0 RID: 36064 RVA: 0x00100988 File Offset: 0x000FEB88
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06008CE1 RID: 36065 RVA: 0x00100990 File Offset: 0x000FEB90
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06008CE2 RID: 36066 RVA: 0x00373F64 File Offset: 0x00372164
	public int GetNetworkCell()
	{
		int result;
		int num;
		this.GetCells(out result, out num);
		return result;
	}

	// Token: 0x04006A52 RID: 27218
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04006A53 RID: 27219
	[SerializeField]
	public CellOffset link1;

	// Token: 0x04006A54 RID: 27220
	[SerializeField]
	public CellOffset link2;

	// Token: 0x04006A55 RID: 27221
	[SerializeField]
	public bool visualizeOnly;

	// Token: 0x04006A56 RID: 27222
	private bool connected;

	// Token: 0x04006A57 RID: 27223
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04006A58 RID: 27224
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
