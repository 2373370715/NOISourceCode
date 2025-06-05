using System;
using UnityEngine;

// Token: 0x02000F14 RID: 3860
public class ModularConduitPortTiler : KMonoBehaviour
{
	// Token: 0x06004D51 RID: 19793 RVA: 0x002736F8 File Offset: 0x002718F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.ModularConduitPort, true);
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				GameTags.ModularConduitPort
			};
		}
	}

	// Token: 0x06004D52 RID: 19794 RVA: 0x00273748 File Offset: 0x00271948
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.leftCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.leftCapDefaultSceneLayerAdjust), ModularConduitPortTiler.leftCapDefaultStr);
		if (this.manageLeftCap)
		{
			this.leftCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.leftCapLaunchpadStr);
			this.leftCapConduit = new KAnimSynchronizedController(component2, component2.GetLayer() + Grid.SceneLayer.Backwall, ModularConduitPortTiler.leftCapConduitStr);
		}
		this.rightCapDefault = new KAnimSynchronizedController(component2, (Grid.SceneLayer)(component2.GetLayer() + this.rightCapDefaultSceneLayerAdjust), ModularConduitPortTiler.rightCapDefaultStr);
		if (this.manageRightCap)
		{
			this.rightCapLaunchpad = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapLaunchpadStr);
			this.rightCapConduit = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), ModularConduitPortTiler.rightCapConduitStr);
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y, this.extents.width + 2, this.extents.height);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("ModularConduitPort.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
		this.CorrectAdjacentLaunchPads();
	}

	// Token: 0x06004D53 RID: 19795 RVA: 0x000D688D File Offset: 0x000D4A8D
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06004D54 RID: 19796 RVA: 0x002738A0 File Offset: 0x00271AA0
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellLeft = this.GetCellLeft();
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellLeft))
		{
			if (this.HasTileableNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellLeft))
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.leftCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (Grid.IsValidCell(cellRight))
		{
			if (this.HasTileableNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Conduit;
			}
			else if (this.HasLaunchpadNeighbour(cellRight))
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Launchpad;
			}
			else
			{
				this.rightCapSetting = ModularConduitPortTiler.AnimCapType.Default;
			}
		}
		if (this.manageLeftCap)
		{
			this.leftCapDefault.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.leftCapConduit.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.leftCapLaunchpad.Enable(this.leftCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
		if (this.manageRightCap)
		{
			this.rightCapDefault.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Default);
			this.rightCapConduit.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Conduit);
			this.rightCapLaunchpad.Enable(this.rightCapSetting == ModularConduitPortTiler.AnimCapType.Launchpad);
		}
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x002739B8 File Offset: 0x00271BB8
	private int GetCellLeft()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num - 1, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x002739F4 File Offset: 0x00271BF4
	private int GetCellRight()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(this.extents.x - num + this.extents.width, 0);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x00273A38 File Offset: 0x00271C38
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

	// Token: 0x06004D58 RID: 19800 RVA: 0x00273A84 File Offset: 0x00271C84
	private bool HasLaunchpadNeighbour(int neighbour_cell)
	{
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<LaunchPad>() != null;
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x000D68A5 File Offset: 0x000D4AA5
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

	// Token: 0x06004D5A RID: 19802 RVA: 0x00273AC0 File Offset: 0x00271CC0
	private void CorrectAdjacentLaunchPads()
	{
		int cellRight = this.GetCellRight();
		if (Grid.IsValidCell(cellRight) && this.HasLaunchpadNeighbour(cellRight))
		{
			Grid.Objects[cellRight, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
		int cellLeft = this.GetCellLeft();
		if (Grid.IsValidCell(cellLeft) && this.HasLaunchpadNeighbour(cellLeft))
		{
			Grid.Objects[cellLeft, 1].GetComponent<ModularConduitPortTiler>().UpdateEndCaps();
		}
	}

	// Token: 0x0400363F RID: 13887
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003640 RID: 13888
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04003641 RID: 13889
	public Tag[] tags;

	// Token: 0x04003642 RID: 13890
	public bool manageLeftCap = true;

	// Token: 0x04003643 RID: 13891
	public bool manageRightCap = true;

	// Token: 0x04003644 RID: 13892
	public int leftCapDefaultSceneLayerAdjust;

	// Token: 0x04003645 RID: 13893
	public int rightCapDefaultSceneLayerAdjust;

	// Token: 0x04003646 RID: 13894
	private Extents extents;

	// Token: 0x04003647 RID: 13895
	private ModularConduitPortTiler.AnimCapType leftCapSetting;

	// Token: 0x04003648 RID: 13896
	private ModularConduitPortTiler.AnimCapType rightCapSetting;

	// Token: 0x04003649 RID: 13897
	private static readonly string leftCapDefaultStr = "#cap_left_default";

	// Token: 0x0400364A RID: 13898
	private static readonly string leftCapLaunchpadStr = "#cap_left_launchpad";

	// Token: 0x0400364B RID: 13899
	private static readonly string leftCapConduitStr = "#cap_left_conduit";

	// Token: 0x0400364C RID: 13900
	private static readonly string rightCapDefaultStr = "#cap_right_default";

	// Token: 0x0400364D RID: 13901
	private static readonly string rightCapLaunchpadStr = "#cap_right_launchpad";

	// Token: 0x0400364E RID: 13902
	private static readonly string rightCapConduitStr = "#cap_right_conduit";

	// Token: 0x0400364F RID: 13903
	private KAnimSynchronizedController leftCapDefault;

	// Token: 0x04003650 RID: 13904
	private KAnimSynchronizedController leftCapLaunchpad;

	// Token: 0x04003651 RID: 13905
	private KAnimSynchronizedController leftCapConduit;

	// Token: 0x04003652 RID: 13906
	private KAnimSynchronizedController rightCapDefault;

	// Token: 0x04003653 RID: 13907
	private KAnimSynchronizedController rightCapLaunchpad;

	// Token: 0x04003654 RID: 13908
	private KAnimSynchronizedController rightCapConduit;

	// Token: 0x02000F15 RID: 3861
	private enum AnimCapType
	{
		// Token: 0x04003656 RID: 13910
		Default,
		// Token: 0x04003657 RID: 13911
		Conduit,
		// Token: 0x04003658 RID: 13912
		Launchpad
	}
}
