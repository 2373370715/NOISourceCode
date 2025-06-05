using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001C94 RID: 7316
public class CodexImageLayoutMB : UIBehaviour
{
	// Token: 0x06009884 RID: 39044 RVA: 0x003BEE94 File Offset: 0x003BD094
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		if (this.image.preserveAspect && this.image.sprite != null && this.image.sprite)
		{
			float num = this.image.sprite.rect.height / this.image.sprite.rect.width;
			this.layoutElement.preferredHeight = num * this.rectTransform.sizeDelta.x;
			this.layoutElement.minHeight = this.layoutElement.preferredHeight;
			return;
		}
		this.layoutElement.preferredHeight = -1f;
		this.layoutElement.preferredWidth = -1f;
		this.layoutElement.minHeight = -1f;
		this.layoutElement.minWidth = -1f;
		this.layoutElement.flexibleHeight = -1f;
		this.layoutElement.flexibleWidth = -1f;
		this.layoutElement.ignoreLayout = false;
	}

	// Token: 0x0400768D RID: 30349
	public RectTransform rectTransform;

	// Token: 0x0400768E RID: 30350
	public LayoutElement layoutElement;

	// Token: 0x0400768F RID: 30351
	public Image image;
}
