using System;

namespace Rendering.World
{
	// Token: 0x02002141 RID: 8513
	public struct Tile
	{
		// Token: 0x0600B55C RID: 46428 RVA: 0x0011A4E5 File Offset: 0x001186E5
		public Tile(int idx, int tile_x, int tile_y, int mask_count)
		{
			this.Idx = idx;
			this.TileCells = new TileCells(tile_x, tile_y);
			this.MaskCount = mask_count;
		}

		// Token: 0x04008F9D RID: 36765
		public int Idx;

		// Token: 0x04008F9E RID: 36766
		public TileCells TileCells;

		// Token: 0x04008F9F RID: 36767
		public int MaskCount;
	}
}
