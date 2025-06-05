using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200125C RID: 4700
public class BreathabilityDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006002 RID: 24578 RVA: 0x002B91D0 File Offset: 0x002B73D0
	public BreathabilityDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BreathabilityTracker>(worldID);
		this.trackerSampleCountSeconds = 50f;
		this.icon = "overlay_oxygen";
		base.AddCriterion("CheckSuffocation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKSUFFOCATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckSuffocation)));
		base.AddCriterion("CheckLowBreathability", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBREATHABILITY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBreathability)));
		base.AddCriterion("CheckBionicOxygen", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.CRITERIA.CHECKLOWBIONICOXYGEN, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLowBionicOxygen)));
	}

	// Token: 0x06006003 RID: 24579 RVA: 0x002B9288 File Offset: 0x002B7488
	private ColonyDiagnostic.DiagnosticResult CheckSuffocation()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (worldItems.Count != 0)
		{
			using (List<MinionIdentity>.Enumerator enumerator = worldItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					SuffocationMonitor.Instance smi = cmp.GetSMI<SuffocationMonitor.Instance>();
					if (smi != null && smi.IsInsideState(smi.sm.noOxygen.suffocating))
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.SUFFOCATING, new global::Tuple<Vector3, GameObject>(smi.transform.position, smi.gameObject));
					}
				}
				goto IL_9B;
			}
			goto IL_8D;
			IL_9B:
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
		}
		IL_8D:
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
	}

	// Token: 0x06006004 RID: 24580 RVA: 0x002B9354 File Offset: 0x002B7554
	private ColonyDiagnostic.DiagnosticResult CheckLowBreathability()
	{
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0 && this.tracker.GetAverageValue(this.trackerSampleCountSeconds) < 60f)
		{
			return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR, null);
		}
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
	}

	// Token: 0x06006005 RID: 24581 RVA: 0x002B93B4 File Offset: 0x002B75B4
	private ColonyDiagnostic.DiagnosticResult CheckLowBionicOxygen()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		if (worldItems.Count != 0)
		{
			foreach (MinionIdentity minionIdentity in worldItems)
			{
				if (minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
				{
					BionicOxygenTankMonitor.Instance smi = minionIdentity.GetSMI<BionicOxygenTankMonitor.Instance>();
					if (smi.OxygenPercentage <= 0f)
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NEAR_OR_EMPTY_BIONIC_TANKS, new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject));
					}
					if (smi.OxygenPercentage < 0.5f)
					{
						return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.POOR_BIONIC_TANKS, new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject));
					}
				}
			}
		}
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.BREATHABILITYDIAGNOSTIC.NORMAL, null);
	}

	// Token: 0x06006006 RID: 24582 RVA: 0x002B8978 File Offset: 0x002B6B78
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}
}
