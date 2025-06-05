using System;

// Token: 0x020019B6 RID: 6582
public class ClusterMapFXAnimator : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x0600894D RID: 35149 RVA: 0x00366428 File Offset: 0x00364628
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.play;
		this.play.OnSignal(this.onAnimComplete, this.finished);
		this.finished.Enter(delegate(ClusterMapFXAnimator.StatesInstance smi)
		{
			smi.DestroyEntity();
		});
	}

	// Token: 0x040067D9 RID: 26585
	private KBatchedAnimController animController;

	// Token: 0x040067DA RID: 26586
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x040067DB RID: 26587
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State play;

	// Token: 0x040067DC RID: 26588
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State finished;

	// Token: 0x040067DD RID: 26589
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.Signal onAnimComplete;

	// Token: 0x020019B7 RID: 6583
	public class StatesInstance : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600894F RID: 35151 RVA: 0x000FE40C File Offset: 0x000FC60C
		public StatesInstance(ClusterMapVisualizer visualizer, ClusterGridEntity entity) : base(visualizer)
		{
			base.sm.entityTarget.Set(entity, this);
			visualizer.GetFirstAnimController().gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
		}

		// Token: 0x06008950 RID: 35152 RVA: 0x000FE449 File Offset: 0x000FC649
		private void OnAnimQueueComplete(object data)
		{
			base.sm.onAnimComplete.Trigger(this);
		}

		// Token: 0x06008951 RID: 35153 RVA: 0x000FE45C File Offset: 0x000FC65C
		public void DestroyEntity()
		{
			base.sm.entityTarget.Get(this).DeleteObject();
		}
	}
}
