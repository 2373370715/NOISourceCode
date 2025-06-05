using System;
using UnityEngine;

// Token: 0x02000C2C RID: 3116
public class FliesFX : GameStateMachine<FliesFX, FliesFX.Instance>
{
	// Token: 0x06003B03 RID: 15107 RVA: 0x00236FE4 File Offset: 0x002351E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("swarm_pre").QueueAnim("swarm_loop", true, null).Exit("DestroyFX", delegate(FliesFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028CE RID: 10446
	public StateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02000C2D RID: 3117
	public new class Instance : GameStateMachine<FliesFX, FliesFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003B05 RID: 15109 RVA: 0x0023704C File Offset: 0x0023524C
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("fly_swarm_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000CA8E2 File Offset: 0x000C8AE2
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
