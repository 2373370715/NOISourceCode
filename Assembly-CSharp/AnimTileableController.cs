using System;
using UnityEngine;

// Token: 0x02000C4E RID: 3150
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileableController")]
public class AnimTileableController : KMonoBehaviour
{
	// Token: 0x06003B7C RID: 15228 RVA: 0x000CADB8 File Offset: 0x000C8FB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00238AF0 File Offset: 0x00236CF0
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		KBatchedAnimController component3 = base.GetComponent<KBatchedAnimController>();
		this.left = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.leftName);
		this.right = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.rightName);
		this.top = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.topName);
		this.bottom = new KAnimSynchronizedController(component3, (Grid.SceneLayer)component3.GetLayer(), this.bottomName);
		this.UpdateEndCaps();
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x000CADEF File Offset: 0x000C8FEF
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x00238C10 File Offset: 0x00236E10
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool enable = true;
		bool enable2 = true;
		bool enable3 = true;
		bool enable4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			enable = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			enable2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			enable3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			enable4 = !this.HasTileableNeighbour(num6);
		}
		this.left.Enable(enable);
		this.right.Enable(enable2);
		this.top.Enable(enable3);
		this.bottom.Enable(enable4);
	}

	// Token: 0x06003B80 RID: 15232 RVA: 0x00238D94 File Offset: 0x00236F94
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06003B81 RID: 15233 RVA: 0x000CAE07 File Offset: 0x000C9007
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

	// Token: 0x04002933 RID: 10547
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002934 RID: 10548
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04002935 RID: 10549
	public Tag[] tags;

	// Token: 0x04002936 RID: 10550
	private Extents extents;

	// Token: 0x04002937 RID: 10551
	public string leftName = "#cap_left";

	// Token: 0x04002938 RID: 10552
	public string rightName = "#cap_right";

	// Token: 0x04002939 RID: 10553
	public string topName = "#cap_top";

	// Token: 0x0400293A RID: 10554
	public string bottomName = "#cap_bottom";

	// Token: 0x0400293B RID: 10555
	private KAnimSynchronizedController left;

	// Token: 0x0400293C RID: 10556
	private KAnimSynchronizedController right;

	// Token: 0x0400293D RID: 10557
	private KAnimSynchronizedController top;

	// Token: 0x0400293E RID: 10558
	private KAnimSynchronizedController bottom;
}
