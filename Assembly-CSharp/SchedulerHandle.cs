using System;

// Token: 0x02000843 RID: 2115
public struct SchedulerHandle
{
	// Token: 0x0600255E RID: 9566 RVA: 0x000BCD83 File Offset: 0x000BAF83
	public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
	{
		this.entry = entry;
		this.scheduler = scheduler;
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x0600255F RID: 9567 RVA: 0x000BCD93 File Offset: 0x000BAF93
	public float TimeRemaining
	{
		get
		{
			if (!this.IsValid)
			{
				return -1f;
			}
			return this.entry.time - this.scheduler.GetTime();
		}
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x000BCDBA File Offset: 0x000BAFBA
	public void FreeResources()
	{
		this.entry.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000BCDCE File Offset: 0x000BAFCE
	public void ClearScheduler()
	{
		if (this.scheduler == null)
		{
			return;
		}
		this.scheduler.Clear(this);
		this.scheduler = null;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06002562 RID: 9570 RVA: 0x000BCDF1 File Offset: 0x000BAFF1
	public bool IsValid
	{
		get
		{
			return this.scheduler != null;
		}
	}

	// Token: 0x040019BB RID: 6587
	public SchedulerEntry entry;

	// Token: 0x040019BC RID: 6588
	private Scheduler scheduler;
}
