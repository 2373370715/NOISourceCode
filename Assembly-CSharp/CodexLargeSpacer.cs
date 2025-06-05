using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CAA RID: 7338
public class CodexLargeSpacer : CodexWidget<CodexLargeSpacer>
{
	// Token: 0x0600990D RID: 39181 RVA: 0x00107FA6 File Offset: 0x001061A6
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
