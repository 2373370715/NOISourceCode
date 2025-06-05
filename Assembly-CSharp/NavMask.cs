using System;

// Token: 0x02000802 RID: 2050
public class NavMask
{
	// Token: 0x0600242A RID: 9258 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public virtual bool IsTraversable(PathFinder.PotentialPath path, int from_cell, int cost, int transition_id, PathFinderAbilities abilities)
	{
		return true;
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ApplyTraversalToPath(ref PathFinder.PotentialPath path, int from_cell)
	{
	}
}
