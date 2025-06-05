using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001A71 RID: 6769
public class UtilityNetworkTubesManager : UtilityNetworkManager<TravelTubeNetwork, TravelTube>
{
	// Token: 0x06008D2D RID: 36141 RVA: 0x00100B9A File Offset: 0x000FED9A
	public UtilityNetworkTubesManager(int game_width, int game_height, int tile_layer) : base(game_width, game_height, tile_layer)
	{
	}

	// Token: 0x06008D2E RID: 36142 RVA: 0x00100BA5 File Offset: 0x000FEDA5
	public override bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		return this.TestForUTurnLeft(cell, new_connection, is_physical_building, out fail_reason) && this.TestForUTurnRight(cell, new_connection, is_physical_building, out fail_reason) && this.TestForNoAdjacentBridge(cell, new_connection, out fail_reason);
	}

	// Token: 0x06008D2F RID: 36143 RVA: 0x00100BCD File Offset: 0x000FEDCD
	public override void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		base.SetConnections(connections, cell, is_physical_building);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
	}

	// Token: 0x06008D30 RID: 36144 RVA: 0x003754EC File Offset: 0x003736EC
	private bool TestForUTurnLeft(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.LeftDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06008D31 RID: 36145 RVA: 0x00375548 File Offset: 0x00373748
	private bool TestForUTurnRight(int first_cell, UtilityConnections first_connection, bool is_physical_building, out string fail_reason)
	{
		int from_cell = first_cell;
		UtilityConnections direction = first_connection;
		int num = 1;
		for (int i = 0; i < 3; i++)
		{
			int num2 = direction.CellInDirection(from_cell);
			UtilityConnections utilityConnections = direction.RightDirection();
			if (this.HasConnection(num2, utilityConnections, is_physical_building))
			{
				num++;
			}
			from_cell = num2;
			direction = utilityConnections;
		}
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_NO_UTURNS;
		return num <= 2;
	}

	// Token: 0x06008D32 RID: 36146 RVA: 0x003755A4 File Offset: 0x003737A4
	private bool TestForNoAdjacentBridge(int cell, UtilityConnections connection, out string fail_reason)
	{
		UtilityConnections direction = connection.LeftDirection();
		UtilityConnections direction2 = connection.RightDirection();
		int cell2 = direction.CellInDirection(cell);
		int cell3 = direction2.CellInDirection(cell);
		GameObject gameObject = Grid.Objects[cell2, 9];
		GameObject gameObject2 = Grid.Objects[cell3, 9];
		fail_reason = UI.TOOLTIPS.HELP_TUBELOCATION_STRAIGHT_BRIDGES;
		return (gameObject == null || gameObject.GetComponent<TravelTubeBridge>() == null) && (gameObject2 == null || gameObject2.GetComponent<TravelTubeBridge>() == null);
	}

	// Token: 0x06008D33 RID: 36147 RVA: 0x00100BE3 File Offset: 0x000FEDE3
	private bool HasConnection(int cell, UtilityConnections connection, bool is_physical_building)
	{
		return (base.GetConnections(cell, is_physical_building) & connection) > (UtilityConnections)0;
	}
}
