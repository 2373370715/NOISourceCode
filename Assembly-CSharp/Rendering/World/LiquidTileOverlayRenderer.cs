using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x02002143 RID: 8515
	public class LiquidTileOverlayRenderer : TileRenderer
	{
		// Token: 0x0600B568 RID: 46440 RVA: 0x0011A551 File Offset: 0x00118751
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
		}

		// Token: 0x0600B569 RID: 46441 RVA: 0x00453B94 File Offset: 0x00451D94
		protected override Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 0, false, true, false, false),
				new Mask(this.Atlas, 1, false, false, false, false)
			};
		}

		// Token: 0x0600B56A RID: 46442 RVA: 0x00453BEC File Offset: 0x00451DEC
		public void OnShadersReloaded()
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num].SetMaterial(material, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x0600B56B RID: 46443 RVA: 0x00453CD8 File Offset: 0x00451ED8
		public override void LoadBrushes()
		{
			this.Brushes = new Brush[ElementLoader.elements.Count * this.Masks.Length];
			foreach (Element element in ElementLoader.elements)
			{
				if (element.IsLiquid && element.substance != null && element.substance.material != null)
				{
					Material material = new Material(element.substance.material);
					this.InitAlphaMaterial(material, element);
					int idx = element.substance.idx;
					for (int i = 0; i < this.Masks.Length; i++)
					{
						int num = idx * this.Masks.Length + i;
						element.substance.RefreshPropertyBlock();
						this.Brushes[num] = new Brush(num, element.id.ToString(), material, this.Masks[i], this.ActiveBrushes, this.DirtyBrushes, this.TileGridWidth, element.substance.propertyBlock);
					}
				}
			}
		}

		// Token: 0x0600B56C RID: 46444 RVA: 0x00453E18 File Offset: 0x00452018
		private void InitAlphaMaterial(Material alpha_material, Element element)
		{
			alpha_material.name = element.name;
			alpha_material.renderQueue = RenderQueues.BlockTiles + element.substance.idx;
			alpha_material.EnableKeyword("ALPHA");
			alpha_material.DisableKeyword("OPAQUE");
			alpha_material.SetTexture("_AlphaTestMap", this.Atlas.texture);
			alpha_material.SetInt("_SrcAlpha", 5);
			alpha_material.SetInt("_DstAlpha", 10);
			alpha_material.SetInt("_ZWrite", 0);
			alpha_material.SetColor("_Colour", element.substance.colour);
		}

		// Token: 0x0600B56D RID: 46445 RVA: 0x00453EB4 File Offset: 0x004520B4
		private bool RenderLiquid(int cell, int cell_above)
		{
			bool result = false;
			if (Grid.Element[cell].IsSolid)
			{
				Element element = Grid.Element[cell_above];
				if (element.IsLiquid && element.substance.material != null)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600B56E RID: 46446 RVA: 0x00453EF8 File Offset: 0x004520F8
		private void SetBrushIdx(int i, ref Tile tile, int substance_idx, LiquidTileOverlayRenderer.LiquidConnections connections, Brush[] brush_array, int[] brush_grid)
		{
			if (connections == LiquidTileOverlayRenderer.LiquidConnections.Empty)
			{
				brush_grid[tile.Idx * 4 + i] = -1;
				return;
			}
			Brush brush = brush_array[substance_idx * tile.MaskCount + connections - LiquidTileOverlayRenderer.LiquidConnections.Left];
			brush.Add(tile.Idx);
			brush_grid[tile.Idx * 4 + i] = brush.Id;
		}

		// Token: 0x0600B56F RID: 46447 RVA: 0x00453F50 File Offset: 0x00452150
		public override void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			if (!this.RenderLiquid(tile.TileCells.Cell0, tile.TileCells.Cell2))
			{
				if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
				{
					this.SetBrushIdx(1, ref tile, Grid.Element[tile.TileCells.Cell3].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Right, brush_array, brush_grid);
				}
				return;
			}
			if (this.RenderLiquid(tile.TileCells.Cell1, tile.TileCells.Cell3))
			{
				this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Both, brush_array, brush_grid);
				return;
			}
			this.SetBrushIdx(0, ref tile, Grid.Element[tile.TileCells.Cell2].substance.idx, LiquidTileOverlayRenderer.LiquidConnections.Left, brush_array, brush_grid);
		}

		// Token: 0x02002144 RID: 8516
		private enum LiquidConnections
		{
			// Token: 0x04008FAE RID: 36782
			Left = 1,
			// Token: 0x04008FAF RID: 36783
			Right,
			// Token: 0x04008FB0 RID: 36784
			Both,
			// Token: 0x04008FB1 RID: 36785
			Empty = 128
		}
	}
}
