using System;
using UnityEngine;

// Token: 0x02002033 RID: 8243
public class SingleCheckboxSideScreen : SideScreenContent
{
	// Token: 0x0600AEAE RID: 44718 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AEAF RID: 44719 RVA: 0x00116082 File Offset: 0x00114282
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.toggle.onValueChanged += this.OnValueChanged;
	}

	// Token: 0x0600AEB0 RID: 44720 RVA: 0x001160A1 File Offset: 0x001142A1
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ICheckboxControl>() != null || target.GetSMI<ICheckboxControl>() != null;
	}

	// Token: 0x0600AEB1 RID: 44721 RVA: 0x00428184 File Offset: 0x00426384
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.target = target.GetComponent<ICheckboxControl>();
		if (this.target == null)
		{
			this.target = target.GetSMI<ICheckboxControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The target provided does not have an ICheckboxControl component");
			return;
		}
		this.label.text = this.target.CheckboxLabel;
		this.toggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(this.target.CheckboxTooltip);
		this.titleKey = this.target.CheckboxTitleKey;
		this.toggle.isOn = this.target.GetCheckboxValue();
		this.toggleCheckMark.enabled = this.toggle.isOn;
	}

	// Token: 0x0600AEB2 RID: 44722 RVA: 0x001160B6 File Offset: 0x001142B6
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.target = null;
	}

	// Token: 0x0600AEB3 RID: 44723 RVA: 0x001160C5 File Offset: 0x001142C5
	private void OnValueChanged(bool value)
	{
		this.target.SetCheckboxValue(value);
		this.toggleCheckMark.enabled = value;
	}

	// Token: 0x04008971 RID: 35185
	public KToggle toggle;

	// Token: 0x04008972 RID: 35186
	public KImage toggleCheckMark;

	// Token: 0x04008973 RID: 35187
	public LocText label;

	// Token: 0x04008974 RID: 35188
	private ICheckboxControl target;
}
