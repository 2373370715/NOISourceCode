using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001472 RID: 5234
public class MopTool : DragTool
{
	// Token: 0x06006C2E RID: 27694 RVA: 0x000EB85F File Offset: 0x000E9A5F
	public static void DestroyInstance()
	{
		MopTool.Instance = null;
	}

	// Token: 0x06006C2F RID: 27695 RVA: 0x000EB867 File Offset: 0x000E9A67
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
		this.interceptNumberKeysForPriority = true;
		MopTool.Instance = this;
	}

	// Token: 0x06006C30 RID: 27696 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C31 RID: 27697 RVA: 0x002F3CB4 File Offset: 0x002F1EB4
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			if (DebugHandler.InstantBuildMode)
			{
				if (Grid.IsValidCell(cell))
				{
					Moppable.MopCell(cell, 1000000f, null);
					return;
				}
			}
			else
			{
				GameObject gameObject = Grid.Objects[cell, 8];
				if (!Grid.Solid[cell] && gameObject == null && Grid.Element[cell].IsLiquid)
				{
					bool flag = Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)];
					bool flag2 = Grid.Mass[cell] <= MopTool.maxMopAmt;
					if (flag && flag2)
					{
						gameObject = Util.KInstantiate(this.Placer, null, null);
						Grid.Objects[cell, 8] = gameObject;
						Vector3 vector = Grid.CellToPosCBC(cell, this.visualizerLayer);
						float num = -0.15f;
						vector.z += num;
						gameObject.transform.SetPosition(vector);
						gameObject.SetActive(true);
					}
					else
					{
						string text = UI.TOOLS.MOP.TOO_MUCH_LIQUID;
						if (!flag)
						{
							text = UI.TOOLS.MOP.NOT_ON_FLOOR;
						}
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, text, null, Grid.CellToPosCBC(cell, this.visualizerLayer), 1.5f, false, false);
					}
				}
				if (gameObject != null)
				{
					Prioritizable component = gameObject.GetComponent<Prioritizable>();
					if (component != null)
					{
						component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
					}
				}
			}
		}
	}

	// Token: 0x06006C32 RID: 27698 RVA: 0x000EAA64 File Offset: 0x000E8C64
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006C33 RID: 27699 RVA: 0x000EAA7C File Offset: 0x000E8C7C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}

	// Token: 0x040051E9 RID: 20969
	private GameObject Placer;

	// Token: 0x040051EA RID: 20970
	public static MopTool Instance;

	// Token: 0x040051EB RID: 20971
	private SimHashes Element;

	// Token: 0x040051EC RID: 20972
	public static float maxMopAmt = 150f;
}
