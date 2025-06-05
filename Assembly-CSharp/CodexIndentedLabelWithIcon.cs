using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CA9 RID: 7337
public class CodexIndentedLabelWithIcon : CodexWidget<CodexIndentedLabelWithIcon>
{
	// Token: 0x17000A12 RID: 2578
	// (get) Token: 0x06009905 RID: 39173 RVA: 0x00107F34 File Offset: 0x00106134
	// (set) Token: 0x06009906 RID: 39174 RVA: 0x00107F3C File Offset: 0x0010613C
	public CodexImage icon { get; set; }

	// Token: 0x17000A13 RID: 2579
	// (get) Token: 0x06009907 RID: 39175 RVA: 0x00107F45 File Offset: 0x00106145
	// (set) Token: 0x06009908 RID: 39176 RVA: 0x00107F4D File Offset: 0x0010614D
	public CodexText label { get; set; }

	// Token: 0x06009909 RID: 39177 RVA: 0x00107F56 File Offset: 0x00106156
	public CodexIndentedLabelWithIcon()
	{
	}

	// Token: 0x0600990A RID: 39178 RVA: 0x00107F5E File Offset: 0x0010615E
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x0600990B RID: 39179 RVA: 0x00107F80 File Offset: 0x00106180
	public CodexIndentedLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x0600990C RID: 39180 RVA: 0x003C0F8C File Offset: 0x003BF18C
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Image componentInChildren = contentGameObject.GetComponentInChildren<Image>();
		this.icon.ConfigureImage(componentInChildren);
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
	}
}
