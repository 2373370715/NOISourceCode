using System;

// Token: 0x02001441 RID: 5185
public interface IFuelTank
{
	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06006A50 RID: 27216
	IStorage Storage { get; }

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06006A51 RID: 27217
	bool ConsumeFuelOnLand { get; }

	// Token: 0x06006A52 RID: 27218
	void DEBUG_FillTank();
}
