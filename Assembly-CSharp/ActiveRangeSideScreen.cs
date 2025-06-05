using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001F86 RID: 8070
public class ActiveRangeSideScreen : SideScreenContent
{
	// Token: 0x0600AA70 RID: 43632 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AA71 RID: 43633 RVA: 0x00415174 File Offset: 0x00413374
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateValueLabel.maxValue = this.target.MaxValue;
		this.activateValueLabel.minValue = this.target.MinValue;
		this.deactivateValueLabel.maxValue = this.target.MaxValue;
		this.deactivateValueLabel.minValue = this.target.MinValue;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
	}

	// Token: 0x0600AA72 RID: 43634 RVA: 0x00415218 File Offset: 0x00413418
	private void OnActivateValueChanged(float new_value)
	{
		this.target.ActivateValue = new_value;
		if (this.target.ActivateValue < this.target.DeactivateValue)
		{
			this.target.ActivateValue = this.target.DeactivateValue;
			this.activateValueSlider.value = this.target.ActivateValue;
		}
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x0600AA73 RID: 43635 RVA: 0x0041529C File Offset: 0x0041349C
	private void OnDeactivateValueChanged(float new_value)
	{
		this.target.DeactivateValue = new_value;
		if (this.target.DeactivateValue > this.target.ActivateValue)
		{
			this.target.DeactivateValue = this.activateValueSlider.value;
			this.deactivateValueSlider.value = this.target.DeactivateValue;
		}
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.RefreshTooltips();
	}

	// Token: 0x0600AA74 RID: 43636 RVA: 0x00415320 File Offset: 0x00413520
	private void RefreshTooltips()
	{
		this.activateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.ActivateTooltip, this.activateValueSlider.value, this.deactivateValueSlider.value));
		this.deactivateValueSlider.GetComponentInChildren<ToolTip>().SetSimpleTooltip(string.Format(this.target.DeactivateTooltip, this.deactivateValueSlider.value, this.activateValueSlider.value));
	}

	// Token: 0x0600AA75 RID: 43637 RVA: 0x001131CF File Offset: 0x001113CF
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IActivationRangeTarget>() != null;
	}

	// Token: 0x0600AA76 RID: 43638 RVA: 0x004153B0 File Offset: 0x004135B0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IActivationRangeTarget>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a IActivationRangeTarget component");
			return;
		}
		this.activateLabel.text = this.target.ActivateSliderLabelText;
		this.deactivateLabel.text = this.target.DeactivateSliderLabelText;
		this.activateValueLabel.Activate();
		this.deactivateValueLabel.Activate();
		this.activateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueSlider.minValue = this.target.MinValue;
		this.activateValueSlider.maxValue = this.target.MaxValue;
		this.activateValueSlider.value = this.target.ActivateValue;
		this.activateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.activateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnActivateValueChanged));
		this.activateValueLabel.SetDisplayValue(this.target.ActivateValue.ToString());
		this.activateValueLabel.onEndEdit += delegate()
		{
			float activateValue = this.target.ActivateValue;
			float.TryParse(this.activateValueLabel.field.text, out activateValue);
			this.OnActivateValueChanged(activateValue);
			this.activateValueSlider.value = activateValue;
		};
		this.deactivateValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueSlider.minValue = this.target.MinValue;
		this.deactivateValueSlider.maxValue = this.target.MaxValue;
		this.deactivateValueSlider.value = this.target.DeactivateValue;
		this.deactivateValueSlider.wholeNumbers = this.target.UseWholeNumbers;
		this.deactivateValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnDeactivateValueChanged));
		this.deactivateValueLabel.SetDisplayValue(this.target.DeactivateValue.ToString());
		this.deactivateValueLabel.onEndEdit += delegate()
		{
			float deactivateValue = this.target.DeactivateValue;
			float.TryParse(this.deactivateValueLabel.field.text, out deactivateValue);
			this.OnDeactivateValueChanged(deactivateValue);
			this.deactivateValueSlider.value = deactivateValue;
		};
		this.RefreshTooltips();
	}

	// Token: 0x0600AA77 RID: 43639 RVA: 0x001131DA File Offset: 0x001113DA
	public override string GetTitle()
	{
		if (this.target != null)
		{
			return this.target.ActivationRangeTitleText;
		}
		return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
	}

	// Token: 0x04008625 RID: 34341
	private IActivationRangeTarget target;

	// Token: 0x04008626 RID: 34342
	[SerializeField]
	private KSlider activateValueSlider;

	// Token: 0x04008627 RID: 34343
	[SerializeField]
	private KSlider deactivateValueSlider;

	// Token: 0x04008628 RID: 34344
	[SerializeField]
	private LocText activateLabel;

	// Token: 0x04008629 RID: 34345
	[SerializeField]
	private LocText deactivateLabel;

	// Token: 0x0400862A RID: 34346
	[Header("Number Input")]
	[SerializeField]
	private KNumberInputField activateValueLabel;

	// Token: 0x0400862B RID: 34347
	[SerializeField]
	private KNumberInputField deactivateValueLabel;
}
