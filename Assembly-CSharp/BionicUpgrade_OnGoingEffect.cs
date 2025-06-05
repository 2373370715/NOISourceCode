using System;
using STRINGS;

// Token: 0x02000C8D RID: 3213
public class BionicUpgrade_OnGoingEffect : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>
{
	// Token: 0x06003CFD RID: 15613 RVA: 0x0023DBE8 File Offset: 0x0023BDE8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_OnGoingEffect.IsOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.ToggleEffect(new Func<BionicUpgrade_OnGoingEffect.Instance, string>(BionicUpgrade_OnGoingEffect.GetEffectName)).Enter(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.ApplySkills)).Exit(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.State.Callback(BionicUpgrade_OnGoingEffect.RemoveSkills)).EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Not(new StateMachine<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline))).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x000CBDE8 File Offset: 0x000C9FE8
	public static string GetEffectName(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return ((BionicUpgrade_OnGoingEffect.Def)smi.def).EFFECT_NAME;
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x000CBDFA File Offset: 0x000C9FFA
	public static void ApplySkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.ApplySkills();
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x000CBE02 File Offset: 0x000CA002
	public static void RemoveSkills(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		smi.RemoveSkills();
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x000CBE0A File Offset: 0x000CA00A
	public static bool IsOnlineAndNotInBatterySaveMode(BionicUpgrade_OnGoingEffect.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.IsInBedTimeChore(smi);
	}

	// Token: 0x02000C8E RID: 3214
	public new class Def : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.Def
	{
		// Token: 0x06003D03 RID: 15619 RVA: 0x000CBE27 File Offset: 0x000CA027
		public Def(string upgradeID, string effectID, string[] skills = null) : base(upgradeID)
		{
			this.EFFECT_NAME = effectID;
			this.SKILLS_IDS = skills;
		}

		// Token: 0x06003D04 RID: 15620 RVA: 0x000CBE3E File Offset: 0x000CA03E
		public override string GetDescription()
		{
			return "BionicUpgrade_OnGoingEffect.Def description not implemented";
		}

		// Token: 0x04002A36 RID: 10806
		public string EFFECT_NAME;

		// Token: 0x04002A37 RID: 10807
		public string[] SKILLS_IDS;
	}

	// Token: 0x02000C8F RID: 3215
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_OnGoingEffect, BionicUpgrade_OnGoingEffect.Instance>.BaseInstance
	{
		// Token: 0x06003D05 RID: 15621 RVA: 0x000CBE45 File Offset: 0x000CA045
		public Instance(IStateMachineTarget master, BionicUpgrade_OnGoingEffect.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x000CBE5B File Offset: 0x000CA05B
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0023DD08 File Offset: 0x0023BF08
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x0023DDA8 File Offset: 0x0023BFA8
		public void ApplySkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.GrantSkill(skillId);
				}
			}
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x0023DDF4 File Offset: 0x0023BFF4
		public void RemoveSkills()
		{
			BionicUpgrade_OnGoingEffect.Def def = (BionicUpgrade_OnGoingEffect.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.UngrantSkill(skillId);
				}
			}
		}

		// Token: 0x04002A38 RID: 10808
		private MinionResume resume;
	}
}
