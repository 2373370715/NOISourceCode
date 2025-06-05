using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FE2 RID: 4066
[SerializationConfig(MemberSerialization.OptIn)]
public class SpaceHeater : StateMachineComponent<SpaceHeater.StatesInstance>, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x060051C5 RID: 20933 RVA: 0x000D9B9D File Offset: 0x000D7D9D
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x060051C6 RID: 20934 RVA: 0x000D9BA5 File Offset: 0x000D7DA5
	public float MaxPower
	{
		get
		{
			return 240f;
		}
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x060051C7 RID: 20935 RVA: 0x000D9BAC File Offset: 0x000D7DAC
	public float MinPower
	{
		get
		{
			return 120f;
		}
	}

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x060051C8 RID: 20936 RVA: 0x000D9BB3 File Offset: 0x000D7DB3
	public float MaxSelfHeatKWs
	{
		get
		{
			return 32f;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x060051C9 RID: 20937 RVA: 0x000D9BBA File Offset: 0x000D7DBA
	public float MinSelfHeatKWs
	{
		get
		{
			return 16f;
		}
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x060051CA RID: 20938 RVA: 0x000D9BC1 File Offset: 0x000D7DC1
	public float MaxExhaustedKWs
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x060051CB RID: 20939 RVA: 0x000D9BC8 File Offset: 0x000D7DC8
	public float MinExhaustedKWs
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x060051CC RID: 20940 RVA: 0x000D9BCF File Offset: 0x000D7DCF
	public float CurrentSelfHeatKW
	{
		get
		{
			return Mathf.Lerp(this.MinSelfHeatKWs, this.MaxSelfHeatKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x060051CD RID: 20941 RVA: 0x000D9BE8 File Offset: 0x000D7DE8
	public float CurrentExhaustedKW
	{
		get
		{
			return Mathf.Lerp(this.MinExhaustedKWs, this.MaxExhaustedKWs, this.UserSliderSetting);
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x060051CE RID: 20942 RVA: 0x000D9C01 File Offset: 0x000D7E01
	public float CurrentPowerConsumption
	{
		get
		{
			return Mathf.Lerp(this.MinPower, this.MaxPower, this.UserSliderSetting);
		}
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x000D9C1A File Offset: 0x000D7E1A
	public static void GenerateHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		if (smi.master.produceHeat)
		{
			SpaceHeater.AddExhaustHeat(smi, dt);
			SpaceHeater.AddSelfHeat(smi, dt);
		}
	}

	// Token: 0x060051D0 RID: 20944 RVA: 0x00280F20 File Offset: 0x0027F120
	private static float AddExhaustHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentExhaustedKW = smi.master.CurrentExhaustedKW;
		StructureTemperatureComponents.ExhaustHeat(smi.master.extents, currentExhaustedKW, smi.master.overheatTemperature, dt);
		return currentExhaustedKW;
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x00280F58 File Offset: 0x0027F158
	public static void RefreshHeatEffect(SpaceHeater.StatesInstance smi)
	{
		if (smi.master.heatEffect != null && smi.master.produceHeat)
		{
			float heatBeingProducedValue = smi.IsInsideState(smi.sm.online.heating) ? (smi.master.CurrentExhaustedKW + smi.master.CurrentSelfHeatKW) : 0f;
			smi.master.heatEffect.SetHeatBeingProducedValue(heatBeingProducedValue);
		}
	}

	// Token: 0x060051D2 RID: 20946 RVA: 0x00280FD0 File Offset: 0x0027F1D0
	private static float AddSelfHeat(SpaceHeater.StatesInstance smi, float dt)
	{
		float currentSelfHeatKW = smi.master.CurrentSelfHeatKW;
		GameComps.StructureTemperatures.ProduceEnergy(smi.master.structureTemperature, currentSelfHeatKW * dt, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, dt);
		return currentSelfHeatKW;
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x00281010 File Offset: 0x0027F210
	public void SetUserSpecifiedPowerConsumptionValue(float value)
	{
		if (this.produceHeat)
		{
			this.UserSliderSetting = (value - this.MinPower) / (this.MaxPower - this.MinPower);
			SpaceHeater.RefreshHeatEffect(base.smi);
			this.energyConsumer.BaseWattageRating = this.CurrentPowerConsumption;
		}
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x00281060 File Offset: 0x0027F260
	protected override void OnPrefabInit()
	{
		if (this.produceHeat)
		{
			this.heatStatusItem = new StatusItem("OperatingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.heatStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = string.Format(str, GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				return str;
			};
			this.heatStatusItem.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)data;
				float num = statesInstance.master.CurrentSelfHeatKW + statesInstance.master.CurrentExhaustedKW;
				str = str.Replace("{0}", GameUtil.GetFormattedHeatEnergy(num * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
				string text = string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.OPERATING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentSelfHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				text += string.Format(BUILDING.STATUSITEMS.OPERATINGENERGY.LINEITEM, BUILDING.STATUSITEMS.OPERATINGENERGY.EXHAUSTING, GameUtil.GetFormattedHeatEnergy(statesInstance.master.CurrentExhaustedKW * 1000f, GameUtil.HeatEnergyFormatterUnit.DTU_S));
				str = str.Replace("{1}", text);
				return str;
			};
		}
		base.OnPrefabInit();
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x002810F8 File Offset: 0x0027F2F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.extents = base.GetComponent<OccupyArea>().GetExtents();
		this.overheatTemperature = base.GetComponent<BuildingComplete>().Def.OverheatTemperature;
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.smi.StartSM();
		this.SetUserSpecifiedPowerConsumptionValue(this.CurrentPowerConsumption);
	}

	// Token: 0x060051D6 RID: 20950 RVA: 0x000D9C39 File Offset: 0x000D7E39
	public void SetLiquidHeater()
	{
		this.heatLiquid = true;
	}

	// Token: 0x060051D7 RID: 20951 RVA: 0x00281198 File Offset: 0x0027F398
	private SpaceHeater.MonitorState MonitorHeating(float dt)
	{
		this.monitorCells.Clear();
		GameUtil.GetNonSolidCells(Grid.PosToCell(base.transform.GetPosition()), this.radius, this.monitorCells);
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < this.monitorCells.Count; i++)
		{
			if (Grid.Mass[this.monitorCells[i]] > this.minimumCellMass && ((Grid.Element[this.monitorCells[i]].IsGas && !this.heatLiquid) || (Grid.Element[this.monitorCells[i]].IsLiquid && this.heatLiquid)))
			{
				num++;
				num2 += Grid.Temperature[this.monitorCells[i]];
			}
		}
		if (num == 0)
		{
			if (!this.heatLiquid)
			{
				return SpaceHeater.MonitorState.NotEnoughGas;
			}
			return SpaceHeater.MonitorState.NotEnoughLiquid;
		}
		else
		{
			if (num2 / (float)num >= this.targetTemperature)
			{
				return SpaceHeater.MonitorState.TooHot;
			}
			return SpaceHeater.MonitorState.ReadyToHeat;
		}
	}

	// Token: 0x060051D8 RID: 20952 RVA: 0x00281298 File Offset: 0x0027F498
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATER_TARGETTEMPERATURE, GameUtil.GetFormattedTemperature(this.targetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x060051D9 RID: 20953 RVA: 0x000D9C42 File Offset: 0x000D7E42
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x060051DA RID: 20954 RVA: 0x000D9C49 File Offset: 0x000D7E49
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.ELECTRICAL.WATT;
		}
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x000D9C55 File Offset: 0x000D7E55
	public float GetSliderMin(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MinPower;
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x000D9C6B File Offset: 0x000D7E6B
	public float GetSliderMax(int index)
	{
		if (!this.produceHeat)
		{
			return 0f;
		}
		return this.MaxPower;
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x000D9C81 File Offset: 0x000D7E81
	public float GetSliderValue(int index)
	{
		return this.CurrentPowerConsumption;
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x000D9C89 File Offset: 0x000D7E89
	public void SetSliderValue(float value, int index)
	{
		this.SetUserSpecifiedPowerConsumptionValue(value);
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x000D9C92 File Offset: 0x000D7E92
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x000D9C99 File Offset: 0x000D7E99
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.SPACEHEATERSIDESCREEN.TOOLTIP"), GameUtil.GetFormattedHeatEnergyRate((this.CurrentSelfHeatKW + this.CurrentExhaustedKW) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic));
	}

	// Token: 0x040039A1 RID: 14753
	public float targetTemperature = 308.15f;

	// Token: 0x040039A2 RID: 14754
	public float minimumCellMass;

	// Token: 0x040039A3 RID: 14755
	public int radius = 2;

	// Token: 0x040039A4 RID: 14756
	[SerializeField]
	private bool heatLiquid;

	// Token: 0x040039A5 RID: 14757
	[Serialize]
	public float UserSliderSetting;

	// Token: 0x040039A6 RID: 14758
	public bool produceHeat;

	// Token: 0x040039A7 RID: 14759
	private StatusItem heatStatusItem;

	// Token: 0x040039A8 RID: 14760
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x040039A9 RID: 14761
	private Extents extents;

	// Token: 0x040039AA RID: 14762
	private float overheatTemperature;

	// Token: 0x040039AB RID: 14763
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040039AC RID: 14764
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040039AD RID: 14765
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x040039AE RID: 14766
	[MyCmpGet]
	private EnergyConsumer energyConsumer;

	// Token: 0x040039AF RID: 14767
	private List<int> monitorCells = new List<int>();

	// Token: 0x02000FE3 RID: 4067
	public class StatesInstance : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.GameInstance
	{
		// Token: 0x060051E3 RID: 20963 RVA: 0x000D9CED File Offset: 0x000D7EED
		public StatesInstance(SpaceHeater master) : base(master)
		{
		}
	}

	// Token: 0x02000FE4 RID: 4068
	public class States : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater>
	{
		// Token: 0x060051E4 RID: 20964 RVA: 0x00281300 File Offset: 0x0027F500
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.offline;
			base.serializable = StateMachine.SerializeType.Never;
			this.statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItemOverTemp.resolveStringCallback = delegate(string str, object obj)
			{
				SpaceHeater.StatesInstance statesInstance = (SpaceHeater.StatesInstance)obj;
				return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.offline.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).EventTransition(GameHashes.OperationalChanged, this.online, (SpaceHeater.StatesInstance smi) => smi.master.operational.IsOperational);
			this.online.EventTransition(GameHashes.OperationalChanged, this.offline, (SpaceHeater.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.online.heating).Update("spaceheater_online", delegate(SpaceHeater.StatesInstance smi, float dt)
			{
				switch (smi.master.MonitorHeating(dt))
				{
				case SpaceHeater.MonitorState.ReadyToHeat:
					smi.GoTo(this.online.heating);
					return;
				case SpaceHeater.MonitorState.TooHot:
					smi.GoTo(this.online.overtemp);
					return;
				case SpaceHeater.MonitorState.NotEnoughLiquid:
					smi.GoTo(this.online.undermassliquid);
					return;
				case SpaceHeater.MonitorState.NotEnoughGas:
					smi.GoTo(this.online.undermassgas);
					return;
				default:
					return;
				}
			}, UpdateRate.SIM_4000ms, false);
			this.online.heating.Enter(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect)).Enter(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).ToggleStatusItem((SpaceHeater.StatesInstance smi) => smi.master.heatStatusItem, (SpaceHeater.StatesInstance smi) => smi).Update(new Action<SpaceHeater.StatesInstance, float>(SpaceHeater.GenerateHeat), UpdateRate.SIM_200ms, false).Exit(delegate(SpaceHeater.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Exit(new StateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State.Callback(SpaceHeater.RefreshHeatEffect));
			this.online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassLiquid, null);
			this.online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemUnderMassGas, null);
			this.online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, this.statusItemOverTemp, null);
		}

		// Token: 0x040039B0 RID: 14768
		public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State offline;

		// Token: 0x040039B1 RID: 14769
		public SpaceHeater.States.OnlineStates online;

		// Token: 0x040039B2 RID: 14770
		private StatusItem statusItemUnderMassLiquid;

		// Token: 0x040039B3 RID: 14771
		private StatusItem statusItemUnderMassGas;

		// Token: 0x040039B4 RID: 14772
		private StatusItem statusItemOverTemp;

		// Token: 0x02000FE5 RID: 4069
		public class OnlineStates : GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State
		{
			// Token: 0x040039B5 RID: 14773
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State heating;

			// Token: 0x040039B6 RID: 14774
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State overtemp;

			// Token: 0x040039B7 RID: 14775
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassliquid;

			// Token: 0x040039B8 RID: 14776
			public GameStateMachine<SpaceHeater.States, SpaceHeater.StatesInstance, SpaceHeater, object>.State undermassgas;
		}
	}

	// Token: 0x02000FE7 RID: 4071
	private enum MonitorState
	{
		// Token: 0x040039C2 RID: 14786
		ReadyToHeat,
		// Token: 0x040039C3 RID: 14787
		TooHot,
		// Token: 0x040039C4 RID: 14788
		NotEnoughLiquid,
		// Token: 0x040039C5 RID: 14789
		NotEnoughGas
	}
}
