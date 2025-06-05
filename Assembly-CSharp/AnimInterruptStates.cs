using System;

// Token: 0x02000107 RID: 263
public class AnimInterruptStates : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>
{
	// Token: 0x06000413 RID: 1043 RVA: 0x000AB5E0 File Offset: 0x000A97E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.play_anim;
		this.play_anim.Enter(new StateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State.Callback(this.PlayAnim)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.PlayInterruptAnim, false);
	}

	// Token: 0x06000414 RID: 1044 RVA: 0x0015DF10 File Offset: 0x0015C110
	private void PlayAnim(AnimInterruptStates.Instance smi)
	{
		KBatchedAnimController kbatchedAnimController = smi.Get<KBatchedAnimController>();
		HashedString[] anims = smi.GetSMI<AnimInterruptMonitor.Instance>().anims;
		kbatchedAnimController.Play(anims[0], KAnim.PlayMode.Once, 1f, 0f);
		for (int i = 1; i < anims.Length; i++)
		{
			kbatchedAnimController.Queue(anims[i], KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x040002E8 RID: 744
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State play_anim;

	// Token: 0x040002E9 RID: 745
	public GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.State behaviourcomplete;

	// Token: 0x02000108 RID: 264
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000109 RID: 265
	public new class Instance : GameStateMachine<AnimInterruptStates, AnimInterruptStates.Instance, IStateMachineTarget, AnimInterruptStates.Def>.GameInstance
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x000AB627 File Offset: 0x000A9827
		public Instance(Chore<AnimInterruptStates.Instance> chore, AnimInterruptStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.PlayInterruptAnim);
		}
	}
}
