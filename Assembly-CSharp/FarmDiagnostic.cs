using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001265 RID: 4709
public class FarmDiagnostic : ColonyDiagnostic
{
	// Token: 0x0600602D RID: 24621 RVA: 0x002B9BDC File Offset: 0x002B7DDC
	public FarmDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_farm";
		base.AddCriterion("CheckHasFarms", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKHASFARMS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasFarms)));
		base.AddCriterion("CheckPlanted", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKPLANTED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckPlanted)));
		base.AddCriterion("CheckWilting", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKWILTING, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckWilting)));
		base.AddCriterion("CheckOperational", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKOPERATIONAL, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckOperational)));
	}

	// Token: 0x0600602E RID: 24622 RVA: 0x000E32CC File Offset: 0x000E14CC
	private void RefreshPlots()
	{
		this.plots = Components.PlantablePlots.GetItems(base.worldID).FindAll((PlantablePlot match) => match.HasDepositTag(GameTags.CropSeed));
	}

	// Token: 0x0600602F RID: 24623 RVA: 0x002B9CA0 File Offset: 0x002B7EA0
	private ColonyDiagnostic.DiagnosticResult CheckHasFarms()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.plots.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE;
		}
		return result;
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x002B9CE8 File Offset: 0x002B7EE8
	private ColonyDiagnostic.DiagnosticResult CheckPlanted()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		bool flag = false;
		using (List<PlantablePlot>.Enumerator enumerator = this.plots.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.plant != null)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE_PLANTED;
		}
		return result;
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x002B9D78 File Offset: 0x002B7F78
	private ColonyDiagnostic.DiagnosticResult CheckWilting()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && plantablePlot.plant.HasTag(GameTags.Wilting))
			{
				StandardCropPlant component = plantablePlot.plant.GetComponent<StandardCropPlant>();
				if (component != null && component.smi.IsInsideState(component.smi.sm.alive.wilting) && component.smi.timeinstate > 15f)
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
					result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.WILTING;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x002B9E88 File Offset: 0x002B8088
	private ColonyDiagnostic.DiagnosticResult CheckOperational()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (PlantablePlot plantablePlot in this.plots)
		{
			if (plantablePlot.plant != null && !plantablePlot.HasTag(GameTags.Operational))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.INOPERATIONAL;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);
				break;
			}
		}
		return result;
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x002B9F3C File Offset: 0x002B813C
	public override string GetAverageValueString()
	{
		if (this.plots == null)
		{
			this.RefreshPlots();
		}
		return TrackerTool.Instance.GetWorldTracker<CropTracker>(base.worldID).GetCurrentValue().ToString() + "/" + this.plots.Count.ToString();
	}

	// Token: 0x06006034 RID: 24628 RVA: 0x002B9F94 File Offset: 0x002B8194
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		this.RefreshPlots();
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x040044EA RID: 17642
	private List<PlantablePlot> plots;
}
