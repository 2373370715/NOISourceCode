using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B8D RID: 7053
public static class ResearchButtonImageToggleStateUtilityFunctions
{
	// Token: 0x06009409 RID: 37897 RVA: 0x0039C274 File Offset: 0x0039A474
	public static void Opacity(this Graphic graphic, float opacity)
	{
		Color color = graphic.color;
		color.a = opacity;
		graphic.color = color;
	}

	// Token: 0x0600940A RID: 37898 RVA: 0x0039C298 File Offset: 0x0039A498
	public static WaitUntil FadeAway(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(0f);
				return true;
			}
			float num = timer / duration;
			num = 1f - num;
			graphic.Opacity(startingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}

	// Token: 0x0600940B RID: 37899 RVA: 0x0039C2F0 File Offset: 0x0039A4F0
	public static WaitUntil FadeToVisible(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		float remainingOpacity = 1f - graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(1f);
				return true;
			}
			float num = timer / duration;
			graphic.Opacity(startingOpacity + remainingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}
}
