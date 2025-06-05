using System;
using UnityEngine;

// Token: 0x02001172 RID: 4466
public class CallAdultMonitor : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>
{
	// Token: 0x06005B04 RID: 23300 RVA: 0x002A4E00 File Offset: 0x002A3000
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.CallAdultBehaviour, new StateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.Transition.ConditionCallback(CallAdultMonitor.ShouldCallAdult), delegate(CallAdultMonitor.Instance smi)
		{
			smi.RefreshCallTime();
		});
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x000DFBB7 File Offset: 0x000DDDB7
	public static bool ShouldCallAdult(CallAdultMonitor.Instance smi)
	{
		return Time.time >= smi.nextCallTime;
	}

	// Token: 0x02001173 RID: 4467
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040040C9 RID: 16585
		public float callMinInterval = 120f;

		// Token: 0x040040CA RID: 16586
		public float callMaxInterval = 240f;
	}

	// Token: 0x02001174 RID: 4468
	public new class Instance : GameStateMachine<CallAdultMonitor, CallAdultMonitor.Instance, IStateMachineTarget, CallAdultMonitor.Def>.GameInstance
	{
		// Token: 0x06005B08 RID: 23304 RVA: 0x000DFBEF File Offset: 0x000DDDEF
		public Instance(IStateMachineTarget master, CallAdultMonitor.Def def) : base(master, def)
		{
			this.RefreshCallTime();
		}

		// Token: 0x06005B09 RID: 23305 RVA: 0x000DFBFF File Offset: 0x000DDDFF
		public void RefreshCallTime()
		{
			this.nextCallTime = Time.time + UnityEngine.Random.value * (base.def.callMaxInterval - base.def.callMinInterval) + base.def.callMinInterval;
		}

		// Token: 0x040040CB RID: 16587
		public float nextCallTime;
	}
}
