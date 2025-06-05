using System;
using UnityEngine;

// Token: 0x02000AD6 RID: 2774
public class NavTactic
{
	// Token: 0x060032EA RID: 13034 RVA: 0x000C58AE File Offset: 0x000C3AAE
	public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x002127F4 File Offset: 0x002109F4
	public NavTactic(int preferredRange, int rangePenalty, int overlapPenalty, int pathCostPenalty, int xPenalty, int preferredX, int yPenalty, int preferredY)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
		this._pathXCostPenalty = xPenalty;
		this._preferredX = preferredX;
		this._pathYCostPenalty = yPenalty;
		this._preferredY = preferredY;
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x0021285C File Offset: 0x00210A5C
	public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
	{
		int result = NavigationReservations.InvalidReservation;
		int num = int.MaxValue;
		for (int i = 0; i < offsets.Length; i++)
		{
			int num2 = Grid.OffsetCell(root, offsets[i]);
			int num3 = 0;
			num3 += this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2);
			num3 += this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2));
			num3 += this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0);
			num3 += this._pathXCostPenalty * Mathf.Abs(this._preferredX - Mathf.Abs(Grid.CellColumn(root) - Grid.CellColumn(num2)));
			num3 += this._pathYCostPenalty * Mathf.Abs(this._preferredY - Mathf.Abs(Grid.CellRow(root) - Grid.CellRow(num2)));
			if (num3 < num && navigator.CanReach(num2))
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x040022C9 RID: 8905
	private int _overlapPenalty = 3;

	// Token: 0x040022CA RID: 8906
	private int _preferredRange;

	// Token: 0x040022CB RID: 8907
	private int _rangePenalty = 2;

	// Token: 0x040022CC RID: 8908
	private int _pathCostPenalty = 1;

	// Token: 0x040022CD RID: 8909
	private int _pathXCostPenalty;

	// Token: 0x040022CE RID: 8910
	private int _preferredX;

	// Token: 0x040022CF RID: 8911
	private int _pathYCostPenalty;

	// Token: 0x040022D0 RID: 8912
	private int _preferredY;
}
