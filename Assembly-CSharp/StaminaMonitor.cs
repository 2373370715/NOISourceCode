using System;
using Klei.AI;

// Token: 0x0200163C RID: 5692
public class StaminaMonitor : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance>
{
	// Token: 0x060075C3 RID: 30147 RVA: 0x00316644 File Offset: 0x00314844
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.ToggleStateMachine((StaminaMonitor.Instance smi) => new UrgeMonitor.Instance(smi.master, Db.Get().Urges.Sleep, Db.Get().Amounts.Stamina, Db.Get().ScheduleBlockTypes.Sleep, 100f, 0f, false)).ToggleStateMachine((StaminaMonitor.Instance smi) => new SleepChoreMonitor.Instance(smi.master));
		this.satisfied.Transition(this.sleepy, (StaminaMonitor.Instance smi) => smi.NeedsToSleep() || smi.WantsToSleep(), UpdateRate.SIM_200ms);
		this.sleepy.Update("Check Sleep State", delegate(StaminaMonitor.Instance smi, float dt)
		{
			smi.TryExitSleepState();
		}, UpdateRate.SIM_1000ms, false).DefaultState(this.sleepy.needssleep);
		this.sleepy.needssleep.Transition(this.sleepy.sleeping, (StaminaMonitor.Instance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleStatusItem(Db.Get().DuplicantStatusItems.Tired, null).ToggleThought(Db.Get().Thoughts.Sleepy, null);
		this.sleepy.sleeping.Enter(delegate(StaminaMonitor.Instance smi)
		{
			smi.CheckDebugFastWorkMode();
		}).Transition(this.satisfied, (StaminaMonitor.Instance smi) => !smi.IsSleeping(), UpdateRate.SIM_200ms);
	}

	// Token: 0x04005880 RID: 22656
	public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005881 RID: 22657
	public StaminaMonitor.SleepyState sleepy;

	// Token: 0x04005882 RID: 22658
	private const float OUTSIDE_SCHEDULE_STAMINA_THRESHOLD = 0f;

	// Token: 0x0200163D RID: 5693
	public class SleepyState : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005883 RID: 22659
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State needssleep;

		// Token: 0x04005884 RID: 22660
		public GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.State sleeping;
	}

	// Token: 0x0200163E RID: 5694
	public new class Instance : GameStateMachine<StaminaMonitor, StaminaMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075C6 RID: 30150 RVA: 0x003167FC File Offset: 0x003149FC
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stamina = Db.Get().Amounts.Stamina.Lookup(base.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
			this.schedulable = base.GetComponent<Schedulable>();
		}

		// Token: 0x060075C7 RID: 30151 RVA: 0x000F1E10 File Offset: 0x000F0010
		public bool NeedsToSleep()
		{
			return this.stamina.value <= 0f;
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x000F1E27 File Offset: 0x000F0027
		public bool WantsToSleep()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Sleep);
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x000F1E57 File Offset: 0x000F0057
		public void TryExitSleepState()
		{
			if (!this.NeedsToSleep() && !this.WantsToSleep())
			{
				base.smi.GoTo(base.smi.sm.satisfied);
			}
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x00316848 File Offset: 0x00314A48
		public bool IsSleeping()
		{
			bool result = false;
			if (this.WantsToSleep() && this.choreDriver.GetComponent<WorkerBase>().GetWorkable() != null)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x000F1E84 File Offset: 0x000F0084
		public void CheckDebugFastWorkMode()
		{
			if (Game.Instance.FastWorkersModeActive)
			{
				this.stamina.value = this.stamina.GetMax();
			}
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x0031687C File Offset: 0x00314A7C
		public bool ShouldExitSleep()
		{
			if (this.schedulable.IsAllowed(Db.Get().ScheduleBlockTypes.Sleep))
			{
				return false;
			}
			Narcolepsy component = base.GetComponent<Narcolepsy>();
			return (!(component != null) || !component.IsNarcolepsing()) && this.stamina.value >= this.stamina.GetMax();
		}

		// Token: 0x04005885 RID: 22661
		private ChoreDriver choreDriver;

		// Token: 0x04005886 RID: 22662
		private Schedulable schedulable;

		// Token: 0x04005887 RID: 22663
		public AmountInstance stamina;
	}
}
