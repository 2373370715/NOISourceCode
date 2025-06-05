using System;
using KSerialization;

// Token: 0x02001E5F RID: 7775
public class TutorialMessage : GenericMessage, IHasDlcRestrictions
{
	// Token: 0x0600A2DA RID: 41690 RVA: 0x0010E54F File Offset: 0x0010C74F
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x0600A2DB RID: 41691 RVA: 0x0010E557 File Offset: 0x0010C757
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x0600A2DC RID: 41692 RVA: 0x0010E55F File Offset: 0x0010C75F
	public TutorialMessage()
	{
	}

	// Token: 0x0600A2DD RID: 41693 RVA: 0x003EE43C File Offset: 0x003EC63C
	public TutorialMessage(Tutorial.TutorialMessages messageId, string title, string body, string tooltip, string videoClipId = null, string videoOverlayName = null, string videoTitleText = null, string icon = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null) : base(title, body, tooltip, null)
	{
		this.messageId = messageId;
		this.videoClipId = videoClipId;
		this.videoOverlayName = videoOverlayName;
		this.videoTitleText = videoTitleText;
		this.icon = icon;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x04007F65 RID: 32613
	[Serialize]
	public Tutorial.TutorialMessages messageId;

	// Token: 0x04007F66 RID: 32614
	public string videoClipId;

	// Token: 0x04007F67 RID: 32615
	public string videoOverlayName;

	// Token: 0x04007F68 RID: 32616
	public string videoTitleText;

	// Token: 0x04007F69 RID: 32617
	public string icon;

	// Token: 0x04007F6A RID: 32618
	public string[] requiredDlcIds;

	// Token: 0x04007F6B RID: 32619
	public string[] forbiddenDlcIds;
}
