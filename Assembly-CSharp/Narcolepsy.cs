using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001690 RID: 5776
[SkipSaveFileSerialization]
public class Narcolepsy : StateMachineComponent<Narcolepsy.StatesInstance>
{
	// Token: 0x0600775F RID: 30559 RVA: 0x000F2F88 File Offset: 0x000F1188
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06007760 RID: 30560 RVA: 0x000F2F95 File Offset: 0x000F1195
	public bool IsNarcolepsing()
	{
		return base.smi.IsNarcolepsing();
	}

	// Token: 0x040059F5 RID: 23029
	public static readonly Chore.Precondition IsNarcolepsingPrecondition = new Chore.Precondition
	{
		id = "IsNarcolepsingPrecondition",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NARCOLEPSING,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			return component != null && component.IsNarcolepsing();
		}
	};

	// Token: 0x02001691 RID: 5777
	public class StatesInstance : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.GameInstance
	{
		// Token: 0x06007763 RID: 30563 RVA: 0x000F2FAA File Offset: 0x000F11AA
		public StatesInstance(Narcolepsy master) : base(master)
		{
		}

		// Token: 0x06007764 RID: 30564 RVA: 0x0031BA08 File Offset: 0x00319C08
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06007765 RID: 30565 RVA: 0x000F2FB3 File Offset: 0x000F11B3
		public bool IsNarcolepsing()
		{
			return this.GetCurrentState() == base.sm.sleepy;
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x000F2FC8 File Offset: 0x000F11C8
		public GameObject CreateFloorLocator()
		{
			Sleepable safeFloorLocator = SleepChore.GetSafeFloorLocator(base.master.gameObject);
			safeFloorLocator.effectName = "NarcolepticSleep";
			safeFloorLocator.stretchOnWake = false;
			return safeFloorLocator.gameObject;
		}
	}

	// Token: 0x02001692 RID: 5778
	public class States : GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy>
	{
		// Token: 0x06007767 RID: 30567 RVA: 0x0031BA2C File Offset: 0x00319C2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Enter("ScheduleNextSleep", delegate(Narcolepsy.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_INTERVAL_MIN, TRAITS.NARCOLEPSY_INTERVAL_MAX), this.sleepy);
			});
			this.sleepy.Enter("Is Already Sleeping Check", delegate(Narcolepsy.StatesInstance smi)
			{
				if (smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping())
				{
					smi.GoTo(this.idle);
					return;
				}
				smi.ScheduleGoTo(this.GetNewInterval(TRAITS.NARCOLEPSY_SLEEPDURATION_MIN, TRAITS.NARCOLEPSY_SLEEPDURATION_MAX), this.idle);
			}).ToggleUrge(Db.Get().Urges.Narcolepsy).ToggleChore(new Func<Narcolepsy.StatesInstance, Chore>(this.CreateNarcolepsyChore), this.idle);
		}

		// Token: 0x06007768 RID: 30568 RVA: 0x0031BABC File Offset: 0x00319CBC
		private Chore CreateNarcolepsyChore(Narcolepsy.StatesInstance smi)
		{
			GameObject bed = smi.CreateFloorLocator();
			SleepChore sleepChore = new SleepChore(Db.Get().ChoreTypes.Narcolepsy, smi.master, bed, true, false);
			sleepChore.AddPrecondition(Narcolepsy.IsNarcolepsingPrecondition, null);
			return sleepChore;
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x000F2FF1 File Offset: 0x000F11F1
		private float GetNewInterval(float min, float max)
		{
			Mathf.Min(Mathf.Max(Util.GaussianRandom(max - min, 1f), min), max);
			return UnityEngine.Random.Range(min, max);
		}

		// Token: 0x040059F6 RID: 23030
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State idle;

		// Token: 0x040059F7 RID: 23031
		public GameStateMachine<Narcolepsy.States, Narcolepsy.StatesInstance, Narcolepsy, object>.State sleepy;
	}
}
