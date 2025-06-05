using System;
using UnityEngine;

// Token: 0x02001F22 RID: 7970
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
	// Token: 0x0600A7A8 RID: 42920 RVA: 0x0011147C File Offset: 0x0010F67C
	public void SetMainEntry(ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenHeaderRow>();
		}
		this.mainRow.SetLine(reportGroup);
	}

	// Token: 0x0400839A RID: 33690
	[SerializeField]
	private ReportScreenHeaderRow rowTemplate;

	// Token: 0x0400839B RID: 33691
	private ReportScreenHeaderRow mainRow;
}
