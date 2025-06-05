using System;
using System.Collections.Generic;

// Token: 0x02000789 RID: 1929
public abstract class MultithreadedCollectChoreContext<ProviderType>
{
	// Token: 0x060021D8 RID: 8664 RVA: 0x000AA024 File Offset: 0x000A8224
	public MultithreadedCollectChoreContext()
	{
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000BA73B File Offset: 0x000B893B
	public void Setup(ProviderType provider, ChoreConsumerState consumerState)
	{
		this.provider = provider;
		this.consumerState = consumerState;
		if (this.succeeded == null || this.succeeded.Length != GlobalJobManager.ThreadCount)
		{
			this.SetupThreadContext();
		}
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x001CDDC4 File Offset: 0x001CBFC4
	private void SetupThreadContext()
	{
		if (this.succeeded != null)
		{
			this.TearDownThreadContext();
		}
		int threadCount = GlobalJobManager.ThreadCount;
		this.succeeded = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		this.failed = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		this.incomplete = new ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[threadCount];
		for (int i = 0; i < threadCount; i++)
		{
			this.succeeded[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
			this.failed[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
			this.incomplete[i] = ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.Allocate();
		}
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x001CDE3C File Offset: 0x001CC03C
	private void TearDownThreadContext()
	{
		int threadCount = GlobalJobManager.ThreadCount;
		for (int i = 0; i < threadCount; i++)
		{
			this.succeeded[i].Recycle();
			this.failed[i].Recycle();
			this.incomplete[i].Recycle();
		}
		this.succeeded = null;
		this.failed = null;
		this.incomplete = null;
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x001CDE98 File Offset: 0x001CC098
	public void Finish(List<Chore.Precondition.Context> pass, List<Chore.Precondition.Context> fail)
	{
		int threadCount = GlobalJobManager.ThreadCount;
		for (int i = 0; i < threadCount; i++)
		{
			pass.AddRange(this.succeeded[i]);
			this.succeeded[i].Clear();
			fail.AddRange(this.failed[i]);
			this.failed[i].Clear();
			foreach (Chore.Precondition.Context item in this.incomplete[i])
			{
				item.FinishPreconditions();
				if (item.IsSuccess())
				{
					pass.Add(item);
				}
				else
				{
					fail.Add(item);
				}
			}
			this.incomplete[i].Clear();
		}
	}

	// Token: 0x060021DD RID: 8669
	public abstract void CollectChore(int index, List<Chore.Precondition.Context> succeed, List<Chore.Precondition.Context> incomplete, List<Chore.Precondition.Context> failed);

	// Token: 0x060021DE RID: 8670 RVA: 0x000BA768 File Offset: 0x000B8968
	public void DefaultCollectChore(int index, int threadIndex)
	{
		this.CollectChore(index, this.succeeded[threadIndex], this.incomplete[threadIndex], this.failed[threadIndex]);
	}

	// Token: 0x040016C3 RID: 5827
	public ProviderType provider;

	// Token: 0x040016C4 RID: 5828
	public ChoreConsumerState consumerState;

	// Token: 0x040016C5 RID: 5829
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] succeeded;

	// Token: 0x040016C6 RID: 5830
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] failed;

	// Token: 0x040016C7 RID: 5831
	public ListPool<Chore.Precondition.Context, MultithreadedCollectChoreContext<ProviderType>>.PooledList[] incomplete;

	// Token: 0x0200078A RID: 1930
	public struct WorkBlock<Parent> : IWorkItem<Parent> where Parent : MultithreadedCollectChoreContext<ProviderType>
	{
		// Token: 0x060021DF RID: 8671 RVA: 0x000BA789 File Offset: 0x000B8989
		public WorkBlock(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x001CDF64 File Offset: 0x001CC164
		void IWorkItem<!1>.Run(Parent shared_data, int threadIndex)
		{
			for (int i = this.start; i < this.end; i++)
			{
				shared_data.DefaultCollectChore(i, threadIndex);
			}
		}

		// Token: 0x040016C8 RID: 5832
		private int start;

		// Token: 0x040016C9 RID: 5833
		private int end;
	}
}
