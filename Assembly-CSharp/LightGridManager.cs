using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017D1 RID: 6097
public static class LightGridManager
{
	// Token: 0x06007D63 RID: 32099 RVA: 0x0033132C File Offset: 0x0032F52C
	public static int ComputeFalloff(float fallOffRate, int cell, int originCell, global::LightShape lightShape, DiscreteShadowCaster.Direction lightDirection)
	{
		int num = originCell;
		if (lightShape == global::LightShape.Quad)
		{
			Vector2I vector2I = Grid.CellToXY(num);
			Vector2I vector2I2 = Grid.CellToXY(cell);
			switch (lightDirection)
			{
			case DiscreteShadowCaster.Direction.North:
			case DiscreteShadowCaster.Direction.South:
			{
				Vector2I vector2I3 = new Vector2I(vector2I2.x, vector2I.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			case DiscreteShadowCaster.Direction.East:
			case DiscreteShadowCaster.Direction.West:
			{
				Vector2I vector2I3 = new Vector2I(vector2I.x, vector2I2.y);
				num = Grid.XYToCell(vector2I3.x, vector2I3.y);
				break;
			}
			}
		}
		return LightGridManager.CalculateFalloff(fallOffRate, cell, num);
	}

	// Token: 0x06007D64 RID: 32100 RVA: 0x000F7163 File Offset: 0x000F5363
	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06007D65 RID: 32101 RVA: 0x000F7180 File Offset: 0x000F5380
	public static void Initialise()
	{
		LightGridManager.previewLux = new int[Grid.CellCount];
	}

	// Token: 0x06007D66 RID: 32102 RVA: 0x000F7191 File Offset: 0x000F5391
	public static void Shutdown()
	{
		LightGridManager.previewLux = null;
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06007D67 RID: 32103 RVA: 0x003313BC File Offset: 0x0032F5BC
	public static void DestroyPreview()
	{
		foreach (global::Tuple<int, int> tuple in LightGridManager.previewLightCells)
		{
			LightGridManager.previewLux[tuple.first] = 0;
		}
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06007D68 RID: 32104 RVA: 0x000F71A3 File Offset: 0x000F53A3
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux)
	{
		LightGridManager.CreatePreview(origin_cell, radius, shape, lux, 0, DiscreteShadowCaster.Direction.South);
	}

	// Token: 0x06007D69 RID: 32105 RVA: 0x00331420 File Offset: 0x0032F620
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
	{
		LightGridManager.previewLightCells.Clear();
		ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape, true);
		foreach (int num in pooledList)
		{
			if (Grid.IsValidCell(num))
			{
				int num2 = lux / LightGridManager.ComputeFalloff(0.5f, num, origin_cell, shape, direction);
				LightGridManager.previewLightCells.Add(new global::Tuple<int, int>(num, num2));
				LightGridManager.previewLux[num] = num2;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x04005E6F RID: 24175
	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	// Token: 0x04005E70 RID: 24176
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04005E71 RID: 24177
	public static int[] previewLux;

	// Token: 0x020017D2 RID: 6098
	public class LightGridEmitter
	{
		// Token: 0x06007D6B RID: 32107 RVA: 0x003314C8 File Offset: 0x0032F6C8
		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int)this.state.radius, this.state.width, this.state.direction, this.state.shape, true);
		}

		// Token: 0x06007D6C RID: 32108 RVA: 0x0033151C File Offset: 0x0032F71C
		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter", null);
			if (update_lit_cells)
			{
				this.UpdateLitCells();
			}
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					int num2 = Mathf.Max(0, Grid.LightCount[num] + this.ComputeLux(num));
					Grid.LightCount[num] = num2;
					LightGridManager.previewLux[num] = num2;
				}
			}
		}

		// Token: 0x06007D6D RID: 32109 RVA: 0x003315C0 File Offset: 0x0032F7C0
		public void RemoveFromGrid()
		{
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					Grid.LightCount[num] = Mathf.Max(0, Grid.LightCount[num] - this.ComputeLux(num));
					LightGridManager.previewLux[num] = 0;
				}
			}
			this.litCells.Clear();
		}

		// Token: 0x06007D6E RID: 32110 RVA: 0x000F71BC File Offset: 0x000F53BC
		public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
		{
			if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
			{
				return false;
			}
			this.RemoveFromGrid();
			this.state = state;
			this.AddToGrid(true);
			return true;
		}

		// Token: 0x06007D6F RID: 32111 RVA: 0x000F71EB File Offset: 0x000F53EB
		private int ComputeLux(int cell)
		{
			return this.state.intensity / this.ComputeFalloff(cell);
		}

		// Token: 0x06007D70 RID: 32112 RVA: 0x000F7200 File Offset: 0x000F5400
		private int ComputeFalloff(int cell)
		{
			return LightGridManager.ComputeFalloff(this.state.falloffRate, cell, this.state.origin, this.state.shape, this.state.direction);
		}

		// Token: 0x04005E72 RID: 24178
		private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;

		// Token: 0x04005E73 RID: 24179
		private List<int> litCells = new List<int>();

		// Token: 0x020017D3 RID: 6099
		[Serializable]
		public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
		{
			// Token: 0x06007D72 RID: 32114 RVA: 0x00331644 File Offset: 0x0032F844
			public bool Equals(LightGridManager.LightGridEmitter.State rhs)
			{
				return this.origin == rhs.origin && this.shape == rhs.shape && this.radius == rhs.radius && this.intensity == rhs.intensity && this.falloffRate == rhs.falloffRate && this.colour == rhs.colour && this.width == rhs.width && this.direction == rhs.direction;
			}

			// Token: 0x04005E74 RID: 24180
			public int origin;

			// Token: 0x04005E75 RID: 24181
			public global::LightShape shape;

			// Token: 0x04005E76 RID: 24182
			public int width;

			// Token: 0x04005E77 RID: 24183
			public DiscreteShadowCaster.Direction direction;

			// Token: 0x04005E78 RID: 24184
			public float radius;

			// Token: 0x04005E79 RID: 24185
			public int intensity;

			// Token: 0x04005E7A RID: 24186
			public float falloffRate;

			// Token: 0x04005E7B RID: 24187
			public Color colour;

			// Token: 0x04005E7C RID: 24188
			public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State
			{
				origin = Grid.InvalidCell,
				shape = global::LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white,
				direction = DiscreteShadowCaster.Direction.South,
				width = 4
			};
		}
	}
}
