using System;
using Klei.AI;
using TUNING;

// Token: 0x020015B1 RID: 5553
public class ExternalTemperatureMonitor : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance>
{
	// Token: 0x0600734E RID: 29518 RVA: 0x000F0011 File Offset: 0x000EE211
	public static float GetExternalColdThreshold(Attributes affected_attributes)
	{
		return -0.039f;
	}

	// Token: 0x0600734F RID: 29519 RVA: 0x000F0018 File Offset: 0x000EE218
	public static float GetExternalWarmThreshold(Attributes affected_attributes)
	{
		return 0.008f;
	}

	// Token: 0x06007350 RID: 29520 RVA: 0x0030EEE0 File Offset: 0x0030D0E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.transitionToTooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).Transition(this.transitionToTooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms);
		this.transitionToTooWarm.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot(), UpdateRate.SIM_200ms).Transition(this.tooWarm, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooHot() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.transitionToTooCool.Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold(), UpdateRate.SIM_200ms).Transition(this.tooCool, (ExternalTemperatureMonitor.Instance smi) => smi.IsTooCold() && smi.timeinstate > 1f, UpdateRate.SIM_200ms);
		this.tooWarm.ToggleTag(GameTags.FeelingWarm).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooHot() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooHot()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
		this.tooCool.ToggleTag(GameTags.FeelingCold).Transition(this.comfortable, (ExternalTemperatureMonitor.Instance smi) => !smi.IsTooCold() && smi.timeinstate > 6f, UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.EffectAdded, this.comfortable, (ExternalTemperatureMonitor.Instance smi, object obj) => !smi.IsTooCold()).Enter(delegate(ExternalTemperatureMonitor.Instance smi)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_ThermalComfort, true);
		});
	}

	// Token: 0x04005676 RID: 22134
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State comfortable;

	// Token: 0x04005677 RID: 22135
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooWarm;

	// Token: 0x04005678 RID: 22136
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooWarm;

	// Token: 0x04005679 RID: 22137
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State transitionToTooCool;

	// Token: 0x0400567A RID: 22138
	public GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.State tooCool;

	// Token: 0x0400567B RID: 22139
	private const float BODY_TEMPERATURE_AFFECT_EXTERNAL_FEEL_THRESHOLD = 0.5f;

	// Token: 0x0400567C RID: 22140
	public static readonly float BASE_STRESS_TOLERANCE_COLD = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS * 0.2f;

	// Token: 0x0400567D RID: 22141
	public static readonly float BASE_STRESS_TOLERANCE_WARM = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS * 0.2f;

	// Token: 0x0400567E RID: 22142
	private const float START_GAME_AVERAGING_DELAY = 6f;

	// Token: 0x0400567F RID: 22143
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04005680 RID: 22144
	private const float TRANSITION_OUT_DELAY = 6f;

	// Token: 0x020015B2 RID: 5554
	public new class Instance : GameStateMachine<ExternalTemperatureMonitor, ExternalTemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06007353 RID: 29523 RVA: 0x000F005D File Offset: 0x000EE25D
		public float GetCurrentColdThreshold
		{
			get
			{
				if (this.internalTemperatureMonitor.IdealTemperatureDelta() > 0.5f)
				{
					return 0f;
				}
				return CreatureSimTemperatureTransfer.PotentialEnergyFlowToCreature(Grid.PosToCell(base.gameObject), this.primaryElement, this.temperatureTransferer, 1f);
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06007354 RID: 29524 RVA: 0x000F0098 File Offset: 0x000EE298
		public float GetCurrentHotThreshold
		{
			get
			{
				return this.HotThreshold;
			}
		}

		// Token: 0x06007355 RID: 29525 RVA: 0x0030F12C File Offset: 0x0030D32C
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.attributes = base.gameObject.GetAttributes();
			this.internalTemperatureMonitor = base.gameObject.GetSMI<TemperatureMonitor.Instance>();
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.temperatureTransferer = base.gameObject.GetComponent<CreatureSimTemperatureTransfer>();
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			this.effects = base.gameObject.GetComponent<Effects>();
			this.traits = base.gameObject.GetComponent<Traits>();
		}

		// Token: 0x06007356 RID: 29526 RVA: 0x0030F240 File Offset: 0x0030D440
		public bool IsTooHot()
		{
			return !this.effects.HasEffect("RefreshingTouch") && !this.effects.HasImmunityTo(this.warmAirEffect) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage > ExternalTemperatureMonitor.GetExternalWarmThreshold(base.smi.attributes);
		}

		// Token: 0x06007357 RID: 29527 RVA: 0x0030F2B0 File Offset: 0x0030D4B0
		public bool IsTooCold()
		{
			for (int i = 0; i < this.immunityToColdEffects.Length; i++)
			{
				if (this.effects.HasEffect(this.immunityToColdEffects[i]))
				{
					return false;
				}
			}
			return !this.effects.HasImmunityTo(this.coldAirEffect) && (!(this.traits != null) || !this.traits.IsEffectIgnored(this.coldAirEffect)) && !WarmthProvider.IsWarmCell(Grid.PosToCell(this)) && this.temperatureTransferer.LastTemperatureRecordIsReliable && base.smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage < ExternalTemperatureMonitor.GetExternalColdThreshold(base.smi.attributes);
		}

		// Token: 0x04005681 RID: 22145
		public float HotThreshold = 306.15f;

		// Token: 0x04005682 RID: 22146
		public Effects effects;

		// Token: 0x04005683 RID: 22147
		public Traits traits;

		// Token: 0x04005684 RID: 22148
		public Attributes attributes;

		// Token: 0x04005685 RID: 22149
		public AmountInstance internalTemperature;

		// Token: 0x04005686 RID: 22150
		private TemperatureMonitor.Instance internalTemperatureMonitor;

		// Token: 0x04005687 RID: 22151
		public CreatureSimTemperatureTransfer temperatureTransferer;

		// Token: 0x04005688 RID: 22152
		public PrimaryElement primaryElement;

		// Token: 0x04005689 RID: 22153
		private Effect warmAirEffect = Db.Get().effects.Get("WarmAir");

		// Token: 0x0400568A RID: 22154
		private Effect coldAirEffect = Db.Get().effects.Get("ColdAir");

		// Token: 0x0400568B RID: 22155
		private Effect[] immunityToColdEffects = new Effect[]
		{
			Db.Get().effects.Get("WarmTouch"),
			Db.Get().effects.Get("WarmTouchFood")
		};
	}
}
