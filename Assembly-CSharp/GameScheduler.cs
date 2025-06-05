using System;
using UnityEngine;

// Token: 0x0200083B RID: 2107
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x06002537 RID: 9527 RVA: 0x000BCB63 File Offset: 0x000BAD63
	public static void DestroyInstance()
	{
		GameScheduler.Instance = null;
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000BCB6B File Offset: 0x000BAD6B
	protected override void OnPrefabInit()
	{
		GameScheduler.Instance = this;
		Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000BCB83 File Offset: 0x000BAD83
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000BCB97 File Offset: 0x000BAD97
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000BCBAE File Offset: 0x000BADAE
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000BCBBB File Offset: 0x000BADBB
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000BCBCF File Offset: 0x000BADCF
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000BCBDC File Offset: 0x000BADDC
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x040019AF RID: 6575
	private Scheduler scheduler = new Scheduler(new GameScheduler.GameSchedulerClock());

	// Token: 0x040019B0 RID: 6576
	public static GameScheduler Instance;

	// Token: 0x0200083C RID: 2108
	public class GameSchedulerClock : SchedulerClock
	{
		// Token: 0x06002540 RID: 9536 RVA: 0x000BCBFC File Offset: 0x000BADFC
		public override float GetTime()
		{
			return GameClock.Instance.GetTime();
		}
	}
}
