using System;
using System.Diagnostics;

// Token: 0x020016DC RID: 5852
[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
	// Token: 0x060078B5 RID: 30901 RVA: 0x000E54FF File Offset: 0x000E36FF
	public OwnableSlotInstance(Assignables assignables, OwnableSlot slot) : base(assignables, slot)
	{
	}
}
