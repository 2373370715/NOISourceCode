using System;
using System.Collections.Generic;

// Token: 0x02000AC0 RID: 2752
public class MessageNotification : Notification
{
	// Token: 0x0600324A RID: 12874 RVA: 0x000ACCFB File Offset: 0x000AAEFB
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x00210294 File Offset: 0x0020E494
	public MessageNotification(Message m) : base(m.GetTitle(), NotificationType.Messages, null, null, false, 0f, null, null, null, true, false, true)
	{
		MessageNotification <>4__this = this;
		this.message = m;
		base.Type = m.GetMessageType();
		this.showDismissButton = m.ShowDismissButton();
		if (!this.message.PlayNotificationSound())
		{
			this.playSound = false;
		}
		base.ToolTip = ((List<Notification> notifications, object data) => <>4__this.OnToolTip(notifications, m.GetTooltip()));
		base.clickFocus = null;
	}

	// Token: 0x04002268 RID: 8808
	public Message message;
}
