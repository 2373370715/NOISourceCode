using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001476 RID: 5238
public class PrioritizeTool : FilteredDragTool
{
	// Token: 0x06006C56 RID: 27734 RVA: 0x000EB9F9 File Offset: 0x000E9BF9
	public static void DestroyInstance()
	{
		PrioritizeTool.Instance = null;
	}

	// Token: 0x06006C57 RID: 27735 RVA: 0x002F421C File Offset: 0x002F241C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.interceptNumberKeysForPriority = true;
		PrioritizeTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
		this.viewMode = OverlayModes.Priorities.ID;
		Game.Instance.prioritizableRenderer.currentTool = this;
	}

	// Token: 0x06006C58 RID: 27736 RVA: 0x002F426C File Offset: 0x002F246C
	public override string GetFilterLayerFromGameObject(GameObject input)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (input.GetComponent<Diggable>())
		{
			flag = true;
		}
		if (input.GetComponent<Constructable>() || (input.GetComponent<Deconstructable>() && input.GetComponent<Deconstructable>().IsMarkedForDeconstruction()))
		{
			flag2 = true;
		}
		if (input.GetComponent<Clearable>() || input.GetComponent<Moppable>() || input.GetComponent<StorageLocker>())
		{
			flag3 = true;
		}
		if (flag2)
		{
			return ToolParameterMenu.FILTERLAYERS.CONSTRUCTION;
		}
		if (flag)
		{
			return ToolParameterMenu.FILTERLAYERS.DIG;
		}
		if (flag3)
		{
			return ToolParameterMenu.FILTERLAYERS.CLEAN;
		}
		return ToolParameterMenu.FILTERLAYERS.OPERATE;
	}

	// Token: 0x06006C59 RID: 27737 RVA: 0x000EBA01 File Offset: 0x000E9C01
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CONSTRUCTION, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIG, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEAN, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.OPERATE, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06006C5A RID: 27738 RVA: 0x002F4300 File Offset: 0x002F2500
	private bool TryPrioritizeGameObject(GameObject target, PrioritySetting priority)
	{
		string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(target);
		if (base.IsActiveLayer(filterLayerFromGameObject))
		{
			Prioritizable component = target.GetComponent<Prioritizable>();
			if (component != null && component.showIcon && component.IsPrioritizable())
			{
				component.SetMasterPriority(priority);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006C5B RID: 27739 RVA: 0x002F4348 File Offset: 0x002F2548
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
		int num = 0;
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				if (gameObject.GetComponent<Pickupable>())
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					while (objectLayerListItem != null)
					{
						GameObject gameObject2 = objectLayerListItem.gameObject;
						objectLayerListItem = objectLayerListItem.nextItem;
						if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && this.TryPrioritizeGameObject(gameObject2, lastSelectedPriority))
						{
							num++;
						}
					}
				}
				else if (this.TryPrioritizeGameObject(gameObject, lastSelectedPriority))
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			PriorityScreen.PlayPriorityConfirmSound(lastSelectedPriority);
		}
	}

	// Token: 0x06006C5C RID: 27740 RVA: 0x002F4404 File Offset: 0x002F2604
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.ShowDiagram(true);
		ToolMenu.Instance.PriorityScreen.Show(true);
		ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
	}

	// Token: 0x06006C5D RID: 27741 RVA: 0x002F4460 File Offset: 0x002F2660
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
		ToolMenu.Instance.PriorityScreen.ShowDiagram(false);
		ToolMenu.Instance.PriorityScreen.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06006C5E RID: 27742 RVA: 0x002F44BC File Offset: 0x002F26BC
	public void Update()
	{
		PrioritySetting lastSelectedPriority = ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority();
		int num = 0;
		if (lastSelectedPriority.priority_class >= PriorityScreen.PriorityClass.high)
		{
			num += 9;
		}
		if (lastSelectedPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
		{
			num = num;
		}
		num += lastSelectedPriority.priority_value;
		Texture2D mainTexture = this.cursors[num - 1];
		MeshRenderer componentInChildren = this.visualizer.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren != null)
		{
			componentInChildren.material.mainTexture = mainTexture;
		}
	}

	// Token: 0x040051F8 RID: 20984
	public GameObject Placer;

	// Token: 0x040051F9 RID: 20985
	public static PrioritizeTool Instance;

	// Token: 0x040051FA RID: 20986
	public Texture2D[] cursors;
}
