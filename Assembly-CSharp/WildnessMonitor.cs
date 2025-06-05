using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200122D RID: 4653
public class WildnessMonitor : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>
{
	// Token: 0x06005E5F RID: 24159 RVA: 0x002AF83C File Offset: 0x002ADA3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.tame;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.wild.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.HideDomesticationSymbol)).Transition(this.tame, (WildnessMonitor.Instance smi) => !WildnessMonitor.IsWild(smi), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.wildEffect).ToggleTag(GameTags.Creatures.Wild);
		this.tame.Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.RefreshAmounts)).Enter(new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State.Callback(WildnessMonitor.ShowDomesticationSymbol)).Transition(this.wild, new StateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.Transition.ConditionCallback(WildnessMonitor.IsWild), UpdateRate.SIM_1000ms).ToggleEffect((WildnessMonitor.Instance smi) => smi.def.tameEffect).Enter(delegate(WildnessMonitor.Instance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.LogCritterTamed(smi.PrefabID());
		});
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x002AF964 File Offset: 0x002ADB64
	private static void HideDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, false);
		}
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x002AF99C File Offset: 0x002ADB9C
	private static void ShowDomesticationSymbol(WildnessMonitor.Instance smi)
	{
		foreach (KAnimHashedString symbol in WildnessMonitor.DOMESTICATION_SYMBOLS)
		{
			smi.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(symbol, true);
		}
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x000E2141 File Offset: 0x000E0341
	private static bool IsWild(WildnessMonitor.Instance smi)
	{
		return smi.wildness.value > 0f;
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x002AF9D4 File Offset: 0x002ADBD4
	private static void RefreshAmounts(WildnessMonitor.Instance smi)
	{
		bool flag = WildnessMonitor.IsWild(smi);
		smi.wildness.hide = !flag;
		AttributeInstance attributeInstance = Db.Get().CritterAttributes.Happiness.Lookup(smi.gameObject);
		if (attributeInstance != null)
		{
			attributeInstance.hide = flag;
		}
		AttributeInstance attributeInstance2 = Db.Get().CritterAttributes.Metabolism.Lookup(smi.gameObject);
		if (attributeInstance2 != null)
		{
			attributeInstance2.hide = flag;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
		if (amountInstance != null)
		{
			amountInstance.hide = flag;
		}
		AmountInstance amountInstance2 = Db.Get().Amounts.Temperature.Lookup(smi.gameObject);
		if (amountInstance2 != null)
		{
			amountInstance2.hide = flag;
		}
		AmountInstance amountInstance3 = Db.Get().Amounts.Fertility.Lookup(smi.gameObject);
		if (amountInstance3 != null)
		{
			amountInstance3.hide = flag;
		}
		AmountInstance amountInstance4 = Db.Get().Amounts.MilkProduction.Lookup(smi.gameObject);
		if (amountInstance4 != null)
		{
			amountInstance4.hide = flag;
		}
		AmountInstance amountInstance5 = Db.Get().Amounts.Beckoning.Lookup(smi.gameObject);
		if (amountInstance5 != null)
		{
			amountInstance5.hide = flag;
		}
	}

	// Token: 0x04004368 RID: 17256
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State wild;

	// Token: 0x04004369 RID: 17257
	public GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.State tame;

	// Token: 0x0400436A RID: 17258
	private static readonly KAnimHashedString[] DOMESTICATION_SYMBOLS = new KAnimHashedString[]
	{
		"tag",
		"snapto_tag"
	};

	// Token: 0x0200122E RID: 4654
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005E66 RID: 24166 RVA: 0x000E218C File Offset: 0x000E038C
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Wildness.Id);
		}

		// Token: 0x0400436B RID: 17259
		public Effect wildEffect;

		// Token: 0x0400436C RID: 17260
		public Effect tameEffect;
	}

	// Token: 0x0200122F RID: 4655
	public new class Instance : GameStateMachine<WildnessMonitor, WildnessMonitor.Instance, IStateMachineTarget, WildnessMonitor.Def>.GameInstance
	{
		// Token: 0x06005E68 RID: 24168 RVA: 0x000E21B2 File Offset: 0x000E03B2
		public Instance(IStateMachineTarget master, WildnessMonitor.Def def) : base(master, def)
		{
			this.wildness = Db.Get().Amounts.Wildness.Lookup(base.gameObject);
			this.wildness.value = this.wildness.GetMax();
		}

		// Token: 0x0400436D RID: 17261
		public AmountInstance wildness;
	}
}
