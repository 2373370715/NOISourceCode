using System;

// Token: 0x02001762 RID: 5986
public abstract class ProcessCondition
{
	// Token: 0x06007B36 RID: 31542
	public abstract ProcessCondition.Status EvaluateCondition();

	// Token: 0x06007B37 RID: 31543
	public abstract bool ShowInUI();

	// Token: 0x06007B38 RID: 31544
	public abstract string GetStatusMessage(ProcessCondition.Status status);

	// Token: 0x06007B39 RID: 31545 RVA: 0x000F59A6 File Offset: 0x000F3BA6
	public string GetStatusMessage()
	{
		return this.GetStatusMessage(this.EvaluateCondition());
	}

	// Token: 0x06007B3A RID: 31546
	public abstract string GetStatusTooltip(ProcessCondition.Status status);

	// Token: 0x06007B3B RID: 31547 RVA: 0x000F59B4 File Offset: 0x000F3BB4
	public string GetStatusTooltip()
	{
		return this.GetStatusTooltip(this.EvaluateCondition());
	}

	// Token: 0x06007B3C RID: 31548 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual StatusItem GetStatusItem(ProcessCondition.Status status)
	{
		return null;
	}

	// Token: 0x06007B3D RID: 31549 RVA: 0x000F59C2 File Offset: 0x000F3BC2
	public virtual ProcessCondition GetParentCondition()
	{
		return this.parentCondition;
	}

	// Token: 0x04005CB8 RID: 23736
	protected ProcessCondition parentCondition;

	// Token: 0x02001763 RID: 5987
	public enum ProcessConditionType
	{
		// Token: 0x04005CBA RID: 23738
		RocketFlight,
		// Token: 0x04005CBB RID: 23739
		RocketPrep,
		// Token: 0x04005CBC RID: 23740
		RocketStorage,
		// Token: 0x04005CBD RID: 23741
		RocketBoard,
		// Token: 0x04005CBE RID: 23742
		All
	}

	// Token: 0x02001764 RID: 5988
	public enum Status
	{
		// Token: 0x04005CC0 RID: 23744
		Failure,
		// Token: 0x04005CC1 RID: 23745
		Warning,
		// Token: 0x04005CC2 RID: 23746
		Ready
	}
}
