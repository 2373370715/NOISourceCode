using System;
using STRINGS;

// Token: 0x020019DD RID: 6621
public class ConditionNoExtraPassengers : ProcessCondition
{
	// Token: 0x06008A00 RID: 35328 RVA: 0x000FEAAE File Offset: 0x000FCCAE
	public ConditionNoExtraPassengers(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06008A01 RID: 35329 RVA: 0x000FEABD File Offset: 0x000FCCBD
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.module.CheckExtraPassengers())
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06008A02 RID: 35330 RVA: 0x000FEACF File Offset: 0x000FCCCF
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.FAILURE;
	}

	// Token: 0x06008A03 RID: 35331 RVA: 0x000FEAEA File Offset: 0x000FCCEA
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.NO_EXTRA_PASSENGERS.TOOLTIP.FAILURE;
	}

	// Token: 0x06008A04 RID: 35332 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006846 RID: 26694
	private PassengerRocketModule module;
}
