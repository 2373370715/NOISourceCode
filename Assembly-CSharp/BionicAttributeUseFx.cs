using System;
using UnityEngine;

// Token: 0x02000C24 RID: 3108
public class BionicAttributeUseFx : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance>
{
	// Token: 0x06003AE5 RID: 15077 RVA: 0x00236A7C File Offset: 0x00234C7C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (BionicAttributeUseFx.Instance smi) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("bionic_upgrade_active_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("bionic_upgrade_active_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("bionic_upgrade_active_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("bionic_upgrade_active_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BionicAttributeUseFx.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028B4 RID: 10420
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x040028B5 RID: 10421
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x040028B6 RID: 10422
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040028B7 RID: 10423
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x040028B8 RID: 10424
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040028B9 RID: 10425
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x040028BA RID: 10426
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02000C25 RID: 3109
	public new class Instance : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003AE7 RID: 15079 RVA: 0x00236B6C File Offset: 0x00234D6C
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("bionic_upgrade_active_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x000CA7C1 File Offset: 0x000C89C1
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
