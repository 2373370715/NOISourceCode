using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200180E RID: 6158
public class ReusableTrap : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>
{
	// Token: 0x06007EC4 RID: 32452 RVA: 0x00339C74 File Offset: 0x00337E74
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.operational;
		this.noOperational.TagTransition(GameTags.Operational, this.operational, false).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).DefaultState(this.noOperational.idle);
		this.noOperational.idle.EnterTransition(this.noOperational.releasing, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ParamTransition<bool>(this.IsArmed, this.noOperational.disarming, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).PlayAnim("off");
		this.noOperational.releasing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.Release)).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.noOperational.idle);
		this.noOperational.disarming.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).PlayAnim("abort_armed").OnAnimQueueComplete(this.noOperational.idle);
		this.operational.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).TagTransition(GameTags.Operational, this.noOperational, true).DefaultState(this.operational.unarmed);
		this.operational.unarmed.ParamTransition<bool>(this.IsArmed, this.operational.armed, GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.IsTrue).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapNeedsArming, null).PlayAnim("unarmed").Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.StartArmTrapWorkChore)).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.CancelArmTrapWorkChore)).WorkableCompleteTransition(new Func<ReusableTrap.Instance, Workable>(ReusableTrap.GetWorkable), this.operational.armed);
		this.operational.armed.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsArmed)).EnterTransition(this.operational.capture.idle, new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.Transition.ConditionCallback(ReusableTrap.StorageContainsCritter)).PlayAnim("armed", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapArmed, null).Toggle("Enable/Disable Trap Trigger", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.EnableTrapTrigger), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Toggle("Enable/Disable Lure", new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.ActivateLure), new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableLure)).EventHandlerTransition(GameHashes.TrapTriggered, this.operational.capture.capturing, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.HasCritter_OnTrapTriggered));
		this.operational.capture.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.DisableTrapTrigger)).Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.MarkAsUnarmed)).ToggleTag(GameTags.Trapped).DefaultState(this.operational.capture.capturing).EventHandlerTransition(GameHashes.OnStorageChange, this.operational.capture.release, new Func<ReusableTrap.Instance, object, bool>(ReusableTrap.OnStorageEmptied));
		this.operational.capture.capturing.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.SetupCapturingAnimations)).Update(new Action<ReusableTrap.Instance, float>(ReusableTrap.OptionalCapturingAnimationUpdate), UpdateRate.RENDER_EVERY_TICK, false).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetCaptureAnimationName), KAnim.PlayMode.Once).OnAnimQueueComplete(this.operational.capture.idle).Exit(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.UnsetCapturingAnimations));
		this.operational.capture.idle.TriggerOnEnter(GameHashes.TrapCaptureCompleted, null).ToggleStatusItem(Db.Get().BuildingStatusItems.TrapHasCritter, (ReusableTrap.Instance smi) => smi.CapturedCritter).PlayAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetIdleAnimationName), KAnim.PlayMode.Once);
		this.operational.capture.release.Enter(new StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State.Callback(ReusableTrap.RefreshLogicOutput)).QueueAnim(new Func<ReusableTrap.Instance, string>(ReusableTrap.GetReleaseAnimationName), false, null).OnAnimQueueComplete(this.operational.unarmed);
	}

	// Token: 0x06007EC5 RID: 32453 RVA: 0x000F7EC8 File Offset: 0x000F60C8
	public static void RefreshLogicOutput(ReusableTrap.Instance smi)
	{
		smi.RefreshLogicOutput();
	}

	// Token: 0x06007EC6 RID: 32454 RVA: 0x000F7ED0 File Offset: 0x000F60D0
	public static void Release(ReusableTrap.Instance smi)
	{
		smi.Release();
	}

	// Token: 0x06007EC7 RID: 32455 RVA: 0x000F7ED8 File Offset: 0x000F60D8
	public static void StartArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06007EC8 RID: 32456 RVA: 0x000F7EE0 File Offset: 0x000F60E0
	public static void CancelArmTrapWorkChore(ReusableTrap.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06007EC9 RID: 32457 RVA: 0x000F7EE8 File Offset: 0x000F60E8
	public static string GetIdleAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture_idle";
		}
		return "capture_idle_large";
	}

	// Token: 0x06007ECA RID: 32458 RVA: 0x000F7EFD File Offset: 0x000F60FD
	public static string GetCaptureAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.IsCapturingLargeCritter)
		{
			return "capture";
		}
		return "capture_large";
	}

	// Token: 0x06007ECB RID: 32459 RVA: 0x000F7F12 File Offset: 0x000F6112
	public static string GetReleaseAnimationName(ReusableTrap.Instance smi)
	{
		if (!smi.WasLastCritterLarge)
		{
			return "release";
		}
		return "release_large";
	}

	// Token: 0x06007ECC RID: 32460 RVA: 0x000F7F27 File Offset: 0x000F6127
	public static bool OnStorageEmptied(ReusableTrap.Instance smi, object obj)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06007ECD RID: 32461 RVA: 0x000F7F32 File Offset: 0x000F6132
	public static bool HasCritter_OnTrapTriggered(ReusableTrap.Instance smi, object capturedItem)
	{
		return smi.HasCritter;
	}

	// Token: 0x06007ECE RID: 32462 RVA: 0x000F7F32 File Offset: 0x000F6132
	public static bool StorageContainsCritter(ReusableTrap.Instance smi)
	{
		return smi.HasCritter;
	}

	// Token: 0x06007ECF RID: 32463 RVA: 0x000F7F27 File Offset: 0x000F6127
	public static bool StorageIsEmpty(ReusableTrap.Instance smi)
	{
		return !smi.HasCritter;
	}

	// Token: 0x06007ED0 RID: 32464 RVA: 0x000F7F3A File Offset: 0x000F613A
	public static void EnableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(true);
	}

	// Token: 0x06007ED1 RID: 32465 RVA: 0x000F7F43 File Offset: 0x000F6143
	public static void DisableTrapTrigger(ReusableTrap.Instance smi)
	{
		smi.SetTrapTriggerActiveState(false);
	}

	// Token: 0x06007ED2 RID: 32466 RVA: 0x000F7F4C File Offset: 0x000F614C
	public static ArmTrapWorkable GetWorkable(ReusableTrap.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x06007ED3 RID: 32467 RVA: 0x000F7F54 File Offset: 0x000F6154
	public static void ActivateLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(true);
	}

	// Token: 0x06007ED4 RID: 32468 RVA: 0x000F7F5D File Offset: 0x000F615D
	public static void DisableLure(ReusableTrap.Instance smi)
	{
		smi.SetLureActiveState(false);
	}

	// Token: 0x06007ED5 RID: 32469 RVA: 0x000F7F66 File Offset: 0x000F6166
	public static void SetupCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.SetupCapturingAnimations();
	}

	// Token: 0x06007ED6 RID: 32470 RVA: 0x000F7F6E File Offset: 0x000F616E
	public static void UnsetCapturingAnimations(ReusableTrap.Instance smi)
	{
		smi.UnsetCapturingAnimations();
	}

	// Token: 0x06007ED7 RID: 32471 RVA: 0x0033A0EC File Offset: 0x003382EC
	public static void OptionalCapturingAnimationUpdate(ReusableTrap.Instance smi, float dt)
	{
		if (smi.def.usingSymbolChaseCapturingAnimations && smi.lastCritterCapturedAnimController != null)
		{
			if (smi.lastCritterCapturedAnimController.currentAnim != smi.CAPTURING_CRITTER_ANIMATION_NAME)
			{
				smi.lastCritterCapturedAnimController.Play(smi.CAPTURING_CRITTER_ANIMATION_NAME, KAnim.PlayMode.Once, 1f, 0f);
			}
			bool flag;
			Vector3 position = smi.animController.GetSymbolTransform(smi.CAPTURING_SYMBOL_NAME, out flag).GetColumn(3);
			smi.lastCritterCapturedAnimController.transform.SetPosition(position);
		}
	}

	// Token: 0x06007ED8 RID: 32472 RVA: 0x000F7F76 File Offset: 0x000F6176
	public static void MarkAsArmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(true, smi, false);
		smi.gameObject.AddTag(GameTags.TrapArmed);
	}

	// Token: 0x06007ED9 RID: 32473 RVA: 0x000F7F9C File Offset: 0x000F619C
	public static void MarkAsUnarmed(ReusableTrap.Instance smi)
	{
		smi.sm.IsArmed.Set(false, smi, false);
		smi.gameObject.RemoveTag(GameTags.TrapArmed);
	}

	// Token: 0x04006054 RID: 24660
	public const string CAPTURE_ANIMATION_NAME = "capture";

	// Token: 0x04006055 RID: 24661
	public const string CAPTURE_LARGE_ANIMATION_NAME = "capture_large";

	// Token: 0x04006056 RID: 24662
	public const string CAPTURE_IDLE_ANIMATION_NAME = "capture_idle";

	// Token: 0x04006057 RID: 24663
	public const string CAPTURE_IDLE_LARGE_ANIMATION_NAME = "capture_idle_large";

	// Token: 0x04006058 RID: 24664
	public const string CAPTURE_RELEASE_ANIMATION_NAME = "release";

	// Token: 0x04006059 RID: 24665
	public const string CAPTURE_RELEASE_LARGE_ANIMATION_NAME = "release_large";

	// Token: 0x0400605A RID: 24666
	public const string UNARMED_ANIMATION_NAME = "unarmed";

	// Token: 0x0400605B RID: 24667
	public const string ARMED_ANIMATION_NAME = "armed";

	// Token: 0x0400605C RID: 24668
	public const string ABORT_ARMED_ANIMATION = "abort_armed";

	// Token: 0x0400605D RID: 24669
	public StateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.BoolParameter IsArmed;

	// Token: 0x0400605E RID: 24670
	public ReusableTrap.NonOperationalStates noOperational;

	// Token: 0x0400605F RID: 24671
	public ReusableTrap.OperationalStates operational;

	// Token: 0x0200180F RID: 6159
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06007EDB RID: 32475 RVA: 0x000F7FCA File Offset: 0x000F61CA
		public bool usingLure
		{
			get
			{
				return this.lures != null && this.lures.Length != 0;
			}
		}

		// Token: 0x04006060 RID: 24672
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04006061 RID: 24673
		public Tag[] lures;

		// Token: 0x04006062 RID: 24674
		public CellOffset releaseCellOffset = CellOffset.none;

		// Token: 0x04006063 RID: 24675
		public bool usingSymbolChaseCapturingAnimations;

		// Token: 0x04006064 RID: 24676
		public Func<string> getTrappedAnimationNameCallback;
	}

	// Token: 0x02001810 RID: 6160
	public class CaptureStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04006065 RID: 24677
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State capturing;

		// Token: 0x04006066 RID: 24678
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x04006067 RID: 24679
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State release;
	}

	// Token: 0x02001811 RID: 6161
	public class OperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x04006068 RID: 24680
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State unarmed;

		// Token: 0x04006069 RID: 24681
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State armed;

		// Token: 0x0400606A RID: 24682
		public ReusableTrap.CaptureStates capture;
	}

	// Token: 0x02001812 RID: 6162
	public class NonOperationalStates : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State
	{
		// Token: 0x0400606B RID: 24683
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State idle;

		// Token: 0x0400606C RID: 24684
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State releasing;

		// Token: 0x0400606D RID: 24685
		public GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.State disarming;
	}

	// Token: 0x02001813 RID: 6163
	public new class Instance : GameStateMachine<ReusableTrap, ReusableTrap.Instance, IStateMachineTarget, ReusableTrap.Def>.GameInstance, TrappedStates.ITrapStateAnimationInstructions
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06007EE0 RID: 32480 RVA: 0x000F7FFB File Offset: 0x000F61FB
		public bool IsCapturingLargeCritter
		{
			get
			{
				return this.HasCritter && this.CapturedCritter.HasTag(GameTags.LargeCreature);
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06007EE1 RID: 32481 RVA: 0x000F8017 File Offset: 0x000F6217
		public bool HasCritter
		{
			get
			{
				return !this.storage.IsEmpty();
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06007EE2 RID: 32482 RVA: 0x000F8027 File Offset: 0x000F6227
		public GameObject CapturedCritter
		{
			get
			{
				if (!this.HasCritter)
				{
					return null;
				}
				return this.storage.items[0];
			}
		}

		// Token: 0x06007EE3 RID: 32483 RVA: 0x000F8044 File Offset: 0x000F6244
		public ArmTrapWorkable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06007EE4 RID: 32484 RVA: 0x0033A190 File Offset: 0x00338390
		public void RefreshLogicOutput()
		{
			bool flag = base.IsInsideState(base.sm.operational) && this.HasCritter;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag ? 1 : 0);
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x000F804C File Offset: 0x000F624C
		public Instance(IStateMachineTarget master, ReusableTrap.Def def) : base(master, def)
		{
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x0033A1DC File Offset: 0x003383DC
		public override void StartSM()
		{
			base.StartSM();
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
			}
			ArmTrapWorkable armTrapWorkable = this.workable;
			armTrapWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(armTrapWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		}

		// Token: 0x06007EE7 RID: 32487 RVA: 0x0033A22C File Offset: 0x0033842C
		private void OnWorkEvent(Workable workable, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped && workable.GetPercentComplete() < 1f && workable.GetPercentComplete() != 0f && base.IsInsideState(base.sm.operational.unarmed))
			{
				this.animController.Play("unarmed", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06007EE8 RID: 32488 RVA: 0x000F806C File Offset: 0x000F626C
		public void SetTrapTriggerActiveState(bool active)
		{
			this.trapTrigger.enabled = active;
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x0033A290 File Offset: 0x00338490
		public void SetLureActiveState(bool activate)
		{
			if (base.def.usingLure)
			{
				Lure.Instance smi = base.gameObject.GetSMI<Lure.Instance>();
				if (smi != null)
				{
					smi.SetActiveLures(activate ? base.def.lures : null);
				}
			}
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x000F807A File Offset: 0x000F627A
		public void SetupCapturingAnimations()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				this.lastCritterCapturedAnimController = this.CapturedCritter.GetComponent<KBatchedAnimController>();
			}
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x0033A2D0 File Offset: 0x003384D0
		public void UnsetCapturingAnimations()
		{
			this.trapTrigger.SetStoredPosition(this.CapturedCritter);
			if (base.def.usingSymbolChaseCapturingAnimations && this.lastCritterCapturedAnimController != null)
			{
				this.lastCritterCapturedAnimController.Play("trapped", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.lastCritterCapturedAnimController = null;
		}

		// Token: 0x06007EEC RID: 32492 RVA: 0x0033A330 File Offset: 0x00338530
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<ArmTrapWorkable>(Db.Get().ChoreTypes.ArmTrap, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06007EED RID: 32493 RVA: 0x000F80A1 File Offset: 0x000F62A1
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("GroundTrap.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x0033A378 File Offset: 0x00338578
		public void Release()
		{
			if (this.HasCritter)
			{
				this.WasLastCritterLarge = this.IsCapturingLargeCritter;
				Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.smi.transform.GetPosition()), base.def.releaseCellOffset), Grid.SceneLayer.Creatures);
				List<GameObject> list = new List<GameObject>();
				Storage storage = this.storage;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				foreach (GameObject gameObject in list)
				{
					gameObject.transform.SetPosition(position);
					KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						component.SetSceneLayer(Grid.SceneLayer.Creatures);
					}
				}
			}
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x000F80C2 File Offset: 0x000F62C2
		public string GetTrappedAnimationName()
		{
			if (base.def.getTrappedAnimationNameCallback != null)
			{
				return base.def.getTrappedAnimationNameCallback();
			}
			return null;
		}

		// Token: 0x0400606E RID: 24686
		public string CAPTURING_CRITTER_ANIMATION_NAME = "caught_loop";

		// Token: 0x0400606F RID: 24687
		public string CAPTURING_SYMBOL_NAME = "creatureSymbol";

		// Token: 0x04006070 RID: 24688
		[MyCmpGet]
		private Storage storage;

		// Token: 0x04006071 RID: 24689
		[MyCmpGet]
		private ArmTrapWorkable workable;

		// Token: 0x04006072 RID: 24690
		[MyCmpGet]
		private TrapTrigger trapTrigger;

		// Token: 0x04006073 RID: 24691
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x04006074 RID: 24692
		[MyCmpGet]
		public LogicPorts logicPorts;

		// Token: 0x04006075 RID: 24693
		public bool WasLastCritterLarge;

		// Token: 0x04006076 RID: 24694
		public KBatchedAnimController lastCritterCapturedAnimController;

		// Token: 0x04006077 RID: 24695
		private Chore chore;
	}
}
