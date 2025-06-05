using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F9A RID: 8090
public class BionicSideScreenUpgradeSlot : KMonoBehaviour
{
	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x0600AAFA RID: 43770 RVA: 0x001138A0 File Offset: 0x00111AA0
	// (set) Token: 0x0600AAF9 RID: 43769 RVA: 0x00113897 File Offset: 0x00111A97
	public BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot { get; private set; }

	// Token: 0x0600AAFB RID: 43771 RVA: 0x001138A8 File Offset: 0x00111AA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnSlotClicked));
	}

	// Token: 0x0600AAFC RID: 43772 RVA: 0x004176A8 File Offset: 0x004158A8
	public void Setup(BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot)
	{
		if (this.upgradeSlot != null)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot2 = this.upgradeSlot;
			upgradeSlot2.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Remove(upgradeSlot2.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.upgradeSlot = upgradeSlot;
		if (upgradeSlot != null)
		{
			upgradeSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.Refresh();
	}

	// Token: 0x0600AAFD RID: 43773 RVA: 0x001138D7 File Offset: 0x00111AD7
	private void OnAssignedUpgradeChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
	{
		this.Refresh();
	}

	// Token: 0x0600AAFE RID: 43774 RVA: 0x00417718 File Offset: 0x00415918
	public void Refresh()
	{
		this.label.color = this.standardColor;
		BionicSideScreenUpgradeSlot.State state = this.upgradeSlot.IsLocked ? BionicSideScreenUpgradeSlot.State.Locked : BionicSideScreenUpgradeSlot.State.Empty;
		if (state == BionicSideScreenUpgradeSlot.State.Empty && this.upgradeSlot.HasUpgradeInstalled)
		{
			state = BionicSideScreenUpgradeSlot.State.Installed;
		}
		else if (state == BionicSideScreenUpgradeSlot.State.Empty && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning())
		{
			state = BionicSideScreenUpgradeSlot.State.Assigned;
		}
		switch (state)
		{
		case BionicSideScreenUpgradeSlot.State.Locked:
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_BLOCKED);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_BLOCKED_SLOT);
			this.label.Opacity(0.5f);
			this.icon.gameObject.SetActive(false);
			break;
		case BionicSideScreenUpgradeSlot.State.Empty:
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_EMPTY);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_NO_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(false);
			break;
		case BionicSideScreenUpgradeSlot.State.Assigned:
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.assignedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(0.5f);
			this.icon.gameObject.SetActive(true);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED);
			this.label.Opacity(1f);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_ASSIGNED, this.upgradeSlot.assignedUpgradeComponent.GetProperName()));
			break;
		case BionicSideScreenUpgradeSlot.State.Installed:
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.installedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(1f);
			this.icon.gameObject.SetActive(true);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED, BionicUpgradeComponentConfig.GenerateTooltipForBooster(this.upgradeSlot.installedUpgradeComponent)));
			break;
		}
		this.SetSelected(this._isSelected);
	}

	// Token: 0x0600AAFF RID: 43775 RVA: 0x001138DF File Offset: 0x00111ADF
	private void OnSlotClicked()
	{
		Action<BionicSideScreenUpgradeSlot> onClick = this.OnClick;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x0600AB00 RID: 43776 RVA: 0x0041798C File Offset: 0x00415B8C
	public void SetSelected(bool isSelected)
	{
		this._isSelected = isSelected;
		bool flag = this.upgradeSlot == null || this.upgradeSlot.IsLocked;
		bool flag2 = this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning();
		bool flag3 = flag2 && this.upgradeSlot.assignedUpgradeComponent.Booster == BionicUpgradeComponentConfig.BoosterType.Basic;
		this.toggle.ChangeState((flag ? 0 : 2) + (flag2 ? 2 : 0) + ((flag2 && flag3) ? 2 : 0) + (isSelected ? 1 : 0));
	}

	// Token: 0x0400868E RID: 34446
	public static string TEXT_BLOCKED_SLOT = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_LOCKED;

	// Token: 0x0400868F RID: 34447
	public static string TEXT_NO_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_EMPTY;

	// Token: 0x04008690 RID: 34448
	public static string TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_ASSIGNED;

	// Token: 0x04008691 RID: 34449
	public static string TEXT_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_INSTALLED;

	// Token: 0x04008692 RID: 34450
	public static string TEXT_TOOLTIP_BLOCKED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_LOCKED;

	// Token: 0x04008693 RID: 34451
	public static string TEXT_TOOLTIP_EMPTY = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_EMPTY;

	// Token: 0x04008694 RID: 34452
	public static string TEXT_TOOLTIP_ASSIGNED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_ASSIGNED;

	// Token: 0x04008695 RID: 34453
	public static string TEXT_TOOLTIP_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED;

	// Token: 0x04008696 RID: 34454
	public MultiToggle toggle;

	// Token: 0x04008697 RID: 34455
	public KImage icon;

	// Token: 0x04008698 RID: 34456
	public LocText label;

	// Token: 0x04008699 RID: 34457
	public ToolTip tooltip;

	// Token: 0x0400869A RID: 34458
	[Header("Effects settings")]
	public float inUseAnimationDuration = 0.5f;

	// Token: 0x0400869B RID: 34459
	public Color standardColor = Color.black;

	// Token: 0x0400869C RID: 34460
	public Color activeColor = Color.blue;

	// Token: 0x0400869D RID: 34461
	public Color activeColorTooltip = Color.blue;

	// Token: 0x0400869E RID: 34462
	public Action<BionicSideScreenUpgradeSlot> OnClick;

	// Token: 0x040086A0 RID: 34464
	private bool _isSelected;

	// Token: 0x02001F9B RID: 8091
	public enum State
	{
		// Token: 0x040086A2 RID: 34466
		Locked,
		// Token: 0x040086A3 RID: 34467
		Empty,
		// Token: 0x040086A4 RID: 34468
		Assigned,
		// Token: 0x040086A5 RID: 34469
		Installed
	}
}
