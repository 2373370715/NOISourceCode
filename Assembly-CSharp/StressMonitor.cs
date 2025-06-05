using System;
using Klei.AI;
using Klei.CustomSettings;

// Token: 0x02001648 RID: 5704
public class StressMonitor : GameStateMachine<StressMonitor, StressMonitor.Instance>
{
	// Token: 0x060075FB RID: 30203 RVA: 0x00316F08 File Offset: 0x00315108
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		default_state = this.satisfied;
		this.root.Update("StressMonitor", delegate(StressMonitor.Instance smi, float dt)
		{
			smi.ReportStress(dt);
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.TriggerOnEnter(GameHashes.NotStressed, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => smi.stress.value >= 60f, UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Neutral, null);
		this.stressed.ToggleStatusItem(Db.Get().DuplicantStatusItems.Stressed, null).Transition(this.satisfied, (StressMonitor.Instance smi) => smi.stress.value < 60f, UpdateRate.SIM_200ms).ToggleReactable((StressMonitor.Instance smi) => smi.CreateConcernReactable()).TriggerOnEnter(GameHashes.Stressed, null);
		this.stressed.tier1.Transition(this.stressed.tier2, (StressMonitor.Instance smi) => smi.HasHadEnough(), UpdateRate.SIM_200ms);
		this.stressed.tier2.TriggerOnEnter(GameHashes.StressedHadEnough, null).Transition(this.stressed.tier1, (StressMonitor.Instance smi) => !smi.HasHadEnough(), UpdateRate.SIM_200ms);
	}

	// Token: 0x040058B1 RID: 22705
	public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040058B2 RID: 22706
	public StressMonitor.Stressed stressed;

	// Token: 0x040058B3 RID: 22707
	private const float StressThreshold_One = 60f;

	// Token: 0x040058B4 RID: 22708
	private const float StressThreshold_Two = 100f;

	// Token: 0x02001649 RID: 5705
	public class Stressed : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040058B5 RID: 22709
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier1;

		// Token: 0x040058B6 RID: 22710
		public GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.State tier2;
	}

	// Token: 0x0200164A RID: 5706
	public new class Instance : GameStateMachine<StressMonitor, StressMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075FE RID: 30206 RVA: 0x003170A8 File Offset: 0x003152A8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.stress = Db.Get().Amounts.Stress.Lookup(base.gameObject);
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.StressBreaks.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.StressBreaks);
			this.allowStressBreak = settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x000F213B File Offset: 0x000F033B
		public bool IsStressed()
		{
			return base.IsInsideState(base.sm.stressed);
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x000F214E File Offset: 0x000F034E
		public bool HasHadEnough()
		{
			return this.allowStressBreak && this.stress.value >= 100f;
		}

		// Token: 0x06007601 RID: 30209 RVA: 0x00317120 File Offset: 0x00315320
		public void ReportStress(float dt)
		{
			for (int num = 0; num != this.stress.deltaAttribute.Modifiers.Count; num++)
			{
				AttributeModifier attributeModifier = this.stress.deltaAttribute.Modifiers[num];
				DebugUtil.DevAssert(!attributeModifier.IsMultiplier, "Reporting stress for multipliers not supported yet.", null);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.StressDelta, attributeModifier.Value * dt, attributeModifier.GetDescription(), base.gameObject.GetProperName());
			}
		}

		// Token: 0x06007602 RID: 30210 RVA: 0x0031719C File Offset: 0x0031539C
		public Reactable CreateConcernReactable()
		{
			return new EmoteReactable(base.master.gameObject, "StressConcern", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 30f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Concern);
		}

		// Token: 0x040058B7 RID: 22711
		public AmountInstance stress;

		// Token: 0x040058B8 RID: 22712
		private bool allowStressBreak = true;
	}
}
