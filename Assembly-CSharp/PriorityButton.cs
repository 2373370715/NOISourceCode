using System;
using UnityEngine;

// Token: 0x02001F0C RID: 7948
[AddComponentMenu("KMonoBehaviour/scripts/PriorityButton")]
public class PriorityButton : KMonoBehaviour
{
	// Token: 0x17000AB8 RID: 2744
	// (get) Token: 0x0600A711 RID: 42769 RVA: 0x00110D42 File Offset: 0x0010EF42
	// (set) Token: 0x0600A712 RID: 42770 RVA: 0x00402F9C File Offset: 0x0040119C
	public PrioritySetting priority
	{
		get
		{
			return this._priority;
		}
		set
		{
			this._priority = value;
			if (this.its != null)
			{
				if (this.priority.priority_class == PriorityScreen.PriorityClass.high)
				{
					this.its.colorStyleSetting = this.highStyle;
				}
				else
				{
					this.its.colorStyleSetting = this.normalStyle;
				}
				this.its.RefreshColorStyle();
				this.its.ResetColor();
			}
		}
	}

	// Token: 0x0600A713 RID: 42771 RVA: 0x00110D4A File Offset: 0x0010EF4A
	protected override void OnPrefabInit()
	{
		this.toggle.onClick += this.OnClick;
	}

	// Token: 0x0600A714 RID: 42772 RVA: 0x00110D63 File Offset: 0x0010EF63
	private void OnClick()
	{
		if (this.playSelectionSound)
		{
			PriorityScreen.PlayPriorityConfirmSound(this.priority);
		}
		if (this.onClick != null)
		{
			this.onClick(this.priority);
		}
	}

	// Token: 0x040082FF RID: 33535
	public KToggle toggle;

	// Token: 0x04008300 RID: 33536
	public LocText text;

	// Token: 0x04008301 RID: 33537
	public ToolTip tooltip;

	// Token: 0x04008302 RID: 33538
	[MyCmpGet]
	private ImageToggleState its;

	// Token: 0x04008303 RID: 33539
	public ColorStyleSetting normalStyle;

	// Token: 0x04008304 RID: 33540
	public ColorStyleSetting highStyle;

	// Token: 0x04008305 RID: 33541
	public bool playSelectionSound = true;

	// Token: 0x04008306 RID: 33542
	public Action<PrioritySetting> onClick;

	// Token: 0x04008307 RID: 33543
	private PrioritySetting _priority;
}
