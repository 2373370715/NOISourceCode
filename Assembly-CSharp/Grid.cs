using System;
using System.Collections.Generic;
using System.Diagnostics;
using ProcGen;
using UnityEngine;

// Token: 0x020013DC RID: 5084
public class Grid
{
	// Token: 0x06006846 RID: 26694 RVA: 0x000E8A44 File Offset: 0x000E6C44
	private static void UpdateBuildMask(int i, Grid.BuildFlags flag, bool state)
	{
		if (state)
		{
			Grid.BuildMasks[i] |= flag;
			return;
		}
		Grid.BuildMasks[i] &= ~flag;
	}

	// Token: 0x06006847 RID: 26695 RVA: 0x000E8A6C File Offset: 0x000E6C6C
	public static void SetSolid(int cell, bool solid, CellSolidEvent ev)
	{
		Grid.UpdateBuildMask(cell, Grid.BuildFlags.Solid, solid);
	}

	// Token: 0x06006848 RID: 26696 RVA: 0x000E8A78 File Offset: 0x000E6C78
	private static void UpdateVisMask(int i, Grid.VisFlags flag, bool state)
	{
		if (state)
		{
			Grid.VisMasks[i] |= flag;
			return;
		}
		Grid.VisMasks[i] &= ~flag;
	}

	// Token: 0x06006849 RID: 26697 RVA: 0x000E8AA0 File Offset: 0x000E6CA0
	private static void UpdateNavValidatorMask(int i, Grid.NavValidatorFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavValidatorMasks[i] |= flag;
			return;
		}
		Grid.NavValidatorMasks[i] &= ~flag;
	}

	// Token: 0x0600684A RID: 26698 RVA: 0x000E8AC8 File Offset: 0x000E6CC8
	private static void UpdateNavMask(int i, Grid.NavFlags flag, bool state)
	{
		if (state)
		{
			Grid.NavMasks[i] |= flag;
			return;
		}
		Grid.NavMasks[i] &= ~flag;
	}

	// Token: 0x0600684B RID: 26699 RVA: 0x000E8AF0 File Offset: 0x000E6CF0
	public static void ResetNavMasksAndDetails()
	{
		Grid.NavMasks = null;
		Grid.tubeEntrances.Clear();
		Grid.restrictions.Clear();
		Grid.suitMarkers.Clear();
	}

	// Token: 0x0600684C RID: 26700 RVA: 0x000E8B16 File Offset: 0x000E6D16
	public static bool DEBUG_GetRestrictions(int cell, out Grid.Restriction restriction)
	{
		return Grid.restrictions.TryGetValue(cell, out restriction);
	}

	// Token: 0x0600684D RID: 26701 RVA: 0x002E6530 File Offset: 0x002E4730
	public static void RegisterRestriction(int cell, Grid.Restriction.Orientation orientation)
	{
		Grid.HasAccessDoor[cell] = true;
		Grid.restrictions[cell] = new Grid.Restriction
		{
			DirectionMasksForMinionInstanceID = new Dictionary<int, Grid.Restriction.Directions>(),
			orientation = orientation
		};
	}

	// Token: 0x0600684E RID: 26702 RVA: 0x000E8B24 File Offset: 0x000E6D24
	public static void UnregisterRestriction(int cell)
	{
		Grid.restrictions.Remove(cell);
		Grid.HasAccessDoor[cell] = false;
	}

	// Token: 0x0600684F RID: 26703 RVA: 0x000E8B3E File Offset: 0x000E6D3E
	public static void SetRestriction(int cell, int minionInstanceID, Grid.Restriction.Directions directions)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID[minionInstanceID] = directions;
	}

	// Token: 0x06006850 RID: 26704 RVA: 0x000E8B57 File Offset: 0x000E6D57
	public static void ClearRestriction(int cell, int minionInstanceID)
	{
		Grid.restrictions[cell].DirectionMasksForMinionInstanceID.Remove(minionInstanceID);
	}

	// Token: 0x06006851 RID: 26705 RVA: 0x002E6574 File Offset: 0x002E4774
	public static bool HasPermission(int cell, int minionInstanceID, int fromCell, NavType fromNavType)
	{
		if (!Grid.HasAccessDoor[cell])
		{
			return true;
		}
		Grid.Restriction restriction = Grid.restrictions[cell];
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = Grid.CellToXY(fromCell);
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		int num = vector2I.x - vector2I2.x;
		int num2 = vector2I.y - vector2I2.y;
		switch (restriction.orientation)
		{
		case Grid.Restriction.Orientation.Vertical:
			if (num < 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num > 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.Horizontal:
			if (num2 > 0)
			{
				directions |= Grid.Restriction.Directions.Left;
			}
			if (num2 < 0)
			{
				directions |= Grid.Restriction.Directions.Right;
			}
			break;
		case Grid.Restriction.Orientation.SingleCell:
			if (Math.Abs(num) != 1 && Math.Abs(num2) != 1 && fromNavType != NavType.Teleport)
			{
				directions |= Grid.Restriction.Directions.Teleport;
			}
			break;
		}
		Grid.Restriction.Directions directions2 = (Grid.Restriction.Directions)0;
		return (!restriction.DirectionMasksForMinionInstanceID.TryGetValue(minionInstanceID, out directions2) && !restriction.DirectionMasksForMinionInstanceID.TryGetValue(-1, out directions2)) || (directions2 & directions) == (Grid.Restriction.Directions)0;
	}

	// Token: 0x06006852 RID: 26706 RVA: 0x002E6654 File Offset: 0x002E4854
	public static void RegisterTubeEntrance(int cell, int reservationCapacity)
	{
		DebugUtil.Assert(!Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = true;
		Grid.tubeEntrances[cell] = new Grid.TubeEntrance
		{
			reservationCapacity = reservationCapacity,
			reservedInstanceIDs = new HashSet<int>()
		};
	}

	// Token: 0x06006853 RID: 26707 RVA: 0x000E8B70 File Offset: 0x000E6D70
	public static void UnregisterTubeEntrance(int cell)
	{
		DebugUtil.Assert(Grid.tubeEntrances.ContainsKey(cell));
		Grid.HasTubeEntrance[cell] = false;
		Grid.tubeEntrances.Remove(cell);
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x002E66A8 File Offset: 0x002E48A8
	public static bool ReserveTubeEntrance(int cell, int minionInstanceID, bool reserve)
	{
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		if (!reserve)
		{
			return reservedInstanceIDs.Remove(minionInstanceID);
		}
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		if (reservedInstanceIDs.Count == tubeEntrance.reservationCapacity)
		{
			return false;
		}
		DebugUtil.Assert(reservedInstanceIDs.Add(minionInstanceID));
		return true;
	}

	// Token: 0x06006855 RID: 26709 RVA: 0x002E6700 File Offset: 0x002E4900
	public static void SetTubeEntranceReservationCapacity(int cell, int newReservationCapacity)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.reservationCapacity = newReservationCapacity;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x002E6740 File Offset: 0x002E4940
	public static bool HasUsableTubeEntrance(int cell, int minionInstanceID)
	{
		if (!Grid.HasTubeEntrance[cell])
		{
			return false;
		}
		Grid.TubeEntrance tubeEntrance = Grid.tubeEntrances[cell];
		if (!tubeEntrance.operational)
		{
			return false;
		}
		HashSet<int> reservedInstanceIDs = tubeEntrance.reservedInstanceIDs;
		return reservedInstanceIDs.Count < tubeEntrance.reservationCapacity || reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x06006857 RID: 26711 RVA: 0x000E8B9A File Offset: 0x000E6D9A
	public static bool HasReservedTubeEntrance(int cell, int minionInstanceID)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		return Grid.tubeEntrances[cell].reservedInstanceIDs.Contains(minionInstanceID);
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x002E6790 File Offset: 0x002E4990
	public static void SetTubeEntranceOperational(int cell, bool operational)
	{
		DebugUtil.Assert(Grid.HasTubeEntrance[cell]);
		Grid.TubeEntrance value = Grid.tubeEntrances[cell];
		value.operational = operational;
		Grid.tubeEntrances[cell] = value;
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x002E67D0 File Offset: 0x002E49D0
	public static void RegisterSuitMarker(int cell)
	{
		DebugUtil.Assert(!Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = true;
		Grid.suitMarkers[cell] = new Grid.SuitMarker
		{
			suitCount = 0,
			lockerCount = 0,
			flags = Grid.SuitMarker.Flags.Operational,
			minionIDsWithSuitReservations = new HashSet<int>(),
			minionIDsWithEmptyLockerReservations = new HashSet<int>()
		};
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x000E8BC2 File Offset: 0x000E6DC2
	public static void UnregisterSuitMarker(int cell)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.HasSuitMarker[cell] = false;
		Grid.suitMarkers.Remove(cell);
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x002E6840 File Offset: 0x002E4A40
	public static bool ReserveSuit(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithSuitReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag);
			return flag;
		}
		if (minionIDsWithSuitReservations.Count >= suitMarker.suitCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithSuitReservations.Add(minionInstanceID));
		return true;
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x002E68A0 File Offset: 0x002E4AA0
	public static bool ReserveEmptyLocker(int cell, int minionInstanceID, bool reserve)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell], "No suit marker");
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		if (!reserve)
		{
			bool flag = minionIDsWithEmptyLockerReservations.Remove(minionInstanceID);
			DebugUtil.Assert(flag, "Reservation not removed");
			return flag;
		}
		if (minionIDsWithEmptyLockerReservations.Count >= suitMarker.emptyLockerCount)
		{
			return false;
		}
		DebugUtil.Assert(minionIDsWithEmptyLockerReservations.Add(minionInstanceID), "Reservation not made");
		return true;
	}

	// Token: 0x0600685D RID: 26717 RVA: 0x002E6910 File Offset: 0x002E4B10
	public static void UpdateSuitMarker(int cell, int fullLockerCount, int emptyLockerCount, Grid.SuitMarker.Flags flags, PathFinder.PotentialPath.Flags pathFlags)
	{
		DebugUtil.Assert(Grid.HasSuitMarker[cell]);
		Grid.SuitMarker value = Grid.suitMarkers[cell];
		value.suitCount = fullLockerCount;
		value.lockerCount = fullLockerCount + emptyLockerCount;
		value.flags = flags;
		value.pathFlags = pathFlags;
		Grid.suitMarkers[cell] = value;
	}

	// Token: 0x0600685E RID: 26718 RVA: 0x000E8BEC File Offset: 0x000E6DEC
	public static bool TryGetSuitMarkerFlags(int cell, out Grid.SuitMarker.Flags flags, out PathFinder.PotentialPath.Flags pathFlags)
	{
		if (Grid.HasSuitMarker[cell])
		{
			flags = Grid.suitMarkers[cell].flags;
			pathFlags = Grid.suitMarkers[cell].pathFlags;
			return true;
		}
		flags = (Grid.SuitMarker.Flags)0;
		pathFlags = PathFinder.PotentialPath.Flags.None;
		return false;
	}

	// Token: 0x0600685F RID: 26719 RVA: 0x002E6968 File Offset: 0x002E4B68
	public static bool HasSuit(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithSuitReservations = suitMarker.minionIDsWithSuitReservations;
		return minionIDsWithSuitReservations.Count < suitMarker.suitCount || minionIDsWithSuitReservations.Contains(minionInstanceID);
	}

	// Token: 0x06006860 RID: 26720 RVA: 0x002E69B0 File Offset: 0x002E4BB0
	public static bool HasEmptyLocker(int cell, int minionInstanceID)
	{
		if (!Grid.HasSuitMarker[cell])
		{
			return false;
		}
		Grid.SuitMarker suitMarker = Grid.suitMarkers[cell];
		HashSet<int> minionIDsWithEmptyLockerReservations = suitMarker.minionIDsWithEmptyLockerReservations;
		return minionIDsWithEmptyLockerReservations.Count < suitMarker.emptyLockerCount || minionIDsWithEmptyLockerReservations.Contains(minionInstanceID);
	}

	// Token: 0x06006861 RID: 26721 RVA: 0x002E69F8 File Offset: 0x002E4BF8
	public unsafe static void InitializeCells()
	{
		for (int num = 0; num != Grid.WidthInCells * Grid.HeightInCells; num++)
		{
			ushort index = Grid.elementIdx[num];
			Element element = ElementLoader.elements[(int)index];
			Grid.Element[num] = element;
			if (element.IsSolid)
			{
				Grid.BuildMasks[num] |= Grid.BuildFlags.Solid;
			}
			else
			{
				Grid.BuildMasks[num] &= ~Grid.BuildFlags.Solid;
			}
			Grid.RenderedByWorld[num] = (element.substance != null && element.substance.renderedByWorld && Grid.Objects[num, 9] == null);
		}
	}

	// Token: 0x06006862 RID: 26722 RVA: 0x000E8C28 File Offset: 0x000E6E28
	public static bool IsInitialized()
	{
		return Grid.mass != null;
	}

	// Token: 0x06006863 RID: 26723 RVA: 0x002E6AA8 File Offset: 0x002E4CA8
	public static int GetCellInDirection(int cell, Direction d)
	{
		switch (d)
		{
		case Direction.Up:
			return Grid.CellAbove(cell);
		case Direction.Right:
			return Grid.CellRight(cell);
		case Direction.Down:
			return Grid.CellBelow(cell);
		case Direction.Left:
			return Grid.CellLeft(cell);
		case Direction.None:
			return cell;
		}
		return -1;
	}

	// Token: 0x06006864 RID: 26724 RVA: 0x002E6AF4 File Offset: 0x002E4CF4
	public static bool Raycast(int cell, Vector2I direction, out int hitDistance, int maxDistance = 100, Grid.BuildFlags layerMask = Grid.BuildFlags.Any)
	{
		bool flag = false;
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = vector2I + direction * maxDistance;
		int num = cell;
		int num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
		int num3 = 0;
		int num4 = 0;
		float num5 = (float)maxDistance * 0.5f;
		while ((float)num3 < num5)
		{
			if (!Grid.IsValidCell(num) || (Grid.BuildMasks[num] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				flag = true;
				break;
			}
			if (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) != ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor))
			{
				num4 = maxDistance - num3;
			}
			vector2I += direction;
			vector2I2 -= direction;
			num = Grid.XYToCell(vector2I.x, vector2I.y);
			num2 = Grid.XYToCell(vector2I2.x, vector2I2.y);
			num3++;
		}
		if (!flag && maxDistance % 2 == 0)
		{
			flag = (!Grid.IsValidCell(num2) || (Grid.BuildMasks[num2] & layerMask) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor));
		}
		hitDistance = (flag ? num3 : ((num4 > 0) ? num4 : maxDistance));
		return flag | hitDistance == num4;
	}

	// Token: 0x06006865 RID: 26725 RVA: 0x000E8C36 File Offset: 0x000E6E36
	public static int CellAbove(int cell)
	{
		return cell + Grid.WidthInCells;
	}

	// Token: 0x06006866 RID: 26726 RVA: 0x000E8C3F File Offset: 0x000E6E3F
	public static int CellBelow(int cell)
	{
		return cell - Grid.WidthInCells;
	}

	// Token: 0x06006867 RID: 26727 RVA: 0x000E8C48 File Offset: 0x000E6E48
	public static int CellLeft(int cell)
	{
		if (cell % Grid.WidthInCells <= 0)
		{
			return Grid.InvalidCell;
		}
		return cell - 1;
	}

	// Token: 0x06006868 RID: 26728 RVA: 0x000E8C5D File Offset: 0x000E6E5D
	public static int CellRight(int cell)
	{
		if (cell % Grid.WidthInCells >= Grid.WidthInCells - 1)
		{
			return Grid.InvalidCell;
		}
		return cell + 1;
	}

	// Token: 0x06006869 RID: 26729 RVA: 0x002E6BF8 File Offset: 0x002E4DF8
	public static CellOffset GetOffset(int cell)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return new CellOffset(x, y);
	}

	// Token: 0x0600686A RID: 26730 RVA: 0x002E6C1C File Offset: 0x002E4E1C
	public static int CellUpLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600686B RID: 26731 RVA: 0x002E6C54 File Offset: 0x002E4E54
	public static int CellUpRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell < (Grid.HeightInCells - 1) * Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 + Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600686C RID: 26732 RVA: 0x002E6C94 File Offset: 0x002E4E94
	public static int CellDownLeft(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell > Grid.WidthInCells && cell % Grid.WidthInCells > 0)
		{
			result = cell - 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600686D RID: 26733 RVA: 0x002E6CC4 File Offset: 0x002E4EC4
	public static int CellDownRight(int cell)
	{
		int result = Grid.InvalidCell;
		if (cell >= Grid.WidthInCells && cell % Grid.WidthInCells < Grid.WidthInCells - 1)
		{
			result = cell + 1 - Grid.WidthInCells;
		}
		return result;
	}

	// Token: 0x0600686E RID: 26734 RVA: 0x000E8C78 File Offset: 0x000E6E78
	public static bool IsCellLeftOf(int cell, int other_cell)
	{
		return Grid.CellColumn(cell) < Grid.CellColumn(other_cell);
	}

	// Token: 0x0600686F RID: 26735 RVA: 0x002E6CFC File Offset: 0x002E4EFC
	public static bool IsCellOffsetOf(int cell, int target_cell, CellOffset[] target_offsets)
	{
		int num = target_offsets.Length;
		for (int i = 0; i < num; i++)
		{
			if (cell == Grid.OffsetCell(target_cell, target_offsets[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006870 RID: 26736 RVA: 0x002E6D2C File Offset: 0x002E4F2C
	public static int GetCellDistance(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Abs(num - num3) + Math.Abs(num2 - num4);
	}

	// Token: 0x06006871 RID: 26737 RVA: 0x002E6D60 File Offset: 0x002E4F60
	public static int GetCellRange(int cell_a, int cell_b)
	{
		int num;
		int num2;
		Grid.CellToXY(cell_a, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(cell_b, out num3, out num4);
		return Math.Max(Math.Abs(num - num3), Math.Abs(num2 - num4));
	}

	// Token: 0x06006872 RID: 26738 RVA: 0x002E6D98 File Offset: 0x002E4F98
	public static CellOffset GetOffset(int base_cell, int offset_cell)
	{
		int num;
		int num2;
		Grid.CellToXY(base_cell, out num, out num2);
		int num3;
		int num4;
		Grid.CellToXY(offset_cell, out num3, out num4);
		return new CellOffset(num3 - num, num4 - num2);
	}

	// Token: 0x06006873 RID: 26739 RVA: 0x002E6DC4 File Offset: 0x002E4FC4
	public static CellOffset GetCellOffsetDirection(int base_cell, int offset_cell)
	{
		CellOffset offset = Grid.GetOffset(base_cell, offset_cell);
		offset.x = Mathf.Clamp(offset.x, -1, 1);
		offset.y = Mathf.Clamp(offset.y, -1, 1);
		return offset;
	}

	// Token: 0x06006874 RID: 26740 RVA: 0x000E8C88 File Offset: 0x000E6E88
	public static int OffsetCell(int cell, CellOffset offset)
	{
		return cell + offset.x + offset.y * Grid.WidthInCells;
	}

	// Token: 0x06006875 RID: 26741 RVA: 0x000E8C9F File Offset: 0x000E6E9F
	public static int OffsetCell(int cell, int x, int y)
	{
		return cell + x + y * Grid.WidthInCells;
	}

	// Token: 0x06006876 RID: 26742 RVA: 0x002E6E04 File Offset: 0x002E5004
	public static bool IsCellOffsetValid(int cell, int x, int y)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return num + x >= 0 && num + x < Grid.WidthInCells && num2 + y >= 0 && num2 + y < Grid.HeightInCells;
	}

	// Token: 0x06006877 RID: 26743 RVA: 0x000E8CAC File Offset: 0x000E6EAC
	public static bool IsCellOffsetValid(int cell, CellOffset offset)
	{
		return Grid.IsCellOffsetValid(cell, offset.x, offset.y);
	}

	// Token: 0x06006878 RID: 26744 RVA: 0x000E8CC0 File Offset: 0x000E6EC0
	public static int PosToCell(StateMachine.Instance smi)
	{
		return Grid.PosToCell(smi.transform.GetPosition());
	}

	// Token: 0x06006879 RID: 26745 RVA: 0x000E8CD2 File Offset: 0x000E6ED2
	public static int PosToCell(GameObject go)
	{
		return Grid.PosToCell(go.transform.GetPosition());
	}

	// Token: 0x0600687A RID: 26746 RVA: 0x000E7767 File Offset: 0x000E5967
	public static int PosToCell(KMonoBehaviour cmp)
	{
		return Grid.PosToCell(cmp.transform.GetPosition());
	}

	// Token: 0x0600687B RID: 26747 RVA: 0x002E6E40 File Offset: 0x002E5040
	public static bool IsValidBuildingCell(int cell)
	{
		if (!Grid.IsWorldValidCell(cell))
		{
			return false;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
		if (world == null)
		{
			return false;
		}
		Vector2I vector2I = Grid.CellToXY(cell);
		return (float)vector2I.x >= world.minimumBounds.x && (float)vector2I.x <= world.maximumBounds.x && (float)vector2I.y >= world.minimumBounds.y && (float)vector2I.y <= world.maximumBounds.y - (float)Grid.TopBorderHeight;
	}

	// Token: 0x0600687C RID: 26748 RVA: 0x000E8CE4 File Offset: 0x000E6EE4
	public static bool IsWorldValidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.WorldIdx[cell] != byte.MaxValue;
	}

	// Token: 0x0600687D RID: 26749 RVA: 0x000E8D01 File Offset: 0x000E6F01
	public static bool IsValidCell(int cell)
	{
		return cell >= 0 && cell < Grid.CellCount;
	}

	// Token: 0x0600687E RID: 26750 RVA: 0x000E8D11 File Offset: 0x000E6F11
	public static bool IsValidCellInWorld(int cell, int world)
	{
		return cell >= 0 && cell < Grid.CellCount && (int)Grid.WorldIdx[cell] == world;
	}

	// Token: 0x0600687F RID: 26751 RVA: 0x000E8D2B File Offset: 0x000E6F2B
	public static bool IsActiveWorld(int cell)
	{
		return ClusterManager.Instance != null && ClusterManager.Instance.activeWorldId == (int)Grid.WorldIdx[cell];
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x000E8D4F File Offset: 0x000E6F4F
	public static bool AreCellsInSameWorld(int cell, int world_cell)
	{
		return Grid.IsValidCell(cell) && Grid.IsValidCell(world_cell) && Grid.WorldIdx[cell] == Grid.WorldIdx[world_cell];
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x000E8D73 File Offset: 0x000E6F73
	public static bool IsCellOpenToSpace(int cell)
	{
		return !Grid.IsSolidCell(cell) && !(Grid.Objects[cell, 2] != null) && global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell) == SubWorld.ZoneType.Space;
	}

	// Token: 0x06006882 RID: 26754 RVA: 0x002E6ED8 File Offset: 0x002E50D8
	public static int PosToCell(Vector2 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x06006883 RID: 26755 RVA: 0x002E6F04 File Offset: 0x002E5104
	public static int PosToCell(Vector3 pos)
	{
		float x = pos.x;
		int num = (int)(pos.y + 0.05f);
		int num2 = (int)x;
		return num * Grid.WidthInCells + num2;
	}

	// Token: 0x06006884 RID: 26756 RVA: 0x000E8DA8 File Offset: 0x000E6FA8
	public static void PosToXY(Vector3 pos, out int x, out int y)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out x, out y);
	}

	// Token: 0x06006885 RID: 26757 RVA: 0x000E8DB7 File Offset: 0x000E6FB7
	public static void PosToXY(Vector3 pos, out Vector2I xy)
	{
		Grid.CellToXY(Grid.PosToCell(pos), out xy.x, out xy.y);
	}

	// Token: 0x06006886 RID: 26758 RVA: 0x002E6F30 File Offset: 0x002E5130
	public static Vector2I PosToXY(Vector3 pos)
	{
		Vector2I result;
		Grid.CellToXY(Grid.PosToCell(pos), out result.x, out result.y);
		return result;
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x000E8DD0 File Offset: 0x000E6FD0
	public static int XYToCell(int x, int y)
	{
		return x + y * Grid.WidthInCells;
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x000E8DDB File Offset: 0x000E6FDB
	public static void CellToXY(int cell, out int x, out int y)
	{
		x = Grid.CellColumn(cell);
		y = Grid.CellRow(cell);
	}

	// Token: 0x06006889 RID: 26761 RVA: 0x000E8DED File Offset: 0x000E6FED
	public static Vector2I CellToXY(int cell)
	{
		return new Vector2I(Grid.CellColumn(cell), Grid.CellRow(cell));
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x002E6F58 File Offset: 0x002E5158
	public static Vector3 CellToPos(int cell, float x_offset, float y_offset, float z_offset)
	{
		int widthInCells = Grid.WidthInCells;
		float num = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float num2 = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(num + x_offset, num2 + y_offset, z_offset);
	}

	// Token: 0x0600688B RID: 26763 RVA: 0x002E6F8C File Offset: 0x002E518C
	public static Vector3 CellToPos(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector3(x, y, 0f);
	}

	// Token: 0x0600688C RID: 26764 RVA: 0x002E6FC0 File Offset: 0x002E51C0
	public static Vector3 CellToPos2D(int cell)
	{
		int widthInCells = Grid.WidthInCells;
		float x = Grid.CellSizeInMeters * (float)(cell % widthInCells);
		float y = Grid.CellSizeInMeters * (float)(cell / widthInCells);
		return new Vector2(x, y);
	}

	// Token: 0x0600688D RID: 26765 RVA: 0x000E8E00 File Offset: 0x000E7000
	public static int CellRow(int cell)
	{
		return cell / Grid.WidthInCells;
	}

	// Token: 0x0600688E RID: 26766 RVA: 0x000E8E09 File Offset: 0x000E7009
	public static int CellColumn(int cell)
	{
		return cell % Grid.WidthInCells;
	}

	// Token: 0x0600688F RID: 26767 RVA: 0x000E8E12 File Offset: 0x000E7012
	public static int ClampX(int x)
	{
		return Math.Min(Math.Max(x, 0), Grid.WidthInCells - 1);
	}

	// Token: 0x06006890 RID: 26768 RVA: 0x000E8E27 File Offset: 0x000E7027
	public static int ClampY(int y)
	{
		return Math.Min(Math.Max(y, 0), Grid.HeightInCells - 1);
	}

	// Token: 0x06006891 RID: 26769 RVA: 0x002E6FF4 File Offset: 0x002E51F4
	public static Vector2I Constrain(Vector2I val)
	{
		val.x = Mathf.Max(0, Mathf.Min(val.x, Grid.WidthInCells - 1));
		val.y = Mathf.Max(0, Mathf.Min(val.y, Grid.HeightInCells - 1));
		return val;
	}

	// Token: 0x06006892 RID: 26770 RVA: 0x002E7040 File Offset: 0x002E5240
	public static void Reveal(int cell, byte visibility = 255, bool forceReveal = false)
	{
		bool flag = Grid.Spawnable[cell] == 0 && visibility > 0;
		Grid.Spawnable[cell] = Math.Max(visibility, Grid.Visible[cell]);
		if (forceReveal || !Grid.PreventFogOfWarReveal[cell])
		{
			Grid.Visible[cell] = Math.Max(visibility, Grid.Visible[cell]);
		}
		if (flag && Grid.OnReveal != null)
		{
			Grid.OnReveal(cell);
		}
	}

	// Token: 0x06006893 RID: 26771 RVA: 0x000E8E3C File Offset: 0x000E703C
	public static ObjectLayer GetObjectLayerForConduitType(ConduitType conduit_type)
	{
		switch (conduit_type)
		{
		case ConduitType.Gas:
			return ObjectLayer.GasConduitConnection;
		case ConduitType.Liquid:
			return ObjectLayer.LiquidConduitConnection;
		case ConduitType.Solid:
			return ObjectLayer.SolidConduitConnection;
		default:
			throw new ArgumentException("Invalid value.", "conduit_type");
		}
	}

	// Token: 0x06006894 RID: 26772 RVA: 0x002E70AC File Offset: 0x002E52AC
	public static Vector3 CellToPos(int cell, CellAlignment alignment, Grid.SceneLayer layer)
	{
		switch (alignment)
		{
		case CellAlignment.Bottom:
			return Grid.CellToPosCBC(cell, layer);
		case CellAlignment.Top:
			return Grid.CellToPosCTC(cell, layer);
		case CellAlignment.Left:
			return Grid.CellToPosLCC(cell, layer);
		case CellAlignment.Right:
			return Grid.CellToPosRCC(cell, layer);
		case CellAlignment.RandomInternal:
		{
			Vector3 b = new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, 0f);
			return Grid.CellToPosCCC(cell, layer) + b;
		}
		}
		return Grid.CellToPosCCC(cell, layer);
	}

	// Token: 0x06006895 RID: 26773 RVA: 0x000E8E6C File Offset: 0x000E706C
	public static float GetLayerZ(Grid.SceneLayer layer)
	{
		return -Grid.HalfCellSizeInMeters - Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier;
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x000E8E83 File Offset: 0x000E7083
	public static Vector3 CellToPosCCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x06006897 RID: 26775 RVA: 0x000E8E9B File Offset: 0x000E709B
	public static Vector3 CellToPosCBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x06006898 RID: 26776 RVA: 0x000E8EB3 File Offset: 0x000E70B3
	public static Vector3 CellToPosCCF(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.HalfCellSizeInMeters, -Grid.CellSizeInMeters * (float)layer * Grid.LayerMultiplier);
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x000E8ED4 File Offset: 0x000E70D4
	public static Vector3 CellToPosLCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x000E8EEC File Offset: 0x000E70EC
	public static Vector3 CellToPosRCC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, Grid.HalfCellSizeInMeters, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600689B RID: 26779 RVA: 0x000E8F0A File Offset: 0x000E710A
	public static Vector3 CellToPosRBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.CellSizeInMeters - 0.01f, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600689C RID: 26780 RVA: 0x000E8F28 File Offset: 0x000E7128
	public static Vector3 CellToPosLBC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, 0.01f, 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600689D RID: 26781 RVA: 0x000E8F40 File Offset: 0x000E7140
	public static Vector3 CellToPosCTC(int cell, Grid.SceneLayer layer)
	{
		return Grid.CellToPos(cell, Grid.HalfCellSizeInMeters, Grid.CellSizeInMeters - 0.01f, Grid.GetLayerZ(layer));
	}

	// Token: 0x0600689E RID: 26782 RVA: 0x000E8F5E File Offset: 0x000E715E
	public static bool IsSolidCell(int cell)
	{
		return Grid.IsValidCell(cell) && Grid.Solid[cell];
	}

	// Token: 0x0600689F RID: 26783 RVA: 0x002E7130 File Offset: 0x002E5330
	public unsafe static bool IsSubstantialLiquid(int cell, float threshold = 0.35f)
	{
		if (Grid.IsValidCell(cell))
		{
			ushort num = Grid.elementIdx[cell];
			if ((int)num < ElementLoader.elements.Count)
			{
				Element element = ElementLoader.elements[(int)num];
				if (element.IsLiquid && Grid.mass[cell] >= element.defaultValues.mass * threshold)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060068A0 RID: 26784 RVA: 0x002E7190 File Offset: 0x002E5390
	public static bool IsVisiblyInLiquid(Vector2 pos)
	{
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && Grid.IsLiquid(num))
		{
			int cell = Grid.CellAbove(num);
			if (Grid.IsValidCell(cell) && Grid.IsLiquid(cell))
			{
				return true;
			}
			float num2 = Grid.Mass[num];
			float num3 = (float)((int)pos.y) - pos.y;
			if (num2 / 1000f <= num3)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060068A1 RID: 26785 RVA: 0x002E71F4 File Offset: 0x002E53F4
	public static bool IsNavigatableLiquid(int cell)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			return true;
		}
		if (Grid.IsLiquid(cell))
		{
			if (Grid.Element[num].IsLiquid)
			{
				return true;
			}
			if (Grid.Element[num].IsSolid)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060068A2 RID: 26786 RVA: 0x000E8F75 File Offset: 0x000E7175
	public static bool IsLiquid(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsLiquid;
	}

	// Token: 0x060068A3 RID: 26787 RVA: 0x000E8F96 File Offset: 0x000E7196
	public static bool IsGas(int cell)
	{
		return ElementLoader.elements[(int)Grid.ElementIdx[cell]].IsGas;
	}

	// Token: 0x060068A4 RID: 26788 RVA: 0x002E7254 File Offset: 0x002E5454
	public static void GetVisibleExtents(out int min_x, out int min_y, out int max_x, out int max_y)
	{
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		min_y = (int)vector2.y;
		max_y = (int)(vector.y + 0.5f);
		min_x = (int)vector2.x;
		max_x = (int)(vector.x + 0.5f);
	}

	// Token: 0x060068A5 RID: 26789 RVA: 0x000E8FB7 File Offset: 0x000E71B7
	public static void GetVisibleExtents(out Vector2I min, out Vector2I max)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
	}

	// Token: 0x060068A6 RID: 26790 RVA: 0x002E72F0 File Offset: 0x002E54F0
	public static void GetVisibleCellRangeInActiveWorld(out Vector2I min, out Vector2I max, int padding = 4, float rangeScale = 1.5f)
	{
		Grid.GetVisibleExtents(out min.x, out min.y, out max.x, out max.y);
		min.x -= padding;
		min.y -= padding;
		if (CameraController.Instance != null && DlcManager.IsExpansion1Active())
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			min.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, min.x));
			min.y = Math.Min(vector2I.y + vector2I2.y - 1, Math.Max(vector2I.y, min.y));
			max.x += padding;
			max.y += padding;
			max.x = Math.Min(vector2I.x + vector2I2.x - 1, Math.Max(vector2I.x, max.x));
			max.y = Math.Min(vector2I.y + vector2I2.y - 1 + 20, Math.Max(vector2I.y, max.y));
			return;
		}
		min.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, min.x));
		min.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, min.y));
		max.x += padding;
		max.y += padding;
		max.x = Math.Min((int)((float)Grid.WidthInCells * rangeScale) - 1, Math.Max(0, max.x));
		max.y = Math.Min((int)((float)Grid.HeightInCells * rangeScale) - 1, Math.Max(0, max.y));
	}

	// Token: 0x060068A7 RID: 26791 RVA: 0x002E74BC File Offset: 0x002E56BC
	public static Extents GetVisibleExtentsInActiveWorld(int padding = 4, float rangeScale = 1.5f)
	{
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		return new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
	}

	// Token: 0x060068A8 RID: 26792 RVA: 0x000E8FD6 File Offset: 0x000E71D6
	public static bool IsVisible(int cell)
	{
		return Grid.Visible[cell] > 0 || !PropertyTextures.IsFogOfWarEnabled;
	}

	// Token: 0x060068A9 RID: 26793 RVA: 0x000E8FEC File Offset: 0x000E71EC
	public static bool VisibleBlockingCB(int cell)
	{
		return !Grid.Transparent[cell] && Grid.IsSolidCell(cell);
	}

	// Token: 0x060068AA RID: 26794 RVA: 0x000E9003 File Offset: 0x000E7203
	public static bool VisibilityTest(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.TestLineOfSight(x, y, x2, y2, Grid.VisibleBlockingDelegate, blocking_tile_visible, false);
	}

	// Token: 0x060068AB RID: 26795 RVA: 0x002E7504 File Offset: 0x002E5704
	public static bool VisibilityTest(int cell, int target_cell, bool blocking_tile_visible = false)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		int x2 = 0;
		int y2 = 0;
		Grid.CellToXY(target_cell, out x2, out y2);
		return Grid.VisibilityTest(x, y, x2, y2, blocking_tile_visible);
	}

	// Token: 0x060068AC RID: 26796 RVA: 0x000E9016 File Offset: 0x000E7216
	public static bool PhysicalBlockingCB(int cell)
	{
		return Grid.Solid[cell];
	}

	// Token: 0x060068AD RID: 26797 RVA: 0x000E9023 File Offset: 0x000E7223
	public static bool IsPhysicallyAccessible(int x, int y, int x2, int y2, bool blocking_tile_visible = false)
	{
		return Grid.FastTestLineOfSightSolid(x, y, x2, y2);
	}

	// Token: 0x060068AE RID: 26798 RVA: 0x002E7538 File Offset: 0x002E5738
	public static void CollectCellsInLine(int startCell, int endCell, HashSet<int> outputCells)
	{
		int num = 2;
		int cellDistance = Grid.GetCellDistance(startCell, endCell);
		Vector2 a = (Grid.CellToPos(endCell) - Grid.CellToPos(startCell)).normalized;
		for (float num2 = 0f; num2 < (float)cellDistance; num2 = Mathf.Min(num2 + 1f / (float)num, (float)cellDistance))
		{
			int num3 = Grid.PosToCell(Grid.CellToPos(startCell) + a * num2);
			if (Grid.GetCellDistance(startCell, num3) <= cellDistance)
			{
				outputCells.Add(num3);
			}
		}
	}

	// Token: 0x060068AF RID: 26799 RVA: 0x002E75C4 File Offset: 0x002E57C4
	public static bool IsRangeExposedToSunlight(int cell, int scanRadius, CellOffset scanShape, out int cellsClear, int clearThreshold = 1)
	{
		cellsClear = 0;
		if (Grid.IsValidCell(cell) && (int)Grid.ExposedToSunlight[cell] >= clearThreshold)
		{
			cellsClear++;
		}
		bool flag = true;
		bool flag2 = true;
		int num = 1;
		while (num <= scanRadius && (flag || flag2))
		{
			int num2 = Grid.OffsetCell(cell, scanShape.x * num, scanShape.y * num);
			int num3 = Grid.OffsetCell(cell, -scanShape.x * num, scanShape.y * num);
			if (Grid.IsValidCell(num2) && (int)Grid.ExposedToSunlight[num2] >= clearThreshold)
			{
				cellsClear++;
			}
			if (Grid.IsValidCell(num3) && (int)Grid.ExposedToSunlight[num3] >= clearThreshold)
			{
				cellsClear++;
			}
			num++;
		}
		return cellsClear > 0;
	}

	// Token: 0x060068B0 RID: 26800 RVA: 0x002E7678 File Offset: 0x002E5878
	public static bool FastTestLineOfSightSolid(int x, int y, int x2, int y2)
	{
		int value = x2 - x;
		int num = y2 - y;
		int num2 = 0;
		int num4;
		int num3 = num4 = Math.Sign(value);
		int num5 = Math.Sign(num);
		int num6 = Math.Abs(value);
		int num7 = Math.Abs(num);
		if (num6 <= num7)
		{
			num6 = Math.Abs(num);
			num7 = Math.Abs(value);
			if (num < 0)
			{
				num2 = -1;
			}
			else if (num > 0)
			{
				num2 = 1;
			}
			num4 = 0;
		}
		int num8 = num6 >> 1;
		int num9 = num3 + num5 * Grid.WidthInCells;
		int num10 = num4 + num2 * Grid.WidthInCells;
		int num11 = Grid.XYToCell(x, y);
		for (int i = 1; i < num6; i++)
		{
			num8 += num7;
			if (num8 < num6)
			{
				num11 += num10;
			}
			else
			{
				num8 -= num6;
				num11 += num9;
			}
			if (Grid.Solid[num11])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060068B1 RID: 26801 RVA: 0x002E7748 File Offset: 0x002E5948
	public static bool TestLineOfSightFixedBlockingVisible(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x060068B2 RID: 26802 RVA: 0x002E7864 File Offset: 0x002E5A64
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, Func<int, bool> blocking_tile_visible_cb, bool allow_invalid_cells = false)
	{
		int num = x;
		int num2 = y;
		int num3 = x2 - x;
		int num4 = y2 - y;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		if (num3 < 0)
		{
			num5 = -1;
		}
		else if (num3 > 0)
		{
			num5 = 1;
		}
		if (num4 < 0)
		{
			num6 = -1;
		}
		else if (num4 > 0)
		{
			num6 = 1;
		}
		if (num3 < 0)
		{
			num7 = -1;
		}
		else if (num3 > 0)
		{
			num7 = 1;
		}
		int num9 = Math.Abs(num3);
		int num10 = Math.Abs(num4);
		if (num9 <= num10)
		{
			num9 = Math.Abs(num4);
			num10 = Math.Abs(num3);
			if (num4 < 0)
			{
				num8 = -1;
			}
			else if (num4 > 0)
			{
				num8 = 1;
			}
			num7 = 0;
		}
		int num11 = num9 >> 1;
		for (int i = 0; i <= num9; i++)
		{
			int num12 = Grid.XYToCell(x, y);
			if (!allow_invalid_cells && !Grid.IsValidCell(num12))
			{
				return false;
			}
			bool flag = blocking_cb(num12);
			if ((x != num || y != num2) && flag)
			{
				return blocking_tile_visible_cb(num12) && x == x2 && y == y2;
			}
			num11 += num10;
			if (num11 >= num9)
			{
				num11 -= num9;
				x += num5;
				y += num6;
			}
			else
			{
				x += num7;
				y += num8;
			}
		}
		return true;
	}

	// Token: 0x060068B3 RID: 26803 RVA: 0x000E902E File Offset: 0x000E722E
	public static bool TestLineOfSight(int x, int y, int x2, int y2, Func<int, bool> blocking_cb, bool blocking_tile_visible = false, bool allow_invalid_cells = false)
	{
		return Grid.TestLineOfSightFixedBlockingVisible(x, y, x2, y2, blocking_cb, blocking_tile_visible, allow_invalid_cells);
	}

	// Token: 0x060068B4 RID: 26804 RVA: 0x002E798C File Offset: 0x002E5B8C
	public static bool GetFreeGridSpace(Vector2I size, out Vector2I offset)
	{
		Vector2I gridOffset = BestFit.GetGridOffset(ClusterManager.Instance.WorldContainers, size, out offset);
		if (gridOffset.X <= Grid.WidthInCells && gridOffset.Y <= Grid.HeightInCells)
		{
			SimMessages.SimDataResizeGridAndInitializeVacuumCells(gridOffset, size.x, size.y, offset.x, offset.y);
			Game.Instance.roomProber.Refresh();
			return true;
		}
		return false;
	}

	// Token: 0x060068B5 RID: 26805 RVA: 0x002E79F8 File Offset: 0x002E5BF8
	public static void FreeGridSpace(Vector2I size, Vector2I offset)
	{
		SimMessages.SimDataFreeCells(size.x, size.y, offset.x, offset.y);
		for (int i = offset.y; i < size.y + offset.y + 1; i++)
		{
			for (int j = offset.x - 1; j < size.x + offset.x + 1; j++)
			{
				int num = Grid.XYToCell(j, i);
				if (Grid.IsValidCell(num))
				{
					Grid.Element[num] = ElementLoader.FindElementByHash(SimHashes.Vacuum);
				}
			}
		}
		Game.Instance.roomProber.Refresh();
	}

	// Token: 0x060068B6 RID: 26806 RVA: 0x000E903F File Offset: 0x000E723F
	[Conditional("UNITY_EDITOR")]
	public static void DrawBoxOnCell(int cell, Color color, float offset = 0f)
	{
		Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, 0f);
	}

	// Token: 0x04004F08 RID: 20232
	public static readonly CellOffset[] DefaultOffset = new CellOffset[1];

	// Token: 0x04004F09 RID: 20233
	public static float WidthInMeters;

	// Token: 0x04004F0A RID: 20234
	public static float HeightInMeters;

	// Token: 0x04004F0B RID: 20235
	public static int WidthInCells;

	// Token: 0x04004F0C RID: 20236
	public static int HeightInCells;

	// Token: 0x04004F0D RID: 20237
	public static float CellSizeInMeters;

	// Token: 0x04004F0E RID: 20238
	public static float InverseCellSizeInMeters;

	// Token: 0x04004F0F RID: 20239
	public static float HalfCellSizeInMeters;

	// Token: 0x04004F10 RID: 20240
	public static int CellCount;

	// Token: 0x04004F11 RID: 20241
	public static int InvalidCell = -1;

	// Token: 0x04004F12 RID: 20242
	public static int TopBorderHeight = 2;

	// Token: 0x04004F13 RID: 20243
	public static Dictionary<int, GameObject>[] ObjectLayers;

	// Token: 0x04004F14 RID: 20244
	public static Action<int> OnReveal;

	// Token: 0x04004F15 RID: 20245
	public static Grid.BuildFlags[] BuildMasks;

	// Token: 0x04004F16 RID: 20246
	public static Grid.BuildFlagsFoundationIndexer Foundation;

	// Token: 0x04004F17 RID: 20247
	public static Grid.BuildFlagsSolidIndexer Solid;

	// Token: 0x04004F18 RID: 20248
	public static Grid.BuildFlagsDupeImpassableIndexer DupeImpassable;

	// Token: 0x04004F19 RID: 20249
	public static Grid.BuildFlagsFakeFloorIndexer FakeFloor;

	// Token: 0x04004F1A RID: 20250
	public static Grid.BuildFlagsDupePassableIndexer DupePassable;

	// Token: 0x04004F1B RID: 20251
	public static Grid.BuildFlagsImpassableIndexer CritterImpassable;

	// Token: 0x04004F1C RID: 20252
	public static Grid.BuildFlagsDoorIndexer HasDoor;

	// Token: 0x04004F1D RID: 20253
	public static Grid.VisFlags[] VisMasks;

	// Token: 0x04004F1E RID: 20254
	public static Grid.VisFlagsRevealedIndexer Revealed;

	// Token: 0x04004F1F RID: 20255
	public static Grid.VisFlagsPreventFogOfWarRevealIndexer PreventFogOfWarReveal;

	// Token: 0x04004F20 RID: 20256
	public static Grid.VisFlagsRenderedByWorldIndexer RenderedByWorld;

	// Token: 0x04004F21 RID: 20257
	public static Grid.VisFlagsAllowPathfindingIndexer AllowPathfinding;

	// Token: 0x04004F22 RID: 20258
	public static Grid.NavValidatorFlags[] NavValidatorMasks;

	// Token: 0x04004F23 RID: 20259
	public static Grid.NavValidatorFlagsLadderIndexer HasLadder;

	// Token: 0x04004F24 RID: 20260
	public static Grid.NavValidatorFlagsPoleIndexer HasPole;

	// Token: 0x04004F25 RID: 20261
	public static Grid.NavValidatorFlagsTubeIndexer HasTube;

	// Token: 0x04004F26 RID: 20262
	public static Grid.NavValidatorFlagsNavTeleporterIndexer HasNavTeleporter;

	// Token: 0x04004F27 RID: 20263
	public static Grid.NavValidatorFlagsUnderConstructionIndexer IsTileUnderConstruction;

	// Token: 0x04004F28 RID: 20264
	public static Grid.NavFlags[] NavMasks;

	// Token: 0x04004F29 RID: 20265
	private static Grid.NavFlagsAccessDoorIndexer HasAccessDoor;

	// Token: 0x04004F2A RID: 20266
	public static Grid.NavFlagsTubeEntranceIndexer HasTubeEntrance;

	// Token: 0x04004F2B RID: 20267
	public static Grid.NavFlagsPreventIdleTraversalIndexer PreventIdleTraversal;

	// Token: 0x04004F2C RID: 20268
	public static Grid.NavFlagsReservedIndexer Reserved;

	// Token: 0x04004F2D RID: 20269
	public static Grid.NavFlagsSuitMarkerIndexer HasSuitMarker;

	// Token: 0x04004F2E RID: 20270
	private static Dictionary<int, Grid.Restriction> restrictions = new Dictionary<int, Grid.Restriction>();

	// Token: 0x04004F2F RID: 20271
	private static Dictionary<int, Grid.TubeEntrance> tubeEntrances = new Dictionary<int, Grid.TubeEntrance>();

	// Token: 0x04004F30 RID: 20272
	private static Dictionary<int, Grid.SuitMarker> suitMarkers = new Dictionary<int, Grid.SuitMarker>();

	// Token: 0x04004F31 RID: 20273
	public unsafe static ushort* elementIdx;

	// Token: 0x04004F32 RID: 20274
	public unsafe static float* temperature;

	// Token: 0x04004F33 RID: 20275
	public unsafe static float* radiation;

	// Token: 0x04004F34 RID: 20276
	public unsafe static float* mass;

	// Token: 0x04004F35 RID: 20277
	public unsafe static byte* properties;

	// Token: 0x04004F36 RID: 20278
	public unsafe static byte* strengthInfo;

	// Token: 0x04004F37 RID: 20279
	public unsafe static byte* insulation;

	// Token: 0x04004F38 RID: 20280
	public unsafe static byte* diseaseIdx;

	// Token: 0x04004F39 RID: 20281
	public unsafe static int* diseaseCount;

	// Token: 0x04004F3A RID: 20282
	public unsafe static byte* exposedToSunlight;

	// Token: 0x04004F3B RID: 20283
	public unsafe static float* AccumulatedFlowValues = null;

	// Token: 0x04004F3C RID: 20284
	public static byte[] Visible;

	// Token: 0x04004F3D RID: 20285
	public static byte[] Spawnable;

	// Token: 0x04004F3E RID: 20286
	public static float[] Damage;

	// Token: 0x04004F3F RID: 20287
	public static float[] Decor;

	// Token: 0x04004F40 RID: 20288
	public static bool[] GravitasFacility;

	// Token: 0x04004F41 RID: 20289
	public static byte[] WorldIdx;

	// Token: 0x04004F42 RID: 20290
	public static float[] Loudness;

	// Token: 0x04004F43 RID: 20291
	public static Element[] Element;

	// Token: 0x04004F44 RID: 20292
	public static int[] LightCount;

	// Token: 0x04004F45 RID: 20293
	public static Grid.PressureIndexer Pressure;

	// Token: 0x04004F46 RID: 20294
	public static Grid.LiquidImpermeableIndexer LiquidImpermeable;

	// Token: 0x04004F47 RID: 20295
	public static Grid.TransparentIndexer Transparent;

	// Token: 0x04004F48 RID: 20296
	public static Grid.ElementIdxIndexer ElementIdx;

	// Token: 0x04004F49 RID: 20297
	public static Grid.TemperatureIndexer Temperature;

	// Token: 0x04004F4A RID: 20298
	public static Grid.RadiationIndexer Radiation;

	// Token: 0x04004F4B RID: 20299
	public static Grid.MassIndexer Mass;

	// Token: 0x04004F4C RID: 20300
	public static Grid.PropertiesIndexer Properties;

	// Token: 0x04004F4D RID: 20301
	public static Grid.ExposedToSunlightIndexer ExposedToSunlight;

	// Token: 0x04004F4E RID: 20302
	public static Grid.StrengthInfoIndexer StrengthInfo;

	// Token: 0x04004F4F RID: 20303
	public static Grid.Insulationndexer Insulation;

	// Token: 0x04004F50 RID: 20304
	public static Grid.DiseaseIdxIndexer DiseaseIdx;

	// Token: 0x04004F51 RID: 20305
	public static Grid.DiseaseCountIndexer DiseaseCount;

	// Token: 0x04004F52 RID: 20306
	public static Grid.LightIntensityIndexer LightIntensity;

	// Token: 0x04004F53 RID: 20307
	public static Grid.AccumulatedFlowIndexer AccumulatedFlow;

	// Token: 0x04004F54 RID: 20308
	public static Grid.ObjectLayerIndexer Objects;

	// Token: 0x04004F55 RID: 20309
	public static float LayerMultiplier = 1f;

	// Token: 0x04004F56 RID: 20310
	private static readonly Func<int, bool> VisibleBlockingDelegate = (int cell) => Grid.VisibleBlockingCB(cell);

	// Token: 0x04004F57 RID: 20311
	private static readonly Func<int, bool> PhysicalBlockingDelegate = (int cell) => Grid.PhysicalBlockingCB(cell);

	// Token: 0x020013DD RID: 5085
	[Flags]
	public enum BuildFlags : byte
	{
		// Token: 0x04004F59 RID: 20313
		Solid = 1,
		// Token: 0x04004F5A RID: 20314
		Foundation = 2,
		// Token: 0x04004F5B RID: 20315
		Door = 4,
		// Token: 0x04004F5C RID: 20316
		DupePassable = 8,
		// Token: 0x04004F5D RID: 20317
		DupeImpassable = 16,
		// Token: 0x04004F5E RID: 20318
		CritterImpassable = 32,
		// Token: 0x04004F5F RID: 20319
		FakeFloor = 192,
		// Token: 0x04004F60 RID: 20320
		Any = 255
	}

	// Token: 0x020013DE RID: 5086
	public struct BuildFlagsFoundationIndexer
	{
		// Token: 0x1700067C RID: 1660
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Foundation) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Foundation, value);
			}
		}
	}

	// Token: 0x020013DF RID: 5087
	public struct BuildFlagsSolidIndexer
	{
		// Token: 0x1700067D RID: 1661
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Solid) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}
	}

	// Token: 0x020013E0 RID: 5088
	public struct BuildFlagsDupeImpassableIndexer
	{
		// Token: 0x1700067E RID: 1662
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupeImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupeImpassable, value);
			}
		}
	}

	// Token: 0x020013E1 RID: 5089
	public struct BuildFlagsFakeFloorIndexer
	{
		// Token: 0x1700067F RID: 1663
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.FakeFloor) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x002E7B14 File Offset: 0x002E5D14
		public void Add(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) + 1);
			num = Math.Min(num, 3);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x002E7B54 File Offset: 0x002E5D54
		public void Remove(int i)
		{
			Grid.BuildFlags buildFlags = Grid.BuildMasks[i];
			int num = (int)(((buildFlags & Grid.BuildFlags.FakeFloor) >> 6) - Grid.BuildFlags.Solid);
			num = Math.Max(num, 0);
			Grid.BuildMasks[i] = ((buildFlags & ~Grid.BuildFlags.FakeFloor) | ((Grid.BuildFlags)(num << 6) & Grid.BuildFlags.FakeFloor));
		}
	}

	// Token: 0x020013E2 RID: 5090
	public struct BuildFlagsDupePassableIndexer
	{
		// Token: 0x17000680 RID: 1664
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.DupePassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.DupePassable, value);
			}
		}
	}

	// Token: 0x020013E3 RID: 5091
	public struct BuildFlagsImpassableIndexer
	{
		// Token: 0x17000681 RID: 1665
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.CritterImpassable) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.CritterImpassable, value);
			}
		}
	}

	// Token: 0x020013E4 RID: 5092
	public struct BuildFlagsDoorIndexer
	{
		// Token: 0x17000682 RID: 1666
		public bool this[int i]
		{
			get
			{
				return (Grid.BuildMasks[i] & Grid.BuildFlags.Door) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor);
			}
			set
			{
				Grid.UpdateBuildMask(i, Grid.BuildFlags.Door, value);
			}
		}
	}

	// Token: 0x020013E5 RID: 5093
	[Flags]
	public enum VisFlags : byte
	{
		// Token: 0x04004F62 RID: 20322
		Revealed = 1,
		// Token: 0x04004F63 RID: 20323
		PreventFogOfWarReveal = 2,
		// Token: 0x04004F64 RID: 20324
		RenderedByWorld = 4,
		// Token: 0x04004F65 RID: 20325
		AllowPathfinding = 8
	}

	// Token: 0x020013E6 RID: 5094
	public struct VisFlagsRevealedIndexer
	{
		// Token: 0x17000683 RID: 1667
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.Revealed) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.Revealed, value);
			}
		}
	}

	// Token: 0x020013E7 RID: 5095
	public struct VisFlagsPreventFogOfWarRevealIndexer
	{
		// Token: 0x17000684 RID: 1668
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.PreventFogOfWarReveal) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.PreventFogOfWarReveal, value);
			}
		}
	}

	// Token: 0x020013E8 RID: 5096
	public struct VisFlagsRenderedByWorldIndexer
	{
		// Token: 0x17000685 RID: 1669
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.RenderedByWorld) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.RenderedByWorld, value);
			}
		}
	}

	// Token: 0x020013E9 RID: 5097
	public struct VisFlagsAllowPathfindingIndexer
	{
		// Token: 0x17000686 RID: 1670
		public bool this[int i]
		{
			get
			{
				return (Grid.VisMasks[i] & Grid.VisFlags.AllowPathfinding) > (Grid.VisFlags)0;
			}
			set
			{
				Grid.UpdateVisMask(i, Grid.VisFlags.AllowPathfinding, value);
			}
		}
	}

	// Token: 0x020013EA RID: 5098
	[Flags]
	public enum NavValidatorFlags : byte
	{
		// Token: 0x04004F67 RID: 20327
		Ladder = 1,
		// Token: 0x04004F68 RID: 20328
		Pole = 2,
		// Token: 0x04004F69 RID: 20329
		Tube = 4,
		// Token: 0x04004F6A RID: 20330
		NavTeleporter = 8,
		// Token: 0x04004F6B RID: 20331
		UnderConstruction = 16
	}

	// Token: 0x020013EB RID: 5099
	public struct NavValidatorFlagsLadderIndexer
	{
		// Token: 0x17000687 RID: 1671
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Ladder) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Ladder, value);
			}
		}
	}

	// Token: 0x020013EC RID: 5100
	public struct NavValidatorFlagsPoleIndexer
	{
		// Token: 0x17000688 RID: 1672
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Pole) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Pole, value);
			}
		}
	}

	// Token: 0x020013ED RID: 5101
	public struct NavValidatorFlagsTubeIndexer
	{
		// Token: 0x17000689 RID: 1673
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.Tube) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.Tube, value);
			}
		}
	}

	// Token: 0x020013EE RID: 5102
	public struct NavValidatorFlagsNavTeleporterIndexer
	{
		// Token: 0x1700068A RID: 1674
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.NavTeleporter) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.NavTeleporter, value);
			}
		}
	}

	// Token: 0x020013EF RID: 5103
	public struct NavValidatorFlagsUnderConstructionIndexer
	{
		// Token: 0x1700068B RID: 1675
		public bool this[int i]
		{
			get
			{
				return (Grid.NavValidatorMasks[i] & Grid.NavValidatorFlags.UnderConstruction) > (Grid.NavValidatorFlags)0;
			}
			set
			{
				Grid.UpdateNavValidatorMask(i, Grid.NavValidatorFlags.UnderConstruction, value);
			}
		}
	}

	// Token: 0x020013F0 RID: 5104
	[Flags]
	public enum NavFlags : byte
	{
		// Token: 0x04004F6D RID: 20333
		AccessDoor = 1,
		// Token: 0x04004F6E RID: 20334
		TubeEntrance = 2,
		// Token: 0x04004F6F RID: 20335
		PreventIdleTraversal = 4,
		// Token: 0x04004F70 RID: 20336
		Reserved = 8,
		// Token: 0x04004F71 RID: 20337
		SuitMarker = 16
	}

	// Token: 0x020013F1 RID: 5105
	public struct NavFlagsAccessDoorIndexer
	{
		// Token: 0x1700068C RID: 1676
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.AccessDoor) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.AccessDoor, value);
			}
		}
	}

	// Token: 0x020013F2 RID: 5106
	public struct NavFlagsTubeEntranceIndexer
	{
		// Token: 0x1700068D RID: 1677
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.TubeEntrance) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.TubeEntrance, value);
			}
		}
	}

	// Token: 0x020013F3 RID: 5107
	public struct NavFlagsPreventIdleTraversalIndexer
	{
		// Token: 0x1700068E RID: 1678
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.PreventIdleTraversal) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.PreventIdleTraversal, value);
			}
		}
	}

	// Token: 0x020013F4 RID: 5108
	public struct NavFlagsReservedIndexer
	{
		// Token: 0x1700068F RID: 1679
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.Reserved) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.Reserved, value);
			}
		}
	}

	// Token: 0x020013F5 RID: 5109
	public struct NavFlagsSuitMarkerIndexer
	{
		// Token: 0x17000690 RID: 1680
		public bool this[int i]
		{
			get
			{
				return (Grid.NavMasks[i] & Grid.NavFlags.SuitMarker) > (Grid.NavFlags)0;
			}
			set
			{
				Grid.UpdateNavMask(i, Grid.NavFlags.SuitMarker, value);
			}
		}
	}

	// Token: 0x020013F6 RID: 5110
	public struct Restriction
	{
		// Token: 0x04004F72 RID: 20338
		public const int DefaultID = -1;

		// Token: 0x04004F73 RID: 20339
		public Dictionary<int, Grid.Restriction.Directions> DirectionMasksForMinionInstanceID;

		// Token: 0x04004F74 RID: 20340
		public Grid.Restriction.Orientation orientation;

		// Token: 0x020013F7 RID: 5111
		[Flags]
		public enum Directions : byte
		{
			// Token: 0x04004F76 RID: 20342
			Left = 1,
			// Token: 0x04004F77 RID: 20343
			Right = 2,
			// Token: 0x04004F78 RID: 20344
			Teleport = 4
		}

		// Token: 0x020013F8 RID: 5112
		public enum Orientation : byte
		{
			// Token: 0x04004F7A RID: 20346
			Vertical,
			// Token: 0x04004F7B RID: 20347
			Horizontal,
			// Token: 0x04004F7C RID: 20348
			SingleCell
		}
	}

	// Token: 0x020013F9 RID: 5113
	private struct TubeEntrance
	{
		// Token: 0x04004F7D RID: 20349
		public bool operational;

		// Token: 0x04004F7E RID: 20350
		public int reservationCapacity;

		// Token: 0x04004F7F RID: 20351
		public HashSet<int> reservedInstanceIDs;
	}

	// Token: 0x020013FA RID: 5114
	public struct SuitMarker
	{
		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x000E9251 File Offset: 0x000E7451
		public int emptyLockerCount
		{
			get
			{
				return this.lockerCount - this.suitCount;
			}
		}

		// Token: 0x04004F80 RID: 20352
		public int suitCount;

		// Token: 0x04004F81 RID: 20353
		public int lockerCount;

		// Token: 0x04004F82 RID: 20354
		public Grid.SuitMarker.Flags flags;

		// Token: 0x04004F83 RID: 20355
		public PathFinder.PotentialPath.Flags pathFlags;

		// Token: 0x04004F84 RID: 20356
		public HashSet<int> minionIDsWithSuitReservations;

		// Token: 0x04004F85 RID: 20357
		public HashSet<int> minionIDsWithEmptyLockerReservations;

		// Token: 0x020013FB RID: 5115
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x04004F87 RID: 20359
			OnlyTraverseIfUnequipAvailable = 1,
			// Token: 0x04004F88 RID: 20360
			Operational = 2,
			// Token: 0x04004F89 RID: 20361
			Rotated = 4
		}
	}

	// Token: 0x020013FC RID: 5116
	public struct ObjectLayerIndexer
	{
		// Token: 0x17000692 RID: 1682
		public GameObject this[int cell, int layer]
		{
			get
			{
				GameObject result = null;
				Grid.ObjectLayers[layer].TryGetValue(cell, out result);
				return result;
			}
			set
			{
				if (value == null)
				{
					Grid.ObjectLayers[layer].Remove(cell);
				}
				else
				{
					Grid.ObjectLayers[layer][cell] = value;
				}
				GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.objectLayers[layer], value);
			}
		}
	}

	// Token: 0x020013FD RID: 5117
	public struct PressureIndexer
	{
		// Token: 0x17000693 RID: 1683
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i] * 101.3f;
			}
		}
	}

	// Token: 0x020013FE RID: 5118
	public struct LiquidImpermeableIndexer
	{
		// Token: 0x17000694 RID: 1684
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 2) > 0;
			}
		}
	}

	// Token: 0x020013FF RID: 5119
	public struct TransparentIndexer
	{
		// Token: 0x17000695 RID: 1685
		public unsafe bool this[int i]
		{
			get
			{
				return (Grid.properties[i] & 16) > 0;
			}
		}
	}

	// Token: 0x02001400 RID: 5120
	public struct ElementIdxIndexer
	{
		// Token: 0x17000696 RID: 1686
		public unsafe ushort this[int i]
		{
			get
			{
				return Grid.elementIdx[i];
			}
		}
	}

	// Token: 0x02001401 RID: 5121
	public struct TemperatureIndexer
	{
		// Token: 0x17000697 RID: 1687
		public unsafe float this[int i]
		{
			get
			{
				return Grid.temperature[i];
			}
		}
	}

	// Token: 0x02001402 RID: 5122
	public struct RadiationIndexer
	{
		// Token: 0x17000698 RID: 1688
		public unsafe float this[int i]
		{
			get
			{
				return Grid.radiation[i];
			}
		}
	}

	// Token: 0x02001403 RID: 5123
	public struct MassIndexer
	{
		// Token: 0x17000699 RID: 1689
		public unsafe float this[int i]
		{
			get
			{
				return Grid.mass[i];
			}
		}
	}

	// Token: 0x02001404 RID: 5124
	public struct PropertiesIndexer
	{
		// Token: 0x1700069A RID: 1690
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.properties[i];
			}
		}
	}

	// Token: 0x02001405 RID: 5125
	public struct ExposedToSunlightIndexer
	{
		// Token: 0x1700069B RID: 1691
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.exposedToSunlight[i];
			}
		}
	}

	// Token: 0x02001406 RID: 5126
	public struct StrengthInfoIndexer
	{
		// Token: 0x1700069C RID: 1692
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.strengthInfo[i];
			}
		}
	}

	// Token: 0x02001407 RID: 5127
	public struct Insulationndexer
	{
		// Token: 0x1700069D RID: 1693
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.insulation[i];
			}
		}
	}

	// Token: 0x02001408 RID: 5128
	public struct DiseaseIdxIndexer
	{
		// Token: 0x1700069E RID: 1694
		public unsafe byte this[int i]
		{
			get
			{
				return Grid.diseaseIdx[i];
			}
		}
	}

	// Token: 0x02001409 RID: 5129
	public struct DiseaseCountIndexer
	{
		// Token: 0x1700069F RID: 1695
		public unsafe int this[int i]
		{
			get
			{
				return Grid.diseaseCount[i];
			}
		}
	}

	// Token: 0x0200140A RID: 5130
	public struct AccumulatedFlowIndexer
	{
		// Token: 0x170006A0 RID: 1696
		public unsafe float this[int i]
		{
			get
			{
				return Grid.AccumulatedFlowValues[i];
			}
		}
	}

	// Token: 0x0200140B RID: 5131
	public struct LightIntensityIndexer
	{
		// Token: 0x170006A1 RID: 1697
		public unsafe int this[int i]
		{
			get
			{
				float num = Game.Instance.currentFallbackSunlightIntensity;
				WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[i]);
				if (world != null)
				{
					num = world.currentSunlightIntensity;
				}
				int num2 = (int)((float)Grid.exposedToSunlight[i] / 255f * num);
				int num3 = Grid.LightCount[i];
				return num2 + num3;
			}
		}
	}

	// Token: 0x0200140C RID: 5132
	public enum SceneLayer
	{
		// Token: 0x04004F8B RID: 20363
		WorldSelection = -3,
		// Token: 0x04004F8C RID: 20364
		NoLayer,
		// Token: 0x04004F8D RID: 20365
		Background,
		// Token: 0x04004F8E RID: 20366
		Backwall = 1,
		// Token: 0x04004F8F RID: 20367
		Gas,
		// Token: 0x04004F90 RID: 20368
		GasConduits,
		// Token: 0x04004F91 RID: 20369
		GasConduitBridges,
		// Token: 0x04004F92 RID: 20370
		LiquidConduits,
		// Token: 0x04004F93 RID: 20371
		LiquidConduitBridges,
		// Token: 0x04004F94 RID: 20372
		SolidConduits,
		// Token: 0x04004F95 RID: 20373
		SolidConduitContents,
		// Token: 0x04004F96 RID: 20374
		SolidConduitBridges,
		// Token: 0x04004F97 RID: 20375
		Wires,
		// Token: 0x04004F98 RID: 20376
		WireBridges,
		// Token: 0x04004F99 RID: 20377
		WireBridgesFront,
		// Token: 0x04004F9A RID: 20378
		LogicWires,
		// Token: 0x04004F9B RID: 20379
		LogicGates,
		// Token: 0x04004F9C RID: 20380
		LogicGatesFront,
		// Token: 0x04004F9D RID: 20381
		InteriorWall,
		// Token: 0x04004F9E RID: 20382
		GasFront,
		// Token: 0x04004F9F RID: 20383
		BuildingBack,
		// Token: 0x04004FA0 RID: 20384
		Building,
		// Token: 0x04004FA1 RID: 20385
		BuildingUse,
		// Token: 0x04004FA2 RID: 20386
		BuildingFront,
		// Token: 0x04004FA3 RID: 20387
		TransferArm,
		// Token: 0x04004FA4 RID: 20388
		Ore,
		// Token: 0x04004FA5 RID: 20389
		Creatures,
		// Token: 0x04004FA6 RID: 20390
		Move,
		// Token: 0x04004FA7 RID: 20391
		Front,
		// Token: 0x04004FA8 RID: 20392
		GlassTile,
		// Token: 0x04004FA9 RID: 20393
		Liquid,
		// Token: 0x04004FAA RID: 20394
		Ground,
		// Token: 0x04004FAB RID: 20395
		TileMain,
		// Token: 0x04004FAC RID: 20396
		TileFront,
		// Token: 0x04004FAD RID: 20397
		FXFront,
		// Token: 0x04004FAE RID: 20398
		FXFront2,
		// Token: 0x04004FAF RID: 20399
		SceneMAX
	}
}
