using System;
using STRINGS;

// Token: 0x0200125D RID: 4701
public class ChoreGroupDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006007 RID: 24583 RVA: 0x002B94B4 File Offset: 0x002B76B4
	public ChoreGroupDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.CHOREGROUPDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetChoreGroupTracker(worldID, choreGroup);
		this.name = choreGroup.Name;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.id = "ChoreGroupDiagnostic_" + choreGroup.Id;
	}

	// Token: 0x06006008 RID: 24584 RVA: 0x002B9520 File Offset: 0x002B7720
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetCurrentValue() > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}

	// Token: 0x040044C7 RID: 17607
	public ChoreGroup choreGroup;
}
