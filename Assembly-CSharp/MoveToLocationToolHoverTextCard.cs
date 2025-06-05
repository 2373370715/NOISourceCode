using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001B4C RID: 6988
public class MoveToLocationToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06009290 RID: 37520 RVA: 0x00393A18 File Offset: 0x00391C18
	public override void UpdateHoverElements(List<KSelectable> selected)
	{
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		base.DrawTitle(HoverTextScreen.Instance, hoverTextDrawer);
		base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
		if (!MoveToLocationTool.Instance.CanMoveTo(num))
		{
			hoverTextDrawer.NewLine(26);
			hoverTextDrawer.DrawText(UI.TOOLS.MOVETOLOCATION.UNREACHABLE, this.HoverTextStyleSettings[1]);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}
}
