using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001268 RID: 4712
public class FoodDiagnostic : ColonyDiagnostic
{
	// Token: 0x0600603B RID: 24635 RVA: 0x002BA158 File Offset: 0x002B8358
	public FoodDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<KCalTracker>(worldID);
		this.icon = "icon_category_food";
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		base.AddCriterion("CheckEnoughFood", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKENOUGHFOOD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughFood)));
		base.AddCriterion("CheckStarvation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKSTARVATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStarvation)));
		this.multiplier = MinionIdentity.GetCalorieBurnMultiplier();
		this.recommendedKCalPerDuplicant = 3000f * this.multiplier;
	}

	// Token: 0x0600603C RID: 24636 RVA: 0x002BA218 File Offset: 0x002B8418
	private ColonyDiagnostic.DiagnosticResult CheckAnyFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.PASS, null);
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0)
		{
			if (this.tracker.GetDataTimeLength() < 10f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
				result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
			}
			else if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.FAIL;
			}
		}
		return result;
	}

	// Token: 0x0600603D RID: 24637 RVA: 0x002BA2B0 File Offset: 0x002B84B0
	private ColonyDiagnostic.DiagnosticResult CheckEnoughFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		List<MinionIdentity> list = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).FindAll((MinionIdentity MID) => Db.Get().Amounts.Calories.Lookup(MID) != null);
		if (this.tracker.GetDataTimeLength() < 10f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
		}
		else if ((float)list.Count * (1000f * this.recommendedKCalPerDuplicant) > this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			float currentValue = this.tracker.GetCurrentValue();
			float f = (float)list.Count * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE * this.multiplier;
			string formattedCalories = GameUtil.GetFormattedCalories(currentValue, GameUtil.TimeSlice.None, true);
			string formattedCalories2 = GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true);
			string text = MISC.NOTIFICATIONS.FOODLOW.TOOLTIP;
			text = text.Replace("{0}", formattedCalories);
			text = text.Replace("{1}", formattedCalories2);
			result.Message = text;
		}
		return result;
	}

	// Token: 0x0600603E RID: 24638 RVA: 0x002BA3DC File Offset: 0x002B85DC
	private ColonyDiagnostic.DiagnosticResult CheckStarvation()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (!minionIdentity.IsNull())
			{
				CalorieMonitor.Instance smi = minionIdentity.GetSMI<CalorieMonitor.Instance>();
				if (!smi.IsNullOrStopped() && smi.IsInsideState(smi.sm.hungry.starving))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
					result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.HUNGRY;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x0600603F RID: 24639 RVA: 0x000E333F File Offset: 0x000E153F
	public override string GetCurrentValueString()
	{
		return GameUtil.GetFormattedCalories(this.tracker.GetCurrentValue(), GameUtil.TimeSlice.None, true);
	}

	// Token: 0x06006040 RID: 24640 RVA: 0x002BA4B4 File Offset: 0x002B86B4
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x040044ED RID: 17645
	private const int CYCLES_OF_FOOD = 3;

	// Token: 0x040044EE RID: 17646
	private const float BASE_KCAL_PER_CYCLE = 1000f;

	// Token: 0x040044EF RID: 17647
	private float multiplier = 1f;

	// Token: 0x040044F0 RID: 17648
	private float recommendedKCalPerDuplicant;
}
