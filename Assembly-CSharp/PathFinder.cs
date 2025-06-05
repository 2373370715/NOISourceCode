using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000806 RID: 2054
public class PathFinder
{
	// Token: 0x06002437 RID: 9271 RVA: 0x001D5BF0 File Offset: 0x001D3DF0
	public static void Initialize()
	{
		NavType[] array = new NavType[11];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NavType)i;
		}
		PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
		for (int j = 0; j < Grid.CellCount; j++)
		{
			if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0)
			{
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				GameUtil.FloodFillConditional(j, PathFinder.allowPathfindingFloodFillCb, pooledList, null);
				Grid.AllowPathfinding[j] = true;
				pooledList.Recycle();
			}
		}
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(PathFinder.OnReveal));
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x000AA038 File Offset: 0x000A8238
	private static void OnReveal(int cell)
	{
	}

	// Token: 0x06002439 RID: 9273 RVA: 0x000BBF05 File Offset: 0x000BA105
	public static void UpdatePath(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x001D5C98 File Offset: 0x001D3E98
	public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref PathFinder.Path path)
	{
		if (!path.IsValid())
		{
			return false;
		}
		for (int i = 0; i < path.nodes.Count; i++)
		{
			PathFinder.Path.Node node = path.nodes[i];
			if (i < path.nodes.Count - 1)
			{
				PathFinder.Path.Node node2 = path.nodes[i + 1];
				int num = node.cell * nav_grid.maxLinksPerCell;
				bool flag = false;
				NavGrid.Link link = nav_grid.Links[num];
				while (link.link != PathFinder.InvalidHandle)
				{
					if (link.link == node2.cell && node2.navType == link.endNavType && node.navType == link.startNavType)
					{
						PathFinder.PotentialPath potentialPath = new PathFinder.PotentialPath(node.cell, node.navType, PathFinder.PotentialPath.Flags.None);
						flag = abilities.TraversePath(ref potentialPath, node.cell, node.navType, 0, (int)link.transitionId, false);
						if (flag)
						{
							break;
						}
					}
					num++;
					link = nav_grid.Links[num];
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x001D5DAC File Offset: 0x001D3FAC
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query)
	{
		int invalidCell = PathFinder.InvalidCell;
		NavType nav_type = NavType.NumNavTypes;
		query.ClearResult();
		if (!Grid.IsValidCell(potential_path.cell))
		{
			return;
		}
		PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref nav_type);
		if (invalidCell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, nav_type, out flag);
			query.SetResult(invalidCell, cell.cost, nav_type);
		}
	}

	// Token: 0x0600243C RID: 9276 RVA: 0x000BBF12 File Offset: 0x000BA112
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query);
		if (query.GetResultCell() != PathFinder.InvalidCell)
		{
			PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
			return;
		}
		path.Clear();
	}

	// Token: 0x0600243D RID: 9277 RVA: 0x001D5E10 File Offset: 0x001D4010
	private static void BuildResultPath(int path_cell, NavType path_nav_type, ref PathFinder.Path path)
	{
		if (path_cell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out flag);
			path.Clear();
			path.cost = cell.cost;
			while (path_cell != PathFinder.InvalidCell)
			{
				path.AddNode(new PathFinder.Path.Node
				{
					cell = path_cell,
					navType = cell.navType,
					transitionId = cell.transitionId
				});
				path_cell = cell.parent;
				if (path_cell != PathFinder.InvalidCell)
				{
					cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
				}
			}
			if (path.nodes != null)
			{
				for (int i = 0; i < path.nodes.Count / 2; i++)
				{
					PathFinder.Path.Node value = path.nodes[i];
					path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
					path.nodes[path.nodes.Count - i - 1] = value;
				}
			}
		}
	}

	// Token: 0x0600243E RID: 9278 RVA: 0x001D5F1C File Offset: 0x001D411C
	private static void FindPaths(NavGrid nav_grid, ref PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, PathFinder.PotentialList potentials, ref int result_cell, ref NavType result_nav_type)
	{
		potentials.Clear();
		PathFinder.PathGrid.ResetUpdate();
		PathFinder.PathGrid.BeginUpdate(potential_path.cell, false);
		bool flag;
		PathFinder.Cell cell = PathFinder.PathGrid.GetCell(potential_path, out flag);
		PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, PathFinder.PathGrid, ref cell);
		int num = int.MaxValue;
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = PathFinder.PathGrid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				if (cell.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) && cell.cost < num)
				{
					result_cell = keyValuePair.Value.cell;
					num = cell.cost;
					result_nav_type = cell.navType;
					break;
				}
				PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell.parent, cell.parentNavType);
			}
		}
		PathFinder.PathGrid.EndUpdate(true);
	}

	// Token: 0x0600243F RID: 9279 RVA: 0x000BBF45 File Offset: 0x000BA145
	public static void AddPotential(PathFinder.PotentialPath potential_path, int parent_cell, NavType parent_nav_type, int cost, byte transition_id, PathFinder.PotentialList potentials, PathGrid path_grid, ref PathFinder.Cell cell_data)
	{
		cell_data.cost = cost;
		cell_data.parent = parent_cell;
		cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
		cell_data.transitionId = transition_id;
		potentials.Add(cost, potential_path);
		path_grid.SetCell(potential_path, ref cell_data);
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void BeginDetailSample(string region_name)
	{
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void EndDetailSample(string region_name)
	{
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x001D6058 File Offset: 0x001D4258
	public static bool IsSubmerged(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num = Grid.CellAbove(cell);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsLiquid) || (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Solid[num]);
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x001D60B4 File Offset: 0x001D42B4
	public static void AddPotentials(PathFinder.PotentialScratchPad potential_scratch_pad, PathFinder.PotentialPath potential, int cost, ref PathFinderAbilities abilities, PathFinderQuery query, int max_links_per_cell, NavGrid.Link[] links, PathFinder.PotentialList potentials, PathGrid path_grid, int parent_cell, NavType parent_nav_type)
	{
		if (!Grid.IsValidCell(potential.cell))
		{
			return;
		}
		int num = 0;
		NavGrid.Link[] linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
		int num2 = potential.cell * max_links_per_cell;
		NavGrid.Link link = links[num2];
		for (int link2 = link.link; link2 != PathFinder.InvalidHandle; link2 = link.link)
		{
			if (link.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link.startNavType))
			{
				linksWithCorrectNavType[num++] = link;
			}
			num2++;
			link = links[num2];
		}
		int num3 = 0;
		PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
		for (int i = 0; i < num; i++)
		{
			NavGrid.Link link3 = linksWithCorrectNavType[i];
			int link4 = link3.link;
			bool flag = false;
			PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out flag);
			if (flag)
			{
				int num4 = cost + (int)link3.cost;
				bool flag2 = cell.cost == -1;
				bool flag3 = num4 < cell.cost;
				if (flag2 || flag3)
				{
					linksInCellRange[num3++] = new PathFinder.PotentialScratchPad.PathGridCellData
					{
						pathGridCell = cell,
						link = link3
					};
				}
			}
		}
		for (int j = 0; j < num3; j++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[j];
			int link5 = pathGridCellData.link.link;
			pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link5);
			linksInCellRange[j] = pathGridCellData;
		}
		for (int k = 0; k < num3; k++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData2 = linksInCellRange[k];
			NavGrid.Link link6 = pathGridCellData2.link;
			int link7 = link6.link;
			PathFinder.Cell pathGridCell = pathGridCellData2.pathGridCell;
			int num5 = cost + (int)link6.cost;
			PathFinder.PotentialPath potentialPath = potential;
			potentialPath.cell = link7;
			potentialPath.navType = link6.endNavType;
			if (pathGridCellData2.isSubmerged)
			{
				int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
				num5 += submergedPathCostPenalty;
			}
			PathFinder.PotentialPath.Flags flags = potentialPath.flags;
			bool flag4 = abilities.TraversePath(ref potentialPath, potential.cell, potential.navType, num5, (int)link6.transitionId, pathGridCellData2.isSubmerged);
			PathFinder.PotentialPath.Flags flags2 = potentialPath.flags;
			if (flag4)
			{
				PathFinder.AddPotential(potentialPath, potential.cell, potential.navType, num5, link6.transitionId, potentials, path_grid, ref pathGridCell);
			}
		}
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x000BBF81 File Offset: 0x000BA181
	public static void DestroyStatics()
	{
		PathFinder.PathGrid.OnCleanUp();
		PathFinder.PathGrid = null;
		PathFinder.Temp.Potentials.Clear();
	}

	// Token: 0x040018A6 RID: 6310
	public static int InvalidHandle = -1;

	// Token: 0x040018A7 RID: 6311
	public static int InvalidIdx = -1;

	// Token: 0x040018A8 RID: 6312
	public static int InvalidCell = -1;

	// Token: 0x040018A9 RID: 6313
	public static PathGrid PathGrid;

	// Token: 0x040018AA RID: 6314
	private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell)
	{
		if (Grid.Solid[cell])
		{
			return false;
		}
		if (Grid.AllowPathfinding[cell])
		{
			return false;
		}
		Grid.AllowPathfinding[cell] = true;
		return true;
	};

	// Token: 0x02000807 RID: 2055
	public struct Cell
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x000BBFC6 File Offset: 0x000BA1C6
		public NavType navType
		{
			get
			{
				return (NavType)(this.navTypes & 15);
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x000BBFD2 File Offset: 0x000BA1D2
		public NavType parentNavType
		{
			get
			{
				return (NavType)(this.navTypes >> 4);
			}
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x001D6300 File Offset: 0x001D4500
		public void SetNavTypes(NavType type, NavType parent_type)
		{
			this.navTypes = (byte)(type | parent_type << 4);
		}

		// Token: 0x040018AB RID: 6315
		public int cost;

		// Token: 0x040018AC RID: 6316
		public int parent;

		// Token: 0x040018AD RID: 6317
		public short queryId;

		// Token: 0x040018AE RID: 6318
		private byte navTypes;

		// Token: 0x040018AF RID: 6319
		public byte transitionId;
	}

	// Token: 0x02000808 RID: 2056
	public struct PotentialPath
	{
		// Token: 0x0600244A RID: 9290 RVA: 0x000BBFDD File Offset: 0x000BA1DD
		public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
		{
			this.cell = cell;
			this.navType = nav_type;
			this.flags = flags;
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x000BBFF4 File Offset: 0x000BA1F4
		public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags |= new_flags;
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000BC004 File Offset: 0x000BA204
		public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags &= ~new_flags;
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000BC016 File Offset: 0x000BA216
		public bool HasFlag(PathFinder.PotentialPath.Flags flag)
		{
			return this.HasAnyFlag(flag);
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000BC01F File Offset: 0x000BA21F
		public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
		{
			return (this.flags & mask) > PathFinder.PotentialPath.Flags.None;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x000BC02C File Offset: 0x000BA22C
		// (set) Token: 0x06002450 RID: 9296 RVA: 0x000BC034 File Offset: 0x000BA234
		public PathFinder.PotentialPath.Flags flags { readonly get; private set; }

		// Token: 0x040018B0 RID: 6320
		public int cell;

		// Token: 0x040018B1 RID: 6321
		public NavType navType;

		// Token: 0x02000809 RID: 2057
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x040018B4 RID: 6324
			None = 0,
			// Token: 0x040018B5 RID: 6325
			HasAtmoSuit = 1,
			// Token: 0x040018B6 RID: 6326
			HasJetPack = 2,
			// Token: 0x040018B7 RID: 6327
			HasOxygenMask = 4,
			// Token: 0x040018B8 RID: 6328
			PerformSuitChecks = 8,
			// Token: 0x040018B9 RID: 6329
			HasLeadSuit = 16
		}
	}

	// Token: 0x0200080A RID: 2058
	public struct Path
	{
		// Token: 0x06002451 RID: 9297 RVA: 0x000BC03D File Offset: 0x000BA23D
		public void AddNode(PathFinder.Path.Node node)
		{
			if (this.nodes == null)
			{
				this.nodes = new List<PathFinder.Path.Node>();
			}
			this.nodes.Add(node);
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000BC05E File Offset: 0x000BA25E
		public bool IsValid()
		{
			return this.nodes != null && this.nodes.Count > 1;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000BC078 File Offset: 0x000BA278
		public bool HasArrived()
		{
			return this.nodes != null && this.nodes.Count > 0;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000BC092 File Offset: 0x000BA292
		public void Clear()
		{
			this.cost = 0;
			if (this.nodes != null)
			{
				this.nodes.Clear();
			}
		}

		// Token: 0x040018BA RID: 6330
		public int cost;

		// Token: 0x040018BB RID: 6331
		public List<PathFinder.Path.Node> nodes;

		// Token: 0x0200080B RID: 2059
		public struct Node
		{
			// Token: 0x040018BC RID: 6332
			public int cell;

			// Token: 0x040018BD RID: 6333
			public NavType navType;

			// Token: 0x040018BE RID: 6334
			public byte transitionId;
		}
	}

	// Token: 0x0200080C RID: 2060
	public class PotentialList
	{
		// Token: 0x06002455 RID: 9301 RVA: 0x000BC0AE File Offset: 0x000BA2AE
		public KeyValuePair<int, PathFinder.PotentialPath> Next()
		{
			return this.queue.Dequeue();
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x000BC0BB File Offset: 0x000BA2BB
		public int Count
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000BC0C8 File Offset: 0x000BA2C8
		public void Add(int cost, PathFinder.PotentialPath path)
		{
			this.queue.Enqueue(cost, path);
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000BC0D7 File Offset: 0x000BA2D7
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x040018BF RID: 6335
		private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

		// Token: 0x0200080D RID: 2061
		public class PriorityQueue<TValue>
		{
			// Token: 0x0600245A RID: 9306 RVA: 0x000BC0F7 File Offset: 0x000BA2F7
			public PriorityQueue()
			{
				this._baseHeap = new List<KeyValuePair<int, TValue>>();
			}

			// Token: 0x0600245B RID: 9307 RVA: 0x000BC10A File Offset: 0x000BA30A
			public void Enqueue(int priority, TValue value)
			{
				this.Insert(priority, value);
			}

			// Token: 0x0600245C RID: 9308 RVA: 0x000BC114 File Offset: 0x000BA314
			public KeyValuePair<int, TValue> Dequeue()
			{
				KeyValuePair<int, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}

			// Token: 0x0600245D RID: 9309 RVA: 0x000BC128 File Offset: 0x000BA328
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.Count > 0)
				{
					return this._baseHeap[0];
				}
				throw new InvalidOperationException("Priority queue is empty");
			}

			// Token: 0x0600245E RID: 9310 RVA: 0x001D6320 File Offset: 0x001D4520
			private void ExchangeElements(int pos1, int pos2)
			{
				KeyValuePair<int, TValue> value = this._baseHeap[pos1];
				this._baseHeap[pos1] = this._baseHeap[pos2];
				this._baseHeap[pos2] = value;
			}

			// Token: 0x0600245F RID: 9311 RVA: 0x001D6360 File Offset: 0x001D4560
			private void Insert(int priority, TValue value)
			{
				KeyValuePair<int, TValue> item = new KeyValuePair<int, TValue>(priority, value);
				this._baseHeap.Add(item);
				this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
			}

			// Token: 0x06002460 RID: 9312 RVA: 0x001D6398 File Offset: 0x001D4598
			private int HeapifyFromEndToBeginning(int pos)
			{
				if (pos >= this._baseHeap.Count)
				{
					return -1;
				}
				while (pos > 0)
				{
					int num = (pos - 1) / 2;
					if (this._baseHeap[num].Key - this._baseHeap[pos].Key <= 0)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
				return pos;
			}

			// Token: 0x06002461 RID: 9313 RVA: 0x001D63F8 File Offset: 0x001D45F8
			private void DeleteRoot()
			{
				if (this._baseHeap.Count <= 1)
				{
					this._baseHeap.Clear();
					return;
				}
				this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
				this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
				this.HeapifyFromBeginningToEnd(0);
			}

			// Token: 0x06002462 RID: 9314 RVA: 0x001D6464 File Offset: 0x001D4664
			private void HeapifyFromBeginningToEnd(int pos)
			{
				int count = this._baseHeap.Count;
				if (pos >= count)
				{
					return;
				}
				for (;;)
				{
					int num = pos;
					int num2 = 2 * pos + 1;
					int num3 = 2 * pos + 2;
					if (num2 < count && this._baseHeap[num].Key - this._baseHeap[num2].Key > 0)
					{
						num = num2;
					}
					if (num3 < count && this._baseHeap[num].Key - this._baseHeap[num3].Key > 0)
					{
						num = num3;
					}
					if (num == pos)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
			}

			// Token: 0x06002463 RID: 9315 RVA: 0x000BC14A File Offset: 0x000BA34A
			public void Clear()
			{
				this._baseHeap.Clear();
			}

			// Token: 0x17000111 RID: 273
			// (get) Token: 0x06002464 RID: 9316 RVA: 0x000BC157 File Offset: 0x000BA357
			public int Count
			{
				get
				{
					return this._baseHeap.Count;
				}
			}

			// Token: 0x040018C0 RID: 6336
			private List<KeyValuePair<int, TValue>> _baseHeap;
		}

		// Token: 0x0200080E RID: 2062
		private class HOTQueue<TValue>
		{
			// Token: 0x06002465 RID: 9317 RVA: 0x001D650C File Offset: 0x001D470C
			public KeyValuePair<int, TValue> Dequeue()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				this.count--;
				return this.hotQueue.Dequeue();
			}

			// Token: 0x06002466 RID: 9318 RVA: 0x001D6568 File Offset: 0x001D4768
			public void Enqueue(int priority, TValue value)
			{
				if (priority <= this.hotThreshold)
				{
					this.hotQueue.Enqueue(priority, value);
				}
				else
				{
					this.coldQueue.Enqueue(priority, value);
					this.coldThreshold = Math.Max(this.coldThreshold, priority);
				}
				this.count++;
			}

			// Token: 0x06002467 RID: 9319 RVA: 0x001D65BC File Offset: 0x001D47BC
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				return this.hotQueue.Peek();
			}

			// Token: 0x06002468 RID: 9320 RVA: 0x000BC164 File Offset: 0x000BA364
			public void Clear()
			{
				this.count = 0;
				this.hotThreshold = int.MinValue;
				this.hotQueue.Clear();
				this.coldThreshold = int.MinValue;
				this.coldQueue.Clear();
			}

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06002469 RID: 9321 RVA: 0x000BC199 File Offset: 0x000BA399
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x040018C1 RID: 6337
			private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x040018C2 RID: 6338
			private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x040018C3 RID: 6339
			private int hotThreshold = int.MinValue;

			// Token: 0x040018C4 RID: 6340
			private int coldThreshold = int.MinValue;

			// Token: 0x040018C5 RID: 6341
			private int count;
		}
	}

	// Token: 0x0200080F RID: 2063
	private class Temp
	{
		// Token: 0x040018C6 RID: 6342
		public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
	}

	// Token: 0x02000810 RID: 2064
	public class PotentialScratchPad
	{
		// Token: 0x0600246D RID: 9325 RVA: 0x000BC1E1 File Offset: 0x000BA3E1
		public PotentialScratchPad(int max_links_per_cell)
		{
			this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
			this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
		}

		// Token: 0x040018C7 RID: 6343
		public NavGrid.Link[] linksWithCorrectNavType;

		// Token: 0x040018C8 RID: 6344
		public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

		// Token: 0x02000811 RID: 2065
		public struct PathGridCellData
		{
			// Token: 0x040018C9 RID: 6345
			public PathFinder.Cell pathGridCell;

			// Token: 0x040018CA RID: 6346
			public NavGrid.Link link;

			// Token: 0x040018CB RID: 6347
			public bool isSubmerged;
		}
	}
}
