using System;

// Token: 0x02000AA7 RID: 2727
public class KSelectableProgressBar : KSelectable
{
	// Token: 0x060031CF RID: 12751 RVA: 0x0020DC30 File Offset: 0x0020BE30
	public override string GetName()
	{
		int num = (int)(this.progressBar.PercentFull * (float)this.scaleAmount);
		return string.Format("{0} {1}/{2}", this.entityName, num, this.scaleAmount);
	}

	// Token: 0x04002217 RID: 8727
	[MyCmpGet]
	private ProgressBar progressBar;

	// Token: 0x04002218 RID: 8728
	private int scaleAmount = 100;
}
