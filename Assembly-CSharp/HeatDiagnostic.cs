using System;
using STRINGS;

// Token: 0x0200126A RID: 4714
public class HeatDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006044 RID: 24644 RVA: 0x002BA4F4 File Offset: 0x002B86F4
	public HeatDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<BatteryTracker>(worldID);
		this.trackerSampleCountSeconds = 4f;
		base.AddCriterion("CheckHeat", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.HEATDIAGNOSTIC.CRITERIA.CHECKHEAT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckHeat)));
	}

	// Token: 0x06006045 RID: 24645 RVA: 0x002BA554 File Offset: 0x002B8754
	private ColonyDiagnostic.DiagnosticResult CheckHeat()
	{
		return new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null)
		{
			opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			Message = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.NORMAL
		};
	}
}
