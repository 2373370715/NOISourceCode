using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020019D3 RID: 6611
public class ConditionFlightPathIsClear : ProcessCondition
{
	// Token: 0x060089C6 RID: 35270 RVA: 0x00367D3C File Offset: 0x00365F3C
	public ConditionFlightPathIsClear(GameObject module, int bufferWidth)
	{
		this.module = module.GetComponent<RocketModule>();
		if (this.module is RocketModuleCluster)
		{
			this.moduleInterface = (this.module as RocketModuleCluster).CraftInterface;
		}
		this.bufferWidth = bufferWidth;
	}

	// Token: 0x060089C7 RID: 35271 RVA: 0x000FE8DC File Offset: 0x000FCADC
	public override ProcessCondition.Status EvaluateCondition()
	{
		this.Update();
		if (!this.hasClearSky)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060089C8 RID: 35272 RVA: 0x000FE8EF File Offset: 0x000FCAEF
	public override StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Failure)
		{
			return Db.Get().BuildingStatusItems.PathNotClear;
		}
		return null;
	}

	// Token: 0x060089C9 RID: 35273 RVA: 0x00367D8C File Offset: 0x00365F8C
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.STATUS.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x060089CA RID: 35274 RVA: 0x00367DE0 File Offset: 0x00365FE0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.FLIGHT_PATH_CLEAR.TOOLTIP.FAILURE;
		}
		if (status != ProcessCondition.Status.Ready)
		{
			return Db.Get().BuildingStatusItems.PathNotClear.notificationTooltipText;
		}
		global::Debug.LogError("ConditionFlightPathIsClear: You'll need to add new strings/status items if you want to show the ready state");
		return "";
	}

	// Token: 0x060089CB RID: 35275 RVA: 0x000FE905 File Offset: 0x000FCB05
	public override bool ShowInUI()
	{
		return DlcManager.FeatureClusterSpaceEnabled();
	}

	// Token: 0x060089CC RID: 35276 RVA: 0x00367E34 File Offset: 0x00366034
	public void Update()
	{
		List<Building> list = new List<Building>();
		if (this.moduleInterface != null)
		{
			using (List<Ref<RocketModuleCluster>>.Enumerator enumerator = new List<Ref<RocketModuleCluster>>(this.moduleInterface.ClusterModules).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ref<RocketModuleCluster> @ref = enumerator.Current;
					list.Add(@ref.Get().GetComponent<Building>());
				}
				goto IL_A6;
			}
		}
		foreach (RocketModule rocketModule in this.module.FindLaunchConditionManager().rocketModules)
		{
			list.Add(rocketModule.GetComponent<Building>());
		}
		IL_A6:
		list.Sort(delegate(Building a, Building b)
		{
			int y = Grid.PosToXY(a.transform.GetPosition()).y;
			int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
			return y.CompareTo(y2);
		});
		if (this.moduleInterface != null && this.moduleInterface.CurrentPad == null)
		{
			this.hasClearSky = false;
			return;
		}
		this.hasClearSky = true;
		int num = -1;
		int num2 = 0;
		while (this.hasClearSky && num2 < list.Count)
		{
			Building building = list[num2];
			this.hasClearSky = ConditionFlightPathIsClear.HasModuleAccessToSpace(building, out num);
			num2++;
		}
	}

	// Token: 0x060089CD RID: 35277 RVA: 0x00367F90 File Offset: 0x00366190
	public static bool HasModuleAccessToSpace(Building module, out int obstructionCell)
	{
		WorldContainer myWorld = module.GetMyWorld();
		obstructionCell = -1;
		if (myWorld.id == 255)
		{
			return false;
		}
		int num = (int)myWorld.maximumBounds.y;
		Extents extents = module.GetExtents();
		int cell = Grid.XYToCell(extents.x, extents.y);
		bool result = true;
		for (int i = 0; i < extents.width; i++)
		{
			int num2 = Grid.OffsetCell(cell, new CellOffset(i, 0));
			while (!Grid.IsSolidCell(num2) && Grid.CellToXY(num2).y < num)
			{
				num2 = Grid.CellAbove(num2);
			}
			if (Grid.IsSolidCell(num2) || Grid.CellToXY(num2).y != num)
			{
				obstructionCell = num2;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x060089CE RID: 35278 RVA: 0x0036804C File Offset: 0x0036624C
	public static int PadTopEdgeDistanceToOutOfScreenEdge(GameObject launchpad)
	{
		WorldContainer myWorld = launchpad.GetMyWorld();
		Vector2 maximumBounds = myWorld.maximumBounds;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return (int)CameraController.GetHighestVisibleCell_Height((byte)myWorld.ParentWorldId) - y + 10;
	}

	// Token: 0x060089CF RID: 35279 RVA: 0x00368090 File Offset: 0x00366290
	public static int PadTopEdgeDistanceToCeilingEdge(GameObject launchpad)
	{
		Vector2 maximumBounds = launchpad.GetMyWorld().maximumBounds;
		int num = (int)launchpad.GetMyWorld().maximumBounds.y;
		int y = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition).y;
		return num - Grid.TopBorderHeight - y + 1;
	}

	// Token: 0x060089D0 RID: 35280 RVA: 0x003680DC File Offset: 0x003662DC
	public static bool CheckFlightPathClear(CraftModuleInterface craft, GameObject launchpad, out int obstruction)
	{
		Vector2I vector2I = Grid.CellToXY(launchpad.GetComponent<LaunchPad>().RocketBottomPosition);
		int num = ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(launchpad);
		foreach (Ref<RocketModuleCluster> @ref in craft.ClusterModules)
		{
			Building component = @ref.Get().GetComponent<Building>();
			int widthInCells = component.Def.WidthInCells;
			int moduleRelativeVerticalPosition = craft.GetModuleRelativeVerticalPosition(@ref.Get().gameObject);
			if (moduleRelativeVerticalPosition + component.Def.HeightInCells > num)
			{
				int num2 = Grid.XYToCell(vector2I.x, moduleRelativeVerticalPosition + vector2I.y);
				obstruction = num2;
				return false;
			}
			for (int i = moduleRelativeVerticalPosition; i < num; i++)
			{
				for (int j = 0; j < widthInCells; j++)
				{
					int num3 = Grid.XYToCell(j + (vector2I.x - widthInCells / 2), i + vector2I.y);
					bool flag = Grid.Solid[num3];
					if (!Grid.IsValidCell(num3) || Grid.WorldIdx[num3] != Grid.WorldIdx[launchpad.GetComponent<LaunchPad>().RocketBottomPosition] || flag)
					{
						obstruction = num3;
						return false;
					}
				}
			}
		}
		obstruction = -1;
		return true;
	}

	// Token: 0x060089D1 RID: 35281 RVA: 0x0036823C File Offset: 0x0036643C
	private static bool CanReachSpace(int startCell, out int obstruction, out int highestCellInSky)
	{
		WorldContainer worldContainer = (startCell >= 0) ? ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[startCell]) : null;
		int num = (worldContainer == null) ? Grid.HeightInCells : ((int)worldContainer.maximumBounds.y);
		highestCellInSky = num;
		obstruction = -1;
		int num2 = startCell;
		while (Grid.CellRow(num2) < num)
		{
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2])
			{
				obstruction = num2;
				return false;
			}
			num2 = Grid.CellAbove(num2);
		}
		return true;
	}

	// Token: 0x060089D2 RID: 35282 RVA: 0x003682B4 File Offset: 0x003664B4
	public string GetObstruction()
	{
		if (this.obstructedTile == -1)
		{
			return null;
		}
		if (Grid.Objects[this.obstructedTile, 1] != null)
		{
			return Grid.Objects[this.obstructedTile, 1].GetComponent<Building>().Def.Name;
		}
		return string.Format(BUILDING.STATUSITEMS.PATH_NOT_CLEAR.TILE_FORMAT, Grid.Element[this.obstructedTile].tag.ProperName());
	}

	// Token: 0x04006833 RID: 26675
	private CraftModuleInterface moduleInterface;

	// Token: 0x04006834 RID: 26676
	private RocketModule module;

	// Token: 0x04006835 RID: 26677
	private int bufferWidth;

	// Token: 0x04006836 RID: 26678
	private bool hasClearSky;

	// Token: 0x04006837 RID: 26679
	private int obstructedTile = -1;

	// Token: 0x04006838 RID: 26680
	public const int MAXIMUM_ROCKET_HEIGHT = 35;

	// Token: 0x04006839 RID: 26681
	public const float FIRE_FX_HEIGHT = 10f;
}
