using System;
using UnityEngine;

// Token: 0x02000C39 RID: 3129
public class SuperProductiveFX : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance>
{
	// Token: 0x06003B2F RID: 15151 RVA: 0x00237CDC File Offset: 0x00235EDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (SuperProductiveFX.Instance smi) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("productive_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("productive_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("productive_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("productive_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(SuperProductiveFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028F2 RID: 10482
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x040028F3 RID: 10483
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x040028F4 RID: 10484
	public StateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040028F5 RID: 10485
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x040028F6 RID: 10486
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040028F7 RID: 10487
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x040028F8 RID: 10488
	public GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02000C3A RID: 3130
	public new class Instance : GameStateMachine<SuperProductiveFX, SuperProductiveFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003B31 RID: 15153 RVA: 0x00237DCC File Offset: 0x00235FCC
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("productive_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000CAA63 File Offset: 0x000C8C63
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
