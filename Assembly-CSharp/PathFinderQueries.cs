﻿using System;

public static class PathFinderQueries
{
	public static void Reset()
	{
		PathFinderQueries.cellQuery = new CellQuery();
		PathFinderQueries.cellCostQuery = new CellCostQuery();
		PathFinderQueries.cellArrayQuery = new CellArrayQuery();
		PathFinderQueries.cellOffsetQuery = new CellOffsetQuery();
		PathFinderQueries.safeCellQuery = new SafeCellQuery();
		PathFinderQueries.idleCellQuery = new IdleCellQuery();
		PathFinderQueries.drawNavGridQuery = new DrawNavGridQuery();
		PathFinderQueries.plantableCellQuery = new PlantableCellQuery();
		PathFinderQueries.mineableCellQuery = new MineableCellQuery();
		PathFinderQueries.staterpillarCellQuery = new StaterpillarCellQuery();
		PathFinderQueries.floorCellQuery = new FloorCellQuery();
		PathFinderQueries.buildingPlacementQuery = new BuildingPlacementQuery();
	}

	public static CellQuery cellQuery = new CellQuery();

	public static CellCostQuery cellCostQuery = new CellCostQuery();

	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
