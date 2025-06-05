using System;

// Token: 0x020014E9 RID: 5353
public interface ILogicEventReceiver : ILogicNetworkConnection
{
	// Token: 0x06006F37 RID: 28471
	void ReceiveLogicEvent(int value);

	// Token: 0x06006F38 RID: 28472
	int GetLogicCell();
}
