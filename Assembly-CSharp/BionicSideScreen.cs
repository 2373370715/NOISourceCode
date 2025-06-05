using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F99 RID: 8089
public class BionicSideScreen : SideScreenContent
{
	// Token: 0x0600AAE5 RID: 43749 RVA: 0x004171AC File Offset: 0x004153AC
	private void OnBionicUpgradeSlotClicked(BionicSideScreenUpgradeSlot slotClicked)
	{
		bool flag = slotClicked == null || this.lastSlotSelected == slotClicked.upgradeSlot.GetAssignableSlotInstance();
		bool flag2 = !flag && slotClicked.upgradeSlot.IsLocked;
		this.lastSlotSelected = (flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance());
		this.RefreshSelectedStateInSlots();
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		AssignableSlotInstance assignableSlotInstance = (flag || flag2) ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
		if (this.ownableSidescreen != null)
		{
			this.ownableSidescreen.SetSelectedSlot(assignableSlotInstance);
			return;
		}
		if (flag || flag2)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			return;
		}
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.ownableSecondSideScreenPrefab, bionicUpgrade.Name)).SetSlot(assignableSlotInstance);
	}

	// Token: 0x0600AAE6 RID: 43750 RVA: 0x00417278 File Offset: 0x00415478
	private void RefreshSelectedStateInSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
		}
	}

	// Token: 0x0600AAE7 RID: 43751 RVA: 0x004172C0 File Offset: 0x004154C0
	public void RecreateBionicSlots()
	{
		int num = (this.upgradeMonitor != null) ? this.upgradeMonitor.upgradeComponentSlots.Length : 0;
		for (int i = 0; i < Mathf.Max(num, this.bionicSlots.Count); i++)
		{
			if (i >= this.bionicSlots.Count)
			{
				BionicSideScreenUpgradeSlot item = this.CreateBionicSlot();
				this.bionicSlots.Add(item);
			}
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (i < num)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot = this.upgradeMonitor.upgradeComponentSlots[i];
				bionicSideScreenUpgradeSlot.gameObject.SetActive(true);
				bionicSideScreenUpgradeSlot.Setup(upgradeSlot);
				bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
			}
			else
			{
				bionicSideScreenUpgradeSlot.Setup(null);
				bionicSideScreenUpgradeSlot.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600AAE8 RID: 43752 RVA: 0x0041738C File Offset: 0x0041558C
	private BionicSideScreenUpgradeSlot CreateBionicSlot()
	{
		BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject, false);
		bionicSideScreenUpgradeSlot.OnClick = (Action<BionicSideScreenUpgradeSlot>)Delegate.Combine(bionicSideScreenUpgradeSlot.OnClick, new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked));
		return bionicSideScreenUpgradeSlot;
	}

	// Token: 0x0600AAE9 RID: 43753 RVA: 0x0011381D File Offset: 0x00111A1D
	private void OnBionicBecameOnline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600AAEA RID: 43754 RVA: 0x0011381D File Offset: 0x00111A1D
	private void OnBionicBecameOffline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600AAEB RID: 43755 RVA: 0x0011381D File Offset: 0x00111A1D
	private void OnBionicWattageChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600AAEC RID: 43756 RVA: 0x0011381D File Offset: 0x00111A1D
	private void OnBionicBedTimeChoreStateChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600AAED RID: 43757 RVA: 0x0011381D File Offset: 0x00111A1D
	private void OnBionicUpgradeComponentSlotCountChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600AAEE RID: 43758 RVA: 0x00113825 File Offset: 0x00111A25
	private void OnBionicUpgradeChanged(object o)
	{
		this.RecreateBionicSlots();
	}

	// Token: 0x0600AAEF RID: 43759 RVA: 0x0011382D File Offset: 0x00111A2D
	private void OnBionicTagsChanged(object o)
	{
		if (o == null)
		{
			return;
		}
		if (((TagChangedEventData)o).tag == GameTags.BionicBedTime)
		{
			this.OnBionicBedTimeChoreStateChanged(o);
		}
	}

	// Token: 0x0600AAF0 RID: 43760 RVA: 0x004173E4 File Offset: 0x004155E4
	private void RefreshSlots()
	{
		for (int i = this.bionicSlots.Count - 1; i >= 0; i--)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (bionicSideScreenUpgradeSlot != null)
			{
				bionicSideScreenUpgradeSlot.Refresh();
				bionicSideScreenUpgradeSlot.gameObject.transform.SetAsFirstSibling();
			}
		}
	}

	// Token: 0x0600AAF1 RID: 43761 RVA: 0x00417438 File Offset: 0x00415638
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalBionicSlot.gameObject.SetActive(false);
		this.ownableSidescreen = base.transform.parent.GetComponentInChildren<OwnablesSidescreen>();
		if (this.ownableSidescreen != null)
		{
			OwnablesSidescreen ownablesSidescreen = this.ownableSidescreen;
			ownablesSidescreen.OnSlotInstanceSelected = (Action<AssignableSlotInstance>)Delegate.Combine(ownablesSidescreen.OnSlotInstanceSelected, new Action<AssignableSlotInstance>(this.OnOwnableSidescreenRowSelected));
		}
	}

	// Token: 0x0600AAF2 RID: 43762 RVA: 0x00113851 File Offset: 0x00111A51
	private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
	{
		this.lastSlotSelected = slot;
		this.RefreshSelectedStateInSlots();
	}

	// Token: 0x0600AAF3 RID: 43763 RVA: 0x004174A8 File Offset: 0x004156A8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.lastSlotSelected = null;
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
			this.upgradeMonitor.Unsubscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
			this.upgradeMonitor.Unsubscribe(1095596132, new Action<object>(this.OnBionicUpgradeComponentSlotCountChanged));
		}
		if (this.batteryMonitor != null)
		{
			this.batteryMonitor.Unsubscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
		}
		if (this.bedTimeMonitor != null)
		{
			this.bedTimeMonitor.Unsubscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
		}
		this.batteryMonitor = target.GetSMI<BionicBatteryMonitor.Instance>();
		this.upgradeMonitor = target.GetSMI<BionicUpgradesMonitor.Instance>();
		this.bedTimeMonitor = target.GetSMI<BionicBedTimeMonitor.Instance>();
		this.upgradeMonitor.Subscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
		this.upgradeMonitor.Subscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
		this.upgradeMonitor.Subscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		this.batteryMonitor.Subscribe(1095596132, new Action<object>(this.OnBionicUpgradeComponentSlotCountChanged));
		this.batteryMonitor.Subscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
		this.bedTimeMonitor.Subscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
		this.RecreateBionicSlots();
		this.RefreshSlots();
	}

	// Token: 0x0600AAF4 RID: 43764 RVA: 0x00113860 File Offset: 0x00111A60
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x0600AAF5 RID: 43765 RVA: 0x0041765C File Offset: 0x0041585C
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		}
		this.bedTimeMonitor = null;
		this.upgradeMonitor = null;
		this.lastSlotSelected = null;
	}

	// Token: 0x0600AAF6 RID: 43766 RVA: 0x00113872 File Offset: 0x00111A72
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
	}

	// Token: 0x0600AAF7 RID: 43767 RVA: 0x0011387D File Offset: 0x00111A7D
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x04008686 RID: 34438
	public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;

	// Token: 0x04008687 RID: 34439
	public BionicSideScreenUpgradeSlot originalBionicSlot;

	// Token: 0x04008688 RID: 34440
	private BionicUpgradesMonitor.Instance upgradeMonitor;

	// Token: 0x04008689 RID: 34441
	private BionicBatteryMonitor.Instance batteryMonitor;

	// Token: 0x0400868A RID: 34442
	private BionicBedTimeMonitor.Instance bedTimeMonitor;

	// Token: 0x0400868B RID: 34443
	private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();

	// Token: 0x0400868C RID: 34444
	private OwnablesSidescreen ownableSidescreen;

	// Token: 0x0400868D RID: 34445
	private AssignableSlotInstance lastSlotSelected;
}
