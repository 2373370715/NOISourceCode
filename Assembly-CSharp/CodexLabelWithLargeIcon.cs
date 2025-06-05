using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CAD RID: 7341
public class CodexLabelWithLargeIcon : CodexLabelWithIcon
{
	// Token: 0x17000A16 RID: 2582
	// (get) Token: 0x06009919 RID: 39193 RVA: 0x00108031 File Offset: 0x00106231
	// (set) Token: 0x0600991A RID: 39194 RVA: 0x00108039 File Offset: 0x00106239
	public string linkID { get; set; }

	// Token: 0x0600991B RID: 39195 RVA: 0x00108042 File Offset: 0x00106242
	public CodexLabelWithLargeIcon()
	{
	}

	// Token: 0x0600991C RID: 39196 RVA: 0x003C10C8 File Offset: 0x003BF2C8
	public CodexLabelWithLargeIcon(string text, CodexTextStyle style, global::Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID) : base(text, style, coloredSprite, 128, 128)
	{
		base.icon = new CodexImage(128, 128, coloredSprite);
		base.label = new CodexText(text, style, null);
		this.linkID = targetEntrylinkID;
	}

	// Token: 0x0600991D RID: 39197 RVA: 0x003C1114 File Offset: 0x003BF314
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.icon.ConfigureImage(contentGameObject.GetComponentsInChildren<Image>()[1]);
		if (base.icon.preferredWidth != -1 && base.icon.preferredHeight != -1)
		{
			LayoutElement component = contentGameObject.GetComponentsInChildren<Image>()[1].GetComponent<LayoutElement>();
			component.minWidth = (float)base.icon.preferredHeight;
			component.minHeight = (float)base.icon.preferredWidth;
			component.preferredHeight = (float)base.icon.preferredHeight;
			component.preferredWidth = (float)base.icon.preferredWidth;
		}
		base.label.text = UI.StripLinkFormatting(base.label.text);
		base.label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);
		contentGameObject.GetComponent<KButton>().ClearOnClick();
		contentGameObject.GetComponent<KButton>().onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(this.linkID, false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}
}
