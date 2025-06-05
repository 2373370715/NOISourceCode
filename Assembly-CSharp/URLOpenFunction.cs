using System;
using UnityEngine;

// Token: 0x020020AF RID: 8367
public class URLOpenFunction : MonoBehaviour
{
	// Token: 0x0600B26E RID: 45678 RVA: 0x00118861 File Offset: 0x00116A61
	private void Start()
	{
		if (this.triggerButton != null)
		{
			this.triggerButton.ClearOnClick();
			this.triggerButton.onClick += delegate()
			{
				this.OpenUrl(this.fixedURL);
			};
		}
	}

	// Token: 0x0600B26F RID: 45679 RVA: 0x00118893 File Offset: 0x00116A93
	public void OpenUrl(string url)
	{
		if (url == "blueprints")
		{
			if (LockerMenuScreen.Instance != null)
			{
				LockerMenuScreen.Instance.ShowInventoryScreen();
				return;
			}
		}
		else
		{
			App.OpenWebURL(url);
		}
	}

	// Token: 0x0600B270 RID: 45680 RVA: 0x001188C0 File Offset: 0x00116AC0
	public void SetURL(string url)
	{
		this.fixedURL = url;
	}

	// Token: 0x04008CD5 RID: 36053
	[SerializeField]
	private KButton triggerButton;

	// Token: 0x04008CD6 RID: 36054
	[SerializeField]
	private string fixedURL;
}
