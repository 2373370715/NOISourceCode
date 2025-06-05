using System;
using System.Collections.Generic;
using ProcGen;
using TUNING;

// Token: 0x020020DD RID: 8413
public class BestFit
{
	// Token: 0x0600B355 RID: 45909 RVA: 0x004417E0 File Offset: 0x0043F9E0
	public static Vector2I BestFitWorlds(List<WorldPlacement> worldsToArrange, bool ignoreBestFitY = false)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		Vector2I vector2I = default(Vector2I);
		List<WorldPlacement> list2 = new List<WorldPlacement>(worldsToArrange);
		list2.Sort((WorldPlacement a, WorldPlacement b) => b.height.CompareTo(a.height));
		int height = list2[0].height;
		foreach (WorldPlacement worldPlacement in list2)
		{
			Vector2I vector2I2 = default(Vector2I);
			while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height), list))
			{
				if (ignoreBestFitY)
				{
					vector2I2.x++;
				}
				else if (vector2I2.y + worldPlacement.height >= height + 32)
				{
					vector2I2.y = 0;
					vector2I2.x++;
				}
				else
				{
					vector2I2.y++;
				}
			}
			vector2I.x = Math.Max(worldPlacement.width + vector2I2.x, vector2I.x);
			vector2I.y = Math.Max(worldPlacement.height + vector2I2.y, vector2I.y);
			list.Add(new BestFit.Rect(vector2I2.x, vector2I2.y, worldPlacement.width, worldPlacement.height));
			worldPlacement.SetPosition(vector2I2);
		}
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			vector2I.x += 136;
			vector2I.y = Math.Max(vector2I.y, 136);
		}
		return vector2I;
	}

	// Token: 0x0600B356 RID: 45910 RVA: 0x0044199C File Offset: 0x0043FB9C
	private static bool UnoccupiedSpace(BestFit.Rect RectA, List<BestFit.Rect> placed)
	{
		foreach (BestFit.Rect rect in placed)
		{
			if (RectA.X1 < rect.X2 && RectA.X2 > rect.X1 && RectA.Y1 < rect.Y2 && RectA.Y2 > rect.Y1)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600B357 RID: 45911 RVA: 0x00441A2C File Offset: 0x0043FC2C
	public static Vector2I GetGridOffset(IList<WorldContainer> existingWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I result = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I.x, vector2I.y, newWorldSize.x, newWorldSize.y), list))
		{
			if (vector2I.x + newWorldSize.x >= widthInCells)
			{
				vector2I.x = 0;
				vector2I.y++;
			}
			else
			{
				vector2I.x++;
			}
		}
		Debug.Assert(vector2I.x + newWorldSize.x <= Grid.WidthInCells, "BestFit is trying to expand the grid width, this is unsupported and will break the SIM.");
		result.y = Math.Max(newWorldSize.y + vector2I.y, Grid.HeightInCells);
		newWorldOffset = vector2I;
		return result;
	}

	// Token: 0x0600B358 RID: 45912 RVA: 0x00441B70 File Offset: 0x0043FD70
	public static int CountRocketInteriors(IList<WorldContainer> existingWorlds)
	{
		int num = 0;
		List<BestFit.Rect> list = new List<BestFit.Rect>();
		foreach (WorldContainer worldContainer in existingWorlds)
		{
			list.Add(new BestFit.Rect(worldContainer.WorldOffset.x, worldContainer.WorldOffset.y, worldContainer.WorldSize.x, worldContainer.WorldSize.y));
		}
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		while (BestFit.PlaceWorld(list, rocket_INTERIOR_SIZE, out vector2I))
		{
			num++;
			list.Add(new BestFit.Rect(vector2I.x, vector2I.y, rocket_INTERIOR_SIZE.x, rocket_INTERIOR_SIZE.y));
		}
		return num;
	}

	// Token: 0x0600B359 RID: 45913 RVA: 0x00441C38 File Offset: 0x0043FE38
	private static bool PlaceWorld(List<BestFit.Rect> placedWorlds, Vector2I newWorldSize, out Vector2I newWorldOffset)
	{
		Vector2I vector2I = new Vector2I(Grid.WidthInCells, 0);
		int widthInCells = Grid.WidthInCells;
		Vector2I vector2I2 = default(Vector2I);
		while (!BestFit.UnoccupiedSpace(new BestFit.Rect(vector2I2.x, vector2I2.y, newWorldSize.x, newWorldSize.y), placedWorlds))
		{
			if (vector2I2.x + newWorldSize.x >= widthInCells)
			{
				vector2I2.x = 0;
				vector2I2.y++;
			}
			else
			{
				vector2I2.x++;
			}
		}
		vector2I.y = Math.Max(newWorldSize.y + vector2I2.y, Grid.HeightInCells);
		newWorldOffset = vector2I2;
		return vector2I2.x + newWorldSize.x <= Grid.WidthInCells && vector2I2.y + newWorldSize.y <= Grid.HeightInCells;
	}

	// Token: 0x020020DE RID: 8414
	private struct Rect
	{
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600B35B RID: 45915 RVA: 0x001191D6 File Offset: 0x001173D6
		public int X1
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600B35C RID: 45916 RVA: 0x001191DE File Offset: 0x001173DE
		public int X2
		{
			get
			{
				return this.x + this.width + 2;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600B35D RID: 45917 RVA: 0x001191EF File Offset: 0x001173EF
		public int Y1
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600B35E RID: 45918 RVA: 0x001191F7 File Offset: 0x001173F7
		public int Y2
		{
			get
			{
				return this.y + this.height + 2;
			}
		}

		// Token: 0x0600B35F RID: 45919 RVA: 0x00119208 File Offset: 0x00117408
		public Rect(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		// Token: 0x04008DEB RID: 36331
		private int x;

		// Token: 0x04008DEC RID: 36332
		private int y;

		// Token: 0x04008DED RID: 36333
		private int width;

		// Token: 0x04008DEE RID: 36334
		private int height;
	}
}
