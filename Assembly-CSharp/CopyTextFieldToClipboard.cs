using System;
using UnityEngine;

// Token: 0x020020C0 RID: 8384
[AddComponentMenu("KMonoBehaviour/scripts/CopyTextFieldToClipboard")]
public class CopyTextFieldToClipboard : KMonoBehaviour
{
	// Token: 0x0600B2D3 RID: 45779 RVA: 0x00118C78 File Offset: 0x00116E78
	protected override void OnPrefabInit()
	{
		this.button.onClick += this.OnClick;
	}

	// Token: 0x0600B2D4 RID: 45780 RVA: 0x00118C91 File Offset: 0x00116E91
	private void OnClick()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = this.GetText();
		textEditor.SelectAll();
		textEditor.Copy();
	}

	// Token: 0x04008D3E RID: 36158
	public KButton button;

	// Token: 0x04008D3F RID: 36159
	public Func<string> GetText;
}
