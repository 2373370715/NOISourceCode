using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000798 RID: 1944
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreProvider")]
public class ChoreProvider : KMonoBehaviour
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06002282 RID: 8834 RVA: 0x000BAEDE File Offset: 0x000B90DE
	// (set) Token: 0x06002283 RID: 8835 RVA: 0x000BAEE6 File Offset: 0x000B90E6
	public string Name { get; private set; }

	// Token: 0x06002284 RID: 8836 RVA: 0x001CFADC File Offset: 0x001CDCDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Subscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Subscribe(1142724171, new Action<object>(this.OnEntityMigrated));
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000BAEEF File Offset: 0x000B90EF
	protected override void OnSpawn()
	{
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
		base.OnSpawn();
		this.Name = base.name;
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x001CFB48 File Offset: 0x001CDD48
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		Game.Instance.Unsubscribe(586301400, new Action<object>(this.OnMinionMigrated));
		Game.Instance.Unsubscribe(1142724171, new Action<object>(this.OnEntityMigrated));
		if (ClusterManager.Instance != null)
		{
			ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		}
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x001CFBD8 File Offset: 0x001CDDD8
	protected virtual void OnWorldRemoved(object data)
	{
		int num = (int)data;
		int parentWorldId = ClusterManager.Instance.GetWorld(num).ParentWorldId;
		List<Chore> chores;
		if (this.choreWorldMap.TryGetValue(parentWorldId, out chores))
		{
			this.ClearWorldChores<Chore>(chores, num);
		}
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x001CFC18 File Offset: 0x001CDE18
	protected virtual void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		List<Chore> oldChores;
		if (worldParentChangedEventArgs == null || worldParentChangedEventArgs.lastParentId == 255 || worldParentChangedEventArgs.lastParentId == worldParentChangedEventArgs.world.ParentWorldId || !this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.lastParentId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(worldParentChangedEventArgs.world.ParentWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[worldParentChangedEventArgs.world.ParentWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, worldParentChangedEventArgs.world.ParentWorldId);
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x001CFCC0 File Offset: 0x001CDEC0
	protected virtual void OnEntityMigrated(object data)
	{
		MigrationEventArgs migrationEventArgs = data as MigrationEventArgs;
		List<Chore> oldChores;
		if (migrationEventArgs == null || !(migrationEventArgs.entity == base.gameObject) || migrationEventArgs.prevWorldId == migrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(migrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(migrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[migrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, migrationEventArgs.targetWorldId);
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x001CFD54 File Offset: 0x001CDF54
	protected virtual void OnMinionMigrated(object data)
	{
		MinionMigrationEventArgs minionMigrationEventArgs = data as MinionMigrationEventArgs;
		List<Chore> oldChores;
		if (minionMigrationEventArgs == null || !(minionMigrationEventArgs.minionId.gameObject == base.gameObject) || minionMigrationEventArgs.prevWorldId == minionMigrationEventArgs.targetWorldId || !this.choreWorldMap.TryGetValue(minionMigrationEventArgs.prevWorldId, out oldChores))
		{
			return;
		}
		List<Chore> newChores;
		if (!this.choreWorldMap.TryGetValue(minionMigrationEventArgs.targetWorldId, out newChores))
		{
			newChores = (this.choreWorldMap[minionMigrationEventArgs.targetWorldId] = new List<Chore>());
		}
		this.TransferChores<Chore>(oldChores, newChores, minionMigrationEventArgs.targetWorldId);
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x001CFDF0 File Offset: 0x001CDFF0
	protected void TransferChores<T>(List<T> oldChores, List<T> newChores, int transferId) where T : Chore
	{
		int num = oldChores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			T t = oldChores[i];
			if (t.isNull)
			{
				DebugUtil.DevLogError(string.Concat(new string[]
				{
					"[",
					t.GetType().Name,
					"] ",
					t.GetReportName(null),
					" has no target"
				}));
			}
			else if (t.gameObject.GetMyParentWorldId() == transferId)
			{
				newChores.Add(t);
				oldChores[i] = oldChores[num];
				oldChores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x001CFEAC File Offset: 0x001CE0AC
	protected void ClearWorldChores<T>(List<T> chores, int worldId) where T : Chore
	{
		int num = chores.Count - 1;
		for (int i = num; i >= 0; i--)
		{
			if (chores[i].gameObject.GetMyWorldId() == worldId)
			{
				chores[i] = chores[num];
				chores.RemoveAt(num--);
			}
		}
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x001CFF00 File Offset: 0x001CE100
	public virtual void AddChore(Chore chore)
	{
		chore.provider = this;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list = (this.choreWorldMap[myParentWorldId] = new List<Chore>());
		}
		list.Add(chore);
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x001CFF4C File Offset: 0x001CE14C
	public virtual void RemoveChore(Chore chore)
	{
		if (chore == null)
		{
			return;
		}
		chore.provider = null;
		List<Chore> list = null;
		int myParentWorldId = chore.gameObject.GetMyParentWorldId();
		if (this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			list.Remove(chore);
		}
	}

	// Token: 0x0600228F RID: 8847 RVA: 0x001CFF8C File Offset: 0x001CE18C
	public virtual void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
		List<Chore> list = null;
		int myParentWorldId = consumer_state.gameObject.GetMyParentWorldId();
		if (!this.choreWorldMap.TryGetValue(myParentWorldId, out list))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i].provider == null)
			{
				list[i].Cancel("no provider");
				list[i] = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
			}
		}
		int num = 48;
		if (list.Count > num)
		{
			ChoreProvider.batch_context.Setup(list, consumer_state);
			ChoreProvider.batch_work_items.Reset(ChoreProvider.batch_context);
			for (int j = 0; j < list.Count; j += 16)
			{
				ChoreProvider.batch_work_items.Add(new MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>(j, Math.Min(j + 16, list.Count)));
			}
			GlobalJobManager.Run(ChoreProvider.batch_work_items);
			ChoreProvider.batch_context.Finish(succeeded, failed_contexts);
			return;
		}
		foreach (Chore chore in list)
		{
			chore.CollectChores(consumer_state, succeeded, failed_contexts, false);
		}
	}

	// Token: 0x0400172B RID: 5931
	public Dictionary<int, List<Chore>> choreWorldMap = new Dictionary<int, List<Chore>>();

	// Token: 0x0400172C RID: 5932
	private static ChoreProvider.ChoreProviderCollectContext batch_context = new ChoreProvider.ChoreProviderCollectContext();

	// Token: 0x0400172D RID: 5933
	private static WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext> batch_work_items = new WorkItemCollection<MultithreadedCollectChoreContext<List<Chore>>.WorkBlock<ChoreProvider.ChoreProviderCollectContext>, ChoreProvider.ChoreProviderCollectContext>();

	// Token: 0x02000799 RID: 1945
	private class ChoreProviderCollectContext : MultithreadedCollectChoreContext<List<Chore>>
	{
		// Token: 0x06002292 RID: 8850 RVA: 0x000BAF56 File Offset: 0x000B9156
		public override void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed)
		{
			this.provider[index].CollectChores(this.consumerState, succeed, incomplete, failed, false);
		}
	}
}
