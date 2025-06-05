using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001EB4 RID: 7860
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
	// Token: 0x0600A4E4 RID: 42212 RVA: 0x000AA038 File Offset: 0x000A8238
	protected void OnEnable()
	{
	}

	// Token: 0x0600A4E5 RID: 42213 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnDisable()
	{
	}

	// Token: 0x0600A4E6 RID: 42214 RVA: 0x0010F66D File Offset: 0x0010D86D
	private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
	{
		NotificationAlertBar notificationAlertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject, false);
		notificationAlertBar.Init(notification);
		notificationAlertBar.gameObject.SetActive(true);
		return notificationAlertBar;
	}

	// Token: 0x0600A4E7 RID: 42215 RVA: 0x000AA038 File Offset: 0x000A8238
	private void NotificationsChanged()
	{
	}

	// Token: 0x040080F8 RID: 33016
	public global::Action currentMenu;

	// Token: 0x040080F9 RID: 33017
	public NotificationAlertBar alertBarPrefab;

	// Token: 0x040080FA RID: 33018
	public RectTransform alertContainer;

	// Token: 0x040080FB RID: 33019
	private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();
}
