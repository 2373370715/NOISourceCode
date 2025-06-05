using System;
using STRINGS;

// Token: 0x020019E7 RID: 6631
public class LoadingCompleteCondition : ProcessCondition
{
	// Token: 0x06008A34 RID: 35380 RVA: 0x000FED2C File Offset: 0x000FCF2C
	public LoadingCompleteCondition(Storage target)
	{
		this.target = target;
		this.userControlledTarget = target.GetComponent<IUserControlledCapacity>();
	}

	// Token: 0x06008A35 RID: 35381 RVA: 0x000FED47 File Offset: 0x000FCF47
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.userControlledTarget != null)
		{
			if (this.userControlledTarget.AmountStored < this.userControlledTarget.UserMaxCapacity)
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
		else
		{
			if (!this.target.IsFull())
			{
				return ProcessCondition.Status.Warning;
			}
			return ProcessCondition.Status.Ready;
		}
	}

	// Token: 0x06008A36 RID: 35382 RVA: 0x000FED7D File Offset: 0x000FCF7D
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06008A37 RID: 35383 RVA: 0x000FED94 File Offset: 0x000FCF94
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.LOADING_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06008A38 RID: 35384 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006853 RID: 26707
	private Storage target;

	// Token: 0x04006854 RID: 26708
	private IUserControlledCapacity userControlledTarget;
}
