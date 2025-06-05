using System;

// Token: 0x02001A61 RID: 6753
public interface IUtilityItem
{
	// Token: 0x17000928 RID: 2344
	// (get) Token: 0x06008CAE RID: 36014
	// (set) Token: 0x06008CAF RID: 36015
	UtilityConnections Connections { get; set; }

	// Token: 0x06008CB0 RID: 36016
	void UpdateConnections(UtilityConnections Connections);

	// Token: 0x06008CB1 RID: 36017
	int GetNetworkID();

	// Token: 0x06008CB2 RID: 36018
	UtilityNetwork GetNetworkForDirection(Direction d);
}
