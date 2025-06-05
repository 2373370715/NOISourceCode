using System;
using Klei.AI;
using TUNING;

// Token: 0x02001578 RID: 5496
public class CalorieMonitor : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance>
{
	// Token: 0x06007280 RID: 29312 RVA: 0x0030CF54 File Offset: 0x0030B154
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.Transition(this.hungry, (CalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_200ms);
		this.hungry.DefaultState(this.hungry.normal).Transition(this.satisfied, (CalorieMonitor.Instance smi) => smi.IsSatisfied(), UpdateRate.SIM_200ms).EventTransition(GameHashes.BeginChore, this.eating, (CalorieMonitor.Instance smi) => smi.IsEating());
		this.hungry.working.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.normal, (CalorieMonitor.Instance smi) => smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null);
		this.hungry.normal.EventTransition(GameHashes.ScheduleBlocksChanged, this.hungry.working, (CalorieMonitor.Instance smi) => !smi.IsEatTime()).Transition(this.hungry.starving, (CalorieMonitor.Instance smi) => smi.IsStarving(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Hungry, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.hungry.starving.Transition(this.hungry.normal, (CalorieMonitor.Instance smi) => !smi.IsStarving(), UpdateRate.SIM_200ms).Transition(this.depleted, (CalorieMonitor.Instance smi) => smi.IsDepleted(), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.Starving, null).ToggleUrge(Db.Get().Urges.Eat).ToggleExpression(Db.Get().Expressions.Hungry, null).ToggleThought(Db.Get().Thoughts.Starving, null);
		this.eating.EventTransition(GameHashes.EndChore, this.satisfied, (CalorieMonitor.Instance smi) => !smi.IsEating());
		this.depleted.ToggleTag(GameTags.CaloriesDepleted).Enter(delegate(CalorieMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x040055DD RID: 21981
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040055DE RID: 21982
	public CalorieMonitor.HungryState hungry;

	// Token: 0x040055DF RID: 21983
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State eating;

	// Token: 0x040055E0 RID: 21984
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x040055E1 RID: 21985
	public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State depleted;

	// Token: 0x02001579 RID: 5497
	public class HungryState : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040055E2 RID: 21986
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State working;

		// Token: 0x040055E3 RID: 21987
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x040055E4 RID: 21988
		public GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.State starving;
	}

	// Token: 0x0200157A RID: 5498
	public new class Instance : GameStateMachine<CalorieMonitor, CalorieMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007283 RID: 29315 RVA: 0x000EF800 File Offset: 0x000EDA00
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
		}

		// Token: 0x06007284 RID: 29316 RVA: 0x000EF829 File Offset: 0x000EDA29
		private float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06007285 RID: 29317 RVA: 0x000EF842 File Offset: 0x000EDA42
		public bool IsEatTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Eat);
		}

		// Token: 0x06007286 RID: 29318 RVA: 0x000EF863 File Offset: 0x000EDA63
		public bool IsHungry()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.HUNGRY_THRESHOLD;
		}

		// Token: 0x06007287 RID: 29319 RVA: 0x000EF87C File Offset: 0x000EDA7C
		public bool IsStarving()
		{
			return this.GetCalories0to1() < DUPLICANTSTATS.STANDARD.BaseStats.STARVING_THRESHOLD;
		}

		// Token: 0x06007288 RID: 29320 RVA: 0x000EF895 File Offset: 0x000EDA95
		public bool IsSatisfied()
		{
			return this.GetCalories0to1() > DUPLICANTSTATS.STANDARD.BaseStats.SATISFIED_THRESHOLD;
		}

		// Token: 0x06007289 RID: 29321 RVA: 0x0030D280 File Offset: 0x0030B480
		public bool IsEating()
		{
			ChoreDriver component = base.master.GetComponent<ChoreDriver>();
			return component.HasChore() && component.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x000EF8AE File Offset: 0x000EDAAE
		public bool IsDepleted()
		{
			return this.calories.value <= 0f;
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x000EF8C5 File Offset: 0x000EDAC5
		public bool ShouldExitInfirmary()
		{
			return !this.IsStarving();
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x000EF8D0 File Offset: 0x000EDAD0
		public void Kill()
		{
			if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
			{
				base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation);
			}
		}

		// Token: 0x040055E5 RID: 21989
		public AmountInstance calories;
	}
}
