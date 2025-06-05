using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200147E RID: 5246
public class SandboxFOWTool : BrushTool
{
	// Token: 0x06006C9A RID: 27802 RVA: 0x000EBCAD File Offset: 0x000E9EAD
	public static void DestroyInstance()
	{
		SandboxFOWTool.instance = null;
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06006C9B RID: 27803 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006C9C RID: 27804 RVA: 0x000EBCB5 File Offset: 0x000E9EB5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFOWTool.instance = this;
	}

	// Token: 0x06006C9D RID: 27805 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C9F RID: 27807 RVA: 0x000EBC12 File Offset: 0x000E9E12
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x000EBCC3 File Offset: 0x000E9EC3
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x002F50FC File Offset: 0x002F32FC
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

	// Token: 0x06006CA2 RID: 27810 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x000EBCE8 File Offset: 0x000E9EE8
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		Grid.Reveal(cell, byte.MaxValue, true);
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x002F51B4 File Offset: 0x002F33B4
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		int intSetting = this.settings.GetIntSetting("SandboxTools.BrushSize");
		this.ev = KFMOD.CreateInstance(GlobalAssets.GetSound("SandboxTool_Reveal", false));
		this.ev.setParameterByName("BrushSize", (float)intSetting, false);
		this.ev.start();
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x000EBCFE File Offset: 0x000E9EFE
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.ev.stop(STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x0400520B RID: 21003
	public static SandboxFOWTool instance;

	// Token: 0x0400520C RID: 21004
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x0400520D RID: 21005
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x0400520E RID: 21006
	private EventInstance ev;
}
