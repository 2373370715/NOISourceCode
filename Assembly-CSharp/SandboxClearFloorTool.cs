using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001479 RID: 5241
public class SandboxClearFloorTool : BrushTool
{
	// Token: 0x06006C73 RID: 27763 RVA: 0x000EBB1F File Offset: 0x000E9D1F
	public static void DestroyInstance()
	{
		SandboxClearFloorTool.instance = null;
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06006C74 RID: 27764 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006C75 RID: 27765 RVA: 0x000EBB27 File Offset: 0x000E9D27
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxClearFloorTool.instance = this;
	}

	// Token: 0x06006C76 RID: 27766 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006C77 RID: 27767 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C78 RID: 27768 RVA: 0x002F4AD4 File Offset: 0x002F2CD4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x000EBB35 File Offset: 0x000E9D35
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006C7A RID: 27770 RVA: 0x002EF7C8 File Offset: 0x002ED9C8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006C7B RID: 27771 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006C7C RID: 27772 RVA: 0x000EBA86 File Offset: 0x000E9C86
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006C7D RID: 27773 RVA: 0x002F4B38 File Offset: 0x002F2D38
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		bool flag = false;
		using (List<Pickupable>.Enumerator enumerator = Components.Pickupables.Items.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Pickupable pickup = enumerator.Current;
				if (!(pickup.storage != null) && Grid.PosToCell(pickup) == cell && Components.LiveMinionIdentities.Items.Find((MinionIdentity match) => match.gameObject == pickup.gameObject) == null)
				{
					if (!flag)
					{
						KFMOD.PlayOneShot(this.soundPath, pickup.gameObject.transform.GetPosition(), 1f);
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.SANDBOXTOOLS.CLEARFLOOR.DELETED, pickup.transform, 1.5f, false);
						flag = true;
					}
					Util.KDestroyGameObject(pickup.gameObject);
				}
			}
		}
	}

	// Token: 0x04005201 RID: 20993
	public static SandboxClearFloorTool instance;

	// Token: 0x04005202 RID: 20994
	private string soundPath = GlobalAssets.GetSound("SandboxTool_ClearFloor", false);
}
