using System;
using System.Collections.Generic;

// Token: 0x020007FF RID: 2047
public class NavGridUpdater
{
	// Token: 0x0600241A RID: 9242 RVA: 0x000BBE3F File Offset: 0x000BA03F
	public static void InitializeNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type)
	{
		NavGridUpdater.MarkValidCells(nav_table, validators, bounding_offsets);
		NavGridUpdater.CreateLinks(nav_table, max_links_per_cell, links, transitions_by_nav_type, new Dictionary<int, int>());
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x000BBE59 File Offset: 0x000BA059
	public static void UpdateNavGrid(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions, IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateValidCells(dirty_nav_cells, nav_table, validators, bounding_offsets);
		NavGridUpdater.UpdateLinks(dirty_nav_cells, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x001D5640 File Offset: 0x001D3840
	private static void UpdateValidCells(IEnumerable<int> dirty_solid_cells, NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		foreach (int cell in dirty_solid_cells)
		{
			for (int i = 0; i < validators.Length; i++)
			{
				validators[i].UpdateCell(cell, nav_table, bounding_offsets);
			}
		}
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x000BBE74 File Offset: 0x000BA074
	private static void CreateLinksForCell(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		NavGridUpdater.CreateLinks(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
	}

	// Token: 0x0600241E RID: 9246 RVA: 0x001D569C File Offset: 0x001D389C
	private static void UpdateLinks(IEnumerable<int> dirty_nav_cells, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		foreach (int cell in dirty_nav_cells)
		{
			NavGridUpdater.CreateLinksForCell(cell, nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions);
		}
	}

	// Token: 0x0600241F RID: 9247 RVA: 0x001D56E8 File Offset: 0x001D38E8
	private static void CreateLinks(NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.CreateLinkWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.CreateLinkWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, max_links_per_cell, links, transitions_by_nav_type, teleport_transitions));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06002420 RID: 9248 RVA: 0x001D5738 File Offset: 0x001D3938
	private static void CreateLinks(int cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
	{
		int num = cell * max_links_per_cell;
		int num2 = 0;
		for (int i = 0; i < 11; i++)
		{
			NavType nav_type = (NavType)i;
			NavGrid.Transition[] array = transitions_by_nav_type[i];
			if (array != null && nav_table.IsValid(cell, nav_type))
			{
				NavGrid.Transition[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					NavGrid.Transition transition;
					if ((transition = array2[j]).start == NavType.Teleport && teleport_transitions.ContainsKey(cell))
					{
						int num3;
						int num4;
						Grid.CellToXY(cell, out num3, out num4);
						int num5 = teleport_transitions[cell];
						int num6;
						int num7;
						Grid.CellToXY(teleport_transitions[cell], out num6, out num7);
						transition.x = num6 - num3;
						transition.y = num7 - num4;
					}
					int num8 = transition.IsValid(cell, nav_table);
					if (num8 != Grid.InvalidCell)
					{
						links[num] = new NavGrid.Link(num8, transition.start, transition.end, transition.id, transition.cost);
						num++;
						num2++;
					}
				}
			}
		}
		if (num2 >= max_links_per_cell)
		{
			Debug.LogError("Out of nav links. Need to increase maxLinksPerCell:" + max_links_per_cell.ToString());
		}
		links[num].link = Grid.InvalidCell;
	}

	// Token: 0x06002421 RID: 9249 RVA: 0x001D5864 File Offset: 0x001D3A64
	private static void MarkValidCells(NavTable nav_table, NavTableValidator[] validators, CellOffset[] bounding_offsets)
	{
		WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object> workItemCollection = new WorkItemCollection<NavGridUpdater.MarkValidCellWorkItem, object>();
		workItemCollection.Reset(null);
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			workItemCollection.Add(new NavGridUpdater.MarkValidCellWorkItem(Grid.OffsetCell(0, new CellOffset(0, i)), nav_table, bounding_offsets, validators));
		}
		GlobalJobManager.Run(workItemCollection);
	}

	// Token: 0x06002422 RID: 9250 RVA: 0x000BBDD2 File Offset: 0x000B9FD2
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06002423 RID: 9251 RVA: 0x001D58B0 File Offset: 0x001D3AB0
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGridUpdater.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x04001895 RID: 6293
	public static int InvalidHandle = -1;

	// Token: 0x04001896 RID: 6294
	public static int InvalidIdx = -1;

	// Token: 0x04001897 RID: 6295
	public static int InvalidCell = -1;

	// Token: 0x02000800 RID: 2048
	private struct CreateLinkWorkItem : IWorkItem<object>
	{
		// Token: 0x06002426 RID: 9254 RVA: 0x000BBE97 File Offset: 0x000BA097
		public CreateLinkWorkItem(int start_cell, NavTable nav_table, int max_links_per_cell, NavGrid.Link[] links, NavGrid.Transition[][] transitions_by_nav_type, Dictionary<int, int> teleport_transitions)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.maxLinksPerCell = max_links_per_cell;
			this.links = links;
			this.transitionsByNavType = transitions_by_nav_type;
			this.teleportTransitions = teleport_transitions;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x001D5908 File Offset: 0x001D3B08
		public void Run(object shared_data, int threadIndex)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				NavGridUpdater.CreateLinksForCell(this.startCell + i, this.navTable, this.maxLinksPerCell, this.links, this.transitionsByNavType, this.teleportTransitions);
			}
		}

		// Token: 0x04001898 RID: 6296
		private int startCell;

		// Token: 0x04001899 RID: 6297
		private NavTable navTable;

		// Token: 0x0400189A RID: 6298
		private int maxLinksPerCell;

		// Token: 0x0400189B RID: 6299
		private NavGrid.Link[] links;

		// Token: 0x0400189C RID: 6300
		private NavGrid.Transition[][] transitionsByNavType;

		// Token: 0x0400189D RID: 6301
		private Dictionary<int, int> teleportTransitions;
	}

	// Token: 0x02000801 RID: 2049
	private struct MarkValidCellWorkItem : IWorkItem<object>
	{
		// Token: 0x06002428 RID: 9256 RVA: 0x000BBEC6 File Offset: 0x000BA0C6
		public MarkValidCellWorkItem(int start_cell, NavTable nav_table, CellOffset[] bounding_offsets, NavTableValidator[] validators)
		{
			this.startCell = start_cell;
			this.navTable = nav_table;
			this.boundingOffsets = bounding_offsets;
			this.validators = validators;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x001D5950 File Offset: 0x001D3B50
		public void Run(object shared_data, int threadIndex)
		{
			for (int i = 0; i < Grid.WidthInCells; i++)
			{
				int cell = this.startCell + i;
				NavTableValidator[] array = this.validators;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].UpdateCell(cell, this.navTable, this.boundingOffsets);
				}
			}
		}

		// Token: 0x0400189E RID: 6302
		private NavTable navTable;

		// Token: 0x0400189F RID: 6303
		private CellOffset[] boundingOffsets;

		// Token: 0x040018A0 RID: 6304
		private NavTableValidator[] validators;

		// Token: 0x040018A1 RID: 6305
		private int startCell;
	}
}
