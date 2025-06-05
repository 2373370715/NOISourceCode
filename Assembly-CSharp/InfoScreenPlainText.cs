using System;
using UnityEngine;

// Token: 0x02001D5C RID: 7516
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenPlainText")]
public class InfoScreenPlainText : KMonoBehaviour
{
	// Token: 0x06009CEB RID: 40171 RVA: 0x0010A92E File Offset: 0x00108B2E
	public void SetText(string text)
	{
		this.locText.text = text;
	}

	// Token: 0x04007AE9 RID: 31465
	[SerializeField]
	private LocText locText;
}
