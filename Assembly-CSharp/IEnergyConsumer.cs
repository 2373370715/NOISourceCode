using System;

// Token: 0x020012CE RID: 4814
public interface IEnergyConsumer : ICircuitConnected
{
	// Token: 0x1700060A RID: 1546
	// (get) Token: 0x060062A2 RID: 25250
	float WattsUsed { get; }

	// Token: 0x1700060B RID: 1547
	// (get) Token: 0x060062A3 RID: 25251
	float WattsNeededWhenActive { get; }

	// Token: 0x1700060C RID: 1548
	// (get) Token: 0x060062A4 RID: 25252
	int PowerSortOrder { get; }

	// Token: 0x060062A5 RID: 25253
	void SetConnectionStatus(CircuitManager.ConnectionStatus status);

	// Token: 0x1700060D RID: 1549
	// (get) Token: 0x060062A6 RID: 25254
	string Name { get; }

	// Token: 0x1700060E RID: 1550
	// (get) Token: 0x060062A7 RID: 25255
	bool IsConnected { get; }

	// Token: 0x1700060F RID: 1551
	// (get) Token: 0x060062A8 RID: 25256
	bool IsPowered { get; }
}
