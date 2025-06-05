using System;
using STRINGS;

// Token: 0x020019DE RID: 6622
public class ConditionOnLaunchPad : ProcessCondition
{
	// Token: 0x06008A05 RID: 35333 RVA: 0x000FEB05 File Offset: 0x000FCD05
	public ConditionOnLaunchPad(CraftModuleInterface craftInterface)
	{
		this.craftInterface = craftInterface;
	}

	// Token: 0x06008A06 RID: 35334 RVA: 0x000FEB14 File Offset: 0x000FCD14
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!(this.craftInterface.CurrentPad != null))
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A07 RID: 35335 RVA: 0x00368CF8 File Offset: 0x00366EF8
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A08 RID: 35336 RVA: 0x00368D38 File Offset: 0x00366F38
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.ON_LAUNCHPAD.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A09 RID: 35337 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006847 RID: 26695
	private CraftModuleInterface craftInterface;
}
