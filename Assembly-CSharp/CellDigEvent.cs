using System;
using System.Diagnostics;

// Token: 0x02001305 RID: 4869
public class CellDigEvent : CellEvent
{
	// Token: 0x060063D4 RID: 25556 RVA: 0x000E59F9 File Offset: 0x000E3BF9
	public CellDigEvent(bool enable_logging = true) : base("Dig", "Dig", true, enable_logging)
	{
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x002C99D8 File Offset: 0x002C7BD8
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060063D6 RID: 25558 RVA: 0x000E5A0D File Offset: 0x000E3C0D
	public override string GetDescription(EventInstanceBase ev)
	{
		return base.GetMessagePrefix() + "Dig=true";
	}
}
