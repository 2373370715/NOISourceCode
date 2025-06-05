using System;
using UnityEngine;

// Token: 0x020020AD RID: 8365
[AddComponentMenu("KMonoBehaviour/scripts/UIScheduler")]
public class UIScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x0600B263 RID: 45667 RVA: 0x001187D1 File Offset: 0x001169D1
	public static void DestroyInstance()
	{
		UIScheduler.Instance = null;
	}

	// Token: 0x0600B264 RID: 45668 RVA: 0x001187D9 File Offset: 0x001169D9
	protected override void OnPrefabInit()
	{
		UIScheduler.Instance = this;
	}

	// Token: 0x0600B265 RID: 45669 RVA: 0x001187E1 File Offset: 0x001169E1
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x0600B266 RID: 45670 RVA: 0x001187F5 File Offset: 0x001169F5
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x0600B267 RID: 45671 RVA: 0x0011880C File Offset: 0x00116A0C
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x0600B268 RID: 45672 RVA: 0x00118819 File Offset: 0x00116A19
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x0600B269 RID: 45673 RVA: 0x0011882D File Offset: 0x00116A2D
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x0600B26A RID: 45674 RVA: 0x0011883A File Offset: 0x00116A3A
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04008CD3 RID: 36051
	private Scheduler scheduler = new Scheduler(new UIScheduler.UISchedulerClock());

	// Token: 0x04008CD4 RID: 36052
	public static UIScheduler Instance;

	// Token: 0x020020AE RID: 8366
	public class UISchedulerClock : SchedulerClock
	{
		// Token: 0x0600B26C RID: 45676 RVA: 0x0011885A File Offset: 0x00116A5A
		public override float GetTime()
		{
			return Time.unscaledTime;
		}
	}
}
