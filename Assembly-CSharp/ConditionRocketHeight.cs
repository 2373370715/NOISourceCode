using System;
using STRINGS;

// Token: 0x020019E3 RID: 6627
public class ConditionRocketHeight : ProcessCondition
{
	// Token: 0x06008A20 RID: 35360 RVA: 0x000FEC23 File Offset: 0x000FCE23
	public ConditionRocketHeight(RocketEngineCluster engine)
	{
		this.engine = engine;
	}

	// Token: 0x06008A21 RID: 35361 RVA: 0x000FEC32 File Offset: 0x000FCE32
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.engine.maxHeight < this.engine.GetComponent<RocketModuleCluster>().CraftInterface.RocketHeight)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A22 RID: 35362 RVA: 0x003694D4 File Offset: 0x003676D4
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A23 RID: 35363 RVA: 0x00369514 File Offset: 0x00367714
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.MAX_HEIGHT.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A24 RID: 35364 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400684F RID: 26703
	private RocketEngineCluster engine;
}
