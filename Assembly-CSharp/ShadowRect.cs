using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F7D RID: 8061
public class ShadowRect : MonoBehaviour
{
	// Token: 0x0600AA31 RID: 43569 RVA: 0x00414248 File Offset: 0x00412448
	private void OnEnable()
	{
		if (this.RectShadow != null)
		{
			this.RectShadow.name = "Shadow_" + this.RectMain.name;
			this.MatchRect();
			return;
		}
		global::Debug.LogWarning("Shadowrect is missing rectshadow: " + base.gameObject.name);
	}

	// Token: 0x0600AA32 RID: 43570 RVA: 0x00112F03 File Offset: 0x00111103
	private void Update()
	{
		this.MatchRect();
	}

	// Token: 0x0600AA33 RID: 43571 RVA: 0x004142A4 File Offset: 0x004124A4
	protected virtual void MatchRect()
	{
		if (this.RectShadow == null || this.RectMain == null)
		{
			return;
		}
		if (this.shadowLayoutElement == null)
		{
			this.shadowLayoutElement = this.RectShadow.GetComponent<LayoutElement>();
		}
		if (this.shadowLayoutElement != null && !this.shadowLayoutElement.ignoreLayout)
		{
			this.shadowLayoutElement.ignoreLayout = true;
		}
		if (this.RectShadow.transform.parent != this.RectMain.transform.parent)
		{
			this.RectShadow.transform.SetParent(this.RectMain.transform.parent);
		}
		if (this.RectShadow.GetSiblingIndex() >= this.RectMain.GetSiblingIndex())
		{
			this.RectShadow.SetAsFirstSibling();
		}
		this.RectShadow.transform.localScale = Vector3.one;
		if (this.RectShadow.pivot != this.RectMain.pivot)
		{
			this.RectShadow.pivot = this.RectMain.pivot;
		}
		if (this.RectShadow.anchorMax != this.RectMain.anchorMax)
		{
			this.RectShadow.anchorMax = this.RectMain.anchorMax;
		}
		if (this.RectShadow.anchorMin != this.RectMain.anchorMin)
		{
			this.RectShadow.anchorMin = this.RectMain.anchorMin;
		}
		if (this.RectShadow.sizeDelta != this.RectMain.sizeDelta)
		{
			this.RectShadow.sizeDelta = this.RectMain.sizeDelta;
		}
		if (this.RectShadow.anchoredPosition != this.RectMain.anchoredPosition + this.ShadowOffset)
		{
			this.RectShadow.anchoredPosition = this.RectMain.anchoredPosition + this.ShadowOffset;
		}
		if (this.RectMain.gameObject.activeInHierarchy != this.RectShadow.gameObject.activeInHierarchy)
		{
			this.RectShadow.gameObject.SetActive(this.RectMain.gameObject.activeInHierarchy);
		}
	}

	// Token: 0x040085FA RID: 34298
	public RectTransform RectMain;

	// Token: 0x040085FB RID: 34299
	public RectTransform RectShadow;

	// Token: 0x040085FC RID: 34300
	[SerializeField]
	protected Color shadowColor = new Color(0f, 0f, 0f, 0.6f);

	// Token: 0x040085FD RID: 34301
	[SerializeField]
	protected Vector2 ShadowOffset = new Vector2(1.5f, -1.5f);

	// Token: 0x040085FE RID: 34302
	private LayoutElement shadowLayoutElement;
}
