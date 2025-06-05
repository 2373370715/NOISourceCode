using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001CA0 RID: 7328
public interface ICodexWidget
{
	// Token: 0x060098BE RID: 39102
	void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles);
}
