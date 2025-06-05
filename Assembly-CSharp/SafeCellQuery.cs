using System;

// Token: 0x0200082C RID: 2092
public class SafeCellQuery : PathFinderQuery
{
	// Token: 0x060024DE RID: 9438 RVA: 0x000BC777 File Offset: 0x000BA977
	public SafeCellQuery Reset(MinionBrain brain, bool avoid_light, SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags)0)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetCellFlags = (SafeCellQuery.SafeFlags)0;
		this.avoid_light = avoid_light;
		this.ignoredFlags = ignoredFlags;
		return this;
	}

	// Token: 0x060024DF RID: 9439 RVA: 0x001D77CC File Offset: 0x001D59CC
	public static SafeCellQuery.SafeFlags GetFlags(int cell, MinionBrain brain, bool avoid_light = false, SafeCellQuery.SafeFlags ignoredFlags = (SafeCellQuery.SafeFlags)0)
	{
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (SafeCellQuery.SafeFlags)0;
		}
		bool flag = brain.IsCellClear(cell);
		bool flag2 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0 || !Grid.Element[cell].IsLiquid;
		bool flag3 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace) != (SafeCellQuery.SafeFlags)0 || !Grid.Element[num].IsLiquid;
		bool flag4 = (ignoredFlags & SafeCellQuery.SafeFlags.CorrectTemperature) != (SafeCellQuery.SafeFlags)0 || (Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		bool flag5 = (ignoredFlags & SafeCellQuery.SafeFlags.IsNotRadiated) != (SafeCellQuery.SafeFlags)0 || Grid.Radiation[cell] < 250f;
		bool flag6 = (ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 || brain.OxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, Grid.DefaultOffset, brain.OxygenBreather).IsBreathable;
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
		bool flag8 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		bool flag9 = !avoid_light || SleepChore.IsDarkAtCell(cell);
		if (cell == Grid.PosToCell(brain))
		{
			flag6 = ((ignoredFlags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 || brain.OxygenBreather == null || brain.OxygenBreather.HasOxygen);
		}
		SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags)0;
		if (flag)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsClear;
		}
		if (flag4)
		{
			safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
		}
		if (flag5)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotRadiated;
		}
		if (flag6)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
		}
		if (flag7)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
		}
		if (flag8)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
		}
		if (flag2)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
		}
		if (flag3)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
		}
		if (flag9)
		{
			safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
		}
		return safeFlags;
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x001D79D4 File Offset: 0x001D5BD4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light, this.ignoredFlags);
		bool flag = flags > this.targetCellFlags;
		bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
		if (flag || flag2)
		{
			this.targetCellFlags = flags;
			this.targetCost = cost;
			this.targetCell = cell;
		}
		return false;
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000BC7AC File Offset: 0x000BA9AC
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001952 RID: 6482
	private MinionBrain brain;

	// Token: 0x04001953 RID: 6483
	private int targetCell;

	// Token: 0x04001954 RID: 6484
	private int targetCost;

	// Token: 0x04001955 RID: 6485
	public SafeCellQuery.SafeFlags targetCellFlags;

	// Token: 0x04001956 RID: 6486
	private bool avoid_light;

	// Token: 0x04001957 RID: 6487
	private SafeCellQuery.SafeFlags ignoredFlags;

	// Token: 0x0200082D RID: 2093
	public enum SafeFlags
	{
		// Token: 0x04001959 RID: 6489
		IsClear = 1,
		// Token: 0x0400195A RID: 6490
		IsLightOk,
		// Token: 0x0400195B RID: 6491
		IsNotLadder = 4,
		// Token: 0x0400195C RID: 6492
		IsNotTube = 8,
		// Token: 0x0400195D RID: 6493
		CorrectTemperature = 16,
		// Token: 0x0400195E RID: 6494
		IsNotRadiated = 32,
		// Token: 0x0400195F RID: 6495
		IsBreathable = 64,
		// Token: 0x04001960 RID: 6496
		IsNotLiquidOnMyFace = 128,
		// Token: 0x04001961 RID: 6497
		IsNotLiquid = 256
	}
}
