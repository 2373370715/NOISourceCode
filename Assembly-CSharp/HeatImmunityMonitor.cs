using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020015D0 RID: 5584
public class HeatImmunityMonitor : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance>
{
	// Token: 0x060073F3 RID: 29683 RVA: 0x00311570 File Offset: 0x0030F770
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingWarm, this.warm, false).ParamTransition<float>(this.heatCountdown, this.warm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToCooldownAfterBeingWarm.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle.feelingFine, this.idle.feelingFine);
		this.warm.DefaultState(this.warm.exiting).TagTransition(GameTags.FeelingCold, this.idle, false).ToggleAnims("anim_idle_hot_kanim", 0f).ToggleAnims("anim_loco_run_hot_kanim", 0f).ToggleAnims("anim_loco_walk_hot_kanim", 0f).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleEffect("WarmAir").Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.UpdateShelterCell)).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.UpdateShelterCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<HeatImmunityMonitor.Instance, Chore>(HeatImmunityMonitor.CreateRecoverFromOverheatChore), this.idle, this.warm);
		this.warm.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<HeatImmunityMonitor.Instance, object, bool>(HeatImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingWarm, this.warm.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingHot, null).ParamTransition<float>(this.heatCountdown, this.idle.leftWithDesireToCooldownAfterBeingWarm, GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<HeatImmunityMonitor.Instance, float>(HeatImmunityMonitor.HeatTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ClearTimer));
		this.warm.idle.Enter(new StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(HeatImmunityMonitor.ResetHeatTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hot, (HeatImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingWarm, this.warm.exiting, true);
	}

	// Token: 0x060073F4 RID: 29684 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public static bool OnEffectAdded(HeatImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x060073F5 RID: 29685 RVA: 0x000F07D8 File Offset: 0x000EE9D8
	public static void ClearTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(0f, smi, false);
	}

	// Token: 0x060073F6 RID: 29686 RVA: 0x000F07F2 File Offset: 0x000EE9F2
	public static void ResetHeatTimer(HeatImmunityMonitor.Instance smi)
	{
		smi.sm.heatCountdown.Set(5f, smi, false);
	}

	// Token: 0x060073F7 RID: 29687 RVA: 0x003117F4 File Offset: 0x0030F9F4
	public static void HeatTimerUpdate(HeatImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.HeatCountdown - dt, 0f, 5f);
		smi.sm.heatCountdown.Set(value, smi, false);
	}

	// Token: 0x060073F8 RID: 29688 RVA: 0x000F080C File Offset: 0x000EEA0C
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x060073F9 RID: 29689 RVA: 0x000F080C File Offset: 0x000EEA0C
	private static void UpdateShelterCell(HeatImmunityMonitor.Instance smi)
	{
		smi.UpdateShelterCell();
	}

	// Token: 0x060073FA RID: 29690 RVA: 0x00311830 File Offset: 0x0030FA30
	public static bool HasImmunityEffect(HeatImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("RefreshingTouch");
	}

	// Token: 0x060073FB RID: 29691 RVA: 0x000F0814 File Offset: 0x000EEA14
	private static Chore CreateRecoverFromOverheatChore(HeatImmunityMonitor.Instance smi)
	{
		return new RecoverFromHeatChore(smi.master);
	}

	// Token: 0x04005711 RID: 22289
	private const float EFFECT_DURATION = 5f;

	// Token: 0x04005712 RID: 22290
	public HeatImmunityMonitor.IdleStates idle;

	// Token: 0x04005713 RID: 22291
	public HeatImmunityMonitor.WarmStates warm;

	// Token: 0x04005714 RID: 22292
	public StateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter heatCountdown;

	// Token: 0x020015D1 RID: 5585
	public class WarmStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005715 RID: 22293
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04005716 RID: 22294
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x04005717 RID: 22295
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x020015D2 RID: 5586
	public class IdleStates : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005718 RID: 22296
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x04005719 RID: 22297
		public GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToCooldownAfterBeingWarm;
	}

	// Token: 0x020015D3 RID: 5587
	public new class Instance : GameStateMachine<HeatImmunityMonitor, HeatImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x000F0831 File Offset: 0x000EEA31
		// (set) Token: 0x06007400 RID: 29696 RVA: 0x000F0839 File Offset: 0x000EEA39
		public HeatImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06007401 RID: 29697 RVA: 0x000F0842 File Offset: 0x000EEA42
		// (set) Token: 0x06007402 RID: 29698 RVA: 0x000F084A File Offset: 0x000EEA4A
		public int ShelterCell { get; private set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06007403 RID: 29699 RVA: 0x000F0853 File Offset: 0x000EEA53
		public float HeatCountdown
		{
			get
			{
				return base.smi.sm.heatCountdown.Get(this);
			}
		}

		// Token: 0x06007404 RID: 29700 RVA: 0x000F086B File Offset: 0x000EEA6B
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06007405 RID: 29701 RVA: 0x000F0874 File Offset: 0x000EEA74
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x06007406 RID: 29702 RVA: 0x0031185C File Offset: 0x0030FA5C
		public void UpdateShelterCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int shelterCell = Grid.InvalidCell;
			int num = int.MaxValue;
			HeatImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is HeatImmunityProvider.Instance))
			{
				HeatImmunityProvider.Instance instance2 = instance as HeatImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						shelterCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.ShelterCell = shelterCell;
		}

		// Token: 0x0400571C RID: 22300
		private Navigator navigator;
	}
}
