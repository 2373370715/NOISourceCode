using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002098 RID: 8344
[AddComponentMenu("KMonoBehaviour/scripts/TitleBar")]
public class TitleBar : KMonoBehaviour
{
	// Token: 0x0600B1F5 RID: 45557 RVA: 0x00118390 File Offset: 0x00116590
	public void SetTitle(string Name)
	{
		this.titleText.text = Name;
	}

	// Token: 0x0600B1F6 RID: 45558 RVA: 0x0011839E File Offset: 0x0011659E
	public void SetSubText(string subtext, string tooltip = "")
	{
		this.subtextText.text = subtext;
		this.subtextText.GetComponent<ToolTip>().toolTip = tooltip;
	}

	// Token: 0x0600B1F7 RID: 45559 RVA: 0x001183BD File Offset: 0x001165BD
	public void SetWarningActve(bool state)
	{
		this.WarningNotification.SetActive(state);
	}

	// Token: 0x0600B1F8 RID: 45560 RVA: 0x001183CB File Offset: 0x001165CB
	public void SetWarning(Sprite icon, string label)
	{
		this.SetWarningActve(true);
		this.NotificationIcon.sprite = icon;
		this.NotificationText.text = label;
	}

	// Token: 0x0600B1F9 RID: 45561 RVA: 0x001183EC File Offset: 0x001165EC
	public void SetPortrait(GameObject target)
	{
		this.portrait.SetPortrait(target);
	}

	// Token: 0x04008C56 RID: 35926
	public LocText titleText;

	// Token: 0x04008C57 RID: 35927
	public LocText subtextText;

	// Token: 0x04008C58 RID: 35928
	public GameObject WarningNotification;

	// Token: 0x04008C59 RID: 35929
	public Text NotificationText;

	// Token: 0x04008C5A RID: 35930
	public Image NotificationIcon;

	// Token: 0x04008C5B RID: 35931
	public Sprite techIcon;

	// Token: 0x04008C5C RID: 35932
	public Sprite materialIcon;

	// Token: 0x04008C5D RID: 35933
	public TitleBarPortrait portrait;

	// Token: 0x04008C5E RID: 35934
	public bool userEditable;

	// Token: 0x04008C5F RID: 35935
	public bool setCameraControllerState = true;
}
