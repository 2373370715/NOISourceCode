using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001551 RID: 5457
public class BionicOxygenTankMonitor : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>
{
	// Token: 0x06007195 RID: 29077 RVA: 0x0030A7BC File Offset: 0x003089BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.fistSpawn;
		this.fistSpawn.ParamTransition<bool>(this.HasSpawnedBefore, this.safe, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.IsTrue).Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.StartWithFullTank));
		this.safe.Transition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)), UpdateRate.SIM_200ms);
		this.low.DefaultState(this.low.idle);
		this.low.idle.Transition(this.critical, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical), UpdateRate.SIM_200ms).Transition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe), UpdateRate.SIM_200ms).ScheduleChange(this.low.schedule, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule));
		this.low.schedule.ToggleUrge(Db.Get().Urges.FindOxygenRefill).DefaultState(this.low.schedule.enableSensors).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors));
		this.low.schedule.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.low.schedule.oxygenCanisterMode);
		this.low.schedule.oxygenCanisterMode.DefaultState(this.low.schedule.oxygenCanisterMode.running);
		this.low.schedule.oxygenCanisterMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun)).OnSignal(this.OxygenSourceItemLostSignal, this.low.schedule.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, this.low.schedule.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).Transition(this.critical, (BionicOxygenTankMonitor.Instance smi) => BionicOxygenTankMonitor.AreOxygenLevelsCritical(smi) && !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi), UpdateRate.SIM_200ms).Update(new Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable), UpdateRate.SIM_200ms, false).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master, false), this.low.schedule.oxygenCanisterMode.ends, this.low.schedule.oxygenCanisterMode.ends);
		this.low.schedule.oxygenCanisterMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
		this.low.schedule.environmentAbsorbMode.DefaultState(this.low.schedule.environmentAbsorbMode.running);
		this.low.schedule.environmentAbsorbMode.running.ScheduleChange(this.low.idle, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun)).OnSignal(this.ClosestOxygenSourceChanged, this.low.schedule.oxygenCanisterMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).Transition(this.critical, (BionicOxygenTankMonitor.Instance smi) => BionicOxygenTankMonitor.AreOxygenLevelsCritical(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi), UpdateRate.SIM_200ms).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master, false), this.low.schedule.environmentAbsorbMode.ends, this.low.schedule.environmentAbsorbMode.ends);
		this.low.schedule.environmentAbsorbMode.ends.EnterTransition(this.safe, new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsSafe)).GoTo(this.low.idle);
		this.critical.ToggleUrge(Db.Get().Urges.FindOxygenRefill).Exit(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.DisableOxygenSourceSensors)).DefaultState(this.critical.enableSensors).ToggleExpression(Db.Get().Expressions.RecoverBreath, null).Update(delegate(BionicOxygenTankMonitor.Instance smi, float dt)
		{
			if (smi.master.gameObject.GetAmounts().Get("Breath").value <= DUPLICANTSTATS.BIONICS.Breath.SUFFOCATE_AMOUNT)
			{
				smi.isRecoveringFromSuffocation = true;
			}
		}, UpdateRate.SIM_200ms, false).Exit(delegate(BionicOxygenTankMonitor.Instance smi)
		{
			smi.isRecoveringFromSuffocation = false;
		});
		this.critical.enableSensors.Enter(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State.Callback(BionicOxygenTankMonitor.EnableOxygenSourceSensors)).GoTo(this.critical.oxygenCanisterMode);
		this.critical.oxygenCanisterMode.DefaultState(this.critical.oxygenCanisterMode.running);
		this.critical.oxygenCanisterMode.running.OnSignal(this.ClosestOxygenSourceChanged, this.critical.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).OnSignal(this.OxygenSourceItemLostSignal, this.critical.environmentAbsorbMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable)).OnSignal(this.AbsorbCellChangedSignal, this.critical.environmentAbsorbMode, (BionicOxygenTankMonitor.Instance smi) => !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi) && BionicOxygenTankMonitor.NoOxygenSourceAvailableButAbsorbCellAvailable(smi)).Update(new Action<BionicOxygenTankMonitor.Instance, float>(BionicOxygenTankMonitor.UpdateAbsorbCellIfNoOxygenSourceAvailable), UpdateRate.SIM_200ms, false).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new FindAndConsumeOxygenSourceChore(smi.master, true), this.critical.oxygenCanisterMode.ends, this.critical.oxygenCanisterMode.ends);
		this.critical.oxygenCanisterMode.ends.EnterTransition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.oxygenCanisterMode.running);
		this.critical.environmentAbsorbMode.DefaultState(this.critical.environmentAbsorbMode.running);
		this.critical.environmentAbsorbMode.running.OnSignal(this.ClosestOxygenSourceChanged, this.critical.oxygenCanisterMode, new Func<BionicOxygenTankMonitor.Instance, bool>(BionicOxygenTankMonitor.OxygenSourceItemAvailableAndAbsorbChoreNotStarted)).ToggleChore((BionicOxygenTankMonitor.Instance smi) => new BionicMassOxygenAbsorbChore(smi.master, true), this.critical.environmentAbsorbMode.ends, this.critical.environmentAbsorbMode.ends);
		this.critical.environmentAbsorbMode.ends.EnterTransition(this.low, GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Not(new StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Transition.ConditionCallback(BionicOxygenTankMonitor.AreOxygenLevelsCritical))).GoTo(this.critical.oxygenCanisterMode);
	}

	// Token: 0x06007196 RID: 29078 RVA: 0x000EECB0 File Offset: 0x000ECEB0
	public static bool IsAllowedToSeekOxygenBySchedule(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.IsAllowedToSeekOxygenBySchedule;
	}

	// Token: 0x06007197 RID: 29079 RVA: 0x000EECB8 File Offset: 0x000ECEB8
	public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndSeekChoreHasNotBegun(BionicOxygenTankMonitor.Instance smi)
	{
		return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.FindOxygenSourceChoreIsRunning(smi);
	}

	// Token: 0x06007198 RID: 29080 RVA: 0x000EECCD File Offset: 0x000ECECD
	public static bool IsNotAllowedToSeekOxygenSourceItemsByScheduleAndAbsorbChoreHasNotBegun(BionicOxygenTankMonitor.Instance smi)
	{
		return !BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
	}

	// Token: 0x06007199 RID: 29081 RVA: 0x000EECE2 File Offset: 0x000ECEE2
	public static bool AreOxygenLevelsSafe(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage >= 0.85f;
	}

	// Token: 0x0600719A RID: 29082 RVA: 0x000EECF4 File Offset: 0x000ECEF4
	public static bool AreOxygenLevelsCritical(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.OxygenPercentage <= 0f;
	}

	// Token: 0x0600719B RID: 29083 RVA: 0x000EED06 File Offset: 0x000ECF06
	public static bool IsThereAnOxygenSourceItemAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() != null;
	}

	// Token: 0x0600719C RID: 29084 RVA: 0x000EED14 File Offset: 0x000ECF14
	public static bool AbsorbCellUnavailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.AbsorbOxygenCell == Grid.InvalidCell;
	}

	// Token: 0x0600719D RID: 29085 RVA: 0x000EED23 File Offset: 0x000ECF23
	public static bool AbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.AbsorbOxygenCell != Grid.InvalidCell;
	}

	// Token: 0x0600719E RID: 29086 RVA: 0x000EED35 File Offset: 0x000ECF35
	public static bool NoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return smi.GetClosestOxygenSource() == null;
	}

	// Token: 0x0600719F RID: 29087 RVA: 0x000EED43 File Offset: 0x000ECF43
	public static bool NoOxygenSourceAvailableButAbsorbCellAvailable(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi) && BionicOxygenTankMonitor.AbsorbCellAvailable(smi);
	}

	// Token: 0x060071A0 RID: 29088 RVA: 0x000EED55 File Offset: 0x000ECF55
	public static bool OxygenSourceItemAvailableAndAbsorbChoreNotStarted(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.IsThereAnOxygenSourceItemAvailable(smi) && !BionicOxygenTankMonitor.AbsorbChoreIsRunning(smi);
	}

	// Token: 0x060071A1 RID: 29089 RVA: 0x000EED6A File Offset: 0x000ECF6A
	public static bool AbsorbChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical);
	}

	// Token: 0x060071A2 RID: 29090 RVA: 0x000EED9A File Offset: 0x000ECF9A
	public static bool FindOxygenSourceChoreIsRunning(BionicOxygenTankMonitor.Instance smi)
	{
		return BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem) || BionicOxygenTankMonitor.ChoreIsRunning(smi, Db.Get().ChoreTypes.FindOxygenSourceItem_Critical);
	}

	// Token: 0x060071A3 RID: 29091 RVA: 0x0030AECC File Offset: 0x003090CC
	public static bool ChoreIsRunning(BionicOxygenTankMonitor.Instance smi, ChoreType type)
	{
		ChoreDriver component = smi.GetComponent<ChoreDriver>();
		Chore chore = (component == null) ? null : component.GetCurrentChore();
		return chore != null && chore.choreType == type;
	}

	// Token: 0x060071A4 RID: 29092 RVA: 0x000EEDCA File Offset: 0x000ECFCA
	public static void StartWithFullTank(BionicOxygenTankMonitor.Instance smi)
	{
		smi.AddFirstTimeSpawnedOxygen();
	}

	// Token: 0x060071A5 RID: 29093 RVA: 0x000EEDD2 File Offset: 0x000ECFD2
	public static void EnableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(true);
	}

	// Token: 0x060071A6 RID: 29094 RVA: 0x000EEDDB File Offset: 0x000ECFDB
	public static void DisableOxygenSourceSensors(BionicOxygenTankMonitor.Instance smi)
	{
		smi.SetOxygenSourceSensorsActiveState(false);
	}

	// Token: 0x060071A7 RID: 29095 RVA: 0x000EEDE4 File Offset: 0x000ECFE4
	public static void UpdateAbsorbCellIfNoOxygenSourceAvailable(BionicOxygenTankMonitor.Instance smi, float dt)
	{
		if (BionicOxygenTankMonitor.NoOxygenSourceAvailable(smi))
		{
			smi.UpdatePotentialCellToAbsorbOxygen(Grid.InvalidCell);
		}
	}

	// Token: 0x04005549 RID: 21833
	public const SimHashes INITIAL_TANK_ELEMENT = SimHashes.Oxygen;

	// Token: 0x0400554A RID: 21834
	public static readonly Tag INITIAL_TANK_ELEMENT_TAG = SimHashes.Oxygen.CreateTag();

	// Token: 0x0400554B RID: 21835
	public const float SAFE_TRESHOLD = 0.85f;

	// Token: 0x0400554C RID: 21836
	public const float CRITICAL_TRESHOLD = 0f;

	// Token: 0x0400554D RID: 21837
	public const float OXYGEN_TANK_CAPACITY_IN_SECONDS = 2400f;

	// Token: 0x0400554E RID: 21838
	public static readonly float OXYGEN_TANK_CAPACITY_KG = 2400f * DUPLICANTSTATS.BIONICS.BaseStats.OXYGEN_USED_PER_SECOND;

	// Token: 0x0400554F RID: 21839
	public static float INITIAL_OXYGEN_TEMP = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;

	// Token: 0x04005550 RID: 21840
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State fistSpawn;

	// Token: 0x04005551 RID: 21841
	public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State safe;

	// Token: 0x04005552 RID: 21842
	public BionicOxygenTankMonitor.LowOxygenStates low;

	// Token: 0x04005553 RID: 21843
	public BionicOxygenTankMonitor.SeekOxygenStates critical;

	// Token: 0x04005554 RID: 21844
	private StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.BoolParameter HasSpawnedBefore;

	// Token: 0x04005555 RID: 21845
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal AbsorbCellChangedSignal;

	// Token: 0x04005556 RID: 21846
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal OxygenSourceItemLostSignal;

	// Token: 0x04005557 RID: 21847
	public StateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.Signal ClosestOxygenSourceChanged;

	// Token: 0x02001552 RID: 5458
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001553 RID: 5459
	public class ChoreState : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x04005558 RID: 21848
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State running;

		// Token: 0x04005559 RID: 21849
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State ends;
	}

	// Token: 0x02001554 RID: 5460
	public class SeekOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x0400555A RID: 21850
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State enableSensors;

		// Token: 0x0400555B RID: 21851
		public BionicOxygenTankMonitor.ChoreState oxygenCanisterMode;

		// Token: 0x0400555C RID: 21852
		public BionicOxygenTankMonitor.ChoreState environmentAbsorbMode;
	}

	// Token: 0x02001555 RID: 5461
	public class LowOxygenStates : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State
	{
		// Token: 0x0400555D RID: 21853
		public GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.State idle;

		// Token: 0x0400555E RID: 21854
		public BionicOxygenTankMonitor.SeekOxygenStates schedule;
	}

	// Token: 0x02001556 RID: 5462
	public new class Instance : GameStateMachine<BionicOxygenTankMonitor, BionicOxygenTankMonitor.Instance, IStateMachineTarget, BionicOxygenTankMonitor.Def>.GameInstance, OxygenBreather.IGasProvider
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060071AE RID: 29102 RVA: 0x000EEE09 File Offset: 0x000ED009
		public bool IsAllowedToSeekOxygenBySchedule
		{
			get
			{
				return ScheduleManager.Instance.IsAllowed(this.schedulable, Db.Get().ScheduleBlockTypes.Eat);
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060071AF RID: 29103 RVA: 0x000EEE2A File Offset: 0x000ED02A
		public bool IsEmpty
		{
			get
			{
				return this.AvailableOxygen == 0f;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060071B0 RID: 29104 RVA: 0x000EEE39 File Offset: 0x000ED039
		public float OxygenPercentage
		{
			get
			{
				return this.AvailableOxygen / this.storage.capacityKg;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060071B1 RID: 29105 RVA: 0x000EEE4D File Offset: 0x000ED04D
		public float AvailableOxygen
		{
			get
			{
				return this.storage.GetMassAvailable(GameTags.Breathable);
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060071B2 RID: 29106 RVA: 0x000EEE5F File Offset: 0x000ED05F
		public float SpaceAvailableInTank
		{
			get
			{
				return this.storage.capacityKg - this.AvailableOxygen;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060071B4 RID: 29108 RVA: 0x000EEE7C File Offset: 0x000ED07C
		// (set) Token: 0x060071B3 RID: 29107 RVA: 0x000EEE73 File Offset: 0x000ED073
		public int AbsorbOxygenCell { get; private set; } = Grid.InvalidCell;

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060071B6 RID: 29110 RVA: 0x000EEE8D File Offset: 0x000ED08D
		// (set) Token: 0x060071B5 RID: 29109 RVA: 0x000EEE84 File Offset: 0x000ED084
		public Storage storage { get; private set; }

		// Token: 0x060071B7 RID: 29111 RVA: 0x0030AF54 File Offset: 0x00309154
		public Instance(IStateMachineTarget master, BionicOxygenTankMonitor.Def def) : base(master, def)
		{
			this.query = new AbsorbCellQuery();
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
			Sensors component = base.GetComponent<Sensors>();
			this.schedulable = base.GetComponent<Schedulable>();
			this.navigator = base.GetComponent<Navigator>();
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
			this.brain = base.GetComponent<MinionBrain>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.oxygenSourceSensors = new ClosestPickupableSensor<Pickupable>[]
			{
				component.GetSensor<ClosestOxygenCanisterSensor>()
			};
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.OnItemChanged = (Action<Pickupable>)Delegate.Combine(closestPickupableSensor.OnItemChanged, new Action<Pickupable>(this.OnOxygenSourceSensorItemChanged));
			}
			this.storage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicOxygenTankStorage);
			this.oxygenTankAmountInstance = Db.Get().Amounts.BionicOxygenTank.Lookup(base.gameObject);
			this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(base.gameObject);
			Storage storage = this.storage;
			storage.OnStorageChange = (Action<GameObject>)Delegate.Combine(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x000EEE95 File Offset: 0x000ED095
		public Pickupable GetClosestOxygenSource()
		{
			return this.closestOxygenSource;
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x000EEE9D File Offset: 0x000ED09D
		private void OnOxygenSourceSensorItemChanged(object o)
		{
			this.CompareOxygenSources();
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x000EEEA5 File Offset: 0x000ED0A5
		private void OnOxygenTankStorageChanged(object o)
		{
			this.RefreshAmountInstance();
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x000EEEAD File Offset: 0x000ED0AD
		public void RefreshAmountInstance()
		{
			this.oxygenTankAmountInstance.SetValue(this.AvailableOxygen);
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x0030B0E8 File Offset: 0x003092E8
		public void AddFirstTimeSpawnedOxygen()
		{
			this.storage.AddElement(SimHashes.Oxygen, this.storage.capacityKg - this.AvailableOxygen, BionicOxygenTankMonitor.INITIAL_OXYGEN_TEMP, byte.MaxValue, 0, false, true);
			base.sm.HasSpawnedBefore.Set(true, this, false);
		}

		// Token: 0x060071BD RID: 29117 RVA: 0x0030B13C File Offset: 0x0030933C
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					base.sm.HasSpawnedBefore.Get(this)
				}
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x060071BE RID: 29118 RVA: 0x000EEEC1 File Offset: 0x000ED0C1
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshAmountInstance();
		}

		// Token: 0x060071BF RID: 29119 RVA: 0x0030B180 File Offset: 0x00309380
		public override void OnParamsDeserialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicOxygenTankMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					bool value = dataPackData.Bools[0];
					base.sm.HasSpawnedBefore.Set(value, this, false);
				}
			}
			base.OnParamsDeserialized();
		}

		// Token: 0x060071C0 RID: 29120 RVA: 0x0030B1D4 File Offset: 0x003093D4
		private void CompareOxygenSources()
		{
			Pickupable x = null;
			float num = 2.1474836E+09f;
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				int itemNavCost = closestPickupableSensor.GetItemNavCost();
				if ((float)itemNavCost < num)
				{
					num = (float)itemNavCost;
					x = closestPickupableSensor.GetItem();
				}
			}
			bool flag = x != this.closestOxygenSource;
			this.closestOxygenSource = x;
			if (flag)
			{
				base.sm.ClosestOxygenSourceChanged.Trigger(this);
			}
		}

		// Token: 0x060071C1 RID: 29121 RVA: 0x0030B244 File Offset: 0x00309444
		public void UpdatePotentialCellToAbsorbOxygen(int previouslyReservedCell)
		{
			float breathPercentage = this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).value / this.brain.GetAmounts().Get(Db.Get().Amounts.Breath).GetMax();
			this.query.Reset(this.brain, BionicOxygenTankMonitor.AreOxygenLevelsCritical(this), this.AvailableOxygen, breathPercentage, previouslyReservedCell, this.isRecoveringFromSuffocation);
			this.navigator.RunQuery(base.smi.query);
			int num = base.smi.query.GetResultCell();
			if (num == Grid.PosToCell(base.gameObject) && !GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(num, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather).IsBreathable)
			{
				num = PathFinder.InvalidCell;
			}
			bool flag = this.AbsorbOxygenCell != num;
			this.AbsorbOxygenCell = num;
			if (flag)
			{
				base.sm.AbsorbCellChangedSignal.Trigger(this);
			}
		}

		// Token: 0x060071C2 RID: 29122 RVA: 0x000EEECF File Offset: 0x000ED0CF
		public float AddGas(Sim.MassConsumedCallback mass_cb_info)
		{
			return this.AddGas(ElementLoader.elements[(int)mass_cb_info.elemIdx].id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
		}

		// Token: 0x060071C3 RID: 29123 RVA: 0x0030B33C File Offset: 0x0030953C
		public float AddGas(SimHashes element, float mass, float temperature, byte disseaseIDX = 255, int _disseaseCount = 0)
		{
			float num = Mathf.Min(mass, this.SpaceAvailableInTank);
			float result = mass - num;
			float num2 = num / mass;
			int disease_count = Mathf.CeilToInt((float)_disseaseCount * num2);
			this.storage.AddElement(element, num, temperature, disseaseIDX, disease_count, false, true);
			return result;
		}

		// Token: 0x060071C4 RID: 29124 RVA: 0x0030B37C File Offset: 0x0030957C
		public void SetOxygenSourceSensorsActiveState(bool shouldItBeActive)
		{
			for (int i = 0; i < this.oxygenSourceSensors.Length; i++)
			{
				ClosestPickupableSensor<Pickupable> closestPickupableSensor = this.oxygenSourceSensors[i];
				closestPickupableSensor.SetActive(shouldItBeActive);
				if (shouldItBeActive)
				{
					closestPickupableSensor.Update();
				}
			}
		}

		// Token: 0x060071C5 RID: 29125 RVA: 0x0030B3B8 File Offset: 0x003095B8
		public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
		{
			if (this.IsEmpty)
			{
				return false;
			}
			SimHashes elementConsumed = SimHashes.Vacuum;
			float temperature = 0f;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			this.storage.ConsumeAndGetDisease(GameTags.Breathable, amount, out num, out diseaseInfo, out temperature, out elementConsumed);
			OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, amount, temperature, diseaseInfo.idx, diseaseInfo.count);
			return true;
		}

		// Token: 0x060071C6 RID: 29126 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x060071C7 RID: 29127 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
		{
		}

		// Token: 0x060071C8 RID: 29128 RVA: 0x000EEF04 File Offset: 0x000ED104
		public bool IsLowOxygen()
		{
			return this.OxygenPercentage <= 0f;
		}

		// Token: 0x060071C9 RID: 29129 RVA: 0x000EEF16 File Offset: 0x000ED116
		public bool HasOxygen()
		{
			return !this.IsEmpty;
		}

		// Token: 0x060071CA RID: 29130 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool IsBlocked()
		{
			return false;
		}

		// Token: 0x060071CB RID: 29131 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldEmitCO2()
		{
			return false;
		}

		// Token: 0x060071CC RID: 29132 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool ShouldStoreCO2()
		{
			return false;
		}

		// Token: 0x060071CD RID: 29133 RVA: 0x0030B40C File Offset: 0x0030960C
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			if (this.storage != null)
			{
				Storage storage = this.storage;
				storage.OnStorageChange = (Action<GameObject>)Delegate.Remove(storage.OnStorageChange, new Action<GameObject>(this.OnOxygenTankStorageChanged));
			}
			base.OnCleanUp();
		}

		// Token: 0x0400555F RID: 21855
		public AttributeInstance airConsumptionRate;

		// Token: 0x04005560 RID: 21856
		private Schedulable schedulable;

		// Token: 0x04005561 RID: 21857
		private AmountInstance oxygenTankAmountInstance;

		// Token: 0x04005562 RID: 21858
		private ClosestPickupableSensor<Pickupable>[] oxygenSourceSensors;

		// Token: 0x04005563 RID: 21859
		private Pickupable closestOxygenSource;

		// Token: 0x04005564 RID: 21860
		private Navigator navigator;

		// Token: 0x04005565 RID: 21861
		private AbsorbCellQuery query;

		// Token: 0x04005566 RID: 21862
		private OxygenBreather oxygenBreather;

		// Token: 0x04005567 RID: 21863
		private MinionBrain brain;

		// Token: 0x04005568 RID: 21864
		private MinionStorageDataHolder dataHolder;

		// Token: 0x04005569 RID: 21865
		public bool isRecoveringFromSuffocation;
	}
}
