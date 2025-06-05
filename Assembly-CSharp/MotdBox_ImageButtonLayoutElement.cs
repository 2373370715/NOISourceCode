using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E97 RID: 7831
public class MotdBox_ImageButtonLayoutElement : LayoutElement
{
	// Token: 0x0600A42F RID: 42031 RVA: 0x003F45DC File Offset: 0x003F27DC
	private void UpdateState()
	{
		MotdBox_ImageButtonLayoutElement.Style style = this.style;
		if (style == MotdBox_ImageButtonLayoutElement.Style.WidthExpandsBasedOnHeight)
		{
			this.flexibleHeight = 1f;
			this.preferredHeight = -1f;
			this.minHeight = -1f;
			this.flexibleWidth = 0f;
			this.preferredWidth = this.rectTransform().sizeDelta.y * this.heightToWidthRatio;
			this.minWidth = this.preferredWidth;
			this.ignoreLayout = false;
			return;
		}
		if (style != MotdBox_ImageButtonLayoutElement.Style.HeightExpandsBasedOnWidth)
		{
			return;
		}
		this.flexibleWidth = 1f;
		this.preferredWidth = -1f;
		this.minWidth = -1f;
		this.flexibleHeight = 0f;
		this.preferredHeight = this.rectTransform().sizeDelta.x / this.heightToWidthRatio;
		this.minHeight = this.preferredHeight;
		this.ignoreLayout = false;
	}

	// Token: 0x0600A430 RID: 42032 RVA: 0x0010EFC6 File Offset: 0x0010D1C6
	protected override void OnTransformParentChanged()
	{
		this.UpdateState();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600A431 RID: 42033 RVA: 0x0010EFD4 File Offset: 0x0010D1D4
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateState();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04008059 RID: 32857
	[SerializeField]
	private float heightToWidthRatio;

	// Token: 0x0400805A RID: 32858
	[SerializeField]
	private MotdBox_ImageButtonLayoutElement.Style style;

	// Token: 0x02001E98 RID: 7832
	private enum Style
	{
		// Token: 0x0400805C RID: 32860
		WidthExpandsBasedOnHeight,
		// Token: 0x0400805D RID: 32861
		HeightExpandsBasedOnWidth
	}
}
