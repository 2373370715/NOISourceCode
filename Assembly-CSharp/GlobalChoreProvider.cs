using System;
using System.Collections.Generic;

// Token: 0x020007A3 RID: 1955
public class GlobalChoreProvider : ChoreProvider, IRender200ms
{
	// Token: 0x060022B2 RID: 8882 RVA: 0x000BB022 File Offset: 0x000B9222
	public static void DestroyInstance()
	{
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000BB02A File Offset: 0x000B922A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalChoreProvider.Instance = this;
		this.clearableManager = new ClearableManager();
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x001D0770 File Offset: 0x001CE970
	protected override void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<FetchChore> chores;
		if (this.fetchMap.TryGetValue(parentWorldId, out chores))
		{
			base.ClearWorldChores<FetchChore>(chores, num);
		}
		base.OnWorldRemoved(data);
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x001D07B4 File Offset: 0x001CE9B4
	protected override void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255)
		{
			return;
		}
		base.OnWorldParentChanged(data);
		List<FetchChore> oldChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<FetchChore> newChores;
		if (!this.fetchMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.fetchMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<FetchChore>());
		}
		base.TransferChores<FetchChore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x001D0840 File Offset: 0x001CEA40
	public override void AddChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list = (this.fetchMap[myParentWorldId] = new List<FetchChore>());
			}
			chore.provider = this;
			list.Add(fetchChore);
			return;
		}
		base.AddChore(chore);
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x001D089C File Offset: 0x001CEA9C
	public override void RemoveChore(Chore chore)
	{
		FetchChore fetchChore = chore as FetchChore;
		if (fetchChore != null)
		{
			int myParentWorldId = fetchChore.gameObject.GetMyParentWorldId();
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(myParentWorldId, out list))
			{
				list.Remove(fetchChore);
			}
			chore.provider = null;
			return;
		}
		base.RemoveChore(chore);
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x001D08E8 File Offset: 0x001CEAE8
	public void UpdateFetches(PathProber path_prober)
	{
		List<FetchChore> list = null;
		int myParentWorldId = path_prober.gameObject.GetMyParentWorldId();
		if (!this.fetchMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		this.fetches.Clear();
		Navigator component = path_prober.GetComponent<Navigator>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			FetchChore fetchChore = list[i];
			if (!(fetchChore.driver != null) && (!(fetchChore.automatable != null) || !fetchChore.automatable.GetAutomationOnly()))
			{
				if (fetchChore.provider == null)
				{
					fetchChore.Cancel("no provider");
					list[i] = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
				}
				else
				{
					Storage destination = fetchChore.destination;
					if (!(destination == null))
					{
						int navigationCost = component.GetNavigationCost(destination);
						if (navigationCost != -1)
						{
							this.fetches.Add(new GlobalChoreProvider.Fetch
							{
								chore = fetchChore,
								idsHash = fetchChore.tagsHash,
								cost = navigationCost,
								priority = fetchChore.masterPriority,
								category = destination.fetchCategory
							});
						}
					}
				}
			}
		}
		if (this.fetches.Count > 0)
		{
			this.fetches.Sort(GlobalChoreProvider.Comparer);
			int j = 1;
			int num = 0;
			while (j < this.fetches.Count)
			{
				if (!this.fetches[num].IsBetterThan(this.fetches[j]))
				{
					num++;
					this.fetches[num] = this.fetches[j];
				}
				j++;
			}
			this.fetches.RemoveRange(num + 1, this.fetches.Count - num - 1);
		}
		this.clearableManager.CollectAndSortClearables(component);
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x001D0ADC File Offset: 0x001CECDC
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		base.CollectChores(consumer_state, succeeded, failed_contexts);
		this.clearableManager.CollectChores(this.fetches, consumer_state, succeeded, failed_contexts);
		if (this.fetches.Count > 48)
		{
			GlobalChoreProvider.batch_context.Setup(this, consumer_state);
			GlobalChoreProvider.batch_work_items.Reset(GlobalChoreProvider.batch_context);
			for (int i = 0; i < this.fetches.Count; i += 16)
			{
				GlobalChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>(i, Math.Min(i + 16, this.fetches.Count)));
			}
			GlobalJobManager.Run(GlobalChoreProvider.batch_work_items);
			GlobalChoreProvider.batch_context.Finish(succeeded, failed_contexts);
			return;
		}
		for (int j = 0; j < this.fetches.Count; j++)
		{
			this.fetches[j].chore.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000BB043 File Offset: 0x000B9243
	public HandleVector<int>.Handle RegisterClearable(Clearable clearable)
	{
		return this.clearableManager.RegisterClearable(clearable);
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000BB051 File Offset: 0x000B9251
	public void UnregisterClearable(HandleVector<int>.Handle handle)
	{
		this.clearableManager.UnregisterClearable(handle);
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000BB05F File Offset: 0x000B925F
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		GlobalChoreProvider.Instance = null;
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000BB06D File Offset: 0x000B926D
	public void Render200ms(float dt)
	{
		this.UpdateStorageFetchableBits();
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x001D0BB4 File Offset: 0x001CEDB4
	private void UpdateStorageFetchableBits()
	{
		ChoreType storageFetch = Db.Get().ChoreTypes.StorageFetch;
		ChoreType foodFetch = Db.Get().ChoreTypes.FoodFetch;
		this.storageFetchableTags.Clear();
		List<int> worldIDsSorted = ClusterManager.Instance.GetWorldIDsSorted();
		for (int i = 0; i < worldIDsSorted.Count; i++)
		{
			List<FetchChore> list;
			if (this.fetchMap.TryGetValue(worldIDsSorted[i], out list))
			{
				for (int j = 0; j < list.Count; j++)
				{
					FetchChore fetchChore = list[j];
					if ((fetchChore.choreType == storageFetch || fetchChore.choreType == foodFetch) && fetchChore.destination)
					{
						int cell = Grid.PosToCell(fetchChore.destination);
						if (MinionGroupProber.Get().IsReachable(cell, fetchChore.destination.GetOffsets(cell)))
						{
							this.storageFetchableTags.UnionWith(fetchChore.tags);
						}
					}
				}
			}
		}
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x001D0CA4 File Offset: 0x001CEEA4
	public bool ClearableHasDestination(Pickupable pickupable)
	{
		KPrefabID kprefabID = pickupable.KPrefabID;
		return this.storageFetchableTags.Contains(kprefabID.PrefabTag);
	}

	// Token: 0x04001742 RID: 5954
	public static GlobalChoreProvider Instance;

	// Token: 0x04001743 RID: 5955
	public Dictionary<int, List<FetchChore>> fetchMap = new Dictionary<int, List<FetchChore>>();

	// Token: 0x04001744 RID: 5956
	public List<GlobalChoreProvider.Fetch> fetches = new List<GlobalChoreProvider.Fetch>();

	// Token: 0x04001745 RID: 5957
	private static readonly GlobalChoreProvider.FetchComparer Comparer = new GlobalChoreProvider.FetchComparer();

	// Token: 0x04001746 RID: 5958
	private ClearableManager clearableManager;

	// Token: 0x04001747 RID: 5959
	private HashSet<Tag> storageFetchableTags = new HashSet<Tag>();

	// Token: 0x04001748 RID: 5960
	private static GlobalChoreProvider.GlobalChoreProviderMultithreader batch_context = new GlobalChoreProvider.GlobalChoreProviderMultithreader();

	// Token: 0x04001749 RID: 5961
	private static WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<GlobalChoreProvider>.WorkBlock<GlobalChoreProvider.GlobalChoreProviderMultithreader>, GlobalChoreProvider.GlobalChoreProviderMultithreader>();

	// Token: 0x020007A4 RID: 1956
	public struct Fetch
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x001D0CCC File Offset: 0x001CEECC
		public bool IsBetterThan(GlobalChoreProvider.Fetch fetch)
		{
			if (this.category != fetch.category)
			{
				return false;
			}
			if (this.idsHash != fetch.idsHash)
			{
				return false;
			}
			if (this.chore.choreType != fetch.chore.choreType)
			{
				return false;
			}
			if (this.priority.priority_class > fetch.priority.priority_class)
			{
				return true;
			}
			if (this.priority.priority_class == fetch.priority.priority_class)
			{
				if (this.priority.priority_value > fetch.priority.priority_value)
				{
					return true;
				}
				if (this.priority.priority_value == fetch.priority.priority_value)
				{
					return this.cost <= fetch.cost;
				}
			}
			return false;
		}

		// Token: 0x0400174A RID: 5962
		public FetchChore chore;

		// Token: 0x0400174B RID: 5963
		public int idsHash;

		// Token: 0x0400174C RID: 5964
		public int cost;

		// Token: 0x0400174D RID: 5965
		public PrioritySetting priority;

		// Token: 0x0400174E RID: 5966
		public Storage.FetchCategory category;
	}

	// Token: 0x020007A5 RID: 1957
	private class GlobalChoreProviderMultithreader : MultithreadedCollectChoreContext<GlobalChoreProvider>
	{
		// Token: 0x060022C3 RID: 8899 RVA: 0x000BB0BE File Offset: 0x000B92BE
		public override void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed)
		{
			this.provider.fetches[index].chore.CollectChoresFromGlobalChoreProvider(this.consumerState, succeed, incomplete, failed, false);
		}
	}

	// Token: 0x020007A6 RID: 1958
	private class FetchComparer : IComparer<GlobalChoreProvider.Fetch>
	{
		// Token: 0x060022C5 RID: 8901 RVA: 0x001D0D8C File Offset: 0x001CEF8C
		public int Compare(GlobalChoreProvider.Fetch a, GlobalChoreProvider.Fetch b)
		{
			int num = b.priority.priority_class - a.priority.priority_class;
			if (num != 0)
			{
				return num;
			}
			int num2 = b.priority.priority_value - a.priority.priority_value;
			if (num2 != 0)
			{
				return num2;
			}
			return a.cost - b.cost;
		}
	}

	// Token: 0x020007A7 RID: 1959
	private struct FindTopPriorityTask : IWorkItem<object>
	{
		// Token: 0x060022C7 RID: 8903 RVA: 0x000BB0EE File Offset: 0x000B92EE
		public FindTopPriorityTask(int start, int end, List<Prioritizable> worldCollection)
		{
			this.start = start;
			this.end = end;
			this.worldCollection = worldCollection;
			this.found = false;
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x001D0DE0 File Offset: 0x001CEFE0
		public void Run(object context, int threadIndex)
		{
			if (GlobalChoreProvider.FindTopPriorityTask.abort)
			{
				return;
			}
			int num = this.start;
			while (num != this.end && this.worldCollection.Count > num)
			{
				if (!(this.worldCollection[num] == null) && this.worldCollection[num].IsTopPriority())
				{
					this.found = true;
					break;
				}
				num++;
			}
			if (this.found)
			{
				GlobalChoreProvider.FindTopPriorityTask.abort = true;
			}
		}

		// Token: 0x0400174F RID: 5967
		private int start;

		// Token: 0x04001750 RID: 5968
		private int end;

		// Token: 0x04001751 RID: 5969
		private List<Prioritizable> worldCollection;

		// Token: 0x04001752 RID: 5970
		public bool found;

		// Token: 0x04001753 RID: 5971
		public static bool abort;
	}
}
