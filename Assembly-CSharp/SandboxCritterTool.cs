using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200147B RID: 5243
public class SandboxCritterTool : BrushTool
{
	// Token: 0x06006C81 RID: 27777 RVA: 0x000EBB88 File Offset: 0x000E9D88
	public static void DestroyInstance()
	{
		SandboxCritterTool.instance = null;
	}

	// Token: 0x06006C82 RID: 27778 RVA: 0x000EBB90 File Offset: 0x000E9D90
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxCritterTool.instance = this;
	}

	// Token: 0x06006C83 RID: 27779 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006C84 RID: 27780 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C85 RID: 27781 RVA: 0x000EBB9E File Offset: 0x000E9D9E
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue(6f, true);
	}

	// Token: 0x06006C86 RID: 27782 RVA: 0x000EBB35 File Offset: 0x000E9D35
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006C87 RID: 27783 RVA: 0x002EF7C8 File Offset: 0x002ED9C8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006C88 RID: 27784 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006C89 RID: 27785 RVA: 0x000EBA86 File Offset: 0x000E9C86
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006C8A RID: 27786 RVA: 0x002F4C58 File Offset: 0x002F2E58
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		HashSetPool<GameObject, SandboxCritterTool>.PooledHashSet pooledHashSet = HashSetPool<GameObject, SandboxCritterTool>.Allocate();
		foreach (Health health in Components.Health.Items)
		{
			if (Grid.PosToCell(health) == cell && health.GetComponent<KPrefabID>().HasTag(GameTags.Creature))
			{
				pooledHashSet.Add(health.gameObject);
			}
		}
		foreach (GameObject gameObject in pooledHashSet)
		{
			KFMOD.PlayOneShot(this.soundPath, gameObject.gameObject.transform.GetPosition(), 1f);
			Util.KDestroyGameObject(gameObject);
		}
		pooledHashSet.Recycle();
	}

	// Token: 0x04005204 RID: 20996
	public static SandboxCritterTool instance;

	// Token: 0x04005205 RID: 20997
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
