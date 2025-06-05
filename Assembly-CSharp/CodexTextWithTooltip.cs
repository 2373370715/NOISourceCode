using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CA5 RID: 7333
public class CodexTextWithTooltip : CodexWidget<CodexTextWithTooltip>
{
	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x060098E3 RID: 39139 RVA: 0x00107D68 File Offset: 0x00105F68
	// (set) Token: 0x060098E4 RID: 39140 RVA: 0x00107D70 File Offset: 0x00105F70
	public string text { get; set; }

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x060098E5 RID: 39141 RVA: 0x00107D79 File Offset: 0x00105F79
	// (set) Token: 0x060098E6 RID: 39142 RVA: 0x00107D81 File Offset: 0x00105F81
	public string tooltip { get; set; }

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x060098E7 RID: 39143 RVA: 0x00107D8A File Offset: 0x00105F8A
	// (set) Token: 0x060098E8 RID: 39144 RVA: 0x00107D92 File Offset: 0x00105F92
	public CodexTextStyle style { get; set; }

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x060098EA RID: 39146 RVA: 0x00107DAE File Offset: 0x00105FAE
	// (set) Token: 0x060098E9 RID: 39145 RVA: 0x00107D9B File Offset: 0x00105F9B
	public string stringKey
	{
		get
		{
			return "--> " + (this.text ?? "NULL");
		}
		set
		{
			this.text = Strings.Get(value);
		}
	}

	// Token: 0x060098EB RID: 39147 RVA: 0x00107DC9 File Offset: 0x00105FC9
	public CodexTextWithTooltip()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x060098EC RID: 39148 RVA: 0x00107DD8 File Offset: 0x00105FD8
	public CodexTextWithTooltip(string text, string tooltip, CodexTextStyle style = CodexTextStyle.Body)
	{
		this.text = text;
		this.style = style;
		this.tooltip = tooltip;
	}

	// Token: 0x060098ED RID: 39149 RVA: 0x003C0EA0 File Offset: 0x003BF0A0
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x060098EE RID: 39150 RVA: 0x00107DF5 File Offset: 0x00105FF5
	public void ConfigureTooltip(ToolTip tooltip)
	{
		tooltip.SetSimpleTooltip(this.tooltip);
	}

	// Token: 0x060098EF RID: 39151 RVA: 0x00107E03 File Offset: 0x00106003
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		this.ConfigureTooltip(contentGameObject.GetComponent<ToolTip>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
