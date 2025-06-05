using System;
using STRINGS;

// Token: 0x020019E4 RID: 6628
public class ConditionSufficientFood : ProcessCondition
{
	// Token: 0x06008A25 RID: 35365 RVA: 0x000FEC59 File Offset: 0x000FCE59
	public ConditionSufficientFood(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x06008A26 RID: 35366 RVA: 0x000FEC68 File Offset: 0x000FCE68
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.Edible) <= 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A27 RID: 35367 RVA: 0x000FEC8B File Offset: 0x000FCE8B
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.NAME;
		}
		return UI.STARMAP.NOFOOD.NAME;
	}

	// Token: 0x06008A28 RID: 35368 RVA: 0x000FECA6 File Offset: 0x000FCEA6
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASFOOD.TOOLTIP;
		}
		return UI.STARMAP.NOFOOD.TOOLTIP;
	}

	// Token: 0x06008A29 RID: 35369 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006850 RID: 26704
	private CommandModule module;
}
