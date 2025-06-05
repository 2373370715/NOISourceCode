using System;
using Klei.AI;
using STRINGS;

// Token: 0x02001169 RID: 4457
public class BeeHappinessMonitor : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>
{
	// Token: 0x06005AE3 RID: 23267 RVA: 0x002A4B28 File Offset: 0x002A2D28
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.TriggerOnEnter(GameHashes.Satisfied, null).Transition(this.happy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy), UpdateRate.SIM_1000ms).Transition(this.unhappy, new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.neutralEffect);
		this.happy.TriggerOnEnter(GameHashes.Happy, null).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.happyEffect).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsHappy)), UpdateRate.SIM_1000ms);
		this.unhappy.TriggerOnEnter(GameHashes.Unhappy, null).Transition(this.satisfied, GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Not(new StateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.Transition.ConditionCallback(BeeHappinessMonitor.IsUnhappy)), UpdateRate.SIM_1000ms).ToggleEffect((BeeHappinessMonitor.Instance smi) => this.unhappyEffect);
		this.happyEffect = new Effect("Happy", CREATURES.MODIFIERS.HAPPY_WILD.NAME, CREATURES.MODIFIERS.HAPPY_WILD.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.neutralEffect = new Effect("Neutral", CREATURES.MODIFIERS.NEUTRAL.NAME, CREATURES.MODIFIERS.NEUTRAL.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.unhappyEffect = new Effect("Unhappy", CREATURES.MODIFIERS.GLUM.NAME, CREATURES.MODIFIERS.GLUM.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
	}

	// Token: 0x06005AE4 RID: 23268 RVA: 0x000DF99E File Offset: 0x000DDB9E
	private static bool IsHappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() >= smi.def.happyThreshold;
	}

	// Token: 0x06005AE5 RID: 23269 RVA: 0x000DF9BB File Offset: 0x000DDBBB
	private static bool IsUnhappy(BeeHappinessMonitor.Instance smi)
	{
		return smi.happiness.GetTotalValue() <= smi.def.unhappyThreshold;
	}

	// Token: 0x040040B0 RID: 16560
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State satisfied;

	// Token: 0x040040B1 RID: 16561
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State happy;

	// Token: 0x040040B2 RID: 16562
	private GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.State unhappy;

	// Token: 0x040040B3 RID: 16563
	private Effect happyEffect;

	// Token: 0x040040B4 RID: 16564
	private Effect neutralEffect;

	// Token: 0x040040B5 RID: 16565
	private Effect unhappyEffect;

	// Token: 0x0200116A RID: 4458
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040040B6 RID: 16566
		public float happyThreshold = 4f;

		// Token: 0x040040B7 RID: 16567
		public float unhappyThreshold = -1f;
	}

	// Token: 0x0200116B RID: 4459
	public new class Instance : GameStateMachine<BeeHappinessMonitor, BeeHappinessMonitor.Instance, IStateMachineTarget, BeeHappinessMonitor.Def>.GameInstance
	{
		// Token: 0x06005AEB RID: 23275 RVA: 0x000DFA16 File Offset: 0x000DDC16
		public Instance(IStateMachineTarget master, BeeHappinessMonitor.Def def) : base(master, def)
		{
			this.happiness = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Happiness);
		}

		// Token: 0x040040B8 RID: 16568
		public AttributeInstance happiness;
	}
}
