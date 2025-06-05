using System;
using STRINGS;

// Token: 0x020019DC RID: 6620
public class ConditionHasResource : ProcessCondition
{
	// Token: 0x060089FB RID: 35323 RVA: 0x000FEA6E File Offset: 0x000FCC6E
	public ConditionHasResource(Storage storage, SimHashes resource, float thresholdMass)
	{
		this.storage = storage;
		this.resource = resource;
		this.thresholdMass = thresholdMass;
	}

	// Token: 0x060089FC RID: 35324 RVA: 0x000FEA8B File Offset: 0x000FCC8B
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.storage.GetAmountAvailable(this.resource.CreateTag()) < this.thresholdMass)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060089FD RID: 35325 RVA: 0x00368B70 File Offset: 0x00366D70
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.WARNING, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.STATUS.FAILURE, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x060089FE RID: 35326 RVA: 0x00368C20 File Offset: 0x00366E20
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.READY, this.storage.GetProperName(), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
			else
			{
				result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.WARNING, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
			}
		}
		else
		{
			result = string.Format(UI.STARMAP.LAUNCHCHECKLIST.HAS_RESOURCE.TOOLTIP.FAILURE, this.storage.GetProperName(), GameUtil.GetFormattedMass(this.thresholdMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), ElementLoader.GetElement(this.resource.CreateTag()).name);
		}
		return result;
	}

	// Token: 0x060089FF RID: 35327 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006843 RID: 26691
	private Storage storage;

	// Token: 0x04006844 RID: 26692
	private SimHashes resource;

	// Token: 0x04006845 RID: 26693
	private float thresholdMass;
}
