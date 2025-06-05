using System;
using STRINGS;

// Token: 0x020019E6 RID: 6630
public class InternalConstructionCompleteCondition : ProcessCondition
{
	// Token: 0x06008A2F RID: 35375 RVA: 0x000FECD0 File Offset: 0x000FCED0
	public InternalConstructionCompleteCondition(BuildingInternalConstructor.Instance target)
	{
		this.target = target;
	}

	// Token: 0x06008A30 RID: 35376 RVA: 0x000FECDF File Offset: 0x000FCEDF
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.target.IsRequestingConstruction() && !this.target.HasOutputInStorage())
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A31 RID: 35377 RVA: 0x000FECFE File Offset: 0x000FCEFE
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.STATUS.FAILURE;
	}

	// Token: 0x06008A32 RID: 35378 RVA: 0x000FED15 File Offset: 0x000FCF15
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return (status == ProcessCondition.Status.Ready) ? UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.READY : UI.STARMAP.LAUNCHCHECKLIST.INTERNAL_CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
	}

	// Token: 0x06008A33 RID: 35379 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006852 RID: 26706
	private BuildingInternalConstructor.Instance target;
}
