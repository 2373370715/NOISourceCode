using System;
using STRINGS;

// Token: 0x02001E4B RID: 7755
public class CodexUnlockedMessage : Message
{
	// Token: 0x0600A244 RID: 41540 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public CodexUnlockedMessage()
	{
	}

	// Token: 0x0600A245 RID: 41541 RVA: 0x0010DEAA File Offset: 0x0010C0AA
	public CodexUnlockedMessage(string lock_id, string unlock_message)
	{
		this.lockId = lock_id;
		this.unlockMessage = unlock_message;
	}

	// Token: 0x0600A246 RID: 41542 RVA: 0x0010DEC0 File Offset: 0x0010C0C0
	public string GetLockId()
	{
		return this.lockId;
	}

	// Token: 0x0600A247 RID: 41543 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A248 RID: 41544 RVA: 0x0010DEC8 File Offset: 0x0010C0C8
	public override string GetMessageBody()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
	}

	// Token: 0x0600A249 RID: 41545 RVA: 0x0010DEDF File Offset: 0x0010C0DF
	public override string GetTitle()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
	}

	// Token: 0x0600A24A RID: 41546 RVA: 0x0010DEEB File Offset: 0x0010C0EB
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x0600A24B RID: 41547 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x04007F37 RID: 32567
	private string unlockMessage;

	// Token: 0x04007F38 RID: 32568
	private string lockId;
}
