using System;

// Token: 0x02000AA6 RID: 2726
public class KSelectableHealthBar : KSelectable
{
	// Token: 0x060031CD RID: 12749 RVA: 0x0020DBEC File Offset: 0x0020BDEC
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04002215 RID: 8725
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04002216 RID: 8726
	private int scaleAmount = 100;
}
