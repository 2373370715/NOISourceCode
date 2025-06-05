using System;
using System.Diagnostics;

// Token: 0x0200130C RID: 4876
public class CellSolidFilterEvent : CellEvent
{
	// Token: 0x060063E8 RID: 25576 RVA: 0x000E5AAD File Offset: 0x000E3CAD
	public CellSolidFilterEvent(string id, bool enable_logging = true) : base(id, "filtered", false, enable_logging)
	{
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x002CA278 File Offset: 0x002C8478
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

	// Token: 0x060063EA RID: 25578 RVA: 0x002CA300 File Offset: 0x002C8500
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Filtered Solid Event solid=" + cellEventInstance.data.ToString();
	}
}
