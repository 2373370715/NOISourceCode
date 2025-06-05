using System;
using UnityEngine;

// Token: 0x0200200F RID: 8207
public class ProgressBarSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x0600ADA3 RID: 44451 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600ADA4 RID: 44452 RVA: 0x0011545B File Offset: 0x0011365B
	public override int GetSideScreenSortOrder()
	{
		return -10;
	}

	// Token: 0x0600ADA5 RID: 44453 RVA: 0x0011545F File Offset: 0x0011365F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IProgressBarSideScreen>() != null;
	}

	// Token: 0x0600ADA6 RID: 44454 RVA: 0x0011546A File Offset: 0x0011366A
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetObject = target.GetComponent<IProgressBarSideScreen>();
		this.RefreshBar();
	}

	// Token: 0x0600ADA7 RID: 44455 RVA: 0x00423874 File Offset: 0x00421A74
	private void RefreshBar()
	{
		this.progressBar.SetMaxValue(this.targetObject.GetProgressBarMaxValue());
		this.progressBar.SetFillPercentage(this.targetObject.GetProgressBarFillPercentage());
		this.progressBar.label.SetText(this.targetObject.GetProgressBarLabel());
		this.label.SetText(this.targetObject.GetProgressBarTitleLabel());
		this.progressBar.GetComponentInChildren<ToolTip>().SetSimpleTooltip(this.targetObject.GetProgressBarTooltip());
	}

	// Token: 0x0600ADA8 RID: 44456 RVA: 0x00115485 File Offset: 0x00113685
	public void Render1000ms(float dt)
	{
		this.RefreshBar();
	}

	// Token: 0x040088AE RID: 34990
	public LocText label;

	// Token: 0x040088AF RID: 34991
	public GenericUIProgressBar progressBar;

	// Token: 0x040088B0 RID: 34992
	public IProgressBarSideScreen targetObject;
}
