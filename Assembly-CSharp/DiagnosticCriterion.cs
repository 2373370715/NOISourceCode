using System;

// Token: 0x02001263 RID: 4707
public class DiagnosticCriterion
{
	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06006023 RID: 24611 RVA: 0x000E3271 File Offset: 0x000E1471
	// (set) Token: 0x06006024 RID: 24612 RVA: 0x000E3279 File Offset: 0x000E1479
	public string id { get; private set; }

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06006025 RID: 24613 RVA: 0x000E3282 File Offset: 0x000E1482
	// (set) Token: 0x06006026 RID: 24614 RVA: 0x000E328A File Offset: 0x000E148A
	public string name { get; private set; }

	// Token: 0x06006027 RID: 24615 RVA: 0x000E3293 File Offset: 0x000E1493
	public DiagnosticCriterion(string name, Func<ColonyDiagnostic.DiagnosticResult> action)
	{
		this.name = name;
		this.evaluateAction = action;
	}

	// Token: 0x06006028 RID: 24616 RVA: 0x000E32A9 File Offset: 0x000E14A9
	public void SetID(string id)
	{
		this.id = id;
	}

	// Token: 0x06006029 RID: 24617 RVA: 0x000E32B2 File Offset: 0x000E14B2
	public ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		return this.evaluateAction();
	}

	// Token: 0x040044E8 RID: 17640
	private Func<ColonyDiagnostic.DiagnosticResult> evaluateAction;
}
