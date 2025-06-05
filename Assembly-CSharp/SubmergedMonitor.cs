using System;

// Token: 0x0200121A RID: 4634
public class SubmergedMonitor : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>
{
	// Token: 0x06005DF4 RID: 24052 RVA: 0x002AE7C8 File Offset: 0x002AC9C8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Hover);
		}, UpdateRate.SIM_1000ms, false).Transition(this.submerged, (SubmergedMonitor.Instance smi) => smi.IsSubmerged(), UpdateRate.SIM_1000ms);
		this.submerged.Enter("SetNavType", delegate(SubmergedMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}).Update("SetNavType", delegate(SubmergedMonitor.Instance smi, float dt)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Swim);
		}, UpdateRate.SIM_1000ms, false).Transition(this.satisfied, (SubmergedMonitor.Instance smi) => !smi.IsSubmerged(), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Submerged);
	}

	// Token: 0x0400430F RID: 17167
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State satisfied;

	// Token: 0x04004310 RID: 17168
	public GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.State submerged;

	// Token: 0x0200121B RID: 4635
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200121C RID: 4636
	public new class Instance : GameStateMachine<SubmergedMonitor, SubmergedMonitor.Instance, IStateMachineTarget, SubmergedMonitor.Def>.GameInstance
	{
		// Token: 0x06005DF7 RID: 24055 RVA: 0x000E1C06 File Offset: 0x000DFE06
		public Instance(IStateMachineTarget master, SubmergedMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005DF8 RID: 24056 RVA: 0x000E1C10 File Offset: 0x000DFE10
		public bool IsSubmerged()
		{
			return Grid.IsSubstantialLiquid(Grid.PosToCell(base.transform.GetPosition()), 0.35f);
		}
	}
}
