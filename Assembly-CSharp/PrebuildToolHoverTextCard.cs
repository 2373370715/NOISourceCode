using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001B89 RID: 7049
public class PrebuildToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x060093E9 RID: 37865 RVA: 0x0039BBA8 File Offset: 0x00399DA8
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		if (!this.errorMessage.IsNullOrWhiteSpace())
		{
			bool flag = true;
			foreach (string text in this.errorMessage.Split('\n', StringSplitOptions.None))
			{
				if (!flag)
				{
					hoverTextDrawer.NewLine(26);
				}
				hoverTextDrawer.DrawText(text.ToUpper(), this.HoverTextStyleSettings[flag ? 0 : 1]);
				flag = false;
			}
		}
		hoverTextDrawer.NewLine(26);
		if (KInputManager.currentControllerIsGamepad)
		{
			hoverTextDrawer.DrawIcon(KInputManager.steamInputInterpreter.GetActionSprite(global::Action.MouseRight, false), 20);
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("icon_mouse_right"), 20);
		}
		hoverTextDrawer.DrawText(this.backStr, this.Styles_Instruction.Standard);
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04007034 RID: 28724
	public string errorMessage;

	// Token: 0x04007035 RID: 28725
	public BuildingDef currentDef;
}
