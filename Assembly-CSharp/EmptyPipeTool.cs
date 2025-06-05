using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200146B RID: 5227
public class EmptyPipeTool : FilteredDragTool
{
	// Token: 0x06006BE0 RID: 27616 RVA: 0x000EB43F File Offset: 0x000E963F
	public static void DestroyInstance()
	{
		EmptyPipeTool.Instance = null;
	}

	// Token: 0x06006BE1 RID: 27617 RVA: 0x000EB447 File Offset: 0x000E9647
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EmptyPipeTool.Instance = this;
	}

	// Token: 0x06006BE2 RID: 27618 RVA: 0x002F28FC File Offset: 0x002F0AFC
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			if (base.IsActiveLayer((ObjectLayer)i))
			{
				GameObject gameObject = Grid.Objects[cell, i];
				if (!(gameObject == null))
				{
					IEmptyConduitWorkable component = gameObject.GetComponent<IEmptyConduitWorkable>();
					if (!component.IsNullOrDestroyed())
					{
						if (DebugHandler.InstantBuildMode)
						{
							component.EmptyContents();
						}
						else
						{
							component.MarkForEmptying();
							Prioritizable component2 = gameObject.GetComponent<Prioritizable>();
							if (component2 != null)
							{
								component2.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BE3 RID: 27619 RVA: 0x000EB16F File Offset: 0x000E936F
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006BE4 RID: 27620 RVA: 0x000EB187 File Offset: 0x000E9387
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x06006BE5 RID: 27621 RVA: 0x000EB455 File Offset: 0x000E9655
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x040051BB RID: 20923
	public static EmptyPipeTool Instance;
}
