using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011DA RID: 4570
[SkipSaveFileSerialization]
public class IlluminationVulnerable : StateMachineComponent<IlluminationVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, IIlluminationTracker
{
	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06005CE1 RID: 23777 RVA: 0x000E1029 File Offset: 0x000DF229
	public int LightIntensityThreshold
	{
		get
		{
			if (this.minLuxAttributeInstance != null)
			{
				return Mathf.RoundToInt(this.minLuxAttributeInstance.GetTotalValue());
			}
			return Mathf.RoundToInt(base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.MinLightLux));
		}
	}

	// Token: 0x06005CE2 RID: 23778 RVA: 0x000E1063 File Offset: 0x000DF263
	public string GetIlluminationUITooltip()
	{
		if ((this.prefersDarkness && this.IsComfortable()) || (!this.prefersDarkness && !this.IsComfortable()))
		{
			return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_DARK;
		}
		return UI.TOOLTIPS.VITALS_CHECKBOX_ILLUMINATION_LIGHT;
	}

	// Token: 0x06005CE3 RID: 23779 RVA: 0x000E109A File Offset: 0x000DF29A
	public string GetIlluminationUILabel()
	{
		return Db.Get().Amounts.Illumination.Name + "\n    • " + (this.prefersDarkness ? UI.GAMEOBJECTEFFECTS.DARKNESS.ToString() : GameUtil.GetFormattedLux(this.LightIntensityThreshold));
	}

	// Token: 0x06005CE4 RID: 23780 RVA: 0x000E10D9 File Offset: 0x000DF2D9
	public bool ShouldIlluminationUICheckboxBeChecked()
	{
		return this.IsComfortable();
	}

	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06005CE5 RID: 23781 RVA: 0x000E10E1 File Offset: 0x000DF2E1
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x06005CE6 RID: 23782 RVA: 0x002AA9B8 File Offset: 0x002A8BB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Illumination, base.gameObject));
		this.minLuxAttributeInstance = base.gameObject.GetAttributes().Add(Db.Get().PlantAttributes.MinLightLux);
	}

	// Token: 0x06005CE7 RID: 23783 RVA: 0x000E1103 File Offset: 0x000DF303
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005CE8 RID: 23784 RVA: 0x000E1116 File Offset: 0x000DF316
	public void SetPrefersDarkness(bool prefersDarkness = false)
	{
		this.prefersDarkness = prefersDarkness;
	}

	// Token: 0x06005CE9 RID: 23785 RVA: 0x000E111F File Offset: 0x000DF31F
	protected override void OnCleanUp()
	{
		this.handle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x06005CEA RID: 23786 RVA: 0x000E1132 File Offset: 0x000DF332
	public bool IsCellSafe(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		if (this.prefersDarkness)
		{
			return Grid.LightIntensity[cell] == 0;
		}
		return Grid.LightIntensity[cell] >= this.LightIntensityThreshold;
	}

	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06005CEB RID: 23787 RVA: 0x000E116B File Offset: 0x000DF36B
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Darkness,
				WiltCondition.Condition.IlluminationComfort
			};
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06005CEC RID: 23788 RVA: 0x002AAA1C File Offset: 0x002A8C1C
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.too_bright))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Bright.GetName(this);
			}
			if (base.smi.IsInsideState(base.smi.sm.too_dark))
			{
				return Db.Get().CreatureStatusItems.Crop_Too_Dark.GetName(this);
			}
			return "";
		}
	}

	// Token: 0x06005CED RID: 23789 RVA: 0x000E117B File Offset: 0x000DF37B
	public bool IsComfortable()
	{
		return base.smi.IsInsideState(base.smi.sm.comfortable);
	}

	// Token: 0x06005CEE RID: 23790 RVA: 0x002AAA94 File Offset: 0x002A8C94
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.prefersDarkness)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_DARKNESS, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_DARKNESS, Descriptor.DescriptorType.Requirement, false)
			};
		}
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_LIGHT.Replace("{Lux}", GameUtil.GetFormattedLux(this.LightIntensityThreshold)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x0400422E RID: 16942
	private OccupyArea _occupyArea;

	// Token: 0x0400422F RID: 16943
	private SchedulerHandle handle;

	// Token: 0x04004230 RID: 16944
	public bool prefersDarkness;

	// Token: 0x04004231 RID: 16945
	private AttributeInstance minLuxAttributeInstance;

	// Token: 0x020011DB RID: 4571
	public class StatesInstance : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.GameInstance
	{
		// Token: 0x06005CF0 RID: 23792 RVA: 0x000E11A0 File Offset: 0x000DF3A0
		public StatesInstance(IlluminationVulnerable master) : base(master)
		{
		}

		// Token: 0x04004232 RID: 16946
		public bool hasMaturity;
	}

	// Token: 0x020011DC RID: 4572
	public class States : GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable>
	{
		// Token: 0x06005CF1 RID: 23793 RVA: 0x002AAB18 File Offset: 0x002A8D18
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			this.root.Update("Illumination", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject);
				if (Grid.IsValidCell(num))
				{
					smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue((float)Grid.LightCount[num]);
					return;
				}
				smi.master.GetAmounts().Get(Db.Get().Amounts.Illumination).SetValue(0f);
			}, UpdateRate.SIM_1000ms, false);
			this.comfortable.Update("Illumination.Comfortable", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (!smi.master.IsCellSafe(cell))
				{
					GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State state = smi.master.prefersDarkness ? this.too_bright : this.too_dark;
					smi.GoTo(state);
				}
			}, UpdateRate.SIM_1000ms, false).Enter(delegate(IlluminationVulnerable.StatesInstance smi)
			{
				smi.master.Trigger(1113102781, null);
			});
			this.too_dark.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_dark", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.too_bright.TriggerOnEnter(GameHashes.IlluminationDiscomfort, null).Update("Illumination.too_bright", delegate(IlluminationVulnerable.StatesInstance smi, float dt)
			{
				int cell = Grid.PosToCell(smi.master.gameObject);
				if (smi.master.IsCellSafe(cell))
				{
					smi.GoTo(this.comfortable);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04004233 RID: 16947
		public StateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.BoolParameter illuminated;

		// Token: 0x04004234 RID: 16948
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State comfortable;

		// Token: 0x04004235 RID: 16949
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_dark;

		// Token: 0x04004236 RID: 16950
		public GameStateMachine<IlluminationVulnerable.States, IlluminationVulnerable.StatesInstance, IlluminationVulnerable, object>.State too_bright;
	}
}
