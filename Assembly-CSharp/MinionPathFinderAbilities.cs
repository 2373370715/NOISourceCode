using System;
using System.Diagnostics;

// Token: 0x02000658 RID: 1624
public class MinionPathFinderAbilities : PathFinderAbilities
{
	// Token: 0x06001CEF RID: 7407 RVA: 0x001B9560 File Offset: 0x001B7760
	public MinionPathFinderAbilities(Navigator navigator) : base(navigator)
	{
		this.transitionVoidOffsets = new CellOffset[navigator.NavGrid.transitions.Length][];
		for (int i = 0; i < this.transitionVoidOffsets.Length; i++)
		{
			this.transitionVoidOffsets[i] = navigator.NavGrid.transitions[i].voidOffsets;
		}
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000B7811 File Offset: 0x000B5A11
	protected override void Refresh(Navigator navigator)
	{
		this.proxyID = navigator.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
		this.out_of_fuel = navigator.HasTag(GameTags.JetSuitOutOfFuel);
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000B7844 File Offset: 0x000B5A44
	public void SetIdleNavMaskEnabled(bool enabled)
	{
		this.idleNavMaskEnabled = enabled;
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000B784D File Offset: 0x000B5A4D
	private static bool IsAccessPermitted(int proxyID, int cell, int from_cell, NavType from_nav_type)
	{
		return Grid.HasPermission(cell, proxyID, from_cell, from_nav_type);
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000B7858 File Offset: 0x000B5A58
	public override int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		if (!path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasLeadSuit))
		{
			return (int)(link.cost * 2);
		}
		return 0;
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x001B95C0 File Offset: 0x001B77C0
	public override bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged)
	{
		if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, path.cell, from_cell, from_nav_type))
		{
			return false;
		}
		foreach (CellOffset offset in this.transitionVoidOffsets[transition_id])
		{
			int cell = Grid.OffsetCell(from_cell, offset);
			if (!MinionPathFinderAbilities.IsAccessPermitted(this.proxyID, cell, from_cell, from_nav_type))
			{
				return false;
			}
		}
		if (path.navType == NavType.Tube && from_nav_type == NavType.Floor && !Grid.HasUsableTubeEntrance(from_cell, this.prefabInstanceID))
		{
			return false;
		}
		if (path.navType == NavType.Hover && (this.out_of_fuel || !path.HasFlag(PathFinder.PotentialPath.Flags.HasJetPack)))
		{
			return false;
		}
		Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
		PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
		bool flag = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out flags2) && (flags & Grid.SuitMarker.Flags.Operational) > (Grid.SuitMarker.Flags)0;
		bool flag2 = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);
		bool flag3 = path.HasAnyFlag(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
		if (flag)
		{
			bool flag4 = path.HasFlag(flags2);
			if (flag2)
			{
				if (!flag3 && !Grid.HasSuit(from_cell, this.prefabInstanceID))
				{
					return false;
				}
			}
			else if (flag3 && (flags & Grid.SuitMarker.Flags.OnlyTraverseIfUnequipAvailable) != (Grid.SuitMarker.Flags)0 && (!flag4 || !Grid.HasEmptyLocker(from_cell, this.prefabInstanceID)))
			{
				return false;
			}
		}
		if (this.idleNavMaskEnabled && (Grid.PreventIdleTraversal[path.cell] || Grid.PreventIdleTraversal[from_cell]))
		{
			return false;
		}
		if (flag)
		{
			if (flag2)
			{
				if (!flag3)
				{
					path.SetFlags(flags2);
				}
			}
			else
			{
				path.ClearFlags(PathFinder.PotentialPath.Flags.HasAtmoSuit | PathFinder.PotentialPath.Flags.HasJetPack | PathFinder.PotentialPath.Flags.HasOxygenMask | PathFinder.PotentialPath.Flags.HasLeadSuit);
			}
		}
		return true;
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void BeginSample(string region_name)
	{
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_NAVIGATION_MASK_PROFILING")]
	private void EndSample(string region_name)
	{
	}

	// Token: 0x04001250 RID: 4688
	private CellOffset[][] transitionVoidOffsets;

	// Token: 0x04001251 RID: 4689
	private int proxyID;

	// Token: 0x04001252 RID: 4690
	private bool out_of_fuel;

	// Token: 0x04001253 RID: 4691
	private bool idleNavMaskEnabled;
}
