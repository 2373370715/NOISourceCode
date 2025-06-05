using System;
using UnityEngine;

// Token: 0x0200081F RID: 2079
public class DrawNavGridQuery : PathFinderQuery
{
	// Token: 0x060024A5 RID: 9381 RVA: 0x000BC493 File Offset: 0x000BA693
	public DrawNavGridQuery Reset(MinionBrain brain)
	{
		return this;
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x001D6D94 File Offset: 0x001D4F94
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (parent_cell == Grid.InvalidCell || (int)Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			return false;
		}
		GL.Color(Color.white);
		GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
		GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
		return false;
	}
}
