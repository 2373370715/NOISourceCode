using System;
using System.Diagnostics;

// Token: 0x02001306 RID: 4870
public class CellElementEvent : CellEvent
{
	// Token: 0x060063D7 RID: 25559 RVA: 0x000E5A1F File Offset: 0x000E3C1F
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x060063D8 RID: 25560 RVA: 0x002C997C File Offset: 0x002C7B7C
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060063D9 RID: 25561 RVA: 0x002C9A04 File Offset: 0x002C7C04
	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
