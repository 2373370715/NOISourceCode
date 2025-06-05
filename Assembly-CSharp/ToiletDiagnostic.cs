using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02001276 RID: 4726
public class ToiletDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006069 RID: 24681 RVA: 0x002BB544 File Offset: 0x002B9744
	public ToiletDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_toilet";
		this.tracker = TrackerTool.Instance.GetWorldTracker<WorkingToiletTracker>(worldID);
		this.NO_MINIONS_WITH_BLADDER = (base.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_MINIONS_PLANETOID);
		base.AddCriterion("CheckHasAnyToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKHASANYTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHasAnyToilets)));
		base.AddCriterion("CheckEnoughToilets", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKENOUGHTOILETS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughToilets)));
		base.AddCriterion("CheckBladders", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.CRITERIA.CHECKBLADDERS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckBladders)));
	}

	// Token: 0x0600606A RID: 24682 RVA: 0x002BB610 File Offset: 0x002B9810
	private ColonyDiagnostic.DiagnosticResult CheckHasAnyToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else if (this.toilets.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_TOILETS;
		}
		return result;
	}

	// Token: 0x0600606B RID: 24683 RVA: 0x002BB67C File Offset: 0x002B987C
	private ColonyDiagnostic.DiagnosticResult CheckEnoughToilets()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			if (this.tracker.GetDataTimeLength() > 10f && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) <= 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NO_WORKING_TOILETS;
			}
		}
		return result;
	}

	// Token: 0x0600606C RID: 24684 RVA: 0x002BB720 File Offset: 0x002B9920
	private ColonyDiagnostic.DiagnosticResult CheckBladders()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (this.minionsWithBladders.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = this.NO_MINIONS_WITH_BLADDER;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.NORMAL;
			WorldContainer world = ClusterManager.Instance.GetWorld(base.worldID);
			foreach (PeeChoreMonitor.Instance instance in Components.CriticalBladders.Items)
			{
				int myWorldId = instance.master.gameObject.GetMyWorldId();
				if (myWorldId == base.worldID || world.GetChildWorldIds().Contains(myWorldId))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
					result.Message = UI.COLONY_DIAGNOSTICS.TOILETDIAGNOSTIC.TOILET_URGENT;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600606D RID: 24685 RVA: 0x000E3450 File Offset: 0x000E1650
	private bool MinionFilter(MinionIdentity minion)
	{
		return minion.modifiers.amounts.Has(Db.Get().Amounts.Bladder);
	}

	// Token: 0x0600606E RID: 24686 RVA: 0x002BB818 File Offset: 0x002B9A18
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, this.NO_MINIONS_WITH_BLADDER, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		this.RefreshData();
		return base.Evaluate();
	}

	// Token: 0x0600606F RID: 24687 RVA: 0x000E3471 File Offset: 0x000E1671
	private void RefreshData()
	{
		this.minionsWithBladders = Components.LiveMinionIdentities.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
		this.toilets = Components.Toilets.GetWorldItems(base.worldID, true);
	}

	// Token: 0x06006070 RID: 24688 RVA: 0x002BB854 File Offset: 0x002B9A54
	public override string GetAverageValueString()
	{
		if (this.minionsWithBladders == null || this.minionsWithBladders.Count == 0)
		{
			this.RefreshData();
		}
		int num = this.toilets.Count;
		for (int i = 0; i < this.toilets.Count; i++)
		{
			if (!this.toilets[i].IsNullOrDestroyed() && !this.toilets[i].IsUsable())
			{
				num--;
			}
		}
		return num.ToString() + ":" + this.minionsWithBladders.Count.ToString();
	}

	// Token: 0x040044F7 RID: 17655
	private const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x040044F8 RID: 17656
	private List<MinionIdentity> minionsWithBladders;

	// Token: 0x040044F9 RID: 17657
	private List<IUsable> toilets;

	// Token: 0x040044FA RID: 17658
	private readonly string NO_MINIONS_WITH_BLADDER;
}
