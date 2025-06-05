using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011FF RID: 4607
public class RadiationVulnerable : GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>
{
	// Token: 0x06005DA0 RID: 23968 RVA: 0x002AD764 File Offset: 0x002AB964
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.comfortable.Transition(this.too_dark, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() == -1, UpdateRate.SIM_1000ms).Transition(this.too_bright, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() == 1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationComfort, null);
		this.too_dark.Transition(this.comfortable, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() != -1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationDiscomfort, null);
		this.too_bright.Transition(this.comfortable, (RadiationVulnerable.StatesInstance smi) => smi.GetRadiationThresholdCrossed() != 1, UpdateRate.SIM_1000ms).TriggerOnEnter(GameHashes.RadiationDiscomfort, null);
	}

	// Token: 0x040042D1 RID: 17105
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State comfortable;

	// Token: 0x040042D2 RID: 17106
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State too_dark;

	// Token: 0x040042D3 RID: 17107
	public GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.State too_bright;

	// Token: 0x02001200 RID: 4608
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005DA2 RID: 23970 RVA: 0x002AD85C File Offset: 0x002ABA5C
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			float preModifiedAttributeValue = component.GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinRadiationThreshold);
			string preModifiedAttributeFormattedValue = component.GetPreModifiedAttributeFormattedValue(Db.Get().PlantAttributes.MinRadiationThreshold);
			string preModifiedAttributeFormattedValue2 = component.GetPreModifiedAttributeFormattedValue(Db.Get().PlantAttributes.MaxRadiationThreshold);
			MutantPlant component2 = go.GetComponent<MutantPlant>();
			bool flag = component2 != null && component2.IsOriginal;
			if (preModifiedAttributeValue <= 0f)
			{
				return new List<Descriptor>
				{
					new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_NO_MIN_RADIATION.Replace("{MaxRads}", preModifiedAttributeFormattedValue2), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_NO_MIN_RADIATION.Replace("{MaxRads}", preModifiedAttributeFormattedValue2) + (flag ? UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP.ToString() : ""), Descriptor.DescriptorType.Requirement, false)
				};
			}
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RADIATION.Replace("{MinRads}", preModifiedAttributeFormattedValue).Replace("{MaxRads}", preModifiedAttributeFormattedValue2), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RADIATION.Replace("{MinRads}", preModifiedAttributeFormattedValue).Replace("{MaxRads}", preModifiedAttributeFormattedValue2) + (flag ? UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_SEED_TOOLTIP.ToString() : ""), Descriptor.DescriptorType.Requirement, false)
			};
		}
	}

	// Token: 0x02001201 RID: 4609
	public class StatesInstance : GameStateMachine<RadiationVulnerable, RadiationVulnerable.StatesInstance, IStateMachineTarget, RadiationVulnerable.Def>.GameInstance, IWiltCause
	{
		// Token: 0x06005DA4 RID: 23972 RVA: 0x002AD984 File Offset: 0x002ABB84
		public StatesInstance(IStateMachineTarget master, RadiationVulnerable.Def def) : base(master, def)
		{
			this.minRadiationAttributeInstance = Db.Get().PlantAttributes.MinRadiationThreshold.Lookup(base.gameObject);
			this.maxRadiationAttributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(base.gameObject);
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x002AD9DC File Offset: 0x002ABBDC
		public int GetRadiationThresholdCrossed()
		{
			int num = Grid.PosToCell(base.master.gameObject);
			if (!Grid.IsValidCell(num))
			{
				return 0;
			}
			if (Grid.Radiation[num] < this.minRadiationAttributeInstance.GetTotalValue())
			{
				return -1;
			}
			if (Grid.Radiation[num] <= this.maxRadiationAttributeInstance.GetTotalValue())
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06005DA6 RID: 23974 RVA: 0x000E18A6 File Offset: 0x000DFAA6
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.Radiation
				};
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06005DA7 RID: 23975 RVA: 0x002ADA3C File Offset: 0x002ABC3C
		public string WiltStateString
		{
			get
			{
				if (base.smi.IsInsideState(base.smi.sm.too_dark))
				{
					return Db.Get().CreatureStatusItems.Crop_Too_NonRadiated.GetName(this);
				}
				if (base.smi.IsInsideState(base.smi.sm.too_bright))
				{
					return Db.Get().CreatureStatusItems.Crop_Too_Radiated.GetName(this);
				}
				return "";
			}
		}

		// Token: 0x040042D4 RID: 17108
		private AttributeInstance minRadiationAttributeInstance;

		// Token: 0x040042D5 RID: 17109
		private AttributeInstance maxRadiationAttributeInstance;
	}
}
