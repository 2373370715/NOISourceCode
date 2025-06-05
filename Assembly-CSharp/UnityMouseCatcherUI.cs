using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C20 RID: 3104
public class UnityMouseCatcherUI
{
	// Token: 0x06003ADA RID: 15066 RVA: 0x002366F8 File Offset: 0x002348F8
	public static Canvas ManifestCanvas()
	{
		if (UnityMouseCatcherUI.m_instance_canvas != null && UnityMouseCatcherUI.m_instance_canvas)
		{
			return UnityMouseCatcherUI.m_instance_canvas;
		}
		GameObject gameObject = new GameObject("UnityMouseCatcherUI Canvas");
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 32767;
		canvas.pixelPerfect = false;
		UnityMouseCatcherUI.m_instance_canvas = canvas;
		gameObject.AddComponent<GraphicRaycaster>();
		GameObject gameObject2 = new GameObject("ImGui Consume Input", new Type[]
		{
			typeof(RectTransform)
		});
		gameObject2.transform.SetParent(gameObject.transform, false);
		RectTransform component = gameObject2.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.sizeDelta = Vector2.zero;
		component.anchoredPosition = Vector2.zero;
		Image image = gameObject2.AddComponent<Image>();
		image.sprite = Resources.Load<Sprite>("1x1_white");
		image.color = new Color(1f, 1f, 1f, 0f);
		image.raycastTarget = true;
		return UnityMouseCatcherUI.m_instance_canvas;
	}

	// Token: 0x06003ADB RID: 15067 RVA: 0x00236800 File Offset: 0x00234A00
	public static void SetEnabled(bool is_enabled)
	{
		Canvas canvas = UnityMouseCatcherUI.ManifestCanvas();
		if (canvas.gameObject.activeSelf != is_enabled)
		{
			canvas.gameObject.SetActive(is_enabled);
		}
		if (canvas.enabled != is_enabled)
		{
			canvas.enabled = is_enabled;
		}
	}

	// Token: 0x040028A8 RID: 10408
	private static Canvas m_instance_canvas;
}
