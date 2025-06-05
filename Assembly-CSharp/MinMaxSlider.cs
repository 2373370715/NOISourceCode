using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020CD RID: 8397
[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x0600B300 RID: 45824 RVA: 0x00118F21 File Offset: 0x00117121
	// (set) Token: 0x0600B301 RID: 45825 RVA: 0x00118F29 File Offset: 0x00117129
	public MinMaxSlider.Mode mode { get; private set; }

	// Token: 0x0600B302 RID: 45826 RVA: 0x0043F514 File Offset: 0x0043D714
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToolTip component = base.transform.parent.gameObject.GetComponent<ToolTip>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(this.toolTip);
			this.toolTip = component;
		}
		this.minSlider.value = this.currentMinValue;
		this.maxSlider.value = this.currentMaxValue;
		this.minSlider.interactable = this.interactable;
		this.maxSlider.interactable = this.interactable;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.direction = (this.maxSlider.direction = this.direction);
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = false;
		}
		this.minSlider.gameObject.SetActive(false);
		if (this.mode != MinMaxSlider.Mode.Single)
		{
			this.minSlider.gameObject.SetActive(true);
		}
		if (this.extraSlider != null)
		{
			this.extraSlider.value = this.currentExtraValue;
			this.extraSlider.wholeNumbers = (this.minSlider.wholeNumbers = (this.maxSlider.wholeNumbers = this.wholeNumbers));
			this.extraSlider.direction = this.direction;
			this.extraSlider.interactable = this.interactable;
			this.extraSlider.maxValue = this.maxLimit;
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.gameObject.SetActive(false);
			if (this.mode == MinMaxSlider.Mode.Triple)
			{
				this.extraSlider.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600B303 RID: 45827 RVA: 0x0043F704 File Offset: 0x0043D904
	public void SetIcon(Image newIcon)
	{
		this.icon = newIcon;
		this.icon.gameObject.transform.SetParent(base.transform);
		this.icon.gameObject.transform.SetAsFirstSibling();
		this.icon.rectTransform().anchoredPosition = Vector2.zero;
	}

	// Token: 0x0600B304 RID: 45828 RVA: 0x0043F760 File Offset: 0x0043D960
	public void SetMode(MinMaxSlider.Mode mode)
	{
		this.mode = mode;
		if (mode == MinMaxSlider.Mode.Single && this.extraSlider != null)
		{
			this.extraSlider.gameObject.SetActive(false);
			this.extraSlider.handleRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B305 RID: 45829 RVA: 0x00118F32 File Offset: 0x00117132
	private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
	{
		trans.anchorMin = min;
		trans.anchorMax = max;
	}

	// Token: 0x0600B306 RID: 45830 RVA: 0x0043F7AC File Offset: 0x0043D9AC
	public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
	{
		this.minSlider.value = currentMin;
		this.currentMinValue = currentMin;
		this.maxSlider.value = currentMax;
		this.currentMaxValue = currentMax;
		this.minLimit = min;
		this.maxLimit = max;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		if (this.extraSlider != null)
		{
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.maxValue = this.maxLimit;
		}
	}

	// Token: 0x0600B307 RID: 45831 RVA: 0x00118F42 File Offset: 0x00117142
	public void SetExtraValue(float current)
	{
		this.extraSlider.value = current;
		this.toolTip.toolTip = base.transform.parent.name + ": " + current.ToString("F2");
	}

	// Token: 0x0600B308 RID: 45832 RVA: 0x0043F868 File Offset: 0x0043DA68
	public void SetMaxValue(float current, float max)
	{
		float num = current / max * 100f;
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = (num > 100f);
		}
		this.maxSlider.value = Mathf.Min(100f, num);
		if (this.toolTip != null)
		{
			this.toolTip.toolTip = string.Concat(new string[]
			{
				base.transform.parent.name,
				": ",
				current.ToString("F2"),
				"/",
				max.ToString("F2")
			});
		}
	}

	// Token: 0x0600B309 RID: 45833 RVA: 0x0043F91C File Offset: 0x0043DB1C
	private void Update()
	{
		if (!this.interactable)
		{
			return;
		}
		this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
		this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
		{
			this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
			this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
			this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
			return;
		}
		this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
		this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
	}

	// Token: 0x0600B30A RID: 45834 RVA: 0x0043FAA8 File Offset: 0x0043DCA8
	public void OnMinValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
			this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
		}
		else
		{
			this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
		}
		if (this.onMinChange != null)
		{
			this.onMinChange(this);
		}
	}

	// Token: 0x0600B30B RID: 45835 RVA: 0x0043FB6C File Offset: 0x0043DD6C
	public void OnMaxValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
		}
		else
		{
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		}
		if (this.onMaxChange != null)
		{
			this.onMaxChange(this);
		}
	}

	// Token: 0x0600B30C RID: 45836 RVA: 0x0043FC4C File Offset: 0x0043DE4C
	public void Lock(bool shouldLock)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Drag)
		{
			this.lockRange = shouldLock;
			this.range = this.maxSlider.value - this.minSlider.value;
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x0600B30D RID: 45837 RVA: 0x0043FC9C File Offset: 0x0043DE9C
	public void ToggleLock()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Toggle)
		{
			this.lockRange = !this.lockRange;
			if (this.lockRange)
			{
				this.range = this.maxSlider.value - this.minSlider.value;
			}
		}
	}

	// Token: 0x0600B30E RID: 45838 RVA: 0x0043FCF0 File Offset: 0x0043DEF0
	public void OnDrag()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange && this.lockType == MinMaxSlider.LockingType.Drag)
		{
			float num = KInputManager.GetMousePos().x - this.mousePos.x;
			if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
			{
				num = KInputManager.GetMousePos().y - this.mousePos.y;
			}
			this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x04008D7B RID: 36219
	public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

	// Token: 0x04008D7D RID: 36221
	public bool lockRange;

	// Token: 0x04008D7E RID: 36222
	public bool interactable = true;

	// Token: 0x04008D7F RID: 36223
	public float minLimit;

	// Token: 0x04008D80 RID: 36224
	public float maxLimit = 100f;

	// Token: 0x04008D81 RID: 36225
	public float range = 50f;

	// Token: 0x04008D82 RID: 36226
	public float barWidth = 10f;

	// Token: 0x04008D83 RID: 36227
	public float barHeight = 100f;

	// Token: 0x04008D84 RID: 36228
	public float currentMinValue = 10f;

	// Token: 0x04008D85 RID: 36229
	public float currentMaxValue = 90f;

	// Token: 0x04008D86 RID: 36230
	public float currentExtraValue = 50f;

	// Token: 0x04008D87 RID: 36231
	public Slider.Direction direction;

	// Token: 0x04008D88 RID: 36232
	public bool wholeNumbers = true;

	// Token: 0x04008D89 RID: 36233
	public Action<MinMaxSlider> onMinChange;

	// Token: 0x04008D8A RID: 36234
	public Action<MinMaxSlider> onMaxChange;

	// Token: 0x04008D8B RID: 36235
	public Slider minSlider;

	// Token: 0x04008D8C RID: 36236
	public Slider maxSlider;

	// Token: 0x04008D8D RID: 36237
	public Slider extraSlider;

	// Token: 0x04008D8E RID: 36238
	public RectTransform minRect;

	// Token: 0x04008D8F RID: 36239
	public RectTransform maxRect;

	// Token: 0x04008D90 RID: 36240
	public RectTransform bgFill;

	// Token: 0x04008D91 RID: 36241
	public RectTransform mgFill;

	// Token: 0x04008D92 RID: 36242
	public RectTransform fgFill;

	// Token: 0x04008D93 RID: 36243
	public Text title;

	// Token: 0x04008D94 RID: 36244
	[MyCmpGet]
	public ToolTip toolTip;

	// Token: 0x04008D95 RID: 36245
	public Image icon;

	// Token: 0x04008D96 RID: 36246
	public Image isOverPowered;

	// Token: 0x04008D97 RID: 36247
	private Vector3 mousePos;

	// Token: 0x020020CE RID: 8398
	public enum LockingType
	{
		// Token: 0x04008D99 RID: 36249
		Toggle,
		// Token: 0x04008D9A RID: 36250
		Drag
	}

	// Token: 0x020020CF RID: 8399
	public enum Mode
	{
		// Token: 0x04008D9C RID: 36252
		Single,
		// Token: 0x04008D9D RID: 36253
		Double,
		// Token: 0x04008D9E RID: 36254
		Triple
	}
}
