using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000AFB RID: 2811
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Placeable")]
public class Placeable : KMonoBehaviour
{
	// Token: 0x06003412 RID: 13330 RVA: 0x00215ED8 File Offset: 0x002140D8
	public bool IsValidPlaceLocation(int cell, out string reason)
	{
		if (this.placementRules.Contains(Placeable.PlacementRules.RestrictToWorld) && (int)Grid.WorldIdx[cell] != this.restrictWorldId)
		{
			reason = UI.TOOLS.PLACE.REASONS.RESTRICT_TO_WORLD;
			return false;
		}
		if (!this.occupyArea.CanOccupyArea(cell, this.occupyArea.objectLayers[0]))
		{
			reason = UI.TOOLS.PLACE.REASONS.CAN_OCCUPY_AREA;
			return false;
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.OnFoundation))
		{
			bool flag = this.occupyArea.TestAreaBelow(cell, null, new Func<int, object, bool>(this.FoundationTest));
			if (this.checkRootCellOnly)
			{
				flag = this.FoundationTest(Grid.CellBelow(cell), null);
			}
			if (!flag)
			{
				reason = UI.TOOLS.PLACE.REASONS.ON_FOUNDATION;
				return false;
			}
		}
		if (this.placementRules.Contains(Placeable.PlacementRules.VisibleToSpace))
		{
			bool flag2 = this.occupyArea.TestArea(cell, null, new Func<int, object, bool>(this.SunnySpaceTest));
			if (this.checkRootCellOnly)
			{
				flag2 = this.SunnySpaceTest(cell, null);
			}
			if (!flag2)
			{
				reason = UI.TOOLS.PLACE.REASONS.VISIBLE_TO_SPACE;
				return false;
			}
		}
		reason = "ok!";
		return true;
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x00215FDC File Offset: 0x002141DC
	private bool SunnySpaceTest(int cell, object data)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int x;
		int startY;
		Grid.CellToXY(cell, out x, out startY);
		int num = (int)Grid.WorldIdx[cell];
		if (num == 255)
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld(num);
		int top = world.WorldOffset.y + world.WorldSize.y;
		return !Grid.Solid[cell] && !Grid.Foundation[cell] && (Grid.ExposedToSunlight[cell] >= 253 || this.ClearPathToSky(x, startY, top));
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x00216070 File Offset: 0x00214270
	private bool ClearPathToSky(int x, int startY, int top)
	{
		for (int i = startY; i < top; i++)
		{
			int i2 = Grid.XYToCell(x, i);
			if (Grid.Solid[i2] || Grid.Foundation[i2])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000C6697 File Offset: 0x000C4897
	private bool FoundationTest(int cell, object data)
	{
		return Grid.IsValidBuildingCell(cell) && (Grid.Solid[cell] || Grid.Foundation[cell]);
	}

	// Token: 0x0400239D RID: 9117
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400239E RID: 9118
	public string kAnimName;

	// Token: 0x0400239F RID: 9119
	public string animName;

	// Token: 0x040023A0 RID: 9120
	public List<Placeable.PlacementRules> placementRules = new List<Placeable.PlacementRules>();

	// Token: 0x040023A1 RID: 9121
	[NonSerialized]
	public int restrictWorldId;

	// Token: 0x040023A2 RID: 9122
	public bool checkRootCellOnly;

	// Token: 0x02000AFC RID: 2812
	public enum PlacementRules
	{
		// Token: 0x040023A4 RID: 9124
		OnFoundation,
		// Token: 0x040023A5 RID: 9125
		VisibleToSpace,
		// Token: 0x040023A6 RID: 9126
		RestrictToWorld
	}
}
