using System;

// Token: 0x020001A6 RID: 422
public class HiveEatingMonitor : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>
{
	// Token: 0x060005DD RID: 1501 RVA: 0x000ACAF0 File Offset: 0x000AACF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToEat, new StateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.Transition.ConditionCallback(HiveEatingMonitor.ShouldEat), null);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000ACB18 File Offset: 0x000AAD18
	public static bool ShouldEat(HiveEatingMonitor.Instance smi)
	{
		return smi.storage.FindFirst(smi.def.consumedOre) != null;
	}

	// Token: 0x020001A7 RID: 423
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400044A RID: 1098
		public Tag consumedOre;
	}

	// Token: 0x020001A8 RID: 424
	public new class Instance : GameStateMachine<HiveEatingMonitor, HiveEatingMonitor.Instance, IStateMachineTarget, HiveEatingMonitor.Def>.GameInstance
	{
		// Token: 0x060005E1 RID: 1505 RVA: 0x000ACB3E File Offset: 0x000AAD3E
		public Instance(IStateMachineTarget master, HiveEatingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0400044B RID: 1099
		[MyCmpReq]
		public Storage storage;
	}
}
