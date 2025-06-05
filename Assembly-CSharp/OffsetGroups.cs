using System;
using System.Collections.Generic;

// Token: 0x020016C8 RID: 5832
public static class OffsetGroups
{
	// Token: 0x06007861 RID: 30817 RVA: 0x0031E330 File Offset: 0x0031C530
	public static CellOffset[] InitGrid(int x0, int x1, int y0, int y1)
	{
		List<CellOffset> list = new List<CellOffset>();
		for (int i = y0; i <= y1; i++)
		{
			for (int j = x0; j <= x1; j++)
			{
				list.Add(new CellOffset(j, i));
			}
		}
		CellOffset[] array = list.ToArray();
		Array.Sort<CellOffset>(array, 0, array.Length, new OffsetGroups.CellOffsetComparer());
		return array;
	}

	// Token: 0x06007862 RID: 30818 RVA: 0x0031E380 File Offset: 0x0031C580
	public static CellOffset[][] BuildReachabilityTable(CellOffset[] area_offsets, CellOffset[][] table, CellOffset[] filter)
	{
		Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>> dictionary = null;
		Dictionary<CellOffset[], CellOffset[][]> dictionary2 = null;
		CellOffset[][] array = null;
		if (OffsetGroups.reachabilityTableCache.TryGetValue(area_offsets, out dictionary) && dictionary.TryGetValue(table, out dictionary2) && dictionary2.TryGetValue((filter == null) ? OffsetGroups.nullFilter : filter, out array))
		{
			return array;
		}
		HashSet<CellOffset> hashSet = new HashSet<CellOffset>();
		foreach (CellOffset a in area_offsets)
		{
			foreach (CellOffset[] array2 in table)
			{
				if (filter == null || Array.IndexOf<CellOffset>(filter, array2[0]) == -1)
				{
					CellOffset item = a + array2[0];
					hashSet.Add(item);
				}
			}
		}
		List<CellOffset[]> list = new List<CellOffset[]>();
		foreach (CellOffset cellOffset in hashSet)
		{
			CellOffset b = area_offsets[0];
			foreach (CellOffset cellOffset2 in area_offsets)
			{
				if ((cellOffset - b).GetOffsetDistance() > (cellOffset - cellOffset2).GetOffsetDistance())
				{
					b = cellOffset2;
				}
			}
			foreach (CellOffset[] array3 in table)
			{
				if ((filter == null || Array.IndexOf<CellOffset>(filter, array3[0]) == -1) && array3[0] + b == cellOffset)
				{
					CellOffset[] array4 = new CellOffset[array3.Length];
					for (int k = 0; k < array3.Length; k++)
					{
						array4[k] = array3[k] + b;
					}
					list.Add(array4);
				}
			}
		}
		array = list.ToArray();
		Array.Sort<CellOffset[]>(array, (CellOffset[] x, CellOffset[] y) => x[0].GetOffsetDistance().CompareTo(y[0].GetOffsetDistance()));
		if (dictionary == null)
		{
			dictionary = new Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>();
			OffsetGroups.reachabilityTableCache.Add(area_offsets, dictionary);
		}
		if (dictionary2 == null)
		{
			dictionary2 = new Dictionary<CellOffset[], CellOffset[][]>();
			dictionary.Add(table, dictionary2);
		}
		dictionary2.Add((filter == null) ? OffsetGroups.nullFilter : filter, array);
		return array;
	}

	// Token: 0x04005A6E RID: 23150
	public static CellOffset[] Use = new CellOffset[1];

	// Token: 0x04005A6F RID: 23151
	public static CellOffset[] Chat = new CellOffset[]
	{
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1)
	};

	// Token: 0x04005A70 RID: 23152
	public static CellOffset[] LeftOnly = new CellOffset[]
	{
		new CellOffset(-1, 0)
	};

	// Token: 0x04005A71 RID: 23153
	public static CellOffset[] RightOnly = new CellOffset[]
	{
		new CellOffset(1, 0)
	};

	// Token: 0x04005A72 RID: 23154
	public static CellOffset[] LeftOrRight = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04005A73 RID: 23155
	public static CellOffset[] Standard = OffsetGroups.InitGrid(-2, 2, -3, 3);

	// Token: 0x04005A74 RID: 23156
	public static CellOffset[] LiquidSource = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0),
		new CellOffset(0, 1),
		new CellOffset(0, -1),
		new CellOffset(1, 1),
		new CellOffset(1, -1),
		new CellOffset(-1, 1),
		new CellOffset(-1, -1),
		new CellOffset(2, 0),
		new CellOffset(-2, 0)
	};

	// Token: 0x04005A75 RID: 23157
	public static CellOffset[][] InvertedStandardTable = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		}
	});

	// Token: 0x04005A76 RID: 23158
	public static CellOffset[][] InvertedStandardTableWithCorners = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1)
		}
	});

	// Token: 0x04005A77 RID: 23159
	public static CellOffset[][] InvertedWideTable = OffsetTable.Mirror(new CellOffset[][]
	{
		new CellOffset[]
		{
			new CellOffset(0, 0)
		},
		new CellOffset[]
		{
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(0, -2)
		},
		new CellOffset[]
		{
			new CellOffset(0, -3),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, 2),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, 3),
			new CellOffset(0, 3),
			new CellOffset(0, 2),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(1, -3),
			new CellOffset(1, -2),
			new CellOffset(0, -2),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, 2),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, 3),
			new CellOffset(1, 3),
			new CellOffset(1, 2),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(2, -1),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -2),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(2, -3),
			new CellOffset(1, -2),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, 0),
			new CellOffset(2, 0),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, 1),
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(0, 1)
		},
		new CellOffset[]
		{
			new CellOffset(3, 1),
			new CellOffset(2, 1),
			new CellOffset(1, 1),
			new CellOffset(1, 0)
		},
		new CellOffset[]
		{
			new CellOffset(3, -1),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(0, -1)
		},
		new CellOffset[]
		{
			new CellOffset(3, -1),
			new CellOffset(2, -1),
			new CellOffset(1, -1),
			new CellOffset(1, 0)
		}
	});

	// Token: 0x04005A78 RID: 23160
	private static Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>> reachabilityTableCache = new Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>>();

	// Token: 0x04005A79 RID: 23161
	private static readonly CellOffset[] nullFilter = new CellOffset[0];

	// Token: 0x020016C9 RID: 5833
	private class CellOffsetComparer : IComparer<CellOffset>
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x0031F798 File Offset: 0x0031D998
		public int Compare(CellOffset a, CellOffset b)
		{
			int num = Math.Abs(a.x) + Math.Abs(a.y);
			int value = Math.Abs(b.x) + Math.Abs(b.y);
			return num.CompareTo(value);
		}
	}
}
