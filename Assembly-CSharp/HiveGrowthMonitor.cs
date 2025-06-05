using System;

// Token: 0x020001AE RID: 430
public class HiveGrowthMonitor : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
	// Token: 0x060005ED RID: 1517 RVA: 0x000ACBE5 File Offset: 0x000AADE5
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing), null);
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x000ACC0D File Offset: 0x000AAE0D
	public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
	{
		return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
	}

	// Token: 0x020001AF RID: 431
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001B0 RID: 432
	public new class Instance : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x000ACC25 File Offset: 0x000AAE25
		public Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : base(master, def)
		{
		}
	}
}
