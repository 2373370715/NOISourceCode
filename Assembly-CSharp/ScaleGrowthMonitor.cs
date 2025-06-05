using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001211 RID: 4625
public class ScaleGrowthMonitor : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
	// Token: 0x06005DD3 RID: 24019 RVA: 0x002ADDD8 File Offset: 0x002ABFD8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(ScaleGrowthMonitor.Instance smi)
		{
			ScaleGrowthMonitor.UpdateScales(smi, 0f);
		}).Update(new Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales), UpdateRate.SIM_1000ms, false);
		this.growing.DefaultState(this.growing.growing).Transition(this.fullyGrown, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.growing.growing.Transition(this.growing.stunted, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere)), UpdateRate.SIM_1000ms).Enter(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier)).Exit(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier));
		GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state = this.growing.stunted.Transition(this.growing.growing, new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere), UpdateRate.SIM_1000ms);
		string name = CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME;
		string tooltip = CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.fullyGrown.ToggleBehaviour(GameTags.Creatures.ScalesGrown, (ScaleGrowthMonitor.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).Transition(this.growing, GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005DD4 RID: 24020 RVA: 0x002ADF64 File Offset: 0x002AC164
	private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
	{
		if (smi.def.targetAtmosphere == (SimHashes)0)
		{
			return true;
		}
		int num = Grid.PosToCell(smi);
		return Grid.IsValidCell(num) && Grid.Element[num].id == smi.def.targetAtmosphere;
	}

	// Token: 0x06005DD5 RID: 24021 RVA: 0x000E1A8F File Offset: 0x000DFC8F
	private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x06005DD6 RID: 24022 RVA: 0x000E1AAC File Offset: 0x000DFCAC
	private static void ApplyModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);
	}

	// Token: 0x06005DD7 RID: 24023 RVA: 0x000E1AC4 File Offset: 0x000DFCC4
	private static void RemoveModifier(ScaleGrowthMonitor.Instance smi)
	{
		smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);
	}

	// Token: 0x06005DD8 RID: 24024 RVA: 0x002ADFAC File Offset: 0x002AC1AC
	private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < ScaleGrowthMonitor.SCALE_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i <= num - 1;
				component.SetSymbolVisiblity(ScaleGrowthMonitor.SCALE_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x040042EF RID: 17135
	public ScaleGrowthMonitor.GrowingState growing;

	// Token: 0x040042F0 RID: 17136
	public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;

	// Token: 0x040042F1 RID: 17137
	private AttributeModifier scaleGrowthModifier;

	// Token: 0x040042F2 RID: 17138
	private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[]
	{
		"scale_0",
		"scale_1",
		"scale_2",
		"scale_3",
		"scale_4"
	};

	// Token: 0x02001212 RID: 4626
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005DDB RID: 24027 RVA: 0x000E1AE4 File Offset: 0x000DFCE4
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x06005DDC RID: 24028 RVA: 0x002AE094 File Offset: 0x002AC294
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			if (this.targetAtmosphere == (SimHashes)0)
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1", false)).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), Descriptor.DescriptorType.Effect, false));
			}
			return list;
		}

		// Token: 0x040042F3 RID: 17139
		public int levelCount;

		// Token: 0x040042F4 RID: 17140
		public float defaultGrowthRate;

		// Token: 0x040042F5 RID: 17141
		public SimHashes targetAtmosphere;

		// Token: 0x040042F6 RID: 17142
		public Tag itemDroppedOnShear;

		// Token: 0x040042F7 RID: 17143
		public float dropMass;
	}

	// Token: 0x02001213 RID: 4627
	public class GrowingState : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
	{
		// Token: 0x040042F8 RID: 17144
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;

		// Token: 0x040042F9 RID: 17145
		public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
	}

	// Token: 0x02001214 RID: 4628
	public new class Instance : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance, IShearable
	{
		// Token: 0x06005DDF RID: 24031 RVA: 0x002AE268 File Offset: 0x002AC468
		public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
			this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME, false, false, true);
		}

		// Token: 0x06005DE0 RID: 24032 RVA: 0x000E1B12 File Offset: 0x000DFD12
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x06005DE1 RID: 24033 RVA: 0x002AE2F4 File Offset: 0x002AC4F4
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
			this.scaleGrowth.value = 0f;
			ScaleGrowthMonitor.UpdateScales(this, 0f);
		}

		// Token: 0x040042FA RID: 17146
		public AmountInstance scaleGrowth;

		// Token: 0x040042FB RID: 17147
		public AttributeModifier scaleGrowthModifier;

		// Token: 0x040042FC RID: 17148
		public int currentScaleLevel = -1;
	}
}
