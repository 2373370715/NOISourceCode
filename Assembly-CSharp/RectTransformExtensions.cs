using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public static class RectTransformExtensions
{
	// Token: 0x06001C2F RID: 7215 RVA: 0x001B8534 File Offset: 0x001B6734
	public static RectTransform Fill(this RectTransform rectTransform)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(0f, 0f);
		rectTransform.sizeDelta = new Vector2(0f, 0f);
		return rectTransform;
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x001B8598 File Offset: 0x001B6798
	public static RectTransform Fill(this RectTransform rectTransform, Padding padding)
	{
		rectTransform.anchorMin = new Vector2(0f, 0f);
		rectTransform.anchorMax = new Vector2(1f, 1f);
		rectTransform.anchoredPosition = new Vector2(padding.left, padding.bottom);
		rectTransform.sizeDelta = new Vector2(-padding.right, -padding.top);
		return rectTransform;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000B6F23 File Offset: 0x000B5123
	public static RectTransform Pivot(this RectTransform rectTransform, float x, float y)
	{
		rectTransform.pivot = new Vector2(x, y);
		return rectTransform;
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000B6F33 File Offset: 0x000B5133
	public static RectTransform Pivot(this RectTransform rectTransform, Vector2 pivot)
	{
		rectTransform.pivot = pivot;
		return rectTransform;
	}
}
