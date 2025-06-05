using System;

// Token: 0x0200016B RID: 363
public class DiggerStates : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>
{
	// Token: 0x0600053D RID: 1341 RVA: 0x000AC363 File Offset: 0x000AA563
	private static bool ShouldStopHiding(DiggerStates.Instance smi)
	{
		return !GameplayEventManager.Instance.IsGameplayEventRunningWithTag(GameTags.SpaceDanger);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00161790 File Offset: 0x0015F990
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.move;
		this.move.MoveTo((DiggerStates.Instance smi) => smi.GetTunnelCell(), this.hide, this.behaviourcomplete, false);
		this.hide.Transition(this.behaviourcomplete, new StateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.Transition.ConditionCallback(DiggerStates.ShouldStopHiding), UpdateRate.SIM_4000ms);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Tunnel, false);
	}

	// Token: 0x040003DC RID: 988
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State move;

	// Token: 0x040003DD RID: 989
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State hide;

	// Token: 0x040003DE RID: 990
	public GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.State behaviourcomplete;

	// Token: 0x0200016C RID: 364
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200016D RID: 365
	public new class Instance : GameStateMachine<DiggerStates, DiggerStates.Instance, IStateMachineTarget, DiggerStates.Def>.GameInstance
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x000AC37F File Offset: 0x000AA57F
		public Instance(Chore<DiggerStates.Instance> chore, DiggerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Tunnel);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00161810 File Offset: 0x0015FA10
		public int GetTunnelCell()
		{
			DiggerMonitor.Instance smi = base.smi.GetSMI<DiggerMonitor.Instance>();
			if (smi != null)
			{
				return smi.lastDigCell;
			}
			return -1;
		}
	}
}
