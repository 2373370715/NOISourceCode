using System;
using UnityEngine;

// Token: 0x02001451 RID: 5201
public class AttackTool : DragTool
{
	// Token: 0x06006AFA RID: 27386 RVA: 0x002EE5F8 File Offset: 0x002EC7F8
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(downPos, upPos), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(downPos, upPos), false);
		AttackTool.MarkForAttack(regularizedPos, regularizedPos2, true);
	}

	// Token: 0x06006AFB RID: 27387 RVA: 0x002EE640 File Offset: 0x002EC840
	public static void MarkForAttack(Vector2 min, Vector2 max, bool mark)
	{
		foreach (FactionAlignment factionAlignment in Components.FactionAlignments.Items)
		{
			if (!factionAlignment.IsNullOrDestroyed())
			{
				Vector2 vector = Grid.PosToXY(factionAlignment.transform.GetPosition());
				if (vector.x >= min.x && vector.x < max.x && vector.y >= min.y && vector.y < max.y)
				{
					if (mark)
					{
						if (FactionManager.Instance.GetDisposition(FactionManager.FactionID.Duplicant, factionAlignment.Alignment) != FactionManager.Disposition.Assist)
						{
							factionAlignment.SetPlayerTargeted(true);
							Prioritizable component = factionAlignment.GetComponent<Prioritizable>();
							if (component != null)
							{
								component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
							}
						}
					}
					else
					{
						factionAlignment.gameObject.Trigger(2127324410, null);
					}
				}
			}
		}
	}

	// Token: 0x06006AFC RID: 27388 RVA: 0x000EAA64 File Offset: 0x000E8C64
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		ToolMenu.Instance.PriorityScreen.Show(true);
	}

	// Token: 0x06006AFD RID: 27389 RVA: 0x000EAA7C File Offset: 0x000E8C7C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		ToolMenu.Instance.PriorityScreen.Show(false);
	}
}
