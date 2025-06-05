using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001229 RID: 4649
public class WellFedShearable : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
	// Token: 0x06005E4B RID: 24139 RVA: 0x002AF2C0 File Offset: 0x002AD4C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		this.root.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Enter(delegate(WellFedShearable.Instance smi)
		{
			if (smi.def.hideSymbols != null)
			{
				foreach (KAnimHashedString symbol in smi.def.hideSymbols)
				{
					smi.animController.SetSymbolVisiblity(symbol, false);
				}
			}
		}).Update(new Action<WellFedShearable.Instance, float>(WellFedShearable.UpdateScales), UpdateRate.SIM_1000ms, false).EventHandler(GameHashes.CaloriesConsumed, delegate(WellFedShearable.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.growing.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).Transition(this.fullyGrown, new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown), UpdateRate.SIM_1000ms);
		this.fullyGrown.Enter(delegate(WellFedShearable.Instance smi)
		{
			WellFedShearable.UpdateScales(smi, 0f);
		}).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (WellFedShearable.Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt), null).EventTransition(GameHashes.Molt, this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown))).Transition(this.growing, GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Not(new StateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.Transition.ConditionCallback(WellFedShearable.AreScalesFullyGrown)), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005E4C RID: 24140 RVA: 0x000E20C6 File Offset: 0x000E02C6
	private static bool AreScalesFullyGrown(WellFedShearable.Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	// Token: 0x06005E4D RID: 24141 RVA: 0x002AF438 File Offset: 0x002AD638
	private static void UpdateScales(WellFedShearable.Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			for (int i = 0; i < smi.def.scaleGrowthSymbols.Length; i++)
			{
				bool is_visible = i <= num - 1;
				smi.animController.SetSymbolVisiblity(smi.def.scaleGrowthSymbols[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}

	// Token: 0x04004351 RID: 17233
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State growing;

	// Token: 0x04004352 RID: 17234
	public GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.State fullyGrown;

	// Token: 0x0200122A RID: 4650
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005E4F RID: 24143 RVA: 0x000E1AE4 File Offset: 0x000DFCE4
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		// Token: 0x06005E50 RID: 24144 RVA: 0x002AF4B4 File Offset: 0x002AD6B4
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(this.growthDurationCycles * 600f, "F1", false)), Descriptor.DescriptorType.Effect, false)
			};
		}

		// Token: 0x04004353 RID: 17235
		public string effectId;

		// Token: 0x04004354 RID: 17236
		public float caloriesPerCycle;

		// Token: 0x04004355 RID: 17237
		public float growthDurationCycles;

		// Token: 0x04004356 RID: 17238
		public int levelCount;

		// Token: 0x04004357 RID: 17239
		public Tag itemDroppedOnShear;

		// Token: 0x04004358 RID: 17240
		public float dropMass;

		// Token: 0x04004359 RID: 17241
		public Tag requiredDiet = null;

		// Token: 0x0400435A RID: 17242
		public KAnimHashedString[] scaleGrowthSymbols = WellFedShearable.Def.SCALE_SYMBOL_NAMES;

		// Token: 0x0400435B RID: 17243
		public KAnimHashedString[] hideSymbols;

		// Token: 0x0400435C RID: 17244
		public static KAnimHashedString[] SCALE_SYMBOL_NAMES = new KAnimHashedString[]
		{
			"scale_0",
			"scale_1",
			"scale_2",
			"scale_3",
			"scale_4"
		};
	}

	// Token: 0x0200122B RID: 4651
	public new class Instance : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>.GameInstance, IShearable
	{
		// Token: 0x06005E53 RID: 24147 RVA: 0x002AF5F4 File Offset: 0x002AD7F4
		public Instance(IStateMachineTarget master, WellFedShearable.Def def) : base(master, def)
		{
			this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			this.scaleGrowth.value = this.scaleGrowth.GetMax();
		}

		// Token: 0x06005E54 RID: 24148 RVA: 0x000E210A File Offset: 0x000E030A
		public bool IsFullyGrown()
		{
			return this.currentScaleLevel == base.def.levelCount;
		}

		// Token: 0x06005E55 RID: 24149 RVA: 0x002AF648 File Offset: 0x002AD848
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			if (base.def.requiredDiet != null && caloriesConsumedEvent.tag != base.def.requiredDiet)
			{
				return;
			}
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x06005E56 RID: 24150 RVA: 0x002AF6F4 File Offset: 0x002AD8F4
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
			WellFedShearable.UpdateScales(this, 0f);
		}

		// Token: 0x0400435D RID: 17245
		[MyCmpGet]
		private Effects effects;

		// Token: 0x0400435E RID: 17246
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x0400435F RID: 17247
		public AmountInstance scaleGrowth;

		// Token: 0x04004360 RID: 17248
		public int currentScaleLevel = -1;
	}
}
