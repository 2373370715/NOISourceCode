using System;
using UnityEngine;

// Token: 0x02001463 RID: 5219
public class DeconstructTool : FilteredDragTool
{
	// Token: 0x06006B95 RID: 27541 RVA: 0x000EB150 File Offset: 0x000E9350
	public static void DestroyInstance()
	{
		DeconstructTool.Instance = null;
	}

	// Token: 0x06006B96 RID: 27542 RVA: 0x000EB158 File Offset: 0x000E9358
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DeconstructTool.Instance = this;
	}

	// Token: 0x06006B97 RID: 27543 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B98 RID: 27544 RVA: 0x000EAF78 File Offset: 0x000E9178
	protected override string GetConfirmSound()
	{
		return "Tile_Confirm_NegativeTool";
	}

	// Token: 0x06006B99 RID: 27545 RVA: 0x000EAF7F File Offset: 0x000E917F
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06006B9A RID: 27546 RVA: 0x000EB166 File Offset: 0x000E9366
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.DeconstructCell(cell);
	}

	// Token: 0x06006B9B RID: 27547 RVA: 0x002F16D0 File Offset: 0x002EF8D0
	public void DeconstructCell(int cell)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
				if (base.IsActiveLayer(filterLayerFromGameObject))
				{
					gameObject.Trigger(-790448070, null);
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06006B9C RID: 27548 RVA: 0x000EB16F File Offset: 0x000E936F
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006B9D RID: 27549 RVA: 0x000EB187 File Offset: 0x000E9387
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04005194 RID: 20884
	public static DeconstructTool Instance;
}
