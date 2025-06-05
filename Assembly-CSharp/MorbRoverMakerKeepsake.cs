using System;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class MorbRoverMakerKeepsake : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>
{
	// Token: 0x06001521 RID: 5409 RVA: 0x0019D8B0 File Offset: 0x0019BAB0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.silent;
		this.silent.PlayAnim("silent").Enter(new StateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State.Callback(MorbRoverMakerKeepsake.CalculateNextActivationTime)).Update(new Action<MorbRoverMakerKeepsake.Instance, float>(MorbRoverMakerKeepsake.TimerUpdate), UpdateRate.SIM_200ms, false);
		this.talking.PlayAnim("idle").OnAnimQueueComplete(this.silent);
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x000B3CD4 File Offset: 0x000B1ED4
	public static void CalculateNextActivationTime(MorbRoverMakerKeepsake.Instance smi)
	{
		smi.CalculateNextActivationTime();
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x000B3CDC File Offset: 0x000B1EDC
	public static void TimerUpdate(MorbRoverMakerKeepsake.Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() > smi.NextActivationTime)
		{
			smi.GoTo(smi.sm.talking);
		}
	}

	// Token: 0x04000E83 RID: 3715
	public const string SILENT_ANIMATION_NAME = "silent";

	// Token: 0x04000E84 RID: 3716
	public const string TALKING_ANIMATION_NAME = "idle";

	// Token: 0x04000E85 RID: 3717
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State silent;

	// Token: 0x04000E86 RID: 3718
	public GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.State talking;

	// Token: 0x020004CD RID: 1229
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000E87 RID: 3719
		public Vector2 OperationalRandomnessRange = new Vector2(120f, 600f);
	}

	// Token: 0x020004CE RID: 1230
	public new class Instance : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>.GameInstance
	{
		// Token: 0x06001526 RID: 5414 RVA: 0x000B3D26 File Offset: 0x000B1F26
		public Instance(IStateMachineTarget master, MorbRoverMakerKeepsake.Def def) : base(master, def)
		{
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0019D920 File Offset: 0x0019BB20
		public void CalculateNextActivationTime()
		{
			float time = GameClock.Instance.GetTime();
			float minInclusive = time + base.def.OperationalRandomnessRange.x;
			float maxInclusive = time + base.def.OperationalRandomnessRange.y;
			this.NextActivationTime = UnityEngine.Random.Range(minInclusive, maxInclusive);
		}

		// Token: 0x04000E88 RID: 3720
		public float NextActivationTime = -1f;
	}
}
