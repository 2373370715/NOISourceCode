using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F15 RID: 7957
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class RawImageAspectRatioFitter : AspectRatioFitter
{
	// Token: 0x0600A75A RID: 42842 RVA: 0x00404904 File Offset: 0x00402B04
	private void UpdateAspectRatio()
	{
		if (this.targetImage != null && this.targetImage.texture != null)
		{
			base.aspectRatio = (float)this.targetImage.texture.width / (float)this.targetImage.texture.height;
			return;
		}
		base.aspectRatio = 1f;
	}

	// Token: 0x0600A75B RID: 42843 RVA: 0x0011107D File Offset: 0x0010F27D
	protected override void OnTransformParentChanged()
	{
		this.UpdateAspectRatio();
		base.OnTransformParentChanged();
	}

	// Token: 0x0600A75C RID: 42844 RVA: 0x0011108B File Offset: 0x0010F28B
	protected override void OnRectTransformDimensionsChange()
	{
		this.UpdateAspectRatio();
		base.OnRectTransformDimensionsChange();
	}

	// Token: 0x0400834D RID: 33613
	[SerializeField]
	private RawImage targetImage;
}
