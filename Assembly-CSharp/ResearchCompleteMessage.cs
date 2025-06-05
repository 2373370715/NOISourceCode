using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E59 RID: 7769
public class ResearchCompleteMessage : Message
{
	// Token: 0x0600A2B4 RID: 41652 RVA: 0x0010E381 File Offset: 0x0010C581
	public ResearchCompleteMessage()
	{
	}

	// Token: 0x0600A2B5 RID: 41653 RVA: 0x0010E394 File Offset: 0x0010C594
	public ResearchCompleteMessage(Tech tech)
	{
		this.tech.Set(tech);
	}

	// Token: 0x0600A2B6 RID: 41654 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A2B7 RID: 41655 RVA: 0x003EE0F4 File Offset: 0x003EC2F4
	public override string GetMessageBody()
	{
		Tech tech = this.tech.Get();
		string text = "";
		for (int i = 0; i < tech.unlockedItems.Count; i++)
		{
			if (i != 0)
			{
				text += ", ";
			}
			text += tech.unlockedItems[i].Name;
		}
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.MESSAGEBODY, tech.Name, text);
	}

	// Token: 0x0600A2B8 RID: 41656 RVA: 0x0010E3B3 File Offset: 0x0010C5B3
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.RESEARCHCOMPLETE.NAME;
	}

	// Token: 0x0600A2B9 RID: 41657 RVA: 0x003EE168 File Offset: 0x003EC368
	public override string GetTooltip()
	{
		Tech tech = this.tech.Get();
		return string.Format(MISC.NOTIFICATIONS.RESEARCHCOMPLETE.TOOLTIP, tech.Name);
	}

	// Token: 0x0600A2BA RID: 41658 RVA: 0x0010E3BF File Offset: 0x0010C5BF
	public override bool IsValid()
	{
		return this.tech.Get() != null;
	}

	// Token: 0x04007F5C RID: 32604
	[Serialize]
	private ResourceRef<Tech> tech = new ResourceRef<Tech>();
}
