using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200162C RID: 5676
public class SleepChoreMonitor : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance>
{
	// Token: 0x06007576 RID: 30070 RVA: 0x00315434 File Offset: 0x00313634
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.AssignablesChanged, delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
		});
		this.satisfied.EventTransition(GameHashes.AddUrge, this.checkforbed, (SleepChoreMonitor.Instance smi) => smi.HasSleepUrge());
		this.checkforbed.Enter("SetBed", delegate(SleepChoreMonitor.Instance smi)
		{
			smi.UpdateBed();
			if (smi.GetSMI<StaminaMonitor.Instance>().NeedsToSleep())
			{
				if (this.bed.Get(smi) != null && smi.IsBedReachable())
				{
					smi.GoTo(this.passingout_bedassigned);
					return;
				}
				smi.GoTo(this.passingout);
				return;
			}
			else
			{
				if (this.bed.Get(smi) == null || !smi.IsBedReachable())
				{
					smi.GoTo(this.sleeponfloor);
					return;
				}
				smi.GoTo(this.bedassigned);
				return;
			}
		});
		this.passingout.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreatePassingOutChore), this.satisfied, this.satisfied);
		this.passingout_bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateExhaustedSleepChore), this.satisfied, this.satisfied);
		this.sleeponfloor.EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventHandlerTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi, object data) => smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepOnFloorChore), this.satisfied, this.satisfied);
		this.bedassigned.ParamTransition<GameObject>(this.bed, this.checkforbed, (SleepChoreMonitor.Instance smi, GameObject p) => p == null).EventTransition(GameHashes.AssignablesChanged, this.checkforbed, null).EventTransition(GameHashes.AssignableReachabilityChanged, this.checkforbed, (SleepChoreMonitor.Instance smi) => !smi.IsBedReachable()).ToggleChore(new Func<SleepChoreMonitor.Instance, Chore>(this.CreateSleepChore), this.satisfied, this.satisfied);
	}

	// Token: 0x06007577 RID: 30071 RVA: 0x003156C4 File Offset: 0x003138C4
	private Chore CreatePassingOutChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreatePassedOutLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, false);
	}

	// Token: 0x06007578 RID: 30072 RVA: 0x003156F8 File Offset: 0x003138F8
	private Chore CreateSleepOnFloorChore(SleepChoreMonitor.Instance smi)
	{
		GameObject gameObject = smi.CreateFloorLocator();
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, gameObject, true, true);
	}

	// Token: 0x06007579 RID: 30073 RVA: 0x000F1A8E File Offset: 0x000EFC8E
	private Chore CreateSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true);
	}

	// Token: 0x0600757A RID: 30074 RVA: 0x0031572C File Offset: 0x0031392C
	private Chore CreateExhaustedSleepChore(SleepChoreMonitor.Instance smi)
	{
		return new SleepChore(Db.Get().ChoreTypes.Sleep, smi.master, this.bed.Get(smi), false, true, new StatusItem[]
		{
			Db.Get().DuplicantStatusItems.SleepingExhausted
		});
	}

	// Token: 0x04005840 RID: 22592
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005841 RID: 22593
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State checkforbed;

	// Token: 0x04005842 RID: 22594
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout;

	// Token: 0x04005843 RID: 22595
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State passingout_bedassigned;

	// Token: 0x04005844 RID: 22596
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State sleeponfloor;

	// Token: 0x04005845 RID: 22597
	public GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.State bedassigned;

	// Token: 0x04005846 RID: 22598
	public StateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.TargetParameter bed;

	// Token: 0x0200162D RID: 5677
	public new class Instance : GameStateMachine<SleepChoreMonitor, SleepChoreMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600757D RID: 30077 RVA: 0x000F1AC0 File Offset: 0x000EFCC0
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x00315808 File Offset: 0x00313A08
		public void UpdateBed()
		{
			Ownables soleOwner = base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().GetSoleOwner();
			Assignable assignable = soleOwner.GetAssignable(Db.Get().AssignableSlots.MedicalBed);
			Assignable assignable2;
			if (assignable != null && assignable.CanAutoAssignTo(base.sm.masterTarget.Get(base.smi).GetComponent<MinionIdentity>().assignableProxy.Get()))
			{
				assignable2 = assignable;
			}
			else
			{
				assignable2 = soleOwner.GetAssignable(Db.Get().AssignableSlots.Bed);
				if (assignable2 == null)
				{
					assignable2 = soleOwner.AutoAssignSlot(Db.Get().AssignableSlots.Bed);
					if (assignable2 != null)
					{
						AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
						if (sensor.IsEnabled)
						{
							sensor.Update();
						}
					}
				}
			}
			base.smi.sm.bed.Set(assignable2, base.smi);
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x000F1AC9 File Offset: 0x000EFCC9
		public bool HasSleepUrge()
		{
			return base.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x003158FC File Offset: 0x00313AFC
		public bool IsBedReachable()
		{
			AssignableReachabilitySensor sensor = base.GetComponent<Sensors>().GetSensor<AssignableReachabilitySensor>();
			return sensor.IsReachable(Db.Get().AssignableSlots.Bed) || sensor.IsReachable(Db.Get().AssignableSlots.MedicalBed);
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x000F1AE5 File Offset: 0x000EFCE5
		public GameObject CreatePassedOutLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "PassedOutSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x06007582 RID: 30082 RVA: 0x000F1B24 File Offset: 0x000EFD24
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "FloorSleep";
			safeFloorLocator.wakeEffects = new List<string>
			{
				"SoreBack"
			};
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}

		// Token: 0x04005847 RID: 22599
		private int locatorCell;

		// Token: 0x04005848 RID: 22600
		public GameObject locator;
	}
}
