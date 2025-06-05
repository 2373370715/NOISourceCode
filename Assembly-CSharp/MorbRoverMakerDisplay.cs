using System;

// Token: 0x020004C7 RID: 1223
public class MorbRoverMakerDisplay : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>
{
	// Token: 0x06001511 RID: 5393 RVA: 0x0019D5BC File Offset: 0x0019B7BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Never;
		default_state = this.off.idle;
		this.root.Target(this.monitor);
		this.off.DefaultState(this.off.idle);
		this.off.entering.PlayAnim("display_off").OnAnimQueueComplete(this.off.idle);
		this.off.idle.Target(this.masterTarget).EventTransition(GameHashes.TagsChanged, this.off.exiting, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.ShouldBeOn)).Target(this.monitor).PlayAnim("display_off_idle", KAnim.PlayMode.Loop);
		this.off.exiting.PlayAnim("display_on").OnAnimQueueComplete(this.on);
		this.on.Target(this.masterTarget).TagTransition(GameTags.Operational, this.off.entering, true).Target(this.monitor).DefaultState(this.on.idle);
		this.on.idle.Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_idle", KAnim.PlayMode.Loop);
		this.on.noGerm.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.germ, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.HasGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_no_germ", KAnim.PlayMode.Loop);
		this.on.germ.Transition(this.on.idle, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.GermsNoLongerNeeded), UpdateRate.SIM_200ms).Transition(this.on.noGerm, new StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.Transition.ConditionCallback(MorbRoverMakerDisplay.NoGermsAddedAndGermsAreNeeded), UpdateRate.SIM_200ms).PlayAnim("display_germ", KAnim.PlayMode.Loop);
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000B3BE5 File Offset: 0x000B1DE5
	public static bool NoGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && !smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x000B3BFA File Offset: 0x000B1DFA
	public static bool HasGermsAddedAndGermsAreNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.GermsAreNeeded && smi.HasRecentlyConsumedGerms;
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x000B3C0C File Offset: 0x000B1E0C
	public static bool ShouldBeOn(MorbRoverMakerDisplay.Instance smi)
	{
		return smi.ShouldBeOn();
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x000B3C14 File Offset: 0x000B1E14
	public static bool GermsNoLongerNeeded(MorbRoverMakerDisplay.Instance smi)
	{
		return !smi.GermsAreNeeded;
	}

	// Token: 0x04000E6C RID: 3692
	public const string METER_TARGET_NAME = "meter_display_target";

	// Token: 0x04000E6D RID: 3693
	public const string OFF_IDLE_ANIM_NAME = "display_off_idle";

	// Token: 0x04000E6E RID: 3694
	public const string OFF_ENTERING_ANIM_NAME = "display_off";

	// Token: 0x04000E6F RID: 3695
	public const string OFF_EXITING_ANIM_NAME = "display_on";

	// Token: 0x04000E70 RID: 3696
	public const string GERM_ICON_ANIM_NAME = "display_germ";

	// Token: 0x04000E71 RID: 3697
	public const string NO_GERM_ANIM_NAME = "display_no_germ";

	// Token: 0x04000E72 RID: 3698
	public const string ON_IDLE_ANIM_NAME = "display_idle";

	// Token: 0x04000E73 RID: 3699
	public StateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.TargetParameter monitor;

	// Token: 0x04000E74 RID: 3700
	public MorbRoverMakerDisplay.OffStates off;

	// Token: 0x04000E75 RID: 3701
	public MorbRoverMakerDisplay.OnStates on;

	// Token: 0x020004C8 RID: 1224
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000E76 RID: 3702
		public float Timeout = 1f;
	}

	// Token: 0x020004C9 RID: 1225
	public class OffStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04000E77 RID: 3703
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State entering;

		// Token: 0x04000E78 RID: 3704
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04000E79 RID: 3705
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State exiting;
	}

	// Token: 0x020004CA RID: 1226
	public class OnStates : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State
	{
		// Token: 0x04000E7A RID: 3706
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State idle;

		// Token: 0x04000E7B RID: 3707
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State shake;

		// Token: 0x04000E7C RID: 3708
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State noGerm;

		// Token: 0x04000E7D RID: 3709
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State germ;

		// Token: 0x04000E7E RID: 3710
		public GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.State checkmark;
	}

	// Token: 0x020004CB RID: 1227
	public new class Instance : GameStateMachine<MorbRoverMakerDisplay, MorbRoverMakerDisplay.Instance, IStateMachineTarget, MorbRoverMakerDisplay.Def>.GameInstance
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x000B3C42 File Offset: 0x000B1E42
		public bool HasRecentlyConsumedGerms
		{
			get
			{
				return GameClock.Instance.GetTime() - this.lastTimeGermsConsumed < base.def.Timeout;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x000B3C62 File Offset: 0x000B1E62
		public bool GermsAreNeeded
		{
			get
			{
				return this.morbRoverMaker.MorbDevelopment_Progress < 1f;
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x0019D7D4 File Offset: 0x0019B9D4
		public Instance(IStateMachineTarget master, MorbRoverMakerDisplay.Def def) : base(master, def)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			this.meter = new MeterController(component, "meter_display_target", "display_off_idle", Meter.Offset.UserSpecified, Grid.SceneLayer.BuildingFront, Array.Empty<string>());
			base.sm.monitor.Set(this.meter.gameObject, base.smi, false);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0019D83C File Offset: 0x0019BA3C
		public override void StartSM()
		{
			this.morbRoverMaker = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			MorbRoverMaker.Instance instance = this.morbRoverMaker;
			instance.GermsAdded = (Action<long>)Delegate.Combine(instance.GermsAdded, new Action<long>(this.OnGermsAdded));
			MorbRoverMaker.Instance instance2 = this.morbRoverMaker;
			instance2.OnUncovered = (System.Action)Delegate.Combine(instance2.OnUncovered, new System.Action(this.OnUncovered));
			base.StartSM();
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000B3C76 File Offset: 0x000B1E76
		private void OnGermsAdded(long amount)
		{
			this.lastTimeGermsConsumed = GameClock.Instance.GetTime();
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000B3C88 File Offset: 0x000B1E88
		public bool ShouldBeOn()
		{
			return this.morbRoverMaker.HasBeenRevealed && this.operational.IsOperational;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x000B3CA4 File Offset: 0x000B1EA4
		private void OnUncovered()
		{
			if (base.IsInsideState(base.sm.off.idle))
			{
				this.GoTo(base.sm.off.exiting);
			}
		}

		// Token: 0x04000E7F RID: 3711
		private float lastTimeGermsConsumed = -1f;

		// Token: 0x04000E80 RID: 3712
		[MyCmpReq]
		private Operational operational;

		// Token: 0x04000E81 RID: 3713
		private MorbRoverMaker.Instance morbRoverMaker;

		// Token: 0x04000E82 RID: 3714
		private MeterController meter;
	}
}
