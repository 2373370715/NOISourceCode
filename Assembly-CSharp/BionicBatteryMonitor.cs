using System;
using System.Collections.Generic;
using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using UnityEngine;

// Token: 0x02001534 RID: 5428
public class BionicBatteryMonitor : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>
{
	// Token: 0x060070EB RID: 28907 RVA: 0x003088E4 File Offset: 0x00306AE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.firstSpawn;
		this.firstSpawn.ParamTransition<bool>(this.InitialElectrobanksSpawned, this.online, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsTrue).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SpawnAndInstallInitialElectrobanks));
		this.online.TriggerOnEnter(GameHashes.BionicOnline, null).Transition(this.offline, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.DoesNotHaveCharge), UpdateRate.SIM_200ms).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.ReorganizeElectrobankStorage)).Update(new Action<BionicBatteryMonitor.Instance, float>(BionicBatteryMonitor.DischargeUpdate), UpdateRate.SIM_200ms, false).DefaultState(this.online.idle);
		this.online.idle.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).OnSignal(this.OnElectrobankStorageChanged, this.online.upkeep, new Func<BionicBatteryMonitor.Instance, bool>(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleChanged, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep)).EventTransition(GameHashes.ScheduleBlocksTick, this.online.upkeep, new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep));
		this.online.upkeep.ParamTransition<int>(this.ChargedElectrobankCount, this.online.critical, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsLTEOne_Int).EventTransition(GameHashes.ScheduleBlocksChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleChanged, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).EventTransition(GameHashes.ScheduleBlocksTick, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Not(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Transition.ConditionCallback(BionicBatteryMonitor.WantsToUpkeep))).OnSignal(this.OnElectrobankStorageChanged, this.online.idle, (BionicBatteryMonitor.Instance smi) => !BionicBatteryMonitor.WantsToUpkeep(smi)).DefaultState(this.online.upkeep.seekElectrobank);
		this.online.upkeep.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), this.online.idle);
		this.online.critical.DefaultState(this.online.critical.seekElectrobank).ParamTransition<int>(this.ChargedElectrobankCount, this.online.idle, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTOne_Int).DoTutorial(Tutorial.TutorialMessages.TM_BionicBattery);
		this.online.critical.seekElectrobank.ToggleUrge(Db.Get().Urges.ReloadElectrobank).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new ReloadElectrobankChore(smi.master), null);
		this.offline.DefaultState(this.offline.waitingForBatteryDelivery).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((BionicBatteryMonitor.Instance smi) => new BeOfflineChore(smi.master), null).ToggleUrge(Db.Get().Urges.BeOffline).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOffline)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DropAllDischargedElectrobanks)).TriggerOnEnter(GameHashes.BionicOffline, null);
		this.offline.waitingForBatteryDelivery.ParamTransition<int>(this.ChargedElectrobankCount, this.offline.waitingForBatteryInstallation, GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IsGTZero_Int).Toggle("Enable Delivery of new Electrobanks", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnableManualDelivery), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisableManualDelivery)).Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent));
		this.offline.waitingForBatteryInstallation.Toggle("Enable User Prioritization", new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.EnablePrioritizationComponent), new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.DisablePrioritizationComponent)).Enter(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.StartReanimateWorkChore)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.CancelReanimateWorkChore)).WorkableCompleteTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.reboot).DefaultState(this.offline.waitingForBatteryInstallation.waiting);
		this.offline.waitingForBatteryInstallation.waiting.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicWaitingForReboot, null).WorkableStartTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.working);
		this.offline.waitingForBatteryInstallation.working.WorkableStopTransition(new Func<BionicBatteryMonitor.Instance, Workable>(BionicBatteryMonitor.GetReanimateWorkable), this.offline.waitingForBatteryInstallation.waiting);
		this.offline.reboot.PlayAnim("power_up").OnAnimQueueComplete(this.online).ScheduleGoTo(10f, this.online).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.AutomaticallyDropAllDepletedElectrobanks)).Exit(new StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State.Callback(BionicBatteryMonitor.SetOnline));
	}

	// Token: 0x060070EC RID: 28908 RVA: 0x000EE4A8 File Offset: 0x000EC6A8
	public static ReanimateBionicWorkable GetReanimateWorkable(BionicBatteryMonitor.Instance smi)
	{
		return smi.reanimateWorkable;
	}

	// Token: 0x060070ED RID: 28909 RVA: 0x000EE4B0 File Offset: 0x000EC6B0
	public static bool DoesNotHaveCharge(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 0f;
	}

	// Token: 0x060070EE RID: 28910 RVA: 0x000EE4C2 File Offset: 0x000EC6C2
	public static bool IsCriticallyLow(BionicBatteryMonitor.Instance smi)
	{
		return smi.ChargedElectrobankCount <= 1;
	}

	// Token: 0x060070EF RID: 28911 RVA: 0x000EE4D0 File Offset: 0x000EC6D0
	public static bool ChargeIsBelowNotificationThreshold(BionicBatteryMonitor.Instance smi)
	{
		return smi.CurrentCharge <= 30000f;
	}

	// Token: 0x060070F0 RID: 28912 RVA: 0x000EE4E2 File Offset: 0x000EC6E2
	public static bool IsAnyElectrobankAvailableToBeFetched(BionicBatteryMonitor.Instance smi)
	{
		return smi.GetClosestElectrobank() != null;
	}

	// Token: 0x060070F1 RID: 28913 RVA: 0x000EE4F0 File Offset: 0x000EC6F0
	public static bool WantsToInstallNewBattery(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.IsCriticallyLow(smi) || (smi.InUpkeepTime && smi.ChargedElectrobankCount < smi.ElectrobankCountCapacity);
	}

	// Token: 0x060070F2 RID: 28914 RVA: 0x000EE514 File Offset: 0x000EC714
	public static bool WantsToUpkeep(BionicBatteryMonitor.Instance smi)
	{
		return BionicBatteryMonitor.WantsToInstallNewBattery(smi);
	}

	// Token: 0x060070F3 RID: 28915 RVA: 0x000EE51C File Offset: 0x000EC71C
	public static void SpawnAndInstallInitialElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.SpawnAndInstallInitialElectrobanks();
	}

	// Token: 0x060070F4 RID: 28916 RVA: 0x000EE524 File Offset: 0x000EC724
	public static void RefreshCharge(BionicBatteryMonitor.Instance smi)
	{
		smi.RefreshCharge();
	}

	// Token: 0x060070F5 RID: 28917 RVA: 0x000EE52C File Offset: 0x000EC72C
	public static void EnableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(true);
	}

	// Token: 0x060070F6 RID: 28918 RVA: 0x000EE535 File Offset: 0x000EC735
	public static void DisableManualDelivery(BionicBatteryMonitor.Instance smi)
	{
		smi.SetManualDeliveryEnableState(false);
	}

	// Token: 0x060070F7 RID: 28919 RVA: 0x000EE53E File Offset: 0x000EC73E
	public static void StartReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x060070F8 RID: 28920 RVA: 0x000EE546 File Offset: 0x000EC746
	public static void CancelReanimateWorkChore(BionicBatteryMonitor.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x060070F9 RID: 28921 RVA: 0x000EE54E File Offset: 0x000EC74E
	public static void SetOffline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(false);
	}

	// Token: 0x060070FA RID: 28922 RVA: 0x000EE557 File Offset: 0x000EC757
	public static void SetOnline(BionicBatteryMonitor.Instance smi)
	{
		smi.SetOnlineState(true);
	}

	// Token: 0x060070FB RID: 28923 RVA: 0x000EE560 File Offset: 0x000EC760
	public static void AutomaticallyDropAllDepletedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.AutomaticallyDropAllDepletedElectrobanks();
	}

	// Token: 0x060070FC RID: 28924 RVA: 0x000EE568 File Offset: 0x000EC768
	public static void ReorganizeElectrobankStorage(BionicBatteryMonitor.Instance smi)
	{
		smi.ReorganizeElectrobanks();
	}

	// Token: 0x060070FD RID: 28925 RVA: 0x000EE570 File Offset: 0x000EC770
	public static void DropAllDischargedElectrobanks(BionicBatteryMonitor.Instance smi)
	{
		smi.DropAllDischargedElectrobanks();
	}

	// Token: 0x060070FE RID: 28926 RVA: 0x000EE578 File Offset: 0x000EC778
	public static void EnablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
	{
		Prioritizable.AddRef(smi.gameObject);
		smi.gameObject.Trigger(1980521255, null);
	}

	// Token: 0x060070FF RID: 28927 RVA: 0x000EE596 File Offset: 0x000EC796
	public static void DisablePrioritizationComponent(BionicBatteryMonitor.Instance smi)
	{
		Prioritizable.RemoveRef(smi.gameObject);
		smi.gameObject.Trigger(1980521255, null);
	}

	// Token: 0x06007100 RID: 28928 RVA: 0x00308E3C File Offset: 0x0030703C
	public static void DischargeUpdate(BionicBatteryMonitor.Instance smi, float dt)
	{
		float joules = Mathf.Min(dt * smi.Wattage, smi.CurrentCharge);
		smi.ConsumePower(joules);
	}

	// Token: 0x06007101 RID: 28929 RVA: 0x000EE5B4 File Offset: 0x000EC7B4
	private static BionicBatteryMonitor.WattageModifier MakeDifficultyModifier(string id, string desc, float watts)
	{
		return new BionicBatteryMonitor.WattageModifier(id, desc + ": <b>" + ((watts >= 0f) ? "+</b>" : "-</b>") + GameUtil.GetFormattedWattage(Mathf.Abs(watts), GameUtil.WattageFormatterUnit.Automatic, true), watts, watts);
	}

	// Token: 0x06007102 RID: 28930 RVA: 0x00308E64 File Offset: 0x00307064
	public static BionicBatteryMonitor.WattageModifier GetDifficultyModifier()
	{
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
		BionicBatteryMonitor.WattageModifier result;
		if (BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out result))
		{
			return result;
		}
		return BionicBatteryMonitor.difficultyWattages["Default"];
	}

	// Token: 0x040054DA RID: 21722
	public const int DEFAULT_ELECTROBANK_COUNT = 4;

	// Token: 0x040054DB RID: 21723
	public const int BIONIC_SKILL_EXTRA_BATTERY_COUNT = 2;

	// Token: 0x040054DC RID: 21724
	public const int MAX_ELECTROBANK_COUNT = 6;

	// Token: 0x040054DD RID: 21725
	public const float DEFAULT_WATTS = 200f;

	// Token: 0x040054DE RID: 21726
	public const string INITIAL_ELECTROBANK_TYPE_ID = "DisposableElectrobank_RawMetal";

	// Token: 0x040054DF RID: 21727
	public static readonly string ChargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_charged_electrobank\">";

	// Token: 0x040054E0 RID: 21728
	public static readonly string DischargedBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_discharged_electrobank\">";

	// Token: 0x040054E1 RID: 21729
	public static readonly string CriticalBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_critical_electrobank\">";

	// Token: 0x040054E2 RID: 21730
	public static readonly string SavingBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_saving_electrobank\">";

	// Token: 0x040054E3 RID: 21731
	public static readonly string EmptySlotBatteryIcon = "<sprite=\"oni_sprite_assets\" name=\"oni_sprite_assets_empty_slot_electrobank\">";

	// Token: 0x040054E4 RID: 21732
	private const string ANIM_NAME_REBOOT = "power_up";

	// Token: 0x040054E5 RID: 21733
	public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State firstSpawn;

	// Token: 0x040054E6 RID: 21734
	public BionicBatteryMonitor.OnlineStates online;

	// Token: 0x040054E7 RID: 21735
	public BionicBatteryMonitor.OfflineStates offline;

	// Token: 0x040054E8 RID: 21736
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnClosestAvailableElectrobankChangedSignal;

	// Token: 0x040054E9 RID: 21737
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter ChargedElectrobankCount;

	// Token: 0x040054EA RID: 21738
	public StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.IntParameter DepletedElectrobankCount;

	// Token: 0x040054EB RID: 21739
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter InitialElectrobanksSpawned;

	// Token: 0x040054EC RID: 21740
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.BoolParameter IsOnline;

	// Token: 0x040054ED RID: 21741
	private StateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.Signal OnElectrobankStorageChanged;

	// Token: 0x040054EE RID: 21742
	private static readonly Dictionary<string, BionicBatteryMonitor.WattageModifier> difficultyWattages = new Dictionary<string, BionicBatteryMonitor.WattageModifier>
	{
		{
			"VeryHard",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYHARD.NAME, 200f)
		},
		{
			"Hard",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.HARD.NAME, 100f)
		},
		{
			"Default",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.DEFAULT.NAME, 0f)
		},
		{
			"Easy",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.EASY.NAME, -100f)
		},
		{
			"VeryEasy",
			BionicBatteryMonitor.MakeDifficultyModifier("difficultyWattage", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.BIONICPOWERUSE.LEVELS.VERYEASY.NAME, -150f)
		}
	};

	// Token: 0x02001535 RID: 5429
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001536 RID: 5430
	public struct WattageModifier
	{
		// Token: 0x06007106 RID: 28934 RVA: 0x000EE5F2 File Offset: 0x000EC7F2
		public WattageModifier(string id, string name, float value, float potentialValue)
		{
			this.id = id;
			this.name = name;
			this.value = value;
			this.potentialValue = potentialValue;
		}

		// Token: 0x040054EF RID: 21743
		public float potentialValue;

		// Token: 0x040054F0 RID: 21744
		public float value;

		// Token: 0x040054F1 RID: 21745
		public string name;

		// Token: 0x040054F2 RID: 21746
		public string id;
	}

	// Token: 0x02001537 RID: 5431
	public class OnlineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040054F3 RID: 21747
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State idle;

		// Token: 0x040054F4 RID: 21748
		public BionicBatteryMonitor.UpkeepStates upkeep;

		// Token: 0x040054F5 RID: 21749
		public BionicBatteryMonitor.UpkeepStates critical;
	}

	// Token: 0x02001538 RID: 5432
	public class UpkeepStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040054F6 RID: 21750
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State seekElectrobank;
	}

	// Token: 0x02001539 RID: 5433
	public class OfflineStates : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040054F7 RID: 21751
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waitingForBatteryDelivery;

		// Token: 0x040054F8 RID: 21752
		public BionicBatteryMonitor.RebootWorkableState waitingForBatteryInstallation;

		// Token: 0x040054F9 RID: 21753
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State reboot;
	}

	// Token: 0x0200153A RID: 5434
	public class RebootWorkableState : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State
	{
		// Token: 0x040054FA RID: 21754
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State waiting;

		// Token: 0x040054FB RID: 21755
		public GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.State working;
	}

	// Token: 0x0200153B RID: 5435
	public new class Instance : GameStateMachine<BionicBatteryMonitor, BionicBatteryMonitor.Instance, IStateMachineTarget, BionicBatteryMonitor.Def>.GameInstance
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x0600710B RID: 28939 RVA: 0x000EE619 File Offset: 0x000EC819
		public float Wattage
		{
			get
			{
				return this.GetBaseWattage() + this.GetModifiersWattage();
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x0600710C RID: 28940 RVA: 0x000EE628 File Offset: 0x000EC828
		public bool IsOnline
		{
			get
			{
				return base.sm.IsOnline.Get(this);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x0600710D RID: 28941 RVA: 0x000EE63B File Offset: 0x000EC83B
		public bool InUpkeepTime
		{
			get
			{
				return this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x0600710E RID: 28942 RVA: 0x000EE657 File Offset: 0x000EC857
		public bool HaveInitialElectrobanksBeenSpawned
		{
			get
			{
				return base.sm.InitialElectrobanksSpawned.Get(this);
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x0600710F RID: 28943 RVA: 0x000EE66A File Offset: 0x000EC86A
		public bool HasSpaceForNewElectrobank
		{
			get
			{
				return this.ElectrobankCount < this.ElectrobankCountCapacity;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06007110 RID: 28944 RVA: 0x000EE67A File Offset: 0x000EC87A
		public int ElectrobankCount
		{
			get
			{
				return this.ChargedElectrobankCount + this.DepletedElectrobankCount;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06007111 RID: 28945 RVA: 0x000EE689 File Offset: 0x000EC889
		public int ChargedElectrobankCount
		{
			get
			{
				return base.sm.ChargedElectrobankCount.Get(this);
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06007112 RID: 28946 RVA: 0x000EE69C File Offset: 0x000EC89C
		public int DepletedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("EmptyElectrobank") / 20f);
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06007113 RID: 28947 RVA: 0x000EE6BA File Offset: 0x000EC8BA
		public int DamagedElectrobankCount
		{
			get
			{
				return (int)(this.storage.GetMassAvailable("GarbageElectrobank") / 20f);
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x000EE6D8 File Offset: 0x000EC8D8
		public float CurrentCharge
		{
			get
			{
				return this.BionicBattery.value;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06007115 RID: 28949 RVA: 0x000EE6E5 File Offset: 0x000EC8E5
		public int ElectrobankCountCapacity
		{
			get
			{
				return (int)base.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBatteryCountCapacity.Id).GetTotalValue();
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06007117 RID: 28951 RVA: 0x000EE71A File Offset: 0x000EC91A
		// (set) Token: 0x06007116 RID: 28950 RVA: 0x000EE711 File Offset: 0x000EC911
		public ReanimateBionicWorkable reanimateWorkable { get; private set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06007119 RID: 28953 RVA: 0x000EE72B File Offset: 0x000EC92B
		// (set) Token: 0x06007118 RID: 28952 RVA: 0x000EE722 File Offset: 0x000EC922
		public List<BionicBatteryMonitor.WattageModifier> Modifiers { get; set; } = new List<BionicBatteryMonitor.WattageModifier>();

		// Token: 0x0600711A RID: 28954 RVA: 0x00308FA8 File Offset: 0x003071A8
		public Instance(IStateMachineTarget master, BionicBatteryMonitor.Def def) : base(master, def)
		{
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicBatteryStorage);
			this.reanimateWorkable = base.GetComponent<ReanimateBionicWorkable>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.manualDelivery = base.GetComponent<ManualDeliveryKG>();
			this.selectable = base.GetComponent<KSelectable>();
			this.prefabID = base.GetComponent<KPrefabID>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.BionicBattery = Db.Get().Amounts.BionicInternalBattery.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.onDestroyItemsDropped = (Action<List<GameObject>>)Delegate.Combine(storage.onDestroyItemsDropped, new Action<List<GameObject>>(this.OnBatteriesDroppedFromDeath));
			Storage storage2 = this.storage;
			storage2.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage2.OnStorageChange, new Action<GameObject>(this.OnElectrobankStorageChanged));
			base.Subscribe(540773776, new Action<object>(this.OnSkillsChanged));
			this.UpdateCapacityAmount();
			this.ApplyDifficultyModifiers();
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x000EE733 File Offset: 0x000EC933
		public override void StartSM()
		{
			this.closestElectrobankSensor = base.GetComponent<Sensors>().GetSensor<ClosestElectrobankSensor>();
			ClosestElectrobankSensor closestElectrobankSensor = this.closestElectrobankSensor;
			closestElectrobankSensor.OnItemChanged = (Action<Electrobank>)Delegate.Combine(closestElectrobankSensor.OnItemChanged, new Action<Electrobank>(this.OnClosestElectrobankChanged));
			base.StartSM();
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x00309100 File Offset: 0x00307300
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					this.HaveInitialElectrobanksBeenSpawned,
					this.IsOnline
				}
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x00309140 File Offset: 0x00307340
		public override void OnParamsDeserialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicBatteryMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					bool value = (dataPackData.Bools != null && dataPackData.Bools.Length != 0) ? dataPackData.Bools[0] : this.HasSpaceForNewElectrobank;
					bool value2 = (dataPackData.Bools != null && dataPackData.Bools.Length > 1) ? dataPackData.Bools[1] : this.IsOnline;
					base.sm.InitialElectrobanksSpawned.Set(value, this, false);
					base.sm.IsOnline.Set(value2, this, false);
				}
			}
			this.RefreshCharge();
			base.OnParamsDeserialized();
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x003091EC File Offset: 0x003073EC
		public void DropAllDischargedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			foreach (GameObject go in list)
			{
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x0600711F RID: 28959 RVA: 0x0030925C File Offset: 0x0030745C
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			this.UpdateNotifications();
			base.OnCleanUp();
		}

		// Token: 0x06007120 RID: 28960 RVA: 0x000EE773 File Offset: 0x000EC973
		private void OnSkillsChanged(object o)
		{
			if (this.storage.capacityKg != (float)this.ElectrobankCountCapacity)
			{
				this.OnBatteryCapacityChanged();
			}
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x003092AC File Offset: 0x003074AC
		private void ApplyDifficultyModifiers()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.BionicWattage);
			BionicBatteryMonitor.WattageModifier item;
			if (BionicBatteryMonitor.difficultyWattages.TryGetValue(currentQualitySetting.id, out item))
			{
				this.Modifiers.Add(item);
			}
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x003092EC File Offset: 0x003074EC
		private void UpdateCapacityAmount()
		{
			int num = this.ElectrobankCountCapacity - 4;
			this.BionicBattery.maxAttribute.ClearModifiers();
			this.BionicBattery.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.BionicInternalBattery.maxAttribute.Id, 120000f * (float)num, null, false, false, true));
		}

		// Token: 0x06007123 RID: 28963 RVA: 0x0030934C File Offset: 0x0030754C
		private void OnBatteryCapacityChanged()
		{
			this.UpdateCapacityAmount();
			for (int i = this.storage.Count - 1; i >= 0; i--)
			{
				if (this.storage.Count > this.ElectrobankCountCapacity)
				{
					GameObject gameObject = this.storage.items[i];
					Electrobank component = gameObject.GetComponent<Electrobank>();
					this.storage.Drop(gameObject, true);
					Vector3 position = gameObject.transform.position;
					position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					gameObject.transform.position = position;
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
			base.smi.storage.capacityKg = (float)this.ElectrobankCountCapacity;
		}

		// Token: 0x06007124 RID: 28964 RVA: 0x000EE78F File Offset: 0x000EC98F
		private void OnClosestElectrobankChanged(Electrobank newItem)
		{
			base.sm.OnClosestAvailableElectrobankChangedSignal.Trigger(this);
		}

		// Token: 0x06007125 RID: 28965 RVA: 0x000D405A File Offset: 0x000D225A
		public float GetBaseWattage()
		{
			return 200f;
		}

		// Token: 0x06007126 RID: 28966 RVA: 0x00309420 File Offset: 0x00307620
		public float GetModifiersWattage()
		{
			float num = 0f;
			foreach (BionicBatteryMonitor.WattageModifier wattageModifier in this.Modifiers)
			{
				num += wattageModifier.value;
			}
			return num;
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x000EE7A2 File Offset: 0x000EC9A2
		private void OnElectrobankStorageChanged(object o)
		{
			this.ReorganizeElectrobanks();
			this.RefreshCharge();
			base.smi.sm.OnElectrobankStorageChanged.Trigger(this);
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x000EE7C6 File Offset: 0x000EC9C6
		public void ReorganizeElectrobanks()
		{
			this.storage.items.Sort(delegate(GameObject b1, GameObject b2)
			{
				Electrobank component = b1.GetComponent<Electrobank>();
				Electrobank component2 = b2.GetComponent<Electrobank>();
				if (component == null)
				{
					return -1;
				}
				if (component2 == null)
				{
					return 1;
				}
				return component.Charge.CompareTo(component2.Charge);
			});
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x0030947C File Offset: 0x0030767C
		public void CreateWorkableChore()
		{
			if (this.reanimateChore == null)
			{
				this.reanimateChore = new WorkChore<ReanimateBionicWorkable>(Db.Get().ChoreTypes.RescueIncapacitated, this.reanimateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
				this.reanimateChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
			}
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x000EE7F7 File Offset: 0x000EC9F7
		public void CancelWorkChore()
		{
			if (this.reanimateChore != null)
			{
				this.reanimateChore.Cancel("BionicBatteryMonitor.CancelChore");
				this.reanimateChore = null;
			}
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x000EE818 File Offset: 0x000ECA18
		public void SetOnlineState(bool online)
		{
			base.sm.IsOnline.Set(online, this, false);
			this.RefreshCharge();
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x003094D8 File Offset: 0x003076D8
		public void SetManualDeliveryEnableState(bool enable)
		{
			if (!enable)
			{
				this.manualDelivery.capacity = 0f;
				this.manualDelivery.refillMass = 0f;
				this.manualDelivery.RequestedItemTag = null;
				this.manualDelivery.AbortDelivery("Manual delivery disabled");
				return;
			}
			Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
			GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
			base.smi.storage.capacityKg = (float)this.ElectrobankCountCapacity;
			base.smi.manualDelivery.capacity = 1f;
			base.smi.manualDelivery.refillMass = 1f;
			base.smi.manualDelivery.MinimumMass = 1f;
			this.manualDelivery.ForbiddenTags = array;
			this.manualDelivery.RequestedItemTag = GameTags.ChargedPortableBattery;
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x000EE834 File Offset: 0x000ECA34
		public GameObject GetFirstDischargedElectrobankInInventory()
		{
			return this.storage.FindFirst(GameTags.EmptyPortableBattery);
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x000EE846 File Offset: 0x000ECA46
		public Electrobank GetClosestElectrobank()
		{
			return this.closestElectrobankSensor.GetItem();
		}

		// Token: 0x0600712F RID: 28975 RVA: 0x003095B8 File Offset: 0x003077B8
		public void RefreshCharge()
		{
			List<GameObject> list = new List<GameObject>();
			List<GameObject> list2 = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			this.storage.Find(GameTags.EmptyPortableBattery, list2);
			float num = 0f;
			if (this.IsOnline)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Electrobank component = list[i].GetComponent<Electrobank>();
					num += component.Charge;
				}
			}
			this.BionicBattery.SetValue(num);
			base.sm.ChargedElectrobankCount.Set(list.Count, this, false);
			base.sm.DepletedElectrobankCount.Set(list2.Count, this, false);
			this.UpdateNotifications();
		}

		// Token: 0x06007130 RID: 28976 RVA: 0x00309674 File Offset: 0x00307874
		public void ConsumePower(float joules)
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			float num = joules;
			for (int i = 0; i < list.Count; i++)
			{
				Electrobank component = list[i].GetComponent<Electrobank>();
				float joules2 = Mathf.Min(component.Charge, num);
				float num2 = component.RemovePower(joules2, false);
				num -= num2;
				WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountConsumed(component.ID, num2);
			}
			this.RefreshCharge();
		}

		// Token: 0x06007131 RID: 28977 RVA: 0x003096F0 File Offset: 0x003078F0
		public void DebugAddCharge(float joules)
		{
			float num = MathF.Min(joules, (float)this.ElectrobankCountCapacity * 120000f - this.CurrentCharge);
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.ChargedPortableBattery, list);
			int num2 = 0;
			while (num > 0f && num2 < list.Count)
			{
				Electrobank component = list[num2].GetComponent<Electrobank>();
				float num3 = Mathf.Min(120000f - component.Charge, num);
				component.AddPower(num3);
				num -= num3;
				num2++;
			}
			if (num > 0f && list.Count < this.ElectrobankCountCapacity)
			{
				int num4 = this.storage.items.Count - 1;
				while (num > 0f && num4 >= 0)
				{
					GameObject gameObject = this.storage.items[num4];
					if (!(gameObject == null))
					{
						Electrobank component2 = gameObject.GetComponent<Electrobank>();
						if (component2 == null && gameObject.HasTag(GameTags.EmptyPortableBattery))
						{
							this.storage.Drop(gameObject, true);
							GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
							gameObject2.SetActive(true);
							component2 = gameObject2.GetComponent<Electrobank>();
							float joules2 = Mathf.Clamp(component2.Charge - num, 0f, float.MaxValue);
							component2.RemovePower(joules2, true);
							num -= component2.Charge;
							this.storage.Store(gameObject2, false, false, true, false);
						}
					}
					num4--;
				}
			}
			if (num > 0f && this.storage.items.Count < this.ElectrobankCountCapacity)
			{
				do
				{
					GameObject gameObject3 = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
					gameObject3.SetActive(true);
					Electrobank component3 = gameObject3.GetComponent<Electrobank>();
					float joules3 = Mathf.Clamp(component3.Charge - num, 0f, float.MaxValue);
					component3.RemovePower(joules3, true);
					num -= component3.Charge;
					this.storage.Store(gameObject3, false, false, true, false);
				}
				while (num > 0f && this.storage.items.Count < this.ElectrobankCountCapacity && num > 0f);
			}
			this.RefreshCharge();
		}

		// Token: 0x06007132 RID: 28978 RVA: 0x00309958 File Offset: 0x00307B58
		private void UpdateNotifications()
		{
			this.criticalBatteryStatusItemGuid = this.selectable.ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicCriticalBattery, this.criticalBatteryStatusItemGuid, BionicBatteryMonitor.ChargeIsBelowNotificationThreshold(base.smi) && !this.prefabID.HasTag(GameTags.Incapacitated) && !this.prefabID.HasTag(GameTags.Dead), base.gameObject);
		}

		// Token: 0x06007133 RID: 28979 RVA: 0x003099C8 File Offset: 0x00307BC8
		public bool AddOrUpdateModifier(BionicBatteryMonitor.WattageModifier modifier, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifier.id);
			bool flag;
			if (num >= 0)
			{
				flag = (this.Modifiers[num].name != modifier.name || this.Modifiers[num].value != modifier.value || this.Modifiers[num].potentialValue != modifier.potentialValue);
				this.Modifiers[num] = modifier;
			}
			else
			{
				this.Modifiers.Add(modifier);
				flag = true;
			}
			if (flag)
			{
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
			}
			if (triggerCallbacks)
			{
				base.Trigger(1361471071, this.Wattage);
			}
			return flag;
		}

		// Token: 0x06007134 RID: 28980 RVA: 0x00309AD4 File Offset: 0x00307CD4
		public bool RemoveModifier(string modifierID, bool triggerCallbacks = true)
		{
			int num = this.Modifiers.FindIndex((BionicBatteryMonitor.WattageModifier mod) => mod.id == modifierID);
			if (num >= 0)
			{
				this.Modifiers.RemoveAt(num);
				if (triggerCallbacks)
				{
					base.Trigger(1361471071, this.Wattage);
				}
				this.Modifiers.Sort((BionicBatteryMonitor.WattageModifier a, BionicBatteryMonitor.WattageModifier b) => b.value.CompareTo(a.value));
				return true;
			}
			return false;
		}

		// Token: 0x06007135 RID: 28981 RVA: 0x00252FD8 File Offset: 0x002511D8
		private void OnBatteriesDroppedFromDeath(List<GameObject> items)
		{
			if (items != null)
			{
				for (int i = 0; i < items.Count; i++)
				{
					Electrobank component = items[i].GetComponent<Electrobank>();
					if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
					{
						component.RemovePower(component.Charge, true);
					}
				}
			}
		}

		// Token: 0x06007136 RID: 28982 RVA: 0x00309B5C File Offset: 0x00307D5C
		public void SpawnAndInstallInitialElectrobanks()
		{
			for (int i = 0; i < this.ElectrobankCountCapacity; i++)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), base.transform.position);
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			this.RefreshCharge();
			this.SetOnlineState(true);
			base.sm.InitialElectrobanksSpawned.Set(true, this, false);
		}

		// Token: 0x06007137 RID: 28983 RVA: 0x00309BD4 File Offset: 0x00307DD4
		public void AutomaticallyDropAllDepletedElectrobanks()
		{
			List<GameObject> list = new List<GameObject>();
			this.storage.Find(GameTags.EmptyPortableBattery, list);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject go = list[i];
				this.storage.Drop(go, true);
			}
		}

		// Token: 0x040054FC RID: 21756
		public Storage storage;

		// Token: 0x040054FD RID: 21757
		public KPrefabID prefabID;

		// Token: 0x040054FF RID: 21759
		private Schedulable schedulable;

		// Token: 0x04005500 RID: 21760
		private AmountInstance BionicBattery;

		// Token: 0x04005501 RID: 21761
		private ManualDeliveryKG manualDelivery;

		// Token: 0x04005502 RID: 21762
		private ClosestElectrobankSensor closestElectrobankSensor;

		// Token: 0x04005503 RID: 21763
		private KSelectable selectable;

		// Token: 0x04005504 RID: 21764
		private MinionStorageDataHolder dataHolder;

		// Token: 0x04005505 RID: 21765
		private Guid criticalBatteryStatusItemGuid;

		// Token: 0x04005507 RID: 21767
		private Chore reanimateChore;
	}
}
