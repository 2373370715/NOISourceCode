using System;
using System.Collections.Generic;

// Token: 0x02000842 RID: 2114
public class SchedulerGroup
{
	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06002558 RID: 9560 RVA: 0x000BCD0E File Offset: 0x000BAF0E
	// (set) Token: 0x06002559 RID: 9561 RVA: 0x000BCD16 File Offset: 0x000BAF16
	public Scheduler scheduler { get; private set; }

	// Token: 0x0600255A RID: 9562 RVA: 0x000BCD1F File Offset: 0x000BAF1F
	public SchedulerGroup(Scheduler scheduler)
	{
		this.scheduler = scheduler;
		this.Reset();
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x000BCD3F File Offset: 0x000BAF3F
	public void FreeResources()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		this.scheduler = null;
		if (this.handles != null)
		{
			this.handles.Clear();
		}
		this.handles = null;
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x001D8F24 File Offset: 0x001D7124
	public void Reset()
	{
		foreach (SchedulerHandle schedulerHandle in this.handles)
		{
			schedulerHandle.ClearScheduler();
		}
		this.handles.Clear();
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x000BCD75 File Offset: 0x000BAF75
	public void Add(SchedulerHandle handle)
	{
		this.handles.Add(handle);
	}

	// Token: 0x040019BA RID: 6586
	private List<SchedulerHandle> handles = new List<SchedulerHandle>();
}
