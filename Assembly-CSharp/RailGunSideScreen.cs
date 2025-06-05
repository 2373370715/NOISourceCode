using System;
using STRINGS;
using UnityEngine;

// Token: 0x02002011 RID: 8209
public class RailGunSideScreen : SideScreenContent
{
	// Token: 0x0600ADB0 RID: 44464 RVA: 0x004238FC File Offset: 0x00421AFC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.slider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.slider.onMove += delegate()
		{
			this.ReceiveValueFromSlider(this.slider.value);
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput(this.numberInput.currentValue);
		};
		this.numberInput.decimalPlaces = 1;
	}

	// Token: 0x0600ADB1 RID: 44465 RVA: 0x0011549C File Offset: 0x0011369C
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x0600ADB2 RID: 44466 RVA: 0x001154B8 File Offset: 0x001136B8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.selectedGun)
		{
			this.selectedGun = null;
		}
	}

	// Token: 0x0600ADB3 RID: 44467 RVA: 0x001154D4 File Offset: 0x001136D4
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RailGun>() != null;
	}

	// Token: 0x0600ADB4 RID: 44468 RVA: 0x00423990 File Offset: 0x00421B90
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.selectedGun = new_target.GetComponent<RailGun>();
		if (this.selectedGun == null)
		{
			global::Debug.LogError("The gameObject received does not contain a RailGun component");
			return;
		}
		this.targetRailgunHEPStorageSubHandle = this.selectedGun.Subscribe(-1837862626, new Action<object>(this.UpdateHEPLabels));
		this.slider.minValue = this.selectedGun.MinLaunchMass;
		this.slider.maxValue = this.selectedGun.MaxLaunchMass;
		this.slider.value = this.selectedGun.launchMass;
		this.unitsLabel.text = GameUtil.GetCurrentMassUnit(false);
		this.numberInput.minValue = this.selectedGun.MinLaunchMass;
		this.numberInput.maxValue = this.selectedGun.MaxLaunchMass;
		this.numberInput.currentValue = Mathf.Max(this.selectedGun.MinLaunchMass, Mathf.Min(this.selectedGun.MaxLaunchMass, this.selectedGun.launchMass));
		this.UpdateMaxCapacityLabel();
		this.numberInput.Activate();
		this.UpdateHEPLabels(null);
	}

	// Token: 0x0600ADB5 RID: 44469 RVA: 0x001154E2 File Offset: 0x001136E2
	public override void ClearTarget()
	{
		if (this.targetRailgunHEPStorageSubHandle != -1 && this.selectedGun != null)
		{
			this.selectedGun.Unsubscribe(this.targetRailgunHEPStorageSubHandle);
			this.targetRailgunHEPStorageSubHandle = -1;
		}
		this.selectedGun = null;
	}

	// Token: 0x0600ADB6 RID: 44470 RVA: 0x00423ACC File Offset: 0x00421CCC
	public void UpdateHEPLabels(object data = null)
	{
		if (this.selectedGun == null)
		{
			return;
		}
		string text = BUILDINGS.PREFABS.RAILGUN.SIDESCREEN_HEP_REQUIRED;
		text = text.Replace("{current}", this.selectedGun.CurrentEnergy.ToString());
		text = text.Replace("{required}", this.selectedGun.EnergyCost.ToString());
		this.hepStorageInfo.text = text;
	}

	// Token: 0x0600ADB7 RID: 44471 RVA: 0x0011551A File Offset: 0x0011371A
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600ADB8 RID: 44472 RVA: 0x0011551A File Offset: 0x0011371A
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600ADB9 RID: 44473 RVA: 0x00115523 File Offset: 0x00113723
	private void UpdateMaxCapacity(float newValue)
	{
		this.selectedGun.launchMass = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
		this.selectedGun.Trigger(161772031, null);
	}

	// Token: 0x0600ADBA RID: 44474 RVA: 0x00115554 File Offset: 0x00113754
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.selectedGun.launchMass.ToString());
	}

	// Token: 0x040088B1 RID: 34993
	public GameObject content;

	// Token: 0x040088B2 RID: 34994
	private RailGun selectedGun;

	// Token: 0x040088B3 RID: 34995
	public LocText DescriptionText;

	// Token: 0x040088B4 RID: 34996
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x040088B5 RID: 34997
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040088B6 RID: 34998
	[SerializeField]
	private LocText unitsLabel;

	// Token: 0x040088B7 RID: 34999
	[SerializeField]
	private LocText hepStorageInfo;

	// Token: 0x040088B8 RID: 35000
	private int targetRailgunHEPStorageSubHandle = -1;
}
