using System;

// Token: 0x020001E5 RID: 485
public class MoltStatesChore : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>
{
	// Token: 0x0600068F RID: 1679 RVA: 0x00164C84 File Offset: 0x00162E84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.molting;
		this.molting.PlayAnim((MoltStatesChore.Instance smi) => smi.def.moltAnimName, KAnim.PlayMode.Once).ScheduleGoTo(5f, this.complete).OnAnimQueueComplete(this.complete);
		this.complete.BehaviourComplete(GameTags.Creatures.ReadyToMolt, false);
	}

	// Token: 0x040004CB RID: 1227
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State molting;

	// Token: 0x040004CC RID: 1228
	public GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.State complete;

	// Token: 0x020001E6 RID: 486
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040004CD RID: 1229
		public string moltAnimName;
	}

	// Token: 0x020001E7 RID: 487
	public new class Instance : GameStateMachine<MoltStatesChore, MoltStatesChore.Instance, IStateMachineTarget, MoltStatesChore.Def>.GameInstance
	{
		// Token: 0x06000692 RID: 1682 RVA: 0x000AD25E File Offset: 0x000AB45E
		public Instance(Chore<MoltStatesChore.Instance> chore, MoltStatesChore.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ReadyToMolt);
		}
	}
}
