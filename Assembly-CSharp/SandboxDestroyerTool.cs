using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200147C RID: 5244
public class SandboxDestroyerTool : BrushTool
{
	// Token: 0x06006C8C RID: 27788 RVA: 0x000EBBEE File Offset: 0x000E9DEE
	public static void DestroyInstance()
	{
		SandboxDestroyerTool.instance = null;
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06006C8D RID: 27789 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006C8E RID: 27790 RVA: 0x000EBBF6 File Offset: 0x000E9DF6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxDestroyerTool.instance = this;
		this.affectFoundation = true;
	}

	// Token: 0x06006C8F RID: 27791 RVA: 0x000EBC0B File Offset: 0x000E9E0B
	protected override string GetDragSound()
	{
		return "SandboxTool_Delete_Add";
	}

	// Token: 0x06006C90 RID: 27792 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C91 RID: 27793 RVA: 0x000EBC12 File Offset: 0x000E9E12
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06006C92 RID: 27794 RVA: 0x000EBB35 File Offset: 0x000E9D35
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006C93 RID: 27795 RVA: 0x002F4D44 File Offset: 0x002F2F44
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006C94 RID: 27796 RVA: 0x000EBC49 File Offset: 0x000E9E49
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Delete", false));
	}

	// Token: 0x06006C95 RID: 27797 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006C96 RID: 27798 RVA: 0x002F4DFC File Offset: 0x002F2FFC
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.SandBoxTool, 0f, 0f, byte.MaxValue, 0, index);
		HashSetPool<GameObject, SandboxDestroyerTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxDestroyerTool>.Allocate();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			if (Grid.PosToCell(pickupable) == cell)
			{
				pooledHashSet.Add(pickupable.gameObject);
			}
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			if (Grid.PosToCell(buildingComplete) == cell)
			{
				pooledHashSet.Add(buildingComplete.gameObject);
			}
		}
		if (Grid.Objects[cell, 1] != null)
		{
			pooledHashSet.Add(Grid.Objects[cell, 1]);
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.PosToCell(crop) == cell)
			{
				pooledHashSet.Add(crop.gameObject);
			}
		}
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell)
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (Comet comet in Components.Meteors.GetItems((int)Grid.WorldIdx[cell]))
		{
			if (!comet.IsNullOrDestroyed() && Grid.PosToCell(comet) == cell)
			{
				pooledHashSet.Add(comet.gameObject);
			}
		}
		foreach (GameObject original in pooledHashSet)
		{
			Util.KDestroyGameObject(original);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04005206 RID: 20998
	public static SandboxDestroyerTool instance;

	// Token: 0x04005207 RID: 20999
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04005208 RID: 21000
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
