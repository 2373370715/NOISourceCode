using System;
using UnityEngine;

// Token: 0x02001D5B RID: 7515
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenLineItem")]
public class InfoScreenLineItem : KMonoBehaviour
{
	// Token: 0x06009CE8 RID: 40168 RVA: 0x0010A912 File Offset: 0x00108B12
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x06009CE9 RID: 40169 RVA: 0x0010A920 File Offset: 0x00108B20
	public void SetTooltip(string tooltip)
	{
		this.toolTip.toolTip = tooltip;
	}

	// Token: 0x04007AE5 RID: 31461
	[SerializeField]
	private LocText locText;

	// Token: 0x04007AE6 RID: 31462
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x04007AE7 RID: 31463
	private string text;

	// Token: 0x04007AE8 RID: 31464
	private string tooltip;
}
