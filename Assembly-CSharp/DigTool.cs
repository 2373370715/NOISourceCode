using System;
using UnityEngine;

// Token: 0x02001464 RID: 5220
public class DigTool : DragTool
{
	// Token: 0x06006B9F RID: 27551 RVA: 0x000EB1A0 File Offset: 0x000E93A0
	public static void DestroyInstance()
	{
		DigTool.Instance = null;
	}

	// Token: 0x06006BA0 RID: 27552 RVA: 0x000EB1A8 File Offset: 0x000E93A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DigTool.Instance = this;
	}

	// Token: 0x06006BA1 RID: 27553 RVA: 0x000EB1B6 File Offset: 0x000E93B6
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		InterfaceTool.ActiveConfig.DigAction.Uproot(cell);
		InterfaceTool.ActiveConfig.DigAction.Dig(cell, distFromOrigin);
	}

	// Token: 0x06006BA2 RID: 27554 RVA: 0x002F1744 File Offset: 0x002EF944
	public static GameObject PlaceDig(int cell, int animationDelay = 0)
	{
		if (Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Objects[cell, 7] == null)
		{
			for (int i = 0; i < 45; i++)
			{
				if (Grid.Objects[cell, i] != null && Grid.Objects[cell, i].GetComponent<Constructable>() != null)
				{
					return null;
				}
			}
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(new Tag("DigPlacer")), null, null);
			gameObject.SetActive(true);
			Grid.Objects[cell, 7] = gameObject;
			Vector3 vector = Grid.CellToPosCBC(cell, DigTool.Instance.visualizerLayer);
			float num = -0.15f;
			vector.z += num;
			gameObject.transform.SetPosition(vector);
			gameObject.GetComponentInChildren<EasingAnimations>().PlayAnimation("ScaleUp", Mathf.Max(0f, (float)animationDelay * 0.02f));
			return gameObject;
		}
		if (Grid.Objects[cell, 7] != null)
		{
			return Grid.Objects[cell, 7];
		}
		return null;
	}

	// Token: 0x06006BA3 RID: 27555 RVA: 0x000EAA64 File Offset: 0x000E8C64
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006BA4 RID: 27556 RVA: 0x000EAA7C File Offset: 0x000E8C7C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x04005195 RID: 20885
	public static DigTool Instance;
}
