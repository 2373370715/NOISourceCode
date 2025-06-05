using System;

// Token: 0x0200083D RID: 2109
public interface IScheduler
{
	// Token: 0x06002542 RID: 9538
	SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null);
}
