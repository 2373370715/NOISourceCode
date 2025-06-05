using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020019D7 RID: 6615
public class ConditionHasCargoBayForNoseconeHarvest : ProcessCondition
{
	// Token: 0x060089E0 RID: 35296 RVA: 0x000FE9B9 File Offset: 0x000FCBB9
	public ConditionHasCargoBayForNoseconeHarvest(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060089E1 RID: 35297 RVA: 0x003683DC File Offset: 0x003665DC
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (!this.HasHarvestNosecone())
		{
			return ProcessCondition.Status.Ready;
		}
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetComponent<CargoBayCluster>())
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Warning;
	}

	// Token: 0x060089E2 RID: 35298 RVA: 0x00368448 File Offset: 0x00366648
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.STATUS.READY;
			break;
		}
		return result;
	}

	// Token: 0x060089E3 RID: 35299 RVA: 0x00368498 File Offset: 0x00366698
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		switch (status)
		{
		case ProcessCondition.Status.Failure:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.FAILURE;
			break;
		case ProcessCondition.Status.Warning:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.WARNING;
			break;
		case ProcessCondition.Status.Ready:
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_CARGO_BAY_FOR_NOSECONE_HARVEST.TOOLTIP.READY;
			break;
		}
		return result;
	}

	// Token: 0x060089E4 RID: 35300 RVA: 0x000FE9C8 File Offset: 0x000FCBC8
	public override bool ShowInUI()
	{
		return this.HasHarvestNosecone();
	}

	// Token: 0x060089E5 RID: 35301 RVA: 0x003684E8 File Offset: 0x003666E8
	private bool HasHarvestNosecone()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag("NoseconeHarvest"))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0400683E RID: 26686
	private LaunchableRocketCluster launchable;
}
