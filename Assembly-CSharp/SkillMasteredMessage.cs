using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E5B RID: 7771
public class SkillMasteredMessage : Message
{
	// Token: 0x0600A2C6 RID: 41670 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public SkillMasteredMessage()
	{
	}

	// Token: 0x0600A2C7 RID: 41671 RVA: 0x0010E455 File Offset: 0x0010C655
	public SkillMasteredMessage(MinionResume resume)
	{
		this.minionName = resume.GetProperName();
	}

	// Token: 0x0600A2C8 RID: 41672 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A2C9 RID: 41673 RVA: 0x003EE3C8 File Offset: 0x003EC5C8
	public override string GetMessageBody()
	{
		Debug.Assert(this.minionName != null);
		string arg = string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.LINE, this.minionName);
		return string.Format(MISC.NOTIFICATIONS.SKILL_POINT_EARNED.MESSAGEBODY, arg);
	}

	// Token: 0x0600A2CA RID: 41674 RVA: 0x0010E469 File Offset: 0x0010C669
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600A2CB RID: 41675 RVA: 0x0010E480 File Offset: 0x0010C680
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}", this.minionName);
	}

	// Token: 0x0600A2CC RID: 41676 RVA: 0x0010E497 File Offset: 0x0010C697
	public override bool IsValid()
	{
		return this.minionName != null;
	}

	// Token: 0x04007F5F RID: 32607
	[Serialize]
	private string minionName;
}
