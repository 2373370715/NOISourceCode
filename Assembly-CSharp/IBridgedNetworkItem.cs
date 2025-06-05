using System;
using System.Collections.Generic;

// Token: 0x02000D30 RID: 3376
public interface IBridgedNetworkItem
{
	// Token: 0x06004144 RID: 16708
	void AddNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06004145 RID: 16709
	bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks);

	// Token: 0x06004146 RID: 16710
	int GetNetworkCell();
}
