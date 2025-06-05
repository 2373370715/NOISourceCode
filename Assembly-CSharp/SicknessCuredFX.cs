using System;
using UnityEngine;

// Token: 0x02000C36 RID: 3126
public class SicknessCuredFX : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance>
{
	// Token: 0x06003B28 RID: 15144 RVA: 0x00237C14 File Offset: 0x00235E14
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).Exit("DestroyFX", delegate(SicknessCuredFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028EF RID: 10479
	public StateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02000C37 RID: 3127
	public new class Instance : GameStateMachine<SicknessCuredFX, SicknessCuredFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003B2A RID: 15146 RVA: 0x00237C78 File Offset: 0x00235E78
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("recentlyhealed_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x000CAA2A File Offset: 0x000C8C2A
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}
}
