using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02001262 RID: 4706
public class DecorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006020 RID: 24608 RVA: 0x002B99C8 File Offset: 0x002B7BC8
	public DecorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_category_decor";
		base.AddCriterion("CheckDecor", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.DECORDIAGNOSTIC.CRITERIA.CHECKDECOR, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckDecor)));
	}

	// Token: 0x06006021 RID: 24609 RVA: 0x002B9A18 File Offset: 0x002B7C18
	private ColonyDiagnostic.DiagnosticResult CheckDecor()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		return result;
	}

	// Token: 0x06006022 RID: 24610 RVA: 0x002B9A68 File Offset: 0x002B7C68
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
