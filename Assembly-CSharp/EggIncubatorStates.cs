using System;
using UnityEngine;

// Token: 0x02000D85 RID: 3461
public class EggIncubatorStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance>
{
	// Token: 0x0600434D RID: 17229 RVA: 0x002526EC File Offset: 0x002508EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.empty;
		this.empty.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OccupantChanged, this.egg, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasEgg)).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby));
		this.egg.DefaultState(this.egg.unpowered).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasAny))).EventTransition(GameHashes.OccupantChanged, this.baby, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)).ToggleStatusItem(Db.Get().BuildingStatusItems.IncubatorProgress, (EggIncubatorStates.Instance smi) => smi.master.GetComponent<EggIncubator>());
		this.egg.lose_power.PlayAnim("no_power_pre").EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)).OnAnimQueueComplete(this.egg.unpowered);
		this.egg.unpowered.PlayAnim("no_power_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.egg.incubating, new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational));
		this.egg.incubating.PlayAnim("no_power_pst").QueueAnim("working_loop", true, null).EventTransition(GameHashes.OperationalChanged, this.egg.lose_power, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.IsOperational)));
		this.baby.DefaultState(this.baby.idle).EventTransition(GameHashes.OccupantChanged, this.empty, GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Not(new StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(EggIncubatorStates.HasBaby)));
		this.baby.idle.PlayAnim("no_power_pre").QueueAnim("no_power_loop", true, null);
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x000CFE87 File Offset: 0x000CE087
	public static bool IsOperational(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x002528F4 File Offset: 0x00250AF4
	public static bool HasEgg(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Egg);
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x00252924 File Offset: 0x00250B24
	public static bool HasBaby(EggIncubatorStates.Instance smi)
	{
		GameObject occupant = smi.GetComponent<EggIncubator>().Occupant;
		return occupant && occupant.HasTag(GameTags.Creature);
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x000CFE94 File Offset: 0x000CE094
	public static bool HasAny(EggIncubatorStates.Instance smi)
	{
		return smi.GetComponent<EggIncubator>().Occupant;
	}

	// Token: 0x04002E87 RID: 11911
	public StateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.BoolParameter readyToHatch;

	// Token: 0x04002E88 RID: 11912
	public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State empty;

	// Token: 0x04002E89 RID: 11913
	public EggIncubatorStates.EggStates egg;

	// Token: 0x04002E8A RID: 11914
	public EggIncubatorStates.BabyStates baby;

	// Token: 0x02000D86 RID: 3462
	public class EggStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04002E8B RID: 11915
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State incubating;

		// Token: 0x04002E8C RID: 11916
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State lose_power;

		// Token: 0x04002E8D RID: 11917
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State unpowered;
	}

	// Token: 0x02000D87 RID: 3463
	public class BabyStates : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04002E8E RID: 11918
		public GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.State idle;
	}

	// Token: 0x02000D88 RID: 3464
	public new class Instance : GameStateMachine<EggIncubatorStates, EggIncubatorStates.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06004355 RID: 17237 RVA: 0x000CFEB6 File Offset: 0x000CE0B6
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
