using System;
using UnityEngine;

// Token: 0x02000C27 RID: 3111
public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
	// Token: 0x06003AED RID: 15085 RVA: 0x00236BD0 File Offset: 0x00234DD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

	// Token: 0x040028BE RID: 10430
	public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040028BF RID: 10431
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

	// Token: 0x040028C0 RID: 10432
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

	// Token: 0x02000C28 RID: 3112
	public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003AEF RID: 15087 RVA: 0x00236C68 File Offset: 0x00234E68
		public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x000CA822 File Offset: 0x000C8A22
		public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x000CA84A File Offset: 0x000C8A4A
		public void Exit()
		{
			this.DestroyFX();
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x000CA84A File Offset: 0x000C8A4A
		private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x000CA852 File Offset: 0x000C8A52
		private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040028C1 RID: 10433
		private string anim;

		// Token: 0x040028C2 RID: 10434
		private KAnim.PlayMode mode;

		// Token: 0x040028C3 RID: 10435
		private KBatchedAnimController animController;
	}
}
