using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020020A0 RID: 8352
public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600B21A RID: 45594 RVA: 0x001184F2 File Offset: 0x001166F2
	private void Awake()
	{
		this.Selectable = base.GetComponent<Selectable>();
	}

	// Token: 0x0600B21B RID: 45595 RVA: 0x00118500 File Offset: 0x00116700
	public void OnPointerEnter(PointerEventData data)
	{
		this.Direction = 1f;
	}

	// Token: 0x0600B21C RID: 45596 RVA: 0x0011850D File Offset: 0x0011670D
	public void OnPointerExit(PointerEventData data)
	{
		this.Direction = -1f;
	}

	// Token: 0x0600B21D RID: 45597 RVA: 0x0043BE60 File Offset: 0x0043A060
	private void Update()
	{
		if (this.Selectable.interactable)
		{
			float x = base.transform.localScale.x;
			float num = x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed;
			num = Mathf.Min(num, Tween.Scale);
			num = Mathf.Max(num, 1f);
			if (num != x)
			{
				base.transform.localScale = new Vector3(num, num, 1f);
			}
		}
	}

	// Token: 0x04008C9A RID: 35994
	private static float Scale = 1.025f;

	// Token: 0x04008C9B RID: 35995
	private static float ScaleSpeed = 0.5f;

	// Token: 0x04008C9C RID: 35996
	private Selectable Selectable;

	// Token: 0x04008C9D RID: 35997
	private float Direction = -1f;
}
