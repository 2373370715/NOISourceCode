using System;
using UnityEngine;

// Token: 0x02000623 RID: 1571
public static class PresUtil
{
	// Token: 0x06001BFD RID: 7165 RVA: 0x001B81C4 File Offset: 0x001B63C4
	public static Promise MoveAndFade(RectTransform rect, Vector2 targetAnchoredPosition, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		CanvasGroup canvasGroup = rect.FindOrAddComponent<CanvasGroup>();
		return rect.FindOrAddComponent<CoroutineRunner>().Run(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float f)
			{
				canvasGroup.alpha = f;
			}, canvasGroup.alpha, targetAlpha, duration, easing, -1f),
			Updater.Ease(delegate(Vector2 v2)
			{
				rect.anchoredPosition = v2;
			}, rect.anchoredPosition, targetAnchoredPosition, duration, easing, -1f)
		}));
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x001B8268 File Offset: 0x001B6468
	public static Promise OffsetFromAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		return PresUtil.MoveAndFade(rect, offset + anchoredPosition, targetAlpha, duration, easing);
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x001B8290 File Offset: 0x001B6490
	public static Promise OffsetToAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		rect.anchoredPosition = offset + anchoredPosition;
		return PresUtil.MoveAndFade(rect, anchoredPosition, targetAlpha, duration, easing);
	}
}
