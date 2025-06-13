using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TemporaryActionRow : KMonoBehaviour, IRender200ms
{
	public float MaxHeight { get; private set; }

	public bool IsVisible { get; private set; }

	public bool ShouldProgressBarBeEnabled
	{
		get
		{
			return this.ShowTimeout && this.Lifetime > 0f && this.lastSpecifiedLifetime > 0f;
		}
	}

	public float Lifetime { get; private set; } = -1f;

	public bool ShowTimeout { get; set; } = true;

	public bool ShowOnSpawn { get; set; } = true;

	public bool HideOnClick { get; set; } = true;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.button = base.GetComponent<Button>();
		this.button.onClick.AddListener(new UnityAction(this._OnRowClicked));
		this.MaxHeight = this.layoutElement.minHeight;
		this.HideImmediatly();
	}

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

	private void _OnRowHidden()
	{
		Action<TemporaryActionRow> onRowHidden = this.OnRowHidden;
		if (onRowHidden == null)
		{
			return;
		}
		onRowHidden(this);
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.RefreshContentWidth();
		}
	}

	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.IsVisible)
		{
			this.HideImmediatly();
			this._OnRowHidden();
	}

	public void SetLifetime(float lifetime)
	{
		this.Lifetime = lifetime;
		this.lastSpecifiedLifetime = lifetime;
	}

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

	public void Setup(string text, string tooltip, Sprite icon = null)
	{
		this.Label.SetText(text);
		this.Tooltip.SetSimpleTooltip(tooltip);
		this.Image.sprite = icon;
	}

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

	public void HideImmediatly()
	{
		this.AbortCoroutine();
		this.IsVisible = false;
		this.Content.localPosition = new Vector3(-(base.transform as RectTransform).sizeDelta.x, this.Content.localPosition.y, this.Content.localPosition.z);
		this.layoutElement.minHeight = 0f;
	}

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

	private void AbortCoroutine()
	{
		if (this.layoutCoroutine != null)
		{
			base.StopCoroutine(this.layoutCoroutine);
			this.layoutCoroutine = null;
	}

	private void RefreshContentWidth()
	{
		RectTransform rectTransform = base.transform as RectTransform;
		if (rectTransform.sizeDelta.x != this.Content.sizeDelta.x)
		{
			this.Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.sizeDelta.x);
	}

	private void SetContentToHiddenPosition()
	{
		this.RefreshContentWidth();
		Vector3 v = this.Content.anchoredPosition;
		v.x = -(base.transform as RectTransform).sizeDelta.x;
	}

	private Coroutine RunEnterSlideAnimation(System.Action onAnimationEnds = null)
	{
	}

	private Coroutine RunExitSlideAnimation(System.Action onAnimationEnds = null)
	{
	}

	private Coroutine RunEnterHeightAnimation(System.Action onAnimationEnds = null)
	{
	}

	private Coroutine RunExitHeightAnimation(System.Action onAnimationEnds = null)
	{
	}

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
	}

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
	}

	public const float ROW_HEIGHT_ANIM_ENTRY_DURATION = 0.5f;

	public const float ROW_HEIGHT_ANIM_EXIT_DURATION = 0.3f;

	public const float SLIDE_ENTER_ANIM_DURATION = 0.4f;

	public const float SLIDE_EXIT_ANIM_DURATION = 0.4f;

	public RectTransform Content;

	public RectTransform IconSection;

	public RectTransform TimeoutBarSection;

	public KImage Image;

	public Image TimeoutImage;

	public LocText Label;

	public ToolTip Tooltip;

	public Action<TemporaryActionRow> OnRowClicked;

	public Action<TemporaryActionRow> OnRowHidden;

	private LayoutElement layoutElement;

	private Coroutine layoutCoroutine;

	private Button button;

	private bool HasBeenShown;

	private float lastSpecifiedLifetime = -1f;
}
