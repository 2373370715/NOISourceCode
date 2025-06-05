using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001193 RID: 4499
public class CropSleepingMonitor : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>
{
	// Token: 0x06005B98 RID: 23448 RVA: 0x002A66BC File Offset: 0x002A48BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.awake;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update("CropSleepingMonitor.root", delegate(CropSleepingMonitor.Instance smi, float dt)
		{
			int cell = Grid.PosToCell(smi.master.gameObject);
			GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State state = smi.IsCellSafe(cell) ? this.awake : this.sleeping;
			smi.GoTo(state);
		}, UpdateRate.SIM_1000ms, false);
		this.sleeping.TriggerOnEnter(GameHashes.CropSleep, null).ToggleStatusItem(Db.Get().CreatureStatusItems.CropSleeping, (CropSleepingMonitor.Instance smi) => smi);
		this.awake.TriggerOnEnter(GameHashes.CropWakeUp, null);
	}

	// Token: 0x0400412E RID: 16686
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State sleeping;

	// Token: 0x0400412F RID: 16687
	public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State awake;

	// Token: 0x02001194 RID: 4500
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06005B9B RID: 23451 RVA: 0x002A6790 File Offset: 0x002A4990
		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			if (this.prefersDarkness)
			{
				return new List<Descriptor>
				{
					new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
				};
			}
			Klei.AI.Attribute minLightLux = Db.Get().PlantAttributes.MinLightLux;
			AttributeInstance attributeInstance = minLightLux.Lookup(obj);
			int lux = Mathf.RoundToInt((attributeInstance != null) ? attributeInstance.GetTotalValue() : obj.GetComponent<Modifiers>().GetPreModifiedAttributeValue(minLightLux));
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(lux)), Descriptor.DescriptorType.Requirement, false)
			};
		}

		// Token: 0x04004130 RID: 16688
		public bool prefersDarkness;
	}

	// Token: 0x02001195 RID: 4501
	public new class Instance : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.GameInstance
	{
		// Token: 0x06005B9D RID: 23453 RVA: 0x000E01AA File Offset: 0x000DE3AA
		public Instance(IStateMachineTarget master, CropSleepingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x000E01B4 File Offset: 0x000DE3B4
		public bool IsSleeping()
		{
			return this.GetCurrentState() == base.smi.sm.sleeping;
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x002A6840 File Offset: 0x002A4A40
		public bool IsCellSafe(int cell)
		{
			AttributeInstance attributeInstance = Db.Get().PlantAttributes.MinLightLux.Lookup(base.gameObject);
			int num = Grid.LightIntensity[cell];
			if (!base.def.prefersDarkness)
			{
				return (float)num >= attributeInstance.GetTotalValue();
			}
			return num == 0;
		}
	}
}
