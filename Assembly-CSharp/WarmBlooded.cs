using System;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A86 RID: 6790
public class WarmBlooded : StateMachineComponent<WarmBlooded.StatesInstance>
{
	// Token: 0x06008D98 RID: 36248 RVA: 0x0010106D File Offset: 0x000FF26D
	public static bool IsCold(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsCold();
	}

	// Token: 0x06008D99 RID: 36249 RVA: 0x0010107F File Offset: 0x000FF27F
	public static bool IsHot(WarmBlooded.StatesInstance smi)
	{
		return !smi.IsSimpleHeatProducer() && smi.IsHot();
	}

	// Token: 0x06008D9A RID: 36250 RVA: 0x003767D8 File Offset: 0x003749D8
	public static void WarmingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.CoolingKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = smi.IdealTemperature - smi.BodyTemperature;
		float num3 = 1f;
		if ((num - smi.baseTemperatureModification.Value) * dt < num2)
		{
			num3 = Mathf.Clamp(num2 / ((num - smi.baseTemperatureModification.Value) * dt), 0f, 1f);
		}
		smi.bodyRegulator.SetValue(-num * num3);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.CoolingKW * num3 / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06008D9B RID: 36251 RVA: 0x0037689C File Offset: 0x00374A9C
	public static void CoolingRegulator(WarmBlooded.StatesInstance smi, float dt)
	{
		PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
		float num = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
		float num2 = SimUtil.EnergyFlowToTemperatureDelta(smi.master.WarmingKW, component.Element.specificHeatCapacity, component.Mass);
		float num3 = smi.IdealTemperature - smi.BodyTemperature;
		float num4 = 1f;
		if (num2 + num > num3)
		{
			num4 = Mathf.Max(0f, num3 - num) / num2;
		}
		smi.bodyRegulator.SetValue(num2 * num4);
		if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
		{
			smi.burningCalories.SetValue(-smi.master.WarmingKW * num4 * 1000f / smi.master.KCal2Joules);
		}
	}

	// Token: 0x06008D9C RID: 36252 RVA: 0x00101091 File Offset: 0x000FF291
	protected override void OnPrefabInit()
	{
		this.temperature = Db.Get().Amounts.Get(this.TemperatureAmountName).Lookup(base.gameObject);
		this.primaryElement = base.GetComponent<PrimaryElement>();
	}

	// Token: 0x06008D9D RID: 36253 RVA: 0x001010C5 File Offset: 0x000FF2C5
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06008D9E RID: 36254 RVA: 0x001010D2 File Offset: 0x000FF2D2
	public void SetTemperatureImmediate(float t)
	{
		this.temperature.value = t;
	}

	// Token: 0x04006ADC RID: 27356
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04006ADD RID: 27357
	public AmountInstance temperature;

	// Token: 0x04006ADE RID: 27358
	private PrimaryElement primaryElement;

	// Token: 0x04006ADF RID: 27359
	public WarmBlooded.ComplexityType complexity = WarmBlooded.ComplexityType.FullHomeostasis;

	// Token: 0x04006AE0 RID: 27360
	public string TemperatureAmountName = "Temperature";

	// Token: 0x04006AE1 RID: 27361
	public float IdealTemperature = DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;

	// Token: 0x04006AE2 RID: 27362
	public float BaseGenerationKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_BASE_GENERATION_KILOWATTS;

	// Token: 0x04006AE3 RID: 27363
	public string BaseTemperatureModifierDescription = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04006AE4 RID: 27364
	public float KCal2Joules = DUPLICANTSTATS.STANDARD.BaseStats.KCAL2JOULES;

	// Token: 0x04006AE5 RID: 27365
	public float WarmingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_WARMING_KILOWATTS;

	// Token: 0x04006AE6 RID: 27366
	public float CoolingKW = DUPLICANTSTATS.STANDARD.BaseStats.DUPLICANT_COOLING_KILOWATTS;

	// Token: 0x04006AE7 RID: 27367
	public string CaloriesModifierDescription = DUPLICANTS.MODIFIERS.BURNINGCALORIES.NAME;

	// Token: 0x04006AE8 RID: 27368
	public string BodyRegulatorModifierDescription = DUPLICANTS.MODIFIERS.HOMEOSTASIS.NAME;

	// Token: 0x04006AE9 RID: 27369
	public const float TRANSITION_DELAY_HOT = 3f;

	// Token: 0x04006AEA RID: 27370
	public const float TRANSITION_DELAY_COLD = 3f;

	// Token: 0x02001A87 RID: 6791
	public enum ComplexityType
	{
		// Token: 0x04006AEC RID: 27372
		SimpleHeatProduction,
		// Token: 0x04006AED RID: 27373
		HomeostasisWithoutCaloriesImpact,
		// Token: 0x04006AEE RID: 27374
		FullHomeostasis
	}

	// Token: 0x02001A88 RID: 6792
	public class StatesInstance : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.GameInstance
	{
		// Token: 0x06008DA0 RID: 36256 RVA: 0x00376A34 File Offset: 0x00374C34
		public StatesInstance(WarmBlooded smi) : base(smi)
		{
			this.baseTemperatureModification = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BaseTemperatureModifierDescription, false, true, false);
			base.master.GetAttributes().Add(this.baseTemperatureModification);
			if (base.master.complexity != WarmBlooded.ComplexityType.SimpleHeatProduction)
			{
				this.bodyRegulator = new AttributeModifier(base.master.TemperatureAmountName + "Delta", 0f, base.master.BodyRegulatorModifierDescription, false, true, false);
				base.master.GetAttributes().Add(this.bodyRegulator);
			}
			if (base.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
			{
				this.burningCalories = new AttributeModifier("CaloriesDelta", 0f, base.master.CaloriesModifierDescription, false, false, false);
				base.master.GetAttributes().Add(this.burningCalories);
			}
			base.master.SetTemperatureImmediate(this.IdealTemperature);
		}

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06008DA1 RID: 36257 RVA: 0x001010E0 File Offset: 0x000FF2E0
		public float IdealTemperature
		{
			get
			{
				return base.master.IdealTemperature;
			}
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06008DA2 RID: 36258 RVA: 0x001010ED File Offset: 0x000FF2ED
		public float TemperatureDelta
		{
			get
			{
				return this.bodyRegulator.Value;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06008DA3 RID: 36259 RVA: 0x001010FA File Offset: 0x000FF2FA
		public float BodyTemperature
		{
			get
			{
				return base.master.primaryElement.Temperature;
			}
		}

		// Token: 0x06008DA4 RID: 36260 RVA: 0x0010110C File Offset: 0x000FF30C
		public bool IsSimpleHeatProducer()
		{
			return base.master.complexity == WarmBlooded.ComplexityType.SimpleHeatProduction;
		}

		// Token: 0x06008DA5 RID: 36261 RVA: 0x0010111C File Offset: 0x000FF31C
		public bool IsHot()
		{
			return this.BodyTemperature > this.IdealTemperature;
		}

		// Token: 0x06008DA6 RID: 36262 RVA: 0x0010112C File Offset: 0x000FF32C
		public bool IsCold()
		{
			return this.BodyTemperature < this.IdealTemperature;
		}

		// Token: 0x04006AEF RID: 27375
		public AttributeModifier baseTemperatureModification;

		// Token: 0x04006AF0 RID: 27376
		public AttributeModifier bodyRegulator;

		// Token: 0x04006AF1 RID: 27377
		public AttributeModifier burningCalories;
	}

	// Token: 0x02001A89 RID: 6793
	public class States : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded>
	{
		// Token: 0x06008DA7 RID: 36263 RVA: 0x00376B40 File Offset: 0x00374D40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.normal;
			this.root.TagTransition(GameTags.Dead, this.dead, false).Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
				float value = SimUtil.EnergyFlowToTemperatureDelta(smi.master.BaseGenerationKW, component.Element.specificHeatCapacity, component.Mass);
				smi.baseTemperatureModification.SetValue(value);
				CreatureSimTemperatureTransfer component2 = smi.master.GetComponent<CreatureSimTemperatureTransfer>();
				component2.NonSimTemperatureModifiers.Add(smi.baseTemperatureModification);
				if (!smi.IsSimpleHeatProducer())
				{
					component2.NonSimTemperatureModifiers.Add(smi.bodyRegulator);
				}
			});
			this.alive.normal.Transition(this.alive.cold.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold), UpdateRate.SIM_200ms).Transition(this.alive.hot.transition, new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot), UpdateRate.SIM_200ms);
			this.alive.cold.transition.ScheduleGoTo(3f, this.alive.cold.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms);
			this.alive.cold.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsCold)), UpdateRate.SIM_200ms).Update("ColdRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.CoolingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
				if (smi.master.complexity == WarmBlooded.ComplexityType.FullHomeostasis)
				{
					smi.burningCalories.SetValue(0f);
				}
			});
			this.alive.hot.transition.ScheduleGoTo(3f, this.alive.hot.regulating).Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms);
			this.alive.hot.regulating.Transition(this.alive.normal, GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Not(new StateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.Transition.ConditionCallback(WarmBlooded.IsHot)), UpdateRate.SIM_200ms).Update("WarmRegulating", new Action<WarmBlooded.StatesInstance, float>(WarmBlooded.WarmingRegulator), UpdateRate.SIM_200ms, false).Exit(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.bodyRegulator.SetValue(0f);
			});
			this.dead.Enter(delegate(WarmBlooded.StatesInstance smi)
			{
				smi.master.enabled = false;
			});
		}

		// Token: 0x04006AF2 RID: 27378
		public WarmBlooded.States.AliveState alive;

		// Token: 0x04006AF3 RID: 27379
		public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State dead;

		// Token: 0x02001A8A RID: 6794
		public class RegulatingState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04006AF4 RID: 27380
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State transition;

			// Token: 0x04006AF5 RID: 27381
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State regulating;
		}

		// Token: 0x02001A8B RID: 6795
		public class AliveState : GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State
		{
			// Token: 0x04006AF6 RID: 27382
			public GameStateMachine<WarmBlooded.States, WarmBlooded.StatesInstance, WarmBlooded, object>.State normal;

			// Token: 0x04006AF7 RID: 27383
			public WarmBlooded.States.RegulatingState cold;

			// Token: 0x04006AF8 RID: 27384
			public WarmBlooded.States.RegulatingState hot;
		}
	}
}
