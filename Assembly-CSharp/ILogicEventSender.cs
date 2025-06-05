using System;

// Token: 0x020014E8 RID: 5352
public interface ILogicEventSender : ILogicNetworkConnection
{
	// Token: 0x06006F34 RID: 28468
	void LogicTick();

	// Token: 0x06006F35 RID: 28469
	int GetLogicCell();

	// Token: 0x06006F36 RID: 28470
	int GetLogicValue();
}
