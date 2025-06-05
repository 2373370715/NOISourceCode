using System;
using System.Collections.Generic;

namespace Rendering.World
{
	// Token: 0x02002142 RID: 8514
	public abstract class TileRenderer : KMonoBehaviour
	{
		// Token: 0x0600B55D RID: 46429 RVA: 0x00453680 File Offset: 0x00451880
		protected override void OnSpawn()
		{
			this.Masks = this.GetMasks();
			this.TileGridWidth = Grid.WidthInCells + 1;
			this.TileGridHeight = Grid.HeightInCells + 1;
			this.BrushGrid = new int[this.TileGridWidth * this.TileGridHeight * 4];
			for (int i = 0; i < this.BrushGrid.Length; i++)
			{
				this.BrushGrid[i] = -1;
			}
			this.TileGrid = new Tile[this.TileGridWidth * this.TileGridHeight];
			for (int j = 0; j < this.TileGrid.Length; j++)
			{
				int tile_x = j % this.TileGridWidth;
				int tile_y = j / this.TileGridWidth;
				this.TileGrid[j] = new Tile(j, tile_x, tile_y, this.Masks.Length);
			}
			this.LoadBrushes();
			this.VisibleAreaUpdater = new VisibleAreaUpdater(new Action<int>(this.UpdateOutsideView), new Action<int>(this.UpdateInsideView), "TileRenderer");
		}

		// Token: 0x0600B55E RID: 46430 RVA: 0x00453770 File Offset: 0x00451970
		protected virtual Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 2, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, true, false),
				new Mask(this.Atlas, 2, false, false, false, false),
				new Mask(this.Atlas, 1, true, false, false, false),
				new Mask(this.Atlas, 3, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, false, false),
				new Mask(this.Atlas, 3, true, false, false, false),
				new Mask(this.Atlas, 1, true, false, true, false),
				new Mask(this.Atlas, 4, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, false, false),
				new Mask(this.Atlas, 4, false, true, false, false),
				new Mask(this.Atlas, 0, false, false, false, true)
			};
		}

		// Token: 0x0600B55F RID: 46431 RVA: 0x004538FC File Offset: 0x00451AFC
		private void UpdateInsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
				this.DirtyTiles.Add(item);
			}
		}

		// Token: 0x0600B560 RID: 46432 RVA: 0x00453940 File Offset: 0x00451B40
		private void UpdateOutsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
			}
		}

		// Token: 0x0600B561 RID: 46433 RVA: 0x00453974 File Offset: 0x00451B74
		private int[] GetCellTiles(int cell)
		{
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(cell, out num, out num2);
			this.CellTiles[0] = num2 * this.TileGridWidth + num;
			this.CellTiles[1] = num2 * this.TileGridWidth + (num + 1);
			this.CellTiles[2] = (num2 + 1) * this.TileGridWidth + num;
			this.CellTiles[3] = (num2 + 1) * this.TileGridWidth + (num + 1);
			return this.CellTiles;
		}

		// Token: 0x0600B562 RID: 46434
		public abstract void LoadBrushes();

		// Token: 0x0600B563 RID: 46435 RVA: 0x0011A503 File Offset: 0x00118703
		public void MarkDirty(int cell)
		{
			this.VisibleAreaUpdater.UpdateCell(cell);
		}

		// Token: 0x0600B564 RID: 46436 RVA: 0x004539E8 File Offset: 0x00451BE8
		private void LateUpdate()
		{
			foreach (int num in this.ClearTiles)
			{
				this.Clear(ref this.TileGrid[num], this.Brushes, this.BrushGrid);
			}
			this.ClearTiles.Clear();
			foreach (int num2 in this.DirtyTiles)
			{
				this.MarkDirty(ref this.TileGrid[num2], this.Brushes, this.BrushGrid);
			}
			this.DirtyTiles.Clear();
			this.VisibleAreaUpdater.Update();
			foreach (Brush brush in this.DirtyBrushes)
			{
				brush.Refresh();
			}
			this.DirtyBrushes.Clear();
			foreach (Brush brush2 in this.ActiveBrushes)
			{
				brush2.Render();
			}
		}

		// Token: 0x0600B565 RID: 46437
		public abstract void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid);

		// Token: 0x0600B566 RID: 46438 RVA: 0x00453B58 File Offset: 0x00451D58
		public void Clear(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = tile.Idx * 4 + i;
				if (brush_grid[num] != -1)
				{
					brush_array[brush_grid[num]].Remove(tile.Idx);
				}
			}
		}

		// Token: 0x04008FA0 RID: 36768
		private Tile[] TileGrid;

		// Token: 0x04008FA1 RID: 36769
		private int[] BrushGrid;

		// Token: 0x04008FA2 RID: 36770
		protected int TileGridWidth;

		// Token: 0x04008FA3 RID: 36771
		protected int TileGridHeight;

		// Token: 0x04008FA4 RID: 36772
		private int[] CellTiles = new int[4];

		// Token: 0x04008FA5 RID: 36773
		protected Brush[] Brushes;

		// Token: 0x04008FA6 RID: 36774
		protected Mask[] Masks;

		// Token: 0x04008FA7 RID: 36775
		protected List<Brush> DirtyBrushes = new List<Brush>();

		// Token: 0x04008FA8 RID: 36776
		protected List<Brush> ActiveBrushes = new List<Brush>();

		// Token: 0x04008FA9 RID: 36777
		private VisibleAreaUpdater VisibleAreaUpdater;

		// Token: 0x04008FAA RID: 36778
		private HashSet<int> ClearTiles = new HashSet<int>();

		// Token: 0x04008FAB RID: 36779
		private HashSet<int> DirtyTiles = new HashSet<int>();

		// Token: 0x04008FAC RID: 36780
		public TextureAtlas Atlas;
	}
}
