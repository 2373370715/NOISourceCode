using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200146E RID: 5230
public class HarvestTool : DragTool
{
	// Token: 0x06006BFA RID: 27642 RVA: 0x000EB61E File Offset: 0x000E981E
	public static void DestroyInstance()
	{
		HarvestTool.Instance = null;
	}

	// Token: 0x06006BFB RID: 27643 RVA: 0x000EB626 File Offset: 0x000E9826
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		HarvestTool.Instance = this;
		this.options.Add("HARVEST_WHEN_READY", ToolParameterMenu.ToggleState.On);
		this.options.Add("DO_NOT_HARVEST", ToolParameterMenu.ToggleState.Off);
		this.viewMode = OverlayModes.Harvest.ID;
	}

	// Token: 0x06006BFC RID: 27644 RVA: 0x002F2E74 File Offset: 0x002F1074
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			foreach (HarvestDesignatable harvestDesignatable in Components.HarvestDesignatables.Items)
			{
				OccupyArea area = harvestDesignatable.area;
				if (Grid.PosToCell(harvestDesignatable) == cell || (area != null && area.CheckIsOccupying(cell)))
				{
					if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
					{
						harvestDesignatable.SetHarvestWhenReady(true);
					}
					else if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
					{
						Harvestable component = harvestDesignatable.GetComponent<Harvestable>();
						if (component != null)
						{
							component.Trigger(2127324410, null);
						}
						harvestDesignatable.SetHarvestWhenReady(false);
					}
					Prioritizable component2 = harvestDesignatable.GetComponent<Prioritizable>();
					if (component2 != null)
					{
						component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x002F2F74 File Offset: 0x002F1174
	public void Update()
	{
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			if (this.options["HARVEST_WHEN_READY"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[0];
				return;
			}
			if (this.options["DO_NOT_HARVEST"] == ToolParameterMenu.ToggleState.On)
			{
				componentInChildren.material.mainTexture = this.visualizerTextures[1];
			}
		}
	}

	// Token: 0x06006BFE RID: 27646 RVA: 0x000EAF22 File Offset: 0x000E9122
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06006BFF RID: 27647 RVA: 0x000EB661 File Offset: 0x000E9861
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.options);
	}

	// Token: 0x06006C00 RID: 27648 RVA: 0x000EB68E File Offset: 0x000E988E
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
	}

	// Token: 0x040051C4 RID: 20932
	public GameObject Placer;

	// Token: 0x040051C5 RID: 20933
	public static HarvestTool Instance;

	// Token: 0x040051C6 RID: 20934
	public Texture2D[] visualizerTextures;

	// Token: 0x040051C7 RID: 20935
	private Dictionary<string, ToolParameterMenu.ToggleState> options = new Dictionary<string, ToolParameterMenu.ToggleState>();
}
