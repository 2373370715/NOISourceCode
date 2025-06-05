using System;
using STRINGS;

// Token: 0x02001E49 RID: 7753
public class AchievementEarnedMessage : Message
{
	// Token: 0x0600A237 RID: 41527 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x0600A238 RID: 41528 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A239 RID: 41529 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x0600A23A RID: 41530 RVA: 0x0010DE0E File Offset: 0x0010C00E
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.NAME;
	}

	// Token: 0x0600A23B RID: 41531 RVA: 0x0010DE1A File Offset: 0x0010C01A
	public override string GetTooltip()
	{
		return MISC.NOTIFICATIONS.COLONY_ACHIEVEMENT_EARNED.TOOLTIP;
	}

	// Token: 0x0600A23C RID: 41532 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A23D RID: 41533 RVA: 0x0010DE26 File Offset: 0x0010C026
	public override void OnClick()
	{
		RetireColonyUtility.SaveColonySummaryData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
	}
}
