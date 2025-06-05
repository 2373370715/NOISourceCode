using System;
using UnityEngine;

// Token: 0x0200145C RID: 5212
public class ClearTool : DragTool
{
	// Token: 0x06006B68 RID: 27496 RVA: 0x000EAF8E File Offset: 0x000E918E
	public static void DestroyInstance()
	{
		ClearTool.Instance = null;
	}

	// Token: 0x06006B69 RID: 27497 RVA: 0x000EAF96 File Offset: 0x000E9196
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClearTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
	}

	// Token: 0x06006B6A RID: 27498 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B6B RID: 27499 RVA: 0x002F0C40 File Offset: 0x002EEE40
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject == null)
		{
			return;
		}
		ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
		while (objectLayerListItem != null)
		{
			GameObject gameObject2 = objectLayerListItem.gameObject;
			objectLayerListItem = objectLayerListItem.nextItem;
			if (!(gameObject2 == null) && !(gameObject2.GetComponent<MinionIdentity>() != null) && gameObject2.GetComponent<Clearable>().isClearable)
			{
				gameObject2.GetComponent<Clearable>().MarkForClear(false, false);
				Prioritizable component = gameObject2.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}
	}

	// Token: 0x06006B6C RID: 27500 RVA: 0x000EAA64 File Offset: 0x000E8C64
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006B6D RID: 27501 RVA: 0x000EAA7C File Offset: 0x000E8C7C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04005174 RID: 20852
	public static ClearTool Instance;
}
