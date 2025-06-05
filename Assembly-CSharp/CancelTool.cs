using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200145A RID: 5210
public class CancelTool : FilteredDragTool
{
	// Token: 0x06006B5B RID: 27483 RVA: 0x000EAF41 File Offset: 0x000E9141
	public static void DestroyInstance()
	{
		CancelTool.Instance = null;
	}

	// Token: 0x06006B5C RID: 27484 RVA: 0x000EAF49 File Offset: 0x000E9149
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CancelTool.Instance = this;
	}

	// Token: 0x06006B5D RID: 27485 RVA: 0x000EAF57 File Offset: 0x000E9157
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		base.GetDefaultFilters(filters);
		filters.Add(ToolParameterMenu.FILTERLAYERS.CLEANANDCLEAR, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.DIGPLACER, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06006B5E RID: 27486 RVA: 0x000EAF78 File Offset: 0x000E9178
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06006B5F RID: 27487 RVA: 0x000EAF7F File Offset: 0x000E917F
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06006B60 RID: 27488 RVA: 0x002F0A58 File Offset: 0x002EEC58
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(2127324410, null);
				}
			}
		}
	}

	// Token: 0x06006B61 RID: 27489 RVA: 0x002F0AA8 File Offset: 0x002EECA8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, false);
		CaptureTool.MarkForCapture(regularizedPos, regularizedPos2, false);
	}

	// Token: 0x04005173 RID: 20851
	public static CancelTool Instance;
}
