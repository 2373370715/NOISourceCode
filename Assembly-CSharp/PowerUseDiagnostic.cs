﻿using System;
using STRINGS;
using UnityEngine;

public class PowerUseDiagnostic : ColonyDiagnostic
{
	public PowerUseDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<PowerUseTracker>(worldID);
		this.trackerSampleCountSeconds = 30f;
		this.icon = "overlay_power";
		base.AddCriterion("CheckOverWattage", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.CRITERIA.CHECKOVERWATTAGE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOverWattage)));
		base.AddCriterion("CheckPowerUseChange", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.CRITERIA.CHECKPOWERUSECHANGE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPowerChange)));
	}

	private ColonyDiagnostic.DiagnosticResult CheckOverWattage()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.NORMAL;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num] == base.worldID)
				{
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num);
					float maxSafeWattageForCircuit = Game.Instance.circuitManager.GetMaxSafeWattageForCircuit(circuitID);
					float wattsUsedByCircuit = Game.Instance.circuitManager.GetWattsUsedByCircuit(circuitID);
					if (wattsUsedByCircuit > maxSafeWattageForCircuit)
					{
						GameObject gameObject = electricalUtilityNetwork.allWires[0].gameObject;
						result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(gameObject.transform.position, gameObject);
						result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
						result.Message = string.Format(UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.CIRCUIT_OVER_CAPACITY, GameUtil.GetFormattedWattage(wattsUsedByCircuit, GameUtil.WattageFormatterUnit.Automatic, true), GameUtil.GetFormattedWattage(maxSafeWattageForCircuit, GameUtil.WattageFormatterUnit.Automatic, true));
						break;
					}
				}
			}
		}
		return result;
	}

	private ColonyDiagnostic.DiagnosticResult CheckPowerChange()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.NORMAL;
		float num = 60f;
		if (this.tracker.GetDataTimeLength() < num)
		{
			return result;
		}
		float averageValue = this.tracker.GetAverageValue(1f);
		float averageValue2 = this.tracker.GetAverageValue(Mathf.Min(60f, this.trackerSampleCountSeconds));
		float num2 = 240f;
		if (averageValue < num2 && averageValue2 < num2)
		{
			return result;
		}
		float num3 = 0.5f;
		if (Mathf.Abs(averageValue - averageValue2) / averageValue2 > num3)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = string.Format(UI.COLONY_DIAGNOSTICS.POWERUSEDIAGNOSTIC.SIGNIFICANT_POWER_CHANGE_DETECTED, GameUtil.GetFormattedWattage(averageValue2, GameUtil.WattageFormatterUnit.Automatic, true), GameUtil.GetFormattedWattage(averageValue, GameUtil.WattageFormatterUnit.Automatic, true));
		}
		return result;
	}

	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}
