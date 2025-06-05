using System;

// Token: 0x02000815 RID: 2069
public abstract class PathFinderAbilities
{
	// Token: 0x06002473 RID: 9331 RVA: 0x000BC23A File Offset: 0x000BA43A
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000BC249 File Offset: 0x000BA449
	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.Refresh(this.navigator);
	}

	// Token: 0x06002475 RID: 9333
	protected abstract void Refresh(Navigator navigator);

	// Token: 0x06002476 RID: 9334
	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	// Token: 0x06002477 RID: 9335 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	// Token: 0x040018E6 RID: 6374
	protected Navigator navigator;

	// Token: 0x040018E7 RID: 6375
	protected int prefabInstanceID;
}
