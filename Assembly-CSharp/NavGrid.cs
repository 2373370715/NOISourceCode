using System;
using System.Collections.Generic;
using HUSL;
using UnityEngine;

// Token: 0x020007FB RID: 2043
public class NavGrid
{
	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000BBD24 File Offset: 0x000B9F24
	// (set) Token: 0x060023F8 RID: 9208 RVA: 0x000BBD2C File Offset: 0x000B9F2C
	public NavTable NavTable { get; private set; }

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000BBD35 File Offset: 0x000B9F35
	// (set) Token: 0x060023FA RID: 9210 RVA: 0x000BBD3D File Offset: 0x000B9F3D
	public NavGrid.Transition[] transitions { get; set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060023FB RID: 9211 RVA: 0x000BBD46 File Offset: 0x000B9F46
	// (set) Token: 0x060023FC RID: 9212 RVA: 0x000BBD4E File Offset: 0x000B9F4E
	public NavGrid.Transition[][] transitionsByNavType { get; private set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060023FD RID: 9213 RVA: 0x000BBD57 File Offset: 0x000B9F57
	// (set) Token: 0x060023FE RID: 9214 RVA: 0x000BBD5F File Offset: 0x000B9F5F
	public int updateRangeX { get; private set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060023FF RID: 9215 RVA: 0x000BBD68 File Offset: 0x000B9F68
	// (set) Token: 0x06002400 RID: 9216 RVA: 0x000BBD70 File Offset: 0x000B9F70
	public int updateRangeY { get; private set; }

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06002401 RID: 9217 RVA: 0x000BBD79 File Offset: 0x000B9F79
	// (set) Token: 0x06002402 RID: 9218 RVA: 0x000BBD81 File Offset: 0x000B9F81
	public int maxLinksPerCell { get; private set; }

	// Token: 0x06002403 RID: 9219 RVA: 0x000BBD8A File Offset: 0x000B9F8A
	public static NavType MirrorNavType(NavType nav_type)
	{
		if (nav_type == NavType.LeftWall)
		{
			return NavType.RightWall;
		}
		if (nav_type == NavType.RightWall)
		{
			return NavType.LeftWall;
		}
		return nav_type;
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x001D4810 File Offset: 0x001D2A10
	public NavGrid(string id, NavGrid.Transition[] transitions, NavGrid.NavTypeData[] nav_type_data, CellOffset[] bounding_offsets, NavTableValidator[] validators, int update_range_x, int update_range_y, int max_links_per_cell)
	{
		this.DirtyBitFlags = new byte[(Grid.CellCount + 7) / 8];
		this.DirtyCells = new List<int>();
		this.id = id;
		this.Validators = validators;
		this.navTypeData = nav_type_data;
		this.transitions = transitions;
		this.boundingOffsets = bounding_offsets;
		List<NavType> list = new List<NavType>();
		this.updateRangeX = update_range_x;
		this.updateRangeY = update_range_y;
		this.maxLinksPerCell = max_links_per_cell + 1;
		for (int i = 0; i < transitions.Length; i++)
		{
			DebugUtil.Assert(i >= 0 && i <= 255);
			transitions[i].id = (byte)i;
			if (!list.Contains(transitions[i].start))
			{
				list.Add(transitions[i].start);
			}
			if (!list.Contains(transitions[i].end))
			{
				list.Add(transitions[i].end);
			}
		}
		this.ValidNavTypes = list.ToArray();
		this.DebugViewLinkType = new bool[this.ValidNavTypes.Length];
		this.DebugViewValidCellsType = new bool[this.ValidNavTypes.Length];
		foreach (NavType nav_type in this.ValidNavTypes)
		{
			this.GetNavTypeData(nav_type);
		}
		this.Links = new NavGrid.Link[this.maxLinksPerCell * Grid.CellCount];
		this.NavTable = new NavTable(Grid.CellCount);
		this.transitions = transitions;
		this.transitionsByNavType = new NavGrid.Transition[11][];
		for (int k = 0; k < 11; k++)
		{
			List<NavGrid.Transition> list2 = new List<NavGrid.Transition>();
			NavType navType = (NavType)k;
			foreach (NavGrid.Transition transition in transitions)
			{
				if (transition.start == navType)
				{
					list2.Add(transition);
				}
			}
			this.transitionsByNavType[k] = list2.ToArray();
		}
		foreach (NavTableValidator navTableValidator in validators)
		{
			navTableValidator.onDirty = (Action<int>)Delegate.Combine(navTableValidator.onDirty, new Action<int>(this.AddDirtyCell));
		}
		this.potentialScratchPad = new PathFinder.PotentialScratchPad(this.maxLinksPerCell);
		this.InitializeGraph();
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x001D4A5C File Offset: 0x001D2C5C
	public NavGrid.NavTypeData GetNavTypeData(NavType nav_type)
	{
		foreach (NavGrid.NavTypeData navTypeData in this.navTypeData)
		{
			if (navTypeData.navType == nav_type)
			{
				return navTypeData;
			}
		}
		throw new Exception("Missing nav type data for nav type:" + nav_type.ToString());
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x001D4AB0 File Offset: 0x001D2CB0
	public bool HasNavTypeData(NavType nav_type)
	{
		NavGrid.NavTypeData[] array = this.navTypeData;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].navType == nav_type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000BBD99 File Offset: 0x000B9F99
	public HashedString GetIdleAnim(NavType nav_type)
	{
		return this.GetNavTypeData(nav_type).idleAnim;
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000BBDA7 File Offset: 0x000B9FA7
	public void InitializeGraph()
	{
		NavGridUpdater.InitializeNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType);
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x001D4AE4 File Offset: 0x001D2CE4
	public void UpdateGraph()
	{
		int count = this.DirtyCells.Count;
		for (int i = 0; i < count; i++)
		{
			int num;
			int num2;
			Grid.CellToXY(this.DirtyCells[i], out num, out num2);
			int num3 = Grid.ClampX(num - this.updateRangeX);
			int num4 = Grid.ClampY(num2 - this.updateRangeY);
			int num5 = Grid.ClampX(num + this.updateRangeX);
			int num6 = Grid.ClampY(num2 + this.updateRangeY);
			for (int j = num4; j <= num6; j++)
			{
				for (int k = num3; k <= num5; k++)
				{
					this.AddDirtyCell(Grid.XYToCell(k, j));
				}
			}
		}
		this.UpdateGraph(this.DirtyCells);
		foreach (int num7 in this.DirtyCells)
		{
			this.DirtyBitFlags[num7 / 8] = 0;
		}
		this.DirtyCells.Clear();
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x001D4BF4 File Offset: 0x001D2DF4
	public void UpdateGraph(IEnumerable<int> dirty_nav_cells)
	{
		NavGridUpdater.UpdateNavGrid(this.NavTable, this.Validators, this.boundingOffsets, this.maxLinksPerCell, this.Links, this.transitionsByNavType, this.teleportTransitions, dirty_nav_cells);
		if (this.OnNavGridUpdateComplete != null)
		{
			this.OnNavGridUpdateComplete(dirty_nav_cells);
		}
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000BBDD2 File Offset: 0x000B9FD2
	public static void DebugDrawPath(int start_cell, int end_cell)
	{
		Grid.CellToPosCCF(start_cell, Grid.SceneLayer.Move);
		Grid.CellToPosCCF(end_cell, Grid.SceneLayer.Move);
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x001D4C48 File Offset: 0x001D2E48
	public static void DebugDrawPath(PathFinder.Path path)
	{
		if (path.nodes != null)
		{
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				NavGrid.DebugDrawPath(path.nodes[i].cell, path.nodes[i + 1].cell);
			}
		}
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x001D4CA0 File Offset: 0x001D2EA0
	private void DebugDrawValidCells()
	{
		Color white = Color.white;
		int cellCount = Grid.CellCount;
		for (int i = 0; i < cellCount; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				NavType nav_type = (NavType)j;
				if (this.NavTable.IsValid(i, nav_type) && this.DrawNavTypeCell(nav_type, ref white))
				{
					DebugExtension.DebugPoint(NavTypeHelper.GetNavPos(i, nav_type), white, 1f, 0f, false);
				}
			}
		}
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x001D4D0C File Offset: 0x001D2F0C
	private void DebugDrawLinks()
	{
		Color white = Color.white;
		for (int i = 0; i < Grid.CellCount; i++)
		{
			int num = i * this.maxLinksPerCell;
			for (int link = this.Links[num].link; link != NavGrid.InvalidCell; link = this.Links[num].link)
			{
				NavTypeHelper.GetNavPos(i, this.Links[num].startNavType);
				if (this.DrawNavTypeLink(this.Links[num].startNavType, ref white) || this.DrawNavTypeLink(this.Links[num].endNavType, ref white))
				{
					NavTypeHelper.GetNavPos(link, this.Links[num].endNavType);
				}
				num++;
			}
		}
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x001D4DDC File Offset: 0x001D2FDC
	private bool DrawNavTypeLink(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewLinksAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewLinkType[i];
			}
		}
		return false;
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x001D4E28 File Offset: 0x001D3028
	private bool DrawNavTypeCell(NavType nav_type, ref Color color)
	{
		color = this.NavTypeColor(nav_type);
		if (this.DebugViewValidCellsAll)
		{
			return true;
		}
		for (int i = 0; i < this.ValidNavTypes.Length; i++)
		{
			if (this.ValidNavTypes[i] == nav_type)
			{
				return this.DebugViewValidCellsType[i];
			}
		}
		return false;
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x000BBDE6 File Offset: 0x000B9FE6
	public void DebugUpdate()
	{
		if (this.DebugViewValidCells)
		{
			this.DebugDrawValidCells();
		}
		if (this.DebugViewLinks)
		{
			this.DebugDrawLinks();
		}
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x001D4E74 File Offset: 0x001D3074
	public void AddDirtyCell(int cell)
	{
		if (Grid.IsValidCell(cell) && ((int)this.DirtyBitFlags[cell / 8] & 1 << cell % 8) == 0)
		{
			this.DirtyCells.Add(cell);
			byte[] dirtyBitFlags = this.DirtyBitFlags;
			int num = cell / 8;
			dirtyBitFlags[num] |= (byte)(1 << cell % 8);
		}
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x001D4EC8 File Offset: 0x001D30C8
	public void Clear()
	{
		NavTableValidator[] validators = this.Validators;
		for (int i = 0; i < validators.Length; i++)
		{
			validators[i].Clear();
		}
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x001D4EF4 File Offset: 0x001D30F4
	public Color NavTypeColor(NavType navType)
	{
		if (this.debugColorLookup == null)
		{
			this.debugColorLookup = new Color[11];
			for (int i = 0; i < 11; i++)
			{
				double num = (double)i / 11.0;
				IList<double> list = ColorConverter.HUSLToRGB(new double[]
				{
					num * 360.0,
					100.0,
					50.0
				});
				this.debugColorLookup[i] = new Color((float)list[0], (float)list[1], (float)list[2]);
			}
		}
		return this.debugColorLookup[(int)navType];
	}

	// Token: 0x0400185B RID: 6235
	public bool DebugViewAllPaths;

	// Token: 0x0400185C RID: 6236
	public bool DebugViewValidCells;

	// Token: 0x0400185D RID: 6237
	public bool[] DebugViewValidCellsType;

	// Token: 0x0400185E RID: 6238
	public bool DebugViewValidCellsAll;

	// Token: 0x0400185F RID: 6239
	public bool DebugViewLinks;

	// Token: 0x04001860 RID: 6240
	public bool[] DebugViewLinkType;

	// Token: 0x04001861 RID: 6241
	public bool DebugViewLinksAll;

	// Token: 0x04001862 RID: 6242
	public static int InvalidHandle = -1;

	// Token: 0x04001863 RID: 6243
	public static int InvalidIdx = -1;

	// Token: 0x04001864 RID: 6244
	public static int InvalidCell = -1;

	// Token: 0x04001865 RID: 6245
	public Dictionary<int, int> teleportTransitions = new Dictionary<int, int>();

	// Token: 0x04001866 RID: 6246
	public NavGrid.Link[] Links;

	// Token: 0x04001868 RID: 6248
	private byte[] DirtyBitFlags;

	// Token: 0x04001869 RID: 6249
	private List<int> DirtyCells;

	// Token: 0x0400186A RID: 6250
	private NavTableValidator[] Validators = new NavTableValidator[0];

	// Token: 0x0400186B RID: 6251
	private CellOffset[] boundingOffsets;

	// Token: 0x0400186C RID: 6252
	public string id;

	// Token: 0x0400186D RID: 6253
	public bool updateEveryFrame;

	// Token: 0x0400186E RID: 6254
	public PathFinder.PotentialScratchPad potentialScratchPad;

	// Token: 0x0400186F RID: 6255
	public Action<IEnumerable<int>> OnNavGridUpdateComplete;

	// Token: 0x04001872 RID: 6258
	public NavType[] ValidNavTypes;

	// Token: 0x04001873 RID: 6259
	public NavGrid.NavTypeData[] navTypeData;

	// Token: 0x04001877 RID: 6263
	private Color[] debugColorLookup;

	// Token: 0x020007FC RID: 2044
	public struct Link
	{
		// Token: 0x06002416 RID: 9238 RVA: 0x000BBE18 File Offset: 0x000BA018
		public Link(int link, NavType start_nav_type, NavType end_nav_type, byte transition_id, byte cost)
		{
			this.link = link;
			this.startNavType = start_nav_type;
			this.endNavType = end_nav_type;
			this.transitionId = transition_id;
			this.cost = cost;
		}

		// Token: 0x04001878 RID: 6264
		public int link;

		// Token: 0x04001879 RID: 6265
		public NavType startNavType;

		// Token: 0x0400187A RID: 6266
		public NavType endNavType;

		// Token: 0x0400187B RID: 6267
		public byte transitionId;

		// Token: 0x0400187C RID: 6268
		public byte cost;
	}

	// Token: 0x020007FD RID: 2045
	public struct NavTypeData
	{
		// Token: 0x0400187D RID: 6269
		public NavType navType;

		// Token: 0x0400187E RID: 6270
		public Vector2 animControllerOffset;

		// Token: 0x0400187F RID: 6271
		public bool flipX;

		// Token: 0x04001880 RID: 6272
		public bool flipY;

		// Token: 0x04001881 RID: 6273
		public float rotation;

		// Token: 0x04001882 RID: 6274
		public HashedString idleAnim;
	}

	// Token: 0x020007FE RID: 2046
	public struct Transition
	{
		// Token: 0x06002417 RID: 9239 RVA: 0x001D4F9C File Offset: 0x001D319C
		public override string ToString()
		{
			return string.Format("{0}: {1}->{2} ({3}); offset {4},{5}", new object[]
			{
				this.id,
				this.start,
				this.end,
				this.startAxis,
				this.x,
				this.y
			});
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x001D5010 File Offset: 0x001D3210
		public Transition(NavType start, NavType end, int x, int y, NavAxis start_axis, bool is_looping, bool loop_has_pre, bool is_escape, int cost, string anim, CellOffset[] void_offsets, CellOffset[] solid_offsets, NavOffset[] valid_nav_offsets, NavOffset[] invalid_nav_offsets, bool critter = false, float animSpeed = 1f, bool useOffsetX = false)
		{
			DebugUtil.Assert(cost <= 255 && cost >= 0);
			this.id = byte.MaxValue;
			this.start = start;
			this.end = end;
			this.x = x;
			this.y = y;
			this.startAxis = start_axis;
			this.isLooping = is_looping;
			this.isEscape = is_escape;
			this.anim = anim;
			this.preAnim = "";
			this.cost = (byte)cost;
			if (string.IsNullOrEmpty(this.anim))
			{
				this.anim = string.Concat(new string[]
				{
					start.ToString().ToLower(),
					"_",
					end.ToString().ToLower(),
					"_",
					x.ToString(),
					"_",
					y.ToString()
				});
			}
			if (this.isLooping)
			{
				if (loop_has_pre)
				{
					this.preAnim = this.anim + "_pre";
				}
				this.anim += "_loop";
			}
			if (this.startAxis != NavAxis.NA)
			{
				this.anim += ((this.startAxis == NavAxis.X) ? "_x" : "_y");
			}
			this.voidOffsets = void_offsets;
			this.solidOffsets = solid_offsets;
			this.validNavOffsets = valid_nav_offsets;
			this.invalidNavOffsets = invalid_nav_offsets;
			this.isCritter = critter;
			this.useXOffset = useOffsetX;
			this.animSpeed = animSpeed;
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x001D51A4 File Offset: 0x001D33A4
		public int IsValid(int cell, NavTable nav_table)
		{
			if (!Grid.IsCellOffsetValid(cell, this.x, this.y))
			{
				return Grid.InvalidCell;
			}
			int num = Grid.OffsetCell(cell, this.x, this.y);
			if (!nav_table.IsValid(num, this.end))
			{
				return Grid.InvalidCell;
			}
			Grid.BuildFlags buildFlags = Grid.BuildFlags.Solid | Grid.BuildFlags.DupeImpassable;
			if (this.isCritter)
			{
				buildFlags |= Grid.BuildFlags.CritterImpassable;
			}
			foreach (CellOffset cellOffset in this.voidOffsets)
			{
				int num2 = Grid.OffsetCell(cell, cellOffset.x, cellOffset.y);
				if (Grid.IsValidCell(num2) && (Grid.BuildMasks[num2] & buildFlags) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
				{
					if (this.isCritter)
					{
						return Grid.InvalidCell;
					}
					if ((Grid.BuildMasks[num2] & Grid.BuildFlags.DupePassable) == ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
					{
						return Grid.InvalidCell;
					}
				}
			}
			foreach (CellOffset cellOffset2 in this.solidOffsets)
			{
				int num3 = Grid.OffsetCell(cell, cellOffset2.x, cellOffset2.y);
				if (Grid.IsValidCell(num3) && !Grid.Solid[num3])
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset in this.validNavOffsets)
			{
				int cell2 = Grid.OffsetCell(cell, navOffset.offset.x, navOffset.offset.y);
				if (!nav_table.IsValid(cell2, navOffset.navType))
				{
					return Grid.InvalidCell;
				}
			}
			foreach (NavOffset navOffset2 in this.invalidNavOffsets)
			{
				int cell3 = Grid.OffsetCell(cell, navOffset2.offset.x, navOffset2.offset.y);
				if (nav_table.IsValid(cell3, navOffset2.navType))
				{
					return Grid.InvalidCell;
				}
			}
			if (this.start == NavType.Tube)
			{
				if (this.end == NavType.Tube)
				{
					GameObject gameObject = Grid.Objects[cell, 9];
					GameObject gameObject2 = Grid.Objects[num, 9];
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink = gameObject ? gameObject.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					TravelTubeUtilityNetworkLink travelTubeUtilityNetworkLink2 = gameObject2 ? gameObject2.GetComponent<TravelTubeUtilityNetworkLink>() : null;
					if (travelTubeUtilityNetworkLink)
					{
						int num4;
						int num5;
						travelTubeUtilityNetworkLink.GetCells(out num4, out num5);
						if (num != num4 && num != num5)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, num);
						if (utilityConnections == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(num, false) != utilityConnections)
						{
							return Grid.InvalidCell;
						}
					}
					else if (travelTubeUtilityNetworkLink2)
					{
						int num6;
						int num7;
						travelTubeUtilityNetworkLink2.GetCells(out num6, out num7);
						if (cell != num6 && cell != num7)
						{
							return Grid.InvalidCell;
						}
						UtilityConnections utilityConnections2 = UtilityConnectionsExtensions.DirectionFromToCell(num, cell);
						if (utilityConnections2 == (UtilityConnections)0)
						{
							return Grid.InvalidCell;
						}
						if (Game.Instance.travelTubeSystem.GetConnections(cell, false) != utilityConnections2)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						bool flag = this.startAxis == NavAxis.X;
						int cell4 = cell;
						for (int j = 0; j < 2; j++)
						{
							if ((flag && j == 0) || (!flag && j == 1))
							{
								int num8 = (this.x > 0) ? 1 : -1;
								for (int k = 0; k < Mathf.Abs(this.x); k++)
								{
									UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num8 > 0 && (connections & UtilityConnections.Right) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num8 < 0 && (connections & UtilityConnections.Left) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, num8, 0);
								}
							}
							else
							{
								int num9 = (this.y > 0) ? 1 : -1;
								for (int l = 0; l < Mathf.Abs(this.y); l++)
								{
									UtilityConnections connections2 = Game.Instance.travelTubeSystem.GetConnections(cell4, false);
									if (num9 > 0 && (connections2 & UtilityConnections.Up) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									if (num9 < 0 && (connections2 & UtilityConnections.Down) == (UtilityConnections)0)
									{
										return Grid.InvalidCell;
									}
									cell4 = Grid.OffsetCell(cell4, 0, num9);
								}
							}
						}
					}
				}
				else
				{
					UtilityConnections connections3 = Game.Instance.travelTubeSystem.GetConnections(cell, false);
					if (this.y > 0)
					{
						if (connections3 != UtilityConnections.Down)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x > 0)
					{
						if (connections3 != UtilityConnections.Left)
						{
							return Grid.InvalidCell;
						}
					}
					else if (this.x < 0)
					{
						if (connections3 != UtilityConnections.Right)
						{
							return Grid.InvalidCell;
						}
					}
					else
					{
						if (this.y >= 0)
						{
							return Grid.InvalidCell;
						}
						if (connections3 != UtilityConnections.Up)
						{
							return Grid.InvalidCell;
						}
					}
				}
			}
			else if (this.start == NavType.Floor && this.end == NavType.Tube)
			{
				int cell5 = Grid.OffsetCell(cell, this.x, this.y);
				if (Game.Instance.travelTubeSystem.GetConnections(cell5, false) != UtilityConnections.Up)
				{
					return Grid.InvalidCell;
				}
			}
			return num;
		}

		// Token: 0x04001883 RID: 6275
		public NavType start;

		// Token: 0x04001884 RID: 6276
		public NavType end;

		// Token: 0x04001885 RID: 6277
		public NavAxis startAxis;

		// Token: 0x04001886 RID: 6278
		public int x;

		// Token: 0x04001887 RID: 6279
		public int y;

		// Token: 0x04001888 RID: 6280
		public byte id;

		// Token: 0x04001889 RID: 6281
		public byte cost;

		// Token: 0x0400188A RID: 6282
		public bool isLooping;

		// Token: 0x0400188B RID: 6283
		public bool isEscape;

		// Token: 0x0400188C RID: 6284
		public string preAnim;

		// Token: 0x0400188D RID: 6285
		public string anim;

		// Token: 0x0400188E RID: 6286
		public float animSpeed;

		// Token: 0x0400188F RID: 6287
		public CellOffset[] voidOffsets;

		// Token: 0x04001890 RID: 6288
		public CellOffset[] solidOffsets;

		// Token: 0x04001891 RID: 6289
		public NavOffset[] validNavOffsets;

		// Token: 0x04001892 RID: 6290
		public NavOffset[] invalidNavOffsets;

		// Token: 0x04001893 RID: 6291
		public bool isCritter;

		// Token: 0x04001894 RID: 6292
		public bool useXOffset;
	}
}
