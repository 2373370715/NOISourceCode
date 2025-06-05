using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000692 RID: 1682
public class BionicMassOxygenAbsorbChore : Chore<BionicMassOxygenAbsorbChore.Instance>
{
	// Token: 0x06001DD8 RID: 7640 RVA: 0x001BCBF0 File Offset: 0x001BADF0
	public BionicMassOxygenAbsorbChore(IStateMachineTarget target, bool critical) : base(critical ? Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical : Db.Get().ChoreTypes.BionicAbsorbOxygen, target, target.GetComponent<ChoreProvider>(), false, null, null, null, critical ? PriorityScreen.PriorityClass.compulsory : PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BionicMassOxygenAbsorbChore.Instance(this, target.gameObject);
		Func<int> data = new Func<int>(base.smi.UpdateTargetCell);
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveToDynamicCellUntilBegun, data);
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x001BCC88 File Offset: 0x001BAE88
	public override string ResolveString(string str)
	{
		float mass = (base.smi == null) ? 0f : base.smi.GetAverageMassConsumedPerSecond();
		return string.Format(base.ResolveString(str), GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x001BCCCC File Offset: 0x001BAECC
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null context.consumer");
			return;
		}
		if (context.consumerState.consumer.GetSMI<BionicOxygenTankMonitor.Instance>() == null)
		{
			global::Debug.LogError("BionicMassAbsorbOxygenChore null BionicOxygenTankMonitor.Instance");
			return;
		}
		base.smi.ResetMassTrackHistory();
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x000B8290 File Offset: 0x000B6490
	public static bool IsNotAllowedByScheduleAndChoreIsNotCritical(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return !BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && !BionicMassOxygenAbsorbChore.IsAllowedBySchedule(smi);
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000B82A5 File Offset: 0x000B64A5
	public static bool IsAllowedBySchedule(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return BionicOxygenTankMonitor.IsAllowedToSeekOxygenBySchedule(smi.oxygenTankMonitor);
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x000B82B2 File Offset: 0x000B64B2
	public static bool IsCriticalChore(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.master.choreType == Db.Get().ChoreTypes.BionicAbsorbOxygen_Critical;
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000B82D0 File Offset: 0x000B64D0
	public static void ResetOxygenTimer(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.sm.SecondsPassedWithoutOxygen.Set(0f, smi, false);
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x000B82EA File Offset: 0x000B64EA
	public static void RefreshTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi)
	{
		smi.UpdateTargetCell();
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x000B82F3 File Offset: 0x000B64F3
	public static void UpdateTargetSafeCell(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x000B82FB File Offset: 0x000B64FB
	public static bool HasSpaceInOxygenTank(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return smi.oxygenTankMonitor.SpaceAvailableInTank > 0f;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x000B830F File Offset: 0x000B650F
	public static bool ChoreIsCriticalModeAndGiveUpOxygenLevelReached(BionicMassOxygenAbsorbChore.Instance smi)
	{
		return BionicMassOxygenAbsorbChore.IsCriticalChore(smi) && smi.oxygenTankMonitor.OxygenPercentage >= 0.25f;
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x001BCD4C File Offset: 0x001BAF4C
	public static bool BreathIsFull(BionicMassOxygenAbsorbChore.Instance smi)
	{
		AmountInstance amountInstance = smi.gameObject.GetAmounts().Get(Db.Get().Amounts.Breath);
		return amountInstance.value >= amountInstance.GetMax();
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x000B8330 File Offset: 0x000B6530
	public static void UpdateTargetSafeCellOnlyInCriticalMode(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		if (BionicMassOxygenAbsorbChore.IsCriticalChore(smi))
		{
			BionicMassOxygenAbsorbChore.RefreshTargetSafeCell(smi);
		}
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x001BCD8C File Offset: 0x001BAF8C
	public static void AbsorbUpdate(BionicMassOxygenAbsorbChore.Instance smi, float dt)
	{
		float mass = Mathf.Min(dt * BionicMassOxygenAbsorbChore.ABSORB_RATE, smi.oxygenTankMonitor.SpaceAvailableInTank);
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = new BionicMassOxygenAbsorbChore.AbsorbUpdateData(smi, dt);
		int gameCell;
		SimHashes nearBreathableElement = BionicMassOxygenAbsorbChore.GetNearBreathableElement(gameCell = Grid.PosToCell(smi.sm.dupe.Get(smi)), BionicMassOxygenAbsorbChore.ABSORB_RANGE, out gameCell);
		HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(BionicMassOxygenAbsorbChore.OnSimConsumeCallback), absorbUpdateData, "BionicMassOxygenAbsorbChore");
		SimMessages.ConsumeMass(gameCell, nearBreathableElement, mass, 6, handle.index);
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x001BCE18 File Offset: 0x001BB018
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		BionicMassOxygenAbsorbChore.AbsorbUpdateData absorbUpdateData = (BionicMassOxygenAbsorbChore.AbsorbUpdateData)data;
		absorbUpdateData.smi.OnSimConsume(mass_cb_info, absorbUpdateData.dt);
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x000B8340 File Offset: 0x000B6540
	private static void ShowOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, new Func<float>(smi.GetOxygen), true);
		}
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x000B836C File Offset: 0x000B656C
	private static void HideOxygenBar(BionicMassOxygenAbsorbChore.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBionicOxygenTankDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x06001DE9 RID: 7657 RVA: 0x001BCE40 File Offset: 0x001BB040
	public static SimHashes GetNearBreathableElement(int centralCell, CellOffset[] range, out int elementCell)
	{
		float num = 0f;
		int num2 = centralCell;
		SimHashes simHashes = SimHashes.Vacuum;
		foreach (CellOffset offset in range)
		{
			int num3 = Grid.OffsetCell(centralCell, offset);
			SimHashes simHashes2 = SimHashes.Vacuum;
			float breathableMassInCell = BionicMassOxygenAbsorbChore.GetBreathableMassInCell(num3, out simHashes2);
			if (breathableMassInCell > Mathf.Epsilon && (simHashes == SimHashes.Vacuum || breathableMassInCell > num))
			{
				simHashes = simHashes2;
				num = breathableMassInCell;
				num2 = num3;
			}
		}
		elementCell = num2;
		return simHashes;
	}

	// Token: 0x06001DEA RID: 7658 RVA: 0x001BCEB8 File Offset: 0x001BB0B8
	private static float GetBreathableMassInCell(int cell, out SimHashes elementID)
	{
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		elementID = SimHashes.Vacuum;
		return 0f;
	}

	// Token: 0x04001318 RID: 4888
	public static CellOffset[] ABSORB_RANGE = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04001319 RID: 4889
	public const float ABSORB_RATE_IDEAL_CHORE_DURATION = 30f;

	// Token: 0x0400131A RID: 4890
	public static readonly float ABSORB_RATE = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG / 30f;

	// Token: 0x0400131B RID: 4891
	public const int HISTORY_ROW_COUNT = 15;

	// Token: 0x0400131C RID: 4892
	public const float LOW_OXYGEN_TRESHOLD = 2f;

	// Token: 0x0400131D RID: 4893
	public const float GIVE_UP_DURATION_CRTICIAL_MODE = 2f;

	// Token: 0x0400131E RID: 4894
	public const float GIVE_UP_DURATION_LOW_OXYGEN_MODE = 4f;

	// Token: 0x0400131F RID: 4895
	public const float CRITICAL_CHORE_GIVE_UP_OXYGEN_LEVEL_TRESHOLD = 0.25f;

	// Token: 0x04001320 RID: 4896
	public const string ABSORB_ANIM_FILE = "anim_bionic_absorb_kanim";

	// Token: 0x04001321 RID: 4897
	public const string ABSORB_PRE_ANIM_NAME = "absorb_pre";

	// Token: 0x04001322 RID: 4898
	public const string ABSORB_LOOP_ANIM_NAME = "absorb_loop";

	// Token: 0x04001323 RID: 4899
	public const string ABSORB_PST_ANIM_NAME = "absorb_pst";

	// Token: 0x04001324 RID: 4900
	public static CellOffset MouthCellOffset = new CellOffset(0, 1);

	// Token: 0x02000693 RID: 1683
	public class States : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore>
	{
		// Token: 0x06001DEC RID: 7660 RVA: 0x001BCF8C File Offset: 0x001BB18C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.dupe);
			this.root.Exit(delegate(BionicMassOxygenAbsorbChore.Instance smi)
			{
				smi.ChangeCellReservation(Grid.InvalidCell);
			});
			this.move.DefaultState(this.move.onGoing).ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical));
			this.move.onGoing.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.RefreshTargetSafeCell)).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.UpdateTargetSafeCellOnlyInCriticalMode), UpdateRate.RENDER_1000ms, false).MoveTo((BionicMassOxygenAbsorbChore.Instance smi) => smi.targetCell, this.absorb, this.move.fail, true);
			this.move.fail.ReturnFailure();
			this.absorb.ScheduleChange(this.fail, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsNotAllowedByScheduleAndChoreIsNotCritical)).ToggleTag(GameTags.RecoveringBreath).ToggleAnims("anim_bionic_absorb_kanim", 0f).Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ShowOxygenBar)).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.HideOxygenBar)).DefaultState(this.absorb.pre);
			this.absorb.pre.PlayAnim("absorb_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.absorb.loop).ScheduleGoTo(3f, this.absorb.loop).Exit(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer));
			this.absorb.loop.Enter(new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State.Callback(BionicMassOxygenAbsorbChore.ResetOxygenTimer)).ParamTransition<float>(this.SecondsPassedWithoutOxygen, this.absorb.pst, (BionicMassOxygenAbsorbChore.Instance smi, float secondsPassed) => secondsPassed > smi.GetGiveupTimerTimeout()).OnSignal(this.TankFilledSignal, this.absorb.pst).PlayAnim("absorb_loop", KAnim.PlayMode.Loop).Update(new Action<BionicMassOxygenAbsorbChore.Instance, float>(BionicMassOxygenAbsorbChore.AbsorbUpdate), UpdateRate.SIM_200ms, false).Transition(this.absorb.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.ChoreIsCriticalModeAndGiveUpOxygenLevelReached), UpdateRate.SIM_200ms);
			this.absorb.pst.Transition(this.absorb.criticalRecoverBreath.pre, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.IsCriticalChore), UpdateRate.SIM_200ms).PlayAnim("absorb_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.absorb.criticalRecoverBreath.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.absorb.criticalRecoverBreath.pre);
			this.absorb.criticalRecoverBreath.pre.PlayAnim("breathe_pre").QueueAnim("breathe_loop", false, null).OnAnimQueueComplete(this.absorb.criticalRecoverBreath.loop);
			this.absorb.criticalRecoverBreath.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop).ToggleAttributeModifier("Recovering Breath", (BionicMassOxygenAbsorbChore.Instance smi) => smi.recoveringbreath, null).Transition(this.absorb.criticalRecoverBreath.pst, new StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Transition.ConditionCallback(BionicMassOxygenAbsorbChore.BreathIsFull), UpdateRate.SIM_200ms).Transition(this.absorb.criticalRecoverBreath.pst, (BionicMassOxygenAbsorbChore.Instance smi) => smi.UpdateTargetCell() == Grid.InvalidCell, UpdateRate.SIM_200ms);
			this.absorb.criticalRecoverBreath.pst.PlayAnim("breathe_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.complete).ScheduleGoTo(3f, this.complete);
			this.fail.ReturnFailure();
			this.complete.ReturnSuccess();
		}

		// Token: 0x04001325 RID: 4901
		public BionicMassOxygenAbsorbChore.States.MoveStates move;

		// Token: 0x04001326 RID: 4902
		public BionicMassOxygenAbsorbChore.States.MassAbsorbStates absorb;

		// Token: 0x04001327 RID: 4903
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;

		// Token: 0x04001328 RID: 4904
		public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State complete;

		// Token: 0x04001329 RID: 4905
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.FloatParameter SecondsPassedWithoutOxygen;

		// Token: 0x0400132A RID: 4906
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.TargetParameter dupe;

		// Token: 0x0400132B RID: 4907
		public StateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.Signal TankFilledSignal;

		// Token: 0x02000694 RID: 1684
		public class MoveStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x0400132C RID: 4908
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State onGoing;

			// Token: 0x0400132D RID: 4909
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State fail;
		}

		// Token: 0x02000695 RID: 1685
		public class MassAbsorbStates : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
		{
			// Token: 0x0400132E RID: 4910
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

			// Token: 0x0400132F RID: 4911
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

			// Token: 0x04001330 RID: 4912
			public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;

			// Token: 0x04001331 RID: 4913
			public BionicMassOxygenAbsorbChore.States.MassAbsorbStates.CriticalRecover criticalRecoverBreath;

			// Token: 0x02000696 RID: 1686
			public class CriticalRecover : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State
			{
				// Token: 0x04001332 RID: 4914
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pre;

				// Token: 0x04001333 RID: 4915
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State loop;

				// Token: 0x04001334 RID: 4916
				public GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.State pst;
			}
		}
	}

	// Token: 0x02000698 RID: 1688
	public struct AbsorbUpdateData
	{
		// Token: 0x06001DF8 RID: 7672 RVA: 0x000B83E0 File Offset: 0x000B65E0
		public AbsorbUpdateData(BionicMassOxygenAbsorbChore.Instance smi, float dt)
		{
			this.smi = smi;
			this.dt = dt;
		}

		// Token: 0x0400133B RID: 4923
		public BionicMassOxygenAbsorbChore.Instance smi;

		// Token: 0x0400133C RID: 4924
		public float dt;
	}

	// Token: 0x02000699 RID: 1689
	public class Instance : GameStateMachine<BionicMassOxygenAbsorbChore.States, BionicMassOxygenAbsorbChore.Instance, BionicMassOxygenAbsorbChore, object>.GameInstance
	{
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x000B83F0 File Offset: 0x000B65F0
		public float CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD
		{
			get
			{
				return this.oxygenBreather.ConsumptionRate * 8f;
			}
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x000B8403 File Offset: 0x000B6603
		public float GetGiveupTimerTimeout()
		{
			if (this.oxygenTankMonitor == null)
			{
				return 2f;
			}
			if (!BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor))
			{
				return 4f;
			}
			return 2f;
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x000B8434 File Offset: 0x000B6634
		// (set) Token: 0x06001DFB RID: 7675 RVA: 0x000B842B File Offset: 0x000B662B
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06001DFD RID: 7677 RVA: 0x001BD384 File Offset: 0x001BB584
		public Instance(BionicMassOxygenAbsorbChore master, GameObject duplicant) : base(master)
		{
			base.sm.dupe.Set(duplicant, base.smi, false);
			this.oxygenTankMonitor = duplicant.GetSMI<BionicOxygenTankMonitor.Instance>();
			this.oxygenBreather = duplicant.GetComponent<OxygenBreather>();
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float recover_BREATH_DELTA = DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, recover_BREATH_DELTA, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x001BD424 File Offset: 0x001BB624
		public void ChangeCellReservation(int newCell)
		{
			if (this.targetCell != Grid.InvalidCell && Grid.Reserved[this.targetCell])
			{
				Grid.Reserved[this.targetCell] = false;
			}
			if (newCell != Grid.InvalidCell && !Grid.Reserved[newCell])
			{
				Grid.Reserved[newCell] = true;
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x000B843C File Offset: 0x000B663C
		public override void StopSM(string reason)
		{
			this.ChangeCellReservation(Grid.InvalidCell);
			base.StopSM(reason);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x001BD484 File Offset: 0x001BB684
		public int UpdateTargetCell()
		{
			this.oxygenTankMonitor.UpdatePotentialCellToAbsorbOxygen(this.targetCell);
			int absorbOxygenCell = this.oxygenTankMonitor.AbsorbOxygenCell;
			this.ChangeCellReservation(absorbOxygenCell);
			this.targetCell = absorbOxygenCell;
			return absorbOxygenCell;
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x001BD4C0 File Offset: 0x001BB6C0
		public void ResetMassTrackHistory()
		{
			this.massAbsorbedHistory.Clear();
			for (int i = 0; i < 15; i++)
			{
				this.massAbsorbedHistory.Enqueue(0f);
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x000B8450 File Offset: 0x000B6650
		public void AddMassToHistory(float mass_rate_this_tick)
		{
			if (this.massAbsorbedHistory.Count == 15)
			{
				this.massAbsorbedHistory.Dequeue();
			}
			this.massAbsorbedHistory.Enqueue(mass_rate_this_tick);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x001BD4F8 File Offset: 0x001BB6F8
		public float GetAverageMassConsumedPerSecond()
		{
			float num = 0f;
			int num2 = 0;
			foreach (float num3 in this.massAbsorbedHistory)
			{
				num += num3;
				num2++;
			}
			if (num2 <= 0)
			{
				return 0f;
			}
			num /= (float)num2;
			return num;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x001BD564 File Offset: 0x001BB764
		public void OnSimConsume(Sim.MassConsumedCallback mass_cb_info, float dt)
		{
			if (this.oxygenBreather == null || this.oxygenTankMonitor == null || this.oxygenBreather.prefabID.HasTag(GameTags.Dead))
			{
				return;
			}
			this.AddMassToHistory(mass_cb_info.mass / dt);
			GameObject gameObject = this.oxygenBreather.gameObject;
			bool flag = BionicOxygenTankMonitor.AreOxygenLevelsCritical(this.oxygenTankMonitor);
			float num = flag ? this.CRITICAL_OXYGEN_MASS_GIVE_UP_TRESHOLD : 2f;
			if (this.GetAverageMassConsumedPerSecond() <= num)
			{
				base.sm.SecondsPassedWithoutOxygen.Set(base.sm.SecondsPassedWithoutOxygen.Get(base.smi) + dt, base.smi, false);
			}
			else
			{
				BionicMassOxygenAbsorbChore.ResetOxygenTimer(base.smi);
			}
			if (flag)
			{
				float num2 = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND;
				if (mass_cb_info.mass == 0f)
				{
					mass_cb_info.temperature = DUPLICANTSTATS.BIONICS.Temperature.Internal.IDEAL;
				}
				mass_cb_info.mass += DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA * num2 * dt + DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * dt;
			}
			float num3 = this.oxygenTankMonitor.AddGas(mass_cb_info);
			if (num3 > Mathf.Epsilon)
			{
				SimMessages.EmitMass(Grid.PosToCell(gameObject), mass_cb_info.elemIdx, num3, mass_cb_info.temperature, byte.MaxValue, 0, -1);
			}
			if (!BionicMassOxygenAbsorbChore.HasSpaceInOxygenTank(this))
			{
				base.sm.TankFilledSignal.Trigger(this);
			}
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000B8479 File Offset: 0x000B6679
		public float GetOxygen()
		{
			if (this.oxygenTankMonitor != null)
			{
				return this.oxygenTankMonitor.OxygenPercentage;
			}
			return 0f;
		}

		// Token: 0x0400133D RID: 4925
		public AttributeModifier recoveringbreath;

		// Token: 0x0400133F RID: 4927
		public Queue<float> massAbsorbedHistory = new Queue<float>();

		// Token: 0x04001340 RID: 4928
		public int targetCell = Grid.InvalidCell;

		// Token: 0x04001341 RID: 4929
		public BionicOxygenTankMonitor.Instance oxygenTankMonitor;
	}
}
