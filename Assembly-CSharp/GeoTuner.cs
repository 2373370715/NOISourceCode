using System;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000DD3 RID: 3539
public class GeoTuner : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>
{
	// Token: 0x060044F7 RID: 17655 RVA: 0x00257CF8 File Offset: 0x00255EF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType));
		this.nonOperational.DefaultState(this.nonOperational.off).OnSignal(this.geyserSwitchSignal, this.nonOperational.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).TagTransition(GameTags.Operational, this.operational, false);
		this.nonOperational.off.PlayAnim("off");
		this.nonOperational.switchingGeyser.QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.nonOperational.down);
		this.nonOperational.down.PlayAnim("geyser_up").QueueAnim("off", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.PlayAnim("on").Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).DefaultState(this.operational.idle).TagTransition(GameTags.Operational, this.nonOperational, true);
		this.operational.idle.ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull);
		this.operational.noGeyserSelected.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerNoGeyserSelected, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.geyserSelected.switchingGeyser, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNotNull).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorage)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.noGeyserSelected.idle);
		this.operational.noGeyserSelected.idle.PlayAnim("geyser_up").QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange));
		this.operational.geyserSelected.DefaultState(this.operational.geyserSelected.idle).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoTunerGeyserStatus, null).ParamTransition<GameObject>(this.AssignedGeyser, this.operational.noGeyserSelected, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsNull).OnSignal(this.geyserSwitchSignal, this.operational.geyserSelected.switchingGeyser).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		});
		this.operational.geyserSelected.idle.ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.researcherInteractionNeeded, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsFalse);
		this.operational.geyserSelected.switchingGeyser.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.DropStorageIfNotMatching)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshStorageRequirements)).Enter(delegate(GeoTuner.Instance smi)
		{
			smi.RefreshLogicOutput();
		}).QueueAnim("geyser_down", false, null).OnAnimQueueComplete(this.operational.geyserSelected.switchingGeyser.down);
		this.operational.geyserSelected.switchingGeyser.down.QueueAnim("geyser_up", false, null).QueueAnim("on", false, null).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RefreshAnimationGeyserSymbolType)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.TriggerSoundsForGeyserChange)).ScheduleActionNextFrame("Switch Animation Completed", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.idle);
		});
		this.operational.geyserSelected.researcherInteractionNeeded.EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.UpdateRoom, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.blocked, (GeoTuner.Instance smi) => !GeoTuner.WorkRequirementsMet(smi)).EventTransition(GameHashes.OnStorageChange, this.operational.geyserSelected.researcherInteractionNeeded.available, new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Transition.ConditionCallback(GeoTuner.WorkRequirementsMet)).ParamTransition<bool>(this.hasBeenWorkedByResearcher, this.operational.geyserSelected.broadcasting.active, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsTrue).Exit(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer));
		this.operational.geyserSelected.researcherInteractionNeeded.blocked.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).DoNothing();
		this.operational.geyserSelected.researcherInteractionNeeded.available.DefaultState(this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe).ToggleRecurringChore(new Func<GeoTuner.Instance, Chore>(this.CreateResearchChore), null).WorkableCompleteTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.completed);
		this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchNeeded, null).WorkableStartTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress);
		this.operational.geyserSelected.researcherInteractionNeeded.available.inProgress.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerResearchInProgress, null).WorkableStopTransition((GeoTuner.Instance smi) => smi.workable, this.operational.geyserSelected.researcherInteractionNeeded.available.waitingForDupe);
		this.operational.geyserSelected.researcherInteractionNeeded.completed.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.OnResearchCompleted));
		this.operational.geyserSelected.broadcasting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoTunerBroadcasting, null).Toggle("Tuning", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ApplyTuning), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.RemoveTuning));
		this.operational.geyserSelected.broadcasting.onHold.PlayAnim("on").UpdateTransition(this.operational.geyserSelected.broadcasting.active, (GeoTuner.Instance smi, float dt) => !GeoTuner.GeyserExitEruptionTransition(smi, dt), UpdateRate.SIM_200ms, false);
		this.operational.geyserSelected.broadcasting.active.Toggle("EnergyConsumption", delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}, delegate(GeoTuner.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).Toggle("BroadcastingAnimations", new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.PlayBroadcastingAnimation), new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.StopPlayingBroadcastingAnimation)).Update(new Action<GeoTuner.Instance, float>(GeoTuner.ExpirationTimerUpdate), UpdateRate.SIM_200ms, false).UpdateTransition(this.operational.geyserSelected.broadcasting.onHold, new Func<GeoTuner.Instance, float, bool>(GeoTuner.GeyserExitEruptionTransition), UpdateRate.SIM_200ms, false).ParamTransition<float>(this.expirationTimer, this.operational.geyserSelected.broadcasting.expired, GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.IsLTEZero);
		this.operational.geyserSelected.broadcasting.expired.Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ForgetWorkDoneByDupe)).Enter(new StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State.Callback(GeoTuner.ResetExpirationTimer)).ScheduleActionNextFrame("Expired", delegate(GeoTuner.Instance smi)
		{
			smi.GoTo(this.operational.geyserSelected.researcherInteractionNeeded);
		});
	}

	// Token: 0x060044F8 RID: 17656 RVA: 0x00258644 File Offset: 0x00256844
	private static void TriggerSoundsForGeyserChange(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			EventInstance instance = default(EventInstance);
			switch (assignedGeyser.configuration.geyserType.shape)
			{
			case GeyserConfigurator.GeyserShape.Gas:
				instance = SoundEvent.BeginOneShot(GeoTuner.gasGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Liquid:
				instance = SoundEvent.BeginOneShot(GeoTuner.liquidGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			case GeyserConfigurator.GeyserShape.Molten:
				instance = SoundEvent.BeginOneShot(GeoTuner.metalGeyserTuningSoundPath, smi.transform.GetPosition(), 1f, false);
				break;
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x060044F9 RID: 17657 RVA: 0x002586F0 File Offset: 0x002568F0
	private static void RefreshStorageRequirements(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser == null)
		{
			smi.storage.capacityKg = 0f;
			smi.storage.storageFilters = null;
			smi.manualDelivery.capacity = 0f;
			smi.manualDelivery.refillMass = 0f;
			smi.manualDelivery.RequestedItemTag = null;
			smi.manualDelivery.AbortDelivery("No geyser is selected for tuning");
			return;
		}
		GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
		smi.storage.capacityKg = settingsForGeyser.quantity;
		smi.storage.storageFilters = new List<Tag>
		{
			settingsForGeyser.material
		};
		smi.manualDelivery.AbortDelivery("Switching to new delivery request");
		smi.manualDelivery.capacity = settingsForGeyser.quantity;
		smi.manualDelivery.refillMass = settingsForGeyser.quantity;
		smi.manualDelivery.MinimumMass = settingsForGeyser.quantity;
		smi.manualDelivery.RequestedItemTag = settingsForGeyser.material;
	}

	// Token: 0x060044FA RID: 17658 RVA: 0x002587FC File Offset: 0x002569FC
	private static void DropStorage(GeoTuner.Instance smi)
	{
		smi.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x060044FB RID: 17659 RVA: 0x00258824 File Offset: 0x00256A24
	private static void DropStorageIfNotMatching(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = smi.def.GetSettingsForGeyser(assignedGeyser);
			List<GameObject> items = smi.storage.GetItems();
			if (smi.storage.GetItems() != null && items.Count > 0)
			{
				Tag tag = items[0].PrefabID();
				PrimaryElement component = items[0].GetComponent<PrimaryElement>();
				if (tag != settingsForGeyser.material)
				{
					smi.storage.DropAll(false, false, default(Vector3), true, null);
					return;
				}
				float num = component.Mass - settingsForGeyser.quantity;
				if (num > 0f)
				{
					smi.storage.DropSome(tag, num, false, false, default(Vector3), true, false);
					return;
				}
			}
		}
		else
		{
			smi.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x0025890C File Offset: 0x00256B0C
	private static bool GeyserExitEruptionTransition(GeoTuner.Instance smi, float dt)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		return assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent != assignedGeyser.smi.sm.erupt;
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x000D10E6 File Offset: 0x000CF2E6
	public static void OnResearchCompleted(GeoTuner.Instance smi)
	{
		smi.storage.ConsumeAllIgnoringDisease();
		smi.sm.hasBeenWorkedByResearcher.Set(true, smi, false);
	}

	// Token: 0x060044FE RID: 17662 RVA: 0x000D1107 File Offset: 0x000CF307
	public static void PlayBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060044FF RID: 17663 RVA: 0x000D1129 File Offset: 0x000CF329
	public static void StopPlayingBroadcastingAnimation(GeoTuner.Instance smi)
	{
		smi.animController.Play("broadcasting", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004500 RID: 17664 RVA: 0x000D114B File Offset: 0x000CF34B
	public static void RefreshAnimationGeyserSymbolType(GeoTuner.Instance smi)
	{
		smi.RefreshGeyserSymbol();
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x000D1153 File Offset: 0x000CF353
	public static float GetRemainingExpiraionTime(GeoTuner.Instance smi)
	{
		return smi.sm.expirationTimer.Get(smi);
	}

	// Token: 0x06004502 RID: 17666 RVA: 0x00258960 File Offset: 0x00256B60
	private static void ExpirationTimerUpdate(GeoTuner.Instance smi, float dt)
	{
		float num = GeoTuner.GetRemainingExpiraionTime(smi);
		num -= dt;
		smi.sm.expirationTimer.Set(num, smi, false);
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x0025898C File Offset: 0x00256B8C
	private static void ResetExpirationTimer(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			smi.sm.expirationTimer.Set(smi.def.GetSettingsForGeyser(assignedGeyser).duration, smi, false);
			return;
		}
		smi.sm.expirationTimer.Set(0f, smi, false);
	}

	// Token: 0x06004504 RID: 17668 RVA: 0x000D1166 File Offset: 0x000CF366
	private static void ForgetWorkDoneByDupe(GeoTuner.Instance smi)
	{
		smi.sm.hasBeenWorkedByResearcher.Set(false, smi, false);
		smi.workable.WorkTimeRemaining = smi.workable.GetWorkTime();
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x002589E8 File Offset: 0x00256BE8
	private Chore CreateResearchChore(GeoTuner.Instance smi)
	{
		return new WorkChore<GeoTunerWorkable>(Db.Get().ChoreTypes.Research, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x000D1192 File Offset: 0x000CF392
	private static void ApplyTuning(GeoTuner.Instance smi)
	{
		smi.GetAssignedGeyser().AddModification(smi.currentGeyserModification);
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x00258A20 File Offset: 0x00256C20
	private static void RemoveTuning(GeoTuner.Instance smi)
	{
		Geyser assignedGeyser = smi.GetAssignedGeyser();
		if (assignedGeyser != null)
		{
			assignedGeyser.RemoveModification(smi.currentGeyserModification);
		}
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x000D11A5 File Offset: 0x000CF3A5
	public static bool WorkRequirementsMet(GeoTuner.Instance smi)
	{
		return GeoTuner.IsInLabRoom(smi) && smi.storage.MassStored() == smi.storage.capacityKg;
	}

	// Token: 0x06004509 RID: 17673 RVA: 0x000D11C9 File Offset: 0x000CF3C9
	public static bool IsInLabRoom(GeoTuner.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x04002FDE RID: 12254
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Signal geyserSwitchSignal;

	// Token: 0x04002FDF RID: 12255
	private GeoTuner.NonOperationalState nonOperational;

	// Token: 0x04002FE0 RID: 12256
	private GeoTuner.OperationalState operational;

	// Token: 0x04002FE1 RID: 12257
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter FutureGeyser;

	// Token: 0x04002FE2 RID: 12258
	private StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter AssignedGeyser;

	// Token: 0x04002FE3 RID: 12259
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.BoolParameter hasBeenWorkedByResearcher;

	// Token: 0x04002FE4 RID: 12260
	public StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.FloatParameter expirationTimer;

	// Token: 0x04002FE5 RID: 12261
	public static string liquidGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Geyser", false);

	// Token: 0x04002FE6 RID: 12262
	public static string gasGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Vent", false);

	// Token: 0x04002FE7 RID: 12263
	public static string metalGeyserTuningSoundPath = GlobalAssets.GetSound("GeoTuner_Tuning_Volcano", false);

	// Token: 0x04002FE8 RID: 12264
	public const string anim_switchGeyser_down = "geyser_down";

	// Token: 0x04002FE9 RID: 12265
	public const string anim_switchGeyser_up = "geyser_up";

	// Token: 0x04002FEA RID: 12266
	private const string BroadcastingOnHoldAnimationName = "on";

	// Token: 0x04002FEB RID: 12267
	private const string OnAnimName = "on";

	// Token: 0x04002FEC RID: 12268
	private const string OffAnimName = "off";

	// Token: 0x04002FED RID: 12269
	private const string BroadcastingAnimationName = "broadcasting";

	// Token: 0x02000DD4 RID: 3540
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600450E RID: 17678 RVA: 0x00258A4C File Offset: 0x00256C4C
		public GeoTunerConfig.GeotunedGeyserSettings GetSettingsForGeyser(Geyser geyser)
		{
			GeoTunerConfig.GeotunedGeyserSettings result;
			if (!this.geotunedGeyserSettings.TryGetValue(geyser.configuration.typeId, out result))
			{
				DebugUtil.DevLogError(string.Format("Geyser {0} is missing a Geotuner setting, using default", geyser.configuration.typeId));
				return this.defaultSetting;
			}
			return result;
		}

		// Token: 0x04002FEE RID: 12270
		public string OUTPUT_LOGIC_PORT_ID;

		// Token: 0x04002FEF RID: 12271
		public Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunedGeyserSettings;

		// Token: 0x04002FF0 RID: 12272
		public GeoTunerConfig.GeotunedGeyserSettings defaultSetting;
	}

	// Token: 0x02000DD5 RID: 3541
	public class BroadcastingState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FF1 RID: 12273
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State active;

		// Token: 0x04002FF2 RID: 12274
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State onHold;

		// Token: 0x04002FF3 RID: 12275
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State expired;
	}

	// Token: 0x02000DD6 RID: 3542
	public class ResearchProgress : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FF4 RID: 12276
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State waitingForDupe;

		// Token: 0x04002FF5 RID: 12277
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State inProgress;
	}

	// Token: 0x02000DD7 RID: 3543
	public class ResearchState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FF6 RID: 12278
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State blocked;

		// Token: 0x04002FF7 RID: 12279
		public GeoTuner.ResearchProgress available;

		// Token: 0x04002FF8 RID: 12280
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State completed;
	}

	// Token: 0x02000DD8 RID: 3544
	public class SwitchingGeyser : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FF9 RID: 12281
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x02000DD9 RID: 3545
	public class GeyserSelectedState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FFA RID: 12282
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x04002FFB RID: 12283
		public GeoTuner.SwitchingGeyser switchingGeyser;

		// Token: 0x04002FFC RID: 12284
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State resourceNeeded;

		// Token: 0x04002FFD RID: 12285
		public GeoTuner.ResearchState researcherInteractionNeeded;

		// Token: 0x04002FFE RID: 12286
		public GeoTuner.BroadcastingState broadcasting;
	}

	// Token: 0x02000DDA RID: 3546
	public class SimpleIdleState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04002FFF RID: 12287
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;
	}

	// Token: 0x02000DDB RID: 3547
	public class NonOperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04003000 RID: 12288
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State off;

		// Token: 0x04003001 RID: 12289
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State switchingGeyser;

		// Token: 0x04003002 RID: 12290
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State down;
	}

	// Token: 0x02000DDC RID: 3548
	public class OperationalState : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State
	{
		// Token: 0x04003003 RID: 12291
		public GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.State idle;

		// Token: 0x04003004 RID: 12292
		public GeoTuner.SimpleIdleState noGeyserSelected;

		// Token: 0x04003005 RID: 12293
		public GeoTuner.GeyserSelectedState geyserSelected;
	}

	// Token: 0x02000DDD RID: 3549
	public enum GeyserAnimTypeSymbols
	{
		// Token: 0x04003007 RID: 12295
		meter_gas,
		// Token: 0x04003008 RID: 12296
		meter_metal,
		// Token: 0x04003009 RID: 12297
		meter_liquid,
		// Token: 0x0400300A RID: 12298
		meter_board
	}

	// Token: 0x02000DDE RID: 3550
	public new class Instance : GameStateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.GameInstance
	{
		// Token: 0x06004518 RID: 17688 RVA: 0x00258A9C File Offset: 0x00256C9C
		public Instance(IStateMachineTarget master, GeoTuner.Def def) : base(master, def)
		{
			this.originID = UI.StripLinkFormatting("GeoTuner") + " [" + base.gameObject.GetInstanceID().ToString() + "]";
			this.switchGeyserMeter = new MeterController(this.animController, "geyser_target", this.GetAnimationSymbol().ToString(), Meter.Offset.Behind, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x00258B18 File Offset: 0x00256D18
		public override void StartSM()
		{
			base.StartSM();
			Components.GeoTuners.Add(base.smi.GetMyWorldId(), this);
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				assignedGeyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
				this.RefreshModification();
			}
			this.RefreshLogicOutput();
			this.AssignFutureGeyser(this.GetFutureGeyser());
			base.gameObject.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x000D1248 File Offset: 0x000CF448
		public Geyser GetFutureGeyser()
		{
			if (base.smi.sm.FutureGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.FutureGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x000D127A File Offset: 0x000CF47A
		public Geyser GetAssignedGeyser()
		{
			if (base.smi.sm.AssignedGeyser.IsNull(this))
			{
				return null;
			}
			return base.sm.AssignedGeyser.Get(this).GetComponent<Geyser>();
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00258BA0 File Offset: 0x00256DA0
		public void AssignFutureGeyser(Geyser newFutureGeyser)
		{
			bool flag = newFutureGeyser != this.GetFutureGeyser();
			bool flag2 = this.GetAssignedGeyser() != newFutureGeyser;
			base.sm.FutureGeyser.Set(newFutureGeyser, this);
			if (flag)
			{
				if (flag2)
				{
					this.RecreateSwitchGeyserChore();
					return;
				}
				if (this.switchGeyserChore != null)
				{
					this.AbortSwitchGeyserChore("Future Geyser was set to current Geyser");
					return;
				}
			}
			else if (this.switchGeyserChore == null && flag2)
			{
				this.RecreateSwitchGeyserChore();
			}
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00258C10 File Offset: 0x00256E10
		private void AbortSwitchGeyserChore(string reason = "Aborting Switch Geyser Chore")
		{
			if (this.switchGeyserChore != null)
			{
				Chore chore = this.switchGeyserChore;
				chore.onComplete = (Action<Chore>)Delegate.Remove(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
				this.switchGeyserChore.Cancel(reason);
				this.switchGeyserChore = null;
			}
			this.switchGeyserChore = null;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00258C68 File Offset: 0x00256E68
		private Chore RecreateSwitchGeyserChore()
		{
			this.AbortSwitchGeyserChore("Recreating Chore");
			this.switchGeyserChore = new WorkChore<GeoTunerSwitchGeyserWorkable>(Db.Get().ChoreTypes.Toggle, this.switchGeyserWorkable, null, true, null, new Action<Chore>(this.ShowSwitchingGeyserStatusItem), new Action<Chore>(this.HideSwitchingGeyserStatusItem), true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			Chore chore = this.switchGeyserChore;
			chore.onComplete = (Action<Chore>)Delegate.Combine(chore.onComplete, new Action<Chore>(this.OnSwitchGeyserChoreCompleted));
			return this.switchGeyserChore;
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x000D12AC File Offset: 0x000CF4AC
		private void ShowSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, null);
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x000D12CF File Offset: 0x000CF4CF
		private void HideSwitchingGeyserStatusItem(Chore chore)
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00258CF4 File Offset: 0x00256EF4
		private void OnSwitchGeyserChoreCompleted(Chore chore)
		{
			this.GetCurrentState();
			GeoTuner.NonOperationalState nonOperational = base.sm.nonOperational;
			Geyser futureGeyser = this.GetFutureGeyser();
			bool flag = this.GetAssignedGeyser() != futureGeyser;
			if (chore.isComplete && flag)
			{
				this.AssignGeyser(futureGeyser);
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x00258D48 File Offset: 0x00256F48
		public void AssignGeyser(Geyser geyser)
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null && assignedGeyser != geyser)
			{
				GeoTuner.RemoveTuning(base.smi);
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			Geyser geyser2 = assignedGeyser;
			base.sm.AssignedGeyser.Set(geyser, this);
			this.RefreshModification();
			if (geyser2 != geyser)
			{
				if (geyser != null)
				{
					geyser.Subscribe(-593169791, new Action<object>(this.OnEruptionStateChanged));
					geyser.Trigger(1763323737, null);
				}
				if (geyser2 != null)
				{
					geyser2.Trigger(1763323737, null);
				}
				base.sm.geyserSwitchSignal.Trigger(this);
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00258E0C File Offset: 0x0025700C
		private void RefreshModification()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = base.def.GetSettingsForGeyser(assignedGeyser);
				this.currentGeyserModification = settingsForGeyser.template;
				this.currentGeyserModification.originID = this.originID;
				this.enhancementDuration = settingsForGeyser.duration;
				assignedGeyser.Trigger(1763323737, null);
			}
			GeoTuner.RefreshStorageRequirements(this);
			GeoTuner.DropStorageIfNotMatching(this);
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00258E78 File Offset: 0x00257078
		public void RefreshGeyserSymbol()
		{
			this.switchGeyserMeter.meterController.Play(this.GetAnimationSymbol().ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00258EBC File Offset: 0x002570BC
		private GeoTuner.GeyserAnimTypeSymbols GetAnimationSymbol()
		{
			GeoTuner.GeyserAnimTypeSymbols result = GeoTuner.GeyserAnimTypeSymbols.meter_board;
			Geyser assignedGeyser = base.smi.GetAssignedGeyser();
			if (assignedGeyser != null)
			{
				switch (assignedGeyser.configuration.geyserType.shape)
				{
				case GeyserConfigurator.GeyserShape.Gas:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_gas;
					break;
				case GeyserConfigurator.GeyserShape.Liquid:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_liquid;
					break;
				case GeyserConfigurator.GeyserShape.Molten:
					result = GeoTuner.GeyserAnimTypeSymbols.meter_metal;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x000D12F2 File Offset: 0x000CF4F2
		public void OnEruptionStateChanged(object data)
		{
			bool flag = (bool)data;
			this.RefreshLogicOutput();
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00258F10 File Offset: 0x00257110
		public void RefreshLogicOutput()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			bool flag = this.GetCurrentState() != base.smi.sm.nonOperational;
			bool flag2 = assignedGeyser != null && this.GetCurrentState() != base.smi.sm.operational.noGeyserSelected;
			bool flag3 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && (assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.erupt || assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt);
			bool flag4 = flag && flag2 && flag3;
			this.logicPorts.SendSignal(base.def.OUTPUT_LOGIC_PORT_ID, flag4 ? 1 : 0);
			this.switchGeyserMeter.meterController.SetSymbolVisiblity("light_bloom", flag4);
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0025900C File Offset: 0x0025720C
		public void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				GeoTuner.Instance smi = gameObject.GetSMI<GeoTuner.Instance>();
				if (smi != null && smi.GetFutureGeyser() != this.GetFutureGeyser())
				{
					Geyser futureGeyser = smi.GetFutureGeyser();
					if (futureGeyser != null && futureGeyser.GetAmountOfGeotunersPointingOrWillPointAtThisGeyser() < 5)
					{
						this.AssignFutureGeyser(smi.GetFutureGeyser());
					}
				}
			}
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x0025906C File Offset: 0x0025726C
		protected override void OnCleanUp()
		{
			Geyser assignedGeyser = this.GetAssignedGeyser();
			Components.GeoTuners.Remove(base.smi.GetMyWorldId(), this);
			if (assignedGeyser != null)
			{
				assignedGeyser.Unsubscribe(-593169791, new Action<object>(base.smi.OnEruptionStateChanged));
			}
			GeoTuner.RemoveTuning(this);
		}

		// Token: 0x0400300B RID: 12299
		[MyCmpReq]
		public Operational operational;

		// Token: 0x0400300C RID: 12300
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400300D RID: 12301
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x0400300E RID: 12302
		[MyCmpReq]
		public GeoTunerWorkable workable;

		// Token: 0x0400300F RID: 12303
		[MyCmpReq]
		public GeoTunerSwitchGeyserWorkable switchGeyserWorkable;

		// Token: 0x04003010 RID: 12304
		[MyCmpReq]
		public LogicPorts logicPorts;

		// Token: 0x04003011 RID: 12305
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x04003012 RID: 12306
		[MyCmpReq]
		public KBatchedAnimController animController;

		// Token: 0x04003013 RID: 12307
		public MeterController switchGeyserMeter;

		// Token: 0x04003014 RID: 12308
		public string originID;

		// Token: 0x04003015 RID: 12309
		public float enhancementDuration;

		// Token: 0x04003016 RID: 12310
		public Geyser.GeyserModification currentGeyserModification;

		// Token: 0x04003017 RID: 12311
		private Chore switchGeyserChore;
	}
}
