using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02001559 RID: 5465
public class BionicSuffocationMonitor : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>
{
	// Token: 0x060071DE RID: 29150 RVA: 0x0030B48C File Offset: 0x0030968C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.normal;
		this.root.TagTransition(GameTags.Dead, this.dead, false);
		this.normal.ToggleAttributeModifier("Breathing", (BionicSuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.noOxygen, (BionicSuffocationMonitor.Instance smi) => !smi.IsBreathing());
		this.noOxygen.EventTransition(GameHashes.OxygenBreatherHasAirChanged, this.normal, (BionicSuffocationMonitor.Instance smi) => smi.IsBreathing()).TagTransition(GameTags.RecoveringBreath, this.normal, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (BionicSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.noOxygen.holdingbreath);
		this.noOxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.noOxygen.suffocating, (BionicSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.noOxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (BionicSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(BionicSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x0400557A RID: 21882
	public BionicSuffocationMonitor.NoOxygenState noOxygen;

	// Token: 0x0400557B RID: 21883
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State normal;

	// Token: 0x0400557C RID: 21884
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State death;

	// Token: 0x0400557D RID: 21885
	public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State dead;

	// Token: 0x0200155A RID: 5466
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200155B RID: 5467
	public class NoOxygenState : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State
	{
		// Token: 0x0400557E RID: 21886
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State holdingbreath;

		// Token: 0x0400557F RID: 21887
		public GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.State suffocating;
	}

	// Token: 0x0200155C RID: 5468
	public new class Instance : GameStateMachine<BionicSuffocationMonitor, BionicSuffocationMonitor.Instance, IStateMachineTarget, BionicSuffocationMonitor.Def>.GameInstance
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060071E2 RID: 29154 RVA: 0x000EF011 File Offset: 0x000ED211
		// (set) Token: 0x060071E3 RID: 29155 RVA: 0x000EF019 File Offset: 0x000ED219
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x060071E4 RID: 29156 RVA: 0x0030B6B8 File Offset: 0x003098B8
		public Instance(IStateMachineTarget master, BionicSuffocationMonitor.Def def) : base(master, def)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float breath_RATE = DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE;
			this.breathing = new AttributeModifier(deltaAttribute.Id, breath_RATE, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -breath_RATE, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = base.GetComponent<OxygenBreather>();
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x000EF022 File Offset: 0x000ED222
		public bool IsBreathing()
		{
			return this.oxygenBreather.HasOxygen || base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x000EF05A File Offset: 0x000ED25A
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x000EF071 File Offset: 0x000ED271
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= DUPLICANTSTATS.STANDARD.Breath.SUFFOCATE_AMOUNT;
		}

		// Token: 0x060071E8 RID: 29160 RVA: 0x000EF0AB File Offset: 0x000ED2AB
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x04005580 RID: 21888
		private AmountInstance breath;

		// Token: 0x04005581 RID: 21889
		public AttributeModifier breathing;

		// Token: 0x04005582 RID: 21890
		public AttributeModifier holdingbreath;
	}
}
