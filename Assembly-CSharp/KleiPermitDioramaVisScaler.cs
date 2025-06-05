using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001DAC RID: 7596
[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour
{
	// Token: 0x06009EB8 RID: 40632 RVA: 0x0010BBD0 File Offset: 0x00109DD0
	protected override void OnRectTransformDimensionsChange()
	{
		this.Layout();
	}

	// Token: 0x06009EB9 RID: 40633 RVA: 0x0010BBD8 File Offset: 0x00109DD8
	public void Layout()
	{
		KleiPermitDioramaVisScaler.Layout(this.root, this.scaleTarget, this.slot);
	}

	// Token: 0x06009EBA RID: 40634 RVA: 0x003DD97C File Offset: 0x003DBB7C
	public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot)
	{
		float aspectRatio = 2.125f;
		AspectRatioFitter aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
		aspectRatioFitter.aspectRatio = aspectRatio;
		aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
		float num = 1700f;
		float a = Mathf.Max(0.1f, root.rect.width) / num;
		float num2 = 800f;
		float b = Mathf.Max(0.1f, root.rect.height) / num2;
		float d = Mathf.Max(a, b);
		scaleTarget.localScale = Vector3.one * d;
		scaleTarget.sizeDelta = new Vector2(1700f, 800f);
		scaleTarget.anchorMin = Vector2.one * 0.5f;
		scaleTarget.anchorMax = Vector2.one * 0.5f;
		scaleTarget.pivot = Vector2.one * 0.5f;
		scaleTarget.anchoredPosition = Vector2.zero;
	}

	// Token: 0x04007CB3 RID: 31923
	public const float REFERENCE_WIDTH = 1700f;

	// Token: 0x04007CB4 RID: 31924
	public const float REFERENCE_HEIGHT = 800f;

	// Token: 0x04007CB5 RID: 31925
	[SerializeField]
	private RectTransform root;

	// Token: 0x04007CB6 RID: 31926
	[SerializeField]
	private RectTransform scaleTarget;

	// Token: 0x04007CB7 RID: 31927
	[SerializeField]
	private RectTransform slot;
}
