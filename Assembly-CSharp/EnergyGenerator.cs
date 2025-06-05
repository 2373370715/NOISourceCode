using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012D1 RID: 4817
[SerializationConfig(MemberSerialization.OptIn)]
public class EnergyGenerator : Generator, IGameObjectEffectDescriptor, ISingleSliderControl, ISliderControl
{
	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x060062CC RID: 25292 RVA: 0x000E4EBA File Offset: 0x000E30BA
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x060062CD RID: 25293 RVA: 0x000CF907 File Offset: 0x000CDB07
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x060062CE RID: 25294 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x060062CF RID: 25295 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x060062D0 RID: 25296 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x060062D1 RID: 25297 RVA: 0x000E4EC1 File Offset: 0x000E30C1
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x060062D2 RID: 25298 RVA: 0x000E4ECF File Offset: 0x000E30CF
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x060062D3 RID: 25299 RVA: 0x002C5EFC File Offset: 0x002C40FC
	string ISliderControl.GetSliderTooltip(int index)
	{
		ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP"), component.RequestedItemTag.ProperName(), this.batteryRefillPercent * 100f);
	}

	// Token: 0x060062D4 RID: 25300 RVA: 0x000E4EDE File Offset: 0x000E30DE
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALDELIVERYGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x060062D5 RID: 25301 RVA: 0x002C5F40 File Offset: 0x002C4140
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		EnergyGenerator.EnsureStatusItemAvailable();
		base.Subscribe<EnergyGenerator>(824508782, EnergyGenerator.OnActiveChangedDelegate);
		if (!this.ignoreBatteryRefillPercent)
		{
			base.gameObject.AddOrGet<CopyBuildingSettings>();
			base.Subscribe<EnergyGenerator>(-905833192, EnergyGenerator.OnCopySettingsDelegate);
		}
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x002C5F90 File Offset: 0x002C4190
	private void OnCopySettings(object data)
	{
		EnergyGenerator component = ((GameObject)data).GetComponent<EnergyGenerator>();
		if (component != null)
		{
			this.batteryRefillPercent = component.batteryRefillPercent;
		}
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x00273C40 File Offset: 0x00271E40
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x002C5FC0 File Offset: 0x002C41C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasMeter)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
		}
	}

	// Token: 0x060062D9 RID: 25305 RVA: 0x002C6024 File Offset: 0x002C4224
	private bool IsConvertible(float dt)
	{
		bool flag = true;
		foreach (EnergyGenerator.InputItem inputItem in this.formula.inputs)
		{
			float massAvailable = this.storage.GetMassAvailable(inputItem.tag);
			float num = inputItem.consumptionRate * dt;
			flag = (flag && massAvailable >= num);
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x060062DA RID: 25306 RVA: 0x002C6088 File Offset: 0x002C4288
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		if (this.hasMeter)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[0];
			float positionPercent = this.storage.GetMassAvailable(inputItem.tag) / inputItem.maxStoredMass;
			this.meter.SetPositionPercent(positionPercent);
		}
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		bool value = false;
		if (this.operational.IsOperational)
		{
			bool flag = false;
			List<Battery> batteriesOnCircuit = Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID);
			if (!this.ignoreBatteryRefillPercent && batteriesOnCircuit.Count > 0)
			{
				using (List<Battery>.Enumerator enumerator = batteriesOnCircuit.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Battery battery = enumerator.Current;
						if (this.batteryRefillPercent <= 0f && battery.PercentFull <= 0f)
						{
							flag = true;
							break;
						}
						if (battery.PercentFull < this.batteryRefillPercent)
						{
							flag = true;
							break;
						}
					}
					goto IL_105;
				}
			}
			flag = true;
			IL_105:
			if (!this.ignoreBatteryRefillPercent)
			{
				this.selectable.ToggleStatusItem(EnergyGenerator.batteriesSufficientlyFull, !flag, null);
			}
			if (this.delivery != null)
			{
				this.delivery.Pause(!flag, "Circuit has sufficient energy");
			}
			if (this.formula.inputs != null)
			{
				bool flag2 = this.IsConvertible(dt);
				this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedResourceMass, !flag2, this.formula);
				if (flag2)
				{
					foreach (EnergyGenerator.InputItem inputItem2 in this.formula.inputs)
					{
						float amount = inputItem2.consumptionRate * dt;
						this.storage.ConsumeIgnoringDisease(inputItem2.tag, amount);
					}
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					foreach (EnergyGenerator.OutputItem output in this.formula.outputs)
					{
						this.Emit(output, dt, component);
					}
					base.GenerateJoules(base.WattageRating * dt, false);
					this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this);
					value = true;
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x060062DB RID: 25307 RVA: 0x002C6308 File Offset: 0x002C4508
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.inputs == null || this.formula.inputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.inputs.Length; i++)
		{
			EnergyGenerator.InputItem inputItem = this.formula.inputs[i];
			string arg = inputItem.tag.ProperName();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(inputItem.consumptionRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060062DC RID: 25308 RVA: 0x002C63D4 File Offset: 0x002C45D4
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.formula.outputs == null || this.formula.outputs.Length == 0)
		{
			return list;
		}
		for (int i = 0; i < this.formula.outputs.Length; i++)
		{
			EnergyGenerator.OutputItem outputItem = this.formula.outputs[i];
			string arg = ElementLoader.FindElementByHash(outputItem.element).tag.ProperName();
			Descriptor item = default(Descriptor);
			if (outputItem.minTemperature > 0f)
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_MINORENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(outputItem.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect);
			}
			else
			{
				item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_ENTITYTEMP, arg, GameUtil.GetFormattedMass(outputItem.creationRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect);
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060062DD RID: 25309 RVA: 0x002C6524 File Offset: 0x002C4724
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x060062DE RID: 25310 RVA: 0x000E4EE5 File Offset: 0x000E30E5
	public static StatusItem BatteriesSufficientlyFull
	{
		get
		{
			return EnergyGenerator.batteriesSufficientlyFull;
		}
	}

	// Token: 0x060062DF RID: 25311 RVA: 0x002C65C0 File Offset: 0x002C47C0
	public static void EnsureStatusItemAvailable()
	{
		if (EnergyGenerator.batteriesSufficientlyFull == null)
		{
			EnergyGenerator.batteriesSufficientlyFull = new StatusItem("BatteriesSufficientlyFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x060062E0 RID: 25312 RVA: 0x002C65FC File Offset: 0x002C47FC
	public static EnergyGenerator.Formula CreateSimpleFormula(Tag input_element, float input_mass_rate, float max_stored_input_mass, SimHashes output_element = SimHashes.Void, float output_mass_rate = 0f, bool store_output_mass = true, CellOffset output_offset = default(CellOffset), float min_output_temperature = 0f)
	{
		EnergyGenerator.Formula result = default(EnergyGenerator.Formula);
		result.inputs = new EnergyGenerator.InputItem[]
		{
			new EnergyGenerator.InputItem(input_element, input_mass_rate, max_stored_input_mass)
		};
		if (output_element != SimHashes.Void)
		{
			result.outputs = new EnergyGenerator.OutputItem[]
			{
				new EnergyGenerator.OutputItem(output_element, output_mass_rate, store_output_mass, output_offset, min_output_temperature)
			};
		}
		else
		{
			result.outputs = null;
		}
		return result;
	}

	// Token: 0x060062E1 RID: 25313 RVA: 0x002C6664 File Offset: 0x002C4864
	private void Emit(EnergyGenerator.OutputItem output, float dt, PrimaryElement root_pe)
	{
		Element element = ElementLoader.FindElementByHash(output.element);
		float num = output.creationRate * dt;
		if (output.store)
		{
			if (element.IsGas)
			{
				this.storage.AddGasChunk(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			if (element.IsLiquid)
			{
				this.storage.AddLiquid(output.element, num, root_pe.Temperature, byte.MaxValue, 0, true, true);
				return;
			}
			GameObject go = element.substance.SpawnResource(base.transform.GetPosition(), num, root_pe.Temperature, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, true, false, true, false);
			return;
		}
		else
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), output.emitOffset);
			float temperature = Mathf.Max(root_pe.Temperature, output.minTemperature);
			if (element.IsGas)
			{
				SimMessages.ModifyMass(num2, num, byte.MaxValue, 0, CellEventLogger.Instance.EnergyGeneratorModifyMass, temperature, output.element);
				return;
			}
			if (element.IsLiquid)
			{
				ushort elementIndex = ElementLoader.GetElementIndex(output.element);
				FallingWater.instance.AddParticle(num2, elementIndex, num, temperature, byte.MaxValue, 0, true, false, false, false);
				return;
			}
			element.substance.SpawnResource(Grid.CellToPosCCC(num2, Grid.SceneLayer.Front), num, temperature, byte.MaxValue, 0, true, false, false);
			return;
		}
	}

	// Token: 0x040046D1 RID: 18129
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040046D2 RID: 18130
	[MyCmpGet]
	private ManualDeliveryKG delivery;

	// Token: 0x040046D3 RID: 18131
	[SerializeField]
	[Serialize]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x040046D4 RID: 18132
	public bool ignoreBatteryRefillPercent;

	// Token: 0x040046D5 RID: 18133
	public bool hasMeter = true;

	// Token: 0x040046D6 RID: 18134
	private static StatusItem batteriesSufficientlyFull;

	// Token: 0x040046D7 RID: 18135
	public Meter.Offset meterOffset;

	// Token: 0x040046D8 RID: 18136
	[SerializeField]
	public EnergyGenerator.Formula formula;

	// Token: 0x040046D9 RID: 18137
	private MeterController meter;

	// Token: 0x040046DA RID: 18138
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040046DB RID: 18139
	private static readonly EventSystem.IntraObjectHandler<EnergyGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EnergyGenerator>(delegate(EnergyGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x020012D2 RID: 4818
	[DebuggerDisplay("{tag} -{consumptionRate} kg/s")]
	[Serializable]
	public struct InputItem
	{
		// Token: 0x060062E4 RID: 25316 RVA: 0x000E4F3C File Offset: 0x000E313C
		public InputItem(Tag tag, float consumption_rate, float max_stored_mass)
		{
			this.tag = tag;
			this.consumptionRate = consumption_rate;
			this.maxStoredMass = max_stored_mass;
		}

		// Token: 0x040046DC RID: 18140
		public Tag tag;

		// Token: 0x040046DD RID: 18141
		public float consumptionRate;

		// Token: 0x040046DE RID: 18142
		public float maxStoredMass;
	}

	// Token: 0x020012D3 RID: 4819
	[DebuggerDisplay("{element} {creationRate} kg/s")]
	[Serializable]
	public struct OutputItem
	{
		// Token: 0x060062E5 RID: 25317 RVA: 0x000E4F53 File Offset: 0x000E3153
		public OutputItem(SimHashes element, float creation_rate, bool store, float min_temperature = 0f)
		{
			this = new EnergyGenerator.OutputItem(element, creation_rate, store, CellOffset.none, min_temperature);
		}

		// Token: 0x060062E6 RID: 25318 RVA: 0x000E4F65 File Offset: 0x000E3165
		public OutputItem(SimHashes element, float creation_rate, bool store, CellOffset emit_offset, float min_temperature = 0f)
		{
			this.element = element;
			this.creationRate = creation_rate;
			this.store = store;
			this.emitOffset = emit_offset;
			this.minTemperature = min_temperature;
		}

		// Token: 0x040046DF RID: 18143
		public SimHashes element;

		// Token: 0x040046E0 RID: 18144
		public float creationRate;

		// Token: 0x040046E1 RID: 18145
		public bool store;

		// Token: 0x040046E2 RID: 18146
		public CellOffset emitOffset;

		// Token: 0x040046E3 RID: 18147
		public float minTemperature;
	}

	// Token: 0x020012D4 RID: 4820
	[Serializable]
	public struct Formula
	{
		// Token: 0x040046E4 RID: 18148
		public EnergyGenerator.InputItem[] inputs;

		// Token: 0x040046E5 RID: 18149
		public EnergyGenerator.OutputItem[] outputs;

		// Token: 0x040046E6 RID: 18150
		public Tag meterTag;
	}
}
