using System;

// Token: 0x02001185 RID: 4485
public class CreatureSleepMonitor : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>
{
	// Token: 0x06005B5D RID: 23389 RVA: 0x000DFF13 File Offset: 0x000DE113
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.SleepBehaviour, new StateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.Transition.ConditionCallback(CreatureSleepMonitor.ShouldSleep), null);
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x000ABA88 File Offset: 0x000A9C88
	public static bool ShouldSleep(CreatureSleepMonitor.Instance smi)
	{
		return GameClock.Instance.IsNighttime();
	}

	// Token: 0x02001186 RID: 4486
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001187 RID: 4487
	public new class Instance : GameStateMachine<CreatureSleepMonitor, CreatureSleepMonitor.Instance, IStateMachineTarget, CreatureSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06005B61 RID: 23393 RVA: 0x000DFF43 File Offset: 0x000DE143
		public Instance(IStateMachineTarget master, CreatureSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
