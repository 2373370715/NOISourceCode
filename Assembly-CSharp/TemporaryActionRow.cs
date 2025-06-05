using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02002091 RID: 8337
public class TemporaryActionRow : KMonoBehaviour, IRender200ms
{
	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x0600B1AB RID: 45483 RVA: 0x00117F3C File Offset: 0x0011613C
	// (set) Token: 0x0600B1AA RID: 45482 RVA: 0x00117F33 File Offset: 0x00116133
	public float MaxHeight { get; private set; }

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x0600B1AD RID: 45485 RVA: 0x00117F4D File Offset: 0x0011614D
	// (set) Token: 0x0600B1AC RID: 45484 RVA: 0x00117F44 File Offset: 0x00116144
	public bool IsVisible { get; private set; }

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x0600B1AE RID: 45486 RVA: 0x00117F55 File Offset: 0x00116155
	public bool ShouldProgressBarBeEnabled
	{
		get
		{
			return this.ShowTimeout && this.Lifetime > 0f && this.lastSpecifiedLifetime > 0f;
		}
	}

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x0600B1B0 RID: 45488 RVA: 0x00117F84 File Offset: 0x00116184
	// (set) Token: 0x0600B1AF RID: 45487 RVA: 0x00117F7B File Offset: 0x0011617B
	public float Lifetime { get; private set; } = -1f;

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x0600B1B2 RID: 45490 RVA: 0x00117F95 File Offset: 0x00116195
	// (set) Token: 0x0600B1B1 RID: 45489 RVA: 0x00117F8C File Offset: 0x0011618C
	public bool ShowTimeout { get; set; } = true;

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x0600B1B4 RID: 45492 RVA: 0x00117FA6 File Offset: 0x001161A6
	// (set) Token: 0x0600B1B3 RID: 45491 RVA: 0x00117F9D File Offset: 0x0011619D
	public bool ShowOnSpawn { get; set; } = true;

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x0600B1B6 RID: 45494 RVA: 0x00117FB7 File Offset: 0x001161B7
	// (set) Token: 0x0600B1B5 RID: 45493 RVA: 0x00117FAE File Offset: 0x001161AE
	public bool HideOnClick { get; set; } = true;

	// Token: 0x0600B1B7 RID: 45495 RVA: 0x0043A168 File Offset: 0x00438368
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this._OnRowClicked));
		this.MaxHeight = this.layoutElement.minHeight;
		this.HideImmediatly();
	}

	// Token: 0x0600B1B8 RID: 45496 RVA: 0x00117FBF File Offset: 0x001161BF
	private void Update()
	{
		if (!this.HasBeenShown && this.ShowOnSpawn)
		{
			this.RefreshContentWidth();
			if (this.Content.sizeDelta.x > 0f)
			{
				this.Show();
			}
		}
	}

	// Token: 0x0600B1B9 RID: 45497 RVA: 0x00117FF6 File Offset: 0x001161F6
	private void _OnRowClicked()
	{
		Action<TemporaryActionRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked != null)
		{
			onRowClicked(this);
		}
		if (this.HideOnClick)
		{
			this.Hide();
		}
	}

	// Token: 0x0600B1BA RID: 45498 RVA: 0x00118018 File Offset: 0x00116218
	private void _OnRowHidden()
	{
		Action<TemporaryActionRow> onRowHidden = this.OnRowHidden;
		if (onRowHidden == null)
		{
			return;
		}
		onRowHidden(this);
	}

	// Token: 0x0600B1BB RID: 45499 RVA: 0x0011802B File Offset: 0x0011622B
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.RefreshContentWidth();
		}
	}

	// Token: 0x0600B1BC RID: 45500 RVA: 0x00118041 File Offset: 0x00116241
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.IsVisible)
		{
			this.HideImmediatly();
			this._OnRowHidden();
		}
	}

	// Token: 0x0600B1BD RID: 45501 RVA: 0x0011805D File Offset: 0x0011625D
	public void SetLifetime(float lifetime)
	{
		this.Lifetime = lifetime;
		this.lastSpecifiedLifetime = lifetime;
		this.UpdateTimeout();
	}

	// Token: 0x0600B1BE RID: 45502 RVA: 0x0043A1C8 File Offset: 0x004383C8
	private void UpdateTimeout()
	{
		bool shouldProgressBarBeEnabled = this.ShouldProgressBarBeEnabled;
		if (shouldProgressBarBeEnabled != this.TimeoutBarSection.gameObject.activeInHierarchy)
		{
			this.TimeoutBarSection.gameObject.SetActive(shouldProgressBarBeEnabled);
		}
		if (shouldProgressBarBeEnabled)
		{
			this.TimeoutImage.fillAmount = Mathf.Clamp(this.Lifetime / this.lastSpecifiedLifetime, 0f, 1f);
		}
	}

	// Token: 0x0600B1BF RID: 45503 RVA: 0x0043A22C File Offset: 0x0043842C
	public void Render200ms(float dt)
	{
		if (this.HasBeenShown && this.Lifetime > 0f && this.IsVisible)
		{
			this.Lifetime -= dt;
			if (this.Lifetime <= 0f)
			{
				this.Hide();
			}
			this.UpdateTimeout();
		}
	}

	// Token: 0x0600B1C0 RID: 45504 RVA: 0x00118073 File Offset: 0x00116273
	public void Setup(string text, string tooltip, Sprite icon = null)
	{
		this.Label.SetText(text);
		this.Tooltip.SetSimpleTooltip(tooltip);
		this.Image.sprite = icon;
		this.IconSection.gameObject.SetActive(icon != null);
	}

	// Token: 0x0600B1C1 RID: 45505 RVA: 0x0043A280 File Offset: 0x00438480
	public void Show()
	{
		this.AbortCoroutine();
		this.IsVisible = true;
		this.HasBeenShown = true;
		this.button.interactable = true;
		if (base.gameObject.activeInHierarchy)
		{
			this.SetContentToHiddenPosition();
			this.layoutCoroutine = this.RunEnterHeightAnimation(delegate
			{
				this.layoutCoroutine = this.RunEnterSlideAnimation(null);
			});
		}
	}

	// Token: 0x0600B1C2 RID: 45506 RVA: 0x0043A2D8 File Offset: 0x004384D8
	public void HideImmediatly()
	{
		this.AbortCoroutine();
		this.IsVisible = false;
		this.Content.localPosition = new Vector3(-(base.transform as RectTransform).sizeDelta.x, this.Content.localPosition.y, this.Content.localPosition.z);
		this.layoutElement.minHeight = 0f;
		this.button.interactable = false;
	}

	// Token: 0x0600B1C3 RID: 45507 RVA: 0x001180B0 File Offset: 0x001162B0
	public void Hide()
	{
		this.AbortCoroutine();
		this.IsVisible = false;
		this.button.interactable = false;
		if (base.gameObject.activeInHierarchy)
		{
			this.layoutCoroutine = this.RunExitSlideAnimation(delegate
			{
				this.layoutCoroutine = this.RunExitHeightAnimation(new System.Action(this._OnRowHidden));
			});
		}
	}

	// Token: 0x0600B1C4 RID: 45508 RVA: 0x001180F0 File Offset: 0x001162F0
	private void AbortCoroutine()
	{
		if (this.layoutCoroutine != null)
		{
			base.StopCoroutine(this.layoutCoroutine);
			this.layoutCoroutine = null;
		}
	}

	// Token: 0x0600B1C5 RID: 45509 RVA: 0x0043A354 File Offset: 0x00438554
	private void RefreshContentWidth()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform.sizeDelta.x != this.Content.sizeDelta.x)
		{
			this.Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.sizeDelta.x);
		}
	}

	// Token: 0x0600B1C6 RID: 45510 RVA: 0x0043A3A4 File Offset: 0x004385A4
	private void SetContentToHiddenPosition()
	{
		this.RefreshContentWidth();
		Vector3 v = this.Content.anchoredPosition;
		v.x = -(base.transform as RectTransform).sizeDelta.x;
		this.Content.anchoredPosition = v;
	}

	// Token: 0x0600B1C7 RID: 45511 RVA: 0x0011810D File Offset: 0x0011630D
	private Coroutine RunEnterSlideAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.SlideTransitionAnimation(0.4f, true, (float n) => Mathf.Sqrt(n), onAnimationEnds));
	}

	// Token: 0x0600B1C8 RID: 45512 RVA: 0x00118141 File Offset: 0x00116341
	private Coroutine RunExitSlideAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.SlideTransitionAnimation(0.4f, false, (float n) => Mathf.Pow(n, 2f), onAnimationEnds));
	}

	// Token: 0x0600B1C9 RID: 45513 RVA: 0x00118175 File Offset: 0x00116375
	private Coroutine RunEnterHeightAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.HeightTransitionAnimation(0.5f, true, (float n) => Mathf.Sqrt(n), onAnimationEnds));
	}

	// Token: 0x0600B1CA RID: 45514 RVA: 0x001181A9 File Offset: 0x001163A9
	private Coroutine RunExitHeightAnimation(System.Action onAnimationEnds = null)
	{
		return base.StartCoroutine(this.HeightTransitionAnimation(0.3f, false, (float n) => Mathf.Pow(n, 2f), onAnimationEnds));
	}

	// Token: 0x0600B1CB RID: 45515 RVA: 0x001181DD File Offset: 0x001163DD
	private IEnumerator SlideTransitionAnimation(float duration, bool show, Func<float, float> curveModifier = null, System.Action onAnimationEnds = null)
	{
		float num = -(base.transform as RectTransform).sizeDelta.x;
		float num2 = 0f;
		float contentInitialXPosition = show ? num : this.Content.anchoredPosition.x;
		float targetPosition = show ? num2 : num;
		float timePassed = 0f;
		Vector3 v = this.Content.anchoredPosition;
		while (timePassed < duration)
		{
			this.RefreshContentWidth();
			float num3 = timePassed / duration;
			if (curveModifier != null)
			{
				num3 = curveModifier(num3);
			}
			v = this.Content.anchoredPosition;
			v.x = Mathf.Lerp(contentInitialXPosition, targetPosition, num3);
			this.Content.anchoredPosition = v;
			timePassed += Time.unscaledDeltaTime;
			yield return null;
		}
		this.RefreshContentWidth();
		v = this.Content.anchoredPosition;
		v.x = targetPosition;
		this.Content.anchoredPosition = v;
		yield return null;
		if (onAnimationEnds != null)
		{
			onAnimationEnds();
		}
		yield break;
	}

	// Token: 0x0600B1CC RID: 45516 RVA: 0x00118209 File Offset: 0x00116409
	private IEnumerator HeightTransitionAnimation(float duration, bool show, Func<float, float> curveModifier = null, System.Action onAnimationEnds = null)
	{
		Transform transform = base.transform;
		float initialHeight = this.layoutElement.minHeight;
		float targetHeight = show ? this.MaxHeight : 0f;
		float timePassed = 0f;
		float minHeight = this.layoutElement.minHeight;
		while (timePassed < duration)
		{
			this.RefreshContentWidth();
			float num = timePassed / duration;
			if (curveModifier != null)
			{
				num = curveModifier(num);
			}
			minHeight = Mathf.Lerp(initialHeight, targetHeight, num);
			this.layoutElement.minHeight = minHeight;
			timePassed += Time.unscaledDeltaTime;
			yield return null;
		}
		this.RefreshContentWidth();
		minHeight = targetHeight;
		this.layoutElement.minHeight = minHeight;
		yield return null;
		if (onAnimationEnds != null)
		{
			onAnimationEnds();
		}
		yield break;
	}

	// Token: 0x04008C11 RID: 35857
	public const float ROW_HEIGHT_ANIM_ENTRY_DURATION = 0.5f;

	// Token: 0x04008C12 RID: 35858
	public const float ROW_HEIGHT_ANIM_EXIT_DURATION = 0.3f;

	// Token: 0x04008C13 RID: 35859
	public const float SLIDE_ENTER_ANIM_DURATION = 0.4f;

	// Token: 0x04008C14 RID: 35860
	public const float SLIDE_EXIT_ANIM_DURATION = 0.4f;

	// Token: 0x04008C17 RID: 35863
	public RectTransform Content;

	// Token: 0x04008C18 RID: 35864
	public RectTransform IconSection;

	// Token: 0x04008C19 RID: 35865
	public RectTransform TimeoutBarSection;

	// Token: 0x04008C1A RID: 35866
	public KImage Image;

	// Token: 0x04008C1B RID: 35867
	public Image TimeoutImage;

	// Token: 0x04008C1C RID: 35868
	public LocText Label;

	// Token: 0x04008C1D RID: 35869
	public ToolTip Tooltip;

	// Token: 0x04008C1E RID: 35870
	public Action<TemporaryActionRow> OnRowClicked;

	// Token: 0x04008C1F RID: 35871
	public Action<TemporaryActionRow> OnRowHidden;

	// Token: 0x04008C20 RID: 35872
	private LayoutElement layoutElement;

	// Token: 0x04008C21 RID: 35873
	private Coroutine layoutCoroutine;

	// Token: 0x04008C22 RID: 35874
	private Button button;

	// Token: 0x04008C27 RID: 35879
	private bool HasBeenShown;

	// Token: 0x04008C28 RID: 35880
	private float lastSpecifiedLifetime = -1f;
}
