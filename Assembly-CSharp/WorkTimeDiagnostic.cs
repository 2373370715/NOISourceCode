using System;
using STRINGS;

// Token: 0x02001278 RID: 4728
public class WorkTimeDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006074 RID: 24692 RVA: 0x002BBC5C File Offset: 0x002B9E5C
	public WorkTimeDiagnostic(int worldID, ChoreGroup choreGroup) : base(worldID, UI.COLONY_DIAGNOSTICS.WORKTIMEDIAGNOSTIC.ALL_NAME)
	{
		this.choreGroup = choreGroup;
		this.tracker = TrackerTool.Instance.GetWorkTimeTracker(worldID, choreGroup);
		this.trackerSampleCountSeconds = 100f;
		this.name = choreGroup.Name;
		this.id = "WorkTimeDiagnostic_" + choreGroup.Id;
		this.colors[ColonyDiagnostic.DiagnosticResult.Opinion.Good] = Constants.NEUTRAL_COLOR;
	}

	// Token: 0x06006075 RID: 24693 RVA: 0x002BBCD4 File Offset: 0x002B9ED4
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ((this.tracker.GetAverageValue(this.trackerSampleCountSeconds) > 0f) ? ColonyDiagnostic.DiagnosticResult.Opinion.Good : ColonyDiagnostic.DiagnosticResult.Opinion.Normal),
			Message = string.Format(UI.COLONY_DIAGNOSTICS.ALLWORKTIMEDIAGNOSTIC.NORMAL, this.tracker.FormatValueString(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)))
		};
	}

	// Token: 0x040044FB RID: 17659
	public ChoreGroup choreGroup;
}
