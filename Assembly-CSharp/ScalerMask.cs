using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001CFE RID: 7422
[AddComponentMenu("KMonoBehaviour/scripts/ScalerMask")]
public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06009AE8 RID: 39656 RVA: 0x001093EE File Offset: 0x001075EE
	private RectTransform ThisTransform
	{
		get
		{
			if (this._thisTransform == null)
			{
				this._thisTransform = base.GetComponent<RectTransform>();
			}
			return this._thisTransform;
		}
	}

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x06009AE9 RID: 39657 RVA: 0x00109410 File Offset: 0x00107610
	private LayoutElement ThisLayoutElement
	{
		get
		{
			if (this._thisLayoutElement == null)
			{
				this._thisLayoutElement = base.GetComponent<LayoutElement>();
			}
			return this._thisLayoutElement;
		}
	}

	// Token: 0x06009AEA RID: 39658 RVA: 0x003CA278 File Offset: 0x003C8478
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Combine(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Combine(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
	}

	// Token: 0x06009AEB RID: 39659 RVA: 0x003CA2E0 File Offset: 0x003C84E0
	protected override void OnCleanUp()
	{
		DetailsScreen componentInParent = base.GetComponentInParent<DetailsScreen>();
		if (componentInParent)
		{
			DetailsScreen detailsScreen = componentInParent;
			detailsScreen.pointerEnterActions = (KScreen.PointerEnterActions)Delegate.Remove(detailsScreen.pointerEnterActions, new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent));
			DetailsScreen detailsScreen2 = componentInParent;
			detailsScreen2.pointerExitActions = (KScreen.PointerExitActions)Delegate.Remove(detailsScreen2.pointerExitActions, new KScreen.PointerExitActions(this.OnPointerExitGrandparent));
		}
		base.OnCleanUp();
	}

	// Token: 0x06009AEC RID: 39660 RVA: 0x003CA348 File Offset: 0x003C8548
	private void Update()
	{
		if (this.SourceTransform != null)
		{
			this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
		}
		if (this.SourceTransform != null && (!this.hoverLock || !this.grandparentIsHovered || this.isHovered || this.queuedSizeUpdate))
		{
			this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
			this.SourceTransform.anchoredPosition = new Vector2(0f, -this.topPadding);
			this.queuedSizeUpdate = false;
		}
		if (this.hoverIndicator != null)
		{
			if (this.SourceTransform != null && this.SourceTransform.rect.height > this.ThisTransform.rect.height)
			{
				this.hoverIndicator.SetActive(true);
				return;
			}
			this.hoverIndicator.SetActive(false);
		}
	}

	// Token: 0x06009AED RID: 39661 RVA: 0x00109432 File Offset: 0x00107632
	public void UpdateSize()
	{
		this.queuedSizeUpdate = true;
	}

	// Token: 0x06009AEE RID: 39662 RVA: 0x0010943B File Offset: 0x0010763B
	public void OnPointerEnterGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = true;
	}

	// Token: 0x06009AEF RID: 39663 RVA: 0x00109444 File Offset: 0x00107644
	public void OnPointerExitGrandparent(PointerEventData eventData)
	{
		this.grandparentIsHovered = false;
	}

	// Token: 0x06009AF0 RID: 39664 RVA: 0x0010944D File Offset: 0x0010764D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isHovered = true;
	}

	// Token: 0x06009AF1 RID: 39665 RVA: 0x00109456 File Offset: 0x00107656
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isHovered = false;
	}

	// Token: 0x04007908 RID: 30984
	public RectTransform SourceTransform;

	// Token: 0x04007909 RID: 30985
	private RectTransform _thisTransform;

	// Token: 0x0400790A RID: 30986
	private LayoutElement _thisLayoutElement;

	// Token: 0x0400790B RID: 30987
	public GameObject hoverIndicator;

	// Token: 0x0400790C RID: 30988
	public bool hoverLock;

	// Token: 0x0400790D RID: 30989
	private bool grandparentIsHovered;

	// Token: 0x0400790E RID: 30990
	private bool isHovered;

	// Token: 0x0400790F RID: 30991
	private bool queuedSizeUpdate = true;

	// Token: 0x04007910 RID: 30992
	public float topPadding;

	// Token: 0x04007911 RID: 30993
	public float bottomPadding;
}
