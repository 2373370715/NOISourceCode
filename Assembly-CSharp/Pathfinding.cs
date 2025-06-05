using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AF5 RID: 2805
[AddComponentMenu("KMonoBehaviour/scripts/Pathfinding")]
public class Pathfinding : KMonoBehaviour
{
	// Token: 0x060033A5 RID: 13221 RVA: 0x000C6327 File Offset: 0x000C4527
	public static void DestroyInstance()
	{
		Pathfinding.Instance = null;
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x000C6334 File Offset: 0x000C4534
	protected override void OnPrefabInit()
	{
		Pathfinding.Instance = this;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x000C633C File Offset: 0x000C453C
	public void AddNavGrid(NavGrid nav_grid)
	{
		this.NavGrids.Add(nav_grid);
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x002142C4 File Offset: 0x002124C4
	public NavGrid GetNavGrid(string id)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			if (navGrid.id == id)
			{
				return navGrid;
			}
		}
		global::Debug.LogError("Could not find nav grid: " + id);
		return null;
	}

	// Token: 0x060033A9 RID: 13225 RVA: 0x000C634A File Offset: 0x000C454A
	public List<NavGrid> GetNavGrids()
	{
		return this.NavGrids;
	}

	// Token: 0x060033AA RID: 13226 RVA: 0x00214338 File Offset: 0x00212538
	public void ResetNavGrids()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.InitializeGraph();
		}
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x000C6352 File Offset: 0x000C4552
	public void FlushNavGridsOnLoad()
	{
		if (this.navGridsHaveBeenFlushedOnLoad)
		{
			return;
		}
		this.navGridsHaveBeenFlushedOnLoad = true;
		this.UpdateNavGrids(true);
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x00214388 File Offset: 0x00212588
	public void UpdateNavGrids(bool update_all = false)
	{
		update_all = true;
		if (update_all)
		{
			using (List<NavGrid>.Enumerator enumerator = this.NavGrids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NavGrid navGrid = enumerator.Current;
					navGrid.UpdateGraph();
				}
				return;
			}
		}
		foreach (NavGrid navGrid2 in this.NavGrids)
		{
			if (navGrid2.updateEveryFrame)
			{
				navGrid2.UpdateGraph();
			}
		}
		this.NavGrids[this.UpdateIdx].UpdateGraph();
		this.UpdateIdx = (this.UpdateIdx + 1) % this.NavGrids.Count;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x00214458 File Offset: 0x00212658
	public void RenderEveryTick()
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.DebugUpdate();
		}
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x002144A8 File Offset: 0x002126A8
	public void AddDirtyNavGridCell(int cell)
	{
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.AddDirtyCell(cell);
		}
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x002144FC File Offset: 0x002126FC
	public void RefreshNavCell(int cell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		hashSet.Add(cell);
		foreach (NavGrid navGrid in this.NavGrids)
		{
			navGrid.UpdateGraph(hashSet);
		}
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x000C636B File Offset: 0x000C456B
	protected override void OnCleanUp()
	{
		this.NavGrids.Clear();
		OffsetTableTracker.OnPathfindingInvalidated();
	}

	// Token: 0x04002364 RID: 9060
	private List<NavGrid> NavGrids = new List<NavGrid>();

	// Token: 0x04002365 RID: 9061
	private int UpdateIdx;

	// Token: 0x04002366 RID: 9062
	private bool navGridsHaveBeenFlushedOnLoad;

	// Token: 0x04002367 RID: 9063
	public static Pathfinding Instance;
}
