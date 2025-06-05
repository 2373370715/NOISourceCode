using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CA1 RID: 7329
public abstract class CodexWidget<SubClass> : ICodexWidget
{
	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x060098BF RID: 39103 RVA: 0x00107BB4 File Offset: 0x00105DB4
	// (set) Token: 0x060098C0 RID: 39104 RVA: 0x00107BBC File Offset: 0x00105DBC
	public int preferredWidth { get; set; }

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x060098C1 RID: 39105 RVA: 0x00107BC5 File Offset: 0x00105DC5
	// (set) Token: 0x060098C2 RID: 39106 RVA: 0x00107BCD File Offset: 0x00105DCD
	public int preferredHeight { get; set; }

	// Token: 0x060098C3 RID: 39107 RVA: 0x00107BD6 File Offset: 0x00105DD6
	protected CodexWidget()
	{
		this.preferredWidth = -1;
		this.preferredHeight = -1;
	}

	// Token: 0x060098C4 RID: 39108 RVA: 0x00107BEC File Offset: 0x00105DEC
	protected CodexWidget(int preferredWidth, int preferredHeight)
	{
		this.preferredWidth = preferredWidth;
		this.preferredHeight = preferredHeight;
	}

	// Token: 0x060098C5 RID: 39109
	public abstract void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);

	// Token: 0x060098C6 RID: 39110 RVA: 0x00107C02 File Offset: 0x00105E02
	protected void ConfigurePreferredLayout(GameObject contentGameObject)
	{
		LayoutElement componentInChildren = contentGameObject.GetComponentInChildren<LayoutElement>();
		componentInChildren.preferredHeight = (float)this.preferredHeight;
		componentInChildren.preferredWidth = (float)this.preferredWidth;
	}
}
