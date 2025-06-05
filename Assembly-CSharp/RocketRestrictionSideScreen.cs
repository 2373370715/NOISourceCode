using System;
using UnityEngine;

// Token: 0x02002023 RID: 8227
public class RocketRestrictionSideScreen : SideScreenContent
{
	// Token: 0x0600AE36 RID: 44598 RVA: 0x00115AE6 File Offset: 0x00113CE6
	protected override void OnSpawn()
	{
		this.unrestrictedButton.onClick += this.ClickNone;
		this.spaceRestrictedButton.onClick += this.ClickSpace;
	}

	// Token: 0x0600AE37 RID: 44599 RVA: 0x000B1628 File Offset: 0x000AF828
	public override int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x0600AE38 RID: 44600 RVA: 0x00115B16 File Offset: 0x00113D16
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<RocketControlStation.StatesInstance>() != null;
	}

	// Token: 0x0600AE39 RID: 44601 RVA: 0x00425DE4 File Offset: 0x00423FE4
	public override void SetTarget(GameObject new_target)
	{
		if (this.controlStation != null || this.controlStationLogicSubHandle != -1)
		{
			this.ClearTarget();
		}
		this.controlStation = new_target.GetComponent<RocketControlStation>();
		this.controlStationLogicSubHandle = this.controlStation.Subscribe(1861523068, new Action<object>(this.UpdateButtonStates));
		this.UpdateButtonStates(null);
	}

	// Token: 0x0600AE3A RID: 44602 RVA: 0x00115B21 File Offset: 0x00113D21
	public override void ClearTarget()
	{
		if (this.controlStationLogicSubHandle != -1 && this.controlStation != null)
		{
			this.controlStation.Unsubscribe(this.controlStationLogicSubHandle);
			this.controlStationLogicSubHandle = -1;
		}
		this.controlStation = null;
	}

	// Token: 0x0600AE3B RID: 44603 RVA: 0x00425E44 File Offset: 0x00424044
	private void UpdateButtonStates(object data = null)
	{
		bool flag = this.controlStation.IsLogicInputConnected();
		if (!flag)
		{
			this.unrestrictedButton.isOn = !this.controlStation.RestrictWhenGrounded;
			this.spaceRestrictedButton.isOn = this.controlStation.RestrictWhenGrounded;
		}
		this.unrestrictedButton.gameObject.SetActive(!flag);
		this.spaceRestrictedButton.gameObject.SetActive(!flag);
		this.automationControlled.gameObject.SetActive(flag);
	}

	// Token: 0x0600AE3C RID: 44604 RVA: 0x00115B59 File Offset: 0x00113D59
	private void ClickNone()
	{
		this.controlStation.RestrictWhenGrounded = false;
		this.UpdateButtonStates(null);
	}

	// Token: 0x0600AE3D RID: 44605 RVA: 0x00115B6E File Offset: 0x00113D6E
	private void ClickSpace()
	{
		this.controlStation.RestrictWhenGrounded = true;
		this.UpdateButtonStates(null);
	}

	// Token: 0x0400891B RID: 35099
	private RocketControlStation controlStation;

	// Token: 0x0400891C RID: 35100
	[Header("Buttons")]
	public KToggle unrestrictedButton;

	// Token: 0x0400891D RID: 35101
	public KToggle spaceRestrictedButton;

	// Token: 0x0400891E RID: 35102
	public GameObject automationControlled;

	// Token: 0x0400891F RID: 35103
	private int controlStationLogicSubHandle = -1;
}
