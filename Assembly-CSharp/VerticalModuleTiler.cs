using System;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class VerticalModuleTiler : KMonoBehaviour
{
	// Token: 0x06001AC9 RID: 6857 RVA: 0x001B4BB8 File Offset: 0x001B2DB8
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (this.manageTopCap)
		{
			this.topCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.topCapStr);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
		}
		this.PostReorderMove();
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000B5F24 File Offset: 0x000B4124
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000B5F3C File Offset: 0x000B413C
	public void PostReorderMove()
	{
		this.dirty = true;
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000B5F45 File Offset: 0x000B4145
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x001B4C2C File Offset: 0x001B2E2C
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellTop = this.GetCellTop();
		int cellBottom = this.GetCellBottom();
		if (Grid.IsValidCell(cellTop))
		{
			if (this.HasWideNeighbor(cellTop))
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (Grid.IsValidCell(cellBottom))
		{
			if (this.HasWideNeighbor(cellBottom))
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (this.manageTopCap)
		{
			this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x001B4CD0 File Offset: 0x001B2ED0
	private int GetCellTop()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x001B4D14 File Offset: 0x001B2F14
	private int GetCellBottom()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 - 1);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x001B4D50 File Offset: 0x001B2F50
	private bool HasWideNeighbor(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.GetComponent<ReorderableBuilding>() != null && component.GetComponent<Building>().Def.WidthInCells >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x001B4DB0 File Offset: 0x001B2FB0
	private void LateUpdate()
	{
		if (this.animController.Offset != this.m_previousAnimControllerOffset)
		{
			this.m_previousAnimControllerOffset = this.animController.Offset;
			this.bottomCapWide.Dirty();
			this.topCapWide.Dirty();
		}
		if (this.dirty)
		{
			if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
			{
				GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			}
			OccupyArea component = base.GetComponent<OccupyArea>();
			if (component != null)
			{
				this.extents = component.GetExtents();
			}
			Extents extents = new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
			this.UpdateEndCaps();
			this.dirty = false;
		}
	}

	// Token: 0x04001134 RID: 4404
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001135 RID: 4405
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001136 RID: 4406
	private Extents extents;

	// Token: 0x04001137 RID: 4407
	private VerticalModuleTiler.AnimCapType topCapSetting;

	// Token: 0x04001138 RID: 4408
	private VerticalModuleTiler.AnimCapType bottomCapSetting;

	// Token: 0x04001139 RID: 4409
	private bool manageTopCap = true;

	// Token: 0x0400113A RID: 4410
	private bool manageBottomCap = true;

	// Token: 0x0400113B RID: 4411
	private KAnimSynchronizedController topCapWide;

	// Token: 0x0400113C RID: 4412
	private KAnimSynchronizedController bottomCapWide;

	// Token: 0x0400113D RID: 4413
	private static readonly string topCapStr = "#cap_top_5";

	// Token: 0x0400113E RID: 4414
	private static readonly string bottomCapStr = "#cap_bottom_5";

	// Token: 0x0400113F RID: 4415
	private bool dirty;

	// Token: 0x04001140 RID: 4416
	[MyCmpGet]
	private KAnimControllerBase animController;

	// Token: 0x04001141 RID: 4417
	private Vector3 m_previousAnimControllerOffset;

	// Token: 0x020005F2 RID: 1522
	private enum AnimCapType
	{
		// Token: 0x04001143 RID: 4419
		ThreeWide,
		// Token: 0x04001144 RID: 4420
		FiveWide
	}
}
