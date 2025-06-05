using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001273 RID: 4723
public class SelfChargingElectrobankDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006061 RID: 24673 RVA: 0x002BB2BC File Offset: 0x002B94BC
	public SelfChargingElectrobankDiagnostic(int worldID) : base(worldID, UI.SELFCHARGINGBATTERYDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_radiation";
		base.AddCriterion("CheckLifetime", new DiagnosticCriterion(UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA.CHECKSELFCHARGINGBATTERYLIFE, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckLifetime)));
	}

	// Token: 0x06006062 RID: 24674 RVA: 0x000E3403 File Offset: 0x000E1603
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1.Concat(DlcManager.DLC3);
	}

	// Token: 0x06006063 RID: 24675 RVA: 0x002BB318 File Offset: 0x002B9518
	private ColonyDiagnostic.DiagnosticResult CheckLifetime()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.SELFCHARGINGBATTERYDIAGNOSTIC.NORMAL, null);
		foreach (SelfChargingElectrobank selfChargingElectrobank in Components.SelfChargingElectrobanks.GetItems(base.worldID))
		{
			if (selfChargingElectrobank.LifetimeRemaining <= this.WARNING_LIFETIME)
			{
				diagnosticResult.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				if (diagnosticResult.clickThroughObjects == null)
				{
					diagnosticResult.clickThroughObjects = new List<GameObject>();
				}
				diagnosticResult.clickThroughObjects.Add(selfChargingElectrobank.gameObject);
				diagnosticResult.Message = UI.SELFCHARGINGBATTERYDIAGNOSTIC.CRITERIA_BATTERYLIFE_WARNING;
			}
		}
		return diagnosticResult;
	}

	// Token: 0x040044F4 RID: 17652
	private float WARNING_LIFETIME = 600f;
}
