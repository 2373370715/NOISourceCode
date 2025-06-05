using System;
using TUNING;

// Token: 0x02001642 RID: 5698
public class StressBehaviourMonitor : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance>
{
	// Token: 0x060075E6 RID: 30182 RVA: 0x00316C5C File Offset: 0x00314E5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.satisfied.EventTransition(GameHashes.Stressed, this.stressed, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed());
		this.stressed.DefaultState(this.stressed.tierOne).ToggleExpression(Db.Get().Expressions.Unhappy, null).ToggleAnims((StressBehaviourMonitor.Instance smi) => smi.tierOneLocoAnim).Transition(this.satisfied, (StressBehaviourMonitor.Instance smi) => smi.gameObject.GetSMI<StressMonitor.Instance>() != null && !smi.gameObject.GetSMI<StressMonitor.Instance>().IsStressed(), UpdateRate.SIM_200ms);
		this.stressed.tierOne.DefaultState(this.stressed.tierOne.actingOut).EventTransition(GameHashes.StressedHadEnough, this.stressed.tierTwo, null);
		this.stressed.tierOne.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierOneStressChore(), this.stressed.tierOne.reprieve);
		this.stressed.tierOne.reprieve.ScheduleGoTo(30f, this.stressed.tierOne.actingOut);
		this.stressed.tierTwo.DefaultState(this.stressed.tierTwo.actingOut).Update(delegate(StressBehaviourMonitor.Instance smi, float dt)
		{
			smi.sm.timeInTierTwoStressResponse.Set(smi.sm.timeInTierTwoStressResponse.Get(smi) + dt, smi, false);
		}, UpdateRate.SIM_200ms, false).Exit("ResetStress", delegate(StressBehaviourMonitor.Instance smi)
		{
			Db.Get().Amounts.Stress.Lookup(smi.gameObject).SetValue(STRESS.ACTING_OUT_RESET);
		});
		this.stressed.tierTwo.actingOut.ToggleChore((StressBehaviourMonitor.Instance smi) => smi.CreateTierTwoStressChore(), this.stressed.tierTwo.reprieve);
		this.stressed.tierTwo.reprieve.ToggleChore((StressBehaviourMonitor.Instance smi) => new StressIdleChore(smi.master), null).Enter(delegate(StressBehaviourMonitor.Instance smi)
		{
			if (smi.sm.timeInTierTwoStressResponse.Get(smi) >= 150f)
			{
				smi.sm.timeInTierTwoStressResponse.Set(0f, smi, false);
				smi.GoTo(this.stressed);
			}
		}).ScheduleGoTo((StressBehaviourMonitor.Instance smi) => smi.tierTwoReprieveDuration, this.stressed.tierTwo);
	}

	// Token: 0x04005899 RID: 22681
	public const float TIER2_STRESS_RESPONSE_TIMEOUT = 150f;

	// Token: 0x0400589A RID: 22682
	public StateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.FloatParameter timeInTierTwoStressResponse;

	// Token: 0x0400589B RID: 22683
	public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x0400589C RID: 22684
	public StressBehaviourMonitor.StressedState stressed;

	// Token: 0x02001643 RID: 5699
	public class StressedState : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400589D RID: 22685
		public StressBehaviourMonitor.TierOneStates tierOne;

		// Token: 0x0400589E RID: 22686
		public StressBehaviourMonitor.TierTwoStates tierTwo;
	}

	// Token: 0x02001644 RID: 5700
	public class TierOneStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400589F RID: 22687
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x040058A0 RID: 22688
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x02001645 RID: 5701
	public class TierTwoStates : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040058A1 RID: 22689
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State actingOut;

		// Token: 0x040058A2 RID: 22690
		public GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.State reprieve;
	}

	// Token: 0x02001646 RID: 5702
	public new class Instance : GameStateMachine<StressBehaviourMonitor, StressBehaviourMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075EC RID: 30188 RVA: 0x000F1FEB File Offset: 0x000F01EB
		public Instance(IStateMachineTarget master, Func<ChoreProvider, Chore> tier_one_stress_chore_creator, Func<ChoreProvider, Chore> tier_two_stress_chore_creator, string tier_one_loco_anim, float tier_two_reprieve_duration = 3f) : base(master)
		{
			this.tierOneLocoAnim = tier_one_loco_anim;
			this.tierTwoReprieveDuration = tier_two_reprieve_duration;
			this.tierOneStressChoreCreator = tier_one_stress_chore_creator;
			this.tierTwoStressChoreCreator = tier_two_stress_chore_creator;
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x000F201D File Offset: 0x000F021D
		public Chore CreateTierOneStressChore()
		{
			return this.tierOneStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x060075EE RID: 30190 RVA: 0x000F2030 File Offset: 0x000F0230
		public Chore CreateTierTwoStressChore()
		{
			return this.tierTwoStressChoreCreator(base.GetComponent<ChoreProvider>());
		}

		// Token: 0x060075EF RID: 30191 RVA: 0x000F2043 File Offset: 0x000F0243
		public void ManualSetStressTier2TimeCounter(float timerValue)
		{
			base.sm.timeInTierTwoStressResponse.Set(timerValue, this, false);
		}

		// Token: 0x040058A3 RID: 22691
		public Func<ChoreProvider, Chore> tierOneStressChoreCreator;

		// Token: 0x040058A4 RID: 22692
		public Func<ChoreProvider, Chore> tierTwoStressChoreCreator;

		// Token: 0x040058A5 RID: 22693
		public string tierOneLocoAnim = "";

		// Token: 0x040058A6 RID: 22694
		public float tierTwoReprieveDuration;
	}
}
