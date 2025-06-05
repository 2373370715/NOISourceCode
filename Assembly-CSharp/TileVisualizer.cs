using System;
using UnityEngine;

// Token: 0x02000B67 RID: 2919
public class TileVisualizer
{
	// Token: 0x060036EC RID: 14060 RVA: 0x002226E4 File Offset: 0x002208E4
	private static void RefreshCellInternal(int cell, ObjectLayer tile_layer)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, (int)tile_layer];
		if (gameObject != null)
		{
			World.Instance.blockTileRenderer.Rebuild(tile_layer, cell);
			KAnimGraphTileVisualizer componentInChildren = gameObject.GetComponentInChildren<KAnimGraphTileVisualizer>();
			if (componentInChildren != null)
			{
				componentInChildren.Refresh();
				return;
			}
		}
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x000C82C5 File Offset: 0x000C64C5
	private static void RefreshCell(int cell, ObjectLayer tile_layer)
	{
		if (tile_layer == ObjectLayer.NumLayers)
		{
			return;
		}
		TileVisualizer.RefreshCellInternal(cell, tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellAbove(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellBelow(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellLeft(cell), tile_layer);
		TileVisualizer.RefreshCellInternal(Grid.CellRight(cell), tile_layer);
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x000C8304 File Offset: 0x000C6504
	public static void RefreshCell(int cell, ObjectLayer tile_layer, ObjectLayer replacement_layer)
	{
		TileVisualizer.RefreshCell(cell, tile_layer);
		TileVisualizer.RefreshCell(cell, replacement_layer);
	}
}
