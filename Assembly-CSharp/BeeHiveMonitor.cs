using System;

// Token: 0x02000129 RID: 297
public class BeeHiveMonitor : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>
{
	// Token: 0x0600046E RID: 1134 RVA: 0x0015F42C File Offset: 0x0015D62C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Nighttime, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.night, (BeeHiveMonitor.Instance smi) => GameClock.Instance.IsNighttime());
		this.night.EventTransition(GameHashes.NewDay, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.idle, (BeeHiveMonitor.Instance smi) => !GameClock.Instance.IsNighttime()).ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, new StateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.Transition.ConditionCallback(this.ShouldMakeHome), null);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000ABA3F File Offset: 0x000A9C3F
	public bool ShouldMakeHome(BeeHiveMonitor.Instance smi)
	{
		return !this.CanGoHome(smi);
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x000ABA4B File Offset: 0x000A9C4B
	public bool CanGoHome(BeeHiveMonitor.Instance smi)
	{
		return smi.gameObject.GetComponent<Bee>().FindHiveInRoom() != null;
	}

	// Token: 0x04000337 RID: 823
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State idle;

	// Token: 0x04000338 RID: 824
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State night;

	// Token: 0x0200012A RID: 298
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200012B RID: 299
	public new class Instance : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.GameInstance
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x000ABA6B File Offset: 0x000A9C6B
		public Instance(IStateMachineTarget master, BeeHiveMonitor.Def def) : base(master, def)
		{
		}
	}
}
