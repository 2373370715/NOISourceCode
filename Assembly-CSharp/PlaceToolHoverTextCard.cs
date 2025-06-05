using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001B88 RID: 7048
public class PlaceToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x060093E7 RID: 37863 RVA: 0x0039BAA4 File Offset: 0x00399CA4
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
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
		this.ActionName = UI.TOOLS.PLACE.TOOLACTION;
		if (this.currentPlaceable != null && this.currentPlaceable.GetProperName() != null)
		{
			this.ToolName = string.Format(UI.TOOLS.PLACE.NAME, this.currentPlaceable.GetProperName());
		}
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(instance, hoverTextDrawer);
		int min_height = 26;
		int width = 8;
		string text;
		if (this.currentPlaceable != null && !this.currentPlaceable.IsValidPlaceLocation(num, out text))
		{
			hoverTextDrawer.NewLine(min_height);
			hoverTextDrawer.AddIndent(width);
			hoverTextDrawer.DrawText(text, this.HoverTextStyleSettings[1]);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04007033 RID: 28723
	public Placeable currentPlaceable;
}
