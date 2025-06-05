using System;
using STRINGS;

// Token: 0x02001E4E RID: 7758
public class DuplicantsLeftMessage : Message
{
	// Token: 0x0600A25C RID: 41564 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A25D RID: 41565 RVA: 0x0010DFDD File Offset: 0x0010C1DD
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.NAME;
	}

	// Token: 0x0600A25E RID: 41566 RVA: 0x0010DFE9 File Offset: 0x0010C1E9
	public override string GetMessageBody()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.MESSAGEBODY;
	}

	// Token: 0x0600A25F RID: 41567 RVA: 0x0010DFF5 File Offset: 0x0010C1F5
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.DUPLICANTABSORBED.TOOLTIP;
	}
}
