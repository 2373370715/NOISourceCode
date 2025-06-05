using System;
using Klei.AI;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x020021A1 RID: 8609
	public class DuplicantStatusItems : StatusItems
	{
		// Token: 0x0600B7BD RID: 47037 RVA: 0x0011B370 File Offset: 0x00119570
		public DuplicantStatusItems(ResourceSet parent) : base("DuplicantStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x0600B7BE RID: 47038 RVA: 0x00459104 File Offset: 0x00457304
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x0600B7BF RID: 47039 RVA: 0x0045912C File Offset: 0x0045732C
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 2)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x0600B7C0 RID: 47040 RVA: 0x00466DB0 File Offset: 0x00464FB0
		private void CreateStatusItems()
		{
			Func<string, object, string> resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.GetComponent<KSelectable>() != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
				}
				return str;
			};
			Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null)
				{
					str = str.Replace("{Target}", workable.GetComponent<KSelectable>().GetName());
					ComplexFabricatorWorkable complexFabricatorWorkable = workable as ComplexFabricatorWorkable;
					if (complexFabricatorWorkable != null)
					{
						ComplexRecipe currentWorkingOrder = complexFabricatorWorkable.CurrentWorkingOrder;
						if (currentWorkingOrder != null)
						{
							str = str.Replace("{Item}", currentWorkingOrder.FirstResult.ProperName());
						}
					}
				}
				return str;
			};
			this.BedUnreachable = this.CreateStatusItem("BedUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.BedUnreachable.AddNotification(null, null, null);
			this.DailyRationLimitReached = this.CreateStatusItem("DailyRationLimitReached", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.DailyRationLimitReached.AddNotification(null, null, null);
			this.HoldingBreath = this.CreateStatusItem("HoldingBreath", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hungry = this.CreateStatusItem("Hungry", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Slippering = this.CreateStatusItem("Slippering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy = this.CreateStatusItem("Unhappy", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Unhappy.AddNotification(null, null, null);
			this.NervousBreakdown = this.CreateStatusItem("NervousBreakdown", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.NervousBreakdown.AddNotification(null, null, null);
			this.NoRationsAvailable = this.CreateStatusItem("NoRationsAvailable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.PendingPacification = this.CreateStatusItem("PendingPacification", "DUPLICANTS", "status_item_pending_pacification", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned = this.CreateStatusItem("QuarantineAreaUnassigned", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnassigned.AddNotification(null, null, null);
			this.QuarantineAreaUnreachable = this.CreateStatusItem("QuarantineAreaUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.QuarantineAreaUnreachable.AddNotification(null, null, null);
			this.Quarantined = this.CreateStatusItem("Quarantined", "DUPLICANTS", "status_item_quarantined", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable = this.CreateStatusItem("RationsUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsUnreachable.AddNotification(null, null, null);
			this.RationsNotPermitted = this.CreateStatusItem("RationsNotPermitted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.RationsNotPermitted.AddNotification(null, null, null);
			this.Rotten = this.CreateStatusItem("Rotten", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Starving = this.CreateStatusItem("Starving", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Starving.AddNotification(null, null, null);
			this.Suffocating = this.CreateStatusItem("Suffocating", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Suffocating.AddNotification(null, null, null);
			this.Tired = this.CreateStatusItem("Tired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Idle = this.CreateStatusItem("Idle", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.IdleInRockets = this.CreateStatusItem("IdleInRockets", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Pacified = this.CreateStatusItem("Pacified", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dead = this.CreateStatusItem("Dead", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Dead.resolveStringCallback = delegate(string str, object data)
			{
				Death death = (Death)data;
				return str.Replace("{Death}", death.Name);
			};
			this.MoveToSuitNotRequired = this.CreateStatusItem("MoveToSuitNotRequired", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DroppingUnusedInventory = this.CreateStatusItem("DroppingUnusedInventory", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.MovingToSafeArea = this.CreateStatusItem("MovingToSafeArea", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable = this.CreateStatusItem("ToiletUnreachable", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.ToiletUnreachable.AddNotification(null, null, null);
			this.NoUsableToilets = this.CreateStatusItem("NoUsableToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoUsableToilets.AddNotification(null, null, null);
			this.NoToilets = this.CreateStatusItem("NoToilets", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.NoToilets.AddNotification(null, null, null);
			this.BreathingO2 = this.CreateStatusItem("BreathingO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.BreathingO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				float num = (oxygenBreather.O2Accumulator == HandleVector<int>.InvalidHandle) ? 0f : Game.Instance.accumulators.GetAverageRate(oxygenBreather.O2Accumulator);
				return str.Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.BreathingO2Bionic = this.CreateStatusItem("BreathingO2Bionic", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.BreathingO2Bionic.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				float num = (oxygenBreather.O2Accumulator == HandleVector<int>.InvalidHandle) ? 0f : Game.Instance.accumulators.GetAverageRate(oxygenBreather.O2Accumulator);
				return str.Replace("{ConsumptionRate}", GameUtil.GetFormattedMass(-num, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.EmittingCO2 = this.CreateStatusItem("EmittingCO2", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 130);
			this.EmittingCO2.resolveStringCallback = delegate(string str, object data)
			{
				OxygenBreather oxygenBreather = (OxygenBreather)data;
				return str.Replace("{EmittingRate}", GameUtil.GetFormattedMass(oxygenBreather.CO2EmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.Vomiting = this.CreateStatusItem("Vomiting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Coughing = this.CreateStatusItem("Coughing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen = this.CreateStatusItem("LowOxygen", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowOxygen.AddNotification(null, null, null);
			this.RedAlert = this.CreateStatusItem("RedAlert", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Dreaming = this.CreateStatusItem("Dreaming", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping = this.CreateStatusItem("Sleeping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				if (data is SleepChore.StatesInstance)
				{
					string stateChangeNoiseSource = ((SleepChore.StatesInstance)data).stateChangeNoiseSource;
					if (!string.IsNullOrEmpty(stateChangeNoiseSource))
					{
						string text = DUPLICANTS.STATUSITEMS.SLEEPING.TOOLTIP;
						text = text.Replace("{Disturber}", stateChangeNoiseSource);
						str += text;
					}
				}
				return str;
			};
			this.SleepingExhausted = this.CreateStatusItem("SleepingExhausted", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByNoise = this.CreateStatusItem("SleepingInterruptedByNoise", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByLight = this.CreateStatusItem("SleepingInterruptedByLight", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByFearOfDark = this.CreateStatusItem("SleepingInterruptedByFearOfDark", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByMovement = this.CreateStatusItem("SleepingInterruptedByMovement", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.SleepingInterruptedByCold = this.CreateStatusItem("SleepingInterruptedByCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating = this.CreateStatusItem("Eating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Eating.resolveStringCallback = resolveStringCallback;
			this.Digging = this.CreateStatusItem("Digging", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning = this.CreateStatusItem("Cleaning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cleaning.resolveStringCallback = resolveStringCallback;
			this.PickingUp = this.CreateStatusItem("PickingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PickingUp.resolveStringCallback = resolveStringCallback;
			this.Mopping = this.CreateStatusItem("Mopping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking = this.CreateStatusItem("Cooking", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cooking.resolveStringCallback = resolveStringCallback2;
			this.Mushing = this.CreateStatusItem("Mushing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Mushing.resolveStringCallback = resolveStringCallback2;
			this.Researching = this.CreateStatusItem("Researching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Researching.resolveStringCallback = delegate(string str, object data)
			{
				TechInstance activeResearch = Research.Instance.GetActiveResearch();
				if (activeResearch != null)
				{
					return str.Replace("{Tech}", activeResearch.tech.Name);
				}
				return str;
			};
			this.ResearchingFromPOI = this.CreateStatusItem("ResearchingFromPOI", DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.NAME, DUPLICANTS.STATUSITEMS.RESEARCHING_FROM_POI.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.MissionControlling = this.CreateStatusItem("MissionControlling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering = this.CreateStatusItem("Tinkering", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Tinkering.resolveStringCallback = delegate(string str, object data)
			{
				Tinkerable tinkerable = (Tinkerable)data;
				if (tinkerable != null)
				{
					return string.Format(str, tinkerable.tinkerMaterialTag.ProperName());
				}
				return str;
			};
			this.Storing = this.CreateStatusItem("Storing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Storing.resolveStringCallback = delegate(string str, object data)
			{
				Workable workable = (Workable)data;
				if (workable != null && workable.worker as StandardWorker != null)
				{
					KSelectable component = workable.GetComponent<KSelectable>();
					if (component)
					{
						str = str.Replace("{Target}", component.GetName());
					}
					Pickupable pickupable = (workable.worker as StandardWorker).workCompleteData as Pickupable;
					if (workable.worker != null && pickupable)
					{
						KSelectable component2 = pickupable.GetComponent<KSelectable>();
						if (component2)
						{
							str = str.Replace("{Item}", component2.GetName());
						}
					}
				}
				return str;
			};
			this.Building = this.CreateStatusItem("Building", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Building.resolveStringCallback = resolveStringCallback;
			this.Equipping = this.CreateStatusItem("Equipping", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Equipping.resolveStringCallback = resolveStringCallback;
			this.WarmingUp = this.CreateStatusItem("WarmingUp", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WarmingUp.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.Harvesting = this.CreateStatusItem("Harvesting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Ranching = this.CreateStatusItem("Ranching", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Harvesting.resolveStringCallback = resolveStringCallback;
			this.Uprooting = this.CreateStatusItem("Uprooting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Uprooting.resolveStringCallback = resolveStringCallback;
			this.Emptying = this.CreateStatusItem("Emptying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Emptying.resolveStringCallback = resolveStringCallback;
			this.Toggling = this.CreateStatusItem("Toggling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Toggling.resolveStringCallback = resolveStringCallback;
			this.Deconstructing = this.CreateStatusItem("Deconstructing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Deconstructing.resolveStringCallback = resolveStringCallback;
			this.Disinfecting = this.CreateStatusItem("Disinfecting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Disinfecting.resolveStringCallback = resolveStringCallback;
			this.Upgrading = this.CreateStatusItem("Upgrading", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Upgrading.resolveStringCallback = resolveStringCallback;
			this.Fabricating = this.CreateStatusItem("Fabricating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Fabricating.resolveStringCallback = resolveStringCallback2;
			this.Processing = this.CreateStatusItem("Processing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Processing.resolveStringCallback = resolveStringCallback2;
			this.Spicing = this.CreateStatusItem("Spicing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing = this.CreateStatusItem("Clearing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Clearing.resolveStringCallback = resolveStringCallback;
			this.GeneratingPower = this.CreateStatusItem("GeneratingPower", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GeneratingPower.resolveStringCallback = resolveStringCallback;
			this.CloggingToilet = this.CreateStatusItem("CLOGGINGTOILET", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Cold = this.CreateStatusItem("Cold", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Cold.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((ColdImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingCold = this.CreateStatusItem("ExitingCold", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingCold.resolveTooltipCallback = delegate(string str, object data)
			{
				ColdImmunityMonitor.Instance instance = (ColdImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.ColdCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("ColdAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("ColdAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.Hot = this.CreateStatusItem("Hot", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.Hot.resolveTooltipCallback = delegate(string str, object data)
			{
				ExternalTemperatureMonitor.Instance smi = ((HeatImmunityMonitor.Instance)data).GetSMI<ExternalTemperatureMonitor.Instance>();
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				float dtu_s = smi.temperatureTransferer.average_kilowatts_exchanged.GetUnweightedAverage * 1000f;
				str = str.Replace("{currentTransferWattage}", GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic));
				AttributeInstance attributeInstance = smi.attributes.Get("ThermalConductivityBarrier");
				string text = "<b>" + attributeInstance.GetFormattedValue() + "</b>";
				for (int num = 0; num != attributeInstance.Modifiers.Count; num++)
				{
					AttributeModifier attributeModifier = attributeInstance.Modifiers[num];
					text += "\n";
					text = string.Concat(new string[]
					{
						text,
						"    • ",
						attributeModifier.GetDescription(),
						" <b>",
						attributeModifier.GetFormattedString(),
						"</b>"
					});
				}
				str = str.Replace("{conductivityBarrier}", text);
				return str;
			};
			this.ExitingHot = this.CreateStatusItem("ExitingHot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.ExitingHot.resolveTooltipCallback = delegate(string str, object data)
			{
				HeatImmunityMonitor.Instance instance = (HeatImmunityMonitor.Instance)data;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(instance.HeatCountdown, "F0"));
				str = str.Replace("{StressModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[0].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{StaminaModification}", GameUtil.GetFormattedPercent(Db.Get().effects.Get("WarmAir").SelfModifiers[1].Value, GameUtil.TimeSlice.PerCycle));
				str = str.Replace("{AthleticsModification}", Db.Get().effects.Get("WarmAir").SelfModifiers[2].Value.ToString());
				return str;
			};
			this.BodyRegulatingHeating = this.CreateStatusItem("BodyRegulatingHeating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingHeating.resolveStringCallback = delegate(string str, object data)
			{
				WarmBlooded.StatesInstance statesInstance = (WarmBlooded.StatesInstance)data;
				return str.Replace("{TempDelta}", GameUtil.GetFormattedTemperature(statesInstance.TemperatureDelta, GameUtil.TimeSlice.PerSecond, GameUtil.TemperatureInterpretation.Relative, true, false));
			};
			this.BodyRegulatingCooling = this.CreateStatusItem("BodyRegulatingCooling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BodyRegulatingCooling.resolveStringCallback = this.BodyRegulatingHeating.resolveStringCallback;
			this.EntombedChore = this.CreateStatusItem("EntombedChore", "DUPLICANTS", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.EntombedChore.AddNotification(null, null, null);
			this.EarlyMorning = this.CreateStatusItem("EarlyMorning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.NightTime = this.CreateStatusItem("NightTime", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorDecor = this.CreateStatusItem("PoorDecor", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorQualityOfLife = this.CreateStatusItem("PoorQualityOfLife", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.PoorFoodQuality = this.CreateStatusItem("PoorFoodQuality", DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.POOR_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.GoodFoodQuality = this.CreateStatusItem("GoodFoodQuality", DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.NAME, DUPLICANTS.STATUSITEMS.GOOD_FOOD_QUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.Arting = this.CreateStatusItem("Arting", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Arting.resolveStringCallback = resolveStringCallback;
			this.SevereWounds = this.CreateStatusItem("SevereWounds", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.SevereWounds.AddNotification(null, null, null);
			this.BionicOfflineIncapacitated = this.CreateStatusItem("BionicOfflineIncapacitated", "DUPLICANTS", "status_electrobank", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.BionicOfflineIncapacitated.AddNotification(null, null, null);
			this.BionicMicrochipGeneration = this.CreateStatusItem("BionicMicrochipGeneration", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicMicrochipGeneration.resolveStringCallback = delegate(string str, object data)
			{
				float percent = ((BionicMicrochipMonitor.Instance)data).Progress * 100f;
				str = string.Format(str, GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
				return str;
			};
			this.BionicMicrochipGeneration.resolveTooltipCallback = delegate(string str, object data)
			{
				BionicMicrochipMonitor.Instance instance = (BionicMicrochipMonitor.Instance)data;
				float seconds = 150f;
				str = string.Format(str, GameUtil.GetFormattedTime(seconds, "F0"));
				return str;
			};
			this.BionicWantsOilChange = this.CreateStatusItem("BionicWantsOilChange", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.BionicWaitingForReboot = this.CreateStatusItem("BionicWaitingForReboot", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicBeingRebooted = this.CreateStatusItem("BionicBeingRebooted", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk = this.CreateStatusItem("BionicRequiresSkillPerk", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BionicRequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				str = str.Replace("{Boosters}", GameUtil.NamesOfBoostersWithSkillPerk((string)data));
				return str;
			};
			this.Incapacitated = this.CreateStatusItem("Incapacitated", "DUPLICANTS", "status_item_broken", StatusItem.IconType.Custom, NotificationType.DuplicantThreatening, false, OverlayModes.None.ID, true, 2);
			this.Incapacitated.AddNotification(null, null, null);
			this.Incapacitated.resolveStringCallback = delegate(string str, object data)
			{
				IncapacitationMonitor.Instance instance = (IncapacitationMonitor.Instance)data;
				float bleedLifeTime = instance.GetBleedLifeTime(instance);
				str = str.Replace("{CauseOfIncapacitation}", instance.GetCauseOfIncapacitation().Name);
				return str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedTime(bleedLifeTime, "F0"));
			};
			this.Relocating = this.CreateStatusItem("Relocating", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Relocating.resolveStringCallback = resolveStringCallback;
			this.Fighting = this.CreateStatusItem("Fighting", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fighting.AddNotification(null, null, null);
			this.Fleeing = this.CreateStatusItem("Fleeing", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.Fleeing.AddNotification(null, null, null);
			this.Stressed = this.CreateStatusItem("Stressed", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Stressed.AddNotification(null, null, null);
			this.LashingOut = this.CreateStatusItem("LashingOut", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.LashingOut.AddNotification(null, null, null);
			this.LowImmunity = this.CreateStatusItem("LowImmunity", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.LowImmunity.AddNotification(null, null, null);
			this.Studying = this.CreateStatusItem("Studying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.InstallingElectrobank = this.CreateStatusItem("InstallingElectrobank", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Socializing = this.CreateStatusItem("Socializing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.Mingling = this.CreateStatusItem("Mingling", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster = this.CreateStatusItem("BionicExplorerBooster", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 2);
			this.BionicExplorerBooster.resolveStringCallback = delegate(string str, object data)
			{
				BionicUpgrade_ExplorerBoosterMonitor.Instance instance = (BionicUpgrade_ExplorerBoosterMonitor.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.CurrentProgress * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ContactWithGerms = this.CreateStatusItem("ContactWithGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ContactWithGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				str = str.Replace("{Sickness}", name);
				return str;
			};
			this.ContactWithGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				GameUtil.FocusCamera(exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id), 2f, true, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.ExposedToGerms = this.CreateStatusItem("ExposedToGerms", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.Disease.ID, true, 2);
			this.ExposedToGerms.resolveStringCallback = delegate(string str, object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				string name = Db.Get().Sicknesses.Get(exposureStatusData.exposure_type.sickness_id).Name;
				AttributeInstance attributeInstance = Db.Get().Attributes.GermResistance.Lookup(exposureStatusData.owner.gameObject);
				string lastDiseaseSource = exposureStatusData.owner.GetLastDiseaseSource(exposureStatusData.exposure_type.germ_id);
				GermExposureMonitor.Instance smi = exposureStatusData.owner.GetSMI<GermExposureMonitor.Instance>();
				float num = (float)exposureStatusData.exposure_type.base_resistance + GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				float totalValue = attributeInstance.GetTotalValue();
				float resistanceToExposureType = smi.GetResistanceToExposureType(exposureStatusData.exposure_type, -1f);
				float contractionChance = GermExposureMonitor.GetContractionChance(resistanceToExposureType);
				float exposureTier = smi.GetExposureTier(exposureStatusData.exposure_type.germ_id);
				float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)exposureTier - 1] - GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[0];
				str = str.Replace("{Severity}", DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.EXPOSURE_TIERS[(int)exposureTier - 1].ToString());
				str = str.Replace("{Sickness}", name);
				str = str.Replace("{Source}", lastDiseaseSource);
				str = str.Replace("{Base}", GameUtil.GetFormattedSimple(num, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Dupe}", GameUtil.GetFormattedSimple(totalValue, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Total}", GameUtil.GetFormattedSimple(resistanceToExposureType, GameUtil.TimeSlice.None, null));
				str = str.Replace("{ExposureLevelBonus}", GameUtil.GetFormattedSimple(num2, GameUtil.TimeSlice.None, null));
				str = str.Replace("{Chance}", GameUtil.GetFormattedPercent(contractionChance * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.ExposedToGerms.statusItemClickCallback = delegate(object data)
			{
				GermExposureMonitor.ExposureStatusData exposureStatusData = (GermExposureMonitor.ExposureStatusData)data;
				GameUtil.FocusCamera(exposureStatusData.owner.GetLastExposurePosition(exposureStatusData.exposure_type.germ_id), 2f, true, true);
				if (OverlayScreen.Instance.mode == OverlayModes.None.ID)
				{
					OverlayScreen.Instance.ToggleOverlay(OverlayModes.Disease.ID, true);
				}
			};
			this.LightWorkEfficiencyBonus = this.CreateStatusItem("LightWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LightWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LIGHTWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS * 100f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.LaboratoryWorkEfficiencyBonus = this.CreateStatusItem("LaboratoryWorkEfficiencyBonus", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.LaboratoryWorkEfficiencyBonus.resolveStringCallback = delegate(string str, object data)
			{
				string arg = string.Format(DUPLICANTS.STATUSITEMS.LABORATORYWORKEFFICIENCYBONUS.NO_BUILDING_WORK_ATTRIBUTE, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None), true));
				return string.Format(str, arg);
			};
			this.BeingProductive = this.CreateStatusItem("BeingProductive", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistPlanning = this.CreateStatusItem("BalloonArtistPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.BalloonArtistHandingOut = this.CreateStatusItem("BalloonArtistHandingOut", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.Partying = this.CreateStatusItem("Partying", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.DataRainerPlanning = this.CreateStatusItem("DataRainerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.DataRainerRaining = this.CreateStatusItem("DataRainerRaining", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerPlanning = this.CreateStatusItem("RoboDancerPlanning", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.RoboDancerDancing = this.CreateStatusItem("RoboDancerDancing", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WatchRoboDancerWorkable = this.CreateStatusItem("WatchRoboDancerWorkable", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation = this.CreateStatusItem("GasLiquidIrritated", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 2);
			this.GasLiquidIrritation.resolveStringCallback = ((string str, object data) => ((GasLiquidExposureMonitor.Instance)data).IsMajorIrritation() ? DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MAJOR : DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.NAME_MINOR);
			this.GasLiquidIrritation.resolveTooltipCallback = delegate(string str, object data)
			{
				GasLiquidExposureMonitor.Instance instance = (GasLiquidExposureMonitor.Instance)data;
				string text = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP;
				string text2 = "";
				Effect appliedEffect = instance.sm.GetAppliedEffect(instance);
				if (appliedEffect != null)
				{
					text2 = Effect.CreateTooltip(appliedEffect, false, "\n    • ", true);
				}
				string text3 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSED.Replace("{element}", instance.CurrentlyExposedToElement().name);
				float currentExposure = instance.sm.GetCurrentExposure(instance);
				if (currentExposure < 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_DECREASE);
				}
				else if (currentExposure > 0f)
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_INCREASE);
				}
				else
				{
					text3 = text3.Replace("{rate}", DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_RATE_STAYS);
				}
				float seconds = (instance.exposure - instance.minorIrritationThreshold) / Math.Abs(instance.exposureRate);
				string text4 = DUPLICANTS.STATUSITEMS.GASLIQUIDEXPOSURE.TOOLTIP_EXPOSURE_LEVEL.Replace("{time}", GameUtil.GetFormattedTime(seconds, "F0"));
				return string.Concat(new string[]
				{
					text,
					"\n\n",
					text2,
					"\n\n",
					text3,
					"\n\n",
					text4
				});
			};
			this.ExpellingRads = this.CreateStatusItem("ExpellingRads", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingGenes = this.CreateStatusItem("AnalyzingGenes", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.AnalyzingArtifact = this.CreateStatusItem("AnalyzingArtifact", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 2);
			this.MegaBrainTank_Pajamas_Wearing = this.CreateStatusItem("MegaBrainTank_Pajamas_Wearing", DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.NAME, DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback_shouldStillCallIfDataIsNull = true;
			this.MegaBrainTank_Pajamas_Wearing.resolveTooltipCallback = delegate(string str, object data)
			{
				string str2 = DUPLICANTS.STATUSITEMS.WEARING_PAJAMAS.TOOLTIP;
				Effect effect = Db.Get().effects.Get("SleepClinic");
				string str3;
				if (effect != null)
				{
					str3 = Effect.CreateTooltip(effect, false, "\n    • ", true);
				}
				else
				{
					str3 = "";
				}
				return str2 + "\n\n" + str3;
			};
			this.MegaBrainTank_Pajamas_Sleeping = this.CreateStatusItem("MegaBrainTank_Pajamas_Sleeping", DUPLICANTS.STATUSITEMS.DREAMING.NAME, DUPLICANTS.STATUSITEMS.DREAMING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MegaBrainTank_Pajamas_Sleeping.resolveTooltipCallback = delegate(string str, object data)
			{
				ClinicDreamable clinicDreamable = (ClinicDreamable)data;
				return str.Replace("{time}", GameUtil.GetFormattedTime(clinicDreamable.WorkTimeRemaining, "F0"));
			};
			this.FossilHunt_WorkerExcavating = this.CreateStatusItem("FossilHunt_WorkerExcavating", DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.NAME, DUPLICANTS.STATUSITEMS.FOSSILHUNT.WORKEREXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerWorkingOnRevealing = this.CreateStatusItem("MorbRoverMakerWorkingOnRevealing", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_REVEALING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.MorbRoverMakerDoctorWorking = this.CreateStatusItem("MorbRoverMakerDoctorWorking", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_WORKING_BUILDING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.ArmingTrap = this.CreateStatusItem("ArmingTrap", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube = this.CreateStatusItem("WaxedForTransitTube", "DUPLICANTS", "action_speed_up", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.WaxedForTransitTube.resolveTooltipCallback = delegate(string str, object data)
			{
				float percent = (float)data * 100f;
				return str.Replace("{0}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.JoyResponse_HasBalloon = this.CreateStatusItem("JoyResponse_HasBalloon", DUPLICANTS.MODIFIERS.HASBALLOON.NAME, DUPLICANTS.MODIFIERS.HASBALLOON.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HasBalloon.resolveTooltipCallback = delegate(string str, object data)
			{
				EquippableBalloon.StatesInstance statesInstance = (EquippableBalloon.StatesInstance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(statesInstance.transitionTime - GameClock.Instance.GetTime(), "F1", false));
			};
			this.JoyResponse_HeardJoySinger = this.CreateStatusItem("JoyResponse_HeardJoySinger", DUPLICANTS.MODIFIERS.HEARDJOYSINGER.NAME, DUPLICANTS.MODIFIERS.HEARDJOYSINGER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.JoyResponse_HeardJoySinger.resolveTooltipCallback = delegate(string str, object data)
			{
				InspirationEffectMonitor.Instance instance = (InspirationEffectMonitor.Instance)data;
				return str + "\n\n" + DUPLICANTS.MODIFIERS.TIME_REMAINING.Replace("{0}", GameUtil.GetFormattedCycles(instance.sm.inspirationTimeRemaining.Get(instance), "F1", false));
			};
			this.JoyResponse_StickerBombing = this.CreateStatusItem("JoyResponse_StickerBombing", DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.NAME, DUPLICANTS.MODIFIERS.ISSTICKERBOMBING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.Meteorphile = this.CreateStatusItem("Meteorphile", "DUPLICANTS", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 2);
			this.EnteringDock = this.CreateStatusItem("EnteringDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.ENTERINGDOCK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 2);
			this.UnreachableDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.UNREACHABLEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.NoHomeDock = this.CreateStatusItem("UnreachableDock", DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.NOHOMEDOCK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus = this.CreateStatusItem("RemoteWorkerCapacitorStatus", DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.POWERSTATUS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerCapacitorStatus.resolveStringCallback = delegate(string str, object obj)
			{
				RemoteWorkerCapacitor remoteWorkerCapacitor = obj as RemoteWorkerCapacitor;
				float joules = 0f;
				float percent = 0f;
				if (remoteWorkerCapacitor != null)
				{
					joules = remoteWorkerCapacitor.Charge;
					percent = remoteWorkerCapacitor.ChargeRatio * 100f;
				}
				return str.Replace("{CHARGE}", GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None)).Replace("{RATIO}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.RemoteWorkerLowPower = this.CreateStatusItem("RemoteWorkerLowPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfPower = this.CreateStatusItem("RemoteWorkerOutOfPower", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFPOWER.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerHighGunkLevel = this.CreateStatusItem("RemoteWorkerHighGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.HIGHGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerFullGunkLevel = this.CreateStatusItem("RemoteWorkerFullGunkLevel", DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.FULLGUNK.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerLowOil = this.CreateStatusItem("RemoteWorkerLowOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.LOWOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOutOfOil = this.CreateStatusItem("RemoteWorkerOutOfOil", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OUTOFOIL.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerRecharging = this.CreateStatusItem("RemoteWorkerRecharging", DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.RECHARGING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerOiling = this.CreateStatusItem("RemoteWorkerOiling", DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.OILING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.RemoteWorkerDraining = this.CreateStatusItem("RemoteWorkerDraining", DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.NAME, DUPLICANTS.STATUSITEMS.REMOTEWORKER.DRAINING.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, 2);
			this.BionicCriticalBattery = this.CreateStatusItem("BionicCriticalBattery", "DUPLICANTS", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 2);
			this.BionicCriticalBattery.AddNotification(null, null, null);
		}

		// Token: 0x04009404 RID: 37892
		public StatusItem Idle;

		// Token: 0x04009405 RID: 37893
		public StatusItem IdleInRockets;

		// Token: 0x04009406 RID: 37894
		public StatusItem Pacified;

		// Token: 0x04009407 RID: 37895
		public StatusItem PendingPacification;

		// Token: 0x04009408 RID: 37896
		public StatusItem Dead;

		// Token: 0x04009409 RID: 37897
		public StatusItem CloggingToilet;

		// Token: 0x0400940A RID: 37898
		public StatusItem MoveToSuitNotRequired;

		// Token: 0x0400940B RID: 37899
		public StatusItem DroppingUnusedInventory;

		// Token: 0x0400940C RID: 37900
		public StatusItem MovingToSafeArea;

		// Token: 0x0400940D RID: 37901
		public StatusItem BedUnreachable;

		// Token: 0x0400940E RID: 37902
		public StatusItem Hungry;

		// Token: 0x0400940F RID: 37903
		public StatusItem Starving;

		// Token: 0x04009410 RID: 37904
		public StatusItem Rotten;

		// Token: 0x04009411 RID: 37905
		public StatusItem Quarantined;

		// Token: 0x04009412 RID: 37906
		public StatusItem NoRationsAvailable;

		// Token: 0x04009413 RID: 37907
		public StatusItem RationsUnreachable;

		// Token: 0x04009414 RID: 37908
		public StatusItem RationsNotPermitted;

		// Token: 0x04009415 RID: 37909
		public StatusItem DailyRationLimitReached;

		// Token: 0x04009416 RID: 37910
		public StatusItem Scalding;

		// Token: 0x04009417 RID: 37911
		public StatusItem Hot;

		// Token: 0x04009418 RID: 37912
		public StatusItem Cold;

		// Token: 0x04009419 RID: 37913
		public StatusItem ExitingCold;

		// Token: 0x0400941A RID: 37914
		public StatusItem ExitingHot;

		// Token: 0x0400941B RID: 37915
		public StatusItem QuarantineAreaUnassigned;

		// Token: 0x0400941C RID: 37916
		public StatusItem QuarantineAreaUnreachable;

		// Token: 0x0400941D RID: 37917
		public StatusItem Tired;

		// Token: 0x0400941E RID: 37918
		public StatusItem NervousBreakdown;

		// Token: 0x0400941F RID: 37919
		public StatusItem Unhappy;

		// Token: 0x04009420 RID: 37920
		public StatusItem Suffocating;

		// Token: 0x04009421 RID: 37921
		public StatusItem HoldingBreath;

		// Token: 0x04009422 RID: 37922
		public StatusItem ToiletUnreachable;

		// Token: 0x04009423 RID: 37923
		public StatusItem NoUsableToilets;

		// Token: 0x04009424 RID: 37924
		public StatusItem NoToilets;

		// Token: 0x04009425 RID: 37925
		public StatusItem Vomiting;

		// Token: 0x04009426 RID: 37926
		public StatusItem Coughing;

		// Token: 0x04009427 RID: 37927
		public StatusItem Slippering;

		// Token: 0x04009428 RID: 37928
		public StatusItem BreathingO2;

		// Token: 0x04009429 RID: 37929
		public StatusItem BreathingO2Bionic;

		// Token: 0x0400942A RID: 37930
		public StatusItem EmittingCO2;

		// Token: 0x0400942B RID: 37931
		public StatusItem LowOxygen;

		// Token: 0x0400942C RID: 37932
		public StatusItem RedAlert;

		// Token: 0x0400942D RID: 37933
		public StatusItem Digging;

		// Token: 0x0400942E RID: 37934
		public StatusItem Eating;

		// Token: 0x0400942F RID: 37935
		public StatusItem Dreaming;

		// Token: 0x04009430 RID: 37936
		public StatusItem Sleeping;

		// Token: 0x04009431 RID: 37937
		public StatusItem SleepingExhausted;

		// Token: 0x04009432 RID: 37938
		public StatusItem SleepingInterruptedByLight;

		// Token: 0x04009433 RID: 37939
		public StatusItem SleepingInterruptedByNoise;

		// Token: 0x04009434 RID: 37940
		public StatusItem SleepingInterruptedByFearOfDark;

		// Token: 0x04009435 RID: 37941
		public StatusItem SleepingInterruptedByMovement;

		// Token: 0x04009436 RID: 37942
		public StatusItem SleepingInterruptedByCold;

		// Token: 0x04009437 RID: 37943
		public StatusItem SleepingPeacefully;

		// Token: 0x04009438 RID: 37944
		public StatusItem SleepingBadly;

		// Token: 0x04009439 RID: 37945
		public StatusItem SleepingTerribly;

		// Token: 0x0400943A RID: 37946
		public StatusItem Cleaning;

		// Token: 0x0400943B RID: 37947
		public StatusItem PickingUp;

		// Token: 0x0400943C RID: 37948
		public StatusItem Mopping;

		// Token: 0x0400943D RID: 37949
		public StatusItem Cooking;

		// Token: 0x0400943E RID: 37950
		public StatusItem Arting;

		// Token: 0x0400943F RID: 37951
		public StatusItem Mushing;

		// Token: 0x04009440 RID: 37952
		public StatusItem Researching;

		// Token: 0x04009441 RID: 37953
		public StatusItem ResearchingFromPOI;

		// Token: 0x04009442 RID: 37954
		public StatusItem MissionControlling;

		// Token: 0x04009443 RID: 37955
		public StatusItem Tinkering;

		// Token: 0x04009444 RID: 37956
		public StatusItem Storing;

		// Token: 0x04009445 RID: 37957
		public StatusItem Building;

		// Token: 0x04009446 RID: 37958
		public StatusItem Equipping;

		// Token: 0x04009447 RID: 37959
		public StatusItem WarmingUp;

		// Token: 0x04009448 RID: 37960
		public StatusItem GeneratingPower;

		// Token: 0x04009449 RID: 37961
		public StatusItem Ranching;

		// Token: 0x0400944A RID: 37962
		public StatusItem Harvesting;

		// Token: 0x0400944B RID: 37963
		public StatusItem Uprooting;

		// Token: 0x0400944C RID: 37964
		public StatusItem Emptying;

		// Token: 0x0400944D RID: 37965
		public StatusItem Toggling;

		// Token: 0x0400944E RID: 37966
		public StatusItem Deconstructing;

		// Token: 0x0400944F RID: 37967
		public StatusItem Disinfecting;

		// Token: 0x04009450 RID: 37968
		public StatusItem Relocating;

		// Token: 0x04009451 RID: 37969
		public StatusItem Upgrading;

		// Token: 0x04009452 RID: 37970
		public StatusItem Fabricating;

		// Token: 0x04009453 RID: 37971
		public StatusItem Processing;

		// Token: 0x04009454 RID: 37972
		public StatusItem Spicing;

		// Token: 0x04009455 RID: 37973
		public StatusItem Clearing;

		// Token: 0x04009456 RID: 37974
		public StatusItem BodyRegulatingHeating;

		// Token: 0x04009457 RID: 37975
		public StatusItem BodyRegulatingCooling;

		// Token: 0x04009458 RID: 37976
		public StatusItem EntombedChore;

		// Token: 0x04009459 RID: 37977
		public StatusItem EarlyMorning;

		// Token: 0x0400945A RID: 37978
		public StatusItem NightTime;

		// Token: 0x0400945B RID: 37979
		public StatusItem PoorDecor;

		// Token: 0x0400945C RID: 37980
		public StatusItem PoorQualityOfLife;

		// Token: 0x0400945D RID: 37981
		public StatusItem PoorFoodQuality;

		// Token: 0x0400945E RID: 37982
		public StatusItem GoodFoodQuality;

		// Token: 0x0400945F RID: 37983
		public StatusItem SevereWounds;

		// Token: 0x04009460 RID: 37984
		public StatusItem Incapacitated;

		// Token: 0x04009461 RID: 37985
		public StatusItem BionicOfflineIncapacitated;

		// Token: 0x04009462 RID: 37986
		public StatusItem BionicWaitingForReboot;

		// Token: 0x04009463 RID: 37987
		public StatusItem BionicBeingRebooted;

		// Token: 0x04009464 RID: 37988
		public StatusItem BionicRequiresSkillPerk;

		// Token: 0x04009465 RID: 37989
		public StatusItem BionicWantsOilChange;

		// Token: 0x04009466 RID: 37990
		public StatusItem BionicMicrochipGeneration;

		// Token: 0x04009467 RID: 37991
		public StatusItem InstallingElectrobank;

		// Token: 0x04009468 RID: 37992
		public StatusItem Fighting;

		// Token: 0x04009469 RID: 37993
		public StatusItem Fleeing;

		// Token: 0x0400946A RID: 37994
		public StatusItem Stressed;

		// Token: 0x0400946B RID: 37995
		public StatusItem LashingOut;

		// Token: 0x0400946C RID: 37996
		public StatusItem LowImmunity;

		// Token: 0x0400946D RID: 37997
		public StatusItem Studying;

		// Token: 0x0400946E RID: 37998
		public StatusItem Socializing;

		// Token: 0x0400946F RID: 37999
		public StatusItem Mingling;

		// Token: 0x04009470 RID: 38000
		public StatusItem ContactWithGerms;

		// Token: 0x04009471 RID: 38001
		public StatusItem ExposedToGerms;

		// Token: 0x04009472 RID: 38002
		public StatusItem LightWorkEfficiencyBonus;

		// Token: 0x04009473 RID: 38003
		public StatusItem LaboratoryWorkEfficiencyBonus;

		// Token: 0x04009474 RID: 38004
		public StatusItem BeingProductive;

		// Token: 0x04009475 RID: 38005
		public StatusItem BalloonArtistPlanning;

		// Token: 0x04009476 RID: 38006
		public StatusItem BalloonArtistHandingOut;

		// Token: 0x04009477 RID: 38007
		public StatusItem Partying;

		// Token: 0x04009478 RID: 38008
		public StatusItem GasLiquidIrritation;

		// Token: 0x04009479 RID: 38009
		public StatusItem ExpellingRads;

		// Token: 0x0400947A RID: 38010
		public StatusItem AnalyzingGenes;

		// Token: 0x0400947B RID: 38011
		public StatusItem AnalyzingArtifact;

		// Token: 0x0400947C RID: 38012
		public StatusItem MegaBrainTank_Pajamas_Wearing;

		// Token: 0x0400947D RID: 38013
		public StatusItem MegaBrainTank_Pajamas_Sleeping;

		// Token: 0x0400947E RID: 38014
		public StatusItem JoyResponse_HasBalloon;

		// Token: 0x0400947F RID: 38015
		public StatusItem JoyResponse_HeardJoySinger;

		// Token: 0x04009480 RID: 38016
		public StatusItem JoyResponse_StickerBombing;

		// Token: 0x04009481 RID: 38017
		public StatusItem Meteorphile;

		// Token: 0x04009482 RID: 38018
		public StatusItem FossilHunt_WorkerExcavating;

		// Token: 0x04009483 RID: 38019
		public StatusItem MorbRoverMakerDoctorWorking;

		// Token: 0x04009484 RID: 38020
		public StatusItem MorbRoverMakerWorkingOnRevealing;

		// Token: 0x04009485 RID: 38021
		public StatusItem ArmingTrap;

		// Token: 0x04009486 RID: 38022
		public StatusItem WaxedForTransitTube;

		// Token: 0x04009487 RID: 38023
		public StatusItem DataRainerPlanning;

		// Token: 0x04009488 RID: 38024
		public StatusItem DataRainerRaining;

		// Token: 0x04009489 RID: 38025
		public StatusItem RoboDancerPlanning;

		// Token: 0x0400948A RID: 38026
		public StatusItem RoboDancerDancing;

		// Token: 0x0400948B RID: 38027
		public StatusItem WatchRoboDancerWorkable;

		// Token: 0x0400948C RID: 38028
		public StatusItem BionicExplorerBooster;

		// Token: 0x0400948D RID: 38029
		public StatusItem EnteringDock;

		// Token: 0x0400948E RID: 38030
		public StatusItem UnreachableDock;

		// Token: 0x0400948F RID: 38031
		public StatusItem NoHomeDock;

		// Token: 0x04009490 RID: 38032
		public StatusItem RemoteWorkerCapacitorStatus;

		// Token: 0x04009491 RID: 38033
		public StatusItem RemoteWorkerLowPower;

		// Token: 0x04009492 RID: 38034
		public StatusItem RemoteWorkerOutOfPower;

		// Token: 0x04009493 RID: 38035
		public StatusItem RemoteWorkerHighGunkLevel;

		// Token: 0x04009494 RID: 38036
		public StatusItem RemoteWorkerFullGunkLevel;

		// Token: 0x04009495 RID: 38037
		public StatusItem RemoteWorkerLowOil;

		// Token: 0x04009496 RID: 38038
		public StatusItem RemoteWorkerOutOfOil;

		// Token: 0x04009497 RID: 38039
		public StatusItem RemoteWorkerRecharging;

		// Token: 0x04009498 RID: 38040
		public StatusItem RemoteWorkerOiling;

		// Token: 0x04009499 RID: 38041
		public StatusItem RemoteWorkerDraining;

		// Token: 0x0400949A RID: 38042
		public StatusItem BionicCriticalBattery;

		// Token: 0x0400949B RID: 38043
		private const int NONE_OVERLAY = 0;
	}
}
