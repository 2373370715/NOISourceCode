using System;
using STRINGS;

// Token: 0x02001256 RID: 4694
public class AllChoresDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005FE8 RID: 24552 RVA: 0x000E3048 File Offset: 0x000E1248
	public AllChoresDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<AllChoresCountTracker>(worldID);
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
		this.icon = "icon_errand_operate";
	}

	// Token: 0x06005FE9 RID: 24553 RVA: 0x002B848C File Offset: 0x002B668C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetCurrentValue()))
		};
	}
}
