using System;
using KSerialization;

// Token: 0x02001E52 RID: 7762
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
	// Token: 0x0600A278 RID: 41592
	public abstract string GetTitle();

	// Token: 0x0600A279 RID: 41593
	public abstract string GetSound();

	// Token: 0x0600A27A RID: 41594
	public abstract string GetMessageBody();

	// Token: 0x0600A27B RID: 41595
	public abstract string GetTooltip();

	// Token: 0x0600A27C RID: 41596 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool ShowDialog()
	{
		return true;
	}

	// Token: 0x0600A27D RID: 41597 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnCleanUp()
	{
	}

	// Token: 0x0600A27E RID: 41598 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A27F RID: 41599 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool PlayNotificationSound()
	{
		return true;
	}

	// Token: 0x0600A280 RID: 41600 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnClick()
	{
	}

	// Token: 0x0600A281 RID: 41601 RVA: 0x000B17B4 File Offset: 0x000AF9B4
	public virtual NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x0600A282 RID: 41602 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool ShowDismissButton()
	{
		return true;
	}
}
