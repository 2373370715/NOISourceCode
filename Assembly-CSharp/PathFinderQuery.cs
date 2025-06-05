using System;

// Token: 0x02000816 RID: 2070
public class PathFinderQuery
{
	// Token: 0x06002478 RID: 9336 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool IsMatch(int cell, int parent_cell, int cost)
	{
		return true;
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000BC272 File Offset: 0x000BA472
	public void SetResult(int cell, int cost, NavType nav_type)
	{
		this.resultCell = cell;
		this.resultNavType = nav_type;
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000BC282 File Offset: 0x000BA482
	public void ClearResult()
	{
		this.resultCell = -1;
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000BC28B File Offset: 0x000BA48B
	public virtual int GetResultCell()
	{
		return this.resultCell;
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000BC293 File Offset: 0x000BA493
	public NavType GetResultNavType()
	{
		return this.resultNavType;
	}

	// Token: 0x040018E8 RID: 6376
	protected int resultCell;

	// Token: 0x040018E9 RID: 6377
	private NavType resultNavType;
}
