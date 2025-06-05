using System;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public static class NavTypeHelper
{
	// Token: 0x06002435 RID: 9269 RVA: 0x001D5AE8 File Offset: 0x001D3CE8
	public static Vector3 GetNavPos(int cell, NavType nav_type)
	{
		Vector3 zero = Vector3.zero;
		switch (nav_type)
		{
		case NavType.Floor:
			return Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
		case NavType.LeftWall:
			return Grid.CellToPosLCC(cell, Grid.SceneLayer.Move);
		case NavType.RightWall:
			return Grid.CellToPosRCC(cell, Grid.SceneLayer.Move);
		case NavType.Ceiling:
			return Grid.CellToPosCTC(cell, Grid.SceneLayer.Move);
		case NavType.Ladder:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Pole:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Tube:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Solid:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		}
		return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x001D5B90 File Offset: 0x001D3D90
	public static int GetAnchorCell(NavType nav_type, int cell)
	{
		int result = Grid.InvalidCell;
		if (Grid.IsValidCell(cell))
		{
			switch (nav_type)
			{
			case NavType.Floor:
				result = Grid.CellBelow(cell);
				break;
			case NavType.LeftWall:
				result = Grid.CellLeft(cell);
				break;
			case NavType.RightWall:
				result = Grid.CellRight(cell);
				break;
			case NavType.Ceiling:
				result = Grid.CellAbove(cell);
				break;
			default:
				if (nav_type == NavType.Solid)
				{
					result = cell;
				}
				break;
			}
		}
		return result;
	}
}
