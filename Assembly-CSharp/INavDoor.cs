using System;

// Token: 0x02000D78 RID: 3448
public interface INavDoor
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060042E4 RID: 17124
	bool isSpawned { get; }

	// Token: 0x060042E5 RID: 17125
	bool IsOpen();

	// Token: 0x060042E6 RID: 17126
	void Open();

	// Token: 0x060042E7 RID: 17127
	void Close();
}
