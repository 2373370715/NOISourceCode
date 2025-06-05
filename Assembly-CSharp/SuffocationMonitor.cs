using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200164C RID: 5708
public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>
{
	// Token: 0x0600760B RID: 30219 RVA: 0x00317204 File Offset: 0x00315404
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.satisfied;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuffocationMonitor.Instance smi) => smi.increaseBreathModifier, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.CanBreath()).Transition(this.noOxygen, (SuffocationMonitor.Instance smi) => !smi.CanBreath(), UpdateRate.SIM_200ms);
		this.satisfied.normal.Transition(this.satisfied.low, (SuffocationMonitor.Instance smi) => smi.oxygenBreather.IsLowOxygen(), UpdateRate.SIM_200ms);
		this.satisfied.low.Transition(this.satisfied.normal, (SuffocationMonitor.Instance smi) => !smi.oxygenBreather.IsLowOxygen(), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.satisfied, (SuffocationMonitor.Instance smi) => smi.CanBreath()).TagTransition(GameTags.RecoveringBreath, this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuffocationMonitor.Instance smi) => smi.decreaseBreathModifier, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (SuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x040058C0 RID: 22720
	public SuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x040058C1 RID: 22721
	public SuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x040058C2 RID: 22722
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State death;

	// Token: 0x040058C3 RID: 22723
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State dead;

	// Token: 0x0200164D RID: 5709
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200164E RID: 5710
	public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
	{
		// Token: 0x040058C4 RID: 22724
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x040058C5 RID: 22725
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x0200164F RID: 5711
	public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State
	{
		// Token: 0x040058C6 RID: 22726
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State normal;

		// Token: 0x040058C7 RID: 22727
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.State low;
	}

	// Token: 0x02001650 RID: 5712
	public new class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, SuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06007610 RID: 30224 RVA: 0x000F21DA File Offset: 0x000F03DA
		// (set) Token: 0x06007611 RID: 30225 RVA: 0x000F21E2 File Offset: 0x000F03E2
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x06007612 RID: 30226 RVA: 0x003174EC File Offset: 0x003156EC
		public Instance(IStateMachineTarget master, SuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.increaseBreathModifier = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.decreaseBreathModifier = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x000F21EB File Offset: 0x000F03EB
		public override void StartSM()
		{
			base.StartSM();
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x000F21F3 File Offset: 0x000F03F3
		public bool CanBreath()
		{
			return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.oxygenBreather.HasOxygen;
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x000F2230 File Offset: 0x000F0430
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x06007616 RID: 30230 RVA: 0x000F2247 File Offset: 0x000F0447
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x000EF0AB File Offset: 0x000ED2AB
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x040058C8 RID: 22728
		private AmountInstance breath;

		// Token: 0x040058C9 RID: 22729
		public AttributeModifier increaseBreathModifier;

		// Token: 0x040058CA RID: 22730
		public AttributeModifier decreaseBreathModifier;
	}
}
