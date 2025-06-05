using System;
using UnityEngine;

// Token: 0x0200083E RID: 2110
public class Scheduler : IScheduler
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06002543 RID: 9539 RVA: 0x000BCC10 File Offset: 0x000BAE10
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000BCC1D File Offset: 0x000BAE1D
	public Scheduler(SchedulerClock clock)
	{
		this.clock = clock;
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000BCC42 File Offset: 0x000BAE42
	public float GetTime()
	{
		return this.clock.GetTime();
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000BCC4F File Offset: 0x000BAE4F
	private SchedulerHandle Schedule(SchedulerEntry entry)
	{
		this.entries.Enqueue(entry.time, entry);
		return new SchedulerHandle(this, entry);
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x001D8DA0 File Offset: 0x001D6FA0
	private SchedulerHandle Schedule(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		SchedulerEntry entry = new SchedulerEntry(name, time + this.clock.GetTime(), time_interval, callback, callback_data, profiler_obj);
		return this.Schedule(entry);
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x001D8DD0 File Offset: 0x001D6FD0
	public void FreeResources()
	{
		this.clock = null;
		if (this.entries != null)
		{
			while (this.entries.Count > 0)
			{
				this.entries.Dequeue().Value.FreeResources();
			}
		}
		this.entries = null;
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x001D8E20 File Offset: 0x001D7020
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		if (group != null && group.scheduler != this)
		{
			global::Debug.LogError("Scheduler group mismatch!");
		}
		SchedulerHandle schedulerHandle = this.Schedule(name, time, -1f, callback, callback_data, null);
		if (group != null)
		{
			group.Add(schedulerHandle);
		}
		return schedulerHandle;
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000BCC6A File Offset: 0x000BAE6A
	public void Clear(SchedulerHandle handle)
	{
		handle.entry.Clear();
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x001D8E64 File Offset: 0x001D7064
	public void Update()
	{
		if (this.Count == 0)
		{
			return;
		}
		int count = this.Count;
		int num = 0;
		using (new KProfiler.Region("Scheduler.Update", null))
		{
			float time = this.clock.GetTime();
			if (this.previousTime != time)
			{
				this.previousTime = time;
				while (num < count && time >= this.entries.Peek().Key)
				{
					SchedulerEntry value = this.entries.Dequeue().Value;
					if (value.callback != null)
					{
						value.callback(value.callbackData);
					}
					num++;
				}
			}
		}
	}

	// Token: 0x040019B1 RID: 6577
	public FloatHOTQueue<SchedulerEntry> entries = new FloatHOTQueue<SchedulerEntry>();

	// Token: 0x040019B2 RID: 6578
	private SchedulerClock clock;

	// Token: 0x040019B3 RID: 6579
	private float previousTime = float.NegativeInfinity;
}
