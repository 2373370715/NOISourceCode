using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CA4 RID: 7332
public class CodexText : CodexWidget<CodexText>
{
	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x060098D7 RID: 39127 RVA: 0x00107CC2 File Offset: 0x00105EC2
	// (set) Token: 0x060098D8 RID: 39128 RVA: 0x00107CCA File Offset: 0x00105ECA
	public string text { get; set; }

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x060098D9 RID: 39129 RVA: 0x00107CD3 File Offset: 0x00105ED3
	// (set) Token: 0x060098DA RID: 39130 RVA: 0x00107CDB File Offset: 0x00105EDB
	public string messageID { get; set; }

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x060098DB RID: 39131 RVA: 0x00107CE4 File Offset: 0x00105EE4
	// (set) Token: 0x060098DC RID: 39132 RVA: 0x00107CEC File Offset: 0x00105EEC
	public CodexTextStyle style { get; set; }

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x060098DE RID: 39134 RVA: 0x00107D08 File Offset: 0x00105F08
	// (set) Token: 0x060098DD RID: 39133 RVA: 0x00107CF5 File Offset: 0x00105EF5
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

	// Token: 0x060098DF RID: 39135 RVA: 0x00107D23 File Offset: 0x00105F23
	public CodexText()
	{
		this.style = CodexTextStyle.Body;
	}

	// Token: 0x060098E0 RID: 39136 RVA: 0x00107D32 File Offset: 0x00105F32
	public CodexText(string text, CodexTextStyle style = CodexTextStyle.Body, string id = null)
	{
		this.text = text;
		this.style = style;
		if (id != null)
		{
			this.messageID = id;
		}
	}

	// Token: 0x060098E1 RID: 39137 RVA: 0x003C0E54 File Offset: 0x003BF054
	public void ConfigureLabel(LocText label, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		label.gameObject.SetActive(true);
		label.AllowLinks = (this.style == CodexTextStyle.Body);
		label.textStyleSetting = textStyles[this.style];
		label.text = this.text;
		label.ApplySettings();
	}

	// Token: 0x060098E2 RID: 39138 RVA: 0x00107D52 File Offset: 0x00105F52
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureLabel(contentGameObject.GetComponent<LocText>(), textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
