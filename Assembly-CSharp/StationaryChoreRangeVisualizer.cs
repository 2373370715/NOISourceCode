using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B49 RID: 2889
[AddComponentMenu("KMonoBehaviour/scripts/StationaryChoreRangeVisualizer")]
[Obsolete("Deprecated, use RangeVisualizer")]
public class StationaryChoreRangeVisualizer : KMonoBehaviour
{
	// Token: 0x060035B2 RID: 13746 RVA: 0x0021CDC4 File Offset: 0x0021AFC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate);
		if (this.movable)
		{
			Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "StationaryChoreRangeVisualizer.OnSpawn");
			base.Subscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate);
		}
	}

	// Token: 0x060035B3 RID: 13747 RVA: 0x0021CE24 File Offset: 0x0021B024
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1503271301, StationaryChoreRangeVisualizer.OnSelectDelegate, false);
		base.Unsubscribe<StationaryChoreRangeVisualizer>(-1643076535, StationaryChoreRangeVisualizer.OnRotatedDelegate, false);
		this.ClearVisualizers();
		base.OnCleanUp();
	}

	// Token: 0x060035B4 RID: 13748 RVA: 0x0021CE7C File Offset: 0x0021B07C
	private void OnSelect(object data)
	{
		if ((bool)data)
		{
			SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_form", false), base.transform.position, 1f);
			this.UpdateVisualizers();
			return;
		}
		SoundEvent.PlayOneShot(GlobalAssets.GetSound("RadialGrid_disappear", false), base.transform.position, 1f);
		this.ClearVisualizers();
	}

	// Token: 0x060035B5 RID: 13749 RVA: 0x000C7766 File Offset: 0x000C5966
	private void OnRotated(object data)
	{
		this.UpdateVisualizers();
	}

	// Token: 0x060035B6 RID: 13750 RVA: 0x000C7766 File Offset: 0x000C5966
	private void OnCellChange()
	{
		this.UpdateVisualizers();
	}

	// Token: 0x060035B7 RID: 13751 RVA: 0x0021CEE0 File Offset: 0x0021B0E0
	private void UpdateVisualizers()
	{
		this.newCells.Clear();
		CellOffset rotatedCellOffset = this.vision_offset;
		if (this.rotatable)
		{
			rotatedCellOffset = this.rotatable.GetRotatedCellOffset(this.vision_offset);
		}
		int cell = Grid.PosToCell(base.transform.gameObject);
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out num, out num2);
		for (int i = 0; i < this.height; i++)
		{
			for (int j = 0; j < this.width; j++)
			{
				CellOffset rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
				if (this.rotatable)
				{
					rotatedCellOffset2 = this.rotatable.GetRotatedCellOffset(rotatedCellOffset2);
				}
				int num3 = Grid.OffsetCell(cell, rotatedCellOffset2);
				if (Grid.IsValidCell(num3))
				{
					int x;
					int y;
					Grid.CellToXY(num3, out x, out y);
					if (Grid.TestLineOfSight(num, num2, x, y, this.blocking_cb, this.blocking_tile_visible, false))
					{
						this.newCells.Add(num3);
					}
				}
			}
		}
		for (int k = this.visualizers.Count - 1; k >= 0; k--)
		{
			if (this.newCells.Contains(this.visualizers[k].cell))
			{
				this.newCells.Remove(this.visualizers[k].cell);
			}
			else
			{
				this.DestroyEffect(this.visualizers[k].controller);
				this.visualizers.RemoveAt(k);
			}
		}
		for (int l = 0; l < this.newCells.Count; l++)
		{
			KBatchedAnimController controller = this.CreateEffect(this.newCells[l]);
			this.visualizers.Add(new StationaryChoreRangeVisualizer.VisData
			{
				cell = this.newCells[l],
				controller = controller
			});
		}
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x0021D0D0 File Offset: 0x0021B2D0
	private void ClearVisualizers()
	{
		for (int i = 0; i < this.visualizers.Count; i++)
		{
			this.DestroyEffect(this.visualizers[i].controller);
		}
		this.visualizers.Clear();
	}

	// Token: 0x060035B9 RID: 13753 RVA: 0x0021D118 File Offset: 0x0021B318
	private KBatchedAnimController CreateEffect(int cell)
	{
		KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(StationaryChoreRangeVisualizer.AnimName, Grid.CellToPosCCC(cell, this.sceneLayer), null, false, this.sceneLayer, true);
		kbatchedAnimController.destroyOnAnimComplete = false;
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.Play(StationaryChoreRangeVisualizer.PreAnims, KAnim.PlayMode.Loop);
		return kbatchedAnimController;
	}

	// Token: 0x060035BA RID: 13754 RVA: 0x000C776E File Offset: 0x000C596E
	private void DestroyEffect(KBatchedAnimController controller)
	{
		controller.destroyOnAnimComplete = true;
		controller.Play(StationaryChoreRangeVisualizer.PostAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04002519 RID: 9497
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400251A RID: 9498
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400251B RID: 9499
	public int x;

	// Token: 0x0400251C RID: 9500
	public int y;

	// Token: 0x0400251D RID: 9501
	public int width;

	// Token: 0x0400251E RID: 9502
	public int height;

	// Token: 0x0400251F RID: 9503
	public bool movable;

	// Token: 0x04002520 RID: 9504
	public Grid.SceneLayer sceneLayer = Grid.SceneLayer.FXFront;

	// Token: 0x04002521 RID: 9505
	public CellOffset vision_offset;

	// Token: 0x04002522 RID: 9506
	public Func<int, bool> blocking_cb = new Func<int, bool>(Grid.PhysicalBlockingCB);

	// Token: 0x04002523 RID: 9507
	public bool blocking_tile_visible = true;

	// Token: 0x04002524 RID: 9508
	private static readonly string AnimName = "transferarmgrid_kanim";

	// Token: 0x04002525 RID: 9509
	private static readonly HashedString[] PreAnims = new HashedString[]
	{
		"grid_pre",
		"grid_loop"
	};

	// Token: 0x04002526 RID: 9510
	private static readonly HashedString PostAnim = "grid_pst";

	// Token: 0x04002527 RID: 9511
	private List<StationaryChoreRangeVisualizer.VisData> visualizers = new List<StationaryChoreRangeVisualizer.VisData>();

	// Token: 0x04002528 RID: 9512
	private List<int> newCells = new List<int>();

	// Token: 0x04002529 RID: 9513
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnSelectDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnSelect(data);
	});

	// Token: 0x0400252A RID: 9514
	private static readonly EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<StationaryChoreRangeVisualizer>(delegate(StationaryChoreRangeVisualizer component, object data)
	{
		component.OnRotated(data);
	});

	// Token: 0x02000B4A RID: 2890
	private struct VisData
	{
		// Token: 0x0400252B RID: 9515
		public int cell;

		// Token: 0x0400252C RID: 9516
		public KBatchedAnimController controller;
	}
}
