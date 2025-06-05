using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001D87 RID: 7559
public class KPopupMenu : KScreen
{
	// Token: 0x06009DD8 RID: 40408 RVA: 0x003D907C File Offset: 0x003D727C
	public void SetOptions(IList<string> options)
	{
		List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
		for (int i = 0; i < options.Count; i++)
		{
			int index = i;
			string option = options[i];
			list.Add(new KButtonMenu.ButtonInfo(option, global::Action.NumActions, delegate()
			{
				this.SelectOption(option, index);
			}, null, null));
		}
		this.Buttons = list.ToArray();
	}

	// Token: 0x06009DD9 RID: 40409 RVA: 0x003D90F4 File Offset: 0x003D72F4
	public void OnClick()
	{
		if (this.Buttons != null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.buttonMenu.SetButtons(this.Buttons);
			this.buttonMenu.RefreshButtons();
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06009DDA RID: 40410 RVA: 0x0010B20B File Offset: 0x0010940B
	public void SelectOption(string option, int index)
	{
		if (this.OnSelect != null)
		{
			this.OnSelect(option, index);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009DDB RID: 40411 RVA: 0x0010B22E File Offset: 0x0010942E
	public IList<KButtonMenu.ButtonInfo> GetButtons()
	{
		return this.Buttons;
	}

	// Token: 0x04007BF6 RID: 31734
	[SerializeField]
	private KButtonMenu buttonMenu;

	// Token: 0x04007BF7 RID: 31735
	private KButtonMenu.ButtonInfo[] Buttons;

	// Token: 0x04007BF8 RID: 31736
	public Action<string, int> OnSelect;
}
