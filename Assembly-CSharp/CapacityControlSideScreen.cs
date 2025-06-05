using System;
using UnityEngine;

// Token: 0x02001FA0 RID: 8096
public class CapacityControlSideScreen : SideScreenContent
{
	// Token: 0x0600AB1B RID: 43803 RVA: 0x00417DA0 File Offset: 0x00415FA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitsLabel.text = this.target.CapacityUnits;
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

	// Token: 0x0600AB1C RID: 43804 RVA: 0x00113A2C File Offset: 0x00111C2C
	public override bool IsValidForTarget(GameObject target)
	{
		return !target.GetComponent<IUserControlledCapacity>().IsNullOrDestroyed() || target.GetSMI<IUserControlledCapacity>() != null;
	}

	// Token: 0x0600AB1D RID: 43805 RVA: 0x00417E38 File Offset: 0x00416038
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IUserControlledCapacity>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IUserControlledCapacity>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IThresholdSwitch component");
			return;
		}
		this.slider.minValue = this.target.MinCapacity;
		this.slider.maxValue = this.target.MaxCapacity;
		this.slider.value = this.target.UserMaxCapacity;
		this.slider.GetComponentInChildren<ToolTip>();
		this.unitsLabel.text = this.target.CapacityUnits;
		this.numberInput.minValue = this.target.MinCapacity;
		this.numberInput.maxValue = this.target.MaxCapacity;
		this.numberInput.currentValue = Mathf.Max(this.target.MinCapacity, Mathf.Min(this.target.MaxCapacity, this.target.UserMaxCapacity));
		this.numberInput.Activate();
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x0600AB1E RID: 43806 RVA: 0x00113A46 File Offset: 0x00111C46
	private void ReceiveValueFromSlider(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600AB1F RID: 43807 RVA: 0x00113A46 File Offset: 0x00111C46
	private void ReceiveValueFromInput(float newValue)
	{
		this.UpdateMaxCapacity(newValue);
	}

	// Token: 0x0600AB20 RID: 43808 RVA: 0x00113A4F File Offset: 0x00111C4F
	private void UpdateMaxCapacity(float newValue)
	{
		this.target.UserMaxCapacity = newValue;
		this.slider.value = newValue;
		this.UpdateMaxCapacityLabel();
	}

	// Token: 0x0600AB21 RID: 43809 RVA: 0x00417F68 File Offset: 0x00416168
	private void UpdateMaxCapacityLabel()
	{
		this.numberInput.SetDisplayValue(this.target.UserMaxCapacity.ToString());
	}

	// Token: 0x040086B2 RID: 34482
	private IUserControlledCapacity target;

	// Token: 0x040086B3 RID: 34483
	[Header("Slider")]
	[SerializeField]
	private KSlider slider;

	// Token: 0x040086B4 RID: 34484
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField numberInput;

	// Token: 0x040086B5 RID: 34485
	[SerializeField]
	private LocText unitsLabel;
}
