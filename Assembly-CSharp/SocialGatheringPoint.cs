using System;
using KSerialization;
using UnityEngine;

// Token: 0x020018DF RID: 6367
[SerializationConfig(MemberSerialization.OptIn)]
public class SocialGatheringPoint : StateMachineComponent<SocialGatheringPoint.StatesInstance>
{
	// Token: 0x060083BA RID: 33722 RVA: 0x0034FA60 File Offset: 0x0034DC60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workables = new SocialGatheringPointWorkable[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("SocialGatheringPointWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.basePriority = this.basePriority;
			socialGatheringPointWorkable.specificEffect = this.socialEffect;
			socialGatheringPointWorkable.OnWorkableEventCB = new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent);
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.tracker = new SocialChoreTracker(base.gameObject, this.choreOffsets);
		this.tracker.choreCount = this.choreCount;
		this.tracker.CreateChoreCB = new Func<int, Chore>(this.CreateChore);
		base.smi.StartSM();
		Components.SocialGatheringPoints.Add((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
	}

	// Token: 0x060083BB RID: 33723 RVA: 0x0034FB6C File Offset: 0x0034DD6C
	protected override void OnCleanUp()
	{
		if (this.tracker != null)
		{
			this.tracker.Clear();
			this.tracker = null;
		}
		if (this.workables != null)
		{
			for (int i = 0; i < this.workables.Length; i++)
			{
				if (this.workables[i])
				{
					Util.KDestroyGameObject(this.workables[i]);
					this.workables[i] = null;
				}
			}
		}
		Components.SocialGatheringPoints.Remove((int)Grid.WorldIdx[Grid.PosToCell(this)], this);
		base.OnCleanUp();
	}

	// Token: 0x060083BC RID: 33724 RVA: 0x0034FBF0 File Offset: 0x0034DDF0
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<SocialGatheringPointWorkable> workChore = new WorkChore<SocialGatheringPointWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x060083BD RID: 33725 RVA: 0x000FB26C File Offset: 0x000F946C
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.smi.IsInsideState(base.smi.sm.on))
		{
			this.tracker.Update(true);
		}
	}

	// Token: 0x060083BE RID: 33726 RVA: 0x000FB297 File Offset: 0x000F9497
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			if (this.OnSocializeBeginCB != null)
			{
				this.OnSocializeBeginCB();
				return;
			}
		}
		else if (workable_event == Workable.WorkableEvent.WorkStopped && this.OnSocializeEndCB != null)
		{
			this.OnSocializeEndCB();
		}
	}

	// Token: 0x0400644D RID: 25677
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x0400644E RID: 25678
	public int choreCount = 2;

	// Token: 0x0400644F RID: 25679
	public int basePriority;

	// Token: 0x04006450 RID: 25680
	public string socialEffect;

	// Token: 0x04006451 RID: 25681
	public float workTime = 15f;

	// Token: 0x04006452 RID: 25682
	public System.Action OnSocializeBeginCB;

	// Token: 0x04006453 RID: 25683
	public System.Action OnSocializeEndCB;

	// Token: 0x04006454 RID: 25684
	private SocialChoreTracker tracker;

	// Token: 0x04006455 RID: 25685
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x020018E0 RID: 6368
	public class States : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint>
	{
		// Token: 0x060083C0 RID: 33728 RVA: 0x0034FCCC File Offset: 0x0034DECC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.TagTransition(GameTags.Operational, this.on, false);
			this.on.TagTransition(GameTags.Operational, this.off, true).Enter("CreateChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(true);
			}).Exit("CancelChore", delegate(SocialGatheringPoint.StatesInstance smi)
			{
				smi.master.tracker.Update(false);
			});
		}

		// Token: 0x04006456 RID: 25686
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State off;

		// Token: 0x04006457 RID: 25687
		public GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.State on;
	}

	// Token: 0x020018E2 RID: 6370
	public class StatesInstance : GameStateMachine<SocialGatheringPoint.States, SocialGatheringPoint.StatesInstance, SocialGatheringPoint, object>.GameInstance
	{
		// Token: 0x060083C6 RID: 33734 RVA: 0x000FB301 File Offset: 0x000F9501
		public StatesInstance(SocialGatheringPoint smi) : base(smi)
		{
		}
	}
}
