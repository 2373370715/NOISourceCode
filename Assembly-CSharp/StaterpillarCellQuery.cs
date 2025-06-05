using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000830 RID: 2096
public class StaterpillarCellQuery : PathFinderQuery
{
	// Token: 0x060024E8 RID: 9448 RVA: 0x001D7E78 File Offset: 0x001D6078
	public StaterpillarCellQuery Reset(int max_results, GameObject tester, ObjectLayer conduitLayer)
	{
		this.max_results = max_results;
		this.tester = tester;
		this.result_cells.Clear();
		ObjectLayer objectLayer;
		if (conduitLayer <= ObjectLayer.LiquidConduit)
		{
			if (conduitLayer == ObjectLayer.GasConduit)
			{
				objectLayer = ObjectLayer.GasConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.LiquidConduit)
			{
				objectLayer = ObjectLayer.LiquidConduitConnection;
				goto IL_4A;
			}
		}
		else
		{
			if (conduitLayer == ObjectLayer.SolidConduit)
			{
				objectLayer = ObjectLayer.SolidConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.Wire)
			{
				objectLayer = ObjectLayer.WireConnectors;
				goto IL_4A;
			}
		}
		objectLayer = conduitLayer;
		IL_4A:
		this.connectorLayer = objectLayer;
		return this;
	}

	// Token: 0x060024E9 RID: 9449 RVA: 0x000BC7EB File Offset: 0x000BA9EB
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidRoofCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060024EA RID: 9450 RVA: 0x001D7ED8 File Offset: 0x001D60D8
	private bool CheckValidRoofCell(int testCell)
	{
		if (!this.tester.GetComponent<Navigator>().NavGrid.NavTable.IsValid(testCell, NavType.Ceiling))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Down);
		return !Grid.ObjectLayers[1].ContainsKey(testCell) && !Grid.ObjectLayers[1].ContainsKey(cellInDirection) && !Grid.Objects[cellInDirection, (int)this.connectorLayer] && Grid.IsValidBuildingCell(testCell) && Grid.IsValidCell(cellInDirection) && Grid.IsValidBuildingCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection);
	}

	// Token: 0x04001979 RID: 6521
	public List<int> result_cells = new List<int>();

	// Token: 0x0400197A RID: 6522
	private int max_results;

	// Token: 0x0400197B RID: 6523
	private GameObject tester;

	// Token: 0x0400197C RID: 6524
	private ObjectLayer connectorLayer;
}
