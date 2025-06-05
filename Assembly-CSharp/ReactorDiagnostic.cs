using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200126F RID: 4719
public class ReactorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006054 RID: 24660 RVA: 0x002BAD8C File Offset: 0x002B8F8C
	public ReactorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckTemperature", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKTEMPERATURE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTemperature)));
		base.AddCriterion("CheckCoolant", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA.CHECKCOOLANT, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckCoolant)));
	}

	// Token: 0x06006055 RID: 24661 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06006056 RID: 24662 RVA: 0x002BAE04 File Offset: 0x002B9004
	private ColonyDiagnostic.DiagnosticResult CheckTemperature()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.FuelTemperature > 1254.8625f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_TEMPERATURE_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}

	// Token: 0x06006057 RID: 24663 RVA: 0x002BAED0 File Offset: 0x002B90D0
	private ColonyDiagnostic.DiagnosticResult CheckCoolant()
	{
		List<Reactor> worldItems = Components.NuclearReactors.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.NORMAL;
		foreach (Reactor reactor in worldItems)
		{
			if (reactor.On && reactor.ReserveCoolantMass <= 45f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.REACTORDIAGNOSTIC.CRITERIA_COOLANT_WARNING;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(reactor.gameObject.transform.position, reactor.gameObject);
			}
		}
		return result;
	}
}
