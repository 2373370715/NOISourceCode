using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02001793 RID: 6035
public class RemoteChore : WorkChore<RemoteWorkTerminal>
{
	// Token: 0x06007C23 RID: 31779 RVA: 0x0032CE90 File Offset: 0x0032B090
	public RemoteChore(RemoteWorkTerminal terminal) : base(Db.Get().ChoreTypes.RemoteOperate, terminal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
	{
		this.terminal = terminal;
		this.AddPrecondition(RemoteChore.RemoteTerminalHasDock, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockHasWorker, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockAvailable, terminal);
		this.AddPrecondition(RemoteChore.RemoteChoreSubchorePreconditions, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockOperational, terminal);
	}

	// Token: 0x06007C24 RID: 31780 RVA: 0x0032CF08 File Offset: 0x0032B108
	public override void CollectChores(ChoreConsumerState duplicantState, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context context = new Chore.Precondition.Context(this, duplicantState, is_attempting_override, null);
		context.RunPreconditions();
		if (!context.IsComplete())
		{
			ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList pooledList = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
			ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList pooledList2 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
			ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList pooledList3 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
			RemoteWorkerDock currentDock = this.terminal.CurrentDock;
			if (currentDock != null)
			{
				currentDock.CollectChores(duplicantState, pooledList, pooledList3, pooledList2, is_attempting_override);
			}
			foreach (Chore.Precondition.Context context2 in pooledList)
			{
				context.data = context2;
				context.SetPriority(context2.chore);
				incomplete_contexts.Add(context);
			}
			foreach (Chore.Precondition.Context context3 in pooledList3)
			{
				context.data = context3;
				context.SetPriority(context3.chore);
				incomplete_contexts.Add(context);
			}
			List<Chore.PreconditionInstance> preconditions = context.chore.GetPreconditions();
			context.failedPreconditionId = 0;
			while (context.failedPreconditionId < preconditions.Count && !(preconditions[context.failedPreconditionId].condition.id == RemoteChore.RemoteChoreSubchorePreconditions.id))
			{
				context.failedPreconditionId++;
			}
			foreach (Chore.Precondition.Context context4 in pooledList2)
			{
				context.data = context4;
				context.SetPriority(context4.chore);
				failed_contexts.Add(context);
			}
			pooledList.Recycle();
			pooledList2.Recycle();
			pooledList3.Recycle();
			return;
		}
		if (context.IsSuccess())
		{
			ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList pooledList4 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
			ListPool<Chore.Precondition.Context, Chore.Precondition>.PooledList pooledList5 = ListPool<Chore.Precondition.Context, Chore.Precondition>.Allocate();
			RemoteWorkerDock currentDock2 = this.terminal.CurrentDock;
			if (currentDock2 != null)
			{
				currentDock2.CollectChores(duplicantState, pooledList4, null, pooledList5, is_attempting_override);
			}
			foreach (Chore.Precondition.Context context5 in pooledList4)
			{
				context.data = context5;
				context.SetPriority(context5.chore);
				succeeded_contexts.Add(context);
			}
			foreach (Chore.Precondition.Context context6 in pooledList5)
			{
				context.data = context6;
				context.SetPriority(context6.chore);
				failed_contexts.Add(context);
			}
			pooledList4.Recycle();
			pooledList5.Recycle();
			return;
		}
		failed_contexts.Add(context);
	}

	// Token: 0x06007C25 RID: 31781 RVA: 0x0032D1F0 File Offset: 0x0032B3F0
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		base.PrepareChore(ref context);
		DebugUtil.Assert(this.active_subchore == null);
		this.active_subchore = ((Chore.Precondition.Context)context.data).chore;
		RemoteWorkerDock currentDock = this.terminal.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.SetNextChore(this.terminal, (Chore.Precondition.Context)context.data);
	}

	// Token: 0x06007C26 RID: 31782 RVA: 0x0032D250 File Offset: 0x0032B450
	protected override void End(string reason)
	{
		if (this.active_subchore != null && this.active_subchore.driver != null && !this.active_subchore.driver.HasChore())
		{
			this.active_subchore.Reserve(null);
		}
		this.active_subchore = null;
		base.End(reason);
		if (this.terminal.worker != null)
		{
			this.terminal.StopWork(this.terminal.worker, true);
		}
	}

	// Token: 0x04005D8E RID: 23950
	private static Chore.Precondition RemoteTerminalHasDock = new Chore.Precondition
	{
		id = "RemoteDockAssigned",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_DOCK,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((RemoteWorkTerminal)data).CurrentDock != null;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005D8F RID: 23951
	private static Chore.Precondition RemoteDockOperational = new Chore.Precondition
	{
		id = "RemoteDockOperational",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_INOPERABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkTerminal remoteWorkTerminal = (RemoteWorkTerminal)data;
			return remoteWorkTerminal.CurrentDock != null && remoteWorkTerminal.CurrentDock.IsOperational;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005D90 RID: 23952
	private static Chore.Precondition RemoteDockHasWorker = new Chore.Precondition
	{
		id = "RemoteDockHasAvailableWorker",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_WORKER,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkerDock currentDock = ((RemoteWorkTerminal)data).CurrentDock;
			return !(currentDock == null) && (currentDock.HasWorker() && currentDock.RemoteWorker.Available) && !currentDock.RemoteWorker.RequiresMaintnence;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005D91 RID: 23953
	private static Chore.Precondition RemoteDockAvailable = new Chore.Precondition
	{
		id = "RemoteDockAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_UNAVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkTerminal remoteWorkTerminal = (RemoteWorkTerminal)data;
			RemoteWorkerDock currentDock = remoteWorkTerminal.CurrentDock;
			return !(currentDock == null) && currentDock.AvailableForWorkBy(remoteWorkTerminal);
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005D92 RID: 23954
	private static Chore.Precondition RemoteChoreSubchorePreconditions = new Chore.Precondition
	{
		id = "RemoteChorePreconditionsMet",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_SUBCHORE_PRECONDITIONS,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.data == null)
			{
				return true;
			}
			Chore.Precondition.Context context2 = (Chore.Precondition.Context)context.data;
			if (context2.failedPreconditionId != -1)
			{
				return false;
			}
			context2.RunPreconditions();
			return context2.failedPreconditionId == -1;
		},
		canExecuteOnAnyThread = false
	};

	// Token: 0x04005D93 RID: 23955
	private RemoteWorkTerminal terminal;

	// Token: 0x04005D94 RID: 23956
	private Chore active_subchore;
}
