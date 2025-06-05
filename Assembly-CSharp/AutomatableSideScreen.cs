using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F98 RID: 8088
public class AutomatableSideScreen : SideScreenContent
{
	// Token: 0x0600AADF RID: 43743 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AAE0 RID: 43744 RVA: 0x004170E0 File Offset: 0x004152E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.allowManualToggle.transform.parent.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.AUTOMATABLE_SIDE_SCREEN.ALLOWMANUALBUTTONTOOLTIP);
		this.allowManualToggle.onValueChanged += this.OnAllowManualChanged;
	}

	// Token: 0x0600AAE1 RID: 43745 RVA: 0x001137F2 File Offset: 0x001119F2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Automatable>() != null;
	}

	// Token: 0x0600AAE2 RID: 43746 RVA: 0x00417130 File Offset: 0x00415330
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetAutomatable = target.GetComponent<Automatable>();
		if (this.targetAutomatable == null)
		{
			global::Debug.LogError("The target provided does not have an Automatable component");
			return;
		}
		this.allowManualToggle.isOn = !this.targetAutomatable.GetAutomationOnly();
		this.allowManualToggleCheckMark.enabled = this.allowManualToggle.isOn;
	}

	// Token: 0x0600AAE3 RID: 43747 RVA: 0x00113800 File Offset: 0x00111A00
	private void OnAllowManualChanged(bool value)
	{
		this.targetAutomatable.SetAutomationOnly(!value);
		this.allowManualToggleCheckMark.enabled = value;
	}

	// Token: 0x04008680 RID: 34432
	public KToggle allowManualToggle;

	// Token: 0x04008681 RID: 34433
	public KImage allowManualToggleCheckMark;

	// Token: 0x04008682 RID: 34434
	public GameObject content;

	// Token: 0x04008683 RID: 34435
	private GameObject target;

	// Token: 0x04008684 RID: 34436
	public LocText DescriptionText;

	// Token: 0x04008685 RID: 34437
	private Automatable targetAutomatable;
}
