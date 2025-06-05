using System;
using System.Collections.Generic;

// Token: 0x02000928 RID: 2344
public class StateMachineManager : Singleton<StateMachineManager>, IScheduler
{
	// Token: 0x0600291D RID: 10525 RVA: 0x000BF368 File Offset: 0x000BD568
	public void RegisterScheduler(Scheduler scheduler)
	{
		this.scheduler = scheduler;
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000BF371 File Offset: 0x000BD571
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000BF385 File Offset: 0x000BD585
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000BF39C File Offset: 0x000BD59C
	public SchedulerGroup CreateSchedulerGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x001E16DC File Offset: 0x001DF8DC
	public StateMachine CreateStateMachine(Type type)
	{
		StateMachine stateMachine = null;
		if (!this.stateMachines.TryGetValue(type, out stateMachine))
		{
			stateMachine = (StateMachine)Activator.CreateInstance(type);
			stateMachine.CreateStates(stateMachine);
			stateMachine.BindStates();
			stateMachine.InitializeStateMachine();
			this.stateMachines[type] = stateMachine;
			List<Action<StateMachine>> list;
			if (this.stateMachineCreatedCBs.TryGetValue(type, out list))
			{
				foreach (Action<StateMachine> action in list)
				{
					action(stateMachine);
				}
			}
		}
		return stateMachine;
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000BF3A9 File Offset: 0x000BD5A9
	public T CreateStateMachine<T>()
	{
		return (T)((object)this.CreateStateMachine(typeof(T)));
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x001E1778 File Offset: 0x001DF978
	public static void ResetParameters()
	{
		for (int i = 0; i < StateMachineManager.parameters.Length; i++)
		{
			StateMachineManager.parameters[i] = null;
		}
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000BF3C0 File Offset: 0x000BD5C0
	public StateMachine.Instance CreateSMIFromDef(IStateMachineTarget master, StateMachine.BaseDef def)
	{
		StateMachineManager.parameters[0] = master;
		StateMachineManager.parameters[1] = def;
		return (StateMachine.Instance)Activator.CreateInstance(Singleton<StateMachineManager>.Instance.CreateStateMachine(def.GetStateMachineType()).GetStateMachineInstanceType(), StateMachineManager.parameters);
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000BF3F6 File Offset: 0x000BD5F6
	public void Clear()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		if (this.stateMachines != null)
		{
			this.stateMachines.Clear();
		}
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x001E17A0 File Offset: 0x001DF9A0
	public void AddStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (!this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list = new List<Action<StateMachine>>();
			this.stateMachineCreatedCBs[sm_type] = list;
		}
		list.Add(cb);
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x001E17D8 File Offset: 0x001DF9D8
	public void RemoveStateMachineCreatedCallback(Type sm_type, Action<StateMachine> cb)
	{
		List<Action<StateMachine>> list;
		if (this.stateMachineCreatedCBs.TryGetValue(sm_type, out list))
		{
			list.Remove(cb);
		}
	}

	// Token: 0x04001BF5 RID: 7157
	private Scheduler scheduler;

	// Token: 0x04001BF6 RID: 7158
	private Dictionary<Type, StateMachine> stateMachines = new Dictionary<Type, StateMachine>();

	// Token: 0x04001BF7 RID: 7159
	private Dictionary<Type, List<Action<StateMachine>>> stateMachineCreatedCBs = new Dictionary<Type, List<Action<StateMachine>>>();

	// Token: 0x04001BF8 RID: 7160
	private static object[] parameters = new object[2];
}
