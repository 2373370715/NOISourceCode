using System;
using System.Collections.Generic;

// Token: 0x020007A8 RID: 1960
internal class ClearableManager
{
	// Token: 0x060022C9 RID: 8905 RVA: 0x001D0E58 File Offset: 0x001CF058
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.markedClearables.Allocate(new ClearableManager.MarkedClearable
		{
			clearable = clearable,
			pickupable = clearable.GetComponent<Pickupable>(),
			prioritizable = clearable.GetComponent<Prioritizable>()
		});
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000BB10C File Offset: 0x000B930C
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.markedClearables.Free(handle);
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x001D0E9C File Offset: 0x001CF09C
	public void CollectAndSortClearables(Navigator navigator)
	{
		this.sortedClearables.Clear();
		foreach (ClearableManager.MarkedClearable markedClearable in this.markedClearables.GetDataList())
		{
			int navigationCost = markedClearable.pickupable.GetNavigationCost(navigator, markedClearable.pickupable.cachedCell);
			if (navigationCost != -1)
			{
				this.sortedClearables.Add(new ClearableManager.SortedClearable
				{
					pickupable = markedClearable.pickupable,
					masterPriority = markedClearable.prioritizable.GetMasterPriority(),
					cost = navigationCost
				});
			}
		}
		this.sortedClearables.Sort(ClearableManager.SortedClearable.comparer);
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x001D0F60 File Offset: 0x001CF160
	public void CollectChores(List<GlobalChoreProvider.Fetch> fetches, ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		ChoreType transport = Db.Get().ChoreTypes.Transport;
		int personalPriority = consumer_state.consumer.GetPersonalPriority(transport);
		int priority = Game.Instance.advancedPersonalPriorities ? transport.explicitPriority : transport.priority;
		bool flag = false;
		for (int i = 0; i < this.sortedClearables.Count; i++)
		{
			ClearableManager.SortedClearable sortedClearable = this.sortedClearables[i];
			Pickupable pickupable = sortedClearable.pickupable;
			PrioritySetting masterPriority = sortedClearable.masterPriority;
			Chore.Precondition.Context item = default(Chore.Precondition.Context);
			item.personalPriority = personalPriority;
			KPrefabID kprefabID = pickupable.KPrefabID;
			int num = 0;
			while (fetches != null && num < fetches.Count)
			{
				GlobalChoreProvider.Fetch fetch = fetches[num];
				if ((fetch.chore.criteria == FetchChore.MatchCriteria.MatchID && fetch.chore.tags.Contains(kprefabID.PrefabTag)) || (fetch.chore.criteria == FetchChore.MatchCriteria.MatchTags && kprefabID.HasTag(fetch.chore.tagsFirst)))
				{
					item.Set(fetch.chore, consumer_state, false, pickupable);
					item.choreTypeForPermission = transport;
					item.RunPreconditions();
					if (item.IsSuccess())
					{
						item.masterPriority = masterPriority;
						item.priority = priority;
						item.interruptPriority = transport.interruptPriority;
						succeeded.Add(item);
						flag = true;
						break;
					}
				}
				num++;
			}
			if (flag)
			{
				break;
			}
		}
	}

	// Token: 0x04001754 RID: 5972
	private KCompactedVector<ClearableManager.MarkedClearable> markedClearables = new KCompactedVector<ClearableManager.MarkedClearable>(0);

	// Token: 0x04001755 RID: 5973
	private List<ClearableManager.SortedClearable> sortedClearables = new List<ClearableManager.SortedClearable>();

	// Token: 0x020007A9 RID: 1961
	private struct MarkedClearable
	{
		// Token: 0x04001756 RID: 5974
		public Clearable clearable;

		// Token: 0x04001757 RID: 5975
		public Pickupable pickupable;

		// Token: 0x04001758 RID: 5976
		public Prioritizable prioritizable;
	}

	// Token: 0x020007AA RID: 1962
	private struct SortedClearable
	{
		// Token: 0x04001759 RID: 5977
		public Pickupable pickupable;

		// Token: 0x0400175A RID: 5978
		public PrioritySetting masterPriority;

		// Token: 0x0400175B RID: 5979
		public int cost;

		// Token: 0x0400175C RID: 5980
		public static ClearableManager.SortedClearable.Comparer comparer = new ClearableManager.SortedClearable.Comparer();

		// Token: 0x020007AB RID: 1963
		public class Comparer : IComparer<ClearableManager.SortedClearable>
		{
			// Token: 0x060022CF RID: 8911 RVA: 0x001D10D0 File Offset: 0x001CF2D0
			public int Compare(ClearableManager.SortedClearable a, ClearableManager.SortedClearable b)
			{
				int num = b.masterPriority.priority_value - a.masterPriority.priority_value;
				if (num == 0)
				{
					return a.cost - b.cost;
				}
				return num;
			}
		}
	}
}
