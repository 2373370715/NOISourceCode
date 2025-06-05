using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CAC RID: 7340
public class CodexLabelWithIcon : CodexWidget<CodexLabelWithIcon>
{
	// Token: 0x17000A14 RID: 2580
	// (get) Token: 0x06009911 RID: 39185 RVA: 0x00107FBF File Offset: 0x001061BF
	// (set) Token: 0x06009912 RID: 39186 RVA: 0x00107FC7 File Offset: 0x001061C7
	public CodexImage icon { get; set; }

	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x06009913 RID: 39187 RVA: 0x00107FD0 File Offset: 0x001061D0
	// (set) Token: 0x06009914 RID: 39188 RVA: 0x00107FD8 File Offset: 0x001061D8
	public CodexText label { get; set; }

	// Token: 0x06009915 RID: 39189 RVA: 0x00107FE1 File Offset: 0x001061E1
	public CodexLabelWithIcon()
	{
	}

	// Token: 0x06009916 RID: 39190 RVA: 0x00107FE9 File Offset: 0x001061E9
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite)
	{
		this.icon = new CodexImage(coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009917 RID: 39191 RVA: 0x0010800B File Offset: 0x0010620B
	public CodexLabelWithIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, int iconWidth, int iconHeight)
	{
		this.icon = new CodexImage(iconWidth, iconHeight, coloredSprite);
		this.label = new CodexText(text, style, null);
	}

	// Token: 0x06009918 RID: 39192 RVA: 0x003C1028 File Offset: 0x003BF228
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.icon.ConfigureImage(contentGameObject.GetComponentInChildren<Image>());
		if (this.icon.preferredWidth != -1 && this.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentInChildren<Image>().GetComponent<LayoutElement>();
			component.minWidth = (float)this.icon.preferredHeight;
			component.minHeight = (float)this.icon.preferredWidth;
			component.preferredHeight = (float)this.icon.preferredHeight;
			component.preferredWidth = (float)this.icon.preferredWidth;
		}
		this.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
	}
}
