﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	public class CreatureStatusItems : StatusItems
	{
		public CreatureStatusItems(ResourceSet parent) : base("CreatureStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		private void CreateStatusItems()
		{
			this.Dead = new StatusItem("Dead", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Hot = new StatusItem("Hot", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Hot.resolveStringCallback = delegate(string str, object data)
			{
				TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.Hot_Crop = new StatusItem("Hot_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Hot_Crop.resolveStringCallback = delegate(string str, object data)
			{
				TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable)data;
				str = str.Replace("{low_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{high_temperature}", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Scalding = new StatusItem("Scalding", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, true, OverlayModes.None.ID, true, 129022, null);
			this.Scalding.resolveTooltipCallback = delegate(string str, object data)
			{
				float averageExternalTemperature = ((ScaldingMonitor.Instance)data).AverageExternalTemperature;
				float scaldingThreshold = ((ScaldingMonitor.Instance)data).GetScaldingThreshold();
				str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(averageExternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(scaldingThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Scalding.AddNotification(null, null, null);
			this.Scolding = new StatusItem("Scolding", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, true, OverlayModes.None.ID, true, 129022, null);
			this.Scolding.resolveTooltipCallback = delegate(string str, object data)
			{
				float averageExternalTemperature = ((ScaldingMonitor.Instance)data).AverageExternalTemperature;
				float scoldingThreshold = ((ScaldingMonitor.Instance)data).GetScoldingThreshold();
				str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(averageExternalTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(scoldingThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Scolding.AddNotification(null, null, null);
			this.Cold = new StatusItem("Cold", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Cold.resolveStringCallback = delegate(string str, object data)
			{
				TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.Cold_Crop = new StatusItem("Cold_Crop", "CREATURES", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Cold_Crop.resolveStringCallback = delegate(string str, object data)
			{
				TemperatureVulnerable temperatureVulnerable = (TemperatureVulnerable)data;
				str = str.Replace("low_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningLow, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("high_temperature", GameUtil.GetFormattedTemperature(temperatureVulnerable.TemperatureWarningHigh, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Crop_Too_Dark = new StatusItem("Crop_Too_Dark", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Crop_Too_Bright = new StatusItem("Crop_Too_Bright", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Crop_Blighted = new StatusItem("Crop_Blighted", "CREATURES", "status_item_plant_blighted", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Hyperthermia = new StatusItem("Hyperthermia", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.Hyperthermia.resolveTooltipCallback = delegate(string str, object data)
			{
				float value = ((TemperatureMonitor.Instance)data).temperature.value;
				float hyperthermiaThreshold = ((TemperatureMonitor.Instance)data).HyperthermiaThreshold;
				str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hyperthermiaThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Hypothermia = new StatusItem("Hypothermia", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.Hypothermia.resolveTooltipCallback = delegate(string str, object data)
			{
				float value = ((TemperatureMonitor.Instance)data).temperature.value;
				float hypothermiaThreshold = ((TemperatureMonitor.Instance)data).HypothermiaThreshold;
				str = str.Replace("{InternalTemperature}", GameUtil.GetFormattedTemperature(value, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(hypothermiaThreshold, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Suffocating = new StatusItem("Suffocating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Hatching = new StatusItem("Hatching", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Incubating = new StatusItem("Incubating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Drowning = new StatusItem("Drowning", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Drowning.resolveStringCallback = ((string str, object data) => str);
			this.AquaticCreatureSuffocating = new StatusItem("AquaticCreatureSuffocating", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.AquaticCreatureSuffocating.resolveTooltipCallback = delegate(string str, object data)
			{
				AquaticCreatureSuffocationMonitor.Instance instance = (AquaticCreatureSuffocationMonitor.Instance)data;
				str = GameUtil.SafeStringFormat(str, new object[]
				{
					GameUtil.GetFormattedCycles(instance.TimeUntilDeath, "F1", false)
				});
				return str;
			};
			this.ProducingSugarWater = new StatusItem("ProducingSugarWater", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
			this.ProducingSugarWater.resolveStringCallback = delegate(string str, object data)
			{
				SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ProducingSugarWater.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance)data;
				PlantBranchGrower.Instance smi = instance.GetSMI<PlantBranchGrower.Instance>();
				for (int i = 0; i < instance.def.OptimalAmountOfBranches; i++)
				{
					string text = CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE_MISSING;
					string newValue = SpaceTreeBranchConfig.BRANCH_NAMES[i];
					GameObject branch = smi.GetBranch(i);
					if (branch != null)
					{
						SpaceTreeBranch.Instance smi2 = branch.GetSMI<SpaceTreeBranch.Instance>();
						if (smi2 != null && !smi2.isMasterNull)
						{
							if (smi2.IsBranchFullyGrown)
							{
								string formattedPercent = GameUtil.GetFormattedPercent(smi2.Productivity * 100f, GameUtil.TimeSlice.None);
								text = CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE;
								text = text.Replace("{1}", formattedPercent);
							}
							else
							{
								string formattedPercent2 = GameUtil.GetFormattedPercent(smi2.GetcurrentGrowthPercentage() * 100f, GameUtil.TimeSlice.None);
								text = CREATURES.STATUSITEMS.PRODUCINGSUGARWATER.BRANCH_LINE_GROWING;
								text = text.Replace("{1}", formattedPercent2);
							}
						}
					}
					text = text.Replace("{0}", newValue);
					string oldValue = "{BRANCH_" + i.ToString() + "}";
					str = str.Replace(oldValue, text);
				}
				str = str.Replace("{0}", GameUtil.GetFormattedMass(instance.GetProductionSpeed() * 20f / instance.OptimalProductionDuration, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				str = str.Replace("{1}", instance.def.OptimalAmountOfBranches.ToString());
				str = str.Replace("{2}", GameUtil.GetFormattedLux(10000));
				return str;
			};
			this.SugarWaterProductionPaused = new StatusItem("SugarWaterProductionPaused", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.SugarWaterProductionPaused.resolveStringCallback = delegate(string str, object data)
			{
				SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.SugarWaterProductionPaused.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance)data;
				PlantBranchGrower.Instance smi = instance.GetSMI<PlantBranchGrower.Instance>();
				for (int i = 0; i < instance.def.OptimalAmountOfBranches; i++)
				{
					string text = CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE_MISSING;
					string newValue = SpaceTreeBranchConfig.BRANCH_NAMES[i];
					GameObject branch = smi.GetBranch(i);
					if (branch != null)
					{
						SpaceTreeBranch.Instance smi2 = branch.GetSMI<SpaceTreeBranch.Instance>();
						if (smi2 != null && !smi2.isMasterNull)
						{
							if (smi2.IsBranchFullyGrown)
							{
								string formattedPercent = GameUtil.GetFormattedPercent(smi2.Productivity * 100f, GameUtil.TimeSlice.None);
								text = CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE;
								text = text.Replace("{1}", formattedPercent);
							}
							else
							{
								string formattedPercent2 = GameUtil.GetFormattedPercent(smi2.GetcurrentGrowthPercentage() * 100f, GameUtil.TimeSlice.None);
								text = CREATURES.STATUSITEMS.SUGARWATERPRODUCTIONPAUSED.BRANCH_LINE_GROWING;
								text = text.Replace("{1}", formattedPercent2);
							}
						}
					}
					text = text.Replace("{0}", newValue);
					string oldValue = "{BRANCH_" + i.ToString() + "}";
					str = str.Replace(oldValue, text);
				}
				str = str.Replace("{0}", instance.def.OptimalAmountOfBranches.ToString());
				str = str.Replace("{1}", GameUtil.GetFormattedLux(10000));
				return str;
			};
			this.SugarWaterProductionWilted = new StatusItem("SugarWaterProductionWilted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.SugarWaterProductionWilted.resolveStringCallback = delegate(string str, object data)
			{
				SpaceTreePlant.Instance instance = (SpaceTreePlant.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.CurrentProductionProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.SpaceTreeBranchLightStatus = new StatusItem("SpaceTreeBranchLightStatus", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
			this.SpaceTreeBranchLightStatus.resolveStringCallback = delegate(string str, object data)
			{
				SpaceTreeBranch.Instance instance = (SpaceTreeBranch.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(instance.Productivity * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.SpaceTreeBranchLightStatus.resolveTooltipCallback = delegate(string str, object data)
			{
				SpaceTreeBranch.Instance instance = (SpaceTreeBranch.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedLux(instance.def.OPTIMAL_LUX_LEVELS));
				str = str.Replace("{1}", GameUtil.GetFormattedLux(instance.CurrentAmountOfLux));
				return str;
			};
			this.Saturated = new StatusItem("Saturated", "CREATURES", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Saturated.resolveStringCallback = ((string str, object data) => str);
			this.DryingOut = new StatusItem("DryingOut", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026, null);
			this.DryingOut.resolveStringCallback = ((string str, object data) => str);
			this.ReadyForHarvest = new StatusItem("ReadyForHarvest", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026, null);
			this.ReadyForHarvest_Branch = new StatusItem("ReadyForHarvest_Branch", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026, null);
			this.Growing = new StatusItem("Growing", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026, null);
			this.Growing.resolveStringCallback = delegate(string str, object data)
			{
				IManageGrowingStates manageGrowingStates = (IManageGrowingStates)data;
				if (manageGrowingStates.GetCropComponent() != null)
				{
					float seconds = manageGrowingStates.TimeUntilNextHarvest();
					str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(seconds, "F1", false));
				}
				float val = 100f * manageGrowingStates.PercentGrown();
				str = str.Replace("{PercentGrow}", Math.Floor((double)Math.Max(val, 0f)).ToString("F0"));
				return str;
			};
			this.GrowingFruit = new StatusItem("GrowingFruit", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026, null);
			this.GrowingFruit.resolveStringCallback = delegate(string str, object data)
			{
				IManageGrowingStates manageGrowingStates = (IManageGrowingStates)data;
				if (manageGrowingStates.GetCropComponent() != null)
				{
					float seconds = manageGrowingStates.TimeUntilNextHarvest();
					str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(seconds, "F1", false));
				}
				float val = 100f * manageGrowingStates.PercentGrown();
				str = str.Replace("{PercentGrow}", Math.Floor((double)Math.Max(val, 0f)).ToString("F0"));
				return str;
			};
			this.CarnivorousPlantAwaitingVictim = new StatusItem("CarnivorousPlantAwaitingVictim", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 1026, null);
			this.CarnivorousPlantAwaitingVictim.resolveTooltipCallback = delegate(string str, object data)
			{
				string[] formattedPossiblePreyList = ((IPlantConsumeEntities)data).GetFormattedPossiblePreyList();
				string text = "";
				for (int i = 0; i < formattedPossiblePreyList.Length; i++)
				{
					text = text + "\n" + GameUtil.SafeStringFormat(CREATURES.STATUSITEMS.CARNIVOROUSPLANTAWAITINGVICTIM.TOOLTIP_ITEM, new object[]
					{
						formattedPossiblePreyList[i]
					});
				}
				str += text;
				return str;
			};
			this.EnvironmentTooWarm = new StatusItem("EnvironmentTooWarm", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.EnvironmentTooWarm.resolveStringCallback = delegate(string str, object data)
			{
				float temp = Grid.Temperature[Grid.PosToCell(((TemperatureVulnerable)data).gameObject)];
				float temp2 = ((TemperatureVulnerable)data).TemperatureLethalHigh - 1f;
				str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.EnvironmentTooCold = new StatusItem("EnvironmentTooCold", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.EnvironmentTooCold.resolveStringCallback = delegate(string str, object data)
			{
				float temp = Grid.Temperature[Grid.PosToCell(((TemperatureVulnerable)data).gameObject)];
				float temp2 = ((TemperatureVulnerable)data).TemperatureLethalLow + 1f;
				str = str.Replace("{ExternalTemperature}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				str = str.Replace("{TargetTemperature}", GameUtil.GetFormattedTemperature(temp2, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.Entombed = new StatusItem("Entombed", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Entombed.resolveStringCallback = ((string str, object go) => str);
			this.Entombed.resolveTooltipCallback = delegate(string str, object go)
			{
				GameObject go2 = go as GameObject;
				return string.Format(str, GameUtil.GetIdentityDescriptor(go2, GameUtil.IdentityDescriptorTense.Normal));
			};
			this.Wilting = new StatusItem("Wilting", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026, null);
			this.Wilting.resolveStringCallback = delegate(string str, object data)
			{
				Growing growing = data as Growing;
				if (growing != null && data != null)
				{
					AmountInstance amountInstance = growing.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
					str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(amountInstance.GetMax(), growing.TimeUntilNextHarvest()), "F1", false));
				}
				str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
				return str;
			};
			this.WiltingDomestic = new StatusItem("WiltingDomestic", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026, null);
			this.WiltingDomestic.resolveStringCallback = delegate(string str, object data)
			{
				Growing growing = data as Growing;
				if (growing != null && data != null)
				{
					AmountInstance amountInstance = growing.gameObject.GetAmounts().Get(Db.Get().Amounts.Maturity);
					str = str.Replace("{TimeUntilNextHarvest}", GameUtil.GetFormattedCycles(Mathf.Min(amountInstance.GetMax(), growing.TimeUntilNextHarvest()), "F1", false));
				}
				str = str.Replace("{Reasons}", (data as KMonoBehaviour).GetComponent<WiltCondition>().WiltCausesString());
				return str;
			};
			this.WiltingNonGrowing = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 1026, null);
			this.WiltingNonGrowing.resolveStringCallback = delegate(string str, object data)
			{
				str = CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
				str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
				return str;
			};
			this.WiltingNonGrowingDomestic = new StatusItem("WiltingNonGrowing", "CREATURES", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 1026, null);
			this.WiltingNonGrowingDomestic.resolveStringCallback = delegate(string str, object data)
			{
				str = CREATURES.STATUSITEMS.WILTING_NON_GROWING_PLANT.NAME;
				str = str.Replace("{Reasons}", (data as WiltCondition).WiltCausesString());
				return str;
			};
			this.WrongAtmosphere = new StatusItem("WrongAtmosphere", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.WrongAtmosphere.resolveStringCallback = delegate(string str, object data)
			{
				string text = "";
				foreach (Element element in (data as PressureVulnerable).safe_atmospheres)
				{
					text = text + "\n    •  " + element.name;
				}
				str = str.Replace("{elements}", text);
				return str;
			};
			this.AtmosphericPressureTooLow = new StatusItem("AtmosphericPressureTooLow", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.AtmosphericPressureTooLow.resolveStringCallback = delegate(string str, object data)
			{
				PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
				str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.AtmosphericPressureTooHigh = new StatusItem("AtmosphericPressureTooHigh", "CREATURES", "status_item_plant_atmosphere", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.AtmosphericPressureTooHigh.resolveStringCallback = delegate(string str, object data)
			{
				PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
				str = str.Replace("{low_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				str = str.Replace("{high_mass}", GameUtil.GetFormattedMass(pressureVulnerable.pressureWarning_High, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.HealthStatus = new StatusItem("HealthStatus", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.HealthStatus.resolveStringCallback = delegate(string str, object data)
			{
				string newValue = "";
				switch ((Health.HealthState)data)
				{
				case Health.HealthState.Perfect:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.NAME;
					break;
				case Health.HealthState.Scuffed:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.NAME;
					break;
				case Health.HealthState.Injured:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.INJURED.NAME;
					break;
				case Health.HealthState.Critical:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.NAME;
					break;
				case Health.HealthState.Incapacitated:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.NAME;
					break;
				case Health.HealthState.Dead:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.DEAD.NAME;
					break;
				}
				str = str.Replace("{healthState}", newValue);
				return str;
			};
			this.HealthStatus.resolveTooltipCallback = delegate(string str, object data)
			{
				string newValue = "";
				switch ((Health.HealthState)data)
				{
				case Health.HealthState.Perfect:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.PERFECT.TOOLTIP;
					break;
				case Health.HealthState.Scuffed:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.SCUFFED.TOOLTIP;
					break;
				case Health.HealthState.Injured:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.INJURED.TOOLTIP;
					break;
				case Health.HealthState.Critical:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.CRITICAL.TOOLTIP;
					break;
				case Health.HealthState.Incapacitated:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.INCAPACITATED.TOOLTIP;
					break;
				case Health.HealthState.Dead:
					newValue = MISC.STATUSITEMS.HEALTHSTATUS.DEAD.TOOLTIP;
					break;
				}
				str = str.Replace("{healthState}", newValue);
				return str;
			};
			this.Barren = new StatusItem("Barren", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.NeedsFertilizer = new StatusItem("NeedsFertilizer", "CREATURES", "status_item_plant_solid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			Func<string, object, string> resolveStringCallback = (string str, object data) => str;
			this.NeedsFertilizer.resolveStringCallback = resolveStringCallback;
			this.NeedsIrrigation = new StatusItem("NeedsIrrigation", "CREATURES", "status_item_plant_liquid", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			Func<string, object, string> resolveStringCallback2 = (string str, object data) => str;
			this.NeedsIrrigation.resolveStringCallback = resolveStringCallback2;
			this.WrongFertilizer = new StatusItem("WrongFertilizer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Func<string, object, string> resolveStringCallback3 = (string str, object data) => str;
			this.WrongFertilizer.resolveStringCallback = resolveStringCallback3;
			this.WrongFertilizerMajor = new StatusItem("WrongFertilizerMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.WrongFertilizerMajor.resolveStringCallback = resolveStringCallback3;
			this.WrongIrrigation = new StatusItem("WrongIrrigation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Func<string, object, string> resolveStringCallback4 = (string str, object data) => str;
			this.WrongIrrigation.resolveStringCallback = resolveStringCallback4;
			this.WrongIrrigationMajor = new StatusItem("WrongIrrigationMajor", "CREATURES", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.WrongIrrigationMajor.resolveStringCallback = resolveStringCallback4;
			this.CantAcceptFertilizer = new StatusItem("CantAcceptFertilizer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Rotting = new StatusItem("Rotting", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Rotting.resolveStringCallback = ((string str, object data) => str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(277.15f, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)));
			this.Fresh = new StatusItem("Fresh", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Fresh.resolveStringCallback = delegate(string str, object data)
			{
				Rottable.Instance instance = (Rottable.Instance)data;
				return str.Replace("{RotPercentage}", "(" + Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f) + "%)");
			};
			this.Fresh.resolveTooltipCallback = delegate(string str, object data)
			{
				Rottable.Instance instance = (Rottable.Instance)data;
				return str.Replace("{RotTooltip}", instance.GetToolTip());
			};
			this.Stale = new StatusItem("Stale", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Stale.resolveStringCallback = delegate(string str, object data)
			{
				Rottable.Instance instance = (Rottable.Instance)data;
				return str.Replace("{RotPercentage}", "(" + Util.FormatWholeNumber(instance.RotConstitutionPercentage * 100f) + "%)");
			};
			this.Stale.resolveTooltipCallback = delegate(string str, object data)
			{
				Rottable.Instance instance = (Rottable.Instance)data;
				return str.Replace("{RotTooltip}", instance.GetToolTip());
			};
			this.Spoiled = new StatusItem("Spoiled", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			Func<string, object, string> resolveStringCallback5 = delegate(string str, object data)
			{
				IRottable rottable = (IRottable)data;
				return str.Replace("{RotTemperature}", GameUtil.GetFormattedTemperature(rottable.RotTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)).Replace("{PreserveTemperature}", GameUtil.GetFormattedTemperature(rottable.PreserveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.Refrigerated = new StatusItem("Refrigerated", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Refrigerated.resolveStringCallback = resolveStringCallback5;
			this.RefrigeratedFrozen = new StatusItem("RefrigeratedFrozen", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.RefrigeratedFrozen.resolveStringCallback = resolveStringCallback5;
			this.Unrefrigerated = new StatusItem("Unrefrigerated", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Unrefrigerated.resolveStringCallback = resolveStringCallback5;
			this.SterilizingAtmosphere = new StatusItem("SterilizingAtmosphere", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.ContaminatedAtmosphere = new StatusItem("ContaminatedAtmosphere", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Old = new StatusItem("Old", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Old.resolveTooltipCallback = delegate(string str, object data)
			{
				AgeMonitor.Instance instance = (AgeMonitor.Instance)data;
				return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(instance.CyclesUntilDeath * 600f, "F1", false));
			};
			this.ExchangingElementConsume = new StatusItem("ExchangingElementConsume", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.ExchangingElementConsume.resolveStringCallback = delegate(string str, object data)
			{
				EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance)data;
				str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
				str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ExchangingElementConsume.resolveTooltipCallback = delegate(string str, object data)
			{
				EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance)data;
				str = str.Replace("{ConsumeElement}", ElementLoader.FindElementByHash(statesInstance.master.consumedElement).tag.ProperName());
				str = str.Replace("{ConsumeRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ExchangingElementOutput = new StatusItem("ExchangingElementOutput", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.ExchangingElementOutput.resolveStringCallback = delegate(string str, object data)
			{
				EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance)data;
				str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
				str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ExchangingElementOutput.resolveTooltipCallback = delegate(string str, object data)
			{
				EntityElementExchanger.StatesInstance statesInstance = (EntityElementExchanger.StatesInstance)data;
				str = str.Replace("{OutputElement}", ElementLoader.FindElementByHash(statesInstance.master.emittedElement).tag.ProperName());
				str = str.Replace("{OutputRate}", GameUtil.GetFormattedMass(statesInstance.master.consumeRate * statesInstance.master.exchangeRatio, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.Hungry = new StatusItem("Hungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Hungry.resolveTooltipCallback = delegate(string str, object data)
			{
				Diet diet = ((CreatureCalorieMonitor.Instance)data).stomach.diet;
				if (diet.consumedTags.Count > 0)
				{
					string[] array = (from t in diet.consumedTags
					select t.Key.ProperName()).ToArray<string>();
					if (array.Length > 3)
					{
						array = new string[]
						{
							array[0],
							array[1],
							array[2],
							"..."
						};
					}
					string newValue = string.Join(", ", array);
					return str + "\n" + UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue);
				}
				return str;
			};
			this.HiveHungry = new StatusItem("HiveHungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.HiveHungry.resolveTooltipCallback = delegate(string str, object data)
			{
				Diet diet = ((BeehiveCalorieMonitor.Instance)data).stomach.diet;
				if (diet.consumedTags.Count > 0)
				{
					string[] array = (from t in diet.consumedTags
					select t.Key.ProperName()).ToArray<string>();
					if (array.Length > 3)
					{
						array = new string[]
						{
							array[0],
							array[1],
							array[2],
							"..."
						};
					}
					string newValue = string.Join(", ", array);
					return str + "\n" + UI.BUILDINGEFFECTS.DIET_STORED.text.Replace("{Foodlist}", newValue);
				}
				return str;
			};
			this.NoSleepSpot = new StatusItem("NoSleepSpot", "CREATURES", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.OriginalPlantMutation = new StatusItem("OriginalPlantMutation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.UnknownMutation = new StatusItem("UnknownMutation", "CREATURES", "status_item_unknown_mutation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.SpecificPlantMutation = new StatusItem("SpecificPlantMutation", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.SpecificPlantMutation.resolveStringCallback = delegate(string str, object data)
			{
				PlantMutation plantMutation = (PlantMutation)data;
				return str.Replace("{MutationName}", plantMutation.Name);
			};
			this.SpecificPlantMutation.resolveTooltipCallback = delegate(string str, object data)
			{
				PlantMutation plantMutation = (PlantMutation)data;
				str = str.Replace("{MutationName}", plantMutation.Name);
				return str + "\n" + plantMutation.GetTooltip();
			};
			this.Crop_Too_NonRadiated = new StatusItem("Crop_Too_NonRadiated", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.Crop_Too_Radiated = new StatusItem("Crop_Too_Radiated", "CREATURES", "status_item_plant_light", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.ElementGrowthGrowing = new StatusItem("Element_Growth_Growing", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.ElementGrowthGrowing.resolveTooltipCallback = delegate(string str, object data)
			{
				ElementGrowthMonitor.Instance instance = (ElementGrowthMonitor.Instance)data;
				StringBuilder stringBuilder = new StringBuilder(str, str.Length * 2);
				stringBuilder.Replace("{templo}", GameUtil.GetFormattedTemperature(instance.def.minTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				stringBuilder.Replace("{temphi}", GameUtil.GetFormattedTemperature(instance.def.maxTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				if (instance.lastConsumedTemperature > 0f)
				{
					stringBuilder.Append("\n\n");
					stringBuilder.Append(CREATURES.STATUSITEMS.ELEMENT_GROWTH_GROWING.PREFERRED_TEMP);
					stringBuilder.Replace("{element}", ElementLoader.FindElementByHash(instance.lastConsumedElement).name);
					stringBuilder.Replace("{temperature}", GameUtil.GetFormattedTemperature(instance.lastConsumedTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				}
				return stringBuilder.ToString();
			};
			this.ElementGrowthStunted = new StatusItem("Element_Growth_Stunted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.ElementGrowthStunted.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
			this.ElementGrowthStunted.resolveStringCallback = delegate(string str, object data)
			{
				ElementGrowthMonitor.Instance instance = (ElementGrowthMonitor.Instance)data;
				string newValue = (instance.lastConsumedTemperature < instance.def.minTemperature) ? CREATURES.STATUSITEMS.ELEMENT_GROWTH_STUNTED.TOO_COLD : CREATURES.STATUSITEMS.ELEMENT_GROWTH_STUNTED.TOO_HOT;
				str = str.Replace("{reason}", newValue);
				return str;
			};
			this.ElementGrowthHalted = new StatusItem("Element_Growth_Halted", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.ElementGrowthHalted.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
			this.ElementGrowthComplete = new StatusItem("Element_Growth_Complete", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.ElementGrowthComplete.resolveTooltipCallback = this.ElementGrowthGrowing.resolveTooltipCallback;
			this.LookingForFood = new StatusItem("Hungry", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.LookingForGas = new StatusItem("LookingForGas", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.LookingForLiquid = new StatusItem("LookingForLiquid", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Beckoning = new StatusItem("Beckoning", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.BeckoningBlocked = new StatusItem("BeckoningBlocked", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022, null);
			this.MilkProducer = new StatusItem("MilkProducer", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.MilkProducer.resolveStringCallback = delegate(string str, object data)
			{
				MilkProductionMonitor.Instance instance = (MilkProductionMonitor.Instance)data;
				str = str.Replace("{amount}", GameUtil.GetFormattedMass(instance.MilkPercentage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.GettingRanched = new StatusItem("Getting_Ranched", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.GettingMilked = new StatusItem("Getting_Milked", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.MilkFull = new StatusItem("MilkFull", "CREATURES", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.TemperatureHotUncomfortable = new StatusItem("TemperatureHotUncomfortable", CREATURES.STATUSITEMS.TEMPERATURE_HOT_UNCOMFORTABLE.NAME, CREATURES.STATUSITEMS.TEMPERATURE_HOT_UNCOMFORTABLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.TemperatureHotDeadly = new StatusItem("TemperatureHotDeadly", CREATURES.STATUSITEMS.TEMPERATURE_HOT_DEADLY.NAME, CREATURES.STATUSITEMS.TEMPERATURE_HOT_DEADLY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.TemperatureColdUncomfortable = new StatusItem("TemperatureColdUncomfortable", CREATURES.STATUSITEMS.TEMPERATURE_COLD_UNCOMFORTABLE.NAME, CREATURES.STATUSITEMS.TEMPERATURE_COLD_UNCOMFORTABLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
			this.TemperatureColdDeadly = new StatusItem("TemperatureColdDeadly", CREATURES.STATUSITEMS.TEMPERATURE_COLD_DEADLY.NAME, CREATURES.STATUSITEMS.TEMPERATURE_COLD_DEADLY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.TemperatureHotUncomfortable.resolveStringCallback = delegate(string str, object obj)
			{
				CritterTemperatureMonitor.Instance instance = (CritterTemperatureMonitor.Instance)obj;
				return string.Format(str, new object[]
				{
					GameUtil.GetFormattedTemperature(instance.GetTemperatureInternal(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					GameUtil.GetFormattedTemperature(instance.def.temperatureColdUncomfortable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					GameUtil.GetFormattedTemperature(instance.def.temperatureHotUncomfortable, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					Effect.CreateTooltip(instance.sm.uncomfortableEffect, false, "\n    • ", true)
				});
			};
			this.TemperatureHotDeadly.resolveStringCallback = delegate(string str, object obj)
			{
				CritterTemperatureMonitor.Instance instance = (CritterTemperatureMonitor.Instance)obj;
				return string.Format(str, new object[]
				{
					GameUtil.GetFormattedTemperature(instance.GetTemperatureExternal(), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					GameUtil.GetFormattedTemperature(instance.def.temperatureColdDeadly, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					GameUtil.GetFormattedTemperature(instance.def.temperatureHotDeadly, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false),
					Effect.CreateTooltip(instance.sm.deadlyEffect, false, "\n    • ", true)
				});
			};
			this.TemperatureColdUncomfortable.resolveStringCallback = this.TemperatureHotUncomfortable.resolveStringCallback;
			this.TemperatureColdDeadly.resolveStringCallback = this.TemperatureHotDeadly.resolveStringCallback;
			this.TravelingToPollinate = new StatusItem("POLLINATING.MOVINGTO", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.Pollinating = new StatusItem("POLLINATING.INTERACTING", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.NotPollinated = new StatusItem("NOT_POLLINATED", "CREATURES", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}

		public StatusItem Dead;

		public StatusItem HealthStatus;

		public StatusItem Hot;

		public StatusItem Hot_Crop;

		public StatusItem Scalding;

		public StatusItem Scolding;

		public StatusItem Cold;

		public StatusItem Cold_Crop;

		public StatusItem Crop_Too_Dark;

		public StatusItem Crop_Too_Bright;

		public StatusItem Crop_Blighted;

		public StatusItem Hypothermia;

		public StatusItem Hyperthermia;

		public StatusItem Suffocating;

		public StatusItem AquaticCreatureSuffocating;

		public StatusItem Hatching;

		public StatusItem Incubating;

		public StatusItem Drowning;

		public StatusItem Saturated;

		public StatusItem DryingOut;

		public StatusItem Growing;

		public StatusItem GrowingFruit;

		public StatusItem CarnivorousPlantAwaitingVictim;

		public StatusItem ReadyForHarvest;

		public StatusItem ReadyForHarvest_Branch;

		public StatusItem EnvironmentTooWarm;

		public StatusItem EnvironmentTooCold;

		public StatusItem Entombed;

		public StatusItem Wilting;

		public StatusItem WiltingDomestic;

		public StatusItem WiltingNonGrowing;

		public StatusItem WiltingNonGrowingDomestic;

		public StatusItem WrongAtmosphere;

		public StatusItem AtmosphericPressureTooLow;

		public StatusItem AtmosphericPressureTooHigh;

		public StatusItem Barren;

		public StatusItem NeedsFertilizer;

		public StatusItem NeedsIrrigation;

		public StatusItem WrongTemperature;

		public StatusItem WrongFertilizer;

		public StatusItem WrongIrrigation;

		public StatusItem WrongFertilizerMajor;

		public StatusItem WrongIrrigationMajor;

		public StatusItem CantAcceptFertilizer;

		public StatusItem CantAcceptIrrigation;

		public StatusItem Rotting;

		public StatusItem Fresh;

		public StatusItem Stale;

		public StatusItem Spoiled;

		public StatusItem Refrigerated;

		public StatusItem RefrigeratedFrozen;

		public StatusItem Unrefrigerated;

		public StatusItem SterilizingAtmosphere;

		public StatusItem ContaminatedAtmosphere;

		public StatusItem Old;

		public StatusItem ExchangingElementOutput;

		public StatusItem ExchangingElementConsume;

		public StatusItem Hungry;

		public StatusItem HiveHungry;

		public StatusItem NoSleepSpot;

		public StatusItem ProducingSugarWater;

		public StatusItem SugarWaterProductionPaused;

		public StatusItem SugarWaterProductionWilted;

		public StatusItem SpaceTreeBranchLightStatus;

		public StatusItem OriginalPlantMutation;

		public StatusItem UnknownMutation;

		public StatusItem SpecificPlantMutation;

		public StatusItem Crop_Too_NonRadiated;

		public StatusItem Crop_Too_Radiated;

		public StatusItem ElementGrowthGrowing;

		public StatusItem ElementGrowthStunted;

		public StatusItem ElementGrowthHalted;

		public StatusItem ElementGrowthComplete;

		public StatusItem LookingForFood;

		public StatusItem LookingForGas;

		public StatusItem LookingForLiquid;

		public StatusItem Beckoning;

		public StatusItem BeckoningBlocked;

		public StatusItem MilkProducer;

		public StatusItem MilkFull;

		public StatusItem GettingRanched;

		public StatusItem GettingMilked;

		public StatusItem TemperatureHotUncomfortable;

		public StatusItem TemperatureHotDeadly;

		public StatusItem TemperatureColdUncomfortable;

		public StatusItem TemperatureColdDeadly;

		public StatusItem TravelingToPollinate;

		public StatusItem Pollinating;

		public StatusItem NotPollinated;
	}
}
