using System;

// Token: 0x02001655 RID: 5717
public class TaskAvailabilityMonitor : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance>
{
	// Token: 0x0600762F RID: 30255 RVA: 0x003178F8 File Offset: 0x00315AF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.NewDay, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance, this.unavailable, (TaskAvailabilityMonitor.Instance smi) => GameClock.Instance.GetCycle() > 0);
		this.unavailable.Enter("RefreshStatusItem", delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).EventHandler(GameHashes.ScheduleChanged, delegate(TaskAvailabilityMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		});
	}

	// Token: 0x040058E0 RID: 22752
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040058E1 RID: 22753
	public GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.State unavailable;

	// Token: 0x02001656 RID: 5718
	public new class Instance : GameStateMachine<TaskAvailabilityMonitor, TaskAvailabilityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007631 RID: 30257 RVA: 0x000F2354 File Offset: 0x000F0554
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007632 RID: 30258 RVA: 0x003179BC File Offset: 0x00315BBC
		public void RefreshStatusItem()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			WorldContainer myWorld = base.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsModuleInterior && myWorld.ParentWorldId == myWorld.id)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.IdleInRockets, null);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Idle, null);
		}
	}
}
