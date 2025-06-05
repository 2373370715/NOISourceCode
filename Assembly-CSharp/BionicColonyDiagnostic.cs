using System;
using System.Collections.Generic;

// Token: 0x0200125B RID: 4699
public abstract class BionicColonyDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005FFC RID: 24572 RVA: 0x000AA12F File Offset: 0x000A832F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x000E3127 File Offset: 0x000E1327
	public BionicColonyDiagnostic(int worldID, string name) : base(worldID, name)
	{
		this.RefreshData();
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x002B9138 File Offset: 0x002B7338
	protected void RefreshData()
	{
		Components.Cmps<MinionIdentity> cmps;
		if (Components.LiveMinionIdentitiesByModel.TryGetValue(BionicMinionConfig.MODEL, out cmps))
		{
			this.bionics = cmps.GetWorldItems(base.worldID, true, new Func<MinionIdentity, bool>(this.MinionFilter));
			return;
		}
		this.bionics = new List<MinionIdentity>();
	}

	// Token: 0x06005FFF RID: 24575 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected virtual bool MinionFilter(MinionIdentity minion)
	{
		return true;
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x002B9184 File Offset: 0x002B7384
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (this.ignoreInIdleRockets && ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		this.RefreshData();
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = this.GetDefaultResultMessage();
		}
		return diagnosticResult;
	}

	// Token: 0x06006001 RID: 24577
	protected abstract string GetDefaultResultMessage();

	// Token: 0x040044C4 RID: 17604
	protected const bool INCLUDE_CHILD_WORLDS = true;

	// Token: 0x040044C5 RID: 17605
	protected List<MinionIdentity> bionics;

	// Token: 0x040044C6 RID: 17606
	protected bool ignoreInIdleRockets = true;
}
