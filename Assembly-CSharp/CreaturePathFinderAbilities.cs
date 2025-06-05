using System;
using Klei.AI;

// Token: 0x02000657 RID: 1623
public class CreaturePathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001CEC RID: 7404 RVA: 0x000B77F7 File Offset: 0x000B59F7
	public CreaturePathFinderAbilities(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x001B951C File Offset: 0x001B771C
	protected override void Refresh(Navigator navigator)
	{
		if (PathFinder.IsSubmerged(Grid.PosToCell(navigator)))
		{
			this.canTraverseSubmered = true;
			return;
		}
		AttributeInstance attributeInstance = Db.Get().Attributes.MaxUnderwaterTravelCost.Lookup(navigator);
		this.canTraverseSubmered = (attributeInstance == null);
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x000B7800 File Offset: 0x000B5A00
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		return !submerged || this.canTraverseSubmered;
	}

	// Token: 0x0400124F RID: 4687
	public bool canTraverseSubmered;
}
