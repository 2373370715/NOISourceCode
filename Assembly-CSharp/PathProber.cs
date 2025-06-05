using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000819 RID: 2073
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/PathProber")]
public class PathProber : KMonoBehaviour
{
	// Token: 0x0600248C RID: 9356 RVA: 0x000BC302 File Offset: 0x000BA502
	protected override void OnCleanUp()
	{
		if (this.PathGrid != null)
		{
			this.PathGrid.OnCleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000BC31D File Offset: 0x000BA51D
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.PathGrid.SetGroupProber(group_prober);
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000BC32B File Offset: 0x000BA52B
	public void SetValidNavTypes(NavType[] nav_types, int max_probing_radius)
	{
		if (max_probing_radius != 0)
		{
			this.PathGrid = new PathGrid(max_probing_radius * 2, max_probing_radius * 2, true, nav_types);
			return;
		}
		this.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, nav_types);
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000BC35B File Offset: 0x000BA55B
	public int GetCost(int cell)
	{
		return this.PathGrid.GetCost(cell);
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000BC369 File Offset: 0x000BA569
	public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		return this.PathGrid.GetCostIgnoreProberOffset(cell, offsets);
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000BC378 File Offset: 0x000BA578
	public PathGrid GetPathGrid()
	{
		return this.PathGrid;
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x001D6ACC File Offset: 0x001D4CCC
	public void UpdateProbe(NavGrid nav_grid, int cell, NavType nav_type, PathFinderAbilities abilities, PathFinder.PotentialPath.Flags flags)
	{
		if (this.scratchPad == null)
		{
			this.scratchPad = new PathFinder.PotentialScratchPad(nav_grid.maxLinksPerCell);
		}
		bool flag = this.updateCount == -1;
		bool flag2 = this.Potentials.Count == 0 || flag;
		this.PathGrid.BeginUpdate(cell, !flag2);
		if (flag2)
		{
			this.updateCount = 0;
			bool flag3;
			PathFinder.Cell cell2 = this.PathGrid.GetCell(cell, nav_type, out flag3);
			PathFinder.AddPotential(new PathFinder.PotentialPath(cell, nav_type, flags), Grid.InvalidCell, NavType.NumNavTypes, 0, 0, this.Potentials, this.PathGrid, ref cell2);
		}
		int num = (this.potentialCellsPerUpdate <= 0 || flag) ? int.MaxValue : this.potentialCellsPerUpdate;
		this.updateCount++;
		while (this.Potentials.Count > 0 && num > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = this.Potentials.Next();
			num--;
			bool flag3;
			PathFinder.Cell cell3 = this.PathGrid.GetCell(keyValuePair.Value, out flag3);
			if (cell3.cost == keyValuePair.Key)
			{
				PathFinder.AddPotentials(this.scratchPad, keyValuePair.Value, cell3.cost, ref abilities, null, nav_grid.maxLinksPerCell, nav_grid.Links, this.Potentials, this.PathGrid, cell3.parent, cell3.parentNavType);
			}
		}
		bool flag4 = this.Potentials.Count == 0;
		this.PathGrid.EndUpdate(flag4);
		if (flag4)
		{
			int num2 = this.updateCount;
		}
	}

	// Token: 0x040018FB RID: 6395
	public const int InvalidHandle = -1;

	// Token: 0x040018FC RID: 6396
	public const int InvalidIdx = -1;

	// Token: 0x040018FD RID: 6397
	public const int InvalidCell = -1;

	// Token: 0x040018FE RID: 6398
	public const int InvalidCost = -1;

	// Token: 0x040018FF RID: 6399
	private PathGrid PathGrid;

	// Token: 0x04001900 RID: 6400
	private PathFinder.PotentialList Potentials = new PathFinder.PotentialList();

	// Token: 0x04001901 RID: 6401
	public int updateCount = -1;

	// Token: 0x04001902 RID: 6402
	private const int updateCountThreshold = 25;

	// Token: 0x04001903 RID: 6403
	private PathFinder.PotentialScratchPad scratchPad;

	// Token: 0x04001904 RID: 6404
	public int potentialCellsPerUpdate = -1;
}
