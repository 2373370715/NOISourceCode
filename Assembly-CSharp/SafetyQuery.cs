using System;

// Token: 0x0200082B RID: 2091
public class SafetyQuery : PathFinderQuery
{
	// Token: 0x060024DA RID: 9434 RVA: 0x000BC722 File Offset: 0x000BA922
	public SafetyQuery(SafetyChecker checker, KMonoBehaviour cmp, int max_cost)
	{
		this.checker = checker;
		this.cmp = cmp;
		this.maxCost = max_cost;
	}

	// Token: 0x060024DB RID: 9435 RVA: 0x000BC73F File Offset: 0x000BA93F
	public void Reset()
	{
		this.targetCell = PathFinder.InvalidCell;
		this.targetCost = int.MaxValue;
		this.targetConditions = 0;
		this.context = new SafetyChecker.Context(this.cmp);
	}

	// Token: 0x060024DC RID: 9436 RVA: 0x001D7760 File Offset: 0x001D5960
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		bool flag = false;
		int safetyConditions = this.checker.GetSafetyConditions(cell, cost, this.context, out flag);
		if (safetyConditions != 0 && (safetyConditions > this.targetConditions || (safetyConditions == this.targetConditions && cost < this.targetCost)))
		{
			this.targetCell = cell;
			this.targetConditions = safetyConditions;
			this.targetCost = cost;
			if (flag)
			{
				return true;
			}
		}
		return cost >= this.maxCost;
	}

	// Token: 0x060024DD RID: 9437 RVA: 0x000BC76F File Offset: 0x000BA96F
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x0400194B RID: 6475
	private int targetCell;

	// Token: 0x0400194C RID: 6476
	private int targetCost;

	// Token: 0x0400194D RID: 6477
	private int targetConditions;

	// Token: 0x0400194E RID: 6478
	private int maxCost;

	// Token: 0x0400194F RID: 6479
	private SafetyChecker checker;

	// Token: 0x04001950 RID: 6480
	private KMonoBehaviour cmp;

	// Token: 0x04001951 RID: 6481
	private SafetyChecker.Context context;
}
