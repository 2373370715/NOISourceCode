using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200157C RID: 5500
public class ColdImmunityMonitor : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance>
{
	// Token: 0x0600729A RID: 29338 RVA: 0x0030D2C4 File Offset: 0x0030B4C4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.DefaultState(this.idle.feelingFine).TagTransition(GameTags.FeelingCold, this.cold, false).ParamTransition<float>(this.coldCountdown, this.cold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsGTZero);
		this.idle.feelingFine.DoNothing();
		this.idle.leftWithDesireToWarmupAfterBeingCold.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle.feelingFine, this.idle.feelingFine);
		this.cold.DefaultState(this.cold.exiting).TagTransition(GameTags.FeelingWarm, this.idle, false).ToggleAnims("anim_idle_cold_kanim", 0f).ToggleAnims("anim_loco_run_cold_kanim", 0f).ToggleAnims("anim_loco_walk_cold_kanim", 0f).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleEffect("ColdAir").Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.UpdateWarmUpCell)).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.UpdateWarmUpCell), UpdateRate.RENDER_1000ms, false).ToggleChore(new Func<ColdImmunityMonitor.Instance, Chore>(ColdImmunityMonitor.CreateRecoverFromChillyBonesChore), this.idle, this.cold);
		this.cold.exiting.EventHandlerTransition(GameHashes.EffectAdded, this.idle, new Func<ColdImmunityMonitor.Instance, object, bool>(ColdImmunityMonitor.HasImmunityEffect)).TagTransition(GameTags.FeelingCold, this.cold.idle, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.ExitingCold, null).ParamTransition<float>(this.coldCountdown, this.idle.leftWithDesireToWarmupAfterBeingCold, GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.IsZero).Update(new Action<ColdImmunityMonitor.Instance, float>(ColdImmunityMonitor.ColdTimerUpdate), UpdateRate.SIM_200ms, false).Exit(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ClearTimer));
		this.cold.idle.Enter(new StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State.Callback(ColdImmunityMonitor.ResetColdTimer)).ToggleStatusItem(Db.Get().DuplicantStatusItems.Cold, (ColdImmunityMonitor.Instance smi) => smi).TagTransition(GameTags.FeelingCold, this.cold.exiting, true);
	}

	// Token: 0x0600729B RID: 29339 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public static bool OnEffectAdded(ColdImmunityMonitor.Instance smi, object data)
	{
		return true;
	}

	// Token: 0x0600729C RID: 29340 RVA: 0x000EF963 File Offset: 0x000EDB63
	public static void ClearTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(0f, smi, false);
	}

	// Token: 0x0600729D RID: 29341 RVA: 0x000EF97D File Offset: 0x000EDB7D
	public static void ResetColdTimer(ColdImmunityMonitor.Instance smi)
	{
		smi.sm.coldCountdown.Set(5f, smi, false);
	}

	// Token: 0x0600729E RID: 29342 RVA: 0x0030D548 File Offset: 0x0030B748
	public static void ColdTimerUpdate(ColdImmunityMonitor.Instance smi, float dt)
	{
		float value = Mathf.Clamp(smi.ColdCountdown - dt, 0f, 5f);
		smi.sm.coldCountdown.Set(value, smi, false);
	}

	// Token: 0x0600729F RID: 29343 RVA: 0x000EF997 File Offset: 0x000EDB97
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi, float dt)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x060072A0 RID: 29344 RVA: 0x000EF997 File Offset: 0x000EDB97
	private static void UpdateWarmUpCell(ColdImmunityMonitor.Instance smi)
	{
		smi.UpdateWarmUpCell();
	}

	// Token: 0x060072A1 RID: 29345 RVA: 0x0030D584 File Offset: 0x0030B784
	public static bool HasImmunityEffect(ColdImmunityMonitor.Instance smi, object data)
	{
		Effects component = smi.GetComponent<Effects>();
		return component != null && component.HasEffect("WarmTouch");
	}

	// Token: 0x060072A2 RID: 29346 RVA: 0x000EF99F File Offset: 0x000EDB9F
	private static Chore CreateRecoverFromChillyBonesChore(ColdImmunityMonitor.Instance smi)
	{
		return new RecoverFromColdChore(smi.master);
	}

	// Token: 0x040055F2 RID: 22002
	private const float EFFECT_DURATION = 5f;

	// Token: 0x040055F3 RID: 22003
	public ColdImmunityMonitor.IdleStates idle;

	// Token: 0x040055F4 RID: 22004
	public ColdImmunityMonitor.ColdStates cold;

	// Token: 0x040055F5 RID: 22005
	public StateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.FloatParameter coldCountdown;

	// Token: 0x0200157D RID: 5501
	public class ColdStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055F6 RID: 22006
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040055F7 RID: 22007
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State exiting;

		// Token: 0x040055F8 RID: 22008
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State resetChore;
	}

	// Token: 0x0200157E RID: 5502
	public class IdleStates : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055F9 RID: 22009
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State feelingFine;

		// Token: 0x040055FA RID: 22010
		public GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.State leftWithDesireToWarmupAfterBeingCold;
	}

	// Token: 0x0200157F RID: 5503
	public new class Instance : GameStateMachine<ColdImmunityMonitor, ColdImmunityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x060072A6 RID: 29350 RVA: 0x000EF9BC File Offset: 0x000EDBBC
		// (set) Token: 0x060072A7 RID: 29351 RVA: 0x000EF9C4 File Offset: 0x000EDBC4
		public ColdImmunityProvider.Instance NearestImmunityProvider { get; private set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x060072A8 RID: 29352 RVA: 0x000EF9CD File Offset: 0x000EDBCD
		// (set) Token: 0x060072A9 RID: 29353 RVA: 0x000EF9D5 File Offset: 0x000EDBD5
		public int WarmUpCell { get; private set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x060072AA RID: 29354 RVA: 0x000EF9DE File Offset: 0x000EDBDE
		public float ColdCountdown
		{
			get
			{
				return base.smi.sm.coldCountdown.Get(this);
			}
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x000EF9F6 File Offset: 0x000EDBF6
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060072AC RID: 29356 RVA: 0x000EF9FF File Offset: 0x000EDBFF
		public override void StartSM()
		{
			this.navigator = base.gameObject.GetComponent<Navigator>();
			base.StartSM();
		}

		// Token: 0x060072AD RID: 29357 RVA: 0x0030D5B0 File Offset: 0x0030B7B0
		public void UpdateWarmUpCell()
		{
			int myWorldId = this.navigator.GetMyWorldId();
			int warmUpCell = Grid.InvalidCell;
			int num = int.MaxValue;
			ColdImmunityProvider.Instance nearestImmunityProvider = null;
			foreach (StateMachine.Instance instance in Components.EffectImmunityProviderStations.Items.FindAll((StateMachine.Instance t) => t is ColdImmunityProvider.Instance))
			{
				ColdImmunityProvider.Instance instance2 = instance as ColdImmunityProvider.Instance;
				if (instance2.GetMyWorldId() == myWorldId)
				{
					int maxValue = int.MaxValue;
					int bestAvailableCell = instance2.GetBestAvailableCell(this.navigator, out maxValue);
					if (maxValue < num)
					{
						num = maxValue;
						nearestImmunityProvider = instance2;
						warmUpCell = bestAvailableCell;
					}
				}
			}
			this.NearestImmunityProvider = nearestImmunityProvider;
			this.WarmUpCell = warmUpCell;
		}

		// Token: 0x040055FD RID: 22013
		private Navigator navigator;
	}
}
