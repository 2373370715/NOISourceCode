using System;

// Token: 0x0200115E RID: 4446
public class AnimInterruptMonitor : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>
{
	// Token: 0x06005AC2 RID: 23234 RVA: 0x000DF807 File Offset: 0x000DDA07
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.PlayInterruptAnim, new StateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.Transition.ConditionCallback(AnimInterruptMonitor.ShoulPlayAnim), new Action<AnimInterruptMonitor.Instance>(AnimInterruptMonitor.ClearAnim));
	}

	// Token: 0x06005AC3 RID: 23235 RVA: 0x000DF83A File Offset: 0x000DDA3A
	private static bool ShoulPlayAnim(AnimInterruptMonitor.Instance smi)
	{
		return smi.anims != null;
	}

	// Token: 0x06005AC4 RID: 23236 RVA: 0x000DF845 File Offset: 0x000DDA45
	private static void ClearAnim(AnimInterruptMonitor.Instance smi)
	{
		smi.anims = null;
	}

	// Token: 0x0200115F RID: 4447
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001160 RID: 4448
	public new class Instance : GameStateMachine<AnimInterruptMonitor, AnimInterruptMonitor.Instance, IStateMachineTarget, AnimInterruptMonitor.Def>.GameInstance
	{
		// Token: 0x06005AC7 RID: 23239 RVA: 0x000DF856 File Offset: 0x000DDA56
		public Instance(IStateMachineTarget master, AnimInterruptMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x000DF860 File Offset: 0x000DDA60
		public void PlayAnim(HashedString anim)
		{
			this.PlayAnimSequence(new HashedString[]
			{
				anim
			});
		}

		// Token: 0x06005AC9 RID: 23241 RVA: 0x000DF876 File Offset: 0x000DDA76
		public void PlayAnimSequence(HashedString[] anims)
		{
			this.anims = anims;
			base.GetComponent<CreatureBrain>().UpdateBrain();
		}

		// Token: 0x0400409B RID: 16539
		public HashedString[] anims;
	}
}
