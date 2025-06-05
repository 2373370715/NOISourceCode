using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009F6 RID: 2550
public class BeehiveCalorieMonitor : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>
{
	// Token: 0x06002E5E RID: 11870 RVA: 0x00202528 File Offset: 0x00200728
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeehiveCalorieMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.Poop, new StateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.Transition.ConditionCallback(BeehiveCalorieMonitor.ReadyToPoop), delegate(BeehiveCalorieMonitor.Instance smi)
		{
			smi.Poop();
		}).Update(new Action<BeehiveCalorieMonitor.Instance, float>(BeehiveCalorieMonitor.UpdateMetabolismCalorieModifier), UpdateRate.SIM_200ms, false);
		this.normal.Transition(this.hungry, (BeehiveCalorieMonitor.Instance smi) => smi.IsHungry(), UpdateRate.SIM_1000ms);
		this.hungry.ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry()).ToggleStatusItem(Db.Get().CreatureStatusItems.HiveHungry, null).Transition(this.normal, (BeehiveCalorieMonitor.Instance smi) => !smi.IsHungry(), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x000C279F File Offset: 0x000C099F
	private static bool ReadyToPoop(BeehiveCalorieMonitor.Instance smi)
	{
		return smi.stomach.IsReadyToPoop() && Time.time - smi.lastMealOrPoopTime >= smi.def.minimumTimeBeforePooping;
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x000C27CC File Offset: 0x000C09CC
	private static void UpdateMetabolismCalorieModifier(BeehiveCalorieMonitor.Instance smi, float dt)
	{
		smi.deltaCalorieMetabolismModifier.SetValue(1f - smi.metabolism.GetTotalValue() / 100f);
	}

	// Token: 0x04001FBB RID: 8123
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State normal;

	// Token: 0x04001FBC RID: 8124
	public GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.State hungry;

	// Token: 0x020009F7 RID: 2551
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06002E62 RID: 11874 RVA: 0x000C27F8 File Offset: 0x000C09F8
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x00202670 File Offset: 0x00200870
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_HEADER, UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
			float calorie_loss_per_second = 0f;
			foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
			{
				if (attributeModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
				{
					calorie_loss_per_second = attributeModifier.Value;
				}
			}
			BeehiveCalorieMonitor.Instance smi = obj.GetSMI<BeehiveCalorieMonitor.Instance>();
			string newValue = string.Join(", ", (from t in smi.stomach.diet.consumedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue2 = string.Join("\n", (from t in smi.stomach.diet.consumedTags
			select UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
			string newValue3 = string.Join(", ", (from t in smi.stomach.diet.producedTags
			select t.Key.ProperName()).ToArray<string>());
			string newValue4 = string.Join("\n", (from t in smi.stomach.diet.producedTags
			select UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None))).ToArray<string>());
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue3), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue4), Descriptor.DescriptorType.Effect, false));
			return list;
		}

		// Token: 0x04001FBD RID: 8125
		public Diet diet;

		// Token: 0x04001FBE RID: 8126
		public float minConsumedCaloriesBeforePooping = 100f;

		// Token: 0x04001FBF RID: 8127
		public float minimumTimeBeforePooping = 10f;

		// Token: 0x04001FC0 RID: 8128
		public bool storePoop = true;
	}

	// Token: 0x020009FA RID: 2554
	public new class Instance : GameStateMachine<BeehiveCalorieMonitor, BeehiveCalorieMonitor.Instance, IStateMachineTarget, BeehiveCalorieMonitor.Def>.GameInstance
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x00202918 File Offset: 0x00200B18
		public Instance(IStateMachineTarget master, BeehiveCalorieMonitor.Def def) : base(master, def)
		{
			this.calories = Db.Get().Amounts.Calories.Lookup(base.gameObject);
			this.calories.value = this.calories.GetMax() * 0.9f;
			this.stomach = new CreatureCalorieMonitor.Stomach(master.gameObject, def.minConsumedCaloriesBeforePooping, -1f, def.storePoop);
			this.metabolism = base.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
			this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
			this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x002029FC File Offset: 0x00200BFC
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.calories.value += caloriesConsumedEvent.calories;
			this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
			this.lastMealOrPoopTime = Time.time;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000C289C File Offset: 0x000C0A9C
		public void Poop()
		{
			this.lastMealOrPoopTime = Time.time;
			this.stomach.Poop();
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000C28B4 File Offset: 0x000C0AB4
		public float GetCalories0to1()
		{
			return this.calories.value / this.calories.GetMax();
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000C28CD File Offset: 0x000C0ACD
		public bool IsHungry()
		{
			return this.GetCalories0to1() < 0.9f;
		}

		// Token: 0x04001FC6 RID: 8134
		public const float HUNGRY_RATIO = 0.9f;

		// Token: 0x04001FC7 RID: 8135
		public AmountInstance calories;

		// Token: 0x04001FC8 RID: 8136
		[Serialize]
		public CreatureCalorieMonitor.Stomach stomach;

		// Token: 0x04001FC9 RID: 8137
		public float lastMealOrPoopTime;

		// Token: 0x04001FCA RID: 8138
		public AttributeInstance metabolism;

		// Token: 0x04001FCB RID: 8139
		public AttributeModifier deltaCalorieMetabolismModifier;
	}
}
