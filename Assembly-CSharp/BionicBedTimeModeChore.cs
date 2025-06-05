using System;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class BionicBedTimeModeChore : Chore<BionicBedTimeModeChore.Instance>
{
	// Token: 0x06001DB7 RID: 7607 RVA: 0x001BC37C File Offset: 0x001BA57C
	public BionicBedTimeModeChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BionicBedtimeMode, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicBedTimeModeChore.Instance(this, master.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x06001DB8 RID: 7608 RVA: 0x001BC3D4 File Offset: 0x001BA5D4
	public static void BeginWorkOnZone(BionicBedTimeModeChore.Instance smi)
	{
		WorkerBase workerBase = smi.sm.bionic.Get<WorkerBase>(smi);
		DefragmentationZone assignedDefragmentationZone = smi.GetAssignedDefragmentationZone();
		workerBase.StartWork(new WorkerBase.StartWorkInfo(assignedDefragmentationZone));
	}

	// Token: 0x06001DB9 RID: 7609 RVA: 0x000B8150 File Offset: 0x000B6350
	public static bool HasDefragmentationZoneAssignedAndReachable(BionicBedTimeModeChore.Instance smi, GameObject defragmentationZone)
	{
		return defragmentationZone != null && smi.IsDefragmentationZoneReachable();
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x000B8163 File Offset: 0x000B6363
	public static bool HasDefragmentationZoneAssignedAndReachable(BionicBedTimeModeChore.Instance smi)
	{
		return smi.sm.defragmentationZone.Get(smi) != null && smi.IsDefragmentationZoneReachable();
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x000B8186 File Offset: 0x000B6386
	public static bool IsBedTimeAllowed(BionicBedTimeModeChore.Instance smi)
	{
		return BionicBedTimeMonitor.CanGoToBedTime(smi.bedTimeMonitor);
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x000B8193 File Offset: 0x000B6393
	public static void UpdateAssignedDefragmentationZone(BionicBedTimeModeChore.Instance smi)
	{
		smi.UpdateAssignedDefragmentationZone(null);
	}

	// Token: 0x040012F0 RID: 4848
	public const string EFFECT_NAME = "BionicBedTimeEffect";

	// Token: 0x02000688 RID: 1672
	public class States : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore>
	{
		// Token: 0x06001DBD RID: 7613 RVA: 0x001BC404 File Offset: 0x001BA604
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.enter;
			this.root.ToggleEffect("BionicBedTimeEffect");
			this.enter.Transition(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed)), UpdateRate.SIM_200ms).ParamTransition<GameObject>(this.defragmentationZone, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).GoTo(this.defragmentingWithoutAssignable);
			this.unassigning.ScheduleActionNextFrame("Frame delay on unassign", delegate(BionicBedTimeModeChore.Instance smi)
			{
				BionicBedTimeModeChore.UpdateAssignedDefragmentationZone(smi);
				smi.GoTo(this.enter);
			});
			this.approach.InitializeStates(this.bionic, this.defragmentationZone, this.defragmentingOnAssignable, null, null, null).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).ScheduleChange(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, null, null);
			this.defragmentingOnAssignable.OnTargetLost(this.defragmentationZone, this.defragmentingWithoutAssignable).OnSignal(this.defragmentationZoneChangedSignal, this.enter).OnSignal(this.defragmentationZoneUnassignined, this.unassigning).EventTransition(GameHashes.BionicOffline, null, null).ScheduleChange(null, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).ToggleWork("Defragmenting", new Action<BionicBedTimeModeChore.Instance>(BionicBedTimeModeChore.BeginWorkOnZone), (BionicBedTimeModeChore.Instance smi) => smi.GetAssignedDefragmentationZone() != null, this.end, null).ToggleTag(GameTags.BionicBedTime);
			this.defragmentingWithoutAssignable.ParamTransition<GameObject>(this.defragmentationZone, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Parameter<GameObject>.Callback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).EventTransition(GameHashes.AssignableReachabilityChanged, this.approach, new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.HasDefragmentationZoneAssignedAndReachable)).ToggleAnims("anim_bionic_kanim", 0f).ToggleTag(GameTags.BionicBedTime).DefaultState(this.defragmentingWithoutAssignable.pre);
			this.defragmentingWithoutAssignable.pre.PlayAnim("low_power_pre").OnAnimQueueComplete(this.defragmentingWithoutAssignable.loop).ScheduleGoTo(1.5f, this.defragmentingWithoutAssignable.loop);
			this.defragmentingWithoutAssignable.loop.ScheduleChange(this.defragmentingWithoutAssignable.pst, GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Not(new StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Transition.ConditionCallback(BionicBedTimeModeChore.IsBedTimeAllowed))).EventTransition(GameHashes.BionicOffline, this.defragmentingWithoutAssignable.pst, null).PlayAnim("low_power_loop", KAnim.PlayMode.Loop);
			this.defragmentingWithoutAssignable.pst.PlayAnim("low_power_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.end);
			this.end.ReturnSuccess();
		}

		// Token: 0x040012F1 RID: 4849
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040012F2 RID: 4850
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State defragmentingOnAssignable;

		// Token: 0x040012F3 RID: 4851
		public BionicBedTimeModeChore.States.DefragmentingStates defragmentingWithoutAssignable;

		// Token: 0x040012F4 RID: 4852
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State enter;

		// Token: 0x040012F5 RID: 4853
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State end;

		// Token: 0x040012F6 RID: 4854
		public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State unassigning;

		// Token: 0x040012F7 RID: 4855
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter bionic;

		// Token: 0x040012F8 RID: 4856
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.TargetParameter defragmentationZone;

		// Token: 0x040012F9 RID: 4857
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneChangedSignal;

		// Token: 0x040012FA RID: 4858
		public StateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.Signal defragmentationZoneUnassignined;

		// Token: 0x02000689 RID: 1673
		public class DefragmentingStates : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State
		{
			// Token: 0x040012FB RID: 4859
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pre;

			// Token: 0x040012FC RID: 4860
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State loop;

			// Token: 0x040012FD RID: 4861
			public GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.State pst;
		}
	}

	// Token: 0x0200068B RID: 1675
	public class Instance : GameStateMachine<BionicBedTimeModeChore.States, BionicBedTimeModeChore.Instance, BionicBedTimeModeChore, object>.GameInstance
	{
		// Token: 0x06001DC4 RID: 7620 RVA: 0x000B81DA File Offset: 0x000B63DA
		public DefragmentationZone GetAssignedDefragmentationZone()
		{
			return this.lastAssignedDefragmentationZone;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x001BC6A8 File Offset: 0x001BA8A8
		public Instance(BionicBedTimeModeChore master, GameObject duplicant) : base(master)
		{
			this.bedTimeMonitor = duplicant.GetSMI<BionicBedTimeMonitor.Instance>();
			base.sm.bionic.Set(duplicant, this, false);
			this.ownables = base.GetComponent<MinionIdentity>().GetSoleOwner();
			base.gameObject.Subscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
			this.UpdateAssignedDefragmentationZone(null);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000B81E2 File Offset: 0x000B63E2
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1585839766, new Action<object>(this.UpdateAssignedDefragmentationZone));
			base.OnCleanUp();
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x000B8206 File Offset: 0x000B6406
		public override void StartSM()
		{
			this.UpdateAssignedDefragmentationZone(null);
			base.StartSM();
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B8215 File Offset: 0x000B6415
		public bool IsDefragmentationZoneReachable()
		{
			return base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>().IsReachable(Db.Get().AssignableSlots.Bed);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x001BC714 File Offset: 0x001BA914
		public void UpdateAssignedDefragmentationZone(object slotInstanceObject)
		{
			DefragmentationZone y = null;
			AssignableSlotInstance assignableSlotInstance = (slotInstanceObject == null) ? null : ((AssignableSlotInstance)slotInstanceObject);
			Assignable assignable = this.ownables.GetAssignable(Db.Get().AssignableSlots.Bed);
			if (assignableSlotInstance != null && assignableSlotInstance.IsUnassigning())
			{
				base.sm.defragmentationZoneUnassignined.Trigger(this);
				return;
			}
			if (assignable == null)
			{
				assignable = this.ownables.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
			}
			if (assignable != null)
			{
				y = assignable.GetComponent<DefragmentationZone>();
			}
			if (this.lastAssignedDefragmentationZone != y)
			{
				AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
				if (sensor.IsEnabled)
				{
					sensor.Update();
				}
				this.lastAssignedDefragmentationZone = y;
				base.sm.defragmentationZone.Set(this.lastAssignedDefragmentationZone, this);
				base.sm.defragmentationZoneChangedSignal.Trigger(this);
			}
		}

		// Token: 0x04001300 RID: 4864
		public BionicBedTimeMonitor.Instance bedTimeMonitor;

		// Token: 0x04001301 RID: 4865
		private DefragmentationZone lastAssignedDefragmentationZone;

		// Token: 0x04001302 RID: 4866
		private Ownables ownables;
	}
}
