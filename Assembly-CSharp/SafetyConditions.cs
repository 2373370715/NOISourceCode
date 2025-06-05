using System;
using System.Collections.Generic;

// Token: 0x02000825 RID: 2085
public class SafetyConditions
{
	// Token: 0x060024BD RID: 9405 RVA: 0x001D71F4 File Offset: 0x001D53F4
	public SafetyConditions()
	{
		int num = 1;
		this.IsNearby = new SafetyChecker.Condition("IsNearby", num *= 2, (int cell, int cost, SafetyChecker.Context context) => cost > 5);
		this.IsNotLedge = new SafetyChecker.Condition("IsNotLedge", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int i = Grid.CellBelow(Grid.CellLeft(cell));
			if (Grid.Solid[i])
			{
				return false;
			}
			int i2 = Grid.CellBelow(Grid.CellRight(cell));
			return Grid.Solid[i2];
		});
		this.IsNotLiquid = new SafetyChecker.Condition("IsNotLiquid", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !Grid.Element[cell].IsLiquid);
		this.IsNotCoveredInLiquid = new SafetyChecker.Condition("IsNotCoveredInLiquid", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return Grid.IsValidCell(num2) && (!Grid.Element[cell].IsLiquid || !Grid.Element[num2].IsLiquid);
		});
		this.IsNotLadder = new SafetyChecker.Condition("IsNotLadder", num *= 2, (int cell, int cost, SafetyChecker.Context context) => !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole));
		this.IsNotDoor = new SafetyChecker.Condition("IsNotDoor", num *= 2, delegate(int cell, int cost, SafetyChecker.Context context)
		{
			int num2 = Grid.CellAbove(cell);
			return !Grid.HasDoor[cell] && Grid.IsValidCell(num2) && !Grid.HasDoor[num2];
		});
		this.IsCorrectTemperature = new SafetyChecker.Condition("IsCorrectTemperature", num *= 2, (int cell, int cost, SafetyChecker.Context context) => Grid.Temperature[cell] > 285.15f && Grid.Temperature[cell] < 303.15f);
		this.IsWarming = new SafetyChecker.Condition("IsWarming", num *= 2, (int cell, int cost, SafetyChecker.Context context) => WarmthProvider.IsWarmCell(cell));
		this.IsCooling = new SafetyChecker.Condition("IsCooling", num *= 2, (int cell, int cost, SafetyChecker.Context context) => false);
		this.HasSomeOxygen = new SafetyChecker.Condition("HasSomeOxygen", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, Grid.DefaultOffset, context.oxygenBreather).IsBreathable);
		this.HasSomeOxygenAround = new SafetyChecker.Condition("HasSomeOxygenAround", num *= 2, (int cell, int cost, SafetyChecker.Context context) => context.oxygenBreather == null || GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, context.oxygenBreather).IsBreathable);
		this.IsClear = new SafetyChecker.Condition("IsClear", num * 2, (int cell, int cost, SafetyChecker.Context context) => context.minionBrain.IsCellClear(cell));
		this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsWarming
		}.ToArray());
		this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsCooling
		}.ToArray());
		this.AbsorbCellCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotCoveredInLiquid,
			this.IsNotDoor,
			this.HasSomeOxygenAround
		}.ToArray());
		List<SafetyChecker.Condition> list = new List<SafetyChecker.Condition>();
		list.Add(this.HasSomeOxygen);
		list.Add(this.IsNotDoor);
		this.RecoverBreathChecker = new SafetyChecker(list.ToArray());
		List<SafetyChecker.Condition> list2 = new List<SafetyChecker.Condition>(list);
		list2.Add(this.IsNotLiquid);
		list2.Add(this.IsCorrectTemperature);
		this.SafeCellChecker = new SafetyChecker(list2.ToArray());
		this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>(list2)
		{
			this.IsClear,
			this.IsNotLadder
		}.ToArray());
		this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>
		{
			this.IsNotLiquid,
			this.IsNotLedge,
			this.IsNearby
		}.ToArray());
	}

	// Token: 0x04001920 RID: 6432
	public SafetyChecker.Condition IsNotLiquid;

	// Token: 0x04001921 RID: 6433
	public SafetyChecker.Condition IsNotCoveredInLiquid;

	// Token: 0x04001922 RID: 6434
	public SafetyChecker.Condition IsNotLadder;

	// Token: 0x04001923 RID: 6435
	public SafetyChecker.Condition IsCorrectTemperature;

	// Token: 0x04001924 RID: 6436
	public SafetyChecker.Condition IsWarming;

	// Token: 0x04001925 RID: 6437
	public SafetyChecker.Condition IsCooling;

	// Token: 0x04001926 RID: 6438
	public SafetyChecker.Condition HasSomeOxygen;

	// Token: 0x04001927 RID: 6439
	public SafetyChecker.Condition HasSomeOxygenAround;

	// Token: 0x04001928 RID: 6440
	public SafetyChecker.Condition IsClear;

	// Token: 0x04001929 RID: 6441
	public SafetyChecker.Condition IsNotFoundation;

	// Token: 0x0400192A RID: 6442
	public SafetyChecker.Condition IsNotDoor;

	// Token: 0x0400192B RID: 6443
	public SafetyChecker.Condition IsNotLedge;

	// Token: 0x0400192C RID: 6444
	public SafetyChecker.Condition IsNearby;

	// Token: 0x0400192D RID: 6445
	public SafetyChecker WarmUpChecker;

	// Token: 0x0400192E RID: 6446
	public SafetyChecker CoolDownChecker;

	// Token: 0x0400192F RID: 6447
	public SafetyChecker RecoverBreathChecker;

	// Token: 0x04001930 RID: 6448
	public SafetyChecker AbsorbCellCellChecker;

	// Token: 0x04001931 RID: 6449
	public SafetyChecker VomitCellChecker;

	// Token: 0x04001932 RID: 6450
	public SafetyChecker SafeCellChecker;

	// Token: 0x04001933 RID: 6451
	public SafetyChecker IdleCellChecker;
}
