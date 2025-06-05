using System;

// Token: 0x02000A49 RID: 2633
public class IncubatorMonitor : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>
{
	// Token: 0x06002F9E RID: 12190 RVA: 0x00206558 File Offset: 0x00204758
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.not;
		this.not.EventTransition(GameHashes.OnStore, this.in_incubator, new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator));
		this.in_incubator.ToggleTag(GameTags.Creatures.InIncubator).EventTransition(GameHashes.OnStore, this.not, GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Not(new StateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.Transition.ConditionCallback(IncubatorMonitor.InIncubator)));
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x000C35FA File Offset: 0x000C17FA
	public static bool InIncubator(IncubatorMonitor.Instance smi)
	{
		return smi.gameObject.transform.parent && smi.gameObject.transform.parent.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x040020C8 RID: 8392
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State not;

	// Token: 0x040020C9 RID: 8393
	public GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.State in_incubator;

	// Token: 0x02000A4A RID: 2634
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A4B RID: 2635
	public new class Instance : GameStateMachine<IncubatorMonitor, IncubatorMonitor.Instance, IStateMachineTarget, IncubatorMonitor.Def>.GameInstance
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x000C3638 File Offset: 0x000C1838
		public Instance(IStateMachineTarget master, IncubatorMonitor.Def def) : base(master, def)
		{
		}
	}
}
