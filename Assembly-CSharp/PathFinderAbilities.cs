﻿using System;

public abstract class PathFinderAbilities
{
	public PathFinderAbilities(Navigator navigator)
	{
		this.navigator = navigator;
	}

	public void Refresh()
	{
		this.prefabInstanceID = this.navigator.gameObject.GetComponent<KPrefabID>().InstanceID;
		this.Refresh(this.navigator);
	}

	protected abstract void Refresh(Navigator navigator);

	public abstract bool TraversePath(ref PathFinder.PotentialPath path, int from_cell, NavType from_nav_type, int cost, int transition_id, bool submerged);

	public virtual int GetSubmergedPathCostPenalty(PathFinder.PotentialPath path, NavGrid.Link link)
	{
		return 0;
	}

	protected Navigator navigator;

	protected int prefabInstanceID;
}
