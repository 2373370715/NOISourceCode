using System;
using System.Diagnostics;

// Token: 0x02001303 RID: 4867
public class CellAddRemoveSubstanceEvent : CellEvent
{
	// Token: 0x060063CE RID: 25550 RVA: 0x000E59DD File Offset: 0x000E3BDD
	public CellAddRemoveSubstanceEvent(string id, string reason, bool enable_logging = false) : base(id, reason, true, enable_logging)
	{
	}

	// Token: 0x060063CF RID: 25551 RVA: 0x002C98C8 File Offset: 0x002C7AC8
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, float amount, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, (int)(amount * 1000f), this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x002C98FC File Offset: 0x002C7AFC
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		SimHashes data = (SimHashes)cellEventInstance.data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			", Mass=",
			((float)cellEventInstance.data2 / 1000f).ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
