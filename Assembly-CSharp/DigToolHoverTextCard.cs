using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B18 RID: 6936
public class DigToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06009151 RID: 37201 RVA: 0x0038CD68 File Offset: 0x0038AF68
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
		if (Grid.IsVisible(num))
		{
			base.DrawTitle(instance, hoverTextDrawer);
			base.DrawInstructions(HoverTextScreen.Instance, hoverTextDrawer);
			Element element = Grid.Element[num];
			bool flag = false;
			if (Grid.Solid[num] && Diggable.IsDiggable(num))
			{
				flag = true;
			}
			if (flag)
			{
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawText(element.nameUpperCase, this.Styles_Title.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(element.GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				string[] array = HoverTextHelper.MassStringsReadOnly(num);
				hoverTextDrawer.DrawText(array[0], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[1], this.Styles_Values.Property_Decimal.Standard);
				hoverTextDrawer.DrawText(array[2], this.Styles_Values.Property.Standard);
				hoverTextDrawer.DrawText(array[3], this.Styles_Values.Property.Standard);
				hoverTextDrawer.NewLine(26);
				hoverTextDrawer.DrawIcon(instance.GetSprite("dash"), 18);
				hoverTextDrawer.DrawText(GameUtil.GetHardnessString(Grid.Element[num], true), this.Styles_BodyText.Standard);
			}
		}
		else
		{
			hoverTextDrawer.DrawIcon(instance.GetSprite("iconWarning"), 18);
			hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.UNKNOWN, this.Styles_BodyText.Standard);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04006DF2 RID: 28146
	private DigToolHoverTextCard.HoverScreenFields hoverScreenElements;

	// Token: 0x02001B19 RID: 6937
	private struct HoverScreenFields
	{
		// Token: 0x04006DF3 RID: 28147
		public GameObject UnknownAreaLine;

		// Token: 0x04006DF4 RID: 28148
		public Image ElementStateIcon;

		// Token: 0x04006DF5 RID: 28149
		public LocText ElementCategory;

		// Token: 0x04006DF6 RID: 28150
		public LocText ElementName;

		// Token: 0x04006DF7 RID: 28151
		public LocText[] ElementMass;

		// Token: 0x04006DF8 RID: 28152
		public LocText ElementHardness;

		// Token: 0x04006DF9 RID: 28153
		public LocText ElementHardnessDescription;
	}
}
