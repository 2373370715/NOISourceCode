using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001366 RID: 4966
public class GameNavGrids
{
	// Token: 0x060065B7 RID: 26039 RVA: 0x002D3100 File Offset: 0x002D1300
	public GameNavGrids(Pathfinding pathfinding)
	{
		this.CreateDuplicantNavigation(pathfinding);
		this.WalkerGrid1x1 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerBabyGrid1x1 = this.CreateWalkerBabyNavigation(pathfinding, "WalkerBabyNavGrid", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.WalkerGrid1x2 = this.CreateWalkerNavigation(pathfinding, "WalkerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.WalkerGrid2x2 = this.CreateWalkerLargeNavigation(pathfinding, "WalkerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.CreateDreckoNavigation(pathfinding);
		this.CreateDreckoBabyNavigation(pathfinding);
		this.CreateFloaterNavigation(pathfinding);
		this.FlyerGrid1x1 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x1", new CellOffset[]
		{
			new CellOffset(0, 0)
		});
		this.FlyerGrid1x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid1x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		});
		this.FlyerGrid2x2 = this.CreateFlyerNavigation(pathfinding, "FlyerNavGrid2x2", new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(1, 1)
		});
		this.CreateSwimmerNavigation(pathfinding);
		this.CreateDiggerNavigation(pathfinding);
		this.CreateSquirrelNavigation(pathfinding);
	}

	// Token: 0x060065B8 RID: 26040 RVA: 0x002D32A4 File Offset: 0x002D14A4
	private void CreateDuplicantNavigation(Pathfinding pathfinding)
	{
		NavOffset[] invalid_nav_offsets = new NavOffset[]
		{
			new NavOffset(NavType.Floor, 1, 0),
			new NavOffset(NavType.Ladder, 1, 0),
			new NavOffset(NavType.Pole, 1, 0)
		};
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};
		NavGrid.Transition[] setA = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Teleport, 0, 0, NavAxis.NA, false, false, false, 14, "fall_pre", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Teleport, NavType.Floor, 0, 0, NavAxis.NA, false, false, false, 1, "fall_pst", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 14, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Ladder, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 1, 0, NavAxis.NA, false, false, true, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, 1, NavAxis.NA, true, true, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 0, -1, NavAxis.NA, true, true, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Ladder, 2, 0, NavAxis.NA, false, false, true, 25, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, 1, NavAxis.NA, false, false, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1),
				new NavOffset(NavType.Floor, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, -1),
				new NavOffset(NavType.Pole, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ladder, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 20, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 1, 0, NavAxis.NA, false, false, true, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, 1, NavAxis.NA, true, true, true, 50, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 0, -1, NavAxis.NA, true, true, false, 6, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Pole, NavType.Pole, 2, 0, NavAxis.NA, false, false, true, 50, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], invalid_nav_offsets, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Tube, 0, 2, NavAxis.NA, false, false, false, 40, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Floor, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ladder, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Ladder, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, 2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Pole, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, 0, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, 0, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -1, NavAxis.NA, false, false, false, 7, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 1, -2, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -1, NavAxis.NA, false, false, false, 13, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 2, -2, NavAxis.NA, false, false, false, 17, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(1, -1),
				new CellOffset(2, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -1, NavAxis.NA, false, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Pole, 0, -2, NavAxis.NA, false, false, false, 10, "", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 0, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, 1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 0, -1, NavAxis.NA, true, false, false, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, 1)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, 1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.Y, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 0, -1)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Tube, NavType.Tube, 1, -1, NavAxis.X, false, false, false, 10, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Tube, 1, 0)
			}, new NavOffset[0], false, 2.2f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, false, false, 15, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, 1)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, false, 25, "", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0),
				new NavOffset(NavType.Hover, 0, -1)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Hover, 0, 1, NavAxis.NA, false, false, false, 20, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 1, 0, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Floor, 0, -1, NavAxis.NA, false, false, false, 15, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] setB = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false)
		};
		NavGrid.Transition[] transitions = this.MirrorTransitions(this.CombineTransitions(setA, setB));
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_default"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ladder,
				idleAnim = "ladder_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Pole,
				idleAnim = "pole_idle"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Tube,
				idleAnim = "tube_idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "hover_hover_1_0_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Teleport,
				idleAnim = "idle_default"
			}
		};
		this.DuplicantGrid = new NavGrid("MinionNavGrid", transitions, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator(),
			new GameNavGrids.PoleValidator(),
			new GameNavGrids.TubeValidator(),
			new GameNavGrids.TeleporterValidator(),
			new GameNavGrids.FlyingValidator(true, true, true)
		}, 2, 3, 32);
		this.DuplicantGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.DuplicantGrid);
		NavGrid.Transition[] setB2 = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, false, 30, "climb_down_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[]
			{
				new CellOffset(1, 1)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, -1),
				new NavOffset(NavType.Ladder, 1, -1),
				new NavOffset(NavType.Pole, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, false, 30, "climb_up_2_1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(1, 2)
			}, new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0),
				new NavOffset(NavType.Ladder, 1, 0),
				new NavOffset(NavType.Pole, 1, 0),
				new NavOffset(NavType.Floor, 1, 1),
				new NavOffset(NavType.Ladder, 1, 1),
				new NavOffset(NavType.Pole, 1, 1)
			}, false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(this.CombineTransitions(setA, setB2));
		this.RobotGrid = new NavGrid("RobotNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(true),
			new GameNavGrids.LadderValidator()
		}, 2, 3, 22);
		this.RobotGrid.updateEveryFrame = true;
		pathfinding.AddNavGrid(this.RobotGrid);
	}

	// Token: 0x060065B9 RID: 26041 RVA: 0x002D589C File Offset: 0x002D3A9C
	private NavGrid CreateWalkerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060065BA RID: 26042 RVA: 0x002D5AD8 File Offset: 0x002D3CD8
	private NavGrid CreateWalkerBabyNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060065BB RID: 26043 RVA: 0x002D5B84 File Offset: 0x002D3D84
	private NavGrid CreateWalkerLargeNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, true),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 1, "floor_floor_2_1", new CellOffset[]
			{
				new CellOffset(1, 1),
				new CellOffset(1, 2),
				new CellOffset(2, 1),
				new CellOffset(0, 2)
			}, new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0)
			}, new NavOffset[0], new NavOffset[0], true, 1f, true),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, -1, NavAxis.NA, false, false, true, 1, "floor_floor_2_-1", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(2, 0),
				new CellOffset(2, -1),
				new CellOffset(1, 1)
			}, new CellOffset[]
			{
				new CellOffset(2, -2)
			}, new NavOffset[0], new NavOffset[0], true, 1f, true)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false)
		}, 2, 3, array.Length);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060065BC RID: 26044 RVA: 0x002D5D40 File Offset: 0x002D3F40
	private void CreateDreckoNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 2, NavAxis.NA, false, false, true, 5, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 2)
			}, new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 4, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0),
				new NavOffset(NavType.Floor, 2, 2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, 0, 1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, -1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, -1, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 0, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 1, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_1", new CellOffset[0], new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 0, 0)
			}, new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.LeftWall, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -2, -1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, -1),
				new CellOffset(-1, -1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -1, -1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Ceiling, -2, -1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 2, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(-1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.RightWall, -1, 2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 2, 1, NavAxis.NA, false, false, true, 2, "floor_wall_1_-2", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 1, 1)
			}, new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Floor, 2, 1)
			}, true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoGrid = new NavGrid("DreckoNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoGrid);
	}

	// Token: 0x060065BD RID: 26045 RVA: 0x002D66A8 File Offset: 0x002D48A8
	private void CreateDreckoBabyNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.DreckoBabyGrid = new NavGrid("DreckoBabyNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 16);
		pathfinding.AddNavGrid(this.DreckoBabyGrid);
	}

	// Token: 0x060065BE RID: 26046 RVA: 0x002D6B38 File Offset: 0x002D4D38
	private void CreateFloaterNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -1)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, 0)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 0, -2)
			}, false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, 1),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, 0)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, 0, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 2, -1, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1),
				new CellOffset(1, -2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 2, -2)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, 1)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -2, NavAxis.NA, false, false, true, 3, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[]
			{
				new NavOffset(NavType.Hover, 1, -3)
			}, true, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "swim_idle_loop"
			}
		};
		this.FloaterGrid = new NavGrid("FloaterNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.HoverValidator(),
			new GameNavGrids.SwimValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.FloaterGrid);
	}

	// Token: 0x060065BF RID: 26047 RVA: 0x002D7178 File Offset: 0x002D5378
	private NavGrid CreateFlyerNavigation(Pathfinding pathfinding, string id, CellOffset[] bounding_offsets)
	{
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 2, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, 1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 1, -1, NavAxis.NA, true, true, true, 2, "hover_hover_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Hover, NavType.Hover, 0, -1, NavAxis.NA, true, true, true, 3, "hover_hover_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 5, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 10, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 0, 1, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Hover, 1, 0, NavAxis.NA, true, true, true, 1, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Hover,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		NavGrid navGrid = new NavGrid(id, transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FlyingValidator(false, false, false),
			new GameNavGrids.SwimValidator()
		}, 2, 2, 16);
		pathfinding.AddNavGrid(navGrid);
		return navGrid;
	}

	// Token: 0x060065C0 RID: 26048 RVA: 0x002D7524 File Offset: 0x002D5724
	private void CreateSwimmerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 0, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, 1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 1, -1, NavAxis.NA, true, true, true, 2, "swim_swim_1_0", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, 1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Swim, NavType.Swim, 0, -1, NavAxis.NA, true, true, true, 3, "swim_swim_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] array = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Swim,
				idleAnim = "idle_loop"
			}
		};
		this.SwimmerGrid = new NavGrid("SwimmerNavGrid", array, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SwimValidator()
		}, 1, 2, array.Length);
		pathfinding.AddNavGrid(this.SwimmerGrid);
	}

	// Token: 0x060065C1 RID: 26049 RVA: 0x002D7710 File Offset: 0x002D5910
	private void CreateDiggerNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 0, NavAxis.NA, false, false, true, 1, "idle1", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, 1, NavAxis.NA, false, false, true, 1, "idle2", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 0, 1, NavAxis.NA, false, false, true, 1, "idle3", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Solid, 1, -1, NavAxis.NA, false, false, true, 1, "idle4", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Solid, 0, -1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Floor, 0, 1, NavAxis.NA, false, false, true, 1, "drill_out", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Solid, 0, 1, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.Ceiling, 0, -1, NavAxis.NA, false, false, true, 1, "drill_out_ceiling", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.LeftWall, 1, 0, NavAxis.NA, false, false, true, 1, "drill_out_left_wall", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Solid, -1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Solid, NavType.RightWall, -1, 0, NavAxis.NA, false, false, true, 1, "drill_out_right_wall", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Solid, 1, 0, NavAxis.NA, false, true, true, 1, "drill_in", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Solid,
				idleAnim = "idle1"
			}
		};
		this.DiggerGrid = new NavGrid("DiggerNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.SolidValidator(),
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 22);
		pathfinding.AddNavGrid(this.DiggerGrid);
	}

	// Token: 0x060065C2 RID: 26050 RVA: 0x002D7EA8 File Offset: 0x002D60A8
	private void CreateSquirrelNavigation(Pathfinding pathfinding)
	{
		CellOffset[] bounding_offsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};
		NavGrid.Transition[] transitions = new NavGrid.Transition[]
		{
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 0, NavAxis.NA, true, true, true, 1, "", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 2, 0, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, 2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(0, 1),
				new CellOffset(0, 2)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -1, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.Floor, 1, -2, NavAxis.NA, false, false, true, 1, "", new CellOffset[]
			{
				new CellOffset(1, 0),
				new CellOffset(1, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.RightWall, 0, 1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.LeftWall, 0, -1, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.Ceiling, -1, 0, NavAxis.NA, true, true, true, 1, "floor_floor_1_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.RightWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Ceiling, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.LeftWall, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Floor, 0, 0, NavAxis.NA, false, false, true, 1, "floor_wall_0_0", new CellOffset[0], new CellOffset[0], new NavOffset[0], new NavOffset[0], false, 1f, false),
			new NavGrid.Transition(NavType.Floor, NavType.LeftWall, 1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.LeftWall, NavType.Ceiling, -1, -1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, -1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.Ceiling, NavType.RightWall, -1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(-1, 0)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false),
			new NavGrid.Transition(NavType.RightWall, NavType.Floor, 1, 1, NavAxis.NA, false, false, true, 1, "floor_wall_1_-1", new CellOffset[]
			{
				new CellOffset(0, 1)
			}, new CellOffset[0], new NavOffset[0], new NavOffset[0], true, 1f, false)
		};
		NavGrid.Transition[] transitions2 = this.MirrorTransitions(transitions);
		NavGrid.NavTypeData[] nav_type_data = new NavGrid.NavTypeData[]
		{
			new NavGrid.NavTypeData
			{
				navType = NavType.Floor,
				idleAnim = "idle_loop"
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.Ceiling,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0f, -1f, 0f),
				rotation = -3.1415927f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.RightWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(0.5f, -0.5f, 0f),
				rotation = -1.5707964f
			},
			new NavGrid.NavTypeData
			{
				navType = NavType.LeftWall,
				idleAnim = "idle_loop",
				animControllerOffset = new Vector3(-0.5f, -0.5f, 0f),
				rotation = -4.712389f
			}
		};
		this.SquirrelGrid = new NavGrid("SquirrelNavGrid", transitions2, nav_type_data, bounding_offsets, new NavTableValidator[]
		{
			new GameNavGrids.FloorValidator(false),
			new GameNavGrids.WallValidator(),
			new GameNavGrids.CeilingValidator()
		}, 2, 3, 20);
		pathfinding.AddNavGrid(this.SquirrelGrid);
	}

	// Token: 0x060065C3 RID: 26051 RVA: 0x002D84CC File Offset: 0x002D66CC
	private CellOffset[] MirrorOffsets(CellOffset[] offsets)
	{
		List<CellOffset> list = new List<CellOffset>();
		foreach (CellOffset cellOffset in offsets)
		{
			cellOffset.x = -cellOffset.x;
			list.Add(cellOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060065C4 RID: 26052 RVA: 0x002D8514 File Offset: 0x002D6714
	private NavOffset[] MirrorNavOffsets(NavOffset[] offsets)
	{
		List<NavOffset> list = new List<NavOffset>();
		foreach (NavOffset navOffset in offsets)
		{
			navOffset.navType = NavGrid.MirrorNavType(navOffset.navType);
			navOffset.offset.x = -navOffset.offset.x;
			list.Add(navOffset);
		}
		return list.ToArray();
	}

	// Token: 0x060065C5 RID: 26053 RVA: 0x002D8578 File Offset: 0x002D6778
	private NavGrid.Transition[] MirrorTransitions(NavGrid.Transition[] transitions)
	{
		List<NavGrid.Transition> list = new List<NavGrid.Transition>();
		foreach (NavGrid.Transition transition in transitions)
		{
			list.Add(transition);
			if (transition.x != 0 || transition.start == NavType.RightWall || transition.end == NavType.RightWall || transition.start == NavType.LeftWall || transition.end == NavType.LeftWall)
			{
				NavGrid.Transition transition2 = transition;
				transition2.x = -transition2.x;
				transition2.voidOffsets = this.MirrorOffsets(transition.voidOffsets);
				transition2.solidOffsets = this.MirrorOffsets(transition.solidOffsets);
				transition2.validNavOffsets = this.MirrorNavOffsets(transition.validNavOffsets);
				transition2.invalidNavOffsets = this.MirrorNavOffsets(transition.invalidNavOffsets);
				transition2.start = NavGrid.MirrorNavType(transition2.start);
				transition2.end = NavGrid.MirrorNavType(transition2.end);
				list.Add(transition2);
			}
		}
		list.Sort((NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return list.ToArray();
	}

	// Token: 0x060065C6 RID: 26054 RVA: 0x002D8698 File Offset: 0x002D6898
	private NavGrid.Transition[] CombineTransitions(NavGrid.Transition[] setA, NavGrid.Transition[] setB)
	{
		NavGrid.Transition[] array = new NavGrid.Transition[setA.Length + setB.Length];
		Array.Copy(setA, array, setA.Length);
		Array.Copy(setB, 0, array, setA.Length, setB.Length);
		Array.Sort<NavGrid.Transition>(array, (NavGrid.Transition x, NavGrid.Transition y) => x.cost.CompareTo(y.cost));
		return array;
	}

	// Token: 0x04004B7C RID: 19324
	public NavGrid DuplicantGrid;

	// Token: 0x04004B7D RID: 19325
	public NavGrid WalkerGrid1x1;

	// Token: 0x04004B7E RID: 19326
	public NavGrid WalkerBabyGrid1x1;

	// Token: 0x04004B7F RID: 19327
	public NavGrid WalkerGrid1x2;

	// Token: 0x04004B80 RID: 19328
	public NavGrid WalkerGrid2x2;

	// Token: 0x04004B81 RID: 19329
	public NavGrid DreckoGrid;

	// Token: 0x04004B82 RID: 19330
	public NavGrid DreckoBabyGrid;

	// Token: 0x04004B83 RID: 19331
	public NavGrid FloaterGrid;

	// Token: 0x04004B84 RID: 19332
	public NavGrid FlyerGrid1x2;

	// Token: 0x04004B85 RID: 19333
	public NavGrid FlyerGrid1x1;

	// Token: 0x04004B86 RID: 19334
	public NavGrid FlyerGrid2x2;

	// Token: 0x04004B87 RID: 19335
	public NavGrid SwimmerGrid;

	// Token: 0x04004B88 RID: 19336
	public NavGrid DiggerGrid;

	// Token: 0x04004B89 RID: 19337
	public NavGrid SquirrelGrid;

	// Token: 0x04004B8A RID: 19338
	public NavGrid RobotGrid;

	// Token: 0x02001367 RID: 4967
	public class SwimValidator : NavTableValidator
	{
		// Token: 0x060065C7 RID: 26055 RVA: 0x002D86F0 File Offset: 0x002D68F0
		public SwimValidator()
		{
			World instance = World.Instance;
			instance.OnLiquidChanged = (Action<int>)Delegate.Combine(instance.OnLiquidChanged, new Action<int>(this.OnLiquidChanged));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[9], new Action<int, object>(this.OnFoundationTileChanged));
		}

		// Token: 0x060065C8 RID: 26056 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnFoundationTileChanged(int cell, object unused)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065C9 RID: 26057 RVA: 0x002D874C File Offset: 0x002D694C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (!flag)
			{
				flag = Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f);
			}
			bool is_valid = Grid.IsWorldValidCell(cell) && flag && base.IsClear(cell, bounding_offsets, false);
			nav_table.SetValid(cell, NavType.Swim, is_valid);
		}

		// Token: 0x060065CA RID: 26058 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnLiquidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}
	}

	// Token: 0x02001368 RID: 4968
	public class FloorValidator : NavTableValidator
	{
		// Token: 0x060065CB RID: 26059 RVA: 0x002D879C File Offset: 0x002D699C
		public FloorValidator(bool is_dupe)
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
			this.isDupe = is_dupe;
		}

		// Token: 0x060065CC RID: 26060 RVA: 0x002D8800 File Offset: 0x002D6A00
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.FloorValidator.IsWalkableCell(cell, Grid.CellBelow(cell), this.isDupe);
			nav_table.SetValid(cell, NavType.Floor, flag && base.IsClear(cell, bounding_offsets, this.isDupe));
		}

		// Token: 0x060065CD RID: 26061 RVA: 0x002D883C File Offset: 0x002D6A3C
		public static bool IsWalkableCell(int cell, int anchor_cell, bool is_dupe)
		{
			if (!Grid.IsWorldValidCell(cell))
			{
				return false;
			}
			if (!Grid.IsWorldValidCell(anchor_cell))
			{
				return false;
			}
			if (!NavTableValidator.IsCellPassable(cell, is_dupe))
			{
				return false;
			}
			if (Grid.FakeFloor[anchor_cell])
			{
				return true;
			}
			if (Grid.Solid[anchor_cell])
			{
				return !Grid.DupePassable[anchor_cell];
			}
			return is_dupe && (Grid.NavValidatorMasks[cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) == (Grid.NavValidatorFlags)0 && (Grid.NavValidatorMasks[anchor_cell] & (Grid.NavValidatorFlags.Ladder | Grid.NavValidatorFlags.Pole)) > (Grid.NavValidatorFlags)0;
		}

		// Token: 0x060065CE RID: 26062 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065D1 RID: 26065 RVA: 0x002D88DC File Offset: 0x002D6ADC
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x04004B8B RID: 19339
		private bool isDupe;
	}

	// Token: 0x02001369 RID: 4969
	public class WallValidator : NavTableValidator
	{
		// Token: 0x060065D2 RID: 26066 RVA: 0x000E6D66 File Offset: 0x000E4F66
		public WallValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060065D3 RID: 26067 RVA: 0x002D8934 File Offset: 0x002D6B34
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellRight(cell));
			bool flag2 = GameNavGrids.WallValidator.IsWalkableCell(cell, Grid.CellLeft(cell));
			nav_table.SetValid(cell, NavType.RightWall, flag && base.IsClear(cell, bounding_offsets, false));
			nav_table.SetValid(cell, NavType.LeftWall, flag2 && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x000E6D94 File Offset: 0x000E4F94
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.CritterImpassable[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x000E6DD0 File Offset: 0x000E4FD0
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200136A RID: 4970
	public class CeilingValidator : NavTableValidator
	{
		// Token: 0x060065D7 RID: 26071 RVA: 0x000E6DF8 File Offset: 0x000E4FF8
		public CeilingValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060065D8 RID: 26072 RVA: 0x002D898C File Offset: 0x002D6B8C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = GameNavGrids.CeilingValidator.IsWalkableCell(cell, Grid.CellAbove(cell));
			nav_table.SetValid(cell, NavType.Ceiling, flag && base.IsClear(cell, bounding_offsets, false));
		}

		// Token: 0x060065D9 RID: 26073 RVA: 0x002D89C0 File Offset: 0x002D6BC0
		private static bool IsWalkableCell(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.IsWorldValidCell(anchor_cell))
			{
				if (!NavTableValidator.IsCellPassable(cell, false))
				{
					return false;
				}
				if (Grid.Solid[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[cell] && !Grid.FakeFloor[cell])
				{
					return false;
				}
				if (Grid.FakeFloor[anchor_cell])
				{
					return true;
				}
				if (Grid.HasDoor[anchor_cell])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060065DA RID: 26074 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065DB RID: 26075 RVA: 0x000E6E26 File Offset: 0x000E5026
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}

	// Token: 0x0200136B RID: 4971
	public class LadderValidator : NavTableValidator
	{
		// Token: 0x060065DC RID: 26076 RVA: 0x000E6E4E File Offset: 0x000E504E
		public LadderValidator()
		{
			Components.Ladders.Register(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}

		// Token: 0x060065DD RID: 26077 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnAddLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065DE RID: 26078 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnRemoveLadder(Ladder ladder)
		{
			int obj = Grid.PosToCell(ladder);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065DF RID: 26079 RVA: 0x000E6E78 File Offset: 0x000E5078
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Ladder, base.IsClear(cell, bounding_offsets, true) && Grid.HasLadder[cell]);
		}

		// Token: 0x060065E0 RID: 26080 RVA: 0x000E6E9B File Offset: 0x000E509B
		public override void Clear()
		{
			Components.Ladders.Unregister(new Action<Ladder>(this.OnAddLadder), new Action<Ladder>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200136C RID: 4972
	public class PoleValidator : GameNavGrids.LadderValidator
	{
		// Token: 0x060065E1 RID: 26081 RVA: 0x000E6EBF File Offset: 0x000E50BF
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Pole, base.IsClear(cell, bounding_offsets, true) && Grid.HasPole[cell]);
		}
	}

	// Token: 0x0200136D RID: 4973
	public class TubeValidator : NavTableValidator
	{
		// Token: 0x060065E3 RID: 26083 RVA: 0x000E6EEA File Offset: 0x000E50EA
		public TubeValidator()
		{
			Components.ITravelTubePieces.Register(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}

		// Token: 0x060065E4 RID: 26084 RVA: 0x002D8A34 File Offset: 0x002D6C34
		private void OnAddLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x002D8A34 File Offset: 0x002D6C34
		private void OnRemoveLadder(ITravelTubePiece tube)
		{
			int obj = Grid.PosToCell(tube.Position);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x000E6F14 File Offset: 0x000E5114
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			nav_table.SetValid(cell, NavType.Tube, Grid.HasTube[cell]);
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x000E6F29 File Offset: 0x000E5129
		public override void Clear()
		{
			Components.ITravelTubePieces.Unregister(new Action<ITravelTubePiece>(this.OnAddLadder), new Action<ITravelTubePiece>(this.OnRemoveLadder));
		}
	}

	// Token: 0x0200136E RID: 4974
	public class TeleporterValidator : NavTableValidator
	{
		// Token: 0x060065E8 RID: 26088 RVA: 0x000E6F4D File Offset: 0x000E514D
		public TeleporterValidator()
		{
			Components.NavTeleporters.Register(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnAddTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065EA RID: 26090 RVA: 0x002D88B4 File Offset: 0x002D6AB4
		private void OnRemoveTeleporter(NavTeleporter teleporter)
		{
			int obj = Grid.PosToCell(teleporter);
			if (this.onDirty != null)
			{
				this.onDirty(obj);
			}
		}

		// Token: 0x060065EB RID: 26091 RVA: 0x002D8A64 File Offset: 0x002D6C64
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = Grid.IsWorldValidCell(cell) && Grid.HasNavTeleporter[cell];
			nav_table.SetValid(cell, NavType.Teleport, is_valid);
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x000E6F77 File Offset: 0x000E5177
		public override void Clear()
		{
			Components.NavTeleporters.Unregister(new Action<NavTeleporter>(this.OnAddTeleporter), new Action<NavTeleporter>(this.OnRemoveTeleporter));
		}
	}

	// Token: 0x0200136F RID: 4975
	public class FlyingValidator : NavTableValidator
	{
		// Token: 0x060065ED RID: 26093 RVA: 0x002D8A94 File Offset: 0x002D6C94
		public FlyingValidator(bool exclude_floor = false, bool exclude_jet_suit_blockers = false, bool allow_door_traversal = false)
		{
			this.exclude_floor = exclude_floor;
			this.exclude_jet_suit_blockers = exclude_jet_suit_blockers;
			this.allow_door_traversal = allow_door_traversal;
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x060065EE RID: 26094 RVA: 0x002D8B2C File Offset: 0x002D6D2C
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool flag = false;
			if (Grid.IsWorldValidCell(Grid.CellAbove(cell)))
			{
				flag = (!Grid.IsSubstantialLiquid(cell, 0.35f) && base.IsClear(cell, bounding_offsets, this.allow_door_traversal));
				if (flag && this.exclude_floor)
				{
					int cell2 = Grid.CellBelow(cell);
					if (Grid.IsWorldValidCell(cell2))
					{
						flag = base.IsClear(cell2, bounding_offsets, this.allow_door_traversal);
					}
				}
				if (flag && this.exclude_jet_suit_blockers)
				{
					GameObject gameObject = Grid.Objects[cell, 1];
					flag = (gameObject == null || !gameObject.HasTag(GameTags.JetSuitBlocker));
				}
			}
			nav_table.SetValid(cell, NavType.Hover, flag);
		}

		// Token: 0x060065EF RID: 26095 RVA: 0x000E6F9B File Offset: 0x000E519B
		private void OnBuildingChange(int cell, object data)
		{
			this.MarkCellDirty(cell);
		}

		// Token: 0x060065F0 RID: 26096 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065F1 RID: 26097 RVA: 0x002D8BCC File Offset: 0x002D6DCC
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
			GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingChange));
		}

		// Token: 0x04004B8C RID: 19340
		private bool exclude_floor;

		// Token: 0x04004B8D RID: 19341
		private bool exclude_jet_suit_blockers;

		// Token: 0x04004B8E RID: 19342
		private bool allow_door_traversal;

		// Token: 0x04004B8F RID: 19343
		private HandleVector<int>.Handle buildingParititonerEntry;
	}

	// Token: 0x02001370 RID: 4976
	public class HoverValidator : NavTableValidator
	{
		// Token: 0x060065F2 RID: 26098 RVA: 0x002D8C48 File Offset: 0x002D6E48
		public HoverValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Combine(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}

		// Token: 0x060065F3 RID: 26099 RVA: 0x002D8CA8 File Offset: 0x002D6EA8
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			int num = Grid.CellBelow(cell);
			if (Grid.IsWorldValidCell(num))
			{
				bool flag = Grid.Solid[num] || Grid.FakeFloor[num] || Grid.IsSubstantialLiquid(num, 0.35f);
				nav_table.SetValid(cell, NavType.Hover, !Grid.IsSubstantialLiquid(cell, 0.35f) && flag && base.IsClear(cell, bounding_offsets, false));
			}
		}

		// Token: 0x060065F4 RID: 26100 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void MarkCellDirty(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065F5 RID: 26101 RVA: 0x002D8D14 File Offset: 0x002D6F14
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.MarkCellDirty));
			World instance2 = World.Instance;
			instance2.OnLiquidChanged = (Action<int>)Delegate.Remove(instance2.OnLiquidChanged, new Action<int>(this.MarkCellDirty));
		}
	}

	// Token: 0x02001371 RID: 4977
	public class SolidValidator : NavTableValidator
	{
		// Token: 0x060065F6 RID: 26102 RVA: 0x000E6FA4 File Offset: 0x000E51A4
		public SolidValidator()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}

		// Token: 0x060065F7 RID: 26103 RVA: 0x002D8D70 File Offset: 0x002D6F70
		public override void UpdateCell(int cell, NavTable nav_table, CellOffset[] bounding_offsets)
		{
			bool is_valid = GameNavGrids.SolidValidator.IsDiggable(cell, Grid.CellBelow(cell));
			nav_table.SetValid(cell, NavType.Solid, is_valid);
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x002D8D94 File Offset: 0x002D6F94
		public static bool IsDiggable(int cell, int anchor_cell)
		{
			if (Grid.IsWorldValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x060065F9 RID: 26105 RVA: 0x000E6D50 File Offset: 0x000E4F50
		private void OnSolidChanged(int cell)
		{
			if (this.onDirty != null)
			{
				this.onDirty(cell);
			}
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x000E6FD2 File Offset: 0x000E51D2
		public override void Clear()
		{
			World instance = World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
		}
	}
}
