﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class BionicSideScreen : SideScreenContent
{
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

	private void RefreshSelectedStateInSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
		}
	}

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

	private BionicSideScreenUpgradeSlot CreateBionicSlot()
	{
		BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject, false);
		bionicSideScreenUpgradeSlot.OnClick = (Action<BionicSideScreenUpgradeSlot>)Delegate.Combine(bionicSideScreenUpgradeSlot.OnClick, new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked));
		return bionicSideScreenUpgradeSlot;
	}

	private void OnBionicBecameOnline(object o)
	{
		this.RefreshSlots();
	}

	private void OnBionicBecameOffline(object o)
	{
		this.RefreshSlots();
	}

	private void OnBionicWattageChanged(object o)
	{
		this.RefreshSlots();
	}

	private void OnBionicBedTimeChoreStateChanged(object o)
	{
		this.RefreshSlots();
	}

	private void OnBionicUpgradeComponentSlotCountChanged(object o)
	{
		this.RefreshSlots();
	}

	private void OnBionicUpgradeChanged(object o)
	{
		this.RecreateBionicSlots();
	}

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

	private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
	{
		this.lastSlotSelected = slot;
		this.RefreshSelectedStateInSlots();
	}

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

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshSlots();
		}
	}

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

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
	}

	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;

	public BionicSideScreenUpgradeSlot originalBionicSlot;

	private BionicUpgradesMonitor.Instance upgradeMonitor;

	private BionicBatteryMonitor.Instance batteryMonitor;

	private BionicBedTimeMonitor.Instance bedTimeMonitor;

	private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();

	private OwnablesSidescreen ownableSidescreen;

	private AssignableSlotInstance lastSlotSelected;
}
