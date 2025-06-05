using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CA7 RID: 7335
public class CodexDividerLine : CodexWidget<CodexDividerLine>
{
	// Token: 0x06009901 RID: 39169 RVA: 0x00107F12 File Offset: 0x00106112
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		contentGameObject.GetComponent<LayoutElement>().minWidth = 530f;
	}
}
