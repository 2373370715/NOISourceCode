using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x0200218C RID: 8588
	public class BuildingStatusItems : StatusItems
	{
		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0011AFD6 File Offset: 0x001191D6
		public BuildingStatusItems(ResourceSet parent) : base("BuildingStatusItems", parent)
		{
			this.CreateStatusItems();
		}

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x00459104 File Offset: 0x00457304
		private StatusItem CreateStatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, prefix, icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, null));
		}

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x0045912C File Offset: 0x0045732C
		private StatusItem CreateStatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022)
		{
			return base.Add(new StatusItem(id, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null));
		}

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x00459158 File Offset: 0x00457358
		private void CreateStatusItems()
		{
			this.AngerDamage = this.CreateStatusItem("AngerDamage", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.AssignedTo = this.CreateStatusItem("AssignedTo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AssignedTo.resolveStringCallback = delegate(string str, object data)
			{
				IAssignableIdentity assignee = ((Assignable)data).assignee;
				if (!assignee.IsNullOrDestroyed())
				{
					string properName = assignee.GetProperName();
					str = str.Replace("{Assignee}", properName);
				}
				return str;
			};
			this.AssignedToRoom = this.CreateStatusItem("AssignedToRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AssignedToRoom.resolveStringCallback = delegate(string str, object data)
			{
				IAssignableIdentity assignee = ((Assignable)data).assignee;
				if (!assignee.IsNullOrDestroyed())
				{
					string properName = assignee.GetProperName();
					str = str.Replace("{Assignee}", properName);
				}
				return str;
			};
			this.Broken = this.CreateStatusItem("Broken", "BUILDING", "status_item_broken", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Broken.resolveStringCallback = ((string str, object data) => str.Replace("{DamageInfo}", ((BuildingHP.SMInstance)data).master.GetDamageSourceInfo().ToString()));
			this.Broken.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.ChangeStorageTileTarget = this.CreateStatusItem("ChangeStorageTileTarget", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ChangeStorageTileTarget.resolveStringCallback = delegate(string str, object data)
			{
				StorageTile.Instance instance = (StorageTile.Instance)data;
				return str.Replace("{TargetName}", (instance.TargetTag == StorageTile.INVALID_TAG) ? BUILDING.STATUSITEMS.CHANGESTORAGETILETARGET.EMPTY.text : instance.TargetTag.ProperName());
			};
			this.ChangeDoorControlState = this.CreateStatusItem("ChangeDoorControlState", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ChangeDoorControlState.resolveStringCallback = delegate(string str, object data)
			{
				Door door = (Door)data;
				return str.Replace("{ControlState}", door.RequestedState.ToString());
			};
			this.CurrentDoorControlState = this.CreateStatusItem("CurrentDoorControlState", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CurrentDoorControlState.resolveStringCallback = delegate(string str, object data)
			{
				Door door = (Door)data;
				string newValue = Strings.Get("STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE." + door.CurrentState.ToString().ToUpper());
				return str.Replace("{ControlState}", newValue);
			};
			this.GunkEmptierFull = this.CreateStatusItem("GunkEmptierFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.ClinicOutsideHospital = this.CreateStatusItem("ClinicOutsideHospital", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.ConduitBlocked = this.CreateStatusItem("ConduitBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OutputPipeFull = this.CreateStatusItem("OutputPipeFull", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.OutputTileBlocked = this.CreateStatusItem("OutputTileBlocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ConstructionUnreachable = this.CreateStatusItem("ConstructionUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ConduitBlockedMultiples = this.CreateStatusItem("ConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022);
			this.SolidConduitBlockedMultiples = this.CreateStatusItem("SolidConduitBlockedMultiples", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID, true, 129022);
			this.PowerBankChargerInProgress = this.CreateStatusItem("PowerBankChargerInProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PowerBankChargerInProgress.resolveStringCallback = delegate(string str, object data)
			{
				if (data == null)
				{
					return str;
				}
				ElectrobankCharger.Instance instance = (ElectrobankCharger.Instance)data;
				GameObject targetElectrobank = instance.targetElectrobank;
				if (instance.targetElectrobank != null)
				{
					str = string.Format(str, GameUtil.GetFormattedWattage(400f, GameUtil.WattageFormatterUnit.Automatic, true));
				}
				return str;
			};
			this.DigUnreachable = this.CreateStatusItem("DigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MopUnreachable = this.CreateStatusItem("MopUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.StorageUnreachable = this.CreateStatusItem("StorageUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PassengerModuleUnreachable = this.CreateStatusItem("PassengerModuleUnreachable", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DirectionControl = this.CreateStatusItem("DirectionControl", BUILDING.STATUSITEMS.DIRECTION_CONTROL.NAME, BUILDING.STATUSITEMS.DIRECTION_CONTROL.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.DirectionControl.resolveStringCallback = delegate(string str, object data)
			{
				DirectionControl directionControl = (DirectionControl)data;
				string newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.BOTH;
				WorkableReactable.AllowedDirection allowedDirection = directionControl.allowedDirection;
				if (allowedDirection != WorkableReactable.AllowedDirection.Left)
				{
					if (allowedDirection == WorkableReactable.AllowedDirection.Right)
					{
						newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.RIGHT;
					}
				}
				else
				{
					newValue = BUILDING.STATUSITEMS.DIRECTION_CONTROL.DIRECTIONS.LEFT;
				}
				str = str.Replace("{Direction}", newValue);
				return str;
			};
			this.DeadReactorCoolingOff = this.CreateStatusItem("DeadReactorCoolingOff", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DeadReactorCoolingOff.resolveStringCallback = delegate(string str, object data)
			{
				Reactor.StatesInstance smi = (Reactor.StatesInstance)data;
				float num = ((Reactor.StatesInstance)data).sm.timeSinceMeltdown.Get(smi);
				str = str.Replace("{CyclesRemaining}", Util.FormatOneDecimalPlace(Mathf.Max(0f, 3000f - num) / 600f));
				return str;
			};
			this.ConstructableDigUnreachable = this.CreateStatusItem("ConstructableDigUnreachable", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Entombed = this.CreateStatusItem("Entombed", "BUILDING", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Entombed.AddNotification(null, null, null);
			this.Flooded = this.CreateStatusItem("Flooded", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Flooded.AddNotification(null, null, null);
			this.NotSubmerged = this.CreateStatusItem("NotSubmerged", "BUILDING", "status_item_flooded", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.GasVentObstructed = this.CreateStatusItem("GasVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GasVentOverPressure = this.CreateStatusItem("GasVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GeneShuffleCompleted = this.CreateStatusItem("GeneShuffleCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.GeneticAnalysisCompleted = this.CreateStatusItem("GeneticAnalysisCompleted", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InvalidBuildingLocation = this.CreateStatusItem("InvalidBuildingLocation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.LiquidVentObstructed = this.CreateStatusItem("LiquidVentObstructed", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.LiquidVentOverPressure = this.CreateStatusItem("LiquidVentOverPressure", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.MaterialsUnavailable = new MaterialsStatusItem("MaterialsUnavailable", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.None.ID);
			this.MaterialsUnavailable.AddNotification(null, null, null);
			this.MaterialsUnavailable.resolveStringCallback = delegate(string str, object data)
			{
				string text = "";
				Dictionary<Tag, float> dictionary = null;
				if (data is IFetchList)
				{
					dictionary = ((IFetchList)data).GetRemainingMinimum();
				}
				else if (data is Dictionary<Tag, float>)
				{
					dictionary = (data as Dictionary<Tag, float>);
				}
				if (dictionary.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in dictionary)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							if (Assets.IsTagCountable(keyValuePair.Key))
							{
								text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
							}
							else
							{
								text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLE.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			this.MaterialsUnavailableForRefill = new MaterialsStatusItem("MaterialsUnavailableForRefill", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, true, OverlayModes.None.ID);
			this.MaterialsUnavailableForRefill.resolveStringCallback = delegate(string str, object data)
			{
				IFetchList fetchList = (IFetchList)data;
				string text = "";
				Dictionary<Tag, float> remaining = fetchList.GetRemaining();
				if (remaining.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							text += string.Format(BUILDING.STATUSITEMS.MATERIALSUNAVAILABLEFORREFILL.LINE_ITEM, keyValuePair.Key.ProperName());
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			Func<string, object, string> resolveStringCallback = delegate(string str, object data)
			{
				RoomType roomType = Db.Get().RoomTypes.Get((string)data);
				if (roomType != null)
				{
					return str.Replace("{0}", roomType.Name);
				}
				return str;
			};
			this.NoCoolant = this.CreateStatusItem("NoCoolant", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInAnyRoom = this.CreateStatusItem("NotInAnyRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInRequiredRoom = this.CreateStatusItem("NotInRequiredRoom", "BUILDING", "status_item_room_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NotInRequiredRoom.resolveStringCallback = resolveStringCallback;
			this.NotInRecommendedRoom = this.CreateStatusItem("NotInRecommendedRoom", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NotInRecommendedRoom.resolveStringCallback = resolveStringCallback;
			this.MercuryLight_Charging = this.CreateStatusItem("MercuryLight_Charging", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MercuryLight_Charging.resolveStringCallback = delegate(string str, object data)
			{
				MercuryLight.Instance instance = (MercuryLight.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.ChargeLevel * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.MercuryLight_Charging.resolveTooltipCallback = delegate(string str, object data)
			{
				MercuryLight.Instance instance = (MercuryLight.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedTime((1f - instance.ChargeLevel) * instance.def.TURN_ON_DELAY, "F0"));
				return str;
			};
			this.MercuryLight_Depleating = this.CreateStatusItem("MercuryLight_Depleating", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MercuryLight_Depleating.resolveStringCallback = delegate(string str, object data)
			{
				MercuryLight.Instance instance = (MercuryLight.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedPercent(instance.ChargeLevel * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.MercuryLight_Charged = this.CreateStatusItem("MercuryLight_Charged", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MercuryLight_Depleated = this.CreateStatusItem("MercuryLight_Depleated", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WaitingForRepairMaterials = this.CreateStatusItem("WaitingForRepairMaterials", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID, false, 129022);
			this.WaitingForRepairMaterials.resolveStringCallback = delegate(string str, object data)
			{
				KeyValuePair<Tag, float> keyValuePair = (KeyValuePair<Tag, float>)data;
				if (keyValuePair.Value != 0f)
				{
					string newValue = string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
					str = str.Replace("{ItemsRemaining}", newValue);
				}
				return str;
			};
			this.WaitingForMaterials = new MaterialsStatusItem("WaitingForMaterials", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, true, OverlayModes.None.ID);
			this.WaitingForMaterials.resolveStringCallback = delegate(string str, object data)
			{
				IFetchList fetchList = (IFetchList)data;
				string text = "";
				Dictionary<Tag, float> remaining = fetchList.GetRemaining();
				if (remaining.Count > 0)
				{
					bool flag = true;
					foreach (KeyValuePair<Tag, float> keyValuePair in remaining)
					{
						if (keyValuePair.Value != 0f)
						{
							if (!flag)
							{
								text += "\n";
							}
							if (Assets.IsTagCountable(keyValuePair.Key))
							{
								text += string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_UNITS, GameUtil.GetUnitFormattedName(keyValuePair.Key.ProperName(), keyValuePair.Value, false));
							}
							else
							{
								text += string.Format(BUILDING.STATUSITEMS.WAITINGFORMATERIALS.LINE_ITEM_MASS, keyValuePair.Key.ProperName(), GameUtil.GetFormattedMass(keyValuePair.Value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							flag = false;
						}
					}
				}
				str = str.Replace("{ItemsRemaining}", text);
				return str;
			};
			this.WaitingForHighEnergyParticles = new StatusItem("WaitingForRadiation", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.MeltingDown = this.CreateStatusItem("MeltingDown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MissingFoundation = this.CreateStatusItem("MissingFoundation", "BUILDING", "status_item_missing_foundation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeutroniumUnminable = this.CreateStatusItem("NeutroniumUnminable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedGasIn = this.CreateStatusItem("NeedGasIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.NeedGasIn.resolveStringCallback = delegate(string str, object data)
			{
				global::Tuple<ConduitType, Tag> tuple = (global::Tuple<ConduitType, Tag>)data;
				string newValue = string.Format(BUILDING.STATUSITEMS.NEEDGASIN.LINE_ITEM, tuple.second.ProperName());
				str = str.Replace("{GasRequired}", newValue);
				return str;
			};
			this.NeedGasOut = this.CreateStatusItem("NeedGasOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.GasConduits.ID, true, 129022);
			this.NeedLiquidIn = this.CreateStatusItem("NeedLiquidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NeedLiquidIn.resolveStringCallback = delegate(string str, object data)
			{
				global::Tuple<ConduitType, Tag> tuple = (global::Tuple<ConduitType, Tag>)data;
				string newValue = string.Format(BUILDING.STATUSITEMS.NEEDLIQUIDIN.LINE_ITEM, tuple.second.ProperName());
				str = str.Replace("{LiquidRequired}", newValue);
				return str;
			};
			this.NeedLiquidOut = this.CreateStatusItem("NeedLiquidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NeedSolidIn = this.CreateStatusItem("NeedSolidIn", "BUILDING", "status_item_need_supply_in", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedSolidOut = this.CreateStatusItem("NeedSolidOut", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedResourceMass = this.CreateStatusItem("NeedResourceMass", "BUILDING", "status_item_need_resource", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedResourceMass.resolveStringCallback = delegate(string str, object data)
			{
				string text = "";
				EnergyGenerator.Formula formula = (EnergyGenerator.Formula)data;
				if (formula.inputs.Length != 0)
				{
					bool flag = true;
					foreach (EnergyGenerator.InputItem inputItem in formula.inputs)
					{
						if (!flag)
						{
							text += "\n";
							flag = false;
						}
						text += string.Format(BUILDING.STATUSITEMS.NEEDRESOURCEMASS.LINE_ITEM, inputItem.tag.ProperName());
					}
				}
				str = str.Replace("{ResourcesRequired}", text);
				return str;
			};
			this.LiquidPipeEmpty = this.CreateStatusItem("LiquidPipeEmpty", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.LiquidPipeObstructed = this.CreateStatusItem("LiquidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.LiquidConduits.ID, true, 129022);
			this.GasPipeEmpty = this.CreateStatusItem("GasPipeEmpty", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.GasPipeObstructed = this.CreateStatusItem("GasPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.GasConduits.ID, true, 129022);
			this.SolidPipeObstructed = this.CreateStatusItem("SolidPipeObstructed", "BUILDING", "status_item_wrong_resource_in_pipe", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.SolidConveyor.ID, true, 129022);
			this.NeedPlant = this.CreateStatusItem("NeedPlant", "BUILDING", "status_item_need_plant", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedPower = this.CreateStatusItem("NeedPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.NotEnoughPower = this.CreateStatusItem("NotEnoughPower", "BUILDING", "status_item_need_power", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.PowerLoopDetected = this.CreateStatusItem("PowerLoopDetected", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.CoolingWater = this.CreateStatusItem("CoolingWater", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.DispenseRequested = this.CreateStatusItem("DispenseRequested", "BUILDING", "status_item_exclamation", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NewDuplicantsAvailable = this.CreateStatusItem("NewDuplicantsAvailable", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NewDuplicantsAvailable.AddNotification(null, null, null);
			this.NewDuplicantsAvailable.notificationClickCallback = delegate(object data)
			{
				int idx = 0;
				for (int i = 0; i < Components.Telepads.Items.Count; i++)
				{
					if (Components.Telepads[i].GetComponent<KSelectable>().IsSelected)
					{
						idx = (i + 1) % Components.Telepads.Items.Count;
						break;
					}
				}
				Telepad targetTelepad = Components.Telepads[idx];
				GameUtil.FocusCameraOnWorld(targetTelepad.GetMyWorldId(), targetTelepad.transform.GetPosition(), 10f, delegate
				{
					SelectTool.Instance.Select(targetTelepad.GetComponent<KSelectable>(), false);
				}, true);
			};
			this.NoStorageFilterSet = this.CreateStatusItem("NoStorageFilterSet", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoSuitMarker = this.CreateStatusItem("NoSuitMarker", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerWrongSide = this.CreateStatusItem("suitMarkerWrongSide", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerTraversalAnytime = this.CreateStatusItem("suitMarkerTraversalAnytime", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SuitMarkerTraversalOnlyWhenRoomAvailable = this.CreateStatusItem("suitMarkerTraversalOnlyWhenRoomAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoFishableWaterBelow = this.CreateStatusItem("NoFishableWaterBelow", "BUILDING", "status_item_no_fishable_water_below", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoPowerConsumers = this.CreateStatusItem("NoPowerConsumers", "BUILDING", "status_item_no_power_consumers", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.NoWireConnected = this.CreateStatusItem("NoWireConnected", "BUILDING", "status_item_no_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, true, OverlayModes.Power.ID, true, 129022);
			this.NoLogicWireConnected = this.CreateStatusItem("NoLogicWireConnected", "BUILDING", "status_item_no_logic_wire_connected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Logic.ID, true, 129022);
			this.NoTubeConnected = this.CreateStatusItem("NoTubeConnected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoTubeExits = this.CreateStatusItem("NoTubeExits", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.StoredCharge = this.CreateStatusItem("StoredCharge", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.StoredCharge.resolveStringCallback = delegate(string str, object data)
			{
				TravelTubeEntrance.SMInstance sminstance = (TravelTubeEntrance.SMInstance)data;
				if (sminstance != null)
				{
					str = string.Format(str, GameUtil.GetFormattedRoundedJoules(sminstance.master.AvailableJoules), GameUtil.GetFormattedRoundedJoules(sminstance.master.TotalCapacity), GameUtil.GetFormattedRoundedJoules(sminstance.master.UsageJoules));
				}
				return str;
			};
			this.PendingDeconstruction = this.CreateStatusItem("PendingDeconstruction", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingDeconstruction.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.PendingDemolition = this.CreateStatusItem("PendingDemolition", "BUILDING", "status_item_pending_deconstruction", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingDemolition.conditionalOverlayCallback = new Func<HashedString, object, bool>(BuildingStatusItems.ShowInUtilityOverlay);
			this.PendingRepair = this.CreateStatusItem("PendingRepair", "BUILDING", "status_item_pending_repair", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PendingRepair.resolveStringCallback = ((string str, object data) => str.Replace("{DamageInfo}", ((Repairable.SMInstance)data).master.GetComponent<BuildingHP>().GetDamageSourceInfo().ToString()));
			this.PendingRepair.conditionalOverlayCallback = ((HashedString mode, object data) => true);
			this.RequiresSkillPerk = this.CreateStatusItem("RequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				return str;
			};
			this.RequiresSkillPerk.resolveTooltipCallback = delegate(string str, object data)
			{
				str = (Game.IsDlcActiveForCurrentSave("DLC3_ID") ? BUILDING.STATUSITEMS.REQUIRESSKILLPERK.TOOLTIP_DLC3 : BUILDING.STATUSITEMS.REQUIRESSKILLPERK.TOOLTIP);
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				str = str.Replace("{Boosters}", GameUtil.NamesOfBoostersWithSkillPerk((string)data));
				return str;
			};
			this.DigRequiresSkillPerk = this.CreateStatusItem("DigRequiresSkillPerk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.DigRequiresSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				return str;
			};
			this.DigRequiresSkillPerk.resolveTooltipCallback = delegate(string str, object data)
			{
				str = (Game.IsDlcActiveForCurrentSave("DLC3_ID") ? BUILDING.STATUSITEMS.DIGREQUIRESSKILLPERK.TOOLTIP_DLC3 : BUILDING.STATUSITEMS.DIGREQUIRESSKILLPERK.TOOLTIP);
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				str = str.Replace("{Boosters}", GameUtil.NamesOfBoostersWithSkillPerk((string)data));
				return str;
			};
			this.ColonyLacksRequiredSkillPerk = this.CreateStatusItem("ColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ColonyLacksRequiredSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				return str;
			};
			this.ColonyLacksRequiredSkillPerk.resolveTooltipCallback = delegate(string str, object data)
			{
				str = (Game.IsDlcActiveForCurrentSave("DLC3_ID") ? BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP_DLC3 : BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP);
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				str = str.Replace("{Boosters}", GameUtil.NamesOfBoostersWithSkillPerk((string)data));
				return str;
			};
			this.ClusterColonyLacksRequiredSkillPerk = this.CreateStatusItem("ClusterColonyLacksRequiredSkillPerk", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ClusterColonyLacksRequiredSkillPerk.resolveStringCallback = delegate(string str, object data)
			{
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				return str;
			};
			this.ClusterColonyLacksRequiredSkillPerk.resolveTooltipCallback = delegate(string str, object data)
			{
				str = (Game.IsDlcActiveForCurrentSave("DLC3_ID") ? BUILDING.STATUSITEMS.CLUSTERCOLONYLACKSREQUIREDSKILLPERK.TOOLTIP_DLC3 : BUILDING.STATUSITEMS.CLUSTERCOLONYLACKSREQUIREDSKILLPERK.TOOLTIP);
				str = str.Replace("{Skills}", GameUtil.NamesOfSkillsWithSkillPerk((string)data));
				str = str.Replace("{Boosters}", GameUtil.NamesOfBoostersWithSkillPerk((string)data));
				return str;
			};
			this.WorkRequiresMinion = this.CreateStatusItem("WorkRequiresMinion", "BUILDING", "status_item_role_required", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SwitchStatusActive = this.CreateStatusItem("SwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SwitchStatusInactive = this.CreateStatusItem("SwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSwitchStatusActive = this.CreateStatusItem("LogicSwitchStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSwitchStatusInactive = this.CreateStatusItem("LogicSwitchStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSensorStatusActive = this.CreateStatusItem("LogicSensorStatusActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LogicSensorStatusInactive = this.CreateStatusItem("LogicSensorStatusInactive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingFish = this.CreateStatusItem("PendingFish", "BUILDING", "status_item_pending_fish", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingSwitchToggle = this.CreateStatusItem("PendingSwitchToggle", "BUILDING", "status_item_pending_switch_toggle", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingUpgrade = this.CreateStatusItem("PendingUpgrade", "BUILDING", "status_item_pending_upgrade", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PendingWork = this.CreateStatusItem("PendingWork", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PowerButtonOff = this.CreateStatusItem("PowerButtonOff", "BUILDING", "status_item_power_button_off", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PressureOk = this.CreateStatusItem("PressureOk", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
			this.UnderPressure = this.CreateStatusItem("UnderPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Oxygen.ID, true, 129022);
			this.UnderPressure.resolveTooltipCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return str.Replace("{TargetPressure}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.Unassigned = this.CreateStatusItem("Unassigned", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Rooms.ID, true, 129022);
			this.AssignedPublic = this.CreateStatusItem("AssignedPublic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Rooms.ID, true, 129022);
			this.UnderConstruction = this.CreateStatusItem("UnderConstruction", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.UnderConstructionNoWorker = this.CreateStatusItem("UnderConstructionNoWorker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Normal = this.CreateStatusItem("Normal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ManualGeneratorChargingUp = this.CreateStatusItem("ManualGeneratorChargingUp", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ManualGeneratorReleasingEnergy = this.CreateStatusItem("ManualGeneratorReleasingEnergy", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.GeneratorOffline = this.CreateStatusItem("GeneratorOffline", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.Pipe = this.CreateStatusItem("Pipe", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.Pipe.resolveStringCallback = delegate(string str, object data)
			{
				Conduit conduit = (Conduit)data;
				int cell = Grid.PosToCell(conduit);
				ConduitFlow.ConduitContents contents = conduit.GetFlowManager().GetContents(cell);
				string text = BUILDING.STATUSITEMS.PIPECONTENTS.EMPTY;
				if (contents.mass > 0f)
				{
					Element element = ElementLoader.FindElementByHash(contents.element);
					text = string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS, GameUtil.GetFormattedMass(contents.mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), element.name, GameUtil.GetFormattedTemperature(contents.temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
					if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && contents.diseaseIdx != 255)
					{
						text += string.Format(BUILDING.STATUSITEMS.PIPECONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(contents.diseaseIdx, contents.diseaseCount, true));
					}
				}
				str = str.Replace("{Contents}", text);
				return str;
			};
			this.Conveyor = this.CreateStatusItem("Conveyor", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.SolidConveyor.ID, true, 129022);
			this.Conveyor.resolveStringCallback = delegate(string str, object data)
			{
				int cell = Grid.PosToCell((SolidConduit)data);
				SolidConduitFlow solidConduitFlow = Game.Instance.solidConduitFlow;
				SolidConduitFlow.ConduitContents contents = solidConduitFlow.GetContents(cell);
				string text = BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.EMPTY;
				if (contents.pickupableHandle.IsValid())
				{
					Pickupable pickupable = solidConduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable)
					{
						PrimaryElement component = pickupable.GetComponent<PrimaryElement>();
						float mass = component.Mass;
						if (mass > 0f)
						{
							text = string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), pickupable.GetProperName(), GameUtil.GetFormattedTemperature(component.Temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
							if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode == OverlayModes.Disease.ID && component.DiseaseIdx != 255)
							{
								text += string.Format(BUILDING.STATUSITEMS.CONVEYOR_CONTENTS.CONTENTS_WITH_DISEASE, GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, true));
							}
						}
					}
				}
				str = str.Replace("{Contents}", text);
				return str;
			};
			this.FabricatorIdle = this.CreateStatusItem("FabricatorIdle", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorEmpty = this.CreateStatusItem("FabricatorEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorLacksHEP = this.CreateStatusItem("FabricatorLacksHEP", "BUILDING", "status_item_need_high_energy_particles", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FabricatorLacksHEP.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = (ComplexFabricator)data;
				if (complexFabricator != null)
				{
					int num = complexFabricator.HighestHEPQueued();
					HighEnergyParticleStorage component = complexFabricator.GetComponent<HighEnergyParticleStorage>();
					str = str.Replace("{HEPRequired}", num.ToString());
					str = str.Replace("{CurrentHEP}", component.Particles.ToString());
				}
				return str;
			};
			this.FossilMineIdle = this.CreateStatusItem("FossilIdle", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FossilMineEmpty = this.CreateStatusItem("FossilEmpty", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FossilMinePendingWork = this.CreateStatusItem("FossilMinePendingWork", "CODEX.STORY_TRAITS.FOSSILHUNT", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.FossilEntombed = new StatusItem("FossilEntombed", "CODEX.STORY_TRAITS.FOSSILHUNT", "status_item_entombed", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.Toilet = this.CreateStatusItem("Toilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Toilet.resolveStringCallback = delegate(string str, object data)
			{
				Toilet.StatesInstance statesInstance = (Toilet.StatesInstance)data;
				if (statesInstance != null)
				{
					str = str.Replace("{FlushesRemaining}", statesInstance.GetFlushesRemaining().ToString());
				}
				return str;
			};
			this.ToiletNeedsEmptying = this.CreateStatusItem("ToiletNeedsEmptying", "BUILDING", "status_item_toilet_needs_emptying", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DesalinatorNeedsEmptying = this.CreateStatusItem("DesalinatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MilkSeparatorNeedsEmptying = this.CreateStatusItem("MilkSeparatorNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Unusable = this.CreateStatusItem("Unusable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.UnusableGunked = this.CreateStatusItem("UnusableGunked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoResearchSelected = this.CreateStatusItem("NoResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoResearchSelected.AddNotification(null, null, null);
			StatusItem noResearchSelected = this.NoResearchSelected;
			noResearchSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageResearch).mKeyCode.ToString();
				str = str.Replace("{RESEARCH_MENU_KEY}", newValue);
				return str;
			}));
			this.NoResearchSelected.notificationClickCallback = delegate(object d)
			{
				ManagementMenu.Instance.OpenResearch(null);
			};
			this.NoApplicableResearchSelected = this.CreateStatusItem("NoApplicableResearchSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoApplicableResearchSelected.AddNotification(null, null, null);
			this.NoApplicableAnalysisSelected = this.CreateStatusItem("NoApplicableAnalysisSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoApplicableAnalysisSelected.AddNotification(null, null, null);
			StatusItem noApplicableAnalysisSelected = this.NoApplicableAnalysisSelected;
			noApplicableAnalysisSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noApplicableAnalysisSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageStarmap).mKeyCode.ToString();
				str = str.Replace("{STARMAP_MENU_KEY}", newValue);
				return str;
			}));
			this.NoApplicableAnalysisSelected.notificationClickCallback = delegate(object d)
			{
				ManagementMenu.Instance.OpenStarmap();
			};
			this.NoResearchOrDestinationSelected = this.CreateStatusItem("NoResearchOrDestinationSelected", "BUILDING", "status_item_no_research_selected", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			StatusItem noResearchOrDestinationSelected = this.NoResearchOrDestinationSelected;
			noResearchOrDestinationSelected.resolveTooltipCallback = (Func<string, object, string>)Delegate.Combine(noResearchOrDestinationSelected.resolveTooltipCallback, new Func<string, object, string>(delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.ManageStarmap).mKeyCode.ToString();
				str = str.Replace("{STARMAP_MENU_KEY}", newValue);
				string newValue2 = GameInputMapping.FindEntry(global::Action.ManageResearch).mKeyCode.ToString();
				str = str.Replace("{RESEARCH_MENU_KEY}", newValue2);
				return str;
			}));
			this.NoResearchOrDestinationSelected.AddNotification(null, null, null);
			this.ValveRequest = this.CreateStatusItem("ValveRequest", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ValveRequest.resolveStringCallback = delegate(string str, object data)
			{
				Valve valve = (Valve)data;
				str = str.Replace("{QueuedMaxFlow}", GameUtil.GetFormattedMass(valve.QueuedMaxFlow, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingLight = this.CreateStatusItem("EmittingLight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingLight.resolveStringCallback = delegate(string str, object data)
			{
				string newValue = GameInputMapping.FindEntry(global::Action.Overlay5).mKeyCode.ToString();
				str = str.Replace("{LightGridOverlay}", newValue);
				return str;
			};
			this.KettleInsuficientSolids = this.CreateStatusItem("KettleInsuficientSolids", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.KettleInsuficientSolids.resolveStringCallback = delegate(string str, object data)
			{
				IceKettle.Instance instance = (IceKettle.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedMass(instance.def.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.KettleInsuficientFuel = this.CreateStatusItem("KettleInsuficientFuel", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.KettleInsuficientFuel.resolveStringCallback = delegate(string str, object data)
			{
				IceKettle.Instance instance = (IceKettle.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedMass(instance.FuelRequiredForNextBratch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.KettleInsuficientLiquidSpace = this.CreateStatusItem("KettleInsuficientLiquidSpace", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.KettleInsuficientLiquidSpace.resolveStringCallback = delegate(string str, object data)
			{
				IceKettle.Instance instance = (IceKettle.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedMass(instance.LiquidStored, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(instance.LiquidTankCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(instance.def.KGToMeltPerBatch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.KettleMelting = this.CreateStatusItem("KettleMelting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.KettleMelting.resolveStringCallback = delegate(string str, object data)
			{
				IceKettle.Instance instance = (IceKettle.Instance)data;
				str = string.Format(str, GameUtil.GetFormattedTemperature(instance.def.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.RationBoxContents = this.CreateStatusItem("RationBoxContents", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RationBoxContents.resolveStringCallback = delegate(string str, object data)
			{
				RationBox rationBox = (RationBox)data;
				if (rationBox == null)
				{
					return str;
				}
				Storage component = rationBox.GetComponent<Storage>();
				if (component == null)
				{
					return str;
				}
				float num = 0f;
				foreach (GameObject gameObject in component.items)
				{
					Edible component2 = gameObject.GetComponent<Edible>();
					if (component2)
					{
						num += component2.Calories;
					}
				}
				str = str.Replace("{Stored}", GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
				return str;
			};
			this.EmittingElement = this.CreateStatusItem("EmittingElement", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingElement.resolveStringCallback = delegate(string str, object data)
			{
				IElementEmitter elementEmitter = (IElementEmitter)data;
				string newValue = ElementLoader.FindElementByHash(elementEmitter.Element).tag.ProperName();
				str = str.Replace("{ElementType}", newValue);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.AverageEmitRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingOxygenAvg = this.CreateStatusItem("EmittingOxygenAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingOxygenAvg.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingGasAvg = this.CreateStatusItem("EmittingGasAvg", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingGasAvg.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(sublimates.AvgFlowRate(), GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.EmittingBlockedHighPressure = this.CreateStatusItem("EmittingBlockedHighPressure", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingBlockedHighPressure.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				return str;
			};
			this.EmittingBlockedLowTemperature = this.CreateStatusItem("EmittingBlockedLowTemperature", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.EmittingBlockedLowTemperature.resolveStringCallback = delegate(string str, object data)
			{
				Sublimates sublimates = (Sublimates)data;
				str = str.Replace("{Element}", ElementLoader.FindElementByHash(sublimates.info.sublimatedElement).name);
				return str;
			};
			this.PumpingLiquidOrGas = this.CreateStatusItem("PumpingLiquidOrGas", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.PumpingLiquidOrGas.resolveStringCallback = delegate(string str, object data)
			{
				HandleVector<int>.Handle handle = (HandleVector<int>.Handle)data;
				float averageRate = Game.Instance.accumulators.GetAverageRate(handle);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(averageRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.PipeMayMelt = this.CreateStatusItem("PipeMayMelt", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoLiquidElementToPump = this.CreateStatusItem("NoLiquidElementToPump", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.LiquidConduits.ID, true, 129022);
			this.NoGasElementToPump = this.CreateStatusItem("NoGasElementToPump", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.GasConduits.ID, true, 129022);
			this.NoFilterElementSelected = this.CreateStatusItem("NoFilterElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NoLureElementSelected = this.CreateStatusItem("NoLureElementSelected", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ElementConsumer = this.CreateStatusItem("ElementConsumer", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.ElementConsumer.resolveStringCallback = delegate(string str, object data)
			{
				ElementConsumer elementConsumer = (ElementConsumer)data;
				string newValue = ElementLoader.FindElementByHash(elementConsumer.elementToConsume).tag.ProperName();
				str = str.Replace("{ElementTypes}", newValue);
				str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementConsumer.AverageConsumeRate, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				return str;
			};
			this.ElementEmitterOutput = this.CreateStatusItem("ElementEmitterOutput", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.ElementEmitterOutput.resolveStringCallback = delegate(string str, object data)
			{
				ElementEmitter elementEmitter = (ElementEmitter)data;
				if (elementEmitter != null)
				{
					str = str.Replace("{ElementTypes}", elementEmitter.outputElement.Name);
					str = str.Replace("{FlowRate}", GameUtil.GetFormattedMass(elementEmitter.outputElement.massGenerationRate / elementEmitter.emissionFrequency, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				return str;
			};
			this.AwaitingWaste = this.CreateStatusItem("AwaitingWaste", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.AwaitingCompostFlip = this.CreateStatusItem("AwaitingCompostFlip", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, true, OverlayModes.None.ID, true, 129022);
			this.BatteryJoulesAvailable = this.CreateStatusItem("JoulesAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.BatteryJoulesAvailable.resolveStringCallback = delegate(string str, object data)
			{
				Battery battery = (Battery)data;
				str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(battery.JoulesAvailable, "F1", GameUtil.TimeSlice.None));
				str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(battery.Capacity, "F1", GameUtil.TimeSlice.None));
				return str;
			};
			this.ElectrobankJoulesAvailable = this.CreateStatusItem("ElectrobankJoulesAvailable", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ElectrobankJoulesAvailable.resolveStringCallback = delegate(string str, object data)
			{
				ElectrobankDischarger electrobankDischarger = (ElectrobankDischarger)data;
				str = str.Replace("{JoulesAvailable}", GameUtil.GetFormattedJoules(electrobankDischarger.ElectrobankJoulesStored, "F1", GameUtil.TimeSlice.None));
				str = str.Replace("{JoulesCapacity}", GameUtil.GetFormattedJoules(120000f, "F1", GameUtil.TimeSlice.None));
				return str;
			};
			this.Wattage = this.CreateStatusItem("Wattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.Wattage.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.SolarPanelWattage = this.CreateStatusItem("SolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.SolarPanelWattage.resolveStringCallback = delegate(string str, object data)
			{
				SolarPanel solarPanel = (SolarPanel)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(solarPanel.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.ModuleSolarPanelWattage = this.CreateStatusItem("ModuleSolarPanelWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleSolarPanelWattage.resolveStringCallback = delegate(string str, object data)
			{
				ModuleSolarPanel moduleSolarPanel = (ModuleSolarPanel)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(moduleSolarPanel.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.SteamTurbineWattage = this.CreateStatusItem("SteamTurbineWattage", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.SteamTurbineWattage.resolveStringCallback = delegate(string str, object data)
			{
				SteamTurbine steamTurbine = (SteamTurbine)data;
				str = str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.Wattson = this.CreateStatusItem("Wattson", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Wattson.resolveStringCallback = delegate(string str, object data)
			{
				Telepad telepad = (Telepad)data;
				if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
				{
					str = BUILDING.STATUSITEMS.WATTSONGAMEOVER.NAME;
				}
				else if (telepad.GetComponent<Operational>().IsOperational)
				{
					str = str.Replace("{TimeRemaining}", GameUtil.GetFormattedCycles(telepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					str = str.Replace("{TimeRemaining}", BUILDING.STATUSITEMS.WATTSON.UNAVAILABLE);
				}
				return str;
			};
			this.FlushToilet = this.CreateStatusItem("FlushToilet", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlushToilet.resolveStringCallback = delegate(string str, object data)
			{
				FlushToilet.SMInstance sminstance = (FlushToilet.SMInstance)data;
				return BUILDING.STATUSITEMS.FLUSHTOILET.NAME.Replace("{toilet}", sminstance.master.GetProperName());
			};
			this.FlushToilet.resolveTooltipCallback = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILET.TOOLTIP);
			this.FlushToiletInUse = this.CreateStatusItem("FlushToiletInUse", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlushToiletInUse.resolveStringCallback = delegate(string str, object data)
			{
				FlushToilet.SMInstance sminstance = (FlushToilet.SMInstance)data;
				return BUILDING.STATUSITEMS.FLUSHTOILETINUSE.NAME.Replace("{toilet}", sminstance.master.GetProperName());
			};
			this.FlushToiletInUse.resolveTooltipCallback = ((string str, object Database) => BUILDING.STATUSITEMS.FLUSHTOILETINUSE.TOOLTIP);
			this.WireNominal = this.CreateStatusItem("WireNominal", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.WireConnected = this.CreateStatusItem("WireConnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.WireDisconnected = this.CreateStatusItem("WireDisconnected", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022);
			this.Overheated = this.CreateStatusItem("Overheated", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.Overloaded = this.CreateStatusItem("Overloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.LogicOverloaded = this.CreateStatusItem("LogicOverloaded", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.Bad, false, OverlayModes.None.ID, true, 129022);
			this.Cooling = this.CreateStatusItem("Cooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			Func<string, object, string> resolveStringCallback2 = delegate(string str, object data)
			{
				AirConditioner airConditioner = (AirConditioner)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.CoolingStalledColdGas = this.CreateStatusItem("CoolingStalledColdGas", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledColdGas.resolveStringCallback = resolveStringCallback2;
			this.CoolingStalledColdLiquid = this.CreateStatusItem("CoolingStalledColdLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledColdLiquid.resolveStringCallback = resolveStringCallback2;
			Func<string, object, string> resolveStringCallback3 = delegate(string str, object data)
			{
				AirConditioner airConditioner = (AirConditioner)data;
				return string.Format(str, GameUtil.GetFormattedTemperature(airConditioner.lastEnvTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(airConditioner.lastGasTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(airConditioner.maxEnvironmentDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
			};
			this.CoolingStalledHotEnv = this.CreateStatusItem("CoolingStalledHotEnv", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledHotEnv.resolveStringCallback = resolveStringCallback3;
			this.CoolingStalledHotLiquid = this.CreateStatusItem("CoolingStalledHotLiquid", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.CoolingStalledHotLiquid.resolveStringCallback = resolveStringCallback3;
			this.MissingRequirements = this.CreateStatusItem("MissingRequirements", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.GettingReady = this.CreateStatusItem("GettingReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Working = this.CreateStatusItem("Working", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NeedsValidRegion = this.CreateStatusItem("NeedsValidRegion", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NeedSeed = this.CreateStatusItem("NeedSeed", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingSeedDelivery = this.CreateStatusItem("AwaitingSeedDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingBaitDelivery = this.CreateStatusItem("AwaitingBaitDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoAvailableSeed = this.CreateStatusItem("NoAvailableSeed", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.NeedEgg = this.CreateStatusItem("NeedEgg", "BUILDING", "status_item_fabricator_empty", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingEggDelivery = this.CreateStatusItem("AwaitingEggDelivery", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoAvailableEgg = this.CreateStatusItem("NoAvailableEgg", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.Grave = this.CreateStatusItem("Grave", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Grave.resolveStringCallback = delegate(string str, object data)
			{
				Grave.StatesInstance statesInstance = (Grave.StatesInstance)data;
				string text = str.Replace("{DeadDupe}", statesInstance.master.graveName);
				string[] strings = LocString.GetStrings(typeof(NAMEGEN.GRAVE.EPITAPHS));
				int num = statesInstance.master.epitaphIdx % strings.Length;
				return text.Replace("{Epitaph}", strings[num]);
			};
			this.GraveEmpty = this.CreateStatusItem("GraveEmpty", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CannotCoolFurther = this.CreateStatusItem("CannotCoolFurther", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CannotCoolFurther.resolveTooltipCallback = delegate(string str, object data)
			{
				float temp = (float)data;
				return str.Replace("{0}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.BuildingDisabled = this.CreateStatusItem("BuildingDisabled", "BUILDING", "status_item_building_disabled", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.Expired = this.CreateStatusItem("Expired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PumpingStation = this.CreateStatusItem("PumpingStation", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PumpingStation.resolveStringCallback = delegate(string str, object data)
			{
				LiquidPumpingStation liquidPumpingStation = (LiquidPumpingStation)data;
				if (liquidPumpingStation != null)
				{
					return liquidPumpingStation.ResolveString(str);
				}
				return str;
			};
			this.EmptyPumpingStation = this.CreateStatusItem("EmptyPumpingStation", "BUILDING", "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WellPressurizing = this.CreateStatusItem("WellPressurizing", BUILDING.STATUSITEMS.WELL_PRESSURIZING.NAME, BUILDING.STATUSITEMS.WELL_PRESSURIZING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.WellPressurizing.resolveStringCallback = delegate(string str, object data)
			{
				OilWellCap.StatesInstance statesInstance = (OilWellCap.StatesInstance)data;
				if (statesInstance != null)
				{
					return string.Format(str, GameUtil.GetFormattedPercent(100f * statesInstance.GetPressurePercent(), GameUtil.TimeSlice.None));
				}
				return str;
			};
			this.WellOverpressure = this.CreateStatusItem("WellOverpressure", BUILDING.STATUSITEMS.WELL_OVERPRESSURE.NAME, BUILDING.STATUSITEMS.WELL_OVERPRESSURE.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.ReleasingPressure = this.CreateStatusItem("ReleasingPressure", BUILDING.STATUSITEMS.RELEASING_PRESSURE.NAME, BUILDING.STATUSITEMS.RELEASING_PRESSURE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.ReactorMeltdown = this.CreateStatusItem("ReactorMeltdown", BUILDING.STATUSITEMS.REACTORMELTDOWN.NAME, BUILDING.STATUSITEMS.REACTORMELTDOWN.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
			this.TooCold = this.CreateStatusItem("TooCold", BUILDING.STATUSITEMS.TOO_COLD.NAME, BUILDING.STATUSITEMS.TOO_COLD.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.IncubatorProgress = this.CreateStatusItem("IncubatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.IncubatorProgress.resolveStringCallback = delegate(string str, object data)
			{
				EggIncubator eggIncubator = (EggIncubator)data;
				str = str.Replace("{Percent}", GameUtil.GetFormattedPercent(eggIncubator.GetProgress() * 100f, GameUtil.TimeSlice.None));
				return str;
			};
			this.HabitatNeedsEmptying = this.CreateStatusItem("HabitatNeedsEmptying", "BUILDING", "status_item_need_supply_out", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DetectorScanning = this.CreateStatusItem("DetectorScanning", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.IncomingMeteors = this.CreateStatusItem("IncomingMeteors", "BUILDING", "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.HasGantry = this.CreateStatusItem("HasGantry", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissingGantry = this.CreateStatusItem("MissingGantry", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DisembarkingDuplicant = this.CreateStatusItem("DisembarkingDuplicant", "BUILDING", "status_item_new_duplicants_available", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketName = this.CreateStatusItem("RocketName", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketName.resolveStringCallback = delegate(string str, object data)
			{
				RocketModule rocketModule = (RocketModule)data;
				if (rocketModule != null)
				{
					return str.Replace("{0}", rocketModule.GetParentRocketName());
				}
				return str;
			};
			this.RocketName.resolveTooltipCallback = delegate(string str, object data)
			{
				RocketModule rocketModule = (RocketModule)data;
				if (rocketModule != null)
				{
					return str.Replace("{0}", rocketModule.GetParentRocketName());
				}
				return str;
			};
			this.LandedRocketLacksPassengerModule = this.CreateStatusItem("LandedRocketLacksPassengerModule", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.PathNotClear = new StatusItem("PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.PathNotClear.resolveTooltipCallback = delegate(string str, object data)
			{
				ConditionFlightPathIsClear conditionFlightPathIsClear = (ConditionFlightPathIsClear)data;
				if (conditionFlightPathIsClear != null)
				{
					str = string.Format(str, conditionFlightPathIsClear.GetObstruction());
				}
				return str;
			};
			this.InvalidPortOverlap = this.CreateStatusItem("InvalidPortOverlap", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InvalidPortOverlap.AddNotification(null, null, null);
			this.EmergencyPriority = this.CreateStatusItem("EmergencyPriority", BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.TOOLTIP, "status_item_doubleexclamation", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
			this.EmergencyPriority.AddNotification(null, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_NAME, BUILDING.STATUSITEMS.TOP_PRIORITY_CHORE.NOTIFICATION_TOOLTIP);
			this.SkillPointsAvailable = this.CreateStatusItem("SkillPointsAvailable", BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.NAME, BUILDING.STATUSITEMS.SKILL_POINTS_AVAILABLE.TOOLTIP, "status_item_jobs", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.Baited = this.CreateStatusItem("Baited", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.Baited.resolveStringCallback = delegate(string str, object data)
			{
				Element element = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
				str = str.Replace("{0}", element.name);
				return str;
			};
			this.Baited.resolveTooltipCallback = delegate(string str, object data)
			{
				Element element = ElementLoader.FindElementByName(((CreatureBait.StatesInstance)data).master.baitElement.ToString());
				str = str.Replace("{0}", element.name);
				return str;
			};
			this.TanningLightSufficient = this.CreateStatusItem("TanningLightSufficient", BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.TanningLightInsufficient = this.CreateStatusItem("TanningLightInsufficient", BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.NAME, BUILDING.STATUSITEMS.TANNINGLIGHTINSUFFICIENT.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.HotTubWaterTooCold = this.CreateStatusItem("HotTubWaterTooCold", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.HotTubWaterTooCold.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.HotTubTooHot = this.CreateStatusItem("HotTubTooHot", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022);
			this.HotTubTooHot.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{temperature}", GameUtil.GetFormattedTemperature(hotTub.maxOperatingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				return str;
			};
			this.HotTubFilling = this.CreateStatusItem("HotTubFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.HotTubFilling.resolveStringCallback = delegate(string str, object data)
			{
				HotTub hotTub = (HotTub)data;
				str = str.Replace("{fullness}", GameUtil.GetFormattedPercent(hotTub.PercentFull, GameUtil.TimeSlice.None));
				return str;
			};
			this.WindTunnelIntake = this.CreateStatusItem("WindTunnelIntake", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WarpPortalCharging = this.CreateStatusItem("WarpPortalCharging", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022);
			this.WarpPortalCharging.resolveStringCallback = delegate(string str, object data)
			{
				WarpPortal warpPortal = (WarpPortal)data;
				str = str.Replace("{charge}", GameUtil.GetFormattedPercent(100f * (((WarpPortal)data).rechargeProgress / 3000f), GameUtil.TimeSlice.None));
				return str;
			};
			this.WarpPortalCharging.resolveTooltipCallback = delegate(string str, object data)
			{
				WarpPortal warpPortal = (WarpPortal)data;
				str = str.Replace("{cycles}", string.Format("{0:0.0}", (3000f - ((WarpPortal)data).rechargeProgress) / 600f));
				return str;
			};
			this.WarpConduitPartnerDisabled = this.CreateStatusItem("WarpConduitPartnerDisabled", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.WarpConduitPartnerDisabled.resolveStringCallback = ((string str, object data) => str.Replace("{x}", data.ToString()));
			this.CollectingHEP = this.CreateStatusItem("CollectingHEP", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.Radiation.ID, false, 129022);
			this.CollectingHEP.resolveStringCallback = ((string str, object data) => str.Replace("{x}", ((HighEnergyParticleSpawner)data).PredictedPerCycleConsumptionRate.ToString()));
			this.InOrbit = this.CreateStatusItem("InOrbit", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InOrbit.resolveStringCallback = delegate(string str, object data)
			{
				ClusterGridEntity clusterGridEntity = (ClusterGridEntity)data;
				return str.Replace("{Destination}", clusterGridEntity.Name);
			};
			this.WaitingToLand = this.CreateStatusItem("WaitingToLand", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.WaitingToLand.resolveStringCallback = delegate(string str, object data)
			{
				ClusterGridEntity clusterGridEntity = (ClusterGridEntity)data;
				return str.Replace("{Destination}", clusterGridEntity.Name);
			};
			this.InFlight = this.CreateStatusItem("InFlight", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlight.resolveStringCallback = delegate(string str, object data)
			{
				ClusterTraveler clusterTraveler = (ClusterTraveler)data;
				ClusterDestinationSelector component = clusterTraveler.GetComponent<ClusterDestinationSelector>();
				RocketClusterDestinationSelector rocketClusterDestinationSelector = component as RocketClusterDestinationSelector;
				Sprite sprite;
				string newValue;
				string text;
				ClusterGrid.Instance.GetLocationDescription(component.GetDestination(), out sprite, out newValue, out text);
				if (rocketClusterDestinationSelector != null)
				{
					LaunchPad destinationPad = rocketClusterDestinationSelector.GetDestinationPad();
					string newValue2 = (destinationPad != null) ? destinationPad.GetProperName() : UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE.ToString();
					return str.Replace("{Destination_Asteroid}", newValue).Replace("{Destination_Pad}", newValue2).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler.TravelETA(), "F1", false));
				}
				return str.Replace("{Destination_Asteroid}", newValue).Replace("{ETA}", GameUtil.GetFormattedCycles(clusterTraveler.TravelETA(), "F1", false));
			};
			this.DestinationOutOfRange = this.CreateStatusItem("DestinationOutOfRange", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.DestinationOutOfRange.resolveStringCallback = delegate(string str, object data)
			{
				ClusterTraveler clusterTraveler = (ClusterTraveler)data;
				str = str.Replace("{Range}", GameUtil.GetFormattedRocketRange(clusterTraveler.GetComponent<CraftModuleInterface>().RangeInTiles, false));
				return str.Replace("{Distance}", clusterTraveler.RemainingTravelNodes().ToString() + " " + UI.CLUSTERMAP.TILES);
			};
			this.RocketStranded = this.CreateStatusItem("RocketStranded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlAssistingRocket = this.CreateStatusItem("MissionControlAssistingRocket", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlAssistingRocket.resolveStringCallback = delegate(string str, object data)
			{
				Spacecraft spacecraft = data as Spacecraft;
				Clustercraft clustercraft = data as Clustercraft;
				return str.Replace("{0}", (spacecraft != null) ? spacecraft.rocketName : clustercraft.Name);
			};
			this.NoRocketsToMissionControlBoost = this.CreateStatusItem("NoRocketsToMissionControlBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoRocketsToMissionControlClusterBoost = this.CreateStatusItem("NoRocketsToMissionControlClusterBoost", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.NoRocketsToMissionControlClusterBoost.resolveStringCallback = delegate(string str, object data)
			{
				if (str.Contains("{0}"))
				{
					str = str.Replace("{0}", 2.ToString());
				}
				return str;
			};
			this.MissionControlBoosted = this.CreateStatusItem("MissionControlBoosted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MissionControlBoosted.resolveStringCallback = delegate(string str, object data)
			{
				Spacecraft spacecraft = data as Spacecraft;
				Clustercraft clustercraft = data as Clustercraft;
				str = str.Replace("{0}", GameUtil.GetFormattedPercent(20.000004f, GameUtil.TimeSlice.None));
				if (str.Contains("{1}"))
				{
					str = str.Replace("{1}", GameUtil.GetFormattedTime((spacecraft != null) ? spacecraft.controlStationBuffTimeRemaining : clustercraft.controlStationBuffTimeRemaining, "F0"));
				}
				return str;
			};
			this.TransitTubeEntranceWaxReady = this.CreateStatusItem("TransitTubeEntranceWaxReady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TransitTubeEntranceWaxReady.resolveStringCallback = delegate(string str, object data)
			{
				TravelTubeEntrance travelTubeEntrance = data as TravelTubeEntrance;
				str = str.Replace("{0}", GameUtil.GetFormattedMass(travelTubeEntrance.waxPerLaunch, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				str = str.Replace("{1}", travelTubeEntrance.WaxLaunchesAvailable.ToString());
				return str;
			};
			this.SpecialCargoBayClusterCritterStored = this.CreateStatusItem("SpecialCargoBayClusterCritterStored", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpecialCargoBayClusterCritterStored.resolveStringCallback = delegate(string str, object data)
			{
				SpecialCargoBayClusterReceptacle specialCargoBayClusterReceptacle = data as SpecialCargoBayClusterReceptacle;
				if (specialCargoBayClusterReceptacle.Occupant != null)
				{
					str = str.Replace("{0}", specialCargoBayClusterReceptacle.Occupant.GetProperName());
				}
				return str;
			};
			this.RailgunpayloadNeedsEmptying = this.CreateStatusItem("RailgunpayloadNeedsEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.AwaitingEmptyBuilding = this.CreateStatusItem("AwaitingEmptyBuilding", "BUILDING", "action_empty_contents", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.DuplicantActivationRequired = this.CreateStatusItem("DuplicantActivationRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketChecklistIncomplete = this.CreateStatusItem("RocketChecklistIncomplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoEmptying = this.CreateStatusItem("RocketCargoEmptying", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoFilling = this.CreateStatusItem("RocketCargoFilling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketCargoFull = this.CreateStatusItem("RocketCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightAllCargoFull = this.CreateStatusItem("FlightAllCargoFull", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightCargoRemaining = this.CreateStatusItem("FlightCargoRemaining", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FlightCargoRemaining.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return str.Replace("{0}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.PilotNeeded = this.CreateStatusItem("PilotNeeded", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.PilotNeeded.resolveStringCallback = delegate(string str, object data)
			{
				RocketControlStation master = ((RocketControlStation.StatesInstance)data).master;
				return str.Replace("{timeRemaining}", GameUtil.GetFormattedTime(master.TimeRemaining, "F0"));
			};
			this.AutoPilotActive = this.CreateStatusItem("AutoPilotActive", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.InFlightPiloted = this.CreateStatusItem("InFlightPiloted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlightPiloted.resolveTooltipCallback = delegate(string str, object data)
			{
				Clustercraft clustercraft = (Clustercraft)data;
				bool flag;
				clustercraft.ModuleInterface.GetPrimaryPilotModule(out flag);
				if (!flag)
				{
					str = string.Format(BUILDING.STATUSITEMS.INFLIGHTPILOTED.DUPE_TOOLTIP, GameUtil.GetFormattedPercent((clustercraft.PilotSkillMultiplier - 1f) * 100f, GameUtil.TimeSlice.None));
				}
				else
				{
					str = BUILDING.STATUSITEMS.INFLIGHTPILOTED.ROBO_TOOLTIP;
				}
				return str;
			};
			this.InFlightUnpiloted = this.CreateStatusItem("InFlightUnpiloted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlightUnpiloted.resolveTooltipCallback = delegate(string str, object data)
			{
				Clustercraft clustercraft = (Clustercraft)data;
				PassengerRocketModule passengerModule = clustercraft.ModuleInterface.GetPassengerModule();
				RoboPilotModule robotPilotModule = clustercraft.ModuleInterface.GetRobotPilotModule();
				string text = "";
				if (passengerModule != null)
				{
					text = text + "\n    • " + passengerModule.GetProperName();
				}
				if (robotPilotModule != null)
				{
					if (passengerModule == null)
					{
						return BUILDING.STATUSITEMS.INFLIGHTUNPILOTED.ROBO_PILOT_ONLY_TOOLTIP;
					}
					text = text + "\n    • " + robotPilotModule.GetProperName();
				}
				return str.Replace("{penalty}", GameUtil.GetFormattedPercent(50f, GameUtil.TimeSlice.None)).Replace("{modules}", text);
			};
			this.InFlightAutoPiloted = this.CreateStatusItem("InFlightAutoPiloted", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlightAutoPiloted.resolveTooltipCallback = delegate(string str, object data)
			{
				PassengerRocketModule passengerModule = ((Clustercraft)data).ModuleInterface.GetPassengerModule();
				if (passengerModule != null)
				{
					str = str.Replace("{penalty}", GameUtil.GetFormattedPercent(50f, GameUtil.TimeSlice.None)).Replace("{modules}", passengerModule.GetProperName());
				}
				DebugUtil.DevAssert(passengerModule != null, "Auto pilot should never engage if there is no passenger module", null);
				return str;
			};
			this.InFlightSuperPilot = this.CreateStatusItem("InFlightSuperPilot", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.InFlightSuperPilot.resolveTooltipCallback = delegate(string str, object data)
			{
				Clustercraft clustercraft = (Clustercraft)data;
				str = string.Format(str, GameUtil.GetFormattedPercent((clustercraft.PilotSkillMultiplier - 1f) * 100f, GameUtil.TimeSlice.None), GameUtil.GetFormattedPercent(50f, GameUtil.TimeSlice.None));
				return str;
			};
			this.InvalidMaskStationConsumptionState = this.CreateStatusItem("InvalidMaskStationConsumptionState", "BUILDING", "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ClusterTelescopeAllWorkComplete = this.CreateStatusItem("ClusterTelescopeAllWorkComplete", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RocketPlatformCloseToCeiling = this.CreateStatusItem("RocketPlatformCloseToCeiling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.RocketPlatformCloseToCeiling.resolveStringCallback = ((string str, object data) => str.Replace("{distance}", data.ToString()));
			this.ModuleGeneratorPowered = this.CreateStatusItem("ModuleGeneratorPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleGeneratorPowered.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.ModuleGeneratorNotPowered = this.CreateStatusItem("ModuleGeneratorNotPowered", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022);
			this.ModuleGeneratorNotPowered.resolveStringCallback = delegate(string str, object data)
			{
				Generator generator = (Generator)data;
				str = str.Replace("{ActiveWattage}", GameUtil.GetFormattedWattage(0f, GameUtil.WattageFormatterUnit.Automatic, true));
				str = str.Replace("{MaxWattage}", GameUtil.GetFormattedWattage(generator.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.InOrbitRequired = this.CreateStatusItem("InOrbitRequired", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.ReactorRefuelDisabled = this.CreateStatusItem("ReactorRefuelDisabled", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeCooling = this.CreateStatusItem("FridgeCooling", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeCooling.resolveStringCallback = delegate(string str, object data)
			{
				RefrigeratorController.StatesInstance statesInstance = (RefrigeratorController.StatesInstance)data;
				str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.FridgeSteady = this.CreateStatusItem("FridgeSteady", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.FridgeSteady.resolveStringCallback = delegate(string str, object data)
			{
				RefrigeratorController.StatesInstance statesInstance = (RefrigeratorController.StatesInstance)data;
				str = str.Replace("{UsedPower}", GameUtil.GetFormattedWattage(statesInstance.GetSaverPower(), GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{MaxPower}", GameUtil.GetFormattedWattage(statesInstance.GetNormalPower(), GameUtil.WattageFormatterUnit.Automatic, true));
				return str;
			};
			this.TrapNeedsArming = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.NEEDS_ARMING", "BUILDING", "status_item_bait", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.TrapArmed = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.READY", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TrapHasCritter = this.CreateStatusItem("CREATURE_REUSABLE_TRAP.SPRUNG", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.TrapHasCritter.resolveTooltipCallback = delegate(string str, object data)
			{
				string newValue = "";
				if (data != null)
				{
					newValue = ((GameObject)data).GetComponent<KPrefabID>().GetProperName();
				}
				str = str.Replace("{0}", newValue);
				return str;
			};
			this.RailGunCooldown = this.CreateStatusItem("RailGunCooldown", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.RailGunCooldown.resolveStringCallback = delegate(string str, object data)
			{
				RailGun.StatesInstance statesInstance = (RailGun.StatesInstance)data;
				str = str.Replace("{timeleft}", GameUtil.GetFormattedTime(statesInstance.sm.cooldownTimer.Get(statesInstance), "F0"));
				return str;
			};
			this.RailGunCooldown.resolveTooltipCallback = delegate(string str, object data)
			{
				RailGun.StatesInstance statesInstance = (RailGun.StatesInstance)data;
				str = str.Replace("{x}", 6.ToString());
				return str;
			};
			this.NoSurfaceSight = new StatusItem("NOSURFACESIGHT", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.LimitValveLimitReached = this.CreateStatusItem("LimitValveLimitReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LimitValveLimitNotReached = this.CreateStatusItem("LimitValveLimitNotReached", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.LimitValveLimitNotReached.resolveStringCallback = delegate(string str, object data)
			{
				LimitValve limitValve = (LimitValve)data;
				string arg;
				if (limitValve.displayUnitsInsteadOfMass)
				{
					arg = GameUtil.GetFormattedUnits(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, true, LimitValveSideScreen.FLOAT_FORMAT);
				}
				else
				{
					arg = GameUtil.GetFormattedMass(limitValve.RemainingCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, LimitValveSideScreen.FLOAT_FORMAT);
				}
				return string.Format(BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.NAME, arg);
			};
			this.LimitValveLimitNotReached.resolveTooltipCallback = ((string str, object data) => BUILDING.STATUSITEMS.LIMITVALVELIMITNOTREACHED.TOOLTIP);
			this.SpacePOIHarvesting = this.CreateStatusItem("SpacePOIHarvesting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.SpacePOIHarvesting.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return string.Format(BUILDING.STATUSITEMS.SPACEPOIHARVESTING.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.SpacePOIWasting = this.CreateStatusItem("SpacePOIWasting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.SpacePOIWasting.resolveStringCallback = delegate(string str, object data)
			{
				float mass = (float)data;
				return string.Format(BUILDING.STATUSITEMS.SPACEPOIWASTING.NAME, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.RocketRestrictionActive = new StatusItem("ROCKETRESTRICTIONACTIVE", "BUILDING", "status_item_rocket_restricted", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.RocketRestrictionInactive = new StatusItem("ROCKETRESTRICTIONINACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.NoRocketRestriction = new StatusItem("NOROCKETRESTRICTION", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			this.BroadcasterOutOfRange = new StatusItem("BROADCASTEROUTOFRANGE", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.LosingRadbolts = new StatusItem("LOSINGRADBOLTS", "BUILDING", "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.FabricatorAcceptsMutantSeeds = new StatusItem("FABRICATORACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.NoSpiceSelected = new StatusItem("SPICEGRINDERNOSPICE", "BUILDING", "status_item_no_filter_set", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.GeoTunerNoGeyserSelected = new StatusItem("GEOTUNER_NEEDGEYSER", "BUILDING", "status_item_fabricator_select", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			this.GeoTunerResearchNeeded = new StatusItem("GEOTUNER_CHARGE_REQUIRED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerResearchInProgress = new StatusItem("GEOTUNER_CHARGING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerBroadcasting = new StatusItem("GEOTUNER_CHARGED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerBroadcasting.resolveStringCallback = delegate(string str, object data)
			{
				GeoTuner.Instance instance = (GeoTuner.Instance)data;
				str = str.Replace("{0}", ((float)Mathf.CeilToInt(instance.sm.expirationTimer.Get(instance) / instance.enhancementDuration * 100f)).ToString() + "%");
				return str;
			};
			this.GeoTunerBroadcasting.resolveTooltipCallback = delegate(string str, object data)
			{
				GeoTuner.Instance instance = (GeoTuner.Instance)data;
				float seconds = instance.sm.expirationTimer.Get(instance);
				float num = 100f / instance.enhancementDuration;
				str = str.Replace("{0}", GameUtil.GetFormattedTime(seconds, "F0"));
				str = str.Replace("{1}", "-" + num.ToString("0.00") + "%");
				return str;
			};
			this.GeoTunerGeyserStatus = new StatusItem("GEOTUNER_GEYSER_STATUS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeoTunerGeyserStatus.resolveStringCallback = delegate(string str, object data)
			{
				Geyser assignedGeyser = ((GeoTuner.Instance)data).GetAssignedGeyser();
				bool flag = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt;
				bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.dormant;
				if (!flag2)
				{
					bool flag3 = !flag;
				}
				return flag ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_ERUPTING : (flag2 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.NAME_IDLE);
			};
			this.GeoTunerGeyserStatus.resolveTooltipCallback = delegate(string str, object data)
			{
				Geyser assignedGeyser = ((GeoTuner.Instance)data).GetAssignedGeyser();
				if (assignedGeyser != null)
				{
					assignedGeyser.gameObject.GetProperName();
				}
				bool flag = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() != null && assignedGeyser.smi.GetCurrentState().parent == assignedGeyser.smi.sm.erupt;
				bool flag2 = assignedGeyser != null && assignedGeyser.smi.GetCurrentState() == assignedGeyser.smi.sm.dormant;
				if (!flag2)
				{
					bool flag3 = !flag;
				}
				return flag ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_ERUPTING : (flag2 ? BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_DORMANT : BUILDING.STATUSITEMS.GEOTUNER_GEYSER_STATUS.TOOLTIP_IDLE);
			};
			this.GeyserGeotuned = new StatusItem("GEYSER_GEOTUNED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			this.GeyserGeotuned.resolveStringCallback = delegate(string str, object data)
			{
				Geyser geyser = (Geyser)data;
				int num = 0;
				int num2 = Components.GeoTuners.GetItems(geyser.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser);
				for (int i = 0; i < geyser.modifications.Count; i++)
				{
					if (geyser.modifications[i].originID.Contains("GeoTuner"))
					{
						num++;
					}
				}
				str = str.Replace("{0}", num.ToString());
				str = str.Replace("{1}", num2.ToString());
				return str;
			};
			this.GeyserGeotuned.resolveTooltipCallback = delegate(string str, object data)
			{
				Geyser geyser = (Geyser)data;
				int num = 0;
				int num2 = Components.GeoTuners.GetItems(geyser.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetAssignedGeyser() == geyser);
				for (int i = 0; i < geyser.modifications.Count; i++)
				{
					if (geyser.modifications[i].originID.Contains("GeoTuner"))
					{
						num++;
					}
				}
				str = str.Replace("{0}", num.ToString());
				str = str.Replace("{1}", num2.ToString());
				return str;
			};
			this.SkyVisNone = new StatusItem("SkyVisNone", BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_NONE.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, new Func<string, object, string>(BuildingStatusItems.<CreateStatusItems>g__SkyVisResolveStringCallback|322_117));
			this.SkyVisLimited = new StatusItem("SkyVisLimited", BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.NAME, BUILDING.STATUSITEMS.SPACE_VISIBILITY_REDUCED.TOOLTIP, "status_item_no_sky", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, false, new Func<string, object, string>(BuildingStatusItems.<CreateStatusItems>g__SkyVisResolveStringCallback|322_117));
			this.CreatureManipulatorWaiting = this.CreateStatusItem("CreatureManipulatorWaiting", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorProgress = this.CreateStatusItem("CreatureManipulatorProgress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorProgress.resolveStringCallback = delegate(string str, object data)
			{
				GravitasCreatureManipulator.Instance instance = (GravitasCreatureManipulator.Instance)data;
				return string.Format(str, instance.ScannedSpecies.Count, instance.def.numSpeciesToUnlockMorphMode);
			};
			this.CreatureManipulatorProgress.resolveTooltipCallback = delegate(string str, object data)
			{
				GravitasCreatureManipulator.Instance instance = (GravitasCreatureManipulator.Instance)data;
				if (instance.ScannedSpecies.Count == 0)
				{
					str = str + "\n • " + BUILDING.STATUSITEMS.CREATUREMANIPULATORPROGRESS.NO_DATA;
				}
				else
				{
					foreach (Tag tag in instance.ScannedSpecies)
					{
						str = str + "\n • " + Strings.Get("STRINGS.CREATURES.FAMILY_PLURAL." + tag.ToString().ToUpper());
					}
				}
				return str;
			};
			this.CreatureManipulatorMorphModeLocked = this.CreateStatusItem("CreatureManipulatorMorphModeLocked", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorMorphMode = this.CreateStatusItem("CreatureManipulatorMorphMode", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.CreatureManipulatorWorking = this.CreateStatusItem("CreatureManipulatorWorking", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MegaBrainTankActivationProgress = this.CreateStatusItem("MegaBrainTankActivationProgress", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.PROGRESSIONRATE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MegaBrainNotEnoughOxygen = this.CreateStatusItem("MegaBrainNotEnoughOxygen", "BUILDING", "status_item_suit_locker_no_oxygen", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022);
			this.MegaBrainTankActivationProgress.resolveStringCallback = delegate(string str, object data)
			{
				MegaBrainTank.StatesInstance statesInstance = (MegaBrainTank.StatesInstance)data;
				return str.Replace("{ActivationProgress}", string.Format("{0}/{1}", statesInstance.ActivationProgress, 25));
			};
			this.MegaBrainTankDreamAnalysis = this.CreateStatusItem("MegaBrainTankDreamAnalysis", BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.PROGRESS.DREAMANALYSIS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MegaBrainTankDreamAnalysis.resolveStringCallback = delegate(string str, object data)
			{
				MegaBrainTank.StatesInstance statesInstance = (MegaBrainTank.StatesInstance)data;
				return str.Replace("{TimeToComplete}", statesInstance.TimeTilDigested.ToString());
			};
			this.MegaBrainTankComplete = this.CreateStatusItem("MegaBrainTankComplete", BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.NAME, BUILDING.STATUSITEMS.MEGABRAINTANK.COMPLETE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.FossilHuntExcavationOrdered = this.CreateStatusItem("FossilHuntExcavationOrdered", BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.PENDING_EXCAVATION.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.FossilHuntExcavationInProgress = this.CreateStatusItem("FossilHuntExcavationInProgress", BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.NAME, BUILDING.STATUSITEMS.FOSSILHUNT.EXCAVATING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022);
			this.ComplexFabricatorCooking = this.CreateStatusItem("COMPLEXFABRICATOR.COOKING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorCooking.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = data as ComplexFabricator;
				if (complexFabricator != null && complexFabricator.CurrentWorkingOrder != null)
				{
					str = str.Replace("{Item}", complexFabricator.CurrentWorkingOrder.FirstResult.ProperName());
				}
				return str;
			};
			this.ComplexFabricatorProducing = this.CreateStatusItem("COMPLEXFABRICATOR.PRODUCING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorProducing.resolveStringCallback = delegate(string str, object data)
			{
				ComplexFabricator complexFabricator = data as ComplexFabricator;
				if (complexFabricator != null)
				{
					if (complexFabricator.CurrentWorkingOrder != null)
					{
						string newValue = complexFabricator.CurrentWorkingOrder.results[0].facadeID.IsNullOrWhiteSpace() ? complexFabricator.CurrentWorkingOrder.FirstResult.ProperName() : complexFabricator.CurrentWorkingOrder.results[0].facadeID.ProperName();
						str = str.Replace("{Item}", newValue);
					}
					return str;
				}
				TinkerStation tinkerStation = data as TinkerStation;
				if (tinkerStation != null)
				{
					str = str.Replace("{Item}", tinkerStation.outputPrefab.ProperName());
				}
				return str;
			};
			this.ComplexFabricatorResearching = this.CreateStatusItem("COMPLEXFABRICATOR.RESEARCHING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorResearching.resolveStringCallback = delegate(string str, object data)
			{
				if (data is IResearchCenter)
				{
					TechInstance activeResearch = Research.Instance.GetActiveResearch();
					if (activeResearch != null)
					{
						str = str.Replace("{Item}", activeResearch.tech.Name);
						return str;
					}
				}
				str = str.Replace("{Item}", (data as GameObject).GetProperName());
				return str;
			};
			this.ArtifactAnalysisAnalyzing = this.CreateStatusItem("COMPLEXFABRICATOR.ANALYZING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ArtifactAnalysisAnalyzing.resolveStringCallback = delegate(string str, object data)
			{
				if (data as GameObject != null)
				{
					str = str.Replace("{Item}", (data as GameObject).GetProperName());
				}
				return str;
			};
			this.ComplexFabricatorTraining = this.CreateStatusItem("COMPLEXFABRICATOR.UNTRAINING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ComplexFabricatorTraining.resolveStringCallback = delegate(string str, object data)
			{
				ResetSkillsStation resetSkillsStation = data as ResetSkillsStation;
				if (resetSkillsStation != null && resetSkillsStation.assignable.assignee != null)
				{
					str = str.Replace("{Duplicant}", resetSkillsStation.assignable.assignee.GetProperName());
				}
				return str;
			};
			this.TelescopeWorking = this.CreateStatusItem("COMPLEXFABRICATOR.TELESCOPE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.ClusterTelescopeMeteorWorking = this.CreateStatusItem("COMPLEXFABRICATOR.CLUSTERTELESCOPEMETEOR", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
			this.MorbRoverMakerDusty = this.CreateStatusItem("MorbRoverMakerDusty", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DUSTY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerBuildingRevealed = this.CreateStatusItem("MorbRoverMakerBuildingRevealed", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_REVEALED.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_REVEALED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerGermCollectionProgress = this.CreateStatusItem("MorbRoverMakerGermCollectionProgress", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.GERM_COLLECTION_PROGRESS.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.GERM_COLLECTION_PROGRESS.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerGermCollectionProgress.resolveStringCallback = delegate(string str, object data)
			{
				MorbRoverMaker.Instance instance = (MorbRoverMaker.Instance)data;
				return str.Replace("{0}", GameUtil.GetFormattedPercent(instance.MorbDevelopment_Progress * 100f, GameUtil.TimeSlice.None));
			};
			this.MorbRoverMakerGermCollectionProgress.resolveTooltipCallback = delegate(string str, object data)
			{
				MorbRoverMaker.Instance instance = (MorbRoverMaker.Instance)data;
				return str.Replace("{GERM_NAME}", Db.Get().Diseases[instance.def.GERM_TYPE].Name).Replace("{0}", GameUtil.GetFormattedDiseaseAmount(instance.def.MAX_GERMS_TAKEN_PER_PACKAGE, GameUtil.TimeSlice.PerSecond)).Replace("{1}", GameUtil.GetFormattedDiseaseAmount(instance.MorbDevelopment_GermsCollected, GameUtil.TimeSlice.None)).Replace("{2}", GameUtil.GetFormattedDiseaseAmount(instance.def.GERMS_PER_ROVER, GameUtil.TimeSlice.None));
			};
			this.MorbRoverMakerNoGermsConsumedAlert = this.CreateStatusItem("MorbRoverMakerNoGermsConsumedAlert", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.NOGERMSCONSUMEDALERT.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.NOGERMSCONSUMEDALERT.TOOLTIP, "status_item_no_germs", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerNoGermsConsumedAlert.resolveStringCallback = delegate(string str, object data)
			{
				MorbRoverMaker.Instance instance = (MorbRoverMaker.Instance)data;
				return str.Replace("{0}", Db.Get().Diseases[instance.def.GERM_TYPE].Name);
			};
			this.MorbRoverMakerNoGermsConsumedAlert.resolveTooltipCallback = delegate(string str, object data)
			{
				MorbRoverMaker.Instance instance = (MorbRoverMaker.Instance)data;
				return str.Replace("{0}", Db.Get().Diseases[instance.def.GERM_TYPE].Name);
			};
			this.MorbRoverMakerCraftingBody = this.CreateStatusItem("MorbRoverMakerCraftingBody", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.CRAFTING_ROBOT_BODY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.CRAFTING_ROBOT_BODY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerReadyForDoctor = this.CreateStatusItem("MorbRoverMakerReadyForDoctor", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_READY.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.DOCTOR_READY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.MorbRoverMakerDoctorWorking = this.CreateStatusItem("MorbRoverMakerDoctorWorking", CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_WORKED_BY_DOCTOR.NAME, CODEX.STORY_TRAITS.MORB_ROVER_MAKER.STATUSITEMS.BUILDING_BEING_WORKED_BY_DOCTOR.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoVentQuestBlockage = this.CreateStatusItem("GeoVentQuestBlockage", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.QUEST_BLOCKED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.QUEST_BLOCKED_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoVentQuestBlockage.resolveStringCallback = ((string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName()));
			this.GeoVentQuestBlockage.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoVentsDisconnected = this.CreateStatusItem("GeoVentsDisconnected", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.DISCONNECTED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.DISCONNECTED_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoVentsDisconnected.resolveStringCallback = ((string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName()));
			this.GeoVentsDisconnected.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoVentsOverpressure = this.CreateStatusItem("GeoVentsOverpressure", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.OVERPRESSURE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.OVERPRESSURE_TOOLTIP, "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoVentsOverpressure.resolveStringCallback = ((string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName()));
			this.GeoVentsOverpressure.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoControllerCantVent = this.CreateStatusItem("GeoControllerCantVent", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_TOOLTIP, "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoControllerCantVent.resolveStringCallback = delegate(string str, object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				if (geothermalController == null)
				{
					return str;
				}
				GeothermalVent geothermalVent = geothermalController.FirstObstructedVent();
				if (geothermalVent == null)
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_NAME;
				}
				if (geothermalVent.IsEntombed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_ENTOMBED_VENT_NAME;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_UNREADY_CONNECTION_NAME;
			};
			this.GeoControllerCantVent.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoControllerCantVent.resolveTooltipCallback = delegate(string str, object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				if (geothermalController == null)
				{
					return str;
				}
				GeothermalVent geothermalVent = geothermalController.FirstObstructedVent();
				if (geothermalVent == null)
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_NO_CONNECTED_TOOLTIP;
				}
				if (geothermalVent.IsEntombed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_ENTOMBED_VENT_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.CANNOT_PUSH_UNREADY_CONNECTION_TOOLTIP;
			};
			this.GeoControllerCantVent.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
			this.GeoControllerCantVent.statusItemClickCallback = delegate(object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				GeothermalVent geothermalVent = (geothermalController != null) ? geothermalController.FirstObstructedVent() : null;
				if (geothermalVent != null)
				{
					SelectTool.Instance.SelectAndFocus(geothermalVent.transform.position, geothermalVent.GetComponent<KSelectable>());
				}
			};
			this.GeoVentsReady = this.CreateStatusItem("GeoVentsReady", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.READY_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.READY_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoVentsReady.resolveStringCallback = ((string str, object obj) => str.Replace("{Name}", (obj as GeothermalVent).GetProperName()));
			this.GeoVentsReady.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoVentsVenting = this.CreateStatusItem("GeoVentsVenting", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.VENTING_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.VENTING_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoVentsVenting.resolveStringCallback = delegate(string str, object obj)
			{
				GeothermalVent geothermalVent = obj as GeothermalVent;
				return str.Replace("{Name}", geothermalVent.GetProperName()).Replace("{Quantity}", GameUtil.GetFormattedMass(geothermalVent.MaterialAvailable(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.GeoVentsVenting.resolveStringCallback_shouldStillCallIfDataIsNull = false;
			this.GeoVentsVenting.resolveTooltipCallback = delegate(string str, object data)
			{
				GeothermalVent geothermalVent = data as GeothermalVent;
				if (geothermalVent != null)
				{
					return str.Replace("{Quantity}", GameUtil.GetFormattedMass(geothermalVent.MaterialAvailable(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				return str;
			};
			this.GeoVentsReady.resolveTooltipCallback_shouldStillCallIfDataIsNull = false;
			this.GeoQuestPendingReconnectPipes = this.CreateStatusItem("GeoQuestPendingReconnectPipes", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.PENDING_RECONNECTION_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.PENDING_RECONNECTION_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoQuestPendingUncover = this.CreateStatusItem("GeoQuestPendingUncover", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.PENDING_REVEAL_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.VENT.PENDING_REVEAL_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
			this.GeoControllerOffline = this.CreateStatusItem("GeoControllerOffline", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.OFFLINE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.OFFLINE_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoControllerStorageStatus = this.CreateStatusItem("GeoControllerStorageStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoControllerStorageStatus = this.CreateStatusItem("GeoControllerStorageStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_STATUS_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoControllerStorageStatus.resolveStringCallback = delegate(string str, object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				float percent = (geothermalController != null) ? (geothermalController.GetPressure() * 100f) : 0f;
				return str.Replace("{Amount}", GameUtil.GetFormattedPercent(percent, GameUtil.TimeSlice.None));
			};
			this.GeoControllerStorageStatus.resolveTooltipCallback = delegate(string str, object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				float num = (geothermalController != null) ? geothermalController.GetPressure() : 0f;
				return str.Replace("{Amount}", GameUtil.GetFormattedMass(12000f * num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Threshold}", GameUtil.GetFormattedMass(12000f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			};
			this.GeoControllerStorageStatus.resolveStringCallback_shouldStillCallIfDataIsNull = (this.GeoControllerStorageStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);
			this.GeoControllerTemperatureStatus = this.CreateStatusItem("GeoControllerTemperatureStatus", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_TEMPERATURE_NAME, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.STATUSITEMS.CONTROLLER.STORAGE_TEMPERATURE_TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022);
			this.GeoControllerTemperatureStatus.resolveStringCallback = delegate(string str, object obj)
			{
				GeothermalController geothermalController = obj as GeothermalController;
				float temp = (geothermalController != null) ? geothermalController.ComputeContentTemperature() : 0f;
				return str.Replace("{Temp}", GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
			this.GeoControllerTemperatureStatus.resolveStringCallback_shouldStillCallIfDataIsNull = (this.GeoControllerTemperatureStatus.resolveTooltipCallback_shouldStillCallIfDataIsNull = false);
			this.RemoteWorkDockMakingWorker = new StatusItem("RemoteWorkDockMakingWorker", BUILDING.STATUSITEMS.REMOTEWORKERDEPOT.MAKINGWORKER.NAME, BUILDING.STATUSITEMS.REMOTEWORKERDEPOT.MAKINGWORKER.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, 129022, true, null);
			this.RemoteWorkTerminalNoDock = new StatusItem("RemoteWorkTerminalNoDock", BUILDING.STATUSITEMS.REMOTEWORKTERMINAL.NODOCK.NAME, BUILDING.STATUSITEMS.REMOTEWORKTERMINAL.NODOCK.TOOLTIP, "status_item_exclamation", StatusItem.IconType.Custom, NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.DataMinerEfficiency = new StatusItem("RemoteWorkTerminalNoDock", BUILDING.STATUSITEMS.DATAMINER.PRODUCTIONRATE.NAME, BUILDING.STATUSITEMS.DATAMINER.PRODUCTIONRATE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
			this.DataMinerEfficiency.resolveStringCallback = delegate(string str, object obj)
			{
				DataMiner dataMiner = obj as DataMiner;
				return str.Replace("{RATE}", GameUtil.GetFormattedPercent(dataMiner.EfficiencyRate * 100f, GameUtil.TimeSlice.None)).Replace("{TEMP}", GameUtil.GetFormattedTemperature(dataMiner.OperatingTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
			};
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x0045DFC8 File Offset: 0x0045C1C8
		private static bool ShowInUtilityOverlay(HashedString mode, object data)
		{
			Transform transform = (Transform)data;
			bool result = false;
			if (mode == OverlayModes.GasConduits.ID)
			{
				Tag prefabTag = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.GasVentIDs.Contains(prefabTag);
			}
			else if (mode == OverlayModes.LiquidConduits.ID)
			{
				Tag prefabTag2 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.LiquidVentIDs.Contains(prefabTag2);
			}
			else if (mode == OverlayModes.Power.ID)
			{
				Tag prefabTag3 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.WireIDs.Contains(prefabTag3);
			}
			else if (mode == OverlayModes.Logic.ID)
			{
				Tag prefabTag4 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag4);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				Tag prefabTag5 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.SolidConveyorIDs.Contains(prefabTag5);
			}
			else if (mode == OverlayModes.Radiation.ID)
			{
				Tag prefabTag6 = transform.GetComponent<KPrefabID>().PrefabTag;
				result = OverlayScreen.RadiationIDs.Contains(prefabTag6);
			}
			return result;
		}

		// Token: 0x0600B6BB RID: 46779 RVA: 0x0045E0D8 File Offset: 0x0045C2D8
		[CompilerGenerated]
		internal static string <CreateStatusItems>g__SkyVisResolveStringCallback|322_117(string str, object data)
		{
			BuildingStatusItems.ISkyVisInfo skyVisInfo = (BuildingStatusItems.ISkyVisInfo)data;
			return str.Replace("{VISIBILITY}", GameUtil.GetFormattedPercent(skyVisInfo.GetPercentVisible01() * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x040090DE RID: 37086
		public StatusItem MissingRequirements;

		// Token: 0x040090DF RID: 37087
		public StatusItem GettingReady;

		// Token: 0x040090E0 RID: 37088
		public StatusItem Working;

		// Token: 0x040090E1 RID: 37089
		public MaterialsStatusItem MaterialsUnavailable;

		// Token: 0x040090E2 RID: 37090
		public MaterialsStatusItem MaterialsUnavailableForRefill;

		// Token: 0x040090E3 RID: 37091
		public StatusItem AngerDamage;

		// Token: 0x040090E4 RID: 37092
		public StatusItem ClinicOutsideHospital;

		// Token: 0x040090E5 RID: 37093
		public StatusItem DigUnreachable;

		// Token: 0x040090E6 RID: 37094
		public StatusItem MopUnreachable;

		// Token: 0x040090E7 RID: 37095
		public StatusItem StorageUnreachable;

		// Token: 0x040090E8 RID: 37096
		public StatusItem PassengerModuleUnreachable;

		// Token: 0x040090E9 RID: 37097
		public StatusItem ConstructableDigUnreachable;

		// Token: 0x040090EA RID: 37098
		public StatusItem ConstructionUnreachable;

		// Token: 0x040090EB RID: 37099
		public StatusItem CoolingWater;

		// Token: 0x040090EC RID: 37100
		public StatusItem DispenseRequested;

		// Token: 0x040090ED RID: 37101
		public StatusItem NewDuplicantsAvailable;

		// Token: 0x040090EE RID: 37102
		public StatusItem NeedPlant;

		// Token: 0x040090EF RID: 37103
		public StatusItem NeedPower;

		// Token: 0x040090F0 RID: 37104
		public StatusItem NotEnoughPower;

		// Token: 0x040090F1 RID: 37105
		public StatusItem PowerLoopDetected;

		// Token: 0x040090F2 RID: 37106
		public StatusItem NeedLiquidIn;

		// Token: 0x040090F3 RID: 37107
		public StatusItem NeedGasIn;

		// Token: 0x040090F4 RID: 37108
		public StatusItem NeedResourceMass;

		// Token: 0x040090F5 RID: 37109
		public StatusItem NeedSolidIn;

		// Token: 0x040090F6 RID: 37110
		public StatusItem NeedLiquidOut;

		// Token: 0x040090F7 RID: 37111
		public StatusItem NeedGasOut;

		// Token: 0x040090F8 RID: 37112
		public StatusItem NeedSolidOut;

		// Token: 0x040090F9 RID: 37113
		public StatusItem InvalidBuildingLocation;

		// Token: 0x040090FA RID: 37114
		public StatusItem PendingDeconstruction;

		// Token: 0x040090FB RID: 37115
		public StatusItem PendingDemolition;

		// Token: 0x040090FC RID: 37116
		public StatusItem PendingSwitchToggle;

		// Token: 0x040090FD RID: 37117
		public StatusItem GasVentObstructed;

		// Token: 0x040090FE RID: 37118
		public StatusItem LiquidVentObstructed;

		// Token: 0x040090FF RID: 37119
		public StatusItem LiquidPipeEmpty;

		// Token: 0x04009100 RID: 37120
		public StatusItem LiquidPipeObstructed;

		// Token: 0x04009101 RID: 37121
		public StatusItem GasPipeEmpty;

		// Token: 0x04009102 RID: 37122
		public StatusItem GasPipeObstructed;

		// Token: 0x04009103 RID: 37123
		public StatusItem SolidPipeObstructed;

		// Token: 0x04009104 RID: 37124
		public StatusItem PartiallyDamaged;

		// Token: 0x04009105 RID: 37125
		public StatusItem Broken;

		// Token: 0x04009106 RID: 37126
		public StatusItem PendingRepair;

		// Token: 0x04009107 RID: 37127
		public StatusItem PendingUpgrade;

		// Token: 0x04009108 RID: 37128
		public StatusItem RequiresSkillPerk;

		// Token: 0x04009109 RID: 37129
		public StatusItem DigRequiresSkillPerk;

		// Token: 0x0400910A RID: 37130
		public StatusItem ColonyLacksRequiredSkillPerk;

		// Token: 0x0400910B RID: 37131
		public StatusItem ClusterColonyLacksRequiredSkillPerk;

		// Token: 0x0400910C RID: 37132
		public StatusItem WorkRequiresMinion;

		// Token: 0x0400910D RID: 37133
		public StatusItem PendingWork;

		// Token: 0x0400910E RID: 37134
		public StatusItem Flooded;

		// Token: 0x0400910F RID: 37135
		public StatusItem NotSubmerged;

		// Token: 0x04009110 RID: 37136
		public StatusItem PowerButtonOff;

		// Token: 0x04009111 RID: 37137
		public StatusItem SwitchStatusActive;

		// Token: 0x04009112 RID: 37138
		public StatusItem SwitchStatusInactive;

		// Token: 0x04009113 RID: 37139
		public StatusItem LogicSwitchStatusActive;

		// Token: 0x04009114 RID: 37140
		public StatusItem LogicSwitchStatusInactive;

		// Token: 0x04009115 RID: 37141
		public StatusItem LogicSensorStatusActive;

		// Token: 0x04009116 RID: 37142
		public StatusItem LogicSensorStatusInactive;

		// Token: 0x04009117 RID: 37143
		public StatusItem ChangeDoorControlState;

		// Token: 0x04009118 RID: 37144
		public StatusItem CurrentDoorControlState;

		// Token: 0x04009119 RID: 37145
		public StatusItem ChangeStorageTileTarget;

		// Token: 0x0400911A RID: 37146
		public StatusItem Entombed;

		// Token: 0x0400911B RID: 37147
		public MaterialsStatusItem WaitingForMaterials;

		// Token: 0x0400911C RID: 37148
		public StatusItem WaitingForHighEnergyParticles;

		// Token: 0x0400911D RID: 37149
		public StatusItem WaitingForRepairMaterials;

		// Token: 0x0400911E RID: 37150
		public StatusItem MissingFoundation;

		// Token: 0x0400911F RID: 37151
		public StatusItem PowerBankChargerInProgress;

		// Token: 0x04009120 RID: 37152
		public StatusItem NeutroniumUnminable;

		// Token: 0x04009121 RID: 37153
		public StatusItem NoStorageFilterSet;

		// Token: 0x04009122 RID: 37154
		public StatusItem PendingFish;

		// Token: 0x04009123 RID: 37155
		public StatusItem NoFishableWaterBelow;

		// Token: 0x04009124 RID: 37156
		public StatusItem GasVentOverPressure;

		// Token: 0x04009125 RID: 37157
		public StatusItem LiquidVentOverPressure;

		// Token: 0x04009126 RID: 37158
		public StatusItem NoWireConnected;

		// Token: 0x04009127 RID: 37159
		public StatusItem NoLogicWireConnected;

		// Token: 0x04009128 RID: 37160
		public StatusItem NoTubeConnected;

		// Token: 0x04009129 RID: 37161
		public StatusItem NoTubeExits;

		// Token: 0x0400912A RID: 37162
		public StatusItem StoredCharge;

		// Token: 0x0400912B RID: 37163
		public StatusItem NoPowerConsumers;

		// Token: 0x0400912C RID: 37164
		public StatusItem PressureOk;

		// Token: 0x0400912D RID: 37165
		public StatusItem UnderPressure;

		// Token: 0x0400912E RID: 37166
		public StatusItem AssignedTo;

		// Token: 0x0400912F RID: 37167
		public StatusItem Unassigned;

		// Token: 0x04009130 RID: 37168
		public StatusItem AssignedPublic;

		// Token: 0x04009131 RID: 37169
		public StatusItem AssignedToRoom;

		// Token: 0x04009132 RID: 37170
		public StatusItem RationBoxContents;

		// Token: 0x04009133 RID: 37171
		public StatusItem ConduitBlocked;

		// Token: 0x04009134 RID: 37172
		public StatusItem OutputTileBlocked;

		// Token: 0x04009135 RID: 37173
		public StatusItem OutputPipeFull;

		// Token: 0x04009136 RID: 37174
		public StatusItem ConduitBlockedMultiples;

		// Token: 0x04009137 RID: 37175
		public StatusItem SolidConduitBlockedMultiples;

		// Token: 0x04009138 RID: 37176
		public StatusItem MeltingDown;

		// Token: 0x04009139 RID: 37177
		public StatusItem DeadReactorCoolingOff;

		// Token: 0x0400913A RID: 37178
		public StatusItem UnderConstruction;

		// Token: 0x0400913B RID: 37179
		public StatusItem UnderConstructionNoWorker;

		// Token: 0x0400913C RID: 37180
		public StatusItem Normal;

		// Token: 0x0400913D RID: 37181
		public StatusItem ManualGeneratorChargingUp;

		// Token: 0x0400913E RID: 37182
		public StatusItem ManualGeneratorReleasingEnergy;

		// Token: 0x0400913F RID: 37183
		public StatusItem GeneratorOffline;

		// Token: 0x04009140 RID: 37184
		public StatusItem Pipe;

		// Token: 0x04009141 RID: 37185
		public StatusItem Conveyor;

		// Token: 0x04009142 RID: 37186
		public StatusItem FabricatorIdle;

		// Token: 0x04009143 RID: 37187
		public StatusItem FabricatorEmpty;

		// Token: 0x04009144 RID: 37188
		public StatusItem FossilMineIdle;

		// Token: 0x04009145 RID: 37189
		public StatusItem FossilMineEmpty;

		// Token: 0x04009146 RID: 37190
		public StatusItem FossilEntombed;

		// Token: 0x04009147 RID: 37191
		public StatusItem FossilMinePendingWork;

		// Token: 0x04009148 RID: 37192
		public StatusItem FabricatorLacksHEP;

		// Token: 0x04009149 RID: 37193
		public StatusItem FlushToilet;

		// Token: 0x0400914A RID: 37194
		public StatusItem FlushToiletInUse;

		// Token: 0x0400914B RID: 37195
		public StatusItem Toilet;

		// Token: 0x0400914C RID: 37196
		public StatusItem ToiletNeedsEmptying;

		// Token: 0x0400914D RID: 37197
		public StatusItem DesalinatorNeedsEmptying;

		// Token: 0x0400914E RID: 37198
		public StatusItem MilkSeparatorNeedsEmptying;

		// Token: 0x0400914F RID: 37199
		public StatusItem Unusable;

		// Token: 0x04009150 RID: 37200
		public StatusItem UnusableGunked;

		// Token: 0x04009151 RID: 37201
		public StatusItem NoResearchSelected;

		// Token: 0x04009152 RID: 37202
		public StatusItem NoApplicableResearchSelected;

		// Token: 0x04009153 RID: 37203
		public StatusItem NoApplicableAnalysisSelected;

		// Token: 0x04009154 RID: 37204
		public StatusItem NoResearchOrDestinationSelected;

		// Token: 0x04009155 RID: 37205
		public StatusItem Researching;

		// Token: 0x04009156 RID: 37206
		public StatusItem ValveRequest;

		// Token: 0x04009157 RID: 37207
		public StatusItem EmittingLight;

		// Token: 0x04009158 RID: 37208
		public StatusItem EmittingElement;

		// Token: 0x04009159 RID: 37209
		public StatusItem EmittingOxygenAvg;

		// Token: 0x0400915A RID: 37210
		public StatusItem EmittingGasAvg;

		// Token: 0x0400915B RID: 37211
		public StatusItem EmittingBlockedHighPressure;

		// Token: 0x0400915C RID: 37212
		public StatusItem EmittingBlockedLowTemperature;

		// Token: 0x0400915D RID: 37213
		public StatusItem PumpingLiquidOrGas;

		// Token: 0x0400915E RID: 37214
		public StatusItem NoLiquidElementToPump;

		// Token: 0x0400915F RID: 37215
		public StatusItem NoGasElementToPump;

		// Token: 0x04009160 RID: 37216
		public StatusItem PipeFull;

		// Token: 0x04009161 RID: 37217
		public StatusItem PipeMayMelt;

		// Token: 0x04009162 RID: 37218
		public StatusItem ElementConsumer;

		// Token: 0x04009163 RID: 37219
		public StatusItem ElementEmitterOutput;

		// Token: 0x04009164 RID: 37220
		public StatusItem AwaitingWaste;

		// Token: 0x04009165 RID: 37221
		public StatusItem AwaitingCompostFlip;

		// Token: 0x04009166 RID: 37222
		public StatusItem BatteryJoulesAvailable;

		// Token: 0x04009167 RID: 37223
		public StatusItem ElectrobankJoulesAvailable;

		// Token: 0x04009168 RID: 37224
		public StatusItem Wattage;

		// Token: 0x04009169 RID: 37225
		public StatusItem SolarPanelWattage;

		// Token: 0x0400916A RID: 37226
		public StatusItem ModuleSolarPanelWattage;

		// Token: 0x0400916B RID: 37227
		public StatusItem SteamTurbineWattage;

		// Token: 0x0400916C RID: 37228
		public StatusItem Wattson;

		// Token: 0x0400916D RID: 37229
		public StatusItem WireConnected;

		// Token: 0x0400916E RID: 37230
		public StatusItem WireNominal;

		// Token: 0x0400916F RID: 37231
		public StatusItem WireDisconnected;

		// Token: 0x04009170 RID: 37232
		public StatusItem Cooling;

		// Token: 0x04009171 RID: 37233
		public StatusItem CoolingStalledHotEnv;

		// Token: 0x04009172 RID: 37234
		public StatusItem CoolingStalledColdGas;

		// Token: 0x04009173 RID: 37235
		public StatusItem CoolingStalledHotLiquid;

		// Token: 0x04009174 RID: 37236
		public StatusItem CoolingStalledColdLiquid;

		// Token: 0x04009175 RID: 37237
		public StatusItem CannotCoolFurther;

		// Token: 0x04009176 RID: 37238
		public StatusItem NeedsValidRegion;

		// Token: 0x04009177 RID: 37239
		public StatusItem NeedSeed;

		// Token: 0x04009178 RID: 37240
		public StatusItem AwaitingSeedDelivery;

		// Token: 0x04009179 RID: 37241
		public StatusItem AwaitingBaitDelivery;

		// Token: 0x0400917A RID: 37242
		public StatusItem NoAvailableSeed;

		// Token: 0x0400917B RID: 37243
		public StatusItem NeedEgg;

		// Token: 0x0400917C RID: 37244
		public StatusItem AwaitingEggDelivery;

		// Token: 0x0400917D RID: 37245
		public StatusItem NoAvailableEgg;

		// Token: 0x0400917E RID: 37246
		public StatusItem Grave;

		// Token: 0x0400917F RID: 37247
		public StatusItem GraveEmpty;

		// Token: 0x04009180 RID: 37248
		public StatusItem NoFilterElementSelected;

		// Token: 0x04009181 RID: 37249
		public StatusItem NoLureElementSelected;

		// Token: 0x04009182 RID: 37250
		public StatusItem BuildingDisabled;

		// Token: 0x04009183 RID: 37251
		public StatusItem Overheated;

		// Token: 0x04009184 RID: 37252
		public StatusItem Overloaded;

		// Token: 0x04009185 RID: 37253
		public StatusItem LogicOverloaded;

		// Token: 0x04009186 RID: 37254
		public StatusItem Expired;

		// Token: 0x04009187 RID: 37255
		public StatusItem PumpingStation;

		// Token: 0x04009188 RID: 37256
		public StatusItem EmptyPumpingStation;

		// Token: 0x04009189 RID: 37257
		public StatusItem GeneShuffleCompleted;

		// Token: 0x0400918A RID: 37258
		public StatusItem GeneticAnalysisCompleted;

		// Token: 0x0400918B RID: 37259
		public StatusItem DirectionControl;

		// Token: 0x0400918C RID: 37260
		public StatusItem WellPressurizing;

		// Token: 0x0400918D RID: 37261
		public StatusItem WellOverpressure;

		// Token: 0x0400918E RID: 37262
		public StatusItem ReleasingPressure;

		// Token: 0x0400918F RID: 37263
		public StatusItem ReactorMeltdown;

		// Token: 0x04009190 RID: 37264
		public StatusItem NoSuitMarker;

		// Token: 0x04009191 RID: 37265
		public StatusItem SuitMarkerWrongSide;

		// Token: 0x04009192 RID: 37266
		public StatusItem SuitMarkerTraversalAnytime;

		// Token: 0x04009193 RID: 37267
		public StatusItem SuitMarkerTraversalOnlyWhenRoomAvailable;

		// Token: 0x04009194 RID: 37268
		public StatusItem TooCold;

		// Token: 0x04009195 RID: 37269
		public StatusItem NotInAnyRoom;

		// Token: 0x04009196 RID: 37270
		public StatusItem NotInRequiredRoom;

		// Token: 0x04009197 RID: 37271
		public StatusItem NotInRecommendedRoom;

		// Token: 0x04009198 RID: 37272
		public StatusItem IncubatorProgress;

		// Token: 0x04009199 RID: 37273
		public StatusItem HabitatNeedsEmptying;

		// Token: 0x0400919A RID: 37274
		public StatusItem DetectorScanning;

		// Token: 0x0400919B RID: 37275
		public StatusItem IncomingMeteors;

		// Token: 0x0400919C RID: 37276
		public StatusItem HasGantry;

		// Token: 0x0400919D RID: 37277
		public StatusItem MissingGantry;

		// Token: 0x0400919E RID: 37278
		public StatusItem DisembarkingDuplicant;

		// Token: 0x0400919F RID: 37279
		public StatusItem RocketName;

		// Token: 0x040091A0 RID: 37280
		public StatusItem PathNotClear;

		// Token: 0x040091A1 RID: 37281
		public StatusItem InvalidPortOverlap;

		// Token: 0x040091A2 RID: 37282
		public StatusItem EmergencyPriority;

		// Token: 0x040091A3 RID: 37283
		public StatusItem SkillPointsAvailable;

		// Token: 0x040091A4 RID: 37284
		public StatusItem Baited;

		// Token: 0x040091A5 RID: 37285
		public StatusItem NoCoolant;

		// Token: 0x040091A6 RID: 37286
		public StatusItem TanningLightSufficient;

		// Token: 0x040091A7 RID: 37287
		public StatusItem TanningLightInsufficient;

		// Token: 0x040091A8 RID: 37288
		public StatusItem HotTubWaterTooCold;

		// Token: 0x040091A9 RID: 37289
		public StatusItem HotTubTooHot;

		// Token: 0x040091AA RID: 37290
		public StatusItem HotTubFilling;

		// Token: 0x040091AB RID: 37291
		public StatusItem WindTunnelIntake;

		// Token: 0x040091AC RID: 37292
		public StatusItem CollectingHEP;

		// Token: 0x040091AD RID: 37293
		public StatusItem ReactorRefuelDisabled;

		// Token: 0x040091AE RID: 37294
		public StatusItem FridgeCooling;

		// Token: 0x040091AF RID: 37295
		public StatusItem FridgeSteady;

		// Token: 0x040091B0 RID: 37296
		public StatusItem TrapNeedsArming;

		// Token: 0x040091B1 RID: 37297
		public StatusItem TrapArmed;

		// Token: 0x040091B2 RID: 37298
		public StatusItem TrapHasCritter;

		// Token: 0x040091B3 RID: 37299
		public StatusItem WarpPortalCharging;

		// Token: 0x040091B4 RID: 37300
		public StatusItem WarpConduitPartnerDisabled;

		// Token: 0x040091B5 RID: 37301
		public StatusItem InOrbit;

		// Token: 0x040091B6 RID: 37302
		public StatusItem InFlight;

		// Token: 0x040091B7 RID: 37303
		public StatusItem WaitingToLand;

		// Token: 0x040091B8 RID: 37304
		public StatusItem DestinationOutOfRange;

		// Token: 0x040091B9 RID: 37305
		public StatusItem RocketStranded;

		// Token: 0x040091BA RID: 37306
		public StatusItem RailgunpayloadNeedsEmptying;

		// Token: 0x040091BB RID: 37307
		public StatusItem AwaitingEmptyBuilding;

		// Token: 0x040091BC RID: 37308
		public StatusItem DuplicantActivationRequired;

		// Token: 0x040091BD RID: 37309
		public StatusItem RocketChecklistIncomplete;

		// Token: 0x040091BE RID: 37310
		public StatusItem RocketCargoEmptying;

		// Token: 0x040091BF RID: 37311
		public StatusItem RocketCargoFilling;

		// Token: 0x040091C0 RID: 37312
		public StatusItem RocketCargoFull;

		// Token: 0x040091C1 RID: 37313
		public StatusItem FlightAllCargoFull;

		// Token: 0x040091C2 RID: 37314
		public StatusItem FlightCargoRemaining;

		// Token: 0x040091C3 RID: 37315
		public StatusItem LandedRocketLacksPassengerModule;

		// Token: 0x040091C4 RID: 37316
		public StatusItem PilotNeeded;

		// Token: 0x040091C5 RID: 37317
		public StatusItem AutoPilotActive;

		// Token: 0x040091C6 RID: 37318
		public StatusItem InFlightPiloted;

		// Token: 0x040091C7 RID: 37319
		public StatusItem InFlightUnpiloted;

		// Token: 0x040091C8 RID: 37320
		public StatusItem InFlightAutoPiloted;

		// Token: 0x040091C9 RID: 37321
		public StatusItem InFlightSuperPilot;

		// Token: 0x040091CA RID: 37322
		public StatusItem InvalidMaskStationConsumptionState;

		// Token: 0x040091CB RID: 37323
		public StatusItem ClusterTelescopeAllWorkComplete;

		// Token: 0x040091CC RID: 37324
		public StatusItem RocketPlatformCloseToCeiling;

		// Token: 0x040091CD RID: 37325
		public StatusItem ModuleGeneratorPowered;

		// Token: 0x040091CE RID: 37326
		public StatusItem ModuleGeneratorNotPowered;

		// Token: 0x040091CF RID: 37327
		public StatusItem InOrbitRequired;

		// Token: 0x040091D0 RID: 37328
		public StatusItem RailGunCooldown;

		// Token: 0x040091D1 RID: 37329
		public StatusItem NoSurfaceSight;

		// Token: 0x040091D2 RID: 37330
		public StatusItem LimitValveLimitReached;

		// Token: 0x040091D3 RID: 37331
		public StatusItem LimitValveLimitNotReached;

		// Token: 0x040091D4 RID: 37332
		public StatusItem SpacePOIHarvesting;

		// Token: 0x040091D5 RID: 37333
		public StatusItem SpacePOIWasting;

		// Token: 0x040091D6 RID: 37334
		public StatusItem RocketRestrictionActive;

		// Token: 0x040091D7 RID: 37335
		public StatusItem RocketRestrictionInactive;

		// Token: 0x040091D8 RID: 37336
		public StatusItem NoRocketRestriction;

		// Token: 0x040091D9 RID: 37337
		public StatusItem BroadcasterOutOfRange;

		// Token: 0x040091DA RID: 37338
		public StatusItem LosingRadbolts;

		// Token: 0x040091DB RID: 37339
		public StatusItem FabricatorAcceptsMutantSeeds;

		// Token: 0x040091DC RID: 37340
		public StatusItem NoSpiceSelected;

		// Token: 0x040091DD RID: 37341
		public StatusItem MissionControlAssistingRocket;

		// Token: 0x040091DE RID: 37342
		public StatusItem NoRocketsToMissionControlBoost;

		// Token: 0x040091DF RID: 37343
		public StatusItem NoRocketsToMissionControlClusterBoost;

		// Token: 0x040091E0 RID: 37344
		public StatusItem MissionControlBoosted;

		// Token: 0x040091E1 RID: 37345
		public StatusItem TransitTubeEntranceWaxReady;

		// Token: 0x040091E2 RID: 37346
		public StatusItem SpecialCargoBayClusterCritterStored;

		// Token: 0x040091E3 RID: 37347
		public StatusItem ComplexFabricatorCooking;

		// Token: 0x040091E4 RID: 37348
		public StatusItem ComplexFabricatorProducing;

		// Token: 0x040091E5 RID: 37349
		public StatusItem ComplexFabricatorTraining;

		// Token: 0x040091E6 RID: 37350
		public StatusItem ComplexFabricatorResearching;

		// Token: 0x040091E7 RID: 37351
		public StatusItem ArtifactAnalysisAnalyzing;

		// Token: 0x040091E8 RID: 37352
		public StatusItem TelescopeWorking;

		// Token: 0x040091E9 RID: 37353
		public StatusItem ClusterTelescopeMeteorWorking;

		// Token: 0x040091EA RID: 37354
		public StatusItem MercuryLight_Charging;

		// Token: 0x040091EB RID: 37355
		public StatusItem MercuryLight_Charged;

		// Token: 0x040091EC RID: 37356
		public StatusItem MercuryLight_Depleating;

		// Token: 0x040091ED RID: 37357
		public StatusItem MercuryLight_Depleated;

		// Token: 0x040091EE RID: 37358
		public StatusItem GunkEmptierFull;

		// Token: 0x040091EF RID: 37359
		public StatusItem GeoTunerNoGeyserSelected;

		// Token: 0x040091F0 RID: 37360
		public StatusItem GeoTunerResearchNeeded;

		// Token: 0x040091F1 RID: 37361
		public StatusItem GeoTunerResearchInProgress;

		// Token: 0x040091F2 RID: 37362
		public StatusItem GeoTunerBroadcasting;

		// Token: 0x040091F3 RID: 37363
		public StatusItem GeoTunerGeyserStatus;

		// Token: 0x040091F4 RID: 37364
		public StatusItem GeyserGeotuned;

		// Token: 0x040091F5 RID: 37365
		public StatusItem SkyVisNone;

		// Token: 0x040091F6 RID: 37366
		public StatusItem SkyVisLimited;

		// Token: 0x040091F7 RID: 37367
		public StatusItem KettleInsuficientSolids;

		// Token: 0x040091F8 RID: 37368
		public StatusItem KettleInsuficientFuel;

		// Token: 0x040091F9 RID: 37369
		public StatusItem KettleInsuficientLiquidSpace;

		// Token: 0x040091FA RID: 37370
		public StatusItem KettleMelting;

		// Token: 0x040091FB RID: 37371
		public StatusItem CreatureManipulatorWaiting;

		// Token: 0x040091FC RID: 37372
		public StatusItem CreatureManipulatorProgress;

		// Token: 0x040091FD RID: 37373
		public StatusItem CreatureManipulatorMorphModeLocked;

		// Token: 0x040091FE RID: 37374
		public StatusItem CreatureManipulatorMorphMode;

		// Token: 0x040091FF RID: 37375
		public StatusItem CreatureManipulatorWorking;

		// Token: 0x04009200 RID: 37376
		public StatusItem MegaBrainNotEnoughOxygen;

		// Token: 0x04009201 RID: 37377
		public StatusItem MegaBrainTankActivationProgress;

		// Token: 0x04009202 RID: 37378
		public StatusItem MegaBrainTankDreamAnalysis;

		// Token: 0x04009203 RID: 37379
		public StatusItem MegaBrainTankAllDupesAreDead;

		// Token: 0x04009204 RID: 37380
		public StatusItem MegaBrainTankComplete;

		// Token: 0x04009205 RID: 37381
		public StatusItem FossilHuntExcavationOrdered;

		// Token: 0x04009206 RID: 37382
		public StatusItem FossilHuntExcavationInProgress;

		// Token: 0x04009207 RID: 37383
		public StatusItem MorbRoverMakerDusty;

		// Token: 0x04009208 RID: 37384
		public StatusItem MorbRoverMakerBuildingRevealed;

		// Token: 0x04009209 RID: 37385
		public StatusItem MorbRoverMakerGermCollectionProgress;

		// Token: 0x0400920A RID: 37386
		public StatusItem MorbRoverMakerNoGermsConsumedAlert;

		// Token: 0x0400920B RID: 37387
		public StatusItem MorbRoverMakerCraftingBody;

		// Token: 0x0400920C RID: 37388
		public StatusItem MorbRoverMakerReadyForDoctor;

		// Token: 0x0400920D RID: 37389
		public StatusItem MorbRoverMakerDoctorWorking;

		// Token: 0x0400920E RID: 37390
		public StatusItem GeoVentQuestBlockage;

		// Token: 0x0400920F RID: 37391
		public StatusItem GeoVentsDisconnected;

		// Token: 0x04009210 RID: 37392
		public StatusItem GeoVentsOverpressure;

		// Token: 0x04009211 RID: 37393
		public StatusItem GeoControllerCantVent;

		// Token: 0x04009212 RID: 37394
		public StatusItem GeoVentsReady;

		// Token: 0x04009213 RID: 37395
		public StatusItem GeoVentsVenting;

		// Token: 0x04009214 RID: 37396
		public StatusItem GeoQuestPendingReconnectPipes;

		// Token: 0x04009215 RID: 37397
		public StatusItem GeoQuestPendingUncover;

		// Token: 0x04009216 RID: 37398
		public StatusItem GeoControllerOffline;

		// Token: 0x04009217 RID: 37399
		public StatusItem GeoControllerStorageStatus;

		// Token: 0x04009218 RID: 37400
		public StatusItem GeoControllerTemperatureStatus;

		// Token: 0x04009219 RID: 37401
		public StatusItem RemoteWorkDockMakingWorker;

		// Token: 0x0400921A RID: 37402
		public StatusItem RemoteWorkTerminalNoDock;

		// Token: 0x0400921B RID: 37403
		public StatusItem DataMinerEfficiency;

		// Token: 0x0200218D RID: 8589
		public interface ISkyVisInfo
		{
			// Token: 0x0600B6BC RID: 46780
			float GetPercentVisible01();
		}
	}
}
