using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C2C RID: 7212
public class AlternateSiblingColor : KMonoBehaviour
{
	// Token: 0x06009615 RID: 38421 RVA: 0x003AB074 File Offset: 0x003A9274
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int siblingIndex = base.transform.GetSiblingIndex();
		this.RefreshColor(siblingIndex % 2 == 0);
	}

	// Token: 0x06009616 RID: 38422 RVA: 0x00106325 File Offset: 0x00104525
	private void RefreshColor(bool evenIndex)
	{
		if (this.image == null)
		{
			return;
		}
		this.image.color = (evenIndex ? this.evenColor : this.oddColor);
	}

	// Token: 0x06009617 RID: 38423 RVA: 0x00106352 File Offset: 0x00104552
	private void Update()
	{
		if (this.mySiblingIndex != base.transform.GetSiblingIndex())
		{
			this.mySiblingIndex = base.transform.GetSiblingIndex();
			this.RefreshColor(this.mySiblingIndex % 2 == 0);
		}
	}

	// Token: 0x040074BC RID: 29884
	public Color evenColor;

	// Token: 0x040074BD RID: 29885
	public Color oddColor;

	// Token: 0x040074BE RID: 29886
	public Image image;

	// Token: 0x040074BF RID: 29887
	private int mySiblingIndex;
}
