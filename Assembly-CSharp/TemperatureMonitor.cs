using System;
using Klei.AI;
using TUNING;

// Token: 0x02001658 RID: 5720
public class TemperatureMonitor : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance>
{
	// Token: 0x06007639 RID: 30265 RVA: 0x00317A48 File Offset: 0x00315C48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.homeostatic;
		this.root.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.averageTemperature = smi.primaryElement.Temperature;
		}).Update("UpdateTemperature", delegate(TemperatureMonitor.Instance smi, float dt)
		{
			smi.UpdateTemperature(dt);
		}, UpdateRate.SIM_200ms, false);
		this.homeostatic.Transition(this.hyperthermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHyperthermic(), UpdateRate.SIM_200ms).Transition(this.hypothermic_pre, (TemperatureMonitor.Instance smi) => smi.IsHypothermic(), UpdateRate.SIM_200ms).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null);
		this.hyperthermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hyperthermic);
		});
		this.hypothermic_pre.Enter(delegate(TemperatureMonitor.Instance smi)
		{
			smi.GoTo(this.hypothermic);
		});
		this.hyperthermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHyperthermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.CoolDown);
		this.hypothermic.Transition(this.homeostatic, (TemperatureMonitor.Instance smi) => !smi.IsHypothermic(), UpdateRate.SIM_200ms).ToggleUrge(Db.Get().Urges.WarmUp);
	}

	// Token: 0x040058E7 RID: 22759
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State homeostatic;

	// Token: 0x040058E8 RID: 22760
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic;

	// Token: 0x040058E9 RID: 22761
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic;

	// Token: 0x040058EA RID: 22762
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hyperthermic_pre;

	// Token: 0x040058EB RID: 22763
	public GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.State hypothermic_pre;

	// Token: 0x040058EC RID: 22764
	private const float TEMPERATURE_AVERAGING_RANGE = 4f;

	// Token: 0x040058ED RID: 22765
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter warmUpCell;

	// Token: 0x040058EE RID: 22766
	public StateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.IntParameter coolDownCell;

	// Token: 0x02001659 RID: 5721
	public new class Instance : GameStateMachine<TemperatureMonitor, TemperatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600763D RID: 30269 RVA: 0x00317BD8 File Offset: 0x00315DD8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.primaryElement = base.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.warmUpQuery = new SafetyQuery(Game.Instance.safetyConditions.WarmUpChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.coolDownQuery = new SafetyQuery(Game.Instance.safetyConditions.CoolDownChecker, base.GetComponent<KMonoBehaviour>(), int.MaxValue);
			this.navigator = base.GetComponent<Navigator>();
		}

		// Token: 0x0600763E RID: 30270 RVA: 0x00317C84 File Offset: 0x00315E84
		public void UpdateTemperature(float dt)
		{
			base.smi.averageTemperature *= 1f - dt / 4f;
			base.smi.averageTemperature += base.smi.primaryElement.Temperature * (dt / 4f);
			base.smi.temperature.SetValue(base.smi.averageTemperature);
		}

		// Token: 0x0600763F RID: 30271 RVA: 0x000F23A4 File Offset: 0x000F05A4
		public bool IsHyperthermic()
		{
			return this.temperature.value > this.HyperthermiaThreshold;
		}

		// Token: 0x06007640 RID: 30272 RVA: 0x000F23B9 File Offset: 0x000F05B9
		public bool IsHypothermic()
		{
			return this.temperature.value < this.HypothermiaThreshold;
		}

		// Token: 0x06007641 RID: 30273 RVA: 0x00317CF8 File Offset: 0x00315EF8
		public float ExtremeTemperatureDelta()
		{
			if (this.temperature.value > this.HyperthermiaThreshold)
			{
				return this.temperature.value - this.HyperthermiaThreshold;
			}
			if (this.temperature.value < this.HypothermiaThreshold)
			{
				return this.temperature.value - this.HypothermiaThreshold;
			}
			return 0f;
		}

		// Token: 0x06007642 RID: 30274 RVA: 0x000F23CE File Offset: 0x000F05CE
		public float IdealTemperatureDelta()
		{
			return this.temperature.value - DUPLICANTSTATS.STANDARD.Temperature.Internal.IDEAL;
		}

		// Token: 0x06007643 RID: 30275 RVA: 0x000F23F0 File Offset: 0x000F05F0
		public int GetWarmUpCell()
		{
			return base.sm.warmUpCell.Get(base.smi);
		}

		// Token: 0x06007644 RID: 30276 RVA: 0x000F2408 File Offset: 0x000F0608
		public int GetCoolDownCell()
		{
			return base.sm.coolDownCell.Get(base.smi);
		}

		// Token: 0x06007645 RID: 30277 RVA: 0x00317D58 File Offset: 0x00315F58
		public void UpdateWarmUpCell()
		{
			this.warmUpQuery.Reset();
			this.navigator.RunQuery(this.warmUpQuery);
			base.sm.warmUpCell.Set(this.warmUpQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x06007646 RID: 30278 RVA: 0x00317DA4 File Offset: 0x00315FA4
		public void UpdateCoolDownCell()
		{
			this.coolDownQuery.Reset();
			this.navigator.RunQuery(this.coolDownQuery);
			base.sm.coolDownCell.Set(this.coolDownQuery.GetResultCell(), base.smi, false);
		}

		// Token: 0x040058EF RID: 22767
		public AmountInstance temperature;

		// Token: 0x040058F0 RID: 22768
		public PrimaryElement primaryElement;

		// Token: 0x040058F1 RID: 22769
		private Navigator navigator;

		// Token: 0x040058F2 RID: 22770
		private SafetyQuery warmUpQuery;

		// Token: 0x040058F3 RID: 22771
		private SafetyQuery coolDownQuery;

		// Token: 0x040058F4 RID: 22772
		public float averageTemperature;

		// Token: 0x040058F5 RID: 22773
		public float HypothermiaThreshold = 307.15f;

		// Token: 0x040058F6 RID: 22774
		public float HyperthermiaThreshold = 313.15f;
	}
}
