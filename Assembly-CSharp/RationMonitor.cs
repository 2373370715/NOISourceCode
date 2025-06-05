using System;

// Token: 0x0200160D RID: 5645
public class RationMonitor : GameStateMachine<RationMonitor, RationMonitor.Instance>
{
	// Token: 0x060074EC RID: 29932 RVA: 0x00313D00 File Offset: 0x00311F00
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.rationsavailable;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.EatCompleteEater, delegate(RationMonitor.Instance smi, object d)
		{
			smi.OnEatComplete(d);
		}).EventHandler(GameHashes.NewDay, (RationMonitor.Instance smi) => GameClock.Instance, delegate(RationMonitor.Instance smi)
		{
			smi.OnNewDay();
		}).ParamTransition<float>(this.rationsAteToday, this.rationsavailable, (RationMonitor.Instance smi, float p) => smi.HasRationsAvailable()).ParamTransition<float>(this.rationsAteToday, this.outofrations, (RationMonitor.Instance smi, float p) => !smi.HasRationsAvailable());
		this.rationsavailable.DefaultState(this.rationsavailable.noediblesavailable);
		this.rationsavailable.noediblesavailable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.NoRationsAvailable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereAnyEdibles));
		this.rationsavailable.ediblereachablebutnotpermitted.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsNotPermitted).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblesunreachable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.NotIsEdibleInReachButNotPermitted));
		this.rationsavailable.ediblesunreachable.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.RationsUnreachable).EventTransition(GameHashes.ColonyHasRationsChanged, new Func<RationMonitor.Instance, KMonoBehaviour>(RationMonitor.GetSaveGame), this.rationsavailable.noediblesavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.AreThereNoEdibles)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.edibleavailable, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleAvailable)).EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.ediblereachablebutnotpermitted, new StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(RationMonitor.IsEdibleInReachButNotPermitted));
		this.rationsavailable.edibleavailable.ToggleChore((RationMonitor.Instance smi) => new EatChore(smi.master), this.rationsavailable.noediblesavailable).DefaultState(this.rationsavailable.edibleavailable.readytoeat);
		this.rationsavailable.edibleavailable.readytoeat.EventTransition(GameHashes.ClosestEdibleChanged, this.rationsavailable.noediblesavailable, null).EventTransition(GameHashes.BeginChore, this.rationsavailable.edibleavailable.eating, (RationMonitor.Instance smi) => smi.IsEating());
		this.rationsavailable.edibleavailable.eating.DoNothing();
		this.outofrations.InitializeStates(this.masterTarget, Db.Get().DuplicantStatusItems.DailyRationLimitReached);
	}

	// Token: 0x060074ED RID: 29933 RVA: 0x000F14D7 File Offset: 0x000EF6D7
	private static bool AreThereNoEdibles(RationMonitor.Instance smi)
	{
		return !RationMonitor.AreThereAnyEdibles(smi);
	}

	// Token: 0x060074EE RID: 29934 RVA: 0x00314050 File Offset: 0x00312250
	private static bool AreThereAnyEdibles(RationMonitor.Instance smi)
	{
		if (SaveGame.Instance != null)
		{
			ColonyRationMonitor.Instance smi2 = SaveGame.Instance.GetSMI<ColonyRationMonitor.Instance>();
			if (smi2 != null)
			{
				return !smi2.IsOutOfRations();
			}
		}
		return false;
	}

	// Token: 0x060074EF RID: 29935 RVA: 0x000F14E2 File Offset: 0x000EF6E2
	private static KMonoBehaviour GetSaveGame(RationMonitor.Instance smi)
	{
		return SaveGame.Instance;
	}

	// Token: 0x060074F0 RID: 29936 RVA: 0x000F14E9 File Offset: 0x000EF6E9
	private static bool IsEdibleAvailable(RationMonitor.Instance smi)
	{
		return smi.GetEdible() != null;
	}

	// Token: 0x060074F1 RID: 29937 RVA: 0x000F14F7 File Offset: 0x000EF6F7
	private static bool NotIsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return !RationMonitor.IsEdibleInReachButNotPermitted(smi);
	}

	// Token: 0x060074F2 RID: 29938 RVA: 0x000F1502 File Offset: 0x000EF702
	private static bool IsEdibleInReachButNotPermitted(RationMonitor.Instance smi)
	{
		return smi.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().edibleInReachButNotPermitted;
	}

	// Token: 0x040057D5 RID: 22485
	public StateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.FloatParameter rationsAteToday;

	// Token: 0x040057D6 RID: 22486
	public RationMonitor.RationsAvailableState rationsavailable;

	// Token: 0x040057D7 RID: 22487
	public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState outofrations;

	// Token: 0x0200160E RID: 5646
	public class EdibleAvailablestate : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040057D8 RID: 22488
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State readytoeat;

		// Token: 0x040057D9 RID: 22489
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State eating;
	}

	// Token: 0x0200160F RID: 5647
	public class RationsAvailableState : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040057DA RID: 22490
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState noediblesavailable;

		// Token: 0x040057DB RID: 22491
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblereachablebutnotpermitted;

		// Token: 0x040057DC RID: 22492
		public GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.HungrySubState ediblesunreachable;

		// Token: 0x040057DD RID: 22493
		public RationMonitor.EdibleAvailablestate edibleavailable;
	}

	// Token: 0x02001610 RID: 5648
	public new class Instance : GameStateMachine<RationMonitor, RationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060074F6 RID: 29942 RVA: 0x000F1524 File Offset: 0x000EF724
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.choreDriver = master.GetComponent<ChoreDriver>();
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x000F1539 File Offset: 0x000EF739
		public Edible GetEdible()
		{
			return base.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible();
		}

		// Token: 0x060074F8 RID: 29944 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool HasRationsAvailable()
		{
			return true;
		}

		// Token: 0x060074F9 RID: 29945 RVA: 0x000F154B File Offset: 0x000EF74B
		public float GetRationsAteToday()
		{
			return base.sm.rationsAteToday.Get(base.smi);
		}

		// Token: 0x060074FA RID: 29946 RVA: 0x000B95A1 File Offset: 0x000B77A1
		public float GetRationsRemaining()
		{
			return 1f;
		}

		// Token: 0x060074FB RID: 29947 RVA: 0x000F1563 File Offset: 0x000EF763
		public bool IsEating()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().choreType.urge == Db.Get().Urges.Eat;
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x000F159A File Offset: 0x000EF79A
		public void OnNewDay()
		{
			base.smi.sm.rationsAteToday.Set(0f, base.smi, false);
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x00314084 File Offset: 0x00312284
		public void OnEatComplete(object data)
		{
			Edible edible = (Edible)data;
			base.sm.rationsAteToday.Delta(edible.caloriesConsumed, base.smi);
			WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountConsumed(edible.FoodInfo.Id, edible.caloriesConsumed);
		}

		// Token: 0x040057DE RID: 22494
		private ChoreDriver choreDriver;
	}
}
