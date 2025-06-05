using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001220 RID: 4640
[SkipSaveFileSerialization]
public class TemperatureVulnerable : StateMachineComponent<TemperatureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06005E13 RID: 24083 RVA: 0x000E1D9B File Offset: 0x000DFF9B
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

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06005E14 RID: 24084 RVA: 0x000E1DBD File Offset: 0x000DFFBD
	public float TemperatureLethalLow
	{
		get
		{
			return this.internalTemperatureLethal_Low;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06005E15 RID: 24085 RVA: 0x000E1DC5 File Offset: 0x000DFFC5
	public float TemperatureLethalHigh
	{
		get
		{
			return this.internalTemperatureLethal_High;
		}
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06005E16 RID: 24086 RVA: 0x000E1DCD File Offset: 0x000DFFCD
	public float TemperatureWarningLow
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_Low + (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_Low;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x06005E17 RID: 24087 RVA: 0x000E1DFD File Offset: 0x000DFFFD
	public float TemperatureWarningHigh
	{
		get
		{
			if (this.wiltTempRangeModAttribute != null)
			{
				return this.internalTemperatureWarning_High - (1f - this.wiltTempRangeModAttribute.GetTotalValue()) * this.temperatureRangeModScalar;
			}
			return this.internalTemperatureWarning_High;
		}
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06005E18 RID: 24088 RVA: 0x000E1E2D File Offset: 0x000E002D
	public float InternalTemperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06005E19 RID: 24089 RVA: 0x000E1E3A File Offset: 0x000E003A
	public TemperatureVulnerable.TemperatureState GetInternalTemperatureState
	{
		get
		{
			return this.internalTemperatureState;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06005E1A RID: 24090 RVA: 0x000E1E42 File Offset: 0x000E0042
	public bool IsLethal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot || this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold;
		}
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x06005E1B RID: 24091 RVA: 0x000E1E58 File Offset: 0x000E0058
	public bool IsNormal
	{
		get
		{
			return this.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.Normal;
		}
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x06005E1C RID: 24092 RVA: 0x000E1E63 File Offset: 0x000E0063
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[1];
		}
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x06005E1D RID: 24093 RVA: 0x002AEAE0 File Offset: 0x002ACCE0
	public string WiltStateString
	{
		get
		{
			if (base.smi.IsInsideState(base.smi.sm.warningCold))
			{
				return Db.Get().CreatureStatusItems.Cold_Crop.resolveStringCallback(CREATURES.STATUSITEMS.COLD_CROP.NAME, this);
			}
			if (base.smi.IsInsideState(base.smi.sm.warningHot))
			{
				return Db.Get().CreatureStatusItems.Hot_Crop.resolveStringCallback(CREATURES.STATUSITEMS.HOT_CROP.NAME, this);
			}
			return "";
		}
	}

	// Token: 0x06005E1E RID: 24094 RVA: 0x002AEB78 File Offset: 0x002ACD78
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayTemperatureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.Temperature, base.gameObject));
	}

	// Token: 0x06005E1F RID: 24095 RVA: 0x002AEBC0 File Offset: 0x002ACDC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wiltTempRangeModAttribute = this.GetAttributes().Get(Db.Get().PlantAttributes.WiltTempRangeMod);
		this.temperatureRangeModScalar = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.RegisterUpdate1000ms(this);
		base.smi.sm.internalTemp.Set(this.primaryElement.Temperature, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x06005E20 RID: 24096 RVA: 0x000E1E6B File Offset: 0x000E006B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SlicedUpdaterSim1000ms<TemperatureVulnerable>.instance.UnregisterUpdate1000ms(this);
	}

	// Token: 0x06005E21 RID: 24097 RVA: 0x000E1E7E File Offset: 0x000E007E
	public void Configure(float tempWarningLow, float tempLethalLow, float tempWarningHigh, float tempLethalHigh)
	{
		this.internalTemperatureWarning_Low = tempWarningLow;
		this.internalTemperatureLethal_Low = tempLethalLow;
		this.internalTemperatureLethal_High = tempLethalHigh;
		this.internalTemperatureWarning_High = tempWarningHigh;
	}

	// Token: 0x06005E22 RID: 24098 RVA: 0x002AEC4C File Offset: 0x002ACE4C
	public bool IsCellSafe(int cell)
	{
		float averageTemperature = this.GetAverageTemperature(cell);
		return averageTemperature > -1f && averageTemperature > this.TemperatureLethalLow && averageTemperature < this.internalTemperatureLethal_High;
	}

	// Token: 0x06005E23 RID: 24099 RVA: 0x002AEC80 File Offset: 0x002ACE80
	public void SlicedSim1000ms(float dt)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)))
		{
			return;
		}
		base.smi.sm.internalTemp.Set(this.InternalTemperature, base.smi, false);
		this.displayTemperatureAmount.value = this.InternalTemperature;
	}

	// Token: 0x06005E24 RID: 24100 RVA: 0x002AECD4 File Offset: 0x002ACED4
	private static bool GetAverageTemperatureCb(int cell, object data)
	{
		TemperatureVulnerable temperatureVulnerable = data as TemperatureVulnerable;
		if (Grid.Mass[cell] > 0.1f)
		{
			temperatureVulnerable.averageTemp += Grid.Temperature[cell];
			temperatureVulnerable.cellCount++;
		}
		return true;
	}

	// Token: 0x06005E25 RID: 24101 RVA: 0x002AED24 File Offset: 0x002ACF24
	private float GetAverageTemperature(int cell)
	{
		this.averageTemp = 0f;
		this.cellCount = 0;
		this.occupyArea.TestArea(cell, this, TemperatureVulnerable.GetAverageTemperatureCbDelegate);
		if (this.cellCount > 0)
		{
			return this.averageTemp / (float)this.cellCount;
		}
		return -1f;
	}

	// Token: 0x06005E26 RID: 24102 RVA: 0x002AED74 File Offset: 0x002ACF74
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		float num = (this.internalTemperatureWarning_High - this.internalTemperatureWarning_Low) / 2f;
		float temp = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningLow : (this.internalTemperatureWarning_Low + (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		float temp2 = (this.wiltTempRangeModAttribute != null) ? this.TemperatureWarningHigh : (this.internalTemperatureWarning_High - (1f - base.GetComponent<Modifiers>().GetPreModifiedAttributeValue(Db.Get().PlantAttributes.WiltTempRangeMod)) * num);
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_TEMPERATURE, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, false, false), GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x04004320 RID: 17184
	private OccupyArea _occupyArea;

	// Token: 0x04004321 RID: 17185
	[SerializeField]
	private float internalTemperatureLethal_Low;

	// Token: 0x04004322 RID: 17186
	[SerializeField]
	private float internalTemperatureWarning_Low;

	// Token: 0x04004323 RID: 17187
	[SerializeField]
	private float internalTemperatureWarning_High;

	// Token: 0x04004324 RID: 17188
	[SerializeField]
	private float internalTemperatureLethal_High;

	// Token: 0x04004325 RID: 17189
	private AttributeInstance wiltTempRangeModAttribute;

	// Token: 0x04004326 RID: 17190
	private float temperatureRangeModScalar;

	// Token: 0x04004327 RID: 17191
	private const float minimumMassForReading = 0.1f;

	// Token: 0x04004328 RID: 17192
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04004329 RID: 17193
	[MyCmpReq]
	private SimTemperatureTransfer temperatureTransfer;

	// Token: 0x0400432A RID: 17194
	private AmountInstance displayTemperatureAmount;

	// Token: 0x0400432B RID: 17195
	private TemperatureVulnerable.TemperatureState internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;

	// Token: 0x0400432C RID: 17196
	private float averageTemp;

	// Token: 0x0400432D RID: 17197
	private int cellCount;

	// Token: 0x0400432E RID: 17198
	private static readonly Func<int, object, bool> GetAverageTemperatureCbDelegate = (int cell, object data) => TemperatureVulnerable.GetAverageTemperatureCb(cell, data);

	// Token: 0x02001221 RID: 4641
	public class StatesInstance : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.GameInstance
	{
		// Token: 0x06005E29 RID: 24105 RVA: 0x000E1EC3 File Offset: 0x000E00C3
		public StatesInstance(TemperatureVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x02001222 RID: 4642
	public class States : GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable>
	{
		// Token: 0x06005E2A RID: 24106 RVA: 0x002AEE64 File Offset: 0x002AD064
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalCold;
			}).TriggerOnEnter(GameHashes.TooColdFatal, null).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalLow).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.lethalHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.LethalHot;
			}).TriggerOnEnter(GameHashes.TooHotFatal, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalHigh).Enter(new StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State.Callback(TemperatureVulnerable.States.Kill));
			this.warningCold.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningCold;
			}).TriggerOnEnter(GameHashes.TooColdWarning, null).ParamTransition<float>(this.internalTemp, this.lethalCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureLethalLow).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningLow);
			this.warningHot.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.WarningHot;
			}).TriggerOnEnter(GameHashes.TooHotWarning, null).ParamTransition<float>(this.internalTemp, this.lethalHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureLethalHigh).ParamTransition<float>(this.internalTemp, this.normal, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningHigh);
			this.normal.Enter(delegate(TemperatureVulnerable.StatesInstance smi)
			{
				smi.master.internalTemperatureState = TemperatureVulnerable.TemperatureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, null).ParamTransition<float>(this.internalTemp, this.warningHot, (TemperatureVulnerable.StatesInstance smi, float p) => p > smi.master.TemperatureWarningHigh).ParamTransition<float>(this.internalTemp, this.warningCold, (TemperatureVulnerable.StatesInstance smi, float p) => p < smi.master.TemperatureWarningLow);
		}

		// Token: 0x06005E2B RID: 24107 RVA: 0x002AF12C File Offset: 0x002AD32C
		private static void Kill(StateMachine.Instance smi)
		{
			DeathMonitor.Instance smi2 = smi.GetSMI<DeathMonitor.Instance>();
			if (smi2 != null)
			{
				smi2.Kill(Db.Get().Deaths.Generic);
			}
		}

		// Token: 0x0400432F RID: 17199
		public StateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.FloatParameter internalTemp;

		// Token: 0x04004330 RID: 17200
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalCold;

		// Token: 0x04004331 RID: 17201
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State lethalHot;

		// Token: 0x04004332 RID: 17202
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningCold;

		// Token: 0x04004333 RID: 17203
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State warningHot;

		// Token: 0x04004334 RID: 17204
		public GameStateMachine<TemperatureVulnerable.States, TemperatureVulnerable.StatesInstance, TemperatureVulnerable, object>.State normal;
	}

	// Token: 0x02001224 RID: 4644
	public enum TemperatureState
	{
		// Token: 0x04004344 RID: 17220
		LethalCold,
		// Token: 0x04004345 RID: 17221
		WarningCold,
		// Token: 0x04004346 RID: 17222
		Normal,
		// Token: 0x04004347 RID: 17223
		WarningHot,
		// Token: 0x04004348 RID: 17224
		LethalHot
	}
}
