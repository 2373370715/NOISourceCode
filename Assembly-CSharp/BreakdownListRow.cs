using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020BD RID: 8381
[AddComponentMenu("KMonoBehaviour/scripts/BreakdownListRow")]
public class BreakdownListRow : KMonoBehaviour
{
	// Token: 0x0600B2C0 RID: 45760 RVA: 0x0043E65C File Offset: 0x0043C85C
	public void ShowData(string name, string value)
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.nameLabel.text = name;
		this.valueLabel.text = value;
		this.dotOutlineImage.gameObject.SetActive(true);
		Vector2 vector = Vector2.one * 0.6f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.dotInsideImage.gameObject.SetActive(true);
		this.dotInsideImage.color = BreakdownListRow.statusColour[0];
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetHighlighted(false);
		this.SetImportant(false);
	}

	// Token: 0x0600B2C1 RID: 45761 RVA: 0x0043E738 File Offset: 0x0043C938
	public void ShowStatusData(string name, string value, BreakdownListRow.Status dotColor)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
		this.SetStatusColor(dotColor);
	}

	// Token: 0x0600B2C2 RID: 45762 RVA: 0x0043E798 File Offset: 0x0043C998
	public void SetStatusColor(BreakdownListRow.Status dotColor)
	{
		this.checkmarkImage.gameObject.SetActive(dotColor > BreakdownListRow.Status.Default);
		this.checkmarkImage.color = BreakdownListRow.statusColour[(int)dotColor];
		switch (dotColor)
		{
		case BreakdownListRow.Status.Red:
			this.checkmarkImage.sprite = this.statusFailureIcon;
			return;
		case BreakdownListRow.Status.Green:
			this.checkmarkImage.sprite = this.statusSuccessIcon;
			return;
		case BreakdownListRow.Status.Yellow:
			this.checkmarkImage.sprite = this.statusWarningIcon;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600B2C3 RID: 45763 RVA: 0x0043E81C File Offset: 0x0043CA1C
	public void ShowCheckmarkData(string name, string value, BreakdownListRow.Status status)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(true);
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.dotInsideImage.gameObject.SetActive(true);
		this.iconImage.gameObject.SetActive(false);
		this.SetStatusColor(status);
	}

	// Token: 0x0600B2C4 RID: 45764 RVA: 0x0043E880 File Offset: 0x0043CA80
	public void ShowIconData(string name, string value, Sprite sprite)
	{
		this.ShowData(name, value);
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(true);
		this.checkmarkImage.gameObject.SetActive(false);
		this.iconImage.sprite = sprite;
		this.iconImage.color = Color.white;
	}

	// Token: 0x0600B2C5 RID: 45765 RVA: 0x00118BCE File Offset: 0x00116DCE
	public void ShowIconData(string name, string value, Sprite sprite, Color spriteColor)
	{
		this.ShowIconData(name, value, sprite);
		this.iconImage.color = spriteColor;
	}

	// Token: 0x0600B2C6 RID: 45766 RVA: 0x0043E8F8 File Offset: 0x0043CAF8
	public void SetHighlighted(bool highlighted)
	{
		this.isHighlighted = highlighted;
		Vector2 vector = Vector2.one * 0.8f;
		this.dotOutlineImage.rectTransform.localScale.Set(vector.x, vector.y, 1f);
		this.nameLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
		this.valueLabel.alpha = (this.isHighlighted ? 0.9f : 0.5f);
	}

	// Token: 0x0600B2C7 RID: 45767 RVA: 0x0043E984 File Offset: 0x0043CB84
	public void SetDisabled(bool disabled)
	{
		this.isDisabled = disabled;
		this.nameLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
		this.valueLabel.alpha = (this.isDisabled ? 0.4f : 0.5f);
	}

	// Token: 0x0600B2C8 RID: 45768 RVA: 0x0043E9D8 File Offset: 0x0043CBD8
	public void SetImportant(bool important)
	{
		this.isImportant = important;
		this.dotOutlineImage.rectTransform.localScale = Vector3.one;
		this.nameLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.valueLabel.alpha = (this.isImportant ? 1f : 0.5f);
		this.nameLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
		this.valueLabel.fontStyle = (this.isImportant ? FontStyles.Bold : FontStyles.Normal);
	}

	// Token: 0x0600B2C9 RID: 45769 RVA: 0x0043EA70 File Offset: 0x0043CC70
	public void HideIcon()
	{
		this.dotOutlineImage.gameObject.SetActive(false);
		this.dotInsideImage.gameObject.SetActive(false);
		this.iconImage.gameObject.SetActive(false);
		this.checkmarkImage.gameObject.SetActive(false);
	}

	// Token: 0x0600B2CA RID: 45770 RVA: 0x00118BE6 File Offset: 0x00116DE6
	public void AddTooltip(string tooltipText)
	{
		if (this.tooltip == null)
		{
			this.tooltip = base.gameObject.AddComponent<ToolTip>();
		}
		this.tooltip.SetSimpleTooltip(tooltipText);
	}

	// Token: 0x0600B2CB RID: 45771 RVA: 0x00118C13 File Offset: 0x00116E13
	public void ClearTooltip()
	{
		if (this.tooltip != null)
		{
			this.tooltip.ClearMultiStringTooltip();
		}
	}

	// Token: 0x0600B2CC RID: 45772 RVA: 0x00118C2E File Offset: 0x00116E2E
	public void SetValue(string value)
	{
		this.valueLabel.text = value;
	}

	// Token: 0x04008D26 RID: 36134
	private static Color[] statusColour = new Color[]
	{
		new Color(0.34117648f, 0.36862746f, 0.45882353f, 1f),
		new Color(0.72156864f, 0.38431373f, 0f, 1f),
		new Color(0.38431373f, 0.72156864f, 0f, 1f),
		new Color(0.72156864f, 0.72156864f, 0f, 1f)
	};

	// Token: 0x04008D27 RID: 36135
	public Image dotOutlineImage;

	// Token: 0x04008D28 RID: 36136
	public Image dotInsideImage;

	// Token: 0x04008D29 RID: 36137
	public Image iconImage;

	// Token: 0x04008D2A RID: 36138
	public Image checkmarkImage;

	// Token: 0x04008D2B RID: 36139
	public LocText nameLabel;

	// Token: 0x04008D2C RID: 36140
	public LocText valueLabel;

	// Token: 0x04008D2D RID: 36141
	private bool isHighlighted;

	// Token: 0x04008D2E RID: 36142
	private bool isDisabled;

	// Token: 0x04008D2F RID: 36143
	private bool isImportant;

	// Token: 0x04008D30 RID: 36144
	private ToolTip tooltip;

	// Token: 0x04008D31 RID: 36145
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x04008D32 RID: 36146
	[SerializeField]
	private Sprite statusWarningIcon;

	// Token: 0x04008D33 RID: 36147
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x020020BE RID: 8382
	public enum Status
	{
		// Token: 0x04008D35 RID: 36149
		Default,
		// Token: 0x04008D36 RID: 36150
		Red,
		// Token: 0x04008D37 RID: 36151
		Green,
		// Token: 0x04008D38 RID: 36152
		Yellow
	}
}
