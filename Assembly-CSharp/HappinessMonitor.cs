using System;
using Klei.AI;
using STRINGS;

// Token: 0x020011D2 RID: 4562
public class HappinessMonitor : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>
{
	// Token: 0x06005CCA RID: 23754 RVA: 0x002AA2D0 File Offset: 0x002A84D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.happy, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.neutral, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral), UpdateRate.SIM_1000ms).Transition(this.glum, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum), UpdateRate.SIM_1000ms).Transition(this.miserable, new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable), UpdateRate.SIM_1000ms);
		this.happy.DefaultState(this.happy.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Happy);
		this.happy.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.happyWildEffect).TagTransition(GameTags.Creatures.Wild, this.happy.tame, true);
		this.happy.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.happyTameEffect).TagTransition(GameTags.Creatures.Wild, this.happy.wild, false);
		this.neutral.DefaultState(this.neutral.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsNeutral)), UpdateRate.SIM_1000ms);
		this.neutral.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralWildEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.tame, true);
		this.neutral.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.neutralTameEffect).TagTransition(GameTags.Creatures.Wild, this.neutral.wild, false);
		this.glum.DefaultState(this.glum.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsGlum)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.glum.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.glumWildEffect).TagTransition(GameTags.Creatures.Wild, this.glum.tame, true);
		this.glum.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.glumTameEffect).TagTransition(GameTags.Creatures.Wild, this.glum.wild, false);
		this.miserable.DefaultState(this.miserable.wild).Transition(this.satisfied, GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Not(new StateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.Transition.ConditionCallback(HappinessMonitor.IsMisirable)), UpdateRate.SIM_1000ms).ToggleTag(GameTags.Creatures.Unhappy);
		this.miserable.wild.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableWildEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.tame, true);
		this.miserable.tame.ToggleEffect((HappinessMonitor.Instance smi) => this.miserableTameEffect).TagTransition(GameTags.Creatures.Wild, this.miserable.wild, false);
		this.happyWildEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.happyTameEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_TAME.NAME, CREATURES.MODIFIERS.HAPPY_TAME.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralWildEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralTameEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.glumWildEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.glumTameEffect = new Effect("Glum", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableWildEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.miserableTameEffect = new Effect("Miserable", CREATURES.MODIFIERS.MISERABLE.NAME, CREATURES.MODIFIERS.MISERABLE.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
		this.happyTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, 9f, CREATURES.MODIFIERS.HAPPY_TAME.NAME, true, false, true));
		this.glumWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.glumTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.GLUM.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -80f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableTameEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -15f, CREATURES.MODIFIERS.MISERABLE.NAME, false, false, true));
		this.miserableWildEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.MISERABLE.NAME, true, false, true));
	}

	// Token: 0x06005CCB RID: 23755 RVA: 0x000E0F47 File Offset: 0x000DF147
	private static bool IsHappy(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06005CCC RID: 23756 RVA: 0x002AA944 File Offset: 0x002A8B44
	private static bool IsNeutral(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.glumThreshold && totalValue < smi.def.happyThreshold;
	}

	// Token: 0x06005CCD RID: 23757 RVA: 0x002AA97C File Offset: 0x002A8B7C
	private static bool IsGlum(HappinessMonitor.Instance smi)
	{
		float totalValue = smi.happiness.GetTotalValue();
		return totalValue > smi.def.miserableThreshold && totalValue <= smi.def.glumThreshold;
	}

	// Token: 0x06005CCE RID: 23758 RVA: 0x000E0F64 File Offset: 0x000DF164
	private static bool IsMisirable(HappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.miserableThreshold;
	}

	// Token: 0x04004215 RID: 16917
	private GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State satisfied;

	// Token: 0x04004216 RID: 16918
	private HappinessMonitor.HappyState happy;

	// Token: 0x04004217 RID: 16919
	private HappinessMonitor.NeutralState neutral;

	// Token: 0x04004218 RID: 16920
	private HappinessMonitor.UnhappyState glum;

	// Token: 0x04004219 RID: 16921
	private HappinessMonitor.MiserableState miserable;

	// Token: 0x0400421A RID: 16922
	private Effect happyWildEffect;

	// Token: 0x0400421B RID: 16923
	private Effect happyTameEffect;

	// Token: 0x0400421C RID: 16924
	private Effect neutralTameEffect;

	// Token: 0x0400421D RID: 16925
	private Effect neutralWildEffect;

	// Token: 0x0400421E RID: 16926
	private Effect glumWildEffect;

	// Token: 0x0400421F RID: 16927
	private Effect glumTameEffect;

	// Token: 0x04004220 RID: 16928
	private Effect miserableWildEffect;

	// Token: 0x04004221 RID: 16929
	private Effect miserableTameEffect;

	// Token: 0x020011D3 RID: 4563
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004222 RID: 16930
		public float happyThreshold = 4f;

		// Token: 0x04004223 RID: 16931
		public float glumThreshold = -1f;

		// Token: 0x04004224 RID: 16932
		public float miserableThreshold = -10f;
	}

	// Token: 0x020011D4 RID: 4564
	public class MiserableState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04004225 RID: 16933
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04004226 RID: 16934
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020011D5 RID: 4565
	public class NeutralState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04004227 RID: 16935
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x04004228 RID: 16936
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020011D6 RID: 4566
	public class UnhappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x04004229 RID: 16937
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x0400422A RID: 16938
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020011D7 RID: 4567
	public class HappyState : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State
	{
		// Token: 0x0400422B RID: 16939
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State wild;

		// Token: 0x0400422C RID: 16940
		public GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.State tame;
	}

	// Token: 0x020011D8 RID: 4568
	public new class Instance : GameStateMachine<HappinessMonitor, HappinessMonitor.Instance, IStateMachineTarget, HappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06005CDD RID: 23773 RVA: 0x000E0FFA File Offset: 0x000DF1FA
		public Instance(IStateMachineTarget master, HappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x0400422D RID: 16941
		public AttributeInstance happiness;
	}
}
