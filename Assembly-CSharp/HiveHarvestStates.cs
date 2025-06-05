using System;

// Token: 0x020001B1 RID: 433
public class HiveHarvestStates : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>
{
	// Token: 0x060005F2 RID: 1522 RVA: 0x000ACC2F File Offset: 0x000AAE2F
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x020001B2 RID: 434
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001B3 RID: 435
	public new class Instance : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>.GameInstance
	{
		// Token: 0x060005F5 RID: 1525 RVA: 0x000ACC4D File Offset: 0x000AAE4D
		public Instance(Chore<HiveHarvestStates.Instance> chore, HiveHarvestStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.HarvestHiveBehaviour);
		}
	}
}
