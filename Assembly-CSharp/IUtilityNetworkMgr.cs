using System;
using System.Collections.Generic;

// Token: 0x02001A6D RID: 6765
public interface IUtilityNetworkMgr
{
	// Token: 0x06008CEE RID: 36078
	bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason);

	// Token: 0x06008CEF RID: 36079
	void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building);

	// Token: 0x06008CF0 RID: 36080
	void StashVisualGrids();

	// Token: 0x06008CF1 RID: 36081
	void UnstashVisualGrids();

	// Token: 0x06008CF2 RID: 36082
	string GetVisualizerString(int cell);

	// Token: 0x06008CF3 RID: 36083
	string GetVisualizerString(UtilityConnections connections);

	// Token: 0x06008CF4 RID: 36084
	UtilityConnections GetConnections(int cell, bool is_physical_building);

	// Token: 0x06008CF5 RID: 36085
	UtilityConnections GetDisplayConnections(int cell);

	// Token: 0x06008CF6 RID: 36086
	void SetConnections(UtilityConnections connections, int cell, bool is_physical_building);

	// Token: 0x06008CF7 RID: 36087
	void ClearCell(int cell, bool is_physical_building);

	// Token: 0x06008CF8 RID: 36088
	void ForceRebuildNetworks();

	// Token: 0x06008CF9 RID: 36089
	void AddToNetworks(int cell, object item, bool is_endpoint);

	// Token: 0x06008CFA RID: 36090
	void RemoveFromNetworks(int cell, object vent, bool is_endpoint);

	// Token: 0x06008CFB RID: 36091
	object GetEndpoint(int cell);

	// Token: 0x06008CFC RID: 36092
	UtilityNetwork GetNetworkForDirection(int cell, Direction direction);

	// Token: 0x06008CFD RID: 36093
	UtilityNetwork GetNetworkForCell(int cell);

	// Token: 0x06008CFE RID: 36094
	void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06008CFF RID: 36095
	void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener);

	// Token: 0x06008D00 RID: 36096
	IList<UtilityNetwork> GetNetworks();
}
