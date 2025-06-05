using System;
using Klei.AI;
using TUNING;

// Token: 0x02001574 RID: 5492
public class BreathMonitor : GameStateMachine<BreathMonitor, BreathMonitor.Instance>
{
	// Token: 0x0600726E RID: 29294 RVA: 0x0030CC04 File Offset: 0x0030AE04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.full).Transition(this.lowbreath, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsLowBreath), UpdateRate.SIM_200ms);
		this.satisfied.full.Transition(this.satisfied.notfull, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsNotFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.HideBreathBar));
		this.satisfied.notfull.Transition(this.satisfied.full, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar));
		this.lowbreath.DefaultState(this.lowbreath.nowheretorecover).Transition(this.satisfied, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(BreathMonitor.IsFullBreath), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.RecoverBreath, new Func<BreathMonitor.Instance, bool>(BreathMonitor.IsOutOfOxygen)).ToggleUrge(Db.Get().Urges.RecoverBreath).ToggleThought(Db.Get().Thoughts.Suffocating, null).ToggleTag(GameTags.HoldingBreath).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.ShowBreathBar)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).Update(new Action<BreathMonitor.Instance, float>(BreathMonitor.UpdateRecoverBreathCell), UpdateRate.RENDER_1000ms, true);
		this.lowbreath.nowheretorecover.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.recoveryavailable, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsValidRecoverCell));
		this.lowbreath.recoveryavailable.ParamTransition<int>(this.recoverBreathCell, this.lowbreath.nowheretorecover, new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.Parameter<int>.Callback(BreathMonitor.IsNotValidRecoverCell)).Enter(new StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State.Callback(BreathMonitor.UpdateRecoverBreathCell)).ToggleChore(new Func<BreathMonitor.Instance, Chore>(BreathMonitor.CreateRecoverBreathChore), this.lowbreath.nowheretorecover);
	}

	// Token: 0x0600726F RID: 29295 RVA: 0x0030CDFC File Offset: 0x0030AFFC
	private static bool IsLowBreath(BreathMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
		if (!(myWorld == null) && myWorld.AlertManager.IsRedAlert())
		{
			return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}
		return smi.breath.value < DUPLICANTSTATS.STANDARD.Breath.RETREAT_AMOUNT;
	}

	// Token: 0x06007270 RID: 29296 RVA: 0x000EF6FF File Offset: 0x000ED8FF
	private static Chore CreateRecoverBreathChore(BreathMonitor.Instance smi)
	{
		return new RecoverBreathChore(smi.master);
	}

	// Token: 0x06007271 RID: 29297 RVA: 0x000EF70C File Offset: 0x000ED90C
	private static bool IsNotFullBreath(BreathMonitor.Instance smi)
	{
		return !BreathMonitor.IsFullBreath(smi);
	}

	// Token: 0x06007272 RID: 29298 RVA: 0x000EF717 File Offset: 0x000ED917
	private static bool IsFullBreath(BreathMonitor.Instance smi)
	{
		return smi.breath.value >= smi.breath.GetMax();
	}

	// Token: 0x06007273 RID: 29299 RVA: 0x000EF734 File Offset: 0x000ED934
	private static bool IsOutOfOxygen(BreathMonitor.Instance smi)
	{
		return smi.breather.IsOutOfOxygen;
	}

	// Token: 0x06007274 RID: 29300 RVA: 0x000EF741 File Offset: 0x000ED941
	private static void ShowBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, new Func<float>(smi.GetBreath), true);
		}
	}

	// Token: 0x06007275 RID: 29301 RVA: 0x000EF76D File Offset: 0x000ED96D
	private static void HideBreathBar(BreathMonitor.Instance smi)
	{
		if (NameDisplayScreen.Instance != null)
		{
			NameDisplayScreen.Instance.SetBreathDisplay(smi.gameObject, null, false);
		}
	}

	// Token: 0x06007276 RID: 29302 RVA: 0x000EF78E File Offset: 0x000ED98E
	private static bool IsValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return cell != Grid.InvalidCell;
	}

	// Token: 0x06007277 RID: 29303 RVA: 0x000EF79B File Offset: 0x000ED99B
	private static bool IsNotValidRecoverCell(BreathMonitor.Instance smi, int cell)
	{
		return !BreathMonitor.IsValidRecoverCell(smi, cell);
	}

	// Token: 0x06007278 RID: 29304 RVA: 0x000EF7A7 File Offset: 0x000ED9A7
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi, float dt)
	{
		BreathMonitor.UpdateRecoverBreathCell(smi);
	}

	// Token: 0x06007279 RID: 29305 RVA: 0x0030CE6C File Offset: 0x0030B06C
	private static void UpdateRecoverBreathCell(BreathMonitor.Instance smi)
	{
		if (smi.canRecoverBreath)
		{
			smi.query.Reset();
			smi.navigator.RunQuery(smi.query);
			int num = smi.query.GetResultCell();
			if (!GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(num, GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, smi.breather).IsBreathable)
			{
				num = PathFinder.InvalidCell;
			}
			smi.sm.recoverBreathCell.Set(num, smi, false);
		}
	}

	// Token: 0x040055D1 RID: 21969
	public BreathMonitor.SatisfiedState satisfied;

	// Token: 0x040055D2 RID: 21970
	public BreathMonitor.LowBreathState lowbreath;

	// Token: 0x040055D3 RID: 21971
	public StateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.IntParameter recoverBreathCell;

	// Token: 0x02001575 RID: 5493
	public class LowBreathState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055D4 RID: 21972
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State nowheretorecover;

		// Token: 0x040055D5 RID: 21973
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State recoveryavailable;
	}

	// Token: 0x02001576 RID: 5494
	public class SatisfiedState : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055D6 RID: 21974
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State full;

		// Token: 0x040055D7 RID: 21975
		public GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.State notfull;
	}

	// Token: 0x02001577 RID: 5495
	public new class Instance : GameStateMachine<BreathMonitor, BreathMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600727D RID: 29309 RVA: 0x0030CEDC File Offset: 0x0030B0DC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			this.query = new SafetyQuery(Game.Instance.safetyConditions.RecoverBreathChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
			this.breather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x0600727E RID: 29310 RVA: 0x000EF7BF File Offset: 0x000ED9BF
		public int GetRecoverCell()
		{
			return base.sm.recoverBreathCell.Get(base.smi);
		}

		// Token: 0x0600727F RID: 29311 RVA: 0x000EF7D7 File Offset: 0x000ED9D7
		public float GetBreath()
		{
			return this.breath.value / this.breath.GetMax();
		}

		// Token: 0x040055D8 RID: 21976
		public AmountInstance breath;

		// Token: 0x040055D9 RID: 21977
		public SafetyQuery query;

		// Token: 0x040055DA RID: 21978
		public Navigator navigator;

		// Token: 0x040055DB RID: 21979
		public OxygenBreather breather;

		// Token: 0x040055DC RID: 21980
		public bool canRecoverBreath = true;
	}
}
