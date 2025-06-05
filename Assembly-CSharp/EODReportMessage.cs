using System;
using KSerialization;

// Token: 0x02001E4F RID: 7759
public class EODReportMessage : Message
{
	// Token: 0x0600A261 RID: 41569 RVA: 0x0010E001 File Offset: 0x0010C201
	public EODReportMessage(string title, string tooltip)
	{
		this.day = GameUtil.GetCurrentCycle();
		this.title = title;
		this.tooltip = tooltip;
	}

	// Token: 0x0600A262 RID: 41570 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public EODReportMessage()
	{
	}

	// Token: 0x0600A263 RID: 41571 RVA: 0x000AA765 File Offset: 0x000A8965
	public override string GetSound()
	{
		return null;
	}

	// Token: 0x0600A264 RID: 41572 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public override string GetMessageBody()
	{
		return "";
	}

	// Token: 0x0600A265 RID: 41573 RVA: 0x0010E022 File Offset: 0x0010C222
	public override string GetTooltip()
	{
		return this.tooltip;
	}

	// Token: 0x0600A266 RID: 41574 RVA: 0x0010E02A File Offset: 0x0010C22A
	public override string GetTitle()
	{
		return this.title;
	}

	// Token: 0x0600A267 RID: 41575 RVA: 0x0010E032 File Offset: 0x0010C232
	public void OpenReport()
	{
		ManagementMenu.Instance.OpenReports(this.day);
	}

	// Token: 0x0600A268 RID: 41576 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool ShowDialog()
	{
		return false;
	}

	// Token: 0x0600A269 RID: 41577 RVA: 0x0010E044 File Offset: 0x0010C244
	public override void OnClick()
	{
		this.OpenReport();
	}

	// Token: 0x04007F3C RID: 32572
	[Serialize]
	private int day;

	// Token: 0x04007F3D RID: 32573
	[Serialize]
	private string title;

	// Token: 0x04007F3E RID: 32574
	[Serialize]
	private string tooltip;
}
