using System;

// Token: 0x02000821 RID: 2081
public class IdleCellQuery : PathFinderQuery
{
	// Token: 0x060024AC RID: 9388 RVA: 0x000BC500 File Offset: 0x000BA700
	public IdleCellQuery Reset(MinionBrain brain, int max_cost)
	{
		this.brain = brain;
		this.maxCost = max_cost;
		this.targetCell = Grid.InvalidCell;
		return this;
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x001D6EA4 File Offset: 0x001D50A4
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, false, (SafeCellQuery.SafeFlags)0);
		if ((flags & SafeCellQuery.SafeFlags.IsClear) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLadder) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotTube) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsBreathable) != (SafeCellQuery.SafeFlags)0 && (flags & SafeCellQuery.SafeFlags.IsNotLiquid) != (SafeCellQuery.SafeFlags)0)
		{
			this.targetCell = cell;
		}
		return cost > this.maxCost;
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x000BC51C File Offset: 0x000BA71C
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04001911 RID: 6417
	private MinionBrain brain;

	// Token: 0x04001912 RID: 6418
	private int targetCell;

	// Token: 0x04001913 RID: 6419
	private int maxCost;
}
