using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020018D0 RID: 6352
public readonly struct SkyVisibilityInfo
{
	// Token: 0x06008358 RID: 33624 RVA: 0x0034ED34 File Offset: 0x0034CF34
	public SkyVisibilityInfo(CellOffset scanLeftOffset, int scanLeftCount, CellOffset scanRightOffset, int scanRightCount, int verticalStep)
	{
		this.scanLeftOffset = scanLeftOffset;
		this.scanLeftCount = scanLeftCount;
		this.scanRightOffset = scanRightOffset;
		this.scanRightCount = scanRightCount;
		this.verticalStep = verticalStep;
		this.totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
	}

	// Token: 0x06008359 RID: 33625 RVA: 0x000FAD90 File Offset: 0x000F8F90
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(GameObject gameObject)
	{
		return this.GetVisibilityOf(Grid.PosToCell(gameObject));
	}

	// Token: 0x0600835A RID: 33626 RVA: 0x0034ED80 File Offset: 0x0034CF80
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(int buildingCenterCellId)
	{
		int num = 0;
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[buildingCenterCellId]);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, world);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, world);
		if (this.scanLeftOffset.x == this.scanRightOffset.x)
		{
			num = Mathf.Max(0, num - 1);
		}
		return new ValueTuple<bool, float>(num > 0, (float)num / (float)this.totalColumnsCount);
	}

	// Token: 0x0600835B RID: 33627 RVA: 0x0034EE1C File Offset: 0x0034D01C
	public void CollectVisibleCellsTo(HashSet<int> visibleCells, int buildingBottomLeftCellId, WorldContainer originWorld)
	{
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, originWorld);
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, originWorld);
	}

	// Token: 0x0600835C RID: 33628 RVA: 0x0034EE6C File Offset: 0x0034D06C
	private static void ScanAndCollectVisibleCellsTo(HashSet<int> visibleCells, int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			int num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
			if (!SkyVisibilityInfo.IsVisible(num, originWorld))
			{
				break;
			}
			visibleCells.Add(num);
		}
	}

	// Token: 0x0600835D RID: 33629 RVA: 0x0034EEA8 File Offset: 0x0034D0A8
	private static int ScanAndGetVisibleCellCount(int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			if (!SkyVisibilityInfo.IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
			{
				return i;
			}
		}
		return stepCountInclusive + 1;
	}

	// Token: 0x0600835E RID: 33630 RVA: 0x0034EEDC File Offset: 0x0034D0DC
	public static bool IsVisible(int cellId, WorldContainer originWorld)
	{
		if (!Grid.IsValidCell(cellId))
		{
			return false;
		}
		if (Grid.ExposedToSunlight[cellId] > 0)
		{
			return true;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cellId]);
		if (world != null && world.IsModuleInterior)
		{
			return true;
		}
		originWorld != world;
		return false;
	}

	// Token: 0x0400640D RID: 25613
	public readonly CellOffset scanLeftOffset;

	// Token: 0x0400640E RID: 25614
	public readonly int scanLeftCount;

	// Token: 0x0400640F RID: 25615
	public readonly CellOffset scanRightOffset;

	// Token: 0x04006410 RID: 25616
	public readonly int scanRightCount;

	// Token: 0x04006411 RID: 25617
	public readonly int verticalStep;

	// Token: 0x04006412 RID: 25618
	public readonly int totalColumnsCount;
}
