using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F23 RID: 7971
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeaderRow")]
public class ReportScreenHeaderRow : KMonoBehaviour
{
	// Token: 0x0600A7AA RID: 42922 RVA: 0x00405C68 File Offset: 0x00403E68
	public void SetLine(ReportManager.ReportGroup reportGroup)
	{
		LayoutElement component = this.name.GetComponent<LayoutElement>();
		component.minWidth = (component.preferredWidth = this.nameWidth);
		this.spacer.minWidth = this.groupSpacerWidth;
		this.name.text = reportGroup.stringKey;
	}

	// Token: 0x0400839C RID: 33692
	[SerializeField]
	public new LocText name;

	// Token: 0x0400839D RID: 33693
	[SerializeField]
	private LayoutElement spacer;

	// Token: 0x0400839E RID: 33694
	[SerializeField]
	private Image bgImage;

	// Token: 0x0400839F RID: 33695
	public float groupSpacerWidth;

	// Token: 0x040083A0 RID: 33696
	private float nameWidth = 164f;

	// Token: 0x040083A1 RID: 33697
	[SerializeField]
	private Color oddRowColor;
}
