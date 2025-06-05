using System;

// Token: 0x02001086 RID: 4230
public interface IDisconnectable
{
	// Token: 0x060055E7 RID: 21991
	bool Connect();

	// Token: 0x060055E8 RID: 21992
	void Disconnect();

	// Token: 0x060055E9 RID: 21993
	bool IsDisconnected();
}
