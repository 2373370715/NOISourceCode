using System;

// Token: 0x02000813 RID: 2067
public static class PathFinderQueries
{
	// Token: 0x06002471 RID: 9329 RVA: 0x001D6608 File Offset: 0x001D4808
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

	// Token: 0x040018CD RID: 6349
	public static CellQuery cellQuery = new CellQuery();

	// Token: 0x040018CE RID: 6350
	public static CellCostQuery cellCostQuery = new CellCostQuery();

	// Token: 0x040018CF RID: 6351
	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	// Token: 0x040018D0 RID: 6352
	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	// Token: 0x040018D1 RID: 6353
	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	// Token: 0x040018D2 RID: 6354
	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	// Token: 0x040018D3 RID: 6355
	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	// Token: 0x040018D4 RID: 6356
	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	// Token: 0x040018D5 RID: 6357
	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	// Token: 0x040018D6 RID: 6358
	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	// Token: 0x040018D7 RID: 6359
	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	// Token: 0x040018D8 RID: 6360
	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
