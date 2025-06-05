using System;
using UnityEngine;

// Token: 0x02000C2F RID: 3119
public class GameplayEventFX : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance>
{
	// Token: 0x06003B0A RID: 15114 RVA: 0x002370B8 File Offset: 0x002352B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("event_pre").OnAnimQueueComplete(this.single).Exit("DestroyFX", delegate(GameplayEventFX.Instance smi)
		{
			smi.DestroyFX();
		});
		this.single.PlayAnim("event_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.multiple, (GameplayEventFX.Instance smi, int p) => p > 1);
		this.multiple.PlayAnim("event_loop_multiple", KAnim.PlayMode.Loop).ParamTransition<int>(this.notificationCount, this.single, (GameplayEventFX.Instance smi, int p) => p == 1);
	}

	// Token: 0x040028D1 RID: 10449
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040028D2 RID: 10450
	public StateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.IntParameter notificationCount;

	// Token: 0x040028D3 RID: 10451
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State single;

	// Token: 0x040028D4 RID: 10452
	public GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.State multiple;

	// Token: 0x02000C30 RID: 3120
	public new class Instance : GameStateMachine<GameplayEventFX, GameplayEventFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003B0C RID: 15116 RVA: 0x002371A4 File Offset: 0x002353A4
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("event_alert_fx_kanim", base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x000CA91B File Offset: 0x000C8B1B
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040028D5 RID: 10453
		public int previousCount;
	}
}
