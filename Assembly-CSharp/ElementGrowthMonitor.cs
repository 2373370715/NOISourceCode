using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020011B1 RID: 4529
public class ElementGrowthMonitor : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>
{
	// Token: 0x06005C0E RID: 23566 RVA: 0x002A7C30 File Offset: 0x002A5E30
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ElementGrowthMonitor.Instance smi)
		{
			ElementGrowthMonitor.UpdateGrowth(smi, 0f);
		}).Update(new Action<ElementGrowthMonitor.Instance, float>(ElementGrowthMonitor.UpdateGrowth), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.EatSolidComplete, delegate(ElementGrowthMonitor.Instance smi, object data)
		{
			smi.OnEatSolidComplete(data);
		});
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown), UpdateRate.SIM_1000ms).TagTransition(this.HungryTags, this.halted, false);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange)), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthGrowing, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.growing.stunted.Transition(this.growing.growing, new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsConsumedInTemperatureRange), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthStunted, null).Enter(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State.Callback(ElementGrowthMonitor.RemoveModifier));
		this.halted.TagTransition(this.HungryTags, this.growing, true).ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthHalted, null);
		this.fullyGrown.ToggleStatusItem(Db.Get().CreatureStatusItems.ElementGrowthComplete, null).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ElementGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Not(new StateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.Transition.ConditionCallback(ElementGrowthMonitor.IsFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005C0F RID: 23567 RVA: 0x000E06A3 File Offset: 0x000DE8A3
	private static bool IsConsumedInTemperatureRange(ElementGrowthMonitor.Instance smi)
	{
		return smi.lastConsumedTemperature == 0f || (smi.lastConsumedTemperature >= smi.def.minTemperature && smi.lastConsumedTemperature <= smi.def.maxTemperature);
	}

	// Token: 0x06005C10 RID: 23568 RVA: 0x000E06DF File Offset: 0x000DE8DF
	private static bool IsFullyGrown(ElementGrowthMonitor.Instance smi)
	{
		return smi.elementGrowth.value >= smi.elementGrowth.GetMax();
	}

	// Token: 0x06005C11 RID: 23569 RVA: 0x002A7E44 File Offset: 0x002A6044
	private static void ApplyModifier(ElementGrowthMonitor.Instance smi)
	{
		if (smi.IsInsideState(smi.sm.growing.growing))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.growingGrowthModifier);
			return;
		}
		if (smi.IsInsideState(smi.sm.growing.stunted))
		{
			smi.elementGrowth.deltaAttribute.Add(smi.stuntedGrowthModifier);
		}
	}

	// Token: 0x06005C12 RID: 23570 RVA: 0x000E06FC File Offset: 0x000DE8FC
	private static void RemoveModifier(ElementGrowthMonitor.Instance smi)
	{
		smi.elementGrowth.deltaAttribute.Remove(smi.growingGrowthModifier);
		smi.elementGrowth.deltaAttribute.Remove(smi.stuntedGrowthModifier);
	}

	// Token: 0x06005C13 RID: 23571 RVA: 0x002A7EB0 File Offset: 0x002A60B0
	private static void UpdateGrowth(ElementGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.elementGrowth.value / 100f);
		if (smi.currentGrowthLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ElementGrowthMonitor.GROWTH_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i == num - 1;
				component.SetSymbolVisiblity(ElementGrowthMonitor.GROWTH_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentGrowthLevel = num;
		}
	}

	// Token: 0x04004189 RID: 16777
	public Tag[] HungryTags = new Tag[]
	{
		GameTags.Creatures.Hungry
	};

	// Token: 0x0400418A RID: 16778
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State halted;

	// Token: 0x0400418B RID: 16779
	public ElementGrowthMonitor.GrowingState growing;

	// Token: 0x0400418C RID: 16780
	public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x0400418D RID: 16781
	private static HashedString[] GROWTH_SYMBOL_NAMES = new HashedString[]
	{
		"del_ginger1",
		"del_ginger2",
		"del_ginger3",
		"del_ginger4",
		"del_ginger5"
	};

	// Token: 0x020011B2 RID: 4530
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005C16 RID: 23574 RVA: 0x000E074A File Offset: 0x000DE94A
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ElementGrowth.Id);
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x002A7F94 File Offset: 0x002A6194
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_TEMP.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{TempMin}", GameUtil.GetFormattedTemperature(this.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{TempMax}", GameUtil.GetFormattedTemperature(this.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x0400418E RID: 16782
		public int levelCount;

		// Token: 0x0400418F RID: 16783
		public float defaultGrowthRate;

		// Token: 0x04004190 RID: 16784
		public Tag itemDroppedOnShear;

		// Token: 0x04004191 RID: 16785
		public float dropMass;

		// Token: 0x04004192 RID: 16786
		public float minTemperature;

		// Token: 0x04004193 RID: 16787
		public float maxTemperature;
	}

	// Token: 0x020011B3 RID: 4531
	public class GrowingState : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State
	{
		// Token: 0x04004194 RID: 16788
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State growing;

		// Token: 0x04004195 RID: 16789
		public GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x020011B4 RID: 4532
	public new class Instance : GameStateMachine<ElementGrowthMonitor, ElementGrowthMonitor.Instance, IStateMachineTarget, ElementGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x06005C1A RID: 23578 RVA: 0x002A80C8 File Offset: 0x002A62C8
		public Instance(IStateMachineTarget master, ElementGrowthMonitor.Def def) : base(master, def)
		{
			this.elementGrowth = Db.Get().Amounts.ElementGrowth.Lookup(base.gameObject);
			this.elementGrowth.value = this.elementGrowth.GetMax();
			this.growingGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
			this.stuntedGrowthModifier = new AttributeModifier(this.elementGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x002A818C File Offset: 0x002A638C
		public void OnEatSolidComplete(object data)
		{
			KPrefabID kprefabID = (KPrefabID)data;
			if (kprefabID == null)
			{
				return;
			}
			PrimaryElement component = kprefabID.GetComponent<PrimaryElement>();
			this.lastConsumedElement = component.ElementID;
			this.lastConsumedTemperature = component.Temperature;
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x000E0778 File Offset: 0x000DE978
		public bool IsFullyGrown()
		{
			return this.currentGrowthLevel == base.def.levelCount;
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x002A81CC File Offset: 0x002A63CC
		public void Shear()
		{
			PrimaryElement component = base.smi.GetComponent<PrimaryElement>();
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.itemDroppedOnShear), null, null);
			gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			component2.Temperature = component.Temperature;
			component2.Mass = base.def.dropMass;
			component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
			gameObject.SetActive(true);
			Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, UnityEngine.Random.value * 2f + 2f);
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, initial_velocity);
			this.elementGrowth.value = 0f;
			ElementGrowthMonitor.UpdateGrowth(this, 0f);
		}

		// Token: 0x04004196 RID: 16790
		public AmountInstance elementGrowth;

		// Token: 0x04004197 RID: 16791
		public AttributeModifier growingGrowthModifier;

		// Token: 0x04004198 RID: 16792
		public AttributeModifier stuntedGrowthModifier;

		// Token: 0x04004199 RID: 16793
		public int currentGrowthLevel = -1;

		// Token: 0x0400419A RID: 16794
		[Serialize]
		public SimHashes lastConsumedElement;

		// Token: 0x0400419B RID: 16795
		[Serialize]
		public float lastConsumedTemperature;
	}
}
