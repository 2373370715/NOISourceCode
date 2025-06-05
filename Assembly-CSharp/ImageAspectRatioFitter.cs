using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D55 RID: 7509
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class ImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x06009CBE RID: 40126 RVA: 0x003D3454 File Offset: 0x003D1654
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.sprite != null)
		{
			base.aspectRatio = this.targetImage.sprite.rect.width / this.targetImage.sprite.rect.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x06009CBF RID: 40127 RVA: 0x0010A6C1 File Offset: 0x001088C1
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x06009CC0 RID: 40128 RVA: 0x0010A6CF File Offset: 0x001088CF
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x04007ACA RID: 31434
	[SerializeField]
	private Image targetImage;
}
