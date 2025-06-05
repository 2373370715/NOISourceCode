using System;
using System.Collections.Generic;

// Token: 0x02001EB5 RID: 7861
public class NotificationAlertBar : KMonoBehaviour
{
	// Token: 0x0600A4E9 RID: 42217 RVA: 0x003F7CBC File Offset: 0x003F5EBC
	public void Init(ManagementMenuNotification notification)
	{
		this.notification = notification;
		this.thisButton.onClick += this.OnThisButtonClicked;
		this.background.colorStyleSetting = this.alertColorStyle[(int)notification.valence];
		this.background.ApplyColorStyleSetting();
		this.text.text = notification.titleText;
		this.tooltip.SetSimpleTooltip(notification.ToolTip(null, notification.tooltipData));
		this.muteButton.onClick += this.OnMuteButtonClicked;
	}

	// Token: 0x0600A4EA RID: 42218 RVA: 0x003F7D54 File Offset: 0x003F5F54
	private void OnThisButtonClicked()
	{
		NotificationHighlightController componentInParent = base.GetComponentInParent<NotificationHighlightController>();
		if (componentInParent != null)
		{
			componentInParent.SetActiveTarget(this.notification);
			return;
		}
		this.notification.View();
	}

	// Token: 0x0600A4EB RID: 42219 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnMuteButtonClicked()
	{
	}

	// Token: 0x040080FC RID: 33020
	public ManagementMenuNotification notification;

	// Token: 0x040080FD RID: 33021
	public KButton thisButton;

	// Token: 0x040080FE RID: 33022
	public KImage background;

	// Token: 0x040080FF RID: 33023
	public LocText text;

	// Token: 0x04008100 RID: 33024
	public ToolTip tooltip;

	// Token: 0x04008101 RID: 33025
	public KButton muteButton;

	// Token: 0x04008102 RID: 33026
	public List<ColorStyleSetting> alertColorStyle;
}
