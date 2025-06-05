using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
public struct SchedulerEntry
{
	// Token: 0x1700011C RID: 284
	// (get) Token: 0x0600254E RID: 9550 RVA: 0x000BCC78 File Offset: 0x000BAE78
	// (set) Token: 0x0600254F RID: 9551 RVA: 0x000BCC80 File Offset: 0x000BAE80
	public SchedulerEntry.Details details { readonly get; private set; }

	// Token: 0x06002550 RID: 9552 RVA: 0x000BCC89 File Offset: 0x000BAE89
	public SchedulerEntry(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		this.time = time;
		this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000BCCA5 File Offset: 0x000BAEA5
	public void FreeResources()
	{
		this.details = null;
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06002552 RID: 9554 RVA: 0x000BCCAE File Offset: 0x000BAEAE
	public Action<object> callback
	{
		get
		{
			return this.details.callback;
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06002553 RID: 9555 RVA: 0x000BCCBB File Offset: 0x000BAEBB
	public object callbackData
	{
		get
		{
			return this.details.callbackData;
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06002554 RID: 9556 RVA: 0x000BCCC8 File Offset: 0x000BAEC8
	public float timeInterval
	{
		get
		{
			return this.details.timeInterval;
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000BCCD5 File Offset: 0x000BAED5
	public override string ToString()
	{
		return this.time.ToString();
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000BCCE2 File Offset: 0x000BAEE2
	public void Clear()
	{
		this.details.callback = null;
	}

	// Token: 0x040019B4 RID: 6580
	public float time;

	// Token: 0x02000841 RID: 2113
	public class Details
	{
		// Token: 0x06002557 RID: 9559 RVA: 0x000BCCF0 File Offset: 0x000BAEF0
		public Details(string name, Action<object> callback, object callback_data, float time_interval, GameObject profiler_obj)
		{
			this.timeInterval = time_interval;
			this.callback = callback;
			this.callbackData = callback_data;
		}

		// Token: 0x040019B6 RID: 6582
		public Action<object> callback;

		// Token: 0x040019B7 RID: 6583
		public object callbackData;

		// Token: 0x040019B8 RID: 6584
		public float timeInterval;
	}
}
