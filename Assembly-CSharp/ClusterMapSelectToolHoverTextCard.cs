using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001B0B RID: 6923
public class ClusterMapSelectToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06009118 RID: 37144 RVA: 0x0038B518 File Offset: 0x00389718
	public override void ConfigureHoverScreen()
	{
		base.ConfigureHoverScreen();
		HoverTextScreen instance = HoverTextScreen.Instance;
		this.m_iconWarning = instance.GetSprite("iconWarning");
		this.m_iconDash = instance.GetSprite("dash");
		this.m_iconHighlighted = instance.GetSprite("dash_arrow");
	}

	// Token: 0x06009119 RID: 37145 RVA: 0x0038B564 File Offset: 0x00389764
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		if (this.m_iconWarning == null)
		{
			this.ConfigureHoverScreen();
		}
		HoverTextDrawer hoverTextDrawer = HoverTextScreen.Instance.BeginDrawing();
		foreach (KSelectable kselectable in hoverObjects)
		{
			hoverTextDrawer.BeginShadowBar(ClusterMapSelectTool.Instance.GetSelected() == kselectable);
			string unitFormattedName = GameUtil.GetUnitFormattedName(kselectable.gameObject, true);
			hoverTextDrawer.DrawText(unitFormattedName, this.Styles_Title.Standard);
			foreach (StatusItemGroup.Entry entry in kselectable.GetStatusItemGroup())
			{
				if (entry.category != null && entry.category.Id == "Main")
				{
					TextStyleSetting style = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon = (entry.item.sprite != null) ? entry.item.sprite.sprite : this.m_iconWarning;
					Color color = this.IsStatusItemWarning(entry) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon, color, 18, 2);
					hoverTextDrawer.DrawText(entry.GetName(), style);
				}
			}
			foreach (StatusItemGroup.Entry entry2 in kselectable.GetStatusItemGroup())
			{
				if (entry2.category == null || entry2.category.Id != "Main")
				{
					TextStyleSetting style2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard : this.Styles_BodyText.Standard;
					Sprite icon2 = (entry2.item.sprite != null) ? entry2.item.sprite.sprite : this.m_iconWarning;
					Color color2 = this.IsStatusItemWarning(entry2) ? this.Styles_Warning.Standard.textColor : this.Styles_BodyText.Standard.textColor;
					hoverTextDrawer.NewLine(26);
					hoverTextDrawer.DrawIcon(icon2, color2, 18, 2);
					hoverTextDrawer.DrawText(entry2.GetName(), style2);
				}
			}
			hoverTextDrawer.EndShadowBar();
		}
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x0600911A RID: 37146 RVA: 0x001033B1 File Offset: 0x001015B1
	private bool IsStatusItemWarning(StatusItemGroup.Entry item)
	{
		return item.item.notificationType == NotificationType.Bad || item.item.notificationType == NotificationType.BadMinor || item.item.notificationType == NotificationType.DuplicantThreatening;
	}

	// Token: 0x04006DB3 RID: 28083
	private Sprite m_iconWarning;

	// Token: 0x04006DB4 RID: 28084
	private Sprite m_iconDash;

	// Token: 0x04006DB5 RID: 28085
	private Sprite m_iconHighlighted;
}
