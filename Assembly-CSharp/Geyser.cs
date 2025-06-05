using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020013B4 RID: 5044
public class Geyser : StateMachineComponent<Geyser.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x17000670 RID: 1648
	// (get) Token: 0x06006763 RID: 26467 RVA: 0x000E7F09 File Offset: 0x000E6109
	// (set) Token: 0x06006762 RID: 26466 RVA: 0x000E7F00 File Offset: 0x000E6100
	public float timeShift { get; private set; }

	// Token: 0x06006764 RID: 26468 RVA: 0x000E7F11 File Offset: 0x000E6111
	public float GetCurrentLifeTime()
	{
		return GameClock.Instance.GetTime() + this.timeShift;
	}

	// Token: 0x06006765 RID: 26469 RVA: 0x002E0E3C File Offset: 0x002DF03C
	public void AlterTime(float timeOffset)
	{
		this.timeShift = Mathf.Max(timeOffset, -GameClock.Instance.GetTime());
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		this.configuration.GetYearLength();
		if (num2 == 0f)
		{
			if ((num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetOnDuration() - num) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetOnDuration() - num))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.erupt);
			return;
		}
		else
		{
			bool flag = (num4 == 0f && this.configuration.GetYearOnDuration() - num3 < this.configuration.GetIterationLength() - num2) | (num3 == 0f && this.configuration.GetYearOffDuration() - num4 >= this.configuration.GetIterationLength() - num2);
			float num5 = this.RemainingEruptPreTime();
			if (flag && num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			if (num5 <= 0f)
			{
				base.smi.GoTo(base.smi.sm.idle);
				return;
			}
			float num6 = this.PreDuration() - num5;
			if ((num3 == 0f) ? (this.configuration.GetYearOffDuration() - num4 > num6) : (num6 > this.configuration.GetYearOnDuration() - num3))
			{
				base.smi.GoTo(base.smi.sm.dormant);
				return;
			}
			base.smi.GoTo(base.smi.sm.pre_erupt);
			return;
		}
	}

	// Token: 0x06006766 RID: 26470 RVA: 0x002E1038 File Offset: 0x002DF238
	public void ShiftTimeTo(Geyser.TimeShiftStep step)
	{
		float num = this.RemainingEruptTime();
		float num2 = this.RemainingNonEruptTime();
		float num3 = this.RemainingActiveTime();
		float num4 = this.RemainingDormantTime();
		float yearLength = this.configuration.GetYearLength();
		switch (step)
		{
		case Geyser.TimeShiftStep.ActiveState:
		{
			float num5 = (num3 > 0f) ? (this.configuration.GetYearOnDuration() - num3) : (yearLength - num4);
			this.AlterTime(this.timeShift - num5);
			return;
		}
		case Geyser.TimeShiftStep.DormantState:
		{
			float num6 = (num3 > 0f) ? num3 : (-(this.configuration.GetYearOffDuration() - num4));
			this.AlterTime(this.timeShift + num6);
			return;
		}
		case Geyser.TimeShiftStep.NextIteration:
		{
			float num7 = (num > 0f) ? (num + this.configuration.GetOffDuration()) : num2;
			this.AlterTime(this.timeShift + num7);
			return;
		}
		case Geyser.TimeShiftStep.PreviousIteration:
		{
			float num8 = (num > 0f) ? (-(this.configuration.GetOnDuration() - num)) : (-(this.configuration.GetIterationLength() - num2));
			if (num > 0f && Mathf.Abs(num8) < this.configuration.GetOnDuration() * 0.05f)
			{
				num8 -= this.configuration.GetIterationLength();
			}
			this.AlterTime(this.timeShift + num8);
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06006767 RID: 26471 RVA: 0x000E7F24 File Offset: 0x000E6124
	public void AddModification(Geyser.GeyserModification modification)
	{
		this.modifications.Add(modification);
		this.UpdateModifier();
	}

	// Token: 0x06006768 RID: 26472 RVA: 0x000E7F38 File Offset: 0x000E6138
	public void RemoveModification(Geyser.GeyserModification modification)
	{
		this.modifications.Remove(modification);
		this.UpdateModifier();
	}

	// Token: 0x06006769 RID: 26473 RVA: 0x002E1170 File Offset: 0x002DF370
	private void UpdateModifier()
	{
		this.modifier.Clear();
		foreach (Geyser.GeyserModification modification in this.modifications)
		{
			this.modifier.AddValues(modification);
		}
		this.configuration.SetModifier(this.modifier);
		this.ApplyConfigurationEmissionValues(this.configuration);
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x0600676A RID: 26474 RVA: 0x000E7F4D File Offset: 0x000E614D
	public void RefreshGeotunerFeedback()
	{
		this.RefreshGeotunerStatusItem();
		this.RefreshStudiedMeter();
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x002E11F8 File Offset: 0x002DF3F8
	private void RefreshGeotunerStatusItem()
	{
		KSelectable component = base.gameObject.GetComponent<KSelectable>();
		if (this.GetAmountOfGeotunersPointingThisGeyser() > 0)
		{
			component.AddStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
			return;
		}
		component.RemoveStatusItem(Db.Get().BuildingStatusItems.GeyserGeotuned, this);
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x002E1250 File Offset: 0x002DF450
	private void RefreshStudiedMeter()
	{
		if (this.studyable.Studied)
		{
			bool flag = this.GetAmountOfGeotunersPointingThisGeyser() > 0;
			GeyserConfig.TrackerMeterAnimNames trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.tracker;
			if (flag)
			{
				trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker;
				int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
				if (amountOfGeotunersAffectingThisGeyser > 0)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_minor;
				}
				if (amountOfGeotunersAffectingThisGeyser >= 5)
				{
					trackerMeterAnimNames = GeyserConfig.TrackerMeterAnimNames.geotracker_major;
				}
			}
			this.studyable.studiedIndicator.meterController.Play(trackerMeterAnimNames.ToString(), KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x0600676D RID: 26477 RVA: 0x000E7F5B File Offset: 0x000E615B
	public int GetAmountOfGeotunersPointingThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
	}

	// Token: 0x0600676E RID: 26478 RVA: 0x000E7F83 File Offset: 0x000E6183
	public int GetAmountOfGeotunersPointingOrWillPointAtThisGeyser()
	{
		return Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this || x.GetFutureGeyser() == this);
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x002E12BC File Offset: 0x002DF4BC
	public int GetAmountOfGeotunersAffectingThisGeyser()
	{
		int num = 0;
		for (int i = 0; i < this.modifications.Count; i++)
		{
			if (this.modifications[i].originID.Contains("GeoTuner"))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x000E7FAB File Offset: 0x000E61AB
	private void OnGeotunerChanged(object o)
	{
		this.RefreshGeotunerFeedback();
	}

	// Token: 0x06006771 RID: 26481 RVA: 0x002E1304 File Offset: 0x002DF504
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		if (this.configuration == null || this.configuration.typeId == HashedString.Invalid)
		{
			this.configuration = base.GetComponent<GeyserConfigurator>().MakeConfiguration();
		}
		else
		{
			PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
			if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
			{
				SimTemperatureTransfer component2 = base.gameObject.GetComponent<SimTemperatureTransfer>();
				component2.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(component2.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
			}
		}
		this.ApplyConfigurationEmissionValues(this.configuration);
		this.GenerateName();
		base.smi.StartSM();
		Workable component3 = base.GetComponent<Studyable>();
		if (component3 != null)
		{
			component3.alwaysShowProgressBar = true;
		}
		Components.Geysers.Add(base.gameObject.GetMyWorldId(), this);
		base.gameObject.Subscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		this.RefreshStudiedMeter();
		this.UpdateModifier();
	}

	// Token: 0x06006772 RID: 26482 RVA: 0x002E1420 File Offset: 0x002DF620
	private void GenerateName()
	{
		StringKey key = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + this.configuration.geyserType.id.ToUpper() + ".NAME");
		if (this.nameable.savedName == Strings.Get(key))
		{
			int cell = Grid.PosToCell(base.gameObject);
			Quadrant[] quadrantOfCell = base.gameObject.GetMyWorld().GetQuadrantOfCell(cell, 2);
			int num = (int)quadrantOfCell[0];
			string str = num.ToString();
			num = (int)quadrantOfCell[1];
			string text = str + num.ToString();
			string[] array = NAMEGEN.GEYSER_IDS.IDs.ToString().Split('\n', StringSplitOptions.None);
			string text2 = array[UnityEngine.Random.Range(0, array.Length)];
			string name = string.Concat(new string[]
			{
				UI.StripLinkFormatting(base.gameObject.GetProperName()),
				" ",
				text2,
				text,
				"‑",
				UnityEngine.Random.Range(0, 10).ToString()
			});
			this.nameable.SetName(name);
		}
	}

	// Token: 0x06006773 RID: 26483 RVA: 0x002E1534 File Offset: 0x002DF734
	public void ApplyConfigurationEmissionValues(GeyserConfigurator.GeyserInstanceConfiguration config)
	{
		this.emitter.emitRange = 2;
		this.emitter.maxPressure = config.GetMaxPressure();
		this.emitter.outputElement = new ElementConverter.OutputElement(config.GetEmitRate(), config.GetElement(), config.GetTemperature(), false, false, (float)this.outputOffset.x, (float)this.outputOffset.y, 1f, config.GetDiseaseIdx(), Mathf.RoundToInt((float)config.GetDiseaseCount() * config.GetEmitRate()), true);
		if (this.emitter.IsSimActive)
		{
			this.emitter.SetSimActive(true);
		}
	}

	// Token: 0x06006774 RID: 26484 RVA: 0x000E7FB3 File Offset: 0x000E61B3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.gameObject.Unsubscribe(1763323737, new Action<object>(this.OnGeotunerChanged));
		Components.Geysers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06006775 RID: 26485 RVA: 0x002E15D4 File Offset: 0x002DF7D4
	private void OnSimRegistered(SimTemperatureTransfer stt)
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		if (this.configuration.geyserType.geyserTemperature - component.Temperature != 0f)
		{
			component.Temperature = this.configuration.geyserType.geyserTemperature;
		}
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnSimRegistered));
	}

	// Token: 0x06006776 RID: 26486 RVA: 0x002E1644 File Offset: 0x002DF844
	public float RemainingPhaseTimeFrom2(float onDuration, float offDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + offDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = Mathf.Max(onDuration - num2, 0f);
			phase = Geyser.Phase.On;
		}
		else
		{
			result = Mathf.Max(onDuration + offDuration - num2, 0f);
			phase = Geyser.Phase.Off;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x06006777 RID: 26487 RVA: 0x002E1694 File Offset: 0x002DF894
	public float RemainingPhaseTimeFrom4(float onDuration, float pstDuration, float offDuration, float preDuration, float time, Geyser.Phase expectedPhase)
	{
		float num = onDuration + pstDuration + offDuration + preDuration;
		float num2 = time % num;
		float result;
		Geyser.Phase phase;
		if (num2 < onDuration)
		{
			result = onDuration - num2;
			phase = Geyser.Phase.On;
		}
		else if (num2 < onDuration + pstDuration)
		{
			result = onDuration + pstDuration - num2;
			phase = Geyser.Phase.Pst;
		}
		else if (num2 < onDuration + pstDuration + offDuration)
		{
			result = onDuration + pstDuration + offDuration - num2;
			phase = Geyser.Phase.Off;
		}
		else
		{
			result = onDuration + pstDuration + offDuration + preDuration - num2;
			phase = Geyser.Phase.Pre;
		}
		if (expectedPhase != Geyser.Phase.Any && phase != expectedPhase)
		{
			return 0f;
		}
		return result;
	}

	// Token: 0x06006778 RID: 26488 RVA: 0x000E7FED File Offset: 0x000E61ED
	private float IdleDuration()
	{
		return this.configuration.GetOffDuration() * 0.84999996f;
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x000E8000 File Offset: 0x000E6200
	private float PreDuration()
	{
		return this.configuration.GetOffDuration() * 0.1f;
	}

	// Token: 0x0600677A RID: 26490 RVA: 0x000E8013 File Offset: 0x000E6213
	private float PostDuration()
	{
		return this.configuration.GetOffDuration() * 0.05f;
	}

	// Token: 0x0600677B RID: 26491 RVA: 0x000E8026 File Offset: 0x000E6226
	private float EruptDuration()
	{
		return this.configuration.GetOnDuration();
	}

	// Token: 0x0600677C RID: 26492 RVA: 0x000E8033 File Offset: 0x000E6233
	public bool ShouldGoDormant()
	{
		return this.RemainingActiveTime() <= 0f;
	}

	// Token: 0x0600677D RID: 26493 RVA: 0x000E8045 File Offset: 0x000E6245
	public float RemainingIdleTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x0600677E RID: 26494 RVA: 0x000E806C File Offset: 0x000E626C
	public float RemainingEruptPreTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pre);
	}

	// Token: 0x0600677F RID: 26495 RVA: 0x000E8093 File Offset: 0x000E6293
	public float RemainingEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x06006780 RID: 26496 RVA: 0x000E80B8 File Offset: 0x000E62B8
	public float RemainingEruptPostTime()
	{
		return this.RemainingPhaseTimeFrom4(this.EruptDuration(), this.PostDuration(), this.IdleDuration(), this.PreDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Pst);
	}

	// Token: 0x06006781 RID: 26497 RVA: 0x000E80DF File Offset: 0x000E62DF
	public float RemainingNonEruptTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetOnDuration(), this.configuration.GetOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x06006782 RID: 26498 RVA: 0x000E8104 File Offset: 0x000E6304
	public float RemainingDormantTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.Off);
	}

	// Token: 0x06006783 RID: 26499 RVA: 0x000E8129 File Offset: 0x000E6329
	public float RemainingActiveTime()
	{
		return this.RemainingPhaseTimeFrom2(this.configuration.GetYearOnDuration(), this.configuration.GetYearOffDuration(), this.GetCurrentLifeTime(), Geyser.Phase.On);
	}

	// Token: 0x06006784 RID: 26500 RVA: 0x002E1700 File Offset: 0x002DF900
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.configuration.GetElement()).tag.ProperName();
		List<GeoTuner.Instance> items = Components.GeoTuners.GetItems(base.gameObject.GetMyWorldId());
		GeoTuner.Instance instance = items.Find((GeoTuner.Instance g) => g.GetAssignedGeyser() == this);
		int num = items.Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == this);
		bool flag = num > 0;
		string text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		if (flag)
		{
			Func<float, float> func = delegate(float emissionPerCycleModifier)
			{
				float num8 = 600f / this.configuration.GetIterationLength();
				return emissionPerCycleModifier / num8 / this.configuration.GetOnDuration();
			};
			int amountOfGeotunersAffectingThisGeyser = this.GetAmountOfGeotunersAffectingThisGeyser();
			float num2 = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.temperatureModifier * this.configuration.geyserType.temperature) : instance.currentGeyserModification.temperatureModifier;
			float num3 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * this.configuration.scaledRate) : instance.currentGeyserModification.massPerCycleModifier);
			float num4 = (float)amountOfGeotunersAffectingThisGeyser * num2;
			float num5 = (float)amountOfGeotunersAffectingThisGeyser * num3;
			string arg2 = ((num4 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num4, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string arg3 = ((num5 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num5, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			string str = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string str2 = ((num3 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num3, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			text = string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED, ElementLoader.FindElementByHash(this.configuration.GetElement()).name, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_COUNT, amountOfGeotunersAffectingThisGeyser.ToString(), num.ToString());
			text += "\n";
			text = text + "\n" + string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PRODUCTION_GEOTUNED_TOTAL, arg3, arg2);
			for (int i = 0; i < amountOfGeotunersAffectingThisGeyser; i++)
			{
				string text2 = "\n    • " + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE.ToString();
				text2 = text2 + str2 + " " + str;
				text += text2;
			}
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PRODUCTION, arg, GameUtil.GetFormattedMass(this.configuration.GetEmitRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedTemperature(this.configuration.GetTemperature(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), text, Descriptor.DescriptorType.Effect, false));
		if (this.configuration.GetDiseaseIdx() != 255)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_DISEASE, GameUtil.GetFormattedDiseaseName(this.configuration.GetDiseaseIdx(), false)), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_PERIOD, GameUtil.GetFormattedTime(this.configuration.GetOnDuration(), "F0"), GameUtil.GetFormattedTime(this.configuration.GetIterationLength(), "F0")), Descriptor.DescriptorType.Effect, false));
		Studyable component = base.GetComponent<Studyable>();
		if (component && !component.Studied)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_PERIOD, GameUtil.GetFormattedCycles(this.configuration.GetYearOnDuration(), "F1", false), GameUtil.GetFormattedCycles(this.configuration.GetYearLength(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			if (base.smi.IsInsideState(base.smi.sm.dormant))
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_ACTIVE, GameUtil.GetFormattedCycles(this.RemainingDormantTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_NEXT_DORMANT, GameUtil.GetFormattedCycles(this.RemainingActiveTime(), "F1", false)), Descriptor.DescriptorType.Effect, false));
			}
			string text3 = UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT.Replace("{average}", GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{element}", this.configuration.geyserType.element.CreateTag().ProperName());
			if (flag)
			{
				text3 += "\n";
				text3 = text3 + "\n" + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE;
				int amountOfGeotunersAffectingThisGeyser2 = this.GetAmountOfGeotunersAffectingThisGeyser();
				float num6 = (Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (instance.currentGeyserModification.massPerCycleModifier * 100f) : (instance.currentGeyserModification.massPerCycleModifier * 100f / this.configuration.scaledRate);
				float num7 = num6 * (float)amountOfGeotunersAffectingThisGeyser2;
				text3 = text3 + GameUtil.AddPositiveSign(num7.ToString("0.0"), num7 > 0f) + "%";
				for (int j = 0; j < amountOfGeotunersAffectingThisGeyser2; j++)
				{
					string text4 = "\n    • " + UI.BUILDINGEFFECTS.TOOLTIPS.GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW.ToString();
					text4 = text4 + GameUtil.AddPositiveSign(num6.ToString("0.0"), num6 > 0f) + "%";
					text3 += text4;
				}
			}
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.GEYSER_YEAR_AVR_OUTPUT, GameUtil.GetFormattedMass(this.configuration.GetAverageEmission(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), text3, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x04004E08 RID: 19976
	public static Geyser.ModificationMethod massModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E09 RID: 19977
	public static Geyser.ModificationMethod temperatureModificationMethod = Geyser.ModificationMethod.Values;

	// Token: 0x04004E0A RID: 19978
	public static Geyser.ModificationMethod IterationDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E0B RID: 19979
	public static Geyser.ModificationMethod IterationPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E0C RID: 19980
	public static Geyser.ModificationMethod yearDurationModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E0D RID: 19981
	public static Geyser.ModificationMethod yearPercentageModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E0E RID: 19982
	public static Geyser.ModificationMethod maxPressureModificationMethod = Geyser.ModificationMethod.Percentages;

	// Token: 0x04004E0F RID: 19983
	[MyCmpAdd]
	private ElementEmitter emitter;

	// Token: 0x04004E10 RID: 19984
	[MyCmpAdd]
	private UserNameable nameable;

	// Token: 0x04004E11 RID: 19985
	[MyCmpGet]
	private Studyable studyable;

	// Token: 0x04004E12 RID: 19986
	[Serialize]
	public GeyserConfigurator.GeyserInstanceConfiguration configuration;

	// Token: 0x04004E13 RID: 19987
	public Vector2I outputOffset;

	// Token: 0x04004E14 RID: 19988
	public List<Geyser.GeyserModification> modifications = new List<Geyser.GeyserModification>();

	// Token: 0x04004E15 RID: 19989
	private Geyser.GeyserModification modifier;

	// Token: 0x04004E17 RID: 19991
	private const float PRE_PCT = 0.1f;

	// Token: 0x04004E18 RID: 19992
	private const float POST_PCT = 0.05f;

	// Token: 0x020013B5 RID: 5045
	public enum ModificationMethod
	{
		// Token: 0x04004E1A RID: 19994
		Values,
		// Token: 0x04004E1B RID: 19995
		Percentages
	}

	// Token: 0x020013B6 RID: 5046
	public struct GeyserModification
	{
		// Token: 0x0600678C RID: 26508 RVA: 0x002E1E90 File Offset: 0x002E0090
		public void Clear()
		{
			this.massPerCycleModifier = 0f;
			this.temperatureModifier = 0f;
			this.iterationDurationModifier = 0f;
			this.iterationPercentageModifier = 0f;
			this.yearDurationModifier = 0f;
			this.yearPercentageModifier = 0f;
			this.maxPressureModifier = 0f;
			this.modifyElement = false;
			this.newElement = (SimHashes)0;
		}

		// Token: 0x0600678D RID: 26509 RVA: 0x002E1EF8 File Offset: 0x002E00F8
		public void AddValues(Geyser.GeyserModification modification)
		{
			this.massPerCycleModifier += modification.massPerCycleModifier;
			this.temperatureModifier += modification.temperatureModifier;
			this.iterationDurationModifier += modification.iterationDurationModifier;
			this.iterationPercentageModifier += modification.iterationPercentageModifier;
			this.yearDurationModifier += modification.yearDurationModifier;
			this.yearPercentageModifier += modification.yearPercentageModifier;
			this.maxPressureModifier += modification.maxPressureModifier;
			this.modifyElement |= modification.modifyElement;
			this.newElement = ((modification.newElement == (SimHashes)0) ? this.newElement : modification.newElement);
		}

		// Token: 0x0600678E RID: 26510 RVA: 0x000E81B9 File Offset: 0x000E63B9
		public bool IsNewElementInUse()
		{
			return this.modifyElement && this.newElement > (SimHashes)0;
		}

		// Token: 0x04004E1C RID: 19996
		public string originID;

		// Token: 0x04004E1D RID: 19997
		public float massPerCycleModifier;

		// Token: 0x04004E1E RID: 19998
		public float temperatureModifier;

		// Token: 0x04004E1F RID: 19999
		public float iterationDurationModifier;

		// Token: 0x04004E20 RID: 20000
		public float iterationPercentageModifier;

		// Token: 0x04004E21 RID: 20001
		public float yearDurationModifier;

		// Token: 0x04004E22 RID: 20002
		public float yearPercentageModifier;

		// Token: 0x04004E23 RID: 20003
		public float maxPressureModifier;

		// Token: 0x04004E24 RID: 20004
		public bool modifyElement;

		// Token: 0x04004E25 RID: 20005
		public SimHashes newElement;
	}

	// Token: 0x020013B7 RID: 5047
	public class StatesInstance : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.GameInstance
	{
		// Token: 0x0600678F RID: 26511 RVA: 0x000E81CE File Offset: 0x000E63CE
		public StatesInstance(Geyser smi) : base(smi)
		{
		}
	}

	// Token: 0x020013B8 RID: 5048
	public class States : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser>
	{
		// Token: 0x06006790 RID: 26512 RVA: 0x002E1FBC File Offset: 0x002E01BC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.DefaultState(this.idle).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.dormant.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutDormant, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingDormantTime(), this.pre_erupt);
			this.idle.PlayAnim("inactive", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).Enter(delegate(Geyser.StatesInstance smi)
			{
				if (smi.master.ShouldGoDormant())
				{
					smi.GoTo(this.dormant);
				}
			}).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingIdleTime(), this.pre_erupt);
			this.pre_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPreTime(), this.erupt);
			this.erupt.TriggerOnEnter(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => true).TriggerOnExit(GameHashes.GeyserEruption, (Geyser.StatesInstance smi) => false).DefaultState(this.erupt.erupting).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptTime(), this.post_erupt).Enter(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(true);
			}).Exit(delegate(Geyser.StatesInstance smi)
			{
				smi.master.emitter.SetEmitting(false);
			});
			this.erupt.erupting.EventTransition(GameHashes.EmitterBlocked, this.erupt.overpressure, (Geyser.StatesInstance smi) => smi.GetComponent<ElementEmitter>().isEmitterBlocked).PlayAnim("erupt", KAnim.PlayMode.Loop);
			this.erupt.overpressure.EventTransition(GameHashes.EmitterUnblocked, this.erupt.erupting, (Geyser.StatesInstance smi) => !smi.GetComponent<ElementEmitter>().isEmitterBlocked).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).PlayAnim("inactive", KAnim.PlayMode.Loop);
			this.post_erupt.PlayAnim("shake", KAnim.PlayMode.Loop).ToggleMainStatusItem(Db.Get().MiscStatusItems.SpoutIdle, null).ScheduleGoTo((Geyser.StatesInstance smi) => smi.master.RemainingEruptPostTime(), this.idle);
		}

		// Token: 0x04004E26 RID: 20006
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State dormant;

		// Token: 0x04004E27 RID: 20007
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State idle;

		// Token: 0x04004E28 RID: 20008
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State pre_erupt;

		// Token: 0x04004E29 RID: 20009
		public Geyser.States.EruptState erupt;

		// Token: 0x04004E2A RID: 20010
		public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State post_erupt;

		// Token: 0x020013B9 RID: 5049
		public class EruptState : GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State
		{
			// Token: 0x04004E2B RID: 20011
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State erupting;

			// Token: 0x04004E2C RID: 20012
			public GameStateMachine<Geyser.States, Geyser.StatesInstance, Geyser, object>.State overpressure;
		}
	}

	// Token: 0x020013BB RID: 5051
	public enum TimeShiftStep
	{
		// Token: 0x04004E3B RID: 20027
		ActiveState,
		// Token: 0x04004E3C RID: 20028
		DormantState,
		// Token: 0x04004E3D RID: 20029
		NextIteration,
		// Token: 0x04004E3E RID: 20030
		PreviousIteration
	}

	// Token: 0x020013BC RID: 5052
	public enum Phase
	{
		// Token: 0x04004E40 RID: 20032
		Pre,
		// Token: 0x04004E41 RID: 20033
		On,
		// Token: 0x04004E42 RID: 20034
		Pst,
		// Token: 0x04004E43 RID: 20035
		Off,
		// Token: 0x04004E44 RID: 20036
		Any
	}
}
