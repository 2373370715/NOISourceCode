using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020020B3 RID: 8371
[AddComponentMenu("KMonoBehaviour/scripts/VideoOverlay")]
public class VideoOverlay : KMonoBehaviour
{
	// Token: 0x0600B283 RID: 45699 RVA: 0x0043D7CC File Offset: 0x0043B9CC
	public void SetText(List<string> strings)
	{
		if (strings.Count != this.textFields.Count)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				base.name,
				"expects",
				this.textFields.Count,
				"strings passed to it, got",
				strings.Count
			});
		}
		for (int i = 0; i < this.textFields.Count; i++)
		{
			this.textFields[i].text = strings[i];
		}
	}

	// Token: 0x04008CE8 RID: 36072
	public List<LocText> textFields;
}
