using System;

// Token: 0x02001307 RID: 4871
public class CellEvent : EventBase
{
	// Token: 0x060063DA RID: 25562 RVA: 0x000E5A2C File Offset: 0x000E3C2C
	public CellEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id)
	{
		this.reason = reason;
		this.isSend = is_send;
		this.enableLogging = enable_logging;
	}

	// Token: 0x060063DB RID: 25563 RVA: 0x000E5A4B File Offset: 0x000E3C4B
	public string GetMessagePrefix()
	{
		if (this.isSend)
		{
			return ">>>: ";
		}
		return "<<<: ";
	}

	// Token: 0x04004786 RID: 18310
	public string reason;

	// Token: 0x04004787 RID: 18311
	public bool isSend;

	// Token: 0x04004788 RID: 18312
	public bool enableLogging;
}
