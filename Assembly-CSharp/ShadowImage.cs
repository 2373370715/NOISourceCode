using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F7C RID: 8060
public class ShadowImage : ShadowRect
{
	// Token: 0x0600AA2F RID: 43567 RVA: 0x0041411C File Offset: 0x0041231C
	protected override void MatchRect()
	{
		base.MatchRect();
		if (this.RectMain == null || this.RectShadow == null)
		{
			return;
		}
		if (this.shadowImage == null)
		{
			this.shadowImage = this.RectShadow.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			this.mainImage = this.RectMain.GetComponent<Image>();
		}
		if (this.mainImage == null)
		{
			if (this.shadowImage != null)
			{
				this.shadowImage.color = Color.clear;
			}
			return;
		}
		if (this.shadowImage == null)
		{
			return;
		}
		if (this.shadowImage.sprite != this.mainImage.sprite)
		{
			this.shadowImage.sprite = this.mainImage.sprite;
		}
		if (this.shadowImage.color != this.shadowColor)
		{
			if (this.shadowImage.sprite != null)
			{
				this.shadowImage.color = this.shadowColor;
				return;
			}
			this.shadowImage.color = Color.clear;
		}
	}

	// Token: 0x040085F8 RID: 34296
	private Image shadowImage;

	// Token: 0x040085F9 RID: 34297
	private Image mainImage;
}
