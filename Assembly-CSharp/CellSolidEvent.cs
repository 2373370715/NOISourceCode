using System;
using System.Diagnostics;

// Token: 0x0200130B RID: 4875
public class CellSolidEvent : CellEvent
{
	// Token: 0x060063E5 RID: 25573 RVA: 0x000E5A1F File Offset: 0x000E3C1F
	public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x002CA278 File Offset: 0x002C8478
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, bool solid)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x002CA2AC File Offset: 0x002C84AC
	public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}
