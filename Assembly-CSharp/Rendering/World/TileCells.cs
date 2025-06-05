using System;

namespace Rendering.World
{
	// Token: 0x02002140 RID: 8512
	public struct TileCells
	{
		// Token: 0x0600B55B RID: 46427 RVA: 0x004535DC File Offset: 0x004517DC
		public TileCells(int tile_x, int tile_y)
		{
			int val = Grid.WidthInCells - 1;
			int val2 = Grid.HeightInCells - 1;
			this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(tile_y, val2));
			this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(tile_y, val2));
		}

		// Token: 0x04008F99 RID: 36761
		public int Cell0;

		// Token: 0x04008F9A RID: 36762
		public int Cell1;

		// Token: 0x04008F9B RID: 36763
		public int Cell2;

		// Token: 0x04008F9C RID: 36764
		public int Cell3;
	}
}
