using System;
using TUNING;
using UnityEngine;

// Token: 0x0200082E RID: 2094
public class AbsorbCellQuery : PathFinderQuery
{
	// Token: 0x060024E3 RID: 9443 RVA: 0x000BC7B4 File Offset: 0x000BA9B4
	public AbsorbCellQuery()
	{
		this.checker = Game.Instance.safetyConditions.AbsorbCellCellChecker;
	}

	// Token: 0x060024E4 RID: 9444 RVA: 0x001D7A34 File Offset: 0x001D5C34
	public AbsorbCellQuery Reset(MinionBrain brain, bool criticalMode, float currentOxygenTankMass, float breathPercentage, int allowCellEvenIfReserved, bool isRecoveringFromSuffocation)
	{
		this.brain = brain;
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetOxygenScore = float.MinValue;
		this.targetCellSafetyFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		this.targetBreathableMassAvailable = 0f;
		this.criticalMode = criticalMode;
		this.bionicOxygenRemaining = currentOxygenTankMass;
		this.breathPercentage = breathPercentage;
		this.allowCellEvenIfReserved = allowCellEvenIfReserved;
		this.context = new SafetyChecker.Context(brain);
		ScaldingMonitor.Instance instance = (brain == null) ? null : brain.GetSMI<ScaldingMonitor.Instance>();
		this.scaldingTreshold = ((instance == null) ? -1f : instance.GetScaldingThreshold());
		this.isRecoveringFromSuffocation = isRecoveringFromSuffocation;
		return this;
	}

	// Token: 0x060024E5 RID: 9445 RVA: 0x001D7AD8 File Offset: 0x001D5CD8
	public static AbsorbCellQuery.AbsorbOxygenSafeCellFlags GetAbsorbOxygenFlags(int cell, MinionBrain brain, float scaldingTreshold, out float totalBreathableMassAroundCell, out float breathableCellRatioInSample, int allowCellEvenIfReserved)
	{
		totalBreathableMassAroundCell = 0f;
		breathableCellRatioInSample = 0f;
		int num = Grid.CellAbove(cell);
		if (!Grid.IsValidCell(num))
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		if (Grid.Solid[cell] || Grid.Solid[num])
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		if (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[num])
		{
			return (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		}
		bool flag = cell == allowCellEvenIfReserved || brain.IsCellClear(cell);
		bool flag2 = !Grid.Element[cell].IsLiquid;
		bool flag3 = !Grid.Element[num].IsLiquid;
		bool flag4 = scaldingTreshold < 0f || Grid.Temperature[cell] < scaldingTreshold;
		bool flag5 = Grid.Radiation[cell] < 250f;
		bool flag6 = false;
		if (brain.OxygenBreather != null)
		{
			for (int i = 0; i < GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length; i++)
			{
				int num2 = Grid.OffsetCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS[i]);
				if (Grid.IsValidCell(num2) && Grid.AreCellsInSameWorld(cell, num2) && Grid.Element[num2].HasTag(GameTags.Breathable))
				{
					breathableCellRatioInSample += 1f / (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
				}
			}
			flag6 = GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(cell, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, brain.OxygenBreather, out totalBreathableMassAroundCell).IsBreathable;
		}
		bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
		AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenSafeCellFlags = (AbsorbCellQuery.AbsorbOxygenSafeCellFlags)0;
		if (flag4)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotScaldingTemperatures;
		}
		if (flag5)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotRadiated;
		}
		if (flag6)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsBreathable;
		}
		if (flag)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsClear;
		}
		if (flag7)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotTube;
		}
		if (flag2)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquid;
		}
		if (flag3)
		{
			absorbOxygenSafeCellFlags |= AbsorbCellQuery.AbsorbOxygenSafeCellFlags.IsNotLiquidOnMyFace;
		}
		return absorbOxygenSafeCellFlags;
	}

	// Token: 0x060024E6 RID: 9446 RVA: 0x001D7C98 File Offset: 0x001D5E98
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		float num = 0.1f * (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
		float num2 = 2.5f * (float)GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length;
		float num3 = (float)(54 / GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS.Length);
		float num4 = num3 / 2.8f;
		float num5 = (float)cost;
		bool flag;
		this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (flag)
		{
			float num6 = 0.03f;
			float num7 = num5 / 10f;
			float num8 = num6 * num7;
			float num9 = 0f;
			float num10 = 0f;
			AbsorbCellQuery.AbsorbOxygenSafeCellFlags absorbOxygenFlags = AbsorbCellQuery.GetAbsorbOxygenFlags(cell, this.brain, this.scaldingTreshold, out num9, out num10, this.allowCellEvenIfReserved);
			num9 = Mathf.Clamp(num9, 0f, num3);
			if (this.criticalMode)
			{
				if (!this.isRecoveringFromSuffocation && this.breathPercentage > DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT && num9 < num)
				{
					num9 = 0f;
				}
			}
			else if (num9 < num4)
			{
				num9 = 0f;
			}
			float num11 = (float)absorbOxygenFlags;
			float num12 = 10f * num10;
			float num13 = num9 * num12 - num8;
			bool flag2 = false;
			if (this.targetCell == Grid.InvalidCell)
			{
				flag2 = true;
			}
			bool flag3 = this.targetBreathableMassAvailable > 0f;
			bool flag4 = num5 < (float)this.targetCost;
			bool flag5 = this.targetOxygenScore >= num2;
			bool flag6 = num11 >= (float)this.targetCellSafetyFlags || !flag3;
			float num14 = this.targetOxygenScore;
			if (this.criticalMode)
			{
				num14 = Mathf.Min(num2, num14);
			}
			if (num13 >= num14 && flag6)
			{
				if (this.criticalMode)
				{
					if (flag4 || !flag5)
					{
						flag2 = true;
					}
				}
				else
				{
					flag2 = true;
				}
			}
			flag2 = (flag2 && num9 > DUPLICANTSTATS.BIONICS.BaseStats.NO_OXYGEN_THRESHOLD);
			if (flag2)
			{
				this.targetBreathableMassAvailable = num9;
				this.targetCellSafetyFlags = absorbOxygenFlags;
				this.targetCost = cost;
				this.targetCell = cell;
				this.targetOxygenScore = num13;
			}
		}
		return false;
	}

	// Token: 0x060024E7 RID: 9447 RVA: 0x000BC7E3 File Offset: 0x000BA9E3
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001962 RID: 6498
	private MinionBrain brain;

	// Token: 0x04001963 RID: 6499
	private float scaldingTreshold = -1f;

	// Token: 0x04001964 RID: 6500
	private int targetCell;

	// Token: 0x04001965 RID: 6501
	private int targetCost;

	// Token: 0x04001966 RID: 6502
	private float targetOxygenScore;

	// Token: 0x04001967 RID: 6503
	private bool criticalMode;

	// Token: 0x04001968 RID: 6504
	private float bionicOxygenRemaining;

	// Token: 0x04001969 RID: 6505
	private float breathPercentage;

	// Token: 0x0400196A RID: 6506
	private float targetBreathableMassAvailable;

	// Token: 0x0400196B RID: 6507
	public AbsorbCellQuery.AbsorbOxygenSafeCellFlags targetCellSafetyFlags;

	// Token: 0x0400196C RID: 6508
	public float targetCellBreathabilityScore;

	// Token: 0x0400196D RID: 6509
	private int allowCellEvenIfReserved = -1;

	// Token: 0x0400196E RID: 6510
	private SafetyChecker checker;

	// Token: 0x0400196F RID: 6511
	private SafetyChecker.Context context;

	// Token: 0x04001970 RID: 6512
	private bool isRecoveringFromSuffocation;

	// Token: 0x0200082F RID: 2095
	public enum AbsorbOxygenSafeCellFlags
	{
		// Token: 0x04001972 RID: 6514
		IsNotTube = 1,
		// Token: 0x04001973 RID: 6515
		IsNotRadiated,
		// Token: 0x04001974 RID: 6516
		IsBreathable = 4,
		// Token: 0x04001975 RID: 6517
		IsNotScaldingTemperatures = 8,
		// Token: 0x04001976 RID: 6518
		IsClear = 16,
		// Token: 0x04001977 RID: 6519
		IsNotLiquidOnMyFace = 32,
		// Token: 0x04001978 RID: 6520
		IsNotLiquid = 64
	}
}
