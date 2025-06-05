using System;
using System.Diagnostics;

// Token: 0x02001304 RID: 4868
public class CellCallbackEvent : CellEvent
{
	// Token: 0x060063D1 RID: 25553 RVA: 0x000E59E9 File Offset: 0x000E3BE9
	public CellCallbackEvent(string id, bool is_send, bool enable_logging = true) : base(id, "Callback", is_send, enable_logging)
	{
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x002C997C File Offset: 0x002C7B7C
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, callback_id, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x002C99A8 File Offset: 0x002C7BA8
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Callback=" + cellEventInstance.data.ToString();
	}
}
